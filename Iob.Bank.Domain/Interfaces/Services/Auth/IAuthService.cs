using Iob.Bank.Domain.Data.Dtos;

namespace Iob.Bank.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<UserAuthResponseDto> SignInAsync(UserAuthRequestDto authRequest);
}
