using NewsToTrades.News;

var frequency = TimeSpan.FromMinutes(1);

var aggregator = new NewsAggregator();

do
{
    var news = await aggregator.GetCurrent();

    // analyze

    await Task.Delay(frequency);
}
while (true);
