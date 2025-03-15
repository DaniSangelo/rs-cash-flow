using FluentAssertions;
using Microsoft.Extensions.DependencyModel;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses;

public class GetAllExpensesTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/Expenses";
    private readonly string _token;

    public GetAllExpensesTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _token = webApp.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await SendGet(resource: RESOURCE, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        response.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}
