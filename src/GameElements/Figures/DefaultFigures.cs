namespace GeniusSquareWeb.GameElements.Figures
{
    public static class DefaultFigures
    {
        public static Figure[] FigureList => new Figure[9]
        {
            Monomino,
            Domino,
            TrominoL,
            TrominoI,
            TetrominoI,
            TetrominoT,
            TetrominoL,
            TetrominoS,
            TetrominoSquare,
        }
        .OrderBy(figure => figure.Value)
        .ToArray();

        public static IEnumerable<int[,]>[] FigureListOrientations => new IEnumerable<int[,]>[]
        {
            Monomino.GetFigureOrientationsWithValueMultiplier(),
            Domino.GetFigureOrientationsWithValueMultiplier(),
            TrominoI.GetFigureOrientationsWithValueMultiplier(),
            TrominoL.GetFigureOrientationsWithValueMultiplier(),
            TetrominoI.GetFigureOrientationsWithValueMultiplier(),
            TetrominoT.GetFigureOrientationsWithValueMultiplier(),
            TetrominoL.GetFigureOrientationsWithValueMultiplier(),
            TetrominoS.GetFigureOrientationsWithValueMultiplier(),
            TetrominoSquare.GetFigureOrientationsWithValueMultiplier(),
        };

        public readonly static Figure Monomino =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1 }
                    },
                figureValue: 1,
                figureTransformation: FigureTransformation.NoTransformation);

        public static Figure Domino =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 }
                    },
                figureValue: 2,
                figureTransformation: FigureTransformation.TwoRotations);

        public static Figure TrominoL =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 },
                        { 0, 1 }
                    },
                figureValue: 3,
                figureTransformation: FigureTransformation.FourRotations);

        public static Figure TrominoI =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1 },
                    },
                figureValue: 4,
                figureTransformation: FigureTransformation.TwoRotations);


        public static Figure TetrominoSquare =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1,},
                        { 1, 1,},
                    },
                figureValue: 5,
                figureTransformation: FigureTransformation.NoTransformation);

        public static Figure TetrominoL =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1},
                        { 0, 0, 1},
                    },
                figureValue: 6,
                figureTransformation: FigureTransformation.FourRotationsAndReflection);


        public static Figure TetrominoS =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 0},
                        { 0, 1, 1},
                    },
                figureValue: 7,
                figureTransformation: FigureTransformation.TwoRotationsAndReflection);

        public static Figure TetrominoT =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1},
                        { 0, 1, 0},
                    },
                figureValue: 8,
                figureTransformation: FigureTransformation.FourRotations);

        public static Figure TetrominoI =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1, 1},
                    },
                figureValue: 9,
                figureTransformation: FigureTransformation.TwoRotations);
    }
}
