import axios from 'axios';
import type { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';

export class ApiClient {
  private static instance: ApiClient;
  private axiosInstance: AxiosInstance;

  private constructor() {
    this.axiosInstance = axios.create({
      baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Request interceptor
    this.axiosInstance.interceptors.request.use(
      (config) => {
        console.log(`Making ${config.method?.toUpperCase()} request to:`, config.url);
        return config;
      },
      (error) => {
        console.error('Request error:', error);
        return Promise.reject(error);
      }
    );

    // Response interceptor
    this.axiosInstance.interceptors.response.use(
      (response) => {
        console.log('Response received:', response.status);
        return response;
      },
      (error) => {
        console.error('Response error:', error.response?.data || error.message);
        return Promise.reject(error);
      }
    );
  }

  public static getInstance(): ApiClient {
    if (!ApiClient.instance) {
      ApiClient.instance = new ApiClient();
    }
    return ApiClient.instance;
  }

  // Generic request method
  private async request<T>(config: AxiosRequestConfig): Promise<T> {
    try {
      const response: AxiosResponse<T> = await this.axiosInstance.request(config);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const message = error.response?.data?.message || error.message;
        throw new Error(message);
      }
      throw error;
    }
  }

  // GET request
  public async get<T>(url: string, params?: unknown): Promise<T> {
    return this.request<T>({
      method: 'GET',
      url,
      params,
    });
  }

  // POST request
  public async post<T>(url: string, data?: unknown): Promise<T> {
    return this.request<T>({
      method: 'POST',
      url,
      data,
    });
  }

  // PUT request
  public async put<T>(url: string, data?: unknown): Promise<T> {
    return this.request<T>({
      method: 'PUT',
      url,
      data,
    });
  }

  public async delete<T>(url: string): Promise<T> {
    return this.request<T>({
      method: 'DELETE',
      url,
    });
  }

  public async patch<T>(url: string, data?: unknown): Promise<T> {
    return this.request<T>({
      method: 'PATCH',
      url,
      data,
    });
  }
}