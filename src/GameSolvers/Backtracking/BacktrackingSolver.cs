using GeniusSquareWeb.GameElements.Figures;

namespace GeniusSquareWeb.GameSolvers.Backtracking
{
    /// <summary>
    /// Bactracking solver.
    /// </summary>
    public class BacktrackingSolver : IGameSolver
    {
        private IEnumerable<int[,]>[] figureList = DefaultFigures.FigureListOrientations;

        /// <inheritdoc/>
        public SolverResult FindOneSolution(int[,] board)
        {
            int[,] iteratingBoard = board;
            int figureIndex = 0;

            int iterationCount = 0;
            int solutionsFoundCount = 0;
            bool result = SolverHelper(
                iteratingBoard,
                figureIndex,
                ref iterationCount,
                ref solutionsFoundCount,
                false);

            if (result != true)
            {
                throw new Exception("Backtracking solver should have solved the game. Instead it failed");
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
            int figureIndex = 0;

            int iterationCount = 0;
            int solutionFoundCount = 0;
            _ = SolverHelper(
                iteratingBoard,
                figureIndex,
                ref iterationCount,
                ref solutionFoundCount,
                true);

            if (solutionFoundCount == 0)
            {
                throw new Exception("Backtracking should have found some solutions. Instead it found none");
            }

            return new SolverResult
            {
                SolvedBoard = null,
                IterationCount = iterationCount,
                SolutionsFoundCount = solutionFoundCount
            };
        }

        private bool SolverHelper(
            int[,] board,
            int figureIndex,
            ref int iterationCount,
            ref int solutionsFoundCount,
            bool shouldFindAllSolutions = false)
        {
            
            if (figureIndex == 9)
            {
                solutionsFoundCount++;
                if (shouldFindAllSolutions)
                {
                    return false;
                }

                return true;
            }

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
                        if (SolverHelper(board, figureIndex + 1, ref iterationCount, ref solutionsFoundCount, shouldFindAllSolutions))
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
