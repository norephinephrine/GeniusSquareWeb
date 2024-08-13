<template>
  <div v-if="figure.placedCellIndexes === null"
    :id="figureId" class="figure-template"
    draggable='true'
    @dragstart="dragStart"
    @keyup="transformFiguretOnKeyPress"
    tabindex="-1">
      <div
        v-for="(row, rowIndex) in figure.cellMatrix"
        :key="rowIndex"sdfsdf
        class="row">
          <div v-for="(cell, columnIndex) in row"
              :key="columnIndex"
              :class="cell===1 ? 'block' : 'white-block'"
              @mousedown="(event) => mouseDown(event, rowIndex, columnIndex)"
              :style="{ backgroundColor: cell === 1 ? figure.color : '', opacity: figure.opacity}">
          </div>
      </div>
  </div>
  <button v-else 
    @click="$emit('resetFigure', figureId)"
    class="button-reset"
    :style="{ 'border-color': figure.color }">
    Reset</button>
</template>

<script  lang="ts">
import type { PropType } from 'vue';
import type { FigureDataTransfer, Figure } from './FigureTypes';
import { defineComponent } from 'vue'
import { flipOverXAxis, flipOverYAxis, rotateLeft, rotateRight } from './FigureTransformations';
import { enableDragDropTouch } from "./drag-drop-touch.esm.min.js";

export default defineComponent({
  data() {
    return {
      selectedCellRowIndex : 0,
      selectedCellColumnIndex : 0,
    };
  },
  created() {
    enableDragDropTouch();
  },
  props: {
    figureId: {
      type: [String, Number],
      required: true,
    },
    figure: {
      type: Object as PropType<Figure>,
      required: true,
    },
  },
  methods: {
    mouseDown(event: Event, rowIndex: number, columnIndex: number)
    {
      this.selectedCellRowIndex = rowIndex;
      this.selectedCellColumnIndex = columnIndex;
      console.log(rowIndex+ " ", columnIndex)
      this.$emit('selectedFigure',this.figureId)
    },
    dragStart(event: DragEvent ) {      
      let data: FigureDataTransfer =
      { 
        figureId: this.figureId as string,
        figure: this.figure,
        selectedCellColumnIndex: this.selectedCellColumnIndex,
        selectedCellRowIndex: this.selectedCellRowIndex
      };

      event.dataTransfer?.setData("figureData", JSON.stringify(data));
    },
    transformFiguretOnKeyPress(event:KeyboardEvent)
      {
        if (event.key === 'e' || event.key === 'E') {
          this.figure.cellMatrix = rotateRight(this.figure.cellMatrix);
          return;
        }

        if (event.key === 'q' || event.key === 'Q') {
          this.figure.cellMatrix = rotateLeft(this.figure.cellMatrix);
          
          return;
        }
        if (event.key === 'f' || event.key === 'F') {
          this.figure.cellMatrix = flipOverYAxis(this.figure.cellMatrix);
          
          return;
        }

        if (event.key === 'd' || event.key === 'D') {
          this.figure.cellMatrix = flipOverXAxis(this.figure.cellMatrix);
          
          return;
        }
      }
    // decreaseOpacity(event:Event)
    // {
    //   this.isMouseDown = true;
    // },
    // increaseOpacity(event:Event)
    // {
    //   this.isMouseDown = false;
    // }
  },
});
</script>

<style scoped>
.figure-template {
    display: inline-block;
    outline: 0;
}

.row {
  display: flex;
}

.block {
    width: 40px;
    height: 40px;
    border: 1px solid rgba(0, 0, 0, 0.8);
}
.white-block {
  width: 40px;
  height: 40px;
    border: 1px solid white;
    background-color: 1px solid #ccc;
}

.invisible {
  visibility: hidden;
}

.button-reset {
  border: 5px solid black;
  height: 50px;
  width: 100px;
  font-size: 30px;
  margin-bottom: 10px;
  transition-duration: 0.4s;
  background-color: white;
}

.button-reset:hover {
  background-color: lightgrey;
}
</style>