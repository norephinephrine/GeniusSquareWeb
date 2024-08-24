
using Node = GameSolvers.DlxSolver.Node;
namespace GameSolvers
{
    public static class GeniusSquareDancingLinks
    {
        public const int Offset = 100;

        const int FigureCount = 9;

        const int BoardRowCount = 6;
        const int BoardColumnCount = 6;

        // 625 placements + 1 for list header
        const int NodeRowCount = 626;

        // 9 figures + 6x6 cells
        const int NodeColumnCount = 45; 

        public static Node GenerateBoard()
        {
            int[,] board = new int[BoardColumnCount, BoardColumnCount];

            Figure[] figureList = DefaultFigures.FigureList;

            Node[,] nodeMatrix = new Node[NodeRowCount, NodeColumnCount];

            // place list header
            for (int i = 0; i < figureList.Length; i++)
            {
                nodeMatrix[0, i] = new Node();
                nodeMatrix[0, i].ColumnHead = nodeMatrix[0, i];
                nodeMatrix[0, i].Value = Offset + figureList[i].Value;
                nodeMatrix[0, i].Size = 0;
            }

            for (int i = FigureCount; i < FigureCount + BoardRowCount * BoardColumnCount; i++)
            {
                nodeMatrix[0, i] = new Node();
                nodeMatrix[0, i].ColumnHead = nodeMatrix[0, i];
                nodeMatrix[0, i].Value = i - FigureCount;
                nodeMatrix[0, i].Size = 0;
            }

            // place rows
            int rowIndex = 1;
            for (int figureIndex = 0; figureIndex < figureList.Length; figureIndex ++)
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
                            for (int i= 0; i < figureRowCount; i++)
                            {
                                for(int j= 0; j < figureColumCount; j++)
                                {
                                    if (figureOrientation[i , j] != 0)
                                    {
                                        // calculate cell index
                                        int cellIndex =
                                            FigureCount
                                            + (boardRow + i) * 6
                                            + (boardColumn + j);

                                        nodeMatrix[rowIndex, cellIndex] = new();
                                        nodeMatrix[rowIndex, cellIndex].ColumnHead = nodeMatrix[0, cellIndex];

                                        nodeMatrix[0, cellIndex].Size++;
                                    }
                                }    
                            }
                            nodeMatrix[rowIndex, figureIndex] = new();
                            nodeMatrix[rowIndex, figureIndex].ColumnHead = nodeMatrix[0, figureIndex];

                            nodeMatrix[0, figureIndex].Size++;
                            rowIndex++;
                        }
                    }
                }
            }

            // connect the nodes
            for (int i = 0; i < NodeRowCount; i++)
            {
                int startingColumnindex;
                for (startingColumnindex = 0; startingColumnindex < NodeColumnCount; startingColumnindex++)
                {
                    if (nodeMatrix[i , startingColumnindex] != null)
                    {
                        break;
                    }
                }
                Node currentNode = nodeMatrix[i, startingColumnindex];
                currentNode.Right = currentNode;
                currentNode.Left = currentNode;

                Node predecessor = currentNode;
                for (int j = startingColumnindex + 1; j < NodeColumnCount; j++)
                {
                    if (nodeMatrix[i, j] != null)
                    {
                        nodeMatrix[i, j].Right = currentNode;
                        nodeMatrix[i, j].Left = predecessor;
                        predecessor.Right = nodeMatrix[i, j];
                        currentNode.Left = nodeMatrix[i, j];

                        predecessor = nodeMatrix[i, j];
                    }
                }    
            }

            for (int j = 0; j < NodeColumnCount; j++)
            {         
                Node currentNode = nodeMatrix[0, j];
                currentNode.Up = currentNode;
                currentNode.Down = currentNode;

                Node predecessor = currentNode;
                for (int i = 1; i < NodeRowCount; i++)
                {
                    if (nodeMatrix[i, j] != null)
                    {
                        nodeMatrix[i, j].Down = currentNode;
                        nodeMatrix[i, j].Up = predecessor;
                        predecessor.Down = nodeMatrix[i, j];
                        currentNode.Up = nodeMatrix[i, j];

                        predecessor = nodeMatrix[i, j];
                    }
                }
            }

            // create root
            Node root = new Node();
            root.Right = nodeMatrix[0, 0];
            nodeMatrix[0, 0].Left = root;
            root.Left = nodeMatrix[0, NodeColumnCount - 1];
            nodeMatrix[0, NodeColumnCount - 1].Right = root;
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
                    if (board[i,j]!=null)
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
