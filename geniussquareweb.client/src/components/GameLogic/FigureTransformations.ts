export function rotateRight(oldMatrix:number[][]) : number[][]
{

  let oldColumnCount = oldMatrix[0].length;
  let oldRowCount = oldMatrix.length

  // Due to rotation, what was previosly column count in a matrix 
  // is now row count and vice-versa.
  let newColumnCount: number = oldRowCount;
  let newRowCount : number = oldColumnCount;

  // initialize new matrix
  let newMatrix: number[][]= new Array(newRowCount);
  for (let row = 0; row< newRowCount; row++ )
  {
    newMatrix[row] = new Array(newColumnCount)
  }


  // initialize new matrix
  for (let column = 0; column< oldColumnCount; column++ )
  {
      for (let row = oldRowCount -1; row>= 0; row-- )
      {
        newMatrix[column][oldRowCount - 1 - row] = oldMatrix[row][column];
      }
  }

  return newMatrix;
};

export function  rotateLeft(oldMatrix:number[][]) : number[][]
{
  let oldColumnCount = oldMatrix[0].length;
  let oldRowCount = oldMatrix.length

  // Due to rotation, what was previosly column count in a matrix 
  // is now row count and vice-versa.
  let newColumnCount: number = oldRowCount;
  let newRowCount : number = oldColumnCount;

  // initialize new matrix
  let newMatrix: number[][]= new Array(newRowCount);
  for (let row = 0; row< newRowCount; row++ )
  {
    newMatrix[row] = new Array(newColumnCount)
  }


  // initialize new matrix
  for (let column = oldColumnCount - 1; column >= 0; column-- )
  {
      for (let row = 0; row < oldRowCount; row ++ )
      {
        newMatrix[oldColumnCount - 1 - column][row] = oldMatrix[row][column];
      }
  }

  return newMatrix;
}

export function  flipOverYAxis(oldMatrix:number[][]) : number[][]
{
  let rowCount : number = oldMatrix.length;
  let columnCount: number = oldMatrix[0].length;

  // initialize new matrix
  let newMatrix: number[][]= new Array(rowCount);
  for (let row = 0; row< rowCount; row++ )
  {
    newMatrix[row] = new Array(columnCount)
  }

  // initialize new matrix
  for (let column = 0; column < columnCount; column++ )
  {
      for (let row = 0; row < rowCount; row ++ )
      {
        newMatrix[row][columnCount- 1 - column] = oldMatrix[row][column]
      }
  }

  return newMatrix;
}

export function  flipOverXAxis(oldMatrix:number[][]) : number[][]
{
  let rowCount : number = oldMatrix.length;
  let columnCount: number = oldMatrix[0].length;

  // initialize new matrix
  let newMatrix: number[][]= new Array(rowCount);
  for (let row = 0; row< rowCount; row++ )
  {
    newMatrix[row] = new Array(columnCount)
  }

  // initialize new matrix
  for (let column = 0; column < columnCount; column++ )
  {
      for (let row = 0; row < rowCount; row ++ )
      {
        newMatrix[rowCount - 1 - row][column] = oldMatrix[row][column]
      }
  }

  return newMatrix;
}