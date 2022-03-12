<template>
  <Menubar :model="menus" :key="loginChange" />
</template>

<script>
import Settings from '../settings.js';

export default {    
    data() {
      return {
        loginChange : 0,
        menus : Settings.menus
      }
    },
    mounted() {      
      window.mitt.on('login-change', _ => ++this.loginChange);
    },
    beforeUnmount() {
      window.mitt.off('login-change');
    }  
}
</script>