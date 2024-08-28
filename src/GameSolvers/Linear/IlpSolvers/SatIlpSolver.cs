using Google.OrTools.Sat;

namespace GeniusSquareWeb.GameSolvers.Linear.IlpSolvers
{
    /// <summary>
    /// CP-SAT solver that utilises constraint programming.
    /// </summary>
    public class SatIlpSolver : IlpSolver
    {
        /// <inheritdoc/>
        public IEnumerable<LinearColumn> Solve(
            List<LinearColumn> AColumns,
            int[] b,
            int N,
            int M)
        {
            // create model
            CpModel model = new CpModel();

            BoolVar[] X = new BoolVar[M];
            for (int j = 0; j < M; j++)
            {
                X[j] = model.NewBoolVar($"X[{j}]");
            }

            // Add the linear equations
            for (int i = 0; i < N; i++)
            {
                LinearExpr linearExpression = LinearExpr.Constant(0);
                for (int j = 0; j < M; j++)
                {
                    linearExpression += AColumns[j].column[i] * X[j];
                }

                model.Add(linearExpression == b[i]);
            }

            // create solver
            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);

            // create List of selected columns
            List<LinearColumn> columnVariables = new List<LinearColumn>(16);
            for (int j = 0; j < M; j++)
            {
                if (solver.Value(X[j]) == 1)
                {
                    columnVariables.Add(AColumns[j]);
                }
            }

            return columnVariables;
        }
    }
}
