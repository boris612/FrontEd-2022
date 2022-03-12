import mitt from 'mitt'
window.mitt = window.mitt || new mitt()

const JWT_TOKEN = 'jwt';
const REFRESH_TOKEN = 'refreshToken';


export function storeTokens(tokens) {
    localStorage.setItem(JWT_TOKEN, tokens.token);
    localStorage.setItem(REFRESH_TOKEN, tokens.refreshToken);  
    window.mitt.emit('login-change');
}

export function getJwtToken() {
    return localStorage.getItem(JWT_TOKEN);
}

export function getRefreshToken() {
    return localStorage.getItem(REFRESH_TOKEN);
}

export function logout() {
    localStorage.removeItem(JWT_TOKEN);
    localStorage.removeItem(REFRESH_TOKEN); 
    window.mitt.emit('login-change');
}

export function isAuthenticated() {
    const token = getJwtToken();
    if (token) return true;
    else return false;
}