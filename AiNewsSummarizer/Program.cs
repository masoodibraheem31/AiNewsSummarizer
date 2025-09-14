using AiNewsSummarizer.Lib;
using AiNewsSummarizer.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind OpenAI settings
builder.Services.Configure<OpenAiSettings>(
    builder.Configuration.GetSection("OpenAI"));

builder.Services.AddHttpClient<ScraperService>();
builder.Services.AddHttpClient<AiService>();
builder.Services.AddScoped<ScraperService>();
builder.Services.AddScoped<AiService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
