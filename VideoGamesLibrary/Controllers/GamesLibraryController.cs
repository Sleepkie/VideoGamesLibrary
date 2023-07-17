using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoGamesLibrary.DTO;
using VideoGamesLibrary.Interfaces;
using VideoGamesLibrary.Models;

namespace VideoGamesLibrary.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GamesLibraryController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public GamesLibraryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var game = await _unitOfWork.VideoGames.GetAsync(id);

            if (game  == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetAll()
        {
            var list = await _unitOfWork.VideoGames.GetAllAsync();

            if (list.Count() == 0)
            {
                return NotFound();
            }

            return Ok(list);
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetFiltered([FromQuery] QueryParameters parameters)
        {
            var games = await _unitOfWork.VideoGames.GetFilteredAsync(parameters.Genres);

            if (games.Count() == 0)
            {
                return NotFound();
            }

            return Ok(games);
        }

        [HttpPost]
        public async Task<ActionResult> Create(GameDTO game)
        {
            if (game == null)
            {
                return BadRequest();
            }
            var isCreated = await _unitOfWork.VideoGames.AddAsync(game);

            if (!isCreated)
            {
                return BadRequest();
            }
            
            await _unitOfWork.CompleteAsync();
            
            return Ok(game);
        }

        [HttpPut]
        public async Task<ActionResult> Put(GameDTO game)
        {
            if (game == null)
            {
                return BadRequest();
            }
            
            await _unitOfWork.VideoGames.Update(game);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var isDeleted = await _unitOfWork.VideoGames.Delete(id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}