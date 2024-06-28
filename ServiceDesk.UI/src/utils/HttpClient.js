import axios from 'axios';
import { getSessionToken } from './UserSession';

axios.interceptors.request.use(
  request => {
    const token = getSessionToken();

    if (token) {
      request.headers['Authorization'] = 'Bearer ' + token;
    }
  
    return request;
  },
  error => { Promise.reject(error); }
);

export default axios;
