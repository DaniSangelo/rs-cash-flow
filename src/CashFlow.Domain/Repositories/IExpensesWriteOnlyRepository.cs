using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
}
