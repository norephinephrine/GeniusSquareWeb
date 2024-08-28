using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameSolversTests
{
    public static class Utilities
    {
        private static IEnumerable<int[,]>[] figureList = DefaultFigures.FigureListOrientations;
        private const int BlockerValue = -1;
        private const int EmptySpaceValue = 0;
        private const int GameSolutionCount = 62208; // Genius Square has 62208 possible board combinations.


        /// <summary>
        /// Validate all game solutions.
        /// </summary>
        /// <param name="gameSolver">Solver to validate.</param>
        /// <param name="runParallel">Should method run in parallel.</param>
        public static void SolveAndValidateAllGameBoards(IGameSolver gameSolver, bool runParallel)
        {
            // Get a collection of dices with list of distinct dice sides
            IEnumerable<IEnumerable<GameBoardField>> dicesWithDistinctSides =
                DefaultDices.GetAllDefaultDices()
                .Select(dice => dice.GetAllDiceSides().Distinct());

            IEnumerable<GameBoardField> firstDice = dicesWithDistinctSides.ElementAt(0);

            int solutionsFound = 0;

            // some solvers like the DlxSolver are not thread safe so we control which solvers
            // get run parallely.
            if (runParallel)
            {
                Parallel.For(0, dicesWithDistinctSides.ElementAt(0).Count(), (index) =>
                {
                    GameBoard gameBoard = new GameBoard();
                    gameBoard.SetGameBoardField(firstDice.ElementAt(index), BlockerValue);

                    ValidateAllGameSolutionHelper(
                        dicesWithDistinctSides,
                        1, // start at one due to initial loop from Parallel.For
                        dicesWithDistinctSides.Count() - 1,
                        ref solutionsFound,
                        gameBoard,
                        gameSolver);
                });
            }
            else
            {
                GameBoard gameBoard = new GameBoard();
                ValidateAllGameSolutionHelper(
                    dicesWithDistinctSides,
                    0,
                    dicesWithDistinctSides.Count() - 1,
                    ref solutionsFound,
                    gameBoard,
                    gameSolver);
            }

            // compare if the count of solutions found in method is equal to the actual count
            Assert.AreEqual(GameSolutionCount, solutionsFound);
        }

        private static void ValidateAllGameSolutionHelper (
            IEnumerable<IEnumerable<GameBoardField>> dices,
            int currentIndex,
            int maxIndex,
            ref int solutionsFound,
            GameBoard gameBoard,
            IGameSolver gameSolver)
        {
            foreach (GameBoardField diceSide in dices.ElementAt(currentIndex))
            {
                gameBoard.SetGameBoardField(diceSide, BlockerValue);

                if (currentIndex != maxIndex)
                {
                    ValidateAllGameSolutionHelper(dices, currentIndex + 1, maxIndex, ref solutionsFound, gameBoard, gameSolver);
                }
                else
                {
                    int[,] solvedBoard = gameSolver.Solve(gameBoard.Board).SolvedBoard;
                    ValidateGameSolution(gameBoard.Board, solvedBoard);
                    Interlocked.Increment(ref solutionsFound);
                }    

                gameBoard.SetGameBoardField(diceSide, EmptySpaceValue);
            }
        }

        /// <summary>
        /// Validate solution.
        /// Will throw exception if it is invalid.
        /// </summary>
        /// <param name="initialBoard">Initial starting board.</param>
        /// <param name="solvedBoard">Solved board.</param>
        public static void ValidateGameSolution(int[,] initialBoard, int[,] solvedBoard)
        {
            Print2DArray(solvedBoard);

            Assert.IsTrue(
                ValidateOriginalBlockerPlacements(initialBoard, solvedBoard),
                "Blockining blocks from original board don't match the ones found in solution");

            Assert.IsTrue(
                ValidateBlockPlacement(solvedBoard),
                "Solution is invalid.");
        }


        private static bool ValidateOriginalBlockerPlacements(int[,] initialBoard, int[,] solvedBoard)
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

        private static void Print2DArray(int[,] board)
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
