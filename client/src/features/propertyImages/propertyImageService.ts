import { notificationService } from '../../services/notificationService';
import {BaseService} from "../../lib/api/BaseService.ts";
import type { ApiResponse, PropertyImage, PropertyImageFilters, CreatePropertyImageRequestDto, UpdatePropertyImageRequestDto } from '../../types';

class PropertyImageService extends BaseService {
  private static instance: PropertyImageService;
  private constructor() {
    super();
  }

  public static getInstance(): PropertyImageService {
    if (!PropertyImageService.instance) {
      PropertyImageService.instance = new PropertyImageService();
    }
    return PropertyImageService.instance;
  }

  async getPropertyImages(filters?: PropertyImageFilters): Promise<ApiResponse<PropertyImage[]>> {
    try {
      const params = {
        pageNumber: filters?.PageNumber || 1,
        pageSize: filters?.PageSize || 10,
        enabled: filters?.Enabled
      };
      return await this.api.get<ApiResponse<PropertyImage[]>>('/PropertiesImages', { params });
      } catch (error) {
      throw this.handleError(error);
    }
  }

  async createPropertyImage(propertyImage: CreatePropertyImageRequestDto): Promise<PropertyImage> {
    try {
      const result = await this.api.post<ApiResponse<PropertyImage>>('/PropertiesImages', propertyImage);
      notificationService.success('Property image created successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async updatePropertyImage(id: string, propertyImage: CreatePropertyImageRequestDto): Promise<PropertyImage> {
    try {
      const request: UpdatePropertyImageRequestDto = {
        ...propertyImage,
        id
      };
      const result = await this.api.put<ApiResponse<PropertyImage>>(`/PropertiesImages/${id}`, request);
        notificationService.success('Property image updated successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async deletePropertyImage(id: string): Promise<void> {
    try {
  await this.api.delete<void>(`/PropertiesImages/${id}`);
        notificationService.success('Property image deleted successfully');
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async getPropertyImagesByProperty(propertyId: string): Promise<PropertyImage[]> {
    try {
  const response = await this.api.get<ApiResponse<PropertyImage[]>>(`/PropertiesImages/property/${propertyId}`);
        return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async getPropertyImageById(id: string): Promise<PropertyImage> {
    try {
      const response = await this.api.get<ApiResponse<PropertyImage>>(`/PropertiesImages/${id}`);
      return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }
}

export const propertyImageService = PropertyImageService.getInstance();