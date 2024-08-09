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
        public IActionResult Get()
        {
            GameManager manager = new(DefaultDices.GetAllDefaultDices());
            GameBoard initialBoard = manager.CreateInitialBoard();
   

            // transform 2d array into a jagged array to allow serialization to JSON.
            int[,] multiDimensionalArray =  initialBoard.Board;
            int rowCount = multiDimensionalArray.GetLength(0);
            int columnCount = multiDimensionalArray.GetLength(1);

            int[][] jaggedArray = new int[rowCount][];
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                jaggedArray[rowIndex] = new int[columnCount];
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    jaggedArray[rowIndex][columnIndex] = multiDimensionalArray[rowIndex, columnIndex];
                }
            }

            return Ok(jaggedArray);
        }
    }
}
