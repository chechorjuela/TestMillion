import { BaseService } from '../../services/BaseService';
import { API_ENDPOINTS } from '../../config/constants';
import type { PropertyTrace, PropertyTraceFilters } from '../../types';
import type { ApiResponse } from '../../types/common';

class PropertyTraceService extends BaseService<PropertyTrace, PropertyTraceFilters> {
  private static instance: PropertyTraceService;
  
  private constructor() {
    super(API_ENDPOINTS.PROPERTY_TRACES);
  }

  public static getInstance(): PropertyTraceService {
    if (!PropertyTraceService.instance) {
      PropertyTraceService.instance = new PropertyTraceService();
    }
    return PropertyTraceService.instance;
  }

  async getPropertyTraces(filters?: PropertyTraceFilters): Promise<ApiResponse<PropertyTrace[]>> {
    return this.getAll(filters);
  }

  async getPropertyTraceById(id: string): Promise<PropertyTrace> {
    return this.getById(id);
  }

  async createPropertyTrace(propertyTrace: Partial<PropertyTrace>): Promise<PropertyTrace> {
    const formattedData = {
      ...propertyTrace,
      DateSale: propertyTrace.DateSale ? new Date(propertyTrace.DateSale).toISOString().split('T')[0] : undefined,
      Value: propertyTrace.Value ? Number(propertyTrace.Value) : undefined,
      Tax: propertyTrace.Tax ? Number(propertyTrace.Tax) : undefined,
    };
    return this.create(formattedData);
  }

  async updatePropertyTrace(id: string, propertyTrace: Partial<PropertyTrace>): Promise<PropertyTrace> {
    return this.update(id, propertyTrace);
  }

  async deletePropertyTrace(id: string): Promise<void> {
    return this.delete(id);
  }

  async getPropertyTracesByProperty(propertyId: string): Promise<PropertyTrace[]> {
    try {
      const response = await this.api.get<ApiResponse<PropertyTrace[]>>(
        `${this.endpoint}/property/${propertyId}`
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async getPropertyTracesByDateRange(startDate: string, endDate: string): Promise<PropertyTrace[]> {
    try {
      const response = await this.api.get<ApiResponse<PropertyTrace[]>>(
        `${this.endpoint}/date-range`,
        { params: { startDate, endDate } }
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async getPropertyTracesByValue(minValue: number, maxValue: number): Promise<PropertyTrace[]> {
    try {
      const response = await this.api.get<ApiResponse<PropertyTrace[]>>(
        `${this.endpoint}/value-range`,
        { params: { minValue, maxValue } }
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }
}

export const propertyTraceService = PropertyTraceService.getInstance();