using Google.OrTools.LinearSolver;

namespace GeniusSquareWeb.GameSolvers.Linear.IlpSolvers
{
    /// <summary>
    /// SCIP solver
    /// </summary>
    public class ScipIlpSolver : IlpSolver
    {
        /// <inheritdoc/>
        public IEnumerable<LinearColumn> Solve(
            List<LinearColumn> AColumns,
            int[] b,
            int N,
            int M)
        {
            Solver solver = Solver.CreateSolver("SCIP");

            if (solver == null)
            {
                throw new Exception("SCIP solver is not available.");
            }

            // create variables 0-1 variables X
            Variable[] X = new Variable[M];
            for (int j = 0; j < M; j++)
            {
                X[j] = solver.MakeBoolVar($"X[{j}]");
            }

            // Add the linear equations
            for (int i = 0; i < N; i++)
            {
                LinearExpr linearExpression = new LinearExpr();
                for (int j = 0; j < M; j++)
                {
                    linearExpression += AColumns[j].column[i] * X[j];
                }
                solver.Add(linearExpression == b[i]);
            }

            Solver.ResultStatus resultStatus = solver.Solve();

            // create List of selected columns
            List<LinearColumn> columnVariables = new List<LinearColumn>(16);
            for (int j = 0; j < M; j++)
            {
                if (X[j].SolutionValue() == 1)
                {
                    columnVariables.Add(AColumns[j]);
                }
            }

            return columnVariables;
        }
    }
}
