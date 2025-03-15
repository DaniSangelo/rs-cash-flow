using CashFlow.Application.UseCases.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception;
using CommonTestUtilities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var uc = CreateUseCase(user, expense);
        var act = async () => await uc.Execute(expense.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);
        var act = async () => await useCase.Execute(id: 1000);
        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
        var repositoryReadOnly = new ExpenseReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(repositoryWriteOnly, unitOfWork, loggedUser, repositoryReadOnly);
    }
}
