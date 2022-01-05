<template>
  <div class="container mx-auto px-6 bg-yellow-50 h-full">
    <Header></Header>
    <div>
      <router-view />
    </div>
  </div>
</template>
<script lang="ts" setup>
import Header from "../components/Header.vue";
</script>
