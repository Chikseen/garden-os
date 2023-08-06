using System.Net.Http.Headers;
using dotenv.net;

public class ApiService
{
    private readonly HttpClient _client;
    private readonly string _url = string.Empty;

    public ApiService()
    {
        DotEnv.Load();
        string _rpiApiKey = Environment.GetEnvironmentVariable("API_KEY")!;

        _url = Environment.GetEnvironmentVariable("URL")!;

        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rpiApiKey);
    }

    public HttpResponseMessage Post(string route, string json)
    {
        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        return _client.PostAsync($"{_url}{route}", content).Result;
    }
}