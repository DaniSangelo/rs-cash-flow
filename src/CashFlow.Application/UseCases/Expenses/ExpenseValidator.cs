using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;
using System;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_IS_REQUIRED);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE);
        RuleFor(x => x.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_PAYMENT_TYPE);
    }
}
