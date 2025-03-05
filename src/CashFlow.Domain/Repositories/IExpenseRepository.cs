using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpenseRepository
{
    void Add(Expense expense);
}
