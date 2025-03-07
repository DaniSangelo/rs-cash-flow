using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess.Repositories;

internal class ExpenseRepository : IExpensesWriteOnlyRepository, IExpensesReadOnlyRepository, IExpensesUpdateRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpenseRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }

    async Task<Expense?> IExpensesUpdateRepository.GetById(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task<bool> Delete(long id)
    {
        var expense = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
        if (expense is null) return false;
        _dbContext.Expenses.Remove(expense);
        return true;
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }
}
