using System.Net.Http.Json;
using System.Text;

using NewsToTrades.News;

namespace NewsToTrades.Analysis;

public class NewsAnalyzer
{
    private const string Endpoint = "http://localhost:11434/api/generate";

    private const string Model = "llama3";

    public async Task<List<string>> GetBuySignalsAsync(List<NewsEntry> news)
    {
        var prompt = new StringBuilder();

        var answer = await QueryAsync(prompt.ToString());

        // todo: parse

        return new List<string>();
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
