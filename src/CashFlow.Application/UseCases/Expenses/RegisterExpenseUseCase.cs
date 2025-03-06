using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(IExpensesWriteOnlyRepository expenseRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisterExpenseJson >Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);
        var entity = _mapper.Map<Expense>(request);
        await _expenseRepository.Add(entity);
        await _unitOfWork.Commit();
        return _mapper.Map<ResponseRegisterExpenseJson>(entity);
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
        var validatorResult = new RegisterExpenseValidator().Validate(request);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
