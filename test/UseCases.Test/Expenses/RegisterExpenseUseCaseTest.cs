using CashFlow.Application.UseCases.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Threading.Tasks;

namespace UseCases.Test.Expenses;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(loggedUser);
        var act = async () => await useCase.Execute(request);
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_IS_REQUIRED));
    }

    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}
