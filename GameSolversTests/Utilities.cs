using GameSolvers;

namespace GameSolversTests
{
    public static class Utilities
    {
        private static IEnumerable<int[,]>[] figureList = DefaultFigures.FigureList;

        /// <summary>
        /// Validate solution.
        /// Will throw exception if it is invalid.
        /// </summary>
        /// <param name="initialBoard">Initial starting board.</param>
        /// <param name="solvedBoard">Solved board.</param>
        public static void ValidateBlockSolution(int[,] initialBoard, int[,] solvedBoard)
        {
            Print2DArray(solvedBoard);

            Assert.IsTrue(
                ValidateOriginalBlockingBlocks(initialBoard, solvedBoard),
                "Blockining blocks from original board don't match the ones found in solution");

            Assert.IsTrue(
                ValidateBlockPlacement(solvedBoard),
                "Solution is invalid.");
        }


        private static bool ValidateOriginalBlockingBlocks(int[,] initialBoard, int[,] solvedBoard)
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

        private static bool ValidateBlockPlacement(int[,] solvedBoard)
        {
            int rowCount = solvedBoard.GetLength(0);
            int columnCount = solvedBoard.GetLength(1);

            foreach (IEnumerable<int[,]> figureOrientationList in figureList)
            {
                // validate figure placement.
                bool isValidFigure = false;
                for (int startingRow = 0; startingRow < rowCount && isValidFigure == false; startingRow++)
                {
                    for (int startingColumn = 0; startingColumn < columnCount && isValidFigure == false; startingColumn++)
                    {
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

        private static void Print2DArray(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write($"{board[i, j],3}");
                }
                Console.WriteLine();
            }
        }
    }
}
