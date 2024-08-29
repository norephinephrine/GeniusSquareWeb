using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Figures;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using Node = GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver.Node;

namespace GeniusSquareWeb.Server.SolversWithDelay
{
    /// <summary>
    /// Dancing link solver utilising algorithm X with delay.
    /// </summary>
    public class DlxSolverWithDelay
    {
        private const int FigureCount = GameConstants.FigureCount;
        private Figure[] figureList = DefaultFigures.FigureList;

        private Node root;
        private Node[] placedFigure = new Node[FigureCount];
        private Func<int[,], Task<bool>> hubCallback;

        public DlxSolverWithDelay(Func<int[,], Task<bool>> callback)
        {
            this.hubCallback = callback;
            this.root = GeniusSquareDancingLinks.GenerateDancingLinksRoot();
        }

        /// <summary>
        /// Solve board utilising De Bruijn with delay between iterations.
        /// </summary>
        /// <param name="board">Starting board.</param>
        /// <param name="delay">Delay between iterations</param>
        /// <returns></returns>
        public async Task<bool> Solve(
            int[,] board,
            TimeSpan delay)
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

            int numberOfIterations = 0;
            try
            {
                bool result = await DlxIterationWithDelayAsync(0, board, delay);

                if (result != true)
                {
                    throw new Exception("Dlx solver should have solved the game. Instead it failed");
                }

                return true;
            }
            catch (GameOverException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="board"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private async Task<bool> DlxIterationWithDelayAsync(
            int k,
            int[,] board,
            TimeSpan delay)
        {
            await Task.Delay(delay);
            int[,] cloneBoard = (int[,])board.Clone();
            PlaceFiguresOnBoard(cloneBoard, placedFigure);
            await this.hubCallback(cloneBoard);

            if (root.Right == root)
            {
                return true;
            }

            // choose next minimum
            Node c = GetNextMinColumn(root);

            // cover cell
            CoverNode(c);

            // pick row
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

                if (await DlxIterationWithDelayAsync(k + 1,board, delay))
                {
                    return true;
                }

                j = r.Left;
                while (j != r)
                {
                    UncoverNode(j.ColumnHead);
                    j = j.Left;
                }

                placedFigure[k] = null;
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

            // place figures
            foreach (Node figure in placedFigure)
            {
                if (figure == null)
                {
                    continue;
                }    

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
    }
}
