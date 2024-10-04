using AutoMapper;
using FluentValidation;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Exceptions;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Services;

namespace Iob.Bank.Domain.Services;

public class UserService(IDbModule dbModule, IMapper mapper, IHashService hashService) : IUserService
{
    private readonly IDbModule _dbModule = dbModule;
    private readonly IMapper _mapper = mapper;
    private readonly IHashService _hashService = hashService;

    public async Task<UserDto> CreateAsync(UserDto user, long userId)
    {
        using (var transaction = await _dbModule.NewTransactionAsync())
        {
            try
            {
                user.Password = _hashService.GetSha256(user.Password!);
                user.CreatedBy = userId;

                var userCreated = await _dbModule.UserRepository.AddAsync(_mapper.Map<User>(user));

                await _dbModule.CommitChangesAsync(transaction);

                return _mapper.Map<UserDto>(userCreated);
            }
            catch (Exception)
            {
                await _dbModule.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }

    public async Task<UserDto> UpdateAsync(UserDto userDto, long userId)
    {
        using (var transaction = await _dbModule.NewTransactionAsync())
        {
            try
            {
                var user = await _dbModule.UserRepository.GetByIdAsync(userDto.Id)
                    ?? throw new UserNotFoundException(userDto.Id);

                user.Address = userDto.Address;
                user.Email = userDto.Email;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Address = userDto.Address;
                user.ModifiedBy = userId;
                user.Modified = DateTime.Now;

                await _dbModule.CommitChangesAsync(transaction);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception)
            {
                await _dbModule.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        using (var transaction = await _dbModule.NewTransactionAsync())
        {
            try
            {
                var user = await _dbModule.UserRepository.GetByIdAsync(id) ?? throw new UserNotFoundException(id);
                user.Active = false;
                user.ModifiedBy = userId;
                user.Modified = DateTime.Now;

                await _dbModule.CommitChangesAsync(transaction);
                return true;
            }
            catch (Exception)
            {
                await _dbModule.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return (await _dbModule.UserRepository.GetListByAsync(w => w.Active, asNoTracking: true))
            .Select(_mapper.Map<UserDto>);
    }

    public async Task<UserDto> GetAsync(long id)
    {
        var userDto = await _dbModule.UserRepository.GetByAsync(w => w.Id == id, asNoTracking: true)
            ?? throw new UserNotFoundException(id);

        return _mapper.Map<UserDto>(userDto);
    }

    public async Task<bool> CreateWithBankAccountAsync(UserDto user)
    {
        using (var transaction = await _dbModule.NewTransactionAsync())
        {
            try
            {
                user.Password = _hashService.GetSha256(user.Password!);
                var userCreated = await _dbModule.UserRepository.AddAsync(_mapper.Map<User>(user));
                await _dbModule.SaveChangesAsync();

                var bankAccount = new BankAccount()
                {
                    Active = true,
                    Balance = 0,
                    CreatedBy = userCreated.Id,
                    Type = "Conta Corrente",
                    UserId = userCreated.Id,
                };
                await _dbModule.BankAccountRepository.AddAsync(bankAccount);
                await _dbModule.CommitChangesAsync(transaction);
                return true;
            }
            catch (Exception)
            {
                await _dbModule.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }
}
