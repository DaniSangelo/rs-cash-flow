using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses;

public interface IRegisterExpenseUseCase
{
    ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request);
}
