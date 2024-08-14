export type {FigureDataTransfer, Figure, Cell, GameData}

type FigureDataTransfer = {
    figureId: string,
    figure: Figure,
    selectedCellRowIndex: number,
    selectedCellColumnIndex: number
};

type Figure = {
    color: string,
    cellMatrix: Array<Array<number>>,
    placedCellIndexes: Array<[number, number]> | null,
    opacity: number
};

type Cell = {
    figureId: string,
    color: string
    value: number,
};

type GameData =
{
    gameGuid: string,
    board: Array<Array<number>>
}