using GeniusSquareWeb.GameElements.Figures;

namespace GeniusSquareWeb.GameSolvers.DeBruijn
{
    public class DeBruijnSolver : IGameSolver
    {
        private const int FigureCount = 9;

        private IEnumerable<int[,]>[] figureList = DefaultFigures.FigureListOrientations;

        /// <inheritdoc/>
        public SolverResult Solve(int[,] board)
        {
            int[,] iteratingBoard = board;
            bool[] isFigurePlaced = new bool[FigureCount];


            int numberOfIterations = 0;
            bool result = SolverHelper(
                iteratingBoard,
                isFigurePlaced,
                ref numberOfIterations);
            if (result != true)
            {

                throw new Exception("De Bruijn solver should have solved the game. Instead it failed");
            }

            return new SolverResult
            {
                SolvedBoard = board,
                NumberOfIterations = numberOfIterations
            };
        }

        /// <summary>
        /// Solve board utilising De Bruijn with delay between iterations.
        /// </summary>
        /// <param name="board">Starting board.</param>
        /// <param name="delay">Delay between iterations</param>
        /// <returns></returns>
        public async Task SolveWithDelayAsync(
            int[,] board,
            TimeSpan delay)
        {
            int[,] iteratingBoard = board;
            bool[] isFigurePlaced = new bool[FigureCount];


            int numberOfIterations = 0;
            bool result = await SolveWithDelayHelperAsync(
                iteratingBoard,
                isFigurePlaced,
                delay);

            if (result != true)
            {

                throw new Exception("De Bruijn solver should have solved the game. Instead it failed");
            }
        }

        private bool SolverHelper(
            int[,] board,
            bool[] isFigurePlaced,
            ref int numberOfIterations)
        {
            numberOfIterations++;

            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            Tuple<int, int>? holeIndex = FindNextEmptyHole(board);
            if (holeIndex == null)
            {
                return true;
            }

            for (int figureIndex = 0; figureIndex < FigureCount; figureIndex++)
            {
                if (isFigurePlaced[figureIndex])
                {
                    continue;
                }

                foreach (int[,] figureOrientation in figureList[figureIndex])
                {
                    int figureRowCount = figureOrientation.GetLength(0);
                    int figureColumnCount = figureOrientation.GetLength(1);

                    int startingFigureRow = 0;

                    while (figureOrientation[startingFigureRow, 0] == 0)
                    {
                        startingFigureRow++;
                    }

                    int startingRow = holeIndex.Item1 - startingFigureRow;
                    int startingColumn = holeIndex.Item2;

                    // if starting row is below 0 then figure placement is invalid.
                    if (startingRow < 0)
                    {
                        continue;
                    }

                    // validate figure placement.
                    bool isNotValidFigure = false;
                    for (int i = 0; i < figureRowCount; i++)
                    {
                        for (int j = 0; j < figureColumnCount; j++)
                        {
                            int figureRowPlacement = startingRow + i;
                            int figureColumnPlacement = startingColumn + j;

                            if (figureRowPlacement >= rowCount
                                || figureColumnPlacement >= columnCount
                                || board[figureRowPlacement, figureColumnPlacement] != 0 && figureOrientation[i, j] > 0)
                            {
                                isNotValidFigure = true;
                                break;
                            }
                        }

                        if (isNotValidFigure)
                        {
                            break;
                        }
                    }

                    if (isNotValidFigure)
                    {
                        continue;
                    }

                    // place figure
                    for (int i = 0; i < figureRowCount; i++)
                    {
                        for (int j = 0; j < figureColumnCount; j++)
                        {
                            int figureRowPlacement = startingRow + i;
                            int figureColumnPlacement = startingColumn + j;

                            if (figureOrientation[i, j] != 0)
                            {
                                board[figureRowPlacement, figureColumnPlacement] = figureOrientation[i, j];
                            }
                        }
                    }

                    // next figure placement start
                    isFigurePlaced[figureIndex] = true;
                    if (SolverHelper(board, isFigurePlaced, ref numberOfIterations))
                    {
                        return true;
                    }
                    isFigurePlaced[figureIndex] = false;

                    // remove figure
                    for (int i = 0; i < figureRowCount; i++)
                    {
                        for (int j = 0; j < figureColumnCount; j++)
                        {
                            int figureRowPlacement = startingRow + i;
                            int figureColumnPlacement = startingColumn + j;

                            if (board[figureRowPlacement, figureColumnPlacement] > -1
                                && figureOrientation[i, j] != 0)
                            {
                                board[figureRowPlacement, figureColumnPlacement] = 0;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// An async version of SolverHelper with delay between iterations.
        /// 
        /// Made this its own method instead of modifying the abouve mentioned method
        /// to keep the default De Bruijn clean from async logic.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> SolveWithDelayHelperAsync(
            int[,] board,
            bool[] isFigurePlaced,
            TimeSpan delay)
        {
            await Task.Delay(delay);

            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            Tuple<int, int>? holeIndex = FindNextEmptyHole(board);
            if (holeIndex == null)
            {
                return true;
            }

            for (int figureIndex = 0; figureIndex < FigureCount; figureIndex++)
            {
                if (isFigurePlaced[figureIndex])
                {
                    continue;
                }

                foreach (int[,] figureOrientation in figureList[figureIndex])
                {
                    int figureRowCount = figureOrientation.GetLength(0);
                    int figureColumnCount = figureOrientation.GetLength(1);

                    int startingFigureRow = 0;

                    while (figureOrientation[startingFigureRow, 0] == 0)
                    {
                        startingFigureRow++;
                    }

                    int startingRow = holeIndex.Item1 - startingFigureRow;
                    int startingColumn = holeIndex.Item2;

                    // if starting row is below 0 then figure placement is invalid.
                    if (startingRow < 0)
                    {
                        continue;
                    }

                    // validate figure placement.
                    bool isNotValidFigure = false;
                    for (int i = 0; i < figureRowCount; i++)
                    {
                        for (int j = 0; j < figureColumnCount; j++)
                        {
                            int figureRowPlacement = startingRow + i;
                            int figureColumnPlacement = startingColumn + j;

                            if (figureRowPlacement >= rowCount
                                || figureColumnPlacement >= columnCount
                                || board[figureRowPlacement, figureColumnPlacement] != 0 && figureOrientation[i, j] > 0)
                            {
                                isNotValidFigure = true;
                                break;
                            }
                        }

                        if (isNotValidFigure)
                        {
                            break;
                        }
                    }

                    if (isNotValidFigure)
                    {
                        continue;
                    }

                    // place figure
                    for (int i = 0; i < figureRowCount; i++)
                    {
                        for (int j = 0; j < figureColumnCount; j++)
                        {
                            int figureRowPlacement = startingRow + i;
                            int figureColumnPlacement = startingColumn + j;

                            if (figureOrientation[i, j] != 0)
                            {
                                board[figureRowPlacement, figureColumnPlacement] = figureOrientation[i, j];
                            }
                        }
                    }

                    // next figure placement start
                    isFigurePlaced[figureIndex] = true;
                    if (await SolveWithDelayHelperAsync(board, isFigurePlaced, delay))
                    {
                        return true;
                    }
                    isFigurePlaced[figureIndex] = false;

                    // remove figure
                    for (int i = 0; i < figureRowCount; i++)
                    {
                        for (int j = 0; j < figureColumnCount; j++)
                        {
                            int figureRowPlacement = startingRow + i;
                            int figureColumnPlacement = startingColumn + j;

                            if (board[figureRowPlacement, figureColumnPlacement] > -1
                                && figureOrientation[i, j] != 0)
                            {
                                board[figureRowPlacement, figureColumnPlacement] = 0;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private Tuple<int, int>? FindNextEmptyHole(int[,] board)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    if (board[i, j] == 0)
                    {
                        return Tuple.Create(i, j);
                    }
                }
            }

            return null;
        }
    }
}
