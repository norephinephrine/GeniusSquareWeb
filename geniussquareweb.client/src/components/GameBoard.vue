<template>
    <div class="container">
        <div v-for="(row, rowIndex) in boardStates"
            :key="rowIndex"
            class="row">
            <div v-for="(cell, columnIndex) in row" 
                :key="columnIndex" 
                :class="`cell`"
                :style="{ backgroundColor: cell.color }"
                @dragenter="(event:any) => dragEnter(event)"
                @dragover="(event:any) => dragOver(event)"
                @drop="(event:any) => drop(event, rowIndex, columnIndex)">
                <div v-if="cell.value === -1" class="circle"></div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import type { Cell, FigureDataTransfer } from './BoardFigures/FigureTypes';

    export default defineComponent({
        props: {
            boardStates: Array<Array<Cell>>
        },
        methods: {
            dragEnter(ev:any) {
                ev.preventDefault();
            },
            dragOver(ev:any) {
                ev.preventDefault();
            },
            drop(ev:any, rowIndex: number, columnIndex: number) {
                if (this.boardStates == null || ev.dataTransfer?.getData("figureData") === "")
                {
                    return
                }

                ev.preventDefault();
                let data:FigureDataTransfer = JSON.parse(ev.dataTransfer?.getData("figureData"));
                this.$emit("trySetFigureOnBoard", rowIndex, columnIndex, data);
            }
        },
    });
</script>

<style scoped>
.container{
    display: inline-block;
}

.row {
  display: flex;
}

.cell {
    width: 40px;  /* Set a fixed width */
    height: 40px; /* Set a fixed height */
    border: 1px solid rgba(0, 0, 0, 0.8);
    position: relative; /* Positioning for circle */
}

.circle {
    width: 30px;
    height: 30px;
    background-color: grey;
    border-radius: 50%;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}

.cell1 {
    background-color: red;
}

</style>