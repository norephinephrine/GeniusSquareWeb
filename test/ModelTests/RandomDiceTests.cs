using GeniusSquareWeb.Models;

namespace ModelTests
{
    /// <summary>
    /// RandomDice tests.
    /// </summary>
    [TestClass]
    public class RandomDiceTests
    {
        /// <summary>
        /// Should generate different random dice sides.
        /// </summary>
        [TestMethod]
        public void ShouldGenerateDifferentRandomDiceSides()
        {
            // given
            DiceSide[] sides = [new("A1"), new("A2"), new("A3"), new("A4"), new("A5"), new("A6")];
            RandomDice dice = new(sides);

            int count = 5;
            bool differentRolls = false;

            // when
            DiceSide initialRoll = dice.GenerateDiceSide();

            for (int i = 0; i < count; i++)
            {
                DiceSide newRoll = dice.GenerateDiceSide();

                if (initialRoll != newRoll)
                {
                    differentRolls = true;
                    break;
                }
            }

            Assert.IsTrue(differentRolls);
        }
    }
}