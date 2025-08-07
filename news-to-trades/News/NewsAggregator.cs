using NewsToTrades.News.Sources;

namespace NewsToTrades.News;

public class NewsAggregator
{
    private readonly List<INewsSource> _sources = new()
    {
        new RssSource("https://www.n-tv.de/nyticker2/index.rss")
    };

    private readonly List<NewsEntry> _currentNews = new();

    private readonly object _newsLock = new();

    private TimeSpan _maximumAge = TimeSpan.FromMinutes(30);

    private DateTime _lastUpdate;

    public NewsAggregator()
    {
        _lastUpdate = DateTime.UtcNow - _maximumAge;
    }

    public async Task<List<NewsEntry>> GetCurrentAsync()
    {
        var now = DateTime.UtcNow;

        _currentNews.RemoveAll(n => n.Date < now - _maximumAge);

        await Parallel.ForEachAsync(_sources, async (s, _) =>
        {
            var news = await s.GetLatestAsync(_lastUpdate);

            lock (_newsLock)
            {
                foreach (var n in news)
                {
                    if (!_currentNews.Any(existing => existing.ID == n.ID))
                    {
                        _currentNews.Add(n);
                    }
                }
            }
        });

        _lastUpdate = now;

        return _currentNews;
    }

}
