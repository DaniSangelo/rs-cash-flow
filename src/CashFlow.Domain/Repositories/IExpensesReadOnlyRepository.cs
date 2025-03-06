using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
}
