<script setup lang="ts">
import GameBoard from './components/GameBoard.vue';
import GeniusSquareFigures from './components/GeniusSquareFigures.vue';
import GeniusSquareFigures1 from './components/GeniusSquareFigures1.vue';
</script>

<template>
    <main class="wrapper">
        <GameBoard/>
        <hr/>
        <GeniusSquareFigures/>
        <hr/>
        <GeniusSquareFigures1/>
    </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

</style>
