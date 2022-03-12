import axios from 'axios';
import router from '@/router';
import WebService from '@/services/WebService.js'
import {getJwtToken, getRefreshToken, logout} from '@/scripts/token-util.js'

export default 

axios.interceptors.request.use(
    request => {     
        if (!request.url.endsWith('/auth/create_token') &&  !request.url.endsWith('/auth/refresh_token')) {
            const token = getJwtToken();
            if (token) {
                request.headers['Authorization'] = `Bearer ${token}`;
            }        
        }
        return request;
    }
);

axios.interceptors.response.use(
    response => response,
    error => {        
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {     
            if (error.response.headers['token-expired'] === "true") {                          
                if (getJwtToken() && getRefreshToken()) {
                    const loginService = new WebService();
                    return loginService.refreshToken()
                                        .then((token) => {                                            
                                            if (token !== undefined) {
                                                const config = error.config;
                                                config.headers['Authorization'] = `Bearer ${token}`;                
                                                return axios.request(config);
                                            }
                                        })                                                 
                }              
            }
            else {                         
            // logout();
                router.push('login');                  
            }
        }   
        else {     
            return new Promise((resolve, reject) => {                
                reject(error);
            });   
        } 
    }
);