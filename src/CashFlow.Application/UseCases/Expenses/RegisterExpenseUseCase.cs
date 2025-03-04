using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Expenses;

public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);
        return new ResponseRegisterExpenseJson();
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
        var validatorResult = new RegisterExpenseValidator().Validate(request);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
