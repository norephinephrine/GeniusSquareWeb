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
        public SolverResult FindOneSolution(int[,] board)
        {
            int[,] iteratingBoard = board;
            bool[] isFigurePlaced = new bool[FigureCount];


            int iterationCount = 0;
            int solutionsFoundCount = 0;
            bool result = SolverHelper(
                iteratingBoard,
                isFigurePlaced,
                ref iterationCount,
                ref solutionsFoundCount,
                false);
            if (result != true)
            {

                throw new Exception("De Bruijn solver should have solved the game. Instead it failed");
            }

            return new SolverResult
            {
                SolvedBoard = board,
                IterationCount = iterationCount,
                SolutionsFoundCount = solutionsFoundCount,
            };
        }

        /// <inheritdoc/>
        public SolverResult FindAllSolutions(int[,] board)
        {
            int[,] iteratingBoard = board;
            bool[] isFigurePlaced = new bool[FigureCount];

            int iterationCount = 0;
            int solutionsFoundCount = 0;
            _ = SolverHelper(
                iteratingBoard,
                isFigurePlaced,
                ref iterationCount,
                ref solutionsFoundCount,
                true);

            if (solutionsFoundCount == 0)
            {
                throw new Exception("De Bruijn solver should have found a solution. Instead it found none");
            }

            return new SolverResult
            {
                SolvedBoard = null,
                IterationCount = iterationCount,
                SolutionsFoundCount = solutionsFoundCount,
            };
        }

        private bool SolverHelper(
            int[,] board,
            bool[] isFigurePlaced,
            ref int iterationCount,
            ref int solutionsFoundCount,
            bool shouldFindAllSolutions)
        {
            iterationCount++;

            // find next empty cell,
            // If null is returned that means there are
            // no more empty cells and a solution has been found
            Tuple<int, int>? holeIndex = FindNextEmptyCell(board);
            if (holeIndex == null)
            {
                solutionsFoundCount++;

                if (shouldFindAllSolutions)
                {
                    return false;
                }

                return true;
            }


            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

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
                    // if placement is invalid set isNotValidFigure to true.
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

                    // continue to next loop since current placement is invalid.
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
                    isFigurePlaced[figureIndex] = true;

                    // next figure placement start
                    if (SolverHelper(board, isFigurePlaced, ref iterationCount, ref solutionsFoundCount, shouldFindAllSolutions))
                    {
                        return true;
                    }

                    // remove figure
                    isFigurePlaced[figureIndex] = false;
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
