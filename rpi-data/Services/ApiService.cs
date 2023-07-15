using System.Net.Http.Headers;
using dotenv.net;

public class ApiService
{
    HttpClient _client;
    String _rpiId = String.Empty;
    String _rpiApiKey = String.Empty;
    String _url = String.Empty;

    public ApiService()
    {
        DotEnv.Load();
        String _rpiId = Environment.GetEnvironmentVariable("RPI_ID")!;
        String _rpiApiKey = Environment.GetEnvironmentVariable("API_KEY")!;
        String _url = Environment.GetEnvironmentVariable("URL")!;

        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rpiApiKey);
    }

    public HttpResponseMessage Post(String route, String json)
    {
        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        return _client.PostAsync($"{_url}route", content).Result;
    }
}