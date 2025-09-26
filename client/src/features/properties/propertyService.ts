import { BaseService } from '../../lib/api/BaseService';
import { notificationService } from '../../services/notificationService';
import type { Property, PropertyFilters } from '../../types';
import type { ApiResponse } from '../../types/api';

class PropertyService extends BaseService {
  private static instance: PropertyService;
  private constructor() {
    super();
  }

  public static getInstance(): PropertyService {
    if (!PropertyService.instance) {
      PropertyService.instance = new PropertyService();
    }
    return PropertyService.instance;
  }

  async getProperties(filters?: PropertyFilters): Promise<ApiResponse<Property[]>> {
    try {
      return await this.api.get<ApiResponse<Property[]>>('/properties', filters);
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async createProperty(property: Partial<Property>): Promise<Property> {
    try {
      const result = await this.api.post<ApiResponse<Property>>('/properties', property);
      notificationService.success('Property created successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async updateProperty(id: string, property: Partial<Property>): Promise<Property> {
    try {
      const result = await this.api.put<ApiResponse<Property>>(`/properties/${id}`, property);
      notificationService.success('Property updated successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async deleteProperty(id: string): Promise<void> {
    try {
      await this.api.delete<void>(`/properties/${id}`);
      notificationService.success('Property deleted successfully');
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async getPropertyById(id: string): Promise<Property> {
    try {
      const response = await this.api.get<ApiResponse<Property>>(`/properties/${id}`);
      return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }
}

export const propertyService = PropertyService.getInstance();