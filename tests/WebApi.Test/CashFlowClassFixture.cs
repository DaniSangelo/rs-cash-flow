using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class CashFlowClassFixture : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _httpClient;

    public CashFlowClassFixture(CustomWebAppFactory webApp)
    {
        _httpClient = webApp.CreateClient();
    }

    protected async Task<HttpResponseMessage> SendPost(
        string resource,
        object request,
        string token = "",
        string cultureInfo = "en"
    )
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(cultureInfo);
        return await _httpClient.PostAsJsonAsync(resource, request);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string cultureInfo)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));
    }
}
