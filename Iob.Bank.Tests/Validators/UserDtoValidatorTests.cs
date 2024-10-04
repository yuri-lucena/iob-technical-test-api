using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Validators;
using Moq;

namespace Iob.Bank.Tests.Validators;

public class UserDtoValidatorTests
{
    private readonly UserDtoValidator _validator;
    private readonly Mock<IDbModule> _dbModuleMock;

    public UserDtoValidatorTests()
    {
        _dbModuleMock = new Mock<IDbModule>();
        _validator = new UserDtoValidator(_dbModuleMock.Object);
        CreateMock();
    }

    void CreateMock()
    {
        _dbModuleMock.Setup(db => db.UserRepository.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), false)).ReturnsAsync(new User { Email = "existing@email.com" });

    }

    [Fact]
    public void Email_AlreadyExists_ReturnsError()
    {
        var user = new UserDto { Email = "existing@email.com" };
        _dbModuleMock.Setup(db => db.UserRepository.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), false))
            .ReturnsAsync(new User { Email = "existing@email.com" });

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Email);
    }
    [Fact]
    public void Email_Empty_ReturnsError()
    {
        var user = new UserDto { Email = string.Empty };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Email);
    }
    [Fact]
    public void Email_InvalidFormat_ReturnsError()
    {
        var user = new UserDto { Email = "invalid_email" };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Email);
    }
    [Fact]
    public void Password_Empty_ReturnsError()
    {
        var user = new UserDto { Password = string.Empty };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Password);
    }
    [Fact]
    public void Password_LengthLessThan8_ReturnsError()
    {
        var user = new UserDto { Password = "short" };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Password);
    }
    [Fact]
    public void Name_Empty_ReturnsError()
    {
        var user = new UserDto { Name = string.Empty };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Name);
    }
    [Fact]
    public void Name_LengthLessThan3_ReturnsError()
    {
        var user = new UserDto { Name = "ab" };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Name);
    }
    [Fact]
    public void Birthday_Invalid_ReturnsError()
    {
        var user = new UserDto { Birthday = DateTime.Now };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Birthday);
    }
    [Fact]
    public void Identifier_Empty_ReturnsError()
    {
        var user = new UserDto { Identifier = string.Empty };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Identifier);
    }
    [Fact]
    public void Identifier_LengthNotEqual11_ReturnsError()
    {
        var user = new UserDto { Identifier = "1234567890" };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Identifier);
    }
    [Fact]
    public void Address_Empty_ReturnsError()
    {
        var user = new UserDto { Address = string.Empty };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Address);
    }
    [Fact]
    public void Address_LengthLessThan3_ReturnsError()
    {
        var user = new UserDto { Address = "ab" };

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(u => u.Address);
    }
}
