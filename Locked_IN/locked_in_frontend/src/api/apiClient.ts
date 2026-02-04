import axios, { AxiosError } from 'axios';
import { persist } from '@/utils/auth/persistance';

export const API_BASE_URL = 'http://localhost:5122/api';
export const API_BASE_URL_HTTPS = 'https://localhost:7252/api';


export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const apiClientHttps = axios.create({
  baseURL: API_BASE_URL_HTTPS,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});

const setupRequestInterceptor = (client: typeof apiClient) => {
  client.interceptors.request.use(
    (config) => {
      const token = persist.getToken();
      if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );
};

const setupResponseInterceptor = (client: typeof apiClient) => {
  client.interceptors.response.use(
    (response) => {
      return response;
    },
    (error: AxiosError) => {
      if (error.response?.status === 401) {
        persist.clear();
        
        if (typeof window !== 'undefined' && window.location.pathname !== '/login') {
          window.location.href = '/login';
        }
      }
      return Promise.reject(error);
    }
  );
};

setupRequestInterceptor(apiClient);
setupResponseInterceptor(apiClient);

setupRequestInterceptor(apiClientHttps);
setupResponseInterceptor(apiClientHttps);
