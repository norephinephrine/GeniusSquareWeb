using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Dices;

namespace GameElementsTests
{
    /// <summary>
    /// RandomDice tests.
    /// </summary>
    [TestClass]
    public class RandomDiceTests
    {
        /// <summary>
        /// Should fail when Dice side array is null.
        /// </summary>
        [TestMethod]
        public void ShouldFailCreateWhenDiceSideArrayIsNull()
        {
            // when & then
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                RandomDice dice = new RandomDice(null);
            });
        }

        /// <summary>
        /// Should generate different random dice results.
        /// </summary>
        [TestMethod]
        public void ShouldGenerateDifferentDiceResults()
        {
            // given
            GameBoardField[] sides = [
                GameBoardField.A1,
                GameBoardField.A3,
                GameBoardField.A4,
                GameBoardField.C3,
                GameBoardField.C5,
                GameBoardField.F6];

            RandomDice dice = new(sides);

            int count = 5;
            bool differentRolls = false;

            // when
            GameBoardField initialRoll = dice.GenerateDiceResult();

            for (int i = 0; i < count; i++)
            {
                GameBoardField newRoll = dice.GenerateDiceResult();

                if (initialRoll != newRoll)
                {
                    differentRolls = true;
                    break;
                }
            }

            Assert.IsTrue(differentRolls);
        }

        /// <summary>
        /// Should get correct dice side count.
        /// </summary>
        [TestMethod]
        public void ShouldGetCorrectDiceSideCount()
        {
            // given
            GameBoardField[] sides = [
                GameBoardField.A1,
                GameBoardField.A3,
                GameBoardField.A4,
                GameBoardField.C3,
                GameBoardField.C5,
                GameBoardField.F6];

            // when
            RandomDice dice = new(sides);

            // then
            Assert.AreEqual(
                actual: dice.GetDiceSideCount(),
                expected: sides.Count());
        }

        /// <summary>
        /// Should generate same dice output when setting same random seed.
        /// </summary>
        [TestMethod]
        public void ShouldGenerateSameDiceOutputWhenSettingSameRandomSeed()
        {
            // given
            int n = 10;
            int seed = 123456;
            GameBoardField[] sides = [
                GameBoardField.A1,
                GameBoardField.A3,
                GameBoardField.A4,
                GameBoardField.C3,
                GameBoardField.C5,
                GameBoardField.F6];

            // when
            RandomDice dice = new(sides);

            // first dice throw
            RandomDice.SetRandomSeed(seed);
            List<GameBoardField> firstDiceThrows = new List<GameBoardField>();

            for (int i = 0; i < n; i ++)
            {
                firstDiceThrows.Add(dice.GenerateDiceResult());
            }

            // second dice throws
            RandomDice.SetRandomSeed(seed);
            List<GameBoardField> secondDiceThrows = new List<GameBoardField>();

            for (int i = 0; i < n; i++)
            {
                secondDiceThrows.Add(dice.GenerateDiceResult());
            }

            // then
            CollectionAssert.AreEquivalent(
                firstDiceThrows,
                secondDiceThrows);
        }
    }
}