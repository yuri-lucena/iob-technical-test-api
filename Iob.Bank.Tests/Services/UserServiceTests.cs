using AutoMapper;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Services;
using Iob.Bank.Domain.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace Iob.Bank.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IDbModule> _dbModuleMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHashService> _hashServiceMock;
    private readonly UserService _userService;
    public UserServiceTests()
    {
        _dbModuleMock = new Mock<IDbModule>();
        _mapperMock = new Mock<IMapper>();
        _hashServiceMock = new Mock<IHashService>();
        _userService = new UserService(_dbModuleMock.Object, _mapperMock.Object, _hashServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_SuccessfulUserCreation_ReturnsUserDto()
    {
        var userDto = new UserDto();
        var user = new User();
        _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(user);
        _dbModuleMock.Setup(d => d.UserRepository.AddAsync(user)).ReturnsAsync(user);
        _dbModuleMock.Setup(d => d.CommitChangesAsync(It.IsAny<IDbContextTransaction>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

        var result = await _userService.CreateAsync(userDto, 1);

        Assert.Equal(userDto, result);
        _dbModuleMock.Verify(d => d.UserRepository.AddAsync(user), Times.Once);
        _dbModuleMock.Verify(d => d.CommitChangesAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserDto>(user), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ExceptionThrownWhenAddingUserToRepository_RollbackTransactionIsCalled()
    {
        var userDto = new UserDto();
        var user = new User();
        _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(user);
        _dbModuleMock.Setup(d => d.UserRepository.AddAsync(user)).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _userService.CreateAsync(userDto, 1));
        _dbModuleMock.Verify(d => d.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserDto>(user), Times.Never);
        _dbModuleMock.Verify(d => d.UserRepository.AddAsync(user), Times.Once);
        _dbModuleMock.Verify(d => d.CommitChangesAsync(It.IsAny<IDbContextTransaction>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ExceptionThrownWhenCommittingTransaction_RollbackTransactionIsCalled()
    {
        var userDto = new UserDto();
        var user = new User();
        _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(user);
        _dbModuleMock.Setup(d => d.UserRepository.AddAsync(user)).ReturnsAsync(user);
        _dbModuleMock.Setup(d => d.CommitChangesAsync(It.IsAny<IDbContextTransaction>())).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _userService.CreateAsync(userDto, 1));
        _dbModuleMock.Verify(d => d.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        _dbModuleMock.Verify(d => d.UserRepository.AddAsync(user), Times.Once);
        _dbModuleMock.Verify(d => d.CommitChangesAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
    }
}
