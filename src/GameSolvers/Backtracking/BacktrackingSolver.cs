using GeniusSquareWeb.GameElements.Figures;

namespace GeniusSquareWeb.GameSolvers.Backtracking
{
    public class BacktrackingSolver : IGameSolver
    {
        private IEnumerable<int[,]>[] figureList = DefaultFigures.FigureListOrientations;

        /// <inheritdoc/>
        public SolverResult Solve(int[,] board)
        {
            int[,] iteratingBoard = board;
            int figureIndex = 0;

            int iterationCount = 0;
            bool result = SolverHelper(
                iteratingBoard,
                figureIndex,
                ref iterationCount);

            if (result != true)
            {
                throw new Exception("Backtracking solver should have solved the game. Instead it failed");
            }

            return new SolverResult
            {
                SolvedBoard = board,
                NumberOfIterations = iterationCount,
            };
        }

        /// <summary>
        /// Solve board utilising Backtracking with delay between iterations.
        /// </summary>
        /// <param name="board">Starting board.</param>
        /// <param name="delay">Delay between iterations</param>
        /// <returns></returns>
        public async Task SolveWithDelayAsync(
            int[,] board,
            TimeSpan delay)
        {
            int[,] iteratingBoard = board;
            int figureIndex = 0;

            int iterationCount = 0;
            bool result = SolveWithDelayHelperAsync(
                iteratingBoard,
                figureIndex,
                delay);

            if (result != true)
            {
                throw new Exception("Backtracking solver should have solved the game. Instead it failed");
            }
        }

        private bool SolverHelper(
            int[,] board,
            int figureIndex,
            ref int iterationCount)
        {
            iterationCount++;

            IEnumerable<int[,]> figureOrientationList = figureList[figureIndex];
            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            for (int startingRow = 0; startingRow < rowCount; startingRow++)
            {
                for (int startingColumn = 0; startingColumn < columnCount; startingColumn++)
                {
                    foreach (int[,] figureOrientation in figureOrientationList)
                    {
                        int figureRowCount = figureOrientation.GetLength(0);
                        int figureColumnCount = figureOrientation.GetLength(1);

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

                        // if last figureIndex return true, else try to solve it further.
                        if (figureIndex == 8
                            || SolverHelper(board, figureIndex + 1, ref iterationCount))
                        {
                            return true;
                        }

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
            }

            return false;
        }

        /// <summary>
        /// An async version of SolverHelper with delay between iterations.
        /// 
        /// This its own method instead of unifying with the abouve mentioned method
        /// to keep the default De Bruijn clean from async logic.
        /// </summary>
        /// <returns></returns>
        private bool SolveWithDelayHelperAsync(
            int[,] board,
            int figureIndex,
            TimeSpan delay)
        {
            IEnumerable<int[,]> figureOrientationList = figureList[figureIndex];
            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            for (int startingRow = 0; startingRow < rowCount; startingRow++)
            {
                for (int startingColumn = 0; startingColumn < columnCount; startingColumn++)
                {
                    foreach (int[,] figureOrientation in figureOrientationList)
                    {
                        int figureRowCount = figureOrientation.GetLength(0);
                        int figureColumnCount = figureOrientation.GetLength(1);

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

                        // if last figureIndex return true, else try to solve it further.
                        if (figureIndex == 8
                            || SolveWithDelayHelperAsync(board, figureIndex + 1, delay))
                        {
                            return true;
                        }

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
            }

            return false;
        }
    }
}
