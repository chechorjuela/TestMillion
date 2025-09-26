import { BaseService } from '../../services/BaseService';
import { API_ENDPOINTS } from '../../config/constants';
import type { PropertyImage, PropertyImageFilters } from '../../types';
import type { ApiResponse } from '../../types/common';

class PropertyImageService extends BaseService<PropertyImage, PropertyImageFilters> {
  private static instance: PropertyImageService;
  
  private constructor() {
    super(API_ENDPOINTS.PROPERTY_IMAGES);
  }

  public static getInstance(): PropertyImageService {
    if (!PropertyImageService.instance) {
      PropertyImageService.instance = new PropertyImageService();
    }
    return PropertyImageService.instance;
  }

  // Alias para mantener compatibilidad con el código existente
  async getPropertyImages(filters?: PropertyImageFilters): Promise<ApiResponse<PropertyImage[]>> {
    return this.getAll(filters);
  }

  async getPropertyImageById(id: string): Promise<PropertyImage> {
    return this.getById(id);
  }

  async createPropertyImage(propertyImage: Partial<PropertyImage>): Promise<PropertyImage> {
    const formattedData = {
      File: propertyImage.FileUrl || propertyImage['File'],
      Enabled: propertyImage.Enabled ?? true,
      IdProperty: propertyImage.Property?.Id || propertyImage['IdProperty'],
      Name: propertyImage['Name'] || 'Image ' + new Date().toISOString(),
      Description: propertyImage['Description'] || 'Property image description',
      ImagePath: propertyImage['ImagePath'] || propertyImage.FileUrl || propertyImage['File']
    };
    return this.create(formattedData);
  }

  async updatePropertyImage(id: string, propertyImage: Partial<PropertyImage>): Promise<PropertyImage> {
    return this.update(id, propertyImage);
  }

  async deletePropertyImage(id: string): Promise<void> {
    return this.delete(id);
  }

  // Métodos específicos para PropertyImages
  async getPropertyImagesByProperty(propertyId: string): Promise<PropertyImage[]> {
    try {
      const response = await this.api.get<ApiResponse<PropertyImage[]>>(
        `${this.endpoint}/property/${propertyId}`
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async toggleEnabled(id: string): Promise<PropertyImage> {
    try {
      const response = await this.api.post<ApiResponse<PropertyImage>>(
        `${this.endpoint}/${id}/toggle-enabled`
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async setAsMainImage(id: string, propertyId: string): Promise<void> {
    try {
      await this.api.post<ApiResponse<void>>(
        `${this.endpoint}/${id}/set-main`,
        { propertyId }
      );
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }
}

export const propertyImageService = PropertyImageService.getInstance();