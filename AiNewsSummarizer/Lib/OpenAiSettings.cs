namespace AiNewsSummarizer.Lib;

public class OpenAiSettings
{
    public string ApiKey { get; set; } = "";
    public string Model { get; set; } = "gpt-4o-mini";
    public string ApiBase { get; set; } = "https://api.openai.com/v1";
}
