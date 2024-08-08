namespace GeniusSquareWeb.Models
{
    /// <summary>
    /// Static class that holds default dices for Genius Square.
    /// </summary>
    public static class DefaultDices
    {
        public static RandomDice Dice1 =
            new RandomDice([
                GameBoardField.D3,
                GameBoardField.D4,
                GameBoardField.C3,
                GameBoardField.C4,
                GameBoardField.B4,
                GameBoardField.E3]);

        public static RandomDice Dice2 =
            new RandomDice([
                GameBoardField.C5,
                GameBoardField.C6,
                GameBoardField.A4,
                GameBoardField.B5,
                GameBoardField.D6,
                GameBoardField.F6]);

        public static RandomDice Dice3 =
            new RandomDice([
                GameBoardField.E1,
                GameBoardField.F2,
                GameBoardField.F2,
                GameBoardField.A5,
                GameBoardField.A5,
                GameBoardField.B6]);

        public static RandomDice Dice4 =
            new RandomDice([
                GameBoardField.F4,
                GameBoardField.E4,
                GameBoardField.F5,
                GameBoardField.E6,
                GameBoardField.D5,
                GameBoardField.E5]);

        public static RandomDice Dice5 =
            new RandomDice([
                GameBoardField.A2,
                GameBoardField.C2,
                GameBoardField.B1,
                GameBoardField.B2,
                GameBoardField.A3,
                GameBoardField.B3]);

        public static RandomDice Dice6 =
            new RandomDice([
                GameBoardField.D1,
                GameBoardField.D2,
                GameBoardField.A1,
                GameBoardField.C1,
                GameBoardField.F3,
                GameBoardField.E2]);

        public static RandomDice Dice7 =
            new RandomDice([
                GameBoardField.E1,
                GameBoardField.A6,
                GameBoardField.F1,
                GameBoardField.A6,
                GameBoardField.A6,
                GameBoardField.F1]);

        /// <summary>
        /// Get all default dices.
        /// </summary>
        public static IEnumerable<RandomDice> GetAllDefaultDices =>
            new RandomDice[] {Dice1, Dice2, Dice3, Dice4, Dice5, Dice6, Dice7};
    }
}
