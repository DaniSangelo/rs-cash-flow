namespace CashFlow.Application.UseCases.Expenses.Interfaces;

public interface IDeleteExpenseUseCase
{
    public Task Execute(long id);
}
