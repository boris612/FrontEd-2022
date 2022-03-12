<template>  
  <Toast />
  <app-header />
  <router-view />
  <app-footer />
</template>

<script setup>
import AppHeader from './components/AppHeader.vue'
import AppFooter from './components/AppFooter.vue'
</script>

<style lang="scss">
@import "./App.scss";
</style>