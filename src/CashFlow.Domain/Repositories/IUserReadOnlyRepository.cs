namespace CashFlow.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> EmailAlreadyInUse(string email);
}
