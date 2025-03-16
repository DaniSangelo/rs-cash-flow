using System.Net.Mime;
using System.Net;
using FluentAssertions;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/Report";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;

    public GenerateExpensesReportTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _adminToken = webApp.User_Admin.GetToken();
        _teamMemberToken = webApp.User_Team_Member.GetToken();
        _expenseDate = webApp.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await SendGet(resource: $"{RESOURCE}/pdf?month={_expenseDate:yyyy-MM-dd}", token: _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await SendGet(resource: $"{RESOURCE}/excel?month={_expenseDate:yyyy-MM-dd}", token: _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await SendGet(resource: $"{RESOURCE}/excel?month={_expenseDate:yyyy-MM-dd}", token: _teamMemberToken);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await SendGet(resource: $"{RESOURCE}/pdf?month={_expenseDate:yyyy-MM-dd}", token: _teamMemberToken);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
