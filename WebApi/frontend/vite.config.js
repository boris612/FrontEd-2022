import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve : {
    alias: [
      {find: "@", replacement: resolve(__dirname, 'src')}
    ],
    //or alias: {"@" : resolve(__dirname, 'src')}        
  },
  server: {
    port: 3001
  }  
})