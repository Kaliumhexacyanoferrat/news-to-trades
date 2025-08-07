using NewsToTrades.News.Sources;

namespace NewsToTrades.News;

public class NewsAggregator
{
    private readonly List<INewsSource> _sources = new()
    {
        new RssSource("https://www.n-tv.de/nyticker2/index.rss"),
        new RssSource("https://www.xetra.com/xetra-de/newsroom/xetra-newsboard/4218!rss"),
        new RssSource("https://www.finanzen.net/rss/news"),
        new RssSource("https://www.deraktionaer.de/aktionaer-news.rss"),
        new RssSource("https://www.welt.de/feeds/section/wirtschaft.rss"),
        new RssSource("https://newsfeed.zeit.de/wirtschaft/index"),
        new RssSource("https://www.handelsblatt.com/contentexport/feed/finanzen")
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
