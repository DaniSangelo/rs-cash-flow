using CashFlow.Application.UseCases.User;
using CashFlow.Domain.Entities;
using CommonTestUtilities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users;

public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute();
        await act.Should().NotThrowAsync();
    }

    private DeleteUserAccountUseCase CreateUseCase(User user)
    {
        var repository = UserWriteOnlyRepositoryBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        return new DeleteUserAccountUseCase(unitOfWork, repository, loggedUser);
    }
}
