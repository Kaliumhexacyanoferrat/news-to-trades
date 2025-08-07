namespace NewsToTrades.News;

public interface INewsSource
{

    Task<List<NewsEntry>> GetLatestAsync(DateTime after);

}
