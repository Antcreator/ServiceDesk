export function getSessionToken() {
    return localStorage.getItem('token');
}

export function setSessionToken(token) {
    return localStorage.setItem('token', token);
}

export function getSessionUser() {
    const token = getSessionToken();
    const payload = token.split('.')[1];
    const user = JSON.parse(atob(payload));

    return user;
}
