using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User.Interfaces;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
