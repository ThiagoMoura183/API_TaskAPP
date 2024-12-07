using Application.UserCQ.Commands;
using Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserCQ.Validators;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
    public CreateUserCommandValidator() {
        RuleFor(x => x.Email).NotEmpty().WithMessage("O campo 'email' não pode ser vazio.")
            .EmailAddress().WithMessage("O campo 'email' não é válido.")
            .WithErrorCode("400");

        RuleFor(x => x.Username).NotEmpty().WithMessage("O campo 'username' não pode ser vazio.");
    }
}
