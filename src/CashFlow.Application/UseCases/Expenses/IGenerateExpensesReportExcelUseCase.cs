namespace CashFlow.Application.UseCases.Expenses;

public interface IGenerateExpensesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
