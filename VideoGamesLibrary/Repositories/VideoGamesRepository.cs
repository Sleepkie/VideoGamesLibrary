using Microsoft.EntityFrameworkCore;
using System.Linq;
using VideoGamesLibrary.DbContexts;
using VideoGamesLibrary.DTO;
using VideoGamesLibrary.Interfaces;
using VideoGamesLibrary.Models;

namespace VideoGamesLibrary.Repositories
{
    public class VideoGamesRepository : IVideoGamesRepository
    {

        private VideoGamesLibraryContext _context;

        public VideoGamesRepository(VideoGamesLibraryContext context)
        {
            _context = context;
        }

        public async Task<GameDTO?> GetAsync(int id)
        {
            var game = await _context.Games.Include(g => g.Genres).Include(g => g.Developer).ThenInclude(g => g.Games).FirstOrDefaultAsync(g => g.Id == id);

            if (game != null)
            {
                var gameDTO = new GameDTO()
                {
                    Name = game.Name,
                    Id = game.Id,
                    Developer = game.Developer.Name,
                    Genres = game.Genres.Select(g => g.Name).ToList()
                };

                return gameDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var gameDTOList = new List<GameDTO>();

            var gamesFromDB = await _context.Games.Include(g => g.Genres).ToListAsync();

            foreach (var game in gamesFromDB)
            {
                var devName = await _context.Developers.Where(d => d.Id == game.Developerid).FirstAsync();

                gameDTOList.Add(new GameDTO()
                {
                    Id = game.Id,
                    Name = game.Name,
                    Developer = devName.Name,
                    Genres = game.Genres.Select(g => g.Name).ToList()
                });
            }

            return gameDTOList;
        }

        public async Task<IEnumerable<GameDTO>> GetFilteredAsync(string[] genres)
        {

            var allGamesList = await _context.Games.Include(g => g.Developer).Include(g => g.Genres).ToListAsync();

            var gamesList = allGamesList.Where(game => genres.All(genre => game.Genres.Select(gen => gen.Name).Contains(genre))).ToList();

            var gameDTOList = new List<GameDTO>();

            foreach (var game in gamesList)
            {
                gameDTOList.Add(new GameDTO()
                {
                    Id = game.Id,
                    Name = game.Name,
                    Developer = game.Developer.Name,
                    Genres = game.Genres.Select(g => g.Name).ToList()
                });
            }

            return gameDTOList;
        }

        public async Task<bool> AddAsync(GameDTO item)
        {
            var gameGenres = await _context.Genres.Where(g => item.Genres.Contains(g.Name)).ToListAsync();

            var game = new Game();

            game.Name = item.Name;
            var devId = await _context.Developers.Where(d => d.Name == item.Developer).FirstOrDefaultAsync();
            game.Developerid = devId.Id;
            game.Genres = gameGenres;

            _context.Games.Add(game);

            return game != null;
        }

        public async Task<bool> Delete(int id)
        {
            var gameToDelete = await _context.Games.Where(g => g.Id == id).FirstAsync();

            var entity = _context.Games.Remove(gameToDelete);

            return entity != null;
        }

        public async Task<bool> Update(GameDTO item)
        {
            var game = _context.Games.Find(item.Id);

            var gameGenres = await _context.Genres.Include(x => x.Games).Where(g => item.Genres.Contains(g.Name)).ToListAsync();

            if (game != null)
            {
                game.Id = item.Id;
                game.Name = item.Name;
                var devId = await _context.Developers.Where(d => d.Name == item.Developer).FirstOrDefaultAsync();
                game.Developerid = devId.Id;
                game.Genres.Clear();
                game.Genres = gameGenres;
            }

            return game != null;
        }
    }
}
