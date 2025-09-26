import { BaseService } from '../../services/BaseService';
import { API_ENDPOINTS } from '../../config/constants';
import type { Property, PropertyFilters } from '../../types';
import type { ApiResponse } from '../../types/common';

class PropertyService extends BaseService<Property, PropertyFilters> {
  private static instance: PropertyService;
  
  private constructor() {
    super(API_ENDPOINTS.PROPERTIES);
  }

  public static getInstance(): PropertyService {
    if (!PropertyService.instance) {
      PropertyService.instance = new PropertyService();
    }
    return PropertyService.instance;
  }

  async getProperties(filters?: PropertyFilters): Promise<ApiResponse<Property[]>> {
    return this.getAll(filters);
  }

  async getPropertyById(id: string): Promise<Property> {
    return this.getById(id);
  }

  async createProperty(property: Partial<Property>): Promise<Property> {
    return this.create(property);
  }

  async updateProperty(id: string, property: Partial<Property>): Promise<Property> {
    return this.update(id, property);
  }

  async deleteProperty(id: string): Promise<void> {
    return this.delete(id);
  }

  async getPropertiesByOwner(ownerId: string): Promise<Property[]> {
    try {
      const response = await this.api.get<ApiResponse<Property[]>>(
        `${this.endpoint}/owner/${ownerId}`
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  calculatePropertyValue(property: Property): number {
    const baseValue = property.Price;
    const yearFactor = new Date().getFullYear() - property.Year;
    return baseValue * (1 - yearFactor * 0.01); // Depreciación del 1% por año
  }
}

export const propertyService = PropertyService.getInstance();