using AiNewsSummarizer.Lib;
using AiNewsSummarizer.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AiNewsSummarizer.Services;

public class AiService
{
    private readonly HttpClient _http;
    private readonly OpenAiSettings _settings;

    public AiService(HttpClient http, IOptions<OpenAiSettings> options)
    {
        _http = http;
        _settings = options.Value;
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task<NewsItem> SummarizeAsync(string title, string url, string text)
    {
        var prompt = $"""
        Summarize this news article in exactly 3 bullet points:
        {text}
        """;

        var body = new
        {
            model = _settings.Model,
            messages = new[]
            {
                new { role = "system", content = "You are a news summarizer." },
                new { role = "user", content = prompt }
            }
        };

        var response = await _http.PostAsync(
            $"{_settings.ApiBase}/chat/completions",
            new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        );

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);

        var summary = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return new NewsItem(title, url, summary ?? "No summary");
    }
}
