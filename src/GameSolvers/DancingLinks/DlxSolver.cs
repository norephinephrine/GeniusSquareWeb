using GeniusSquareWeb.GameElements;

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

        public int[,] Solve(int[,] board)
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
                        CoverNode(current);
                        nodes.Insert(0, current);
                    }

                    current = current.Right;
                }
            }

            if (!DlxIteration(0))
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
                    UncoverNode(current.ColumnHead);
                }
                while (current != placedFigure[i]);
            }

            foreach (Node node in nodes)
            {
                UncoverNode(node.ColumnHead);
            }

            return board;
        }

        private bool DlxIteration(int k)
        {
            if (root.Right == root)
            {
                return true;
            }

            // choose next minimum
            Node c = GetNextMinColumn(root);

            // cover cell
            CoverNode(c);

            // logic?
            Node r = c.Down;
            while (r != c)
            {
                placedFigure[k] = r;

                Node j = r.Right;
                while (j != r)
                {
                    CoverNode(j.ColumnHead);
                    j = j.Right;
                }

                if (DlxIteration(k + 1))
                {
                    return true;
                }
                // potentailly not needed
                // r = placedFigure[0]
                //c = r.ColumnHead;

                j = r.Left;
                while (j != r)
                {
                    UncoverNode(j.ColumnHead);
                    j = j.Left;
                }

                r = r.Down;
            }

            // uncover cell
            UncoverNode(c);
            return false;
        }

        /// <summary>
        /// Choose next cell with minimum size for DLX to process.
        /// 
        /// </summary>
        private Node GetNextMinColumn(Node root)
        {
            Node iNode = null;
            Node jNode = null;

            iNode = root.Right.Right;
            Node minimumNode = root.Right;
            while (iNode != root)
            {
                if (iNode.Size < minimumNode.Size)
                {
                    minimumNode = iNode;
                }

                iNode = iNode.Right;
            }

            return minimumNode;
        }

        private void CoverNode(Node c)
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

        private void UncoverNode(Node c)
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
