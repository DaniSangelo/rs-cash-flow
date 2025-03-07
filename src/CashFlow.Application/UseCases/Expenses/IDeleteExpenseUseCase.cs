namespace CashFlow.Application.UseCases.Expenses;

public interface IDeleteExpenseUseCase
{
    public Task Execute(long id);
}
