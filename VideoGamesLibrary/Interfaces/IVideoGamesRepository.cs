using Microsoft.Extensions.ObjectPool;
using VideoGamesLibrary.DTO;
using VideoGamesLibrary.Models;

namespace VideoGamesLibrary.Interfaces
{
    public interface IVideoGamesRepository : IRepository<GameDTO>
    {
        Task<IEnumerable<GameDTO>> GetFilteredAsync(string[] genres);
    }
}
