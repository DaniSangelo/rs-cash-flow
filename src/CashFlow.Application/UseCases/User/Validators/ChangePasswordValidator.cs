using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.User.Validators;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
