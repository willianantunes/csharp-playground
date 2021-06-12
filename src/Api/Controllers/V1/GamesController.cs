using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TicTacToeCSharpPlayground.Core.Business;
using TicTacToeCSharpPlayground.Core.Models;
using TicTacToeCSharpPlayground.Infrastructure.Database;
using TicTacToeCSharpPlayground.Infrastructure.Database.Repositories;

namespace TicTacToeCSharpPlayground.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ITicTacToeRepository _ticTacToeRepository;
        private readonly IBoardDealer _boardDealer;
        private readonly IGameDealer _gameDealer;
        private readonly AppDbContext _context;

        public GamesController(ITicTacToeRepository ticTacToeRepository,
            IBoardDealer boardDealer,
            IGameDealer gameDealer, AppDbContext context)
        {
            _ticTacToeRepository = ticTacToeRepository;
            _boardDealer = boardDealer;
            _gameDealer = gameDealer;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
        {
            Log.Information("Getting all games...");
            
            return await _context.Games.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetCurrentGameStatus(Guid id)
        {
            Log.Information("Getting specific game given ID: {Id}", id);
            var game = await _context.Games.FindAsync(id);

            if (game is null)
            {
                Log.Information("No game has been found!");
                return NotFound();
            }

            return game;
        }

        [HttpGet("{boardId}/{playerId}/{movementPosition}")]
        public async Task<ActionResult<Game>> ApplyMovementToTheGame(long boardId, int movementPosition, long playerId)
        {
            var firstLogMessage = "Received board, movement and player: {BoardId} / {MovementPosition} / {PlayerId}";
            Log.Information(firstLogMessage, boardId, movementPosition, playerId);

            Log.Information("Searching board and player...");
            var board = await _ticTacToeRepository.GetBoardByItsId(boardId);
            if (board is null)
                throw new InvalidBoardNotFoundToBePlayedException();
            // TODO player must not be a computer
            var player = await _ticTacToeRepository.GetPlayerByItsId(playerId);
            if (player is null)
                throw new InvalidPlayerNotFoundException();

            Log.Information("Searching for a game...");
            var game = await _gameDealer.GetGameByBoard(board);
            if (game.IsFinished())
                throw new InvalidGameIsNotPlayableAnymoreException();

            if (_boardDealer.PositionIsNotAvailable(game.ConfiguredBoard, movementPosition))
            {
                IList<int> position = _boardDealer.AvailablePositions(game.ConfiguredBoard);
                return BadRequest($"Available positions: {position}");
            }

            Log.Information("Executing movement and evaluating game...");
            var evaluatedGame = await _gameDealer.ExecuteMovementAndEvaluateResult(game, movementPosition, player);

            if (evaluatedGame.IsFinished())
                Log.Information("Game conclusion: {EvaluatedGame}", evaluatedGame);
            else
                Log.Information("Game hasn't finished yet!");

            return evaluatedGame;
        }
    }
}
