using GeniusSquareWeb.GameElements.Dices;
using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System.Collections.Concurrent;

namespace SolverPlotter
{
    /// <summary>
    /// Solver plotter helper
    /// </summary>
    public class SolverPlotterHelper
    {
        private static object mutex = new object();

        private const int BlockerValue = -1;
        private const int EmptySpaceValue = 0;
        private const int GameSolutionCount = 62208; // maximum count of game solutions

        // Get a collection of dices with list of distinct dice sides
        private IEnumerable<IEnumerable<GameBoardField>> DicesWithDistinctSides =
            DefaultDices.GetAllDefaultDices()
            .Select(dice => dice.GetAllDiceSides().Distinct())
            .ToArray();

        /// <summary>
        /// This method will solve boards bassed on the solver callback method and 
        /// it will plot all the iterations and solution for the found board solution results.
        /// </summary>
        //[DynamicData(nameof(FindAllSolutionsToBoard))]
        public void PlotResultsFromSolvingBoard(Func<int[,], SolverResult> solverCallback, string solverName)
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());

            ConcurrentBag<Tuple<int, int>> iterationsAndCountBag = new();

            // when
            IEnumerable<GameBoardField> firstDice = DicesWithDistinctSides.ElementAt(0);
            Parallel.For(0, firstDice.Count(), (index) =>
            {
                GameBoard gameBoard = new GameBoard();
                gameBoard.SetGameBoardField(firstDice.ElementAt(index), BlockerValue);

                // De Bruijn
                SolveBoardHelper(
                    iterationsAndCountBag,
                    DicesWithDistinctSides,
                    1, // start at one due to initial loop from Parallel.For
                    DicesWithDistinctSides.Count() - 1,
                    gameBoard,
                    solverCallback);
            });

            // then
            // write De Bruijn solver results
            int minIteration = iterationsAndCountBag.Min(result => result.Item1);
            int maxIteration = iterationsAndCountBag.Max(result => result.Item1);
            int averageIteration = (int)Math.Floor(iterationsAndCountBag.Average(result => result.Item1));
            int minSolutionCount = iterationsAndCountBag.Min(result => result.Item2);
            int maxSolutionCount = iterationsAndCountBag.Max(result => result.Item2);
            double averageSolutionCount = (int)Math.Floor(iterationsAndCountBag.Average(result => result.Item2));
            if (iterationsAndCountBag.Count != GameSolutionCount)
            {
                throw new Exception($"{GameSolutionCount} should have been solved. But the actual number of solved board is {iterationsAndCountBag.Count}.");
            }

            PlotIterationCount(iterationsAndCountBag, solverName);
            PoltSolutionCount(iterationsAndCountBag, solverName);

            Console.WriteLine($"{solverName}:");
            Console.WriteLine($"\tIteration count");
            Console.WriteLine($"\t\tMinimum: {minIteration}");
            Console.WriteLine($"\t\tMaximum: {maxIteration}");
            Console.WriteLine($"\t\tAverage: {averageIteration}");
            Console.WriteLine($"\tSolution count");
            Console.WriteLine($"\t\tMinimum: {minSolutionCount}");
            Console.WriteLine($"\t\tMaximum: {maxSolutionCount}");
            Console.WriteLine($"\t\tAverage: {averageSolutionCount}");
        }

        private void SolveBoardHelper(
            ConcurrentBag<Tuple<int, int>> solverIterationAndSolutionCountBag,
            IEnumerable<IEnumerable<GameBoardField>> dices,
            int currentIndex,
            int maxIndex,
            GameBoard gameBoard,
            Func<int[,], SolverResult> solverCallback)
        {
            foreach (GameBoardField diceSide in dices.ElementAt(currentIndex))
            {
                gameBoard.SetGameBoardField(diceSide, BlockerValue);

                if (currentIndex != maxIndex)
                {
                    SolveBoardHelper(
                        solverIterationAndSolutionCountBag: solverIterationAndSolutionCountBag,
                        dices: dices,
                        currentIndex: currentIndex + 1,
                        maxIndex: maxIndex,
                        gameBoard: gameBoard,
                        solverCallback: solverCallback);
                }
                else
                {
                    SolverResult result = solverCallback(gameBoard.Board);
                    solverIterationAndSolutionCountBag.Add(new(result.IterationCount, result.SolutionsFoundCount));
                }

                gameBoard.SetGameBoardField(diceSide, EmptySpaceValue);
            }
        }

        private void PlotIterationCount(
            ConcurrentBag<Tuple<int, int>> bag,
            string solverName)
        {
            // convert bag to dict where keys are distinct values from bag
            // and dict values are number of key occurances in bag
            Dictionary<int, int> dict =
                bag
                .GroupBy(x => x.Item1)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());

            double[] xValues = dict.Keys.Select(x => (double)x).ToArray();
            double[] yValues = dict.Values.Select(y => (double)y).ToArray();

            // create plot
            var plotModel = new PlotModel { Title = $"Iteration count graph - {solverName}" };

            // Create logarithmic X and Y axes
            var xAxis = new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Iteration count (log scale)",
                Base = 10
            };
            plotModel.Axes.Add(xAxis);

            var yAxis = new LogarithmicAxis
            {
                Position = AxisPosition.Left,
                Title = "Group size (log scale)",
                Base = 10
            };
            plotModel.Axes.Add(yAxis);

            // Create a data series
            var series = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 1
            };


            // Generate some data that follows a power law or exponential distribution
            for (int i = 0; i < xValues.Count(); i++)
            {
                double x = i;
                double y = Math.Pow(x, 2);  // y = x^2 for demonstration
                series.Points.Add(new ScatterPoint(xValues[i], yValues[i]));
            }

            plotModel.Series.Add(series);

            // export
            using (var stream = File.Create($"{solverName.Replace(" ", "")}-Iteration.pdf"))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(plotModel, stream);
            }
        }

        private void PoltSolutionCount(
            ConcurrentBag<Tuple<int, int>> bag,
            string solverName)
        {
            // convert bag to dict where keys are distinct values from bag
            // and dict values are number of key occurances in bag
            Dictionary<int, int> dict =
                bag
                .GroupBy(x => x.Item2)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());

            double[] xValues = dict.Keys.Select(x => (double)x).ToArray();
            double[] yValues = dict.Values.Select(y => (double)y).ToArray();

            // create plot
            var plotModel = new PlotModel { Title = $"Solution count graph - {solverName}" };

            // Create logarithmic X and Y axes
            var xAxis = new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Solution count (log scale)",
                Base = 10
            };
            plotModel.Axes.Add(xAxis);

            var yAxis = new LogarithmicAxis
            {
                Position = AxisPosition.Left,
                Title = "Group size (log scale)",
                Base = 10
            };
            plotModel.Axes.Add(yAxis);

            // Create a data series
            var series = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 1
            };


            // Generate some data that follows a power law or exponential distribution
            for (int i = 0; i < xValues.Count(); i++)
            {
                double x = i;
                double y = Math.Pow(x, 2);  // y = x^2 for demonstration
                series.Points.Add(new ScatterPoint(xValues[i], yValues[i]));
            }

            plotModel.Series.Add(series);

            // export
            using (var stream = File.Create($"{solverName.Replace(" ", "")}-Solution.pdf"))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(plotModel, stream);
            }
        }
    }
}
