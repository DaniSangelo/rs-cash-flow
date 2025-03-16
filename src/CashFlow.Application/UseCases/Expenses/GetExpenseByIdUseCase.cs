using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Interfaces;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _expenseRepository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expenseRepository, IMapper mapper, ILoggedUser loggedUser)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var result = await _expenseRepository.GetById(loggedUser, id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        return _mapper.Map<ResponseExpenseJson>(result);
    }
}
