import axios, {type AxiosInstance, type AxiosResponse } from 'axios';
import { cacheService } from '../cache/CacheService';

interface RequestConfig {
  useCache?: boolean;
  cacheTime?: number;
}

export class CachedApiClient {
  private static instance: CachedApiClient;
  private axiosInstance: AxiosInstance;
  private readonly DEFAULT_CACHE_TIME = 5 * 60 * 1000; // 5 minutos

  private constructor() {
    this.axiosInstance = axios.create({
      baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.axiosInstance.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response) {
          throw new Error(error.response.data.message || 'Server error');
        } else if (error.request) {
          throw new Error('Network error');
        } else {
          throw new Error('Request configuration error');
        }
      }
    );
  }

  public static getInstance(): CachedApiClient {
    if (!CachedApiClient.instance) {
      CachedApiClient.instance = new CachedApiClient();
    }
    return CachedApiClient.instance;
  }

  private generateCacheKey(url: string, params?: any): string {
    if (!params) {
      return url;
    }
    
    const sortedParams = Object.keys(params)
      .sort()
      .reduce((acc, key) => {
        acc[key] = params[key];
        return acc;
      }, {} as Record<string, any>);

    return `${url}:${JSON.stringify(sortedParams)}`;
  }

  async get<T>(
    url: string,
    params?: any,
    config: RequestConfig = { useCache: true }
  ): Promise<T> {
    const cacheKey = this.generateCacheKey(url, params);
    
    if (config.useCache) {
      const cachedData = cacheService.get<T>(cacheKey);
      if (cachedData) {
        return cachedData;
      }
    }

    const response: AxiosResponse<T> = await this.axiosInstance.get(url, { params });
    
    if (config.useCache) {
      cacheService.set(
        cacheKey,
        response.data,
        config.cacheTime || this.DEFAULT_CACHE_TIME
      );
    }

    return response.data;
  }

  async post<T>(url: string, data: any): Promise<T> {
    const response: AxiosResponse<T> = await this.axiosInstance.post(url, data);
    this.invalidateRelatedCache(url);
    return response.data;
  }

  async put<T>(url: string, data: any): Promise<T> {
    const response: AxiosResponse<T> = await this.axiosInstance.put(url, data);
    // Invalidar caché relacionada
    this.invalidateRelatedCache(url);
    return response.data;
  }

  async delete<T>(url: string): Promise<T> {
    const response: AxiosResponse<T> = await this.axiosInstance.delete(url);
    // Invalidar caché relacionada
    this.invalidateRelatedCache(url);
    return response.data;
  }

  private invalidateRelatedCache(url: string) {
    // Por ahora limpiamos toda la caché cuando se modifica algo
    cacheService.clear();
  }

  // Método para limpiar la caché manualmente
  clearCache(): void {
    cacheService.clear();
  }
}