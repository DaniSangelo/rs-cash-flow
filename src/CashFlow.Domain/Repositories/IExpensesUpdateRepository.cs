using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpensesUpdateRepository
{
    void Update(Expense expense);
    Task<Expense?> GetById(long id);
}
