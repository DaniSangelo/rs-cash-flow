using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses;

public class RegisterExpenseTest : IClassFixture<CustomWebAppFactory>
{
    private const string RESOURCE = "api/Expenses";
    private readonly HttpClient _httpClient;
    private readonly string _token;

    public RegisterExpenseTest(CustomWebAppFactory webApp)
    {
        _httpClient = webApp.CreateClient();
        _token = webApp.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        var result = await _httpClient.PostAsJsonAsync(RESOURCE, request);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Title_Name(string cultureInfo)
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));
        var result = await _httpClient.PostAsJsonAsync(RESOURCE, request);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_IS_REQUIRED", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}
