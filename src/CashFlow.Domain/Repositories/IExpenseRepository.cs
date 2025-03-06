using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpenseRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
}
