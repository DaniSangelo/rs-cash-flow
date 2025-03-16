using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.User;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
