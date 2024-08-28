using GeniusSquareWeb.GameElements;

namespace GameSolversTests
{
    [TestClass]
    public class FigureTests
    {
        /// <summary>
        /// Should properly rotate a figure 2 times.
        /// </summary>
        [TestMethod]
        public void ShouldSucceedRotation2Times()
        {
            // given
            Figure domino = new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 }
                    },
                figureName: "test",
                figureTransformation: FigureTransformation.TwoRotations);

            // when and then
            IEnumerable<int[,]> figureOrientations = domino.GetFigureOrientationsWithValueMultiplier();
            Assert.AreEqual(2, figureOrientations.Count());
            Assert.AreEqual(1, figureOrientations.ElementAt(0).GetLength(0));
            Assert.AreEqual(2, figureOrientations.ElementAt(0).GetLength(1));

            Assert.AreEqual(2, figureOrientations.ElementAt(1).GetLength(0));
            Assert.AreEqual(1, figureOrientations.ElementAt(1).GetLength(1));
        }

        /// <summary>
        /// Should properly rotate 4 times.
        /// </summary>
        [TestMethod]
        public void ShouldSucceedRotation4Times()
        {
            // given
            Figure domino = new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1 },
                        { 1, 0 }
                    },
                figureName: "test",
                figureTransformation: FigureTransformation.FourRotations);

            // when and then
            IEnumerable<int[,]> figureOrientations = domino.GetFigureOrientationsWithValueMultiplier();
            Assert.AreEqual(4, figureOrientations.Count());

            Assert.AreEqual(0, figureOrientations.ElementAt(0)[1,1]);
            Assert.AreEqual(0, figureOrientations.ElementAt(1)[0, 1]);
            Assert.AreEqual(0, figureOrientations.ElementAt(2)[0, 0]);
            Assert.AreEqual(0, figureOrientations.ElementAt(3)[1, 0]);
        }

        /// <summary>
        /// Should properly rotate .
        /// </summary>
        [TestMethod]
        public void ShouldSucceedRotation2TimesWithReflection()
        {
            // given
            Figure domino = new Figure(
                figureShape:
                    new int[,] {
                        { 1, 1, 0 },
                        { 0, 1, 1 }
                    },
                figureName: "test",
                figureTransformation: FigureTransformation.TwoRotationsAndReflection);

            // when and then
            IEnumerable<int[,]> figureOrientations = domino.GetFigureOrientationsWithValueMultiplier();
            Assert.AreEqual(4, figureOrientations.Count());

            Assert.AreEqual(0, figureOrientations.ElementAt(0)[0, 2]);
            Assert.AreEqual(0, figureOrientations.ElementAt(0)[1, 0]);

            Assert.AreEqual(0, figureOrientations.ElementAt(1)[0, 0]);
            Assert.AreEqual(0, figureOrientations.ElementAt(1)[2, 1]);

            Assert.AreEqual(0, figureOrientations.ElementAt(2)[0, 0]);
            Assert.AreEqual(0, figureOrientations.ElementAt(2)[1, 2]);

            Assert.AreEqual(0, figureOrientations.ElementAt(3)[0, 1]);
            Assert.AreEqual(0, figureOrientations.ElementAt(3)[2, 0]);
        }
    }
}
