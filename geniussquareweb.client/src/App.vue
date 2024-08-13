<script setup lang="ts">
import GameManager from './components/GameManager.vue';
</script>

<template>
    <main>
        <GameManager/>
    </main>
    <br/>
    <footer style="text-align: center;">
      <span>E: rotate figure right 90&deg</span>
      <span>Q: rotate figure left 90&deg</span>
      <span>D:  flip figure over X axis</span>
      <span>F:  flip figure over Y axis</span>
    </footer>
</template>

<style scoped>
footer {
  line-height: 1.5;
  text-align: center
}

span {
  font-weight: 800;
  display: block;
}


</style>
