
using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpenseUseCase(IExpensesUpdateRepository expensesRepository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser)
    {
        _expensesRepository = expensesRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.Get();
        var expense = await _expensesRepository.GetById(loggedUser, id);
        if (expense is null || expense.UserId != loggedUser.Id)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }
        expense.Tags.Clear();
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
