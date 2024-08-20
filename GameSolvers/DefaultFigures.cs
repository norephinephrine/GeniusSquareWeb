using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameSolvers
{
    public static class DefaultFigures
    {
        public static IEnumerable<int[,]>[] FigureList => new IEnumerable<int[,]>[]
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
                figureTransformation: FigureTransformation.NoTransformation);

        public static Figure Domino =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 }
                    },
                figureTransformation: FigureTransformation.TwoRotations);

        public static Figure TrominoL =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 },
                        { 0, 1 }
                    },
                figureTransformation: FigureTransformation.FourRotations);

        public static Figure TrominoI =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1 },
                    },
                figureTransformation: FigureTransformation.TwoRotations);


        public static Figure TetrominoSquare =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1,},
                        { 1, 1,},
                    },
                figureTransformation: FigureTransformation.NoTransformation);

        public static Figure TetrominoL =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1},
                        { 0, 0, 1},
                    },
                figureTransformation: FigureTransformation.FourRotationsAndReflection);


        public static Figure TetrominoS =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 0},
                        { 0, 1, 1},
                    },
                figureTransformation: FigureTransformation.TwoRotationsAndReflection);

        public static Figure TetrominoT =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1},
                        { 0, 1, 0},
                    },
                figureTransformation: FigureTransformation.FourRotations);

        public static Figure TetrominoI =
            new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 1, 1},
                    },
                figureTransformation: FigureTransformation.TwoRotations);
    }
}
