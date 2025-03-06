using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.UseCases.Expenses;

public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetAllExpenseUseCase(IExpensesReadOnlyRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await _expenseRepository.GetAll();
        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}
