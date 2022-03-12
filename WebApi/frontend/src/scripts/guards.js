import {isAuthenticated} from '@/scripts/token-util.js'


export function checkLogin(to, from) {
    if (!isAuthenticated()) return {name: 'login'};
}