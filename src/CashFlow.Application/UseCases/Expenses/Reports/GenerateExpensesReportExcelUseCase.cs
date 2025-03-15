using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensios;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports;

class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private const string CURRENCY_SYMBOL = "R$";
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository, ILoggedUser loggedUser)
    {
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();
        var expenses = await _expensesReadOnlyRepository.FilterByMonth(loggedUser, month);

        if (expenses.Count == 0) return [];


        using var workbook = new XLWorkbook
        {
            Author = loggedUser.Name
        };

        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
        InsertHeader(worksheet);

        var raw = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{2}").Value = expense.Title;
            worksheet.Cell($"B{2}").Value = expense.Date;
            worksheet.Cell($"C{2}").Value = expense.PaymentType.PaymentTypeToString();

            worksheet.Cell($"D{2}").Value = expense.Amount;
            worksheet.Cell($"D{2}").Style.NumberFormat.Format = $"- {CURRENCY_SYMBOL} #,##0.00";

            worksheet.Cell($"E{2}").Value = expense.Description;
            raw++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");
        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}
