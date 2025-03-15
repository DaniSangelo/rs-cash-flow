using CashFlow.Application.UseCases.Expenses.Reports.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Fonts;
using CashFlow.Domain.Extensios;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository, ILoggedUser loggedUser)
    {
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        _loggedUser = loggedUser;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();
        var expenses = await _expensesReadOnlyRepository.FilterByMonth(loggedUser, month);
        if (expenses.Count == 0) return [];


        var document = CreateDocument(loggedUser.Name, month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(loggedUser.Name, page);

        var totalSpent = expenses.Sum(expenses => expenses.Amount);
        CreateTotalSpentSection(page, month, totalSpent);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            #region: LINE 1

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);

            #endregion

            #region LINE 2

            var row2 = table.AddRow();
            row2.Height = HEIGHT_ROW_EXPENSE_TABLE;


            row2.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row2.Cells[0]);
            row2.Cells[0].Format.LeftIndent = 20;

            row2.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row2.Cells[1]);

            row2.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row2.Cells[2]);

            AddAmountForExpense(row2.Cells[3], expense.Amount);

            #endregion

            #region LINE 3 (optional)
            if (!string.IsNullOrWhiteSpace(expense.Description))
            {
                var row3 = table.AddRow();
                row3.Height = HEIGHT_ROW_EXPENSE_TABLE;
                row3.Cells[0].AddParagraph(expense.Description);
                row3.Cells[0].Format.Font = new Font
                {
                    Name = FontHelper.WORKSANS_REGULAR,
                    Size = "10",
                    Color = ColorHelper.BLACK
                };
                row3.Cells[0].Shading.Color = ColorHelper.GREEN_LIGHT;
                row3.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row3.Cells[0].MergeRight = 2;
                row3.Cells[0].Format.LeftIndent = 20;

                row2.Cells[3].MergeDown = 1;
            }
            #endregion


            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private void AddWhiteSpace(Table table)
    {
        var spaceRow = table.AddRow();
        spaceRow.Height = "30";
        spaceRow.Borders.Visible = false;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = "14",
            Color = ColorHelper.BLACK
        };
        cell.Shading.Color = ColorHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{amount:f2} {CURRENCY_SYMBOL}");
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = "14",
            Color = ColorHelper.BLACK
        };
        cell.Shading.Color = ColorHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = "14",
            Color = ColorHelper.WHITE
        };
        cell.Shading.Color = ColorHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = "12",
            Color = ColorHelper.BLACK
        };
        cell.Shading.Color = ColorHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalSpent)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{totalSpent:f2} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private Document CreateDocument(string author, DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR}_{month:Y}";
        document.Info.Author = author;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;
        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;
        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();
        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);
        return file.ToArray();
    }

    private void CreateHeaderWithProfilePhotoAndName(string name, Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");
        var row = table.AddRow();
        var assembly = Assembly.GetExecutingAssembly();
        var directoryPath = Path.GetDirectoryName(assembly.Location);
        //row.Cells[0].AddImage(Path.Combine(directoryPath!, "UseCases\\Expenses\\Reports\\Logo", "Bluetooth.png")).Width = "62";
        var image = row.Cells[0].AddImage(Path.Combine(directoryPath!, "UseCases\\Expenses\\Reports\\Logo", "Bluetooth.png"));
        image.Width = "62";
        image.Height = "62";
        row.Cells[1].AddParagraph($"Hello, {name}");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }
}
