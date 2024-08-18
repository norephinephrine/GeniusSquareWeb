namespace GameSolvers
{
    public class Figure
    {
        private static int ValueMultiplier = 0;

        private int valueMultiplier = Interlocked.Increment(ref ValueMultiplier);

        private List<int[,]> figureOrientations;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="figureShape">Figure shape.</param>
        /// <param name="figureTransformation">Figure transformation.</param>
        public Figure(
            int[,] figureShape,
            FigureTransformation figureTransformation)
        {
            int numberOfRotations = this.GetNumberOfRotations(figureTransformation);

            bool shouldFlip =
                figureTransformation == FigureTransformation.TwoRotationsAndReflection
                || figureTransformation == FigureTransformation.FourRotationsAndReflection;

            figureOrientations = new List<int[,]>();

            int[,] newFigure = figureShape;
            for (int i = 0; i < (int) numberOfRotations; i++)
            {
                figureOrientations.Add(newFigure);
                newFigure = RotateRight(newFigure);     
            }

            if (!shouldFlip)
            {
                return;
            }

            // reset figure and rotate it over the X axis
            newFigure = FlipOverXAxis(newFigure);

            for (int i = 0; i < (int)numberOfRotations; i++)
            {
                figureOrientations.Add(newFigure);
                newFigure = RotateRight(newFigure);
            }
        }

        /// <summary>
        /// Get all figure orientations;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int[,]> GetFigureOrientationsNoValueMultiplier()
        {
            List<int[,]> newList = new List<int[,]>();

            foreach (int[,] figureShape in this.figureOrientations)
            {
                newList.Add((int[,])figureShape.Clone());
            }

            return newList;
        }

        /// <summary>
        /// Get all figure orientations where 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int[,]> GetFigureOrientationsWithValueMultiplier()
        {
            List<int[,]> newList = new List<int[,]>();

            foreach (int[,] figureShape in this.figureOrientations)
            {
                int[,] copyList = (int[,])figureShape.Clone();
                for (int i = 0; i < copyList.GetLength(0); i++)
                {
                    for (int j = 0; j < copyList.GetLength(1); j++)
                    {
                        copyList[i, j] *= this.valueMultiplier;
                    }
                }

                newList.Add(copyList);
            }

            return newList;
        }



        private int[,] RotateRight(int[,] oldBoard)
        {
            int oldColumnCount = oldBoard.GetLength(1);
            int oldRowCount = oldBoard.GetLength(0);

            // Due to rotation, what was previosly column count in a matrix 
            // is now row count and vice-versa.
            int newColumnCount = oldRowCount;
            int newRowCount = oldColumnCount;

            // initialize new matrix
            int[,] newBoard = new int[newRowCount, newColumnCount];

            // initialize new matrix
            for (int column = oldColumnCount - 1; column >= 0; column--)
            {
                for (int row = 0; row < oldRowCount; row++)
                {
                    newBoard[oldColumnCount - 1 - column, row] = oldBoard[row, column];
                }
            }

            return newBoard;
        }

        private int[,] FlipOverXAxis(int[,] oldBoard)
        {
            int rowCount = oldBoard.GetLength(0);
            int columnCount = oldBoard.GetLength(1);

            // initialize new matrix
            int[,] newBoard = new int[rowCount, columnCount];

            // initialize new matrix
            for (int column = 0; column < columnCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    newBoard[rowCount - 1 - row, column] = oldBoard[row, column];
                }
            }

            return newBoard;
        }

        private int GetNumberOfRotations(FigureTransformation figureTransformation)
        {
            if (figureTransformation == FigureTransformation.NoTransformation)
            {
                return 1;
            }

            if (figureTransformation == FigureTransformation.TwoRotations
                || figureTransformation == FigureTransformation.TwoRotationsAndReflection)
            {
                return 2;
            }

            if (figureTransformation == FigureTransformation.FourRotations
                || figureTransformation == FigureTransformation.FourRotationsAndReflection)
            {
                return 4;
            }

            throw new ArgumentOutOfRangeException("Enum FigureTransformation value out of bounds");
        }
    }

    public enum FigureTransformation : int
    {
        NoTransformation = 1,
        TwoRotations = 2,
        TwoRotationsAndReflection = 3,
        FourRotations = 4,
        FourRotationsAndReflection = 5
    }
}
