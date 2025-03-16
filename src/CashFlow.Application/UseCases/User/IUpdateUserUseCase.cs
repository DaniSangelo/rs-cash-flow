using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
