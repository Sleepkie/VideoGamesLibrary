using System.Security.Cryptography;
using VideoGamesLibrary.DbContexts;
using VideoGamesLibrary.Interfaces;
using VideoGamesLibrary.Repositories;

namespace VideoGamesLibrary.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VideoGamesLibraryContext _context;

        public IVideoGamesRepository VideoGames
        {
            get;
            private set;
        }
        public UnitOfWork(VideoGamesLibraryContext context)
        {
            _context = context;

            VideoGames = new VideoGamesRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
