<template>
    <div v-if="board" class="grid-container">
        <div 
            v-for="(row, rowIndex) in board.boardState"
            :key="rowIndex"
            class="flex">
            <div 
                v-for="(cell, columnIndex) in row" 
                :key="columnIndex" 
                :class="`grid-item`"
                @dragenter="(event:any) => dragEnter(event)"
                @dragover="(event:any) => dragOver(event)"
                @drop="(event:any) => drop(event, rowIndex, columnIndex)">
                {{  }}
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type GameBoard = {
        boardState: number[][],
    };

    interface Data {
        loading: boolean,
        board: null | GameBoard
    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                board: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
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

                fetch('boardGame')
                    .then(async response => {
                        
                        if (!response.ok)
                        {
                            throw new Error(response.statusText)
                        }

                        const data:number[][] = await response.json();
                        let boardGame: GameBoard = {
                            boardState : data
                        };

                        console.log(data)

                        this.board = boardGame;
                        this.loading = false;
                        return;
                    })
            },
            dragEnter(ev:any) {
                ev.preventDefault();
            },
            dragOver(ev:any) {
                ev.preventDefault();
            },
            drop(ev:any, rowIndex: number, columnIndex: number) {
                ev.preventDefault();
                console.log("Row index:" + rowIndex+", Column index" + columnIndex)
            }   
        },
    });
</script>

<style scoped>
.grid-container {
    display: grid;
    place-items: center;
    grid-template-columns: auto auto auto auto auto auto;
    background-color: #2196F3;
    padding: 10px;
}

.grid-item {
    background-color: rgba(255, 255, 255, 0.8);
    border: 1px solid rgba(0, 0, 0, 0.8);
    padding: 20px;
    font-size: 30px;
    text-align: center;
}
</style>