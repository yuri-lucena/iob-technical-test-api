using System;
using FluentValidation;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Infra;

namespace Iob.Bank.Domain.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator(IDbModule dbModule)
    {
        RuleFor(r => r)
            .Custom((user, context) =>
            {
                var _user = dbModule.UserRepository.GetByAsync(w => w.Email == user.Email).GetAwaiter().GetResult();
                if (_user != null)
                    context.AddFailure(nameof(user.Email), "E-mail já cadastrado");
            });

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("E-mail obrigatório")
            .EmailAddress().WithMessage("E-mail não válido");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Senha obrigatória")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres");

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Nome obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres");

        RuleFor(r => r.Birthday)
            .NotEmpty().WithMessage("Data de nascimento obrigatória")
            .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Usuário deve ter pelo menos 18 anos");

        RuleFor(r => r.Identifier)
            .NotEmpty().WithMessage("Identificador obrigatório")
            .MinimumLength(11).WithMessage("Identificador deve ter pelo menos 11 caracteres")
            .MaximumLength(11).WithMessage("Identificador deve ter no m ximo 11 caracteres");

        RuleFor(r => r.Address)
            .NotEmpty().WithMessage("Endereço  obrigatório")
            .MinimumLength(3).WithMessage("Endere o deve ter pelo menos 3 caracteres");
    }
}

