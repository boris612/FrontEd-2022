import {createRouter, createWebHistory} from 'vue-router'

import HomePage from "../pages/HomePage.vue";
import HelloWorld from "../components/HelloWorld.vue";
import Towns from "../pages/Towns.vue"
import Users from "../pages/Users.vue"
import Schools from "../pages/Schools.vue"
import Students from "../pages/Students.vue"
import Login from "../pages/Login.vue"
import Workshop from "../pages/Workshop.vue"
import Workshops from "../pages/Workshops.vue"
import {checkLogin} from '@/scripts/guards.js'

const routes = [
    { name: 'home', path: '/', component: HomePage },    
    { path: '/hello', component: HelloWorld },
    { path: '/towns', component: Towns, beforeEnter : checkLogin },
    { path: '/schools', component: Schools, beforeEnter : checkLogin },
    { path: '/students', component: Students, beforeEnter : checkLogin },
    { path: '/users', component: Users, beforeEnter : checkLogin },
    { path: '/workshops', component: Workshops, beforeEnter : checkLogin },
    { name: 'workshop-details', path: '/workshop/:id', component : Workshop, beforeEnter : checkLogin},
    { name: 'login', path: '/login', component: Login }     
  ]

const router = createRouter( {
   history: createWebHistory(),
   routes
})

export default router;