using GeniusSquareWeb.GameElements.Figures;

namespace GeniusSquareWeb.GameSolvers.DeBruijn
{
    /// <summary>
    /// De Bruijn solver.
    /// </summary>
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

        private bool SolverHelper(
            int[,] board,
            bool[] isFigurePlaced,
            ref int numberOfIterations)
        {
            numberOfIterations++;

            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            Tuple<int, int>? holeIndex = FindNextEmptyCell(board);
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
        /// Find next empty cell to fill.
        /// </summary>
        /// <param name="board">Game board.</param>
        /// <returns></returns>
        private Tuple<int, int>? FindNextEmptyCell(int[,] board)
        {
            // column
            for (int j = 0; j < board.GetLength(1); j++)
            {
                // row
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
