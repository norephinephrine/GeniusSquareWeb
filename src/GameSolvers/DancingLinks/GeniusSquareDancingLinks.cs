using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Figures;
using Node = GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver.Node;

namespace GeniusSquareWeb.GameSolvers.DancingLinks
{
    public static class GeniusSquareDancingLinks
    {
        private const int Offset = GameConstants.DancingLinkFigureOffset;
        private const int FigureCount = GameConstants.FigureCount;

        const int BoardRowCount = GameConstants.BoardRowCount;
        const int BoardColumnCount = GameConstants.BoardColumnCount;

        private const int NodeRowCount = GameConstants.NodeRowCountDancingLinks;

        private const int NodeColumnCount = GameConstants.NodeColumnCountDancingLinks;

        public static Node GenerateDancingLinksRoot()
        {
            int[,] board = new int[BoardColumnCount, BoardColumnCount];

            Figure[] figureList = DefaultFigures.FigureList;
            Node[] columnHeadRow = new Node[NodeColumnCount];

            // create first list header row
            Node firstNode = new Node();
            firstNode.Right = firstNode;
            firstNode.Left = firstNode;
            firstNode.Up = firstNode;
            firstNode.Down = firstNode;
            firstNode.ColumnHead = firstNode;
            firstNode.Value = Offset + figureList[0].Value;
            firstNode.Size = 0;

            columnHeadRow[0] = firstNode;

            // place list header representing figures
            Node previous = firstNode;
            for (int i = 1; i < figureList.Length; i++)
            {
                Node newNode = new Node();
                newNode.ColumnHead = newNode;
                newNode.Value = Offset + figureList[i].Value;
                newNode.Size = 0;
                newNode.Up = newNode;
                newNode.Down = newNode;

                newNode.Right = firstNode;
                newNode.Left = previous;
                previous.Right = newNode;
                firstNode.Left = newNode;

                previous = newNode;
                columnHeadRow[i] = newNode;
            }

            // place list header representing cells.
            for (int i = FigureCount; i < FigureCount + BoardRowCount * BoardColumnCount; i++)
            {
                Node newNode = new Node();
                newNode.ColumnHead = newNode;
                newNode.Value = i - FigureCount;
                newNode.Size = 0;
                newNode.Up = newNode;
                newNode.Down = newNode;

                newNode.Right = firstNode;
                newNode.Left = previous;
                previous.Right = newNode;
                firstNode.Left = newNode;

                previous = newNode;
                columnHeadRow[i] = newNode;
            }

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
                            // create first row node
                            Node figureColumnHead = columnHeadRow[figureIndex];
                            figureColumnHead.Size++;

                            Node firstRowNode = new Node();
                            firstRowNode.ColumnHead = figureColumnHead;
                            firstRowNode.Up = figureColumnHead.Up;
                            firstRowNode.Down = figureColumnHead;
                            figureColumnHead.Up.Down = firstRowNode;
                            figureColumnHead.Up = firstRowNode;

                            firstRowNode.Right = firstRowNode;
                            firstRowNode.Left = firstRowNode;

                            previous = firstRowNode;
                            for (int i = 0; i < figureRowCount; i++)
                            {
                                for (int j = 0; j < figureColumCount; j++)
                                {
                                    if (figureOrientation[i, j] != 0)
                                    {
                                        // calculate cell index
                                        int cellIndex =
                                            FigureCount
                                            + (boardRow + i) * 6
                                            + boardColumn + j;

                                        Node columnHeadNode = columnHeadRow[cellIndex];
                                        columnHeadNode.Size++;

                                        Node newNode = new();

                                        // link column
                                        newNode.ColumnHead = columnHeadNode;
                                        newNode.Up = columnHeadNode.Up;
                                        newNode.Down = columnHeadNode;
                                        columnHeadNode.Up.Down = newNode;
                                        columnHeadNode.Up = newNode;

                                        newNode.Left = previous;
                                        newNode.Right = firstRowNode;
                                        previous.Right = newNode;
                                        firstRowNode.Left = newNode;

                                        previous = newNode;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // create root
            Node root = new Node();
            root.Right = firstNode;
            root.Left = firstNode.Left;
            firstNode.Left.Right = root;
            firstNode.Left = root;
            root.Value = 9999;

            // print board if needed
            // PrintNodeBoard(nodeMatrix);

            return root;
        }

        private static void PrintNodeBoard(Node[,] board)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write($"{board[0, j].Value,5}");
            }
            Console.WriteLine();

            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write($"{board[0, j].Size,5}");
            }
            Console.WriteLine();


            for (int i = 1; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != null)
                    {
                        Console.Write($"{1,5}");
                    }
                    else
                    {
                        Console.Write($"{0,5}");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
