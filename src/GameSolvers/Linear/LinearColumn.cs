namespace GeniusSquareWeb.GameSolvers.Linear
{
    public struct LinearColumn
    {
        public int[] column;
        public IEnumerable<Tuple<int, int>> nonZeroCells;
        public int figureValue;

        public LinearColumn(
            int[] column,
            IEnumerable<Tuple<int, int>> nonZeroCells,
            int figureValue)
        {
            this.column = column;
            this.nonZeroCells = nonZeroCells;
            this.figureValue = figureValue;
        }
    }
}
