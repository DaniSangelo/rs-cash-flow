using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User.Interfaces;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
