using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Figures;
using GeniusSquareWeb.GameSolvers.Linear.IlpSolvers;
using Google.OrTools.LinearSolver;


namespace GeniusSquareWeb.GameSolvers.Linear
{
    /// <summary>
    /// Transform game board state to a set of linear equations that can be solved
    /// using .
    /// </summary>
    public class LinearSolver : IGameSolver
    {
        private IEnumerable<int[,]>[] figureList = DefaultFigures.FigureListOrientations;
        private LinearColumn[] columnList = LinearGeniusSquare.GetAllPossibleColumns();

        private const int BoardRowCount = GameConstants.BoardRowCount;
        private const int BoardColumnCount = GameConstants.BoardColumnCount;

        private const int LinearRowCount = GameConstants.LinearRowCount;
        private const int MaxColumnCount = GameConstants.MaxLinearColumCount;

        private IlpSolver solver;


        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="solver"></param>
        public LinearSolver(IlpSolver solver)
        {
            this.solver = solver;
        }

        /// <inheritdoc/>
        public SolverResult Solve(int[,] board)
        {
            int[] endBoard = new int[LinearRowCount];

            // create linear board
            for (int i = 0; i < BoardRowCount; i++)
            {
                for (int j = 0; j < BoardColumnCount; j++)
                {
                    if (board[i, j] == -1)
                    {
                        continue;
                    }

                    endBoard[i * BoardColumnCount + j] = 1;
                }
            }

            for (int i = BoardRowCount * BoardColumnCount; i < LinearRowCount; i++ )
            {
                endBoard[i] = 1;
            }

            // reduce column based on blocker positions
            List<LinearColumn> reducedColumnList = new(MaxColumnCount);
            int reducedColumnCount = 0;
            foreach(LinearColumn column in columnList)
            {
                bool skip = false;
                foreach(Tuple<int, int> cell in column.nonZeroCells)
                {
                    if (board[cell.Item1, cell.Item2] == -1)
                    {
                        skip = true;
                        break;
                    }
                }

                if (skip)
                {
                    continue;
                }

                reducedColumnList.Add(column);
                reducedColumnCount++;
            }

            IEnumerable<LinearColumn> resultColumns = this.solver.Solve(
                AColumns: reducedColumnList,
                b: endBoard,
                N: LinearRowCount,
                M: reducedColumnCount);

            return new SolverResult
            {
                SolvedBoard = this.PlaceFiguresOnBoard(
                    resultColumns,
                    board),
                NumberOfIterations = 1
            };
        }

        private int[,] PlaceFiguresOnBoard(
            IEnumerable<LinearColumn> figurePlacements,
            int[,] board)
        {
            foreach (LinearColumn figure in figurePlacements)
            {
                foreach(Tuple<int,int> cell in figure.nonZeroCells)
                {
                    board[cell.Item1, cell.Item2] = figure.figureValue;
                }
            }

            return board;
        }
    }
}
