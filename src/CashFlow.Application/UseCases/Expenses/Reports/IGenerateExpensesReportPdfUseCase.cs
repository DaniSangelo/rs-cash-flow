namespace CashFlow.Application.UseCases.Expenses.Reports;
public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
