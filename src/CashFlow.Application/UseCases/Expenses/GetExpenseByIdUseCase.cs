using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

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
        var result = await _expenseRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        return _mapper.Map<ResponseExpenseJson>(result);
    }
}
