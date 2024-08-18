﻿using GeniusSquareWeb.Models;

namespace GameSolvers
{
    public class DfsSolver : ISolver
    {
        private IEnumerable<int[,]>[] figureList = new IEnumerable<int[,]>[]
        {
            DefaultFigures.Monoid.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.Domino.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TrominoI.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TrominoL.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoI.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoT.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoL.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoS.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoSquare.GetFigureOrientationsWithValueMultiplier(),
        };


        /// <inheritdoc/>
        public int[,] Solve(GameBoard board)
        {
            int[,] iteratingBoard = board.Board;
            int figureIndex = 0;

            bool result = this.SolverHelper(iteratingBoard, figureIndex);
            if (result != true)
            {
                throw new Exception("Dfs solver should have solved the game. Instead it failed");
            }

            return iteratingBoard;
        }

        private bool SolverHelper(int[,] board, int figureIndex)
        {
            IEnumerable<int[,]> listFigureOrientation = figureList[figureIndex];
            int rowCount = board.GetLength(0);
            int columnCount = board.GetLength(1);

            for (int startingRow = 0; startingRow < rowCount; startingRow++)
            {
                for (int startingColumn = 0; startingColumn < columnCount; startingColumn++)
                {
                    foreach (int[,] figureOrientation in listFigureOrientation)
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

                        // if last figureIndex return true, else try to solve it further.
                        if (figureIndex == 8 || this.SolverHelper(board, figureIndex + 1))
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

                                if (board[figureRowPlacement, figureColumnPlacement] > - 1
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
