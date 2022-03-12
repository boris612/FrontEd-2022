import axios from 'axios';
import Settings from '../settings.js'
import {storeTokens, getJwtToken, getRefreshToken} from '@/scripts/token-util.js'

export default class WebService {        
    lookup(what, params) {        
        const url = `${Settings.apiUrl}/lookup/${what}`;        
        return axios.get(url, {params : params}).then(res => res.data);
    }
    
    read(what, params) {                    
        if (params.sortOrder === undefined || params.sortOrder === null) {
            params.sortOrder = 1;
        }
        let url = `${Settings.apiUrl}/${what}?sortorder=${params.sortOrder}`;
       
        if (params.sortField !== undefined && params.sortField !== null) {
            url += `&sort=${params.sortField}`
        }                   
        if (params.first !== undefined) {
            url += `&first=${params.first}`
        }
        if (params.rows !== undefined) {
            url += `&rows=${params.rows}`
        }
        for(const property in params.filters) {             
            if (params.filters[property].value) {          
                const filter = `&filter=${property}(${params.filters[property].matchMode})${params.filters[property].value}`
                url += filter;
            }
        }        
        return axios(url).then(res => res.data);
        //return fetch(url).then(res => res.json());
    }

    create(what, item) {        
        const url = `${Settings.apiUrl}/${what}`;             
        return axios.post(url, item).then(res => res.data);       
    }

    update(what, id, item) {        
        const url = `${Settings.apiUrl}/${what}/${id}`;             
        return axios.put(url, item).then(res => res.data);       
    }

    delete(what, id) {        
        const url = `${Settings.apiUrl}/${what}/${id}`;             
        return axios.delete(url).then(res => res.data);       
    }

    login(username, password) {
        const url = `${Settings.apiUrl}/auth/create_token`;        
        return axios.post(url, {username, password}) 
                    .then(res => {
                        const tokens = res.data;
                        storeTokens(tokens);
                    })                    
    }  
    
    refreshToken() {        
        const model = { "token" : getJwtToken(), 
                        "refreshToken" : getRefreshToken()};
        const url = `${Settings.apiUrl}/auth/refresh_token`;        
        return axios.post(url, model) 
                    .then(res => {                        
                        if (res !== undefined) {
                            const tokens = res.data;
                            storeTokens(tokens);
                            return tokens.token;
                        }
                    });                                             
    }    
}
