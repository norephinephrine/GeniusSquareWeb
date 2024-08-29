using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Dices;
using GeniusSquareWeb.GameSolvers;

namespace GameSolversTests
{
    public static class Utilities
    {
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
            ValidateBoardHelper.Print2DArray(solvedBoard);

            Assert.IsTrue(
                ValidateBoardHelper.ValidateOriginalBlockerPlacements(initialBoard, solvedBoard),
                "Blockining blocks from original board don't match the ones found in solution");

            Assert.IsTrue(
                ValidateBoardHelper.ValidateBlockPlacement(solvedBoard),
                "Solution is invalid.");
        }
    }
}
