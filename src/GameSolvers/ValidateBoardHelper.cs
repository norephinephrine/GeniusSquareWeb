using GeniusSquareWeb.GameElements.Figures;
using System.Text;

namespace GeniusSquareWeb.GameSolvers
{
    public static class ValidateBoardHelper
    {
        private static IEnumerable<int[,]>[] figureList = DefaultFigures.FigureListOrientations;

        public static bool ValidateOriginalBlockerPlacements(int[,] initialBoard, int[,] solvedBoard)
        {
            int rowCount = solvedBoard.GetLength(0);
            int columnCount = solvedBoard.GetLength(1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (initialBoard[i, j] == -1 && solvedBoard[i, j] != -1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool ValidateBlockPlacement(int[,] solvedBoard)
        {
            int rowCount = solvedBoard.GetLength(0);
            int columnCount = solvedBoard.GetLength(1);

            foreach (IEnumerable<int[,]> figureOrientationList in figureList)
            {
                // is any figure placement invalid
                bool isValidFigure = false;

                // board index
                for (int startingRow = 0; startingRow < rowCount && isValidFigure == false; startingRow++)
                {
                    // board column
                    for (int startingColumn = 0; startingColumn < columnCount && isValidFigure == false; startingColumn++)
                    {
                        // get figure orientation e.g. 1 1 or 1
                        //                                    1
                        foreach (int[,] figureOrientation in figureOrientationList)
                        {
                            int figureRowCount = figureOrientation.GetLength(0);
                            int figureColumnCount = figureOrientation.GetLength(1);

                            bool isNotValidPlacement = false;
                            for (int i = 0; i < figureRowCount && isNotValidPlacement == false; i++)
                            {
                                for (int j = 0; j < figureColumnCount && isNotValidPlacement == false; j++)
                                {
                                    int figureRowPlacement = startingRow + i;
                                    int figureColumnPlacement = startingColumn + j;

                                    if (figureRowPlacement >= rowCount
                                        || figureColumnPlacement >= columnCount
                                        || (figureOrientation[i, j] != 0 && figureOrientation[i, j] != solvedBoard[figureRowPlacement, figureColumnPlacement]))
                                    {
                                        isNotValidPlacement = true;
                                    }
                                }
                            }

                            if (!isNotValidPlacement)
                            {
                                isValidFigure = true;
                                break;
                            }
                        }
                    }
                }

                if (!isValidFigure)
                {
                    Console.WriteLine("Failed to find valid placement for figure:");
                    Print2DArray(figureOrientationList.ElementAt(0));
                    return false;
                }
            }

            return true;
        }

        public static void Print2DArray(int[,] board)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    stringBuilder.Append($"{board[i, j],3}");
                }
                stringBuilder.AppendLine();
            }

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}
