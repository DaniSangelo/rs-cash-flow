using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpenseReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _repository;

    public ExpenseReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesReadOnlyRepository>();
    }

    public ExpenseReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);
        return this;
    }

    public ExpenseReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null) _repository.Setup(repo => repo.GetById(user, expense.Id)).ReturnsAsync(expense);
        return this;
    }

    public ExpenseReadOnlyRepositoryBuilder FilterByMonth(User user, List<Expense> expenses)
    {
        _repository.Setup(repo => repo.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);
        return this;
    }

    public IExpensesReadOnlyRepository Build() => _repository.Object;
}
