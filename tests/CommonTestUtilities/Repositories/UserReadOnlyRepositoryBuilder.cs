﻿using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void EmailAlreadyInUse(string email)
    {
        _repository.Setup(userReadOnly => userReadOnly.EmailAlreadyInUse(email)).ReturnsAsync(true);
    }
    public IUserReadOnlyRepository Build() => _repository.Object;
}
