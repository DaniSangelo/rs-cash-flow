using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.UseCases.Expenses;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var result = await _expenseRepository.GetById(id);
        return _mapper.Map<ResponseExpenseJson>(result);
    }
}
