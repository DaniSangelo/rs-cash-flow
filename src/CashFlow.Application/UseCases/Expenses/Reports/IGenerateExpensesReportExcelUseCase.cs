namespace CashFlow.Application.UseCases.Expenses.Reports;

public interface IGenerateExpensesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
