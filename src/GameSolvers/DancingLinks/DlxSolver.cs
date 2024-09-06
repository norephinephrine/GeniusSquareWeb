using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Figures;

namespace GeniusSquareWeb.GameSolvers.DancingLinks
{
    /// <summary>
    /// Dancing link solver utilising algorithm X.
    /// </summary>
    public class DlxSolver : IGameSolver
    {
        private const int FigureCount = GameConstants.FigureCount;
        private Figure[] figureList = DefaultFigures.FigureList;

        private Node root;
        private Node[] placedFigure = new Node[FigureCount];
        public DlxSolver(Node root)
        {
            this.root = root;
        }

        /// <inheritdoc/>
        public SolverResult FindOneSolution(int[,] board)
        {
            // reduce dancing links
            Node current = root.Right;
            List<Node> nodes = new();
            for (int i = 0; i < figureList.Length; i++)
            {
                current = current.Right;
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == -1)
                    {
                        CoverColumn(current);
                        nodes.Insert(0, current);
                    }

                    current = current.Right;
                }
            }

            int iterationCount = 0;
            int solutionsFoundCount = 0;
            if (!DlxIteration(0, ref iterationCount, ref solutionsFoundCount, false))
            {
                throw new Exception("Dlx solver should have solved the game. Instead it failed");

            }

            PlaceFiguresOnBoard(board, placedFigure);

            for (int i = FigureCount - 1; i >= 0; i--)
            {
                current = placedFigure[i];

                do
                {
                    current = current.Left;
                    UncoverColumn(current.ColumnHead);
                }
                while (current != placedFigure[i]);
            }

            foreach (Node node in nodes)
            {
                UncoverColumn(node.ColumnHead);
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
            // reduce dancing links
            Node current = root.Right;
            List<Node> nodes = new();
            for (int i = 0; i < figureList.Length; i++)
            {
                current = current.Right;
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == -1)
                    {
                        CoverColumn(current);
                        nodes.Insert(0, current);
                    }

                    current = current.Right;
                }
            }

            int iterationCount = 0;
            int solutionsFoundCount = 0;
            _ = DlxIteration(0, ref iterationCount, ref solutionsFoundCount, true);
            if (solutionsFoundCount == 0)
            {
                throw new Exception("Dlx solver should found some solutions. Instead it found none.");

            }

            PlaceFiguresOnBoard(board, placedFigure);

            for (int i = FigureCount - 1; i >= 0; i--)
            {
                current = placedFigure[i];

                do
                {
                    current = current.Left;
                    UncoverColumn(current.ColumnHead);
                }
                while (current != placedFigure[i]);
            }

            foreach (Node node in nodes)
            {
                UncoverColumn(node.ColumnHead);
            }

            return new SolverResult
            {
                SolvedBoard = null,
                IterationCount = iterationCount,
                SolutionsFoundCount = solutionsFoundCount,
            };
        }

        private bool DlxIteration(
            int k,
            ref int iterationCount,
            ref int solutionsFoundCount,
            bool shouldFindAllSolutions)
        {
            iterationCount++;

            // if root.Right is equal to root that means that all columns were covered
            // and that a solution has been found.
            if (root.Right == root)
            {
                solutionsFoundCount++;

                if (shouldFindAllSolutions)
                {
                    return false;
                }

                return true;
            }

            // choose next minimum
            Node c = GetNextMinColumn(root);

            // cover column
            CoverColumn(c);

            // pick row
            Node r = c.Down;
            while (r != c)
            {
                // save starting Node in linked list representing row.
                placedFigure[k] = r;

                // cover all columns of the chosen row
                Node j = r.Right;
                while (j != r)
                {
                    CoverColumn(j.ColumnHead);
                    j = j.Right;
                }

                // next iteration
                if (DlxIteration(k + 1, ref iterationCount, ref solutionsFoundCount, shouldFindAllSolutions))
                {
                    return true;
                }

                // uncover all columns of the chosen row
                j = r.Left;
                while (j != r)
                {
                    UncoverColumn(j.ColumnHead);
                    j = j.Left;
                }

                r = r.Down;
            }

            // uncover column
            UncoverColumn(c);
            return false;
        }

        /// <summary>
        /// Choose next column with minimum size for DLX to process. 
        /// </summary>
        private Node GetNextMinColumn(Node root)
        {
            Node currentNode = root.Right;

            Node minimumNode = currentNode;
            currentNode = root.Right.Right;
            while (currentNode != root)
            {
                if (currentNode.Size < minimumNode.Size)
                {
                    minimumNode = currentNode;
                }

                currentNode = currentNode.Right;
            }

            return minimumNode;
        }

        private void CoverColumn(Node c)
        {
            Node i = null;
            Node j = null;

            c.Right.Left = c.Left;
            c.Left.Right = c.Right;

            i = c.Down;
            while (i != c)
            {
                j = i.Right;
                while (j != i)
                {
                    j.Down.Up = j.Up;
                    j.Up.Down = j.Down;

                    j.ColumnHead.Size--;

                    j = j.Right;
                }
                i = i.Down;
            }
        }

        private void UncoverColumn(Node c)
        {
            Node i = null;
            Node j = null;

            i = c.Up;
            while (i != c)
            {
                j = i.Left;
                while (j != i)
                {
                    j.ColumnHead.Size++;

                    j.Down.Up = j;
                    j.Up.Down = j;

                    j.ColumnHead.Size--;
                    j = j.Left;
                }

                i = i.Up;
            }

            c.Right.Left = c;
            c.Left.Right = c;
        }

        private void PlaceFiguresOnBoard(
            int[,] startingBoard,
            Node[] placedFigure)
        {
            const int offset = GameConstants.DancingLinkFigureOffset;

            foreach (Node figure in placedFigure)
            {
                // get figure value
                int value = figure.ColumnHead.Value - offset;
                Node startingNode = figure;
                Node i = figure.Right;

                while (i != figure)
                {
                    if (i.ColumnHead.Value >= offset)
                    {
                        startingNode = i;
                        value = i.ColumnHead.Value - offset;
                    }

                    i = i.Right;
                }

                i = startingNode.Right;
                while (i != startingNode)
                {
                    int index = i.ColumnHead.Value;
                    int modul = startingBoard.GetLength(0);

                    startingBoard[index / modul, index % modul] = value;
                    i = i.Right;
                }
            }
        }

        public class Node
        {
            public Node Left;
            public Node Right;
            public Node Up;
            public Node Down;
            public Node ColumnHead;
            public int Size;
            public int Value;
        }
    }
}
