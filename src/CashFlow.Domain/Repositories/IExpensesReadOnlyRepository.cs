using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll(User user);
    Task<Expense?> GetById(User user, long id);
    Task<List<Expense>> FilterByMonth(DateOnly date);
}
