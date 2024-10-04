using System;
using FluentValidation;
using Iob.Bank.Domain.Data.Dtos;

namespace Iob.Bank.Domain.Validators;

public class UserAuthRequestDtoValidator : AbstractValidator<UserAuthRequestDto>
{
    public UserAuthRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Pass).NotEmpty();
    }
}
