import type { ApiResponse, BaseEntity, BaseFilters } from '../types/common';
import { CachedApiClient } from '../lib/api/CachedApiClient';
import { notificationService } from './notificationService';

export abstract class BaseService<T extends BaseEntity, F extends BaseFilters> {
  protected api: CachedApiClient;
  
  constructor(protected endpoint: string) {
    this.api = CachedApiClient.getInstance();
  }

  async getAll(filters?: Partial<F>): Promise<ApiResponse<T[]>> {
    try {
      const params = {
        pageNumber: filters?.PageNumber || 1,
        pageSize: filters?.PageSize || 10,
        ...(filters && Object.entries(filters)
          .filter(([key]) => !['PageNumber', 'PageSize'].includes(key))
          .reduce((acc, [key, value]) => ({
            ...acc,
            [key.charAt(0).toLowerCase() + key.slice(1)]: value
          }), {}))
      };
      return await this.api.get<ApiResponse<T[]>>(this.endpoint, { params });
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async getById(id: string): Promise<T> {
    try {
      const response = await this.api.get<ApiResponse<T>>(`${this.endpoint}/${id}`);
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async create(data: Omit<T, 'Id' | 'CreatedAt' | 'UpdatedAt'>): Promise<T> {
    try {
      const formattedData = Object.entries(data).reduce((acc, [key, value]) => ({
        ...acc,
        [key.charAt(0).toUpperCase() + key.slice(1)]: value
      }), {});
      const response = await this.api.post<ApiResponse<T>>(this.endpoint, formattedData);
      notificationService.entitySuccess(this.getEntityType(), 'create');
      return response.Data;
    } catch (error) {
      notificationService.entityError(this.getEntityType(), 'create', error);
      this.handleError(error);
      throw error;
    }
  }

  async update(id: string, data: Partial<T>): Promise<T> {
    try {
      const formattedData = Object.entries(data).reduce((acc, [key, value]) => ({
        ...acc,
        [key.charAt(0).toUpperCase() + key.slice(1)]: value
      }), {});
      const response = await this.api.put<ApiResponse<T>>(`${this.endpoint}/${id}`, formattedData);
      notificationService.entitySuccess(this.getEntityType(), 'update');
      return response.Data;
    } catch (error) {
      notificationService.entityError(this.getEntityType(), 'update', error);
      this.handleError(error);
      throw error;
    }
  }

  async delete(id: string): Promise<void> {
    try {
      await this.api.delete(`${this.endpoint}/${id}`);
      notificationService.entitySuccess(this.getEntityType(), 'delete');
    } catch (error) {
      notificationService.entityError(this.getEntityType(), 'delete', error);
      this.handleError(error);
      throw error;
    }
  }

  protected getEntityType(): any {
    if (this.endpoint.includes('Owner')) return 'owner';
    if (this.endpoint.includes('PropertiesImages') || this.endpoint.includes('PropertyImages')) return 'propertyImage';
    if (this.endpoint.includes('PropertiesTrace') || this.endpoint.includes('PropertyTraces')) return 'propertyTrace';
    if (this.endpoint.includes('Properties') || this.endpoint.includes('Property')) return 'property';
    return 'entity';
  }

  protected handleError(error: unknown): never {
    if (error instanceof Error) {
      throw new Error(`Service error: ${error.message}`);
    }
    throw new Error('An unexpected error occurred');
  }
}