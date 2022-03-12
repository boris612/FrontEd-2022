import {isAuthenticated, logout} from '@/scripts/token-util.js';
import router from '@/router/index.js';

export const settings = Object.freeze({
    apiUrl : 'https://localhost:44393',
    pageSize: 5,
    menus : [
        {label: 'Home', icon: 'pi pi-fw pi-home', to: '/'},
        {label: 'Workshops', icon: 'pi pi-fw pi-android', to: '/workshops'},
        {label: 'Students', icon: 'pi pi-fw pi-users', to: '/students'},
        {label: 'Schools', icon: 'pi pi-fw pi-book', to: '/schools'},
        {label: 'Towns', icon: 'pi pi-fw pi-map', to: '/towns'},
        {label: 'Users', icon: 'pi pi-fw pi-users', to: '/users', visible: isAuthenticated},
        {label: 'Login', icon: 'pi pi-fw pi-sign-in', to: '/login', visible: () => !isAuthenticated() },
        {label: 'Logout', icon: 'pi pi-fw pi-sign-out',
                visible : isAuthenticated, 
                command:() => { 
                    logout();
                    router.push({name: 'home'}); 
                }},        
    ]
});

export default settings;