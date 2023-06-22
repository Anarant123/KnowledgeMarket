using System.Net.Http.Headers;

namespace KnowledgeMarketWebAPI.ApiClient;

public class KnowledgeMarketApiClient
{
    internal readonly HttpClient HttpClient;

    private string? _jwt;

    public KnowledgeMarketApiClient(string apiBasePath)
    {
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBasePath)
        };
        Jwt = null;
    }

    public string? Jwt
    {
        get => _jwt;
        set
        {
            _jwt = value;
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);
        }
    }
}