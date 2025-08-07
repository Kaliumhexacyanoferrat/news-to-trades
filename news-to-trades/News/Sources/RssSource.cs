using CodeHollow.FeedReader;

namespace NewsToTrades.News.Sources;

public class RssSource(string feed) : INewsSource
{

    public async Task<List<News>> GetLatestAsync(DateTime after)
    {
        var result = await FeedReader.ReadAsync(feed);

        var found = result.Items
                          .Where(i => i.PublishingDate != null && i.PublishingDate > after)
                          .Select(item => new News(item.Id, item.Title, item.Description, item.PublishingDate!.Value))
                          .ToList();

        return found;
    }

}
