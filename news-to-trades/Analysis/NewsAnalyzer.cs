using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;

using NewsToTrades.News;

namespace NewsToTrades.Analysis;

public class NewsAnalyzer
{
    private const string Endpoint = "http://localhost:11434/api/generate";

    private const string Model = "llama3";

    public async Task<string> GetBuySignalsAsync(List<NewsEntry> news)
    {
        var prompt = await GetPromptAsync(news);

        return await QueryAsync(prompt);
    }

    private async Task<string> GetPromptAsync(List<NewsEntry> news)
    {
        using var template = Assembly.GetExecutingAssembly().GetManifestResourceStream("NewsToTrades.Analysis.Prompt.txt")
            ?? throw new InvalidOperationException("Unable to read prompt template");

        using var reader = new StreamReader(template);

        var prompt = new StringBuilder(await reader.ReadToEndAsync());

        var newsJson = JsonSerializer.Serialize(news, new JsonSerializerOptions { WriteIndented = true });

        prompt.Replace("[news]", newsJson);

        return prompt.ToString();
    }

    private async Task<string> QueryAsync(string prompt)
    {
        using var client = new HttpClient();

        var requestBody = new
        {
            model = Model,
            prompt = prompt,
            stream = false
        };

        var content = JsonContent.Create(requestBody);

        using var response = await client.PostAsync(Endpoint, content);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ModelAnswer>();

        return result?.Response ?? throw new InvalidOperationException("LLM did not produce an answer");
    }

}
