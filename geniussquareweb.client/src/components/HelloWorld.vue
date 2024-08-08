<template>
    <div v-if="board" class="grid-container">
        <div 
            v-for="row in 6" 
            :key="row"
            class="flex">
            <div 
                v-for="(cell, y) in board.boardState.slice((row-1)*6,(row-1)*6 + 6)" 
                :key="y" 
                :class="`grid-item`">
                {{ cell }}
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type GameBoard = {
        boardState: number[],
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

                        const data:number[] = await response.json();
                        let boardGame: GameBoard = {
                            boardState : data
                        };

                        this.board = boardGame;
                        this.loading = false;
                        return;
                    })
            }
        },
    });
</script>

<style scoped>
.grid-container {
  display: grid;
  grid-template-columns: auto auto auto;
  background-color: #2196F3;
  padding: 10px;
  grid-template-columns: repeat(6, 1fr);
}

.grid-item {
  background-color: rgba(255, 255, 255, 0.8);
  border: 1px solid rgba(0, 0, 0, 0.8);
  padding: 20px;
  font-size: 30px;
  text-align: center;
}
</style>