namespace VideoGamesLibrary.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVideoGamesRepository VideoGames { get; }

        Task CompleteAsync();
    }
}
