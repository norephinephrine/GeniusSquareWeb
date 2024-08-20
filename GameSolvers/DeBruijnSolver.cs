using GeniusSquareWeb.Models;

namespace GameSolvers
{
    public class DeBruijnSolver : ISolver
    {
        private const int FigureCount = 9;

        private IEnumerable<int[,]>[] figureList = DefaultFigures.FigureList;
        private bool[] isFigurePlaced = new bool[9];

        public DeBruijnSolver()
        {
            for (int i = 0; i < 9; i++)
            {
                isFigurePlaced[i] = false;
            }
        }

        /// <inheritdoc/>
        public int[,] Solve(GameBoard board)
        {
            int[,] iteratingBoard = board.Board;
            int figureIndex = 0;

            bool result = this.SolverHelper(iteratingBoard);
            if (result != true)
            {
                throw new Exception("Backtracking solver should have solved the game. Instead it failed");
            }

            return iteratingBoard;
        }

        private bool SolverHelper(int[,] board)
        {
            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            Tuple<int, int> holeIndex = this.FindNextEmptyHole(board);
            if (holeIndex == null)
            {
                return true;
            }

            for (int figureIndex = 0; figureIndex < FigureCount; figureIndex ++)
            {
                if (this.isFigurePlaced[figureIndex])
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
                    if(startingRow < 0)
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
                                || (board[figureRowPlacement, figureColumnPlacement] != 0 && figureOrientation[i, j] > 0))
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
                    this.isFigurePlaced[figureIndex] = true;
                    if (this.SolverHelper(board))
                    {
                        return true;
                    }
                    this.isFigurePlaced[figureIndex] = false;

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

        private Tuple<int, int> FindNextEmptyHole(int[,] board)
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
