using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Interfaces;

public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}
