using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Login;

public interface ILoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
