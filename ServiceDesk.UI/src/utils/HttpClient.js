import axios from 'axios';

axios.interceptors.request.use(
    request => {
      const token = localStorage.getItem('token');
  
      if (token) {
        request.headers['Authorization'] = 'Bearer ' + JSON.parse(token);
      }
      
      return request;
    },
    error => { Promise.reject(error); }
);

export default axios;
