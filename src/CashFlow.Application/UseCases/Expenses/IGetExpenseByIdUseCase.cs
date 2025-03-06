using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses;

public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}
