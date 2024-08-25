using GeniusSquareWeb.GameElements;
using System.Collections;

namespace GeniusSquareWeb.GameSolvers.Linear
{
    public static class LinearGeniusSquare
    {
        private const int FigureCount = GameConstants.FigureCount;
        private const int BoardRowCount = GameConstants.BoardRowCount;
        private const int BoardColumnCount = GameConstants.BoardColumnCount;

        // The column size.
        private const int ColumnSize = GameConstants.LinearRowCount;
        private const int MaxColumnCount = GameConstants.MaxLinearColumCount;

        public static LinearColumn[] GetAllPossibleColumns()
        {
            LinearColumn[] columnList = new LinearColumn[MaxColumnCount];
            Figure[] figureList = DefaultFigures.FigureList;
             int columnCount = 0;

            // place rows
            for (int figureIndex = 0; figureIndex < figureList.Length; figureIndex++)
            {
                Figure f = figureList[figureIndex];

                foreach (int[,] figureOrientation in f.GetFigureOrientationsWithValueMultiplier())
                {
                    int figureRowCount = figureOrientation.GetLength(0);
                    int figureColumCount = figureOrientation.GetLength(1);

                    for (int boardRow = 0; boardRow <= BoardRowCount - figureRowCount; boardRow++)
                    {
                        for (int boardColumn = 0; boardColumn <= BoardColumnCount - figureColumCount; boardColumn++)
                        {
                            int[] column = new int[ColumnSize];
                            List<Tuple<int, int>> nonZeroElements =
                                new List<Tuple<int,int>>();

                            for (int i = 0; i < figureRowCount; i++)
                            {
                                for (int j = 0; j < figureColumCount; j++)
                                {
                                    if (figureOrientation[i, j] != 0)
                                    {
                                        int rowPlacement = boardRow + i;
                                        int columnPlacement = boardColumn + j;

                                        column[rowPlacement*BoardColumnCount + columnPlacement] = 1;
                                        nonZeroElements.Add(new Tuple<int, int>(rowPlacement, columnPlacement));
                                    }
                                }
                            }

                            column[BoardRowCount * BoardColumnCount + figureIndex] = 1;
                            columnList[columnCount] = new LinearColumn(column, nonZeroElements, f.Value);
                            columnCount++;
                        }
                    }
                }
            }

            return columnList;
        }
    }
}
