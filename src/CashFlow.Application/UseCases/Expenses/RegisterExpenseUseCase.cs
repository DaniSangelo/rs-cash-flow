using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public RegisterExpenseUseCase(IExpensesWriteOnlyRepository expenseRepository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRegisterExpenseJson >Execute(RequestExpenseJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.Get();
        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggedUser.Id;
        await _expenseRepository.Add(expense);
        await _unitOfWork.Commit();
        return _mapper.Map<ResponseRegisterExpenseJson>(expense);
    }

    private void Validate(RequestExpenseJson request)
    {
        var validatorResult = new ExpenseValidator().Validate(request);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
