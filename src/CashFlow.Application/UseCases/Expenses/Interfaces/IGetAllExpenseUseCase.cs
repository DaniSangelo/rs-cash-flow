using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Interfaces;

public interface IGetAllExpenseUseCase
{
    Task<ResponseExpensesJson> Execute();
}
