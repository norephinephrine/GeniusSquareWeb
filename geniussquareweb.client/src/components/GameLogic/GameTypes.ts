export type {FigureDataTransfer, Figure, Cell, GameData}
export  {FigureColors}

type FigureDataTransfer = {
    figureId: string,
    figure: Figure,
    selectedCellRowIndex: number,
    selectedCellColumnIndex: number
};

type Figure = {
    value: number,
    color: string,
    cellMatrix: Array<Array<number>>,
    placedCellIndexes: Array<[number, number]> | null,
    opacity: number
};

const FigureColors: string[] = ["lightgreen", "lightblue", "purple", "pink", "orange", "green", "red", "brown", "darkgrey"];

type Cell = {
    figureId: string,
    color: string
    value: number,
};

type GameData =
{
    gameGuid: string,
    board: Array<Array<number>>,
    enemyBoard: Array<Array<number>>
}