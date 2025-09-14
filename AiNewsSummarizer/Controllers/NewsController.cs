using AiNewsSummarizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiNewsSummarizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummarizeController : ControllerBase
{
    private readonly ScraperService _scraper;
    private readonly AiService _ai;

    public SummarizeController(ScraperService scraper, AiService ai)
    {
        _scraper = scraper;
        _ai = ai;
    }

    // GET /api/summarize?url=https://example.com/news
    [HttpGet]
    public async Task<IActionResult> Summarize([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("Please provide a valid URL");

        // 1. Scrape content
        var scraped = await _scraper.FetchAsync(url);

        if (string.IsNullOrWhiteSpace(scraped.Text))
            return BadRequest("Could not scrape any content from the URL");

        // 2. Summarize using AI
        var summary = await _ai.SummarizeAsync(scraped.Title, url, scraped.Text);

        return Ok(summary);
    }
}
