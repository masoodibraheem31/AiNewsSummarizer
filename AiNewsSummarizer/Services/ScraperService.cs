using HtmlAgilityPack;

namespace AiNewsSummarizer.Services;

public class ScraperService
{
    private readonly HttpClient _http;

    public ScraperService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(string Title, string Text)> FetchAsync(string url)
    {
        var html = await _http.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText ?? "Untitled";

        // Get all visible text from <p> tags
        var paragraphs = doc.DocumentNode.SelectNodes("//p");
        var text = string.Join("\n", paragraphs?.Select(p => p.InnerText) ?? Array.Empty<string>());

        return (title, text);
    }
}
