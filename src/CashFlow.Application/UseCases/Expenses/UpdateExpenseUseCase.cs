
using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase(IExpensesUpdateRepository expensesRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _expensesRepository = expensesRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);
        var expense = await _expensesRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        _mapper.Map(request, expense);
        _expensesRepository.Update(expense);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);

        }
    }
}
