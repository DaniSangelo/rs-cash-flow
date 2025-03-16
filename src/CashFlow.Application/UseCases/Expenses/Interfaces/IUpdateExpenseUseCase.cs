using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Interfaces;

public interface IUpdateExpenseUseCase
{
    public Task Execute(long id, RequestExpenseJson request);

}
