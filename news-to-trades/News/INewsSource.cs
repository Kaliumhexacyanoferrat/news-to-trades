namespace NewsToTrades.News;

public interface INewsSource
{

    Task<List<News>> GetLatestAsync(DateTime after);

}
