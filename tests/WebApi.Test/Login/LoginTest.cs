using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Login;

public class LoginTest : IClassFixture<CustomWebAppFactory>
{
    private const string RESOURCE = "api/login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public LoginTest(CustomWebAppFactory webApp)
    {
        _httpClient = webApp.CreateClient();
        _email = webApp.GetEmail();
        _name = webApp.GetName();
        _password = webApp.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };
        var response = await _httpClient.PostAsJsonAsync(RESOURCE, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStreamAsync();
        var data = await JsonDocument.ParseAsync(body);
        data.RootElement.GetProperty("name").GetString().Should().Be(_name);
        data.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        var response = await _httpClient.PostAsJsonAsync(RESOURCE, request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessage").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVALID_CREDENTIALS", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}
