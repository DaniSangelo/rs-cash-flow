using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.User.Interfaces;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
