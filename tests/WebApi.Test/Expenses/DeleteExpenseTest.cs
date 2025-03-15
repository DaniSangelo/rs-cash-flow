using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses;

public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _token = webApp.User_Team_Member.GetToken();
        _expenseId = webApp.Expense_Team_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await SendDelete(resource: $"{RESOURCE}/{_expenseId}", token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        result = await SendGet(resource: $"{RESOURCE}/{_expenseId}", token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var result = await SendDelete(resource: $"{RESOURCE}/53", token: _token, culture: culture);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
