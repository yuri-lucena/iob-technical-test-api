using AutoMapper;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Interfaces.Infra.Repositories;
using Iob.Bank.Infra.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Iob.Bank.Infra.Persistence.Repositories;

public class UserRepository(IMapper mapper, DataContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto> AuthenticateUser(UserAuthRequestDto userAuthRequest)
    {
        var user = await DbSet.AsNoTracking()
                    .Include(i => i.UserType)
                    .FirstOrDefaultAsync(w => w.Email == userAuthRequest.Email &&
                                              w.Password == userAuthRequest.Pass);

        return _mapper.Map<UserDto>(user);
    }
}
