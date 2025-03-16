using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Interfaces;

public interface IRegisterExpenseUseCase
{
    Task<ResponseRegisterExpenseJson >Execute(RequestExpenseJson request);
}
