using NewsToTrades.Analysis;
using NewsToTrades.News;

var frequency = TimeSpan.FromMinutes(1);

var aggregator = new NewsAggregator();

var analyzer = new NewsAnalyzer();

do
{
    var news = await aggregator.GetCurrentAsync();

    var signals = await analyzer.GetBuySignalsAsync(news);

    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} - Buy signals: {string.Join(", ", signals)}");

    await Task.Delay(frequency);
}
while (true);
