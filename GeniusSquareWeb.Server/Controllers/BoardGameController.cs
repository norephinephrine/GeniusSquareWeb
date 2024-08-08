using GeniusSquareWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeniusSquareWeb.Server.Controllers
{
    [ApiController]
    [Route("boardGame")]
    public class BoardGameController : ControllerBase
    {

        private readonly ILogger<BoardGameController> _logger;

        public BoardGameController(ILogger<BoardGameController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetInitialBoard")]
        public int[] Get()
        {
            GameManager manager = new(DefaultDices.GetAllDefaultDices());
            GameBoard initialBoard = manager.CreateInitialBoard();

            return initialBoard.Board;
        }
    }
}
