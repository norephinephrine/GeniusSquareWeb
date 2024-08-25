using Google.OrTools.LinearSolver;

namespace GeniusSquareWeb.GameSolvers.Linear.IlpSolvers
{
    /// <summary>
    /// BOP solver that is optimized for binary variables.
    /// </summary>
    public class BopIlpSolver : IlpSolver
    {
        /// <inheritdoc/>
        public IEnumerable<LinearColumn> Solve(
            List<LinearColumn> AColumns,
            int[] b,
            int N,
            int M)
        {
            Solver solver = Solver.CreateSolver("BOP");

            if (solver == null)
            {
                throw new Exception("Bop solver is not available.");
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
