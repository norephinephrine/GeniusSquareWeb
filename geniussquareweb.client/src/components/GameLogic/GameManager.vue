<template>
    <div style="width:100%;">
        <div style="width:400px; float:left;">
        <GameBoard v-if="board"
        :boardStates="board"
        @try-set-figure-on-board="trySetFigureOnBoard"
        />  

        </div>
        <div style="width:10%; float:right;" class="grid-container" tabindex="-1">
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
    <div 
        ref="win-modal"
        class="modal">

        <div class="modal-content">
            <span class="close" @click="closeWinModal">&times;</span>
            <h1 v-if="loading == false"  style="background-color: green; color: white;">
                YOU WON
            </h1>
            <h1 v-else  style="background-color: blue; color: white;" >
                Waiting for server...
            </h1>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import type { FigureDataTransfer, Figure, Cell } from './FigureTypes';
    import GameBoard from './GameBoard.vue';
    import FigureTemplate from './BoardFigures/FigureTemplate.vue';
    import * as signalR from "@microsoft/signalr";

    interface Data {
        connection: signalR.HubConnection | null,
        loading: boolean;
        board: null | Cell[][];
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
        data(): Data {
            return {
                connection: null,
                loading: false,
                board: null,
                placedFigureCount : 0,
                figures: {
                    tetrominoZ: {
                        color: 'red',
                        cellMatrix: [
                            [0, 1, 1],
                            [1, 1, 0],    
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoT: {
                        color: 'brown',
                        cellMatrix: [
                            [0, 1, 0],
                            [1, 1, 1],    
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoL: {
                        color: 'green',
                        cellMatrix: [
                            [1, 0],
                            [1, 0],
                            [1, 1]    
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoSquare: {
                        color: 'orange',
                        cellMatrix: [
                            [1, 1],
                            [1, 1],  
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    tetrominoI: {
                        color: 'darkgrey',
                        cellMatrix: [
                            [1],
                            [1],
                            [1],
                            [1], 
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    trominoI: {
                        color: 'purple',
                        cellMatrix: [
                            [1, 0],
                            [1, 1],  
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    trominoL: {
                        color: 'pink',
                        cellMatrix: [
                            [1],
                            [1],
                            [1],
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    domino: {
                        color: 'lightblue',
                        cellMatrix: [
                            [1],
                            [1],  
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    },
                    monomino: {
                        color: 'lightgreen',
                        cellMatrix: [
                            [1],
                        ],
                        opacity: 1,
                        placedCellIndexes: null,
                    }
                }
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.connection = new signalR.HubConnectionBuilder().withUrl("/gameHub")
                .configureLogging(signalR.LogLevel.Information)
                .build();

            // TODO CHANGE
            this.connection.on("ReceiveMessage", function (user, message) {
                console.log(user, message);
            });
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.board = null;
                this.loading = true;

                fetch('boardGame/initialBoard')
                    .then(async response => {
                        
                        if (!response.ok)
                        {
                            throw new Error(response.statusText)
                        }

                        let data:number[][] = await response.json();

                        this.board = this.createCellMatrix(data);
                        this.loading = false;
                        return;
                    })
            },
            createCellMatrix(numbers: number[][]): Cell[][] {
                return numbers.map(row => 
                    row.map(value => ({
                    figureId: "",
                    value: value,
                    color: "white",
                    }))
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

                this.figures[data.figureId].placedCellIndexes = new Array();

                for (let i = startingRowIndex ; i < startingRowIndex+rowCount; i++ )
                {
                    for(let j = startingColumnIndex; j <startingColumnIndex+columnCount; j++)
                    {
                        if(data.figure.cellMatrix[i - startingRowIndex][j - startingColumnIndex] === 1)
                        {
                            this.board[i][j].value = 1;
                            this.board[i][j].color = data.figure.color;
                            this.board[i][j].figureId = data.figureId;
                            this.figures[data.figureId].placedCellIndexes.push([i,j]);
                        }
                    }
                }

                if (this.isGameWon())
                {
                    this.loading = true;
                    this.$refs["win-modal"].style.display = 'block';

                    // TODO CHANGE
                    fetch('boardGame/validateGameWin')
                    .then(async response => {
                        
                        await response;
                        if (!response.ok)
                        {
                            throw new Error(response.statusText)
                        }
                        else
                        {
                            this.$refs["win-modal"].style.display = 'block';
                        }
                        this.loading = false;
                    })
                };

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
            closeWinModal(ev: Event)
            {
                this.$refs["win-modal"].style.display = 'none';
            }
        },
    });
</script>

<style scoped>
.modal {
  display: none;
  position: fixed;
  z-index: 5;
  padding-top: 100px;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgb(0,0,0); 
  background-color: rgba(0,0,0,0.4);
}

/* Modal Content */
.modal-content {
  background-color: lightgreen;
  margin: auto;
  padding: 20px;
  border: 1px solid #888;
  width: 20%;
  text-align: center;
}

.grid-container {
  display: grid;
  grid-template-columns: auto auto auto;
}

.grid-item {
  z-index: 1;
  width: 200px;
  height: 200px;
}

.close {
  color: red;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: darkred;
  text-decoration: none;
  cursor: pointer;
}
</style>