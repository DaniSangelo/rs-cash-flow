
using CashFlow.Application.UseCases.Expenses.Interfaces;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IExpensesReadOnlyRepository _expensesReadRepository;

    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository expensesRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser,
        IExpensesReadOnlyRepository expensesReadRepository)
    {
        _expensesRepository = expensesRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _expensesReadRepository = expensesReadRepository;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var expense = await _expensesReadRepository.GetById(loggedUser, id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        await _expensesRepository.Delete(id);
        await _unitOfWork.Commit();
    }
}
