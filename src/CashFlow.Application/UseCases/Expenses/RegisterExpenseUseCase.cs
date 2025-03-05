using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpenseRepository _expenseRepository;

    public RegisterExpenseUseCase(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);

        var entity = new Expense
        {
            Amount = request.Amount,
            Date = request.Date,
            Title = request.Title,
            Description = request.Description,
            PaymentType = (Domain.Enums.PaymentType)request.PaymentType
        };

        _expenseRepository.Add(entity);

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
