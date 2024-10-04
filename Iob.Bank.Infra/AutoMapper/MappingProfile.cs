using AutoMapper;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Data.Dtos.Base;
using Iob.Bank.Domain.Data.Entities;
using Iob.Bank.Domain.Data.Entities.Base;

namespace Iob.Bank.Infra.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<BankAccount, BankAccountDto>().ReverseMap();
        CreateMap<BankLaunch, BankLaunchDto>().ReverseMap();
        CreateMap<OperationType, OperationTypeDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserType, UserTypeDto>().ReverseMap();
    }
}
