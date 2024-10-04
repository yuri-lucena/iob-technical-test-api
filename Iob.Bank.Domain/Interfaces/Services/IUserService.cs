using Iob.Bank.Domain.Data.Dtos;

namespace Iob.Bank.Domain.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> CreateAsync(UserDto user, long userId);
    Task<UserDto> UpdateAsync(UserDto user, long userId);
    Task<bool> DeleteAsync(long id, long userId);
    Task<UserDto> GetAsync(long id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<bool> CreateWithBankAccountAsync(UserDto user);
}
