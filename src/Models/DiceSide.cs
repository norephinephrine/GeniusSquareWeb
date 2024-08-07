namespace GeniusSquareWeb.Models
{
    /// <summary>
    /// Class representing a dice side for Genius Square.
    /// </summary>
    public record class DiceSide
    {
        private const char minRowValue = 'A';
        private const char maxRowValue = 'F';

        private const int minColumnValue = 1;
        private const int maxColumnValue = 6;
        
        private int rowIndex;
        private int columnIndex;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="columnIndex">Column index</param>
        /// <exception cref="ArgumentException"></exception>
        public DiceSide(char rowIndex, int columnIndex)
        {
            if (rowIndex < minRowValue || rowIndex > maxRowValue)
            {
                throw new ArgumentException($"Invalid value for row index.Row index {rowIndex}");
            }

            if (columnIndex < minColumnValue || columnIndex > maxColumnValue)
            {
                throw new ArgumentException($"Invalid value for row index.Column index {columnIndex}");
            }

            this.rowIndex = rowIndex - minRowValue;
            this.columnIndex = columnIndex - minColumnValue;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="diceSideValue">Dice side value</param>
        /// <exception cref="ArgumentException"></exception>
        public DiceSide(string diceSideValue)
        {
            if (String.IsNullOrEmpty(diceSideValue))
            {
                throw new ArgumentException("Dice side value is null or empty");
            }

            if (diceSideValue.Length != 2)
            {
                throw new ArgumentException($"Invalid dice side length. Length:{diceSideValue.Length}");
            }

            char rowIndex = diceSideValue[0];
            int columnIndex = diceSideValue[1] - '0';

            if (rowIndex < minRowValue || rowIndex > maxRowValue)
            {
                throw new ArgumentException($"Invalid value for row index. Row index: {rowIndex}");
            }

            if (columnIndex < minColumnValue || columnIndex > maxColumnValue)
            {
                throw new ArgumentException($"Invalid value for row index. Column index: {columnIndex}");
            }

            this.rowIndex = rowIndex - minRowValue;
            this.columnIndex = columnIndex - minColumnValue;
        }

        /// <summary>
        /// Get row index.
        /// </summary>
        public int RowIndex => this.rowIndex;

        /// <summary>
        /// Get column index.
        /// </summary>
        public int ColumnIndex => this.columnIndex;
    }
}
