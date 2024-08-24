using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeniusSquareWeb.GameSolvers
{
    public static class DefaultFigures
    {
        public static Figure[] FigureList => new Figure[9]
        {
            DefaultFigures.Monoid,
            DefaultFigures.Domino,
            DefaultFigures.TrominoI,
            DefaultFigures.TrominoL,
            DefaultFigures.TetrominoI,
            DefaultFigures.TetrominoT,
            DefaultFigures.TetrominoL,
            DefaultFigures.TetrominoS,
            DefaultFigures.TetrominoSquare,
        };

        public static IEnumerable<int[,]>[] FigureListOrientations => new IEnumerable<int[,]>[]
        {
            DefaultFigures.Monoid.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.Domino.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TrominoI.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TrominoL.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoI.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoT.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoL.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoS.GetFigureOrientationsWithValueMultiplier(),
            DefaultFigures.TetrominoSquare.GetFigureOrientationsWithValueMultiplier(),
        };

        public readonly static Figure Monoid =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1 }
                    },
                figureName: nameof(Monoid),
                figureTransformation: FigureTransformation.NoTransformation);

        public static Figure Domino =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 }
                    },
                figureName: nameof(Domino),
                figureTransformation: FigureTransformation.TwoRotations);

        public static Figure TrominoL =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 },
                        { 0, 1 }
                    },
                figureName: nameof(TrominoL),
                figureTransformation: FigureTransformation.FourRotations);

        public static Figure TrominoI =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1 },
                    },
                figureName: nameof(TrominoI),
                figureTransformation: FigureTransformation.TwoRotations);


        public static Figure TetrominoSquare =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1,},
                        { 1, 1,},
                    },
                figureName: nameof(TetrominoSquare),
                figureTransformation: FigureTransformation.NoTransformation);

        public static Figure TetrominoL =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1},
                        { 0, 0, 1},
                    },
                figureName: nameof(TetrominoL),
                figureTransformation: FigureTransformation.FourRotationsAndReflection);


        public static Figure TetrominoS =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 0},
                        { 0, 1, 1},
                    },
                figureName: nameof(TetrominoS),
                figureTransformation: FigureTransformation.TwoRotationsAndReflection);

        public static Figure TetrominoT =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1},
                        { 0, 1, 0},
                    },
                figureName: nameof(TetrominoT),
                figureTransformation: FigureTransformation.FourRotations);

        public static Figure TetrominoI =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1, 1},
                    },
                figureName: nameof(TetrominoI),
                figureTransformation: FigureTransformation.TwoRotations);
    }
}
