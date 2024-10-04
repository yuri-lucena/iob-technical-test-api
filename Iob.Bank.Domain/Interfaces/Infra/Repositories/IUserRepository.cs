using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Interfaces.Infra.Repositories.Base;

namespace Iob.Bank.Domain.Interfaces.Infra.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<UserDto> AuthenticateUser(UserAuthRequestDto userAuthRequest);
}