<template>
    <div style="width:100%;">
        <div style="width:400px; float:left;">
        <span style="color: green;">Player board</span>
        <GameBoard v-if="board"
        :boardStates="board"
        :is-player="true"
        @try-set-figure-on-board="trySetFigureOnBoard"/>  

        </div>
        <div style="width:10%; float:right;" class="grid-container" tabindex="-1">
            <div class="gridItem" style="padding-bottom: 20px;">
                <span>Hold mouse click and drag figures</span>
                <hr/>
                <span>E: rotate figure right 90&deg</span>
                <span>Q: rotate figure left 90&deg</span>
                <span>D:  flip figure over X axis</span>
                <span>F:  flip figure over Y axis</span>
            </div>
            <div class="gridItem"></div>
            <div class="gridItem"></div>
            <div  v-for="(figure, figureKey) in figures"
            :key="figureKey"
            class="grid-item">
            <FigureTemplate 
            :figure="figure"
            :figureId="figureKey"
            @selected-figure="selectedFigure"
            @reset-figure="resetFigure"/>
            </div>
        </div>
    </div>
    <div  style="width:400px; z-index: 5000;" >
        <span style="color: red;">Enemy board</span>
            <GameBoard v-if="currentGame.enemyBoard"
            :is-droppable="false"
            :boardStates="createEnemyCellMatrix(currentGame.enemyBoard)"/> 
    </div>
</template>

<script lang="ts">
    import { defineComponent, type PropType } from 'vue';
    import  { FigureColors } from './GameTypes';
    import type { FigureDataTransfer, Figure, Cell, GameData } from './GameTypes';
    import GameBoard from './GameBoard.vue';
    import FigureTemplate from './FigureTemplate.vue';

    interface Data {
        board: null | Cell[][];
        enemyBoard: null | Cell[][];
        placedFigureCount: number
        figures: 
        {
            [key: string]: Figure
        }
    };

    export default defineComponent({
        components: {
            GameBoard,
            FigureTemplate
        },
        emits: ["winGame", "updateBoard"],
        props: {
            currentGame: {
                type: Object as PropType<GameData>,
                required: true,
            },
        },
        data(): Data {
            return {
                board: null,
                enemyBoard: null,
                placedFigureCount : 0,
                figures: {
                    monomino: {
                        value: 1,
                        color: FigureColors[0],
                        cellMatrix: [
                            [1],
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    domino: {
                        value: 2,
                        color: FigureColors[1],
                        cellMatrix: [
                            [1],
                            [1],  
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    trominoL: {
                        value: 3,
                        color: FigureColors[2],
                        cellMatrix: [
                            [1, 0],
                            [1, 1],  
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    trominoI: {
                        value: 4,
                        color: FigureColors[3],
                        cellMatrix: [
                            [1],
                            [1],
                            [1],
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoSquare: {
                        value: 5,
                        color: FigureColors[4],
                        cellMatrix: [
                            [1, 1],
                            [1, 1],  
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoL: {
                        value: 6,
                        color: FigureColors[5],
                        cellMatrix: [
                            [1, 0],
                            [1, 0],
                            [1, 1]    
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoZ: {
                        value: 7,
                        color: FigureColors[6],
                        cellMatrix: [
                            [0, 1, 1],
                            [1, 1, 0],    
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoT: {
                        value: 8,
                        color: FigureColors[7],
                        cellMatrix: [
                            [0, 1, 0],
                            [1, 1, 1],    
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoI: {
                        value: 9,
                        color: FigureColors[8],
                        cellMatrix: [
                            [1],
                            [1],
                            [1],
                            [1], 
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                }
            };
        },
        created() {
            this.board = this.createCellMatrix(this.currentGame.board);
        },
        methods: {
            createCellMatrix(numbers: number[][]): Cell[][] {
                return numbers.map(row => 
                    row.map(value => ({
                    figureId: "",
                    value: value,
                    color: "white"}))
                );
            },
            createEnemyCellMatrix(numbers: number[][]): Cell[][] {
                return numbers.map(row => 
                    row.map(value =>
                        {
                            if (value > 0)
                            {
                                return (
                                    {
                                    figureId: "",
                                    value: value,
                                    color: FigureColors[value-1] // enemy figure values start from 1 so this is needed
                                    })
                            }
                            return ({
                                figureId: "1",
                                value: value,
                                color: "white"
                            })
                        }
                    )
                );
            },
            async selectedFigure(id: string)
            {
                for (const key in this.figures) {
                    if (key == id)
                    {
                        this.figures[key].opacity = 0.6;
                    }
                    else
                    {
                        this.figures[key].opacity = 1
                    }
                }
            },
            resetFigure(id: string)
            {
                this.figures[id].opacity = 1;

                if(this.figures[id].placedCellIndexes === null || this.board == null)
                {
                    return;
                }

                for (let i = 0; i < this.figures[id].placedCellIndexes.length; i++) {
                    let [first, second] = this.figures[id].placedCellIndexes[i];
                    this.board[first][second].value = 0;
                    this.board[first][second].color = "white";
                    this.board[first][second].figureId = "";
                }

                this.figures[id].placedCellIndexes = null;
                this.$emit(
                    'updateBoard',
                    this.board.map(row => 
                        row.map(cell => cell.value)
                    ));
            },
            trySetFigureOnBoard(cellMatrixRowIndex: number, cellMatrixColumnIndex: number, data: FigureDataTransfer ) {
                if (this.board == null)
                {
                    return
                }

                let startingRowIndex = cellMatrixRowIndex - data.selectedCellRowIndex;
                let startingColumnIndex = cellMatrixColumnIndex - data.selectedCellColumnIndex;

                if (!this.isValidPlacement(this.board, startingRowIndex, startingColumnIndex, data))
                {
                    return
                }

                let rowCount = data.figure.cellMatrix.length;
                let columnCount = data.figure.cellMatrix[0].length;

                var newFigureCellPlacements = new Array();

                for (let i = startingRowIndex ; i < startingRowIndex+rowCount; i++ )
                {
                    for(let j = startingColumnIndex; j <startingColumnIndex+columnCount; j++)
                    {
                        if(data.figure.cellMatrix[i - startingRowIndex][j - startingColumnIndex] === 1)
                        {
                            this.board[i][j].value = data.figure.value;
                            this.board[i][j].color = data.figure.color;
                            this.board[i][j].figureId = data.figureId;
                            newFigureCellPlacements.push([i,j]);
                        }
                    }
                }
                this.figures[data.figureId].placedCellIndexes = newFigureCellPlacements;

                if (this.isGameWon())
                {
                    this.$emit(
                        'winGame',
                        this.board.map(row => 
                            row.map(cell => cell.value)
                        ));
                }
                else
                {
                    this.$emit(
                        'updateBoard',
                        this.board.map(row => 
                            row.map(cell => cell.value)
                        ));
                }

            },
            isValidPlacement(
                board: Cell[][],
                startingRowIndex: number,
                startingColumnIndex: number, 
                data: FigureDataTransfer) : boolean
            {
                if (this.figures[data.figureId].placedCellIndexes !== null)
                {
                    return false;
                }

                let rowCount = data.figure.cellMatrix.length;
                let columnCount = data.figure.cellMatrix[0].length;

                if (startingRowIndex <0 || startingRowIndex + rowCount > 6 || startingColumnIndex < 0 || startingColumnIndex + columnCount> 6)
                {
                    return false;
                }

                for (let i = startingRowIndex ; i < startingRowIndex+rowCount; i++ )
                {
                    for(let j = startingColumnIndex; j <startingColumnIndex+columnCount; j++)
                    {
                       if( board[i][j].value !== 0 && data.figure.cellMatrix[i - startingRowIndex][j - startingColumnIndex] ===1)
                       {
                        return false;
                       }
                    }
                }

                return true;
            },
            isGameWon() : boolean
            {
                for (const key in this.figures) {
                    if (this.figures[key].placedCellIndexes === null)
                    {
                        return false;
                    }
                }

                return true;
            },
        },
    });
</script>

<style scoped>
.grid-container {
  display: grid;
  grid-template-columns: auto auto auto;
}

.grid-item {
  z-index: 1;
  width: 200px;
  height: 200px;
}

span {
  font-weight: 800;
  display: block;
}
</style>