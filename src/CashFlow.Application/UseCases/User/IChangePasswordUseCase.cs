using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
