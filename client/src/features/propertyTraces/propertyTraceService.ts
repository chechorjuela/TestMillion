import { notificationService } from '../../services/notificationService';
import type { ApiResponse, PropertyTrace, PropertyTraceFilters, CreatePropertyTraceRequestDto, UpdatePropertyTraceRequestDto } from '../../types';
import {BaseService} from "../../lib/api/BaseService.ts";

class PropertyTraceService extends BaseService {
  private static instance: PropertyTraceService;
  private constructor() {
    super();
  }

  public static getInstance(): PropertyTraceService {
    if (!PropertyTraceService.instance) {
      PropertyTraceService.instance = new PropertyTraceService();
    }
    return PropertyTraceService.instance;
  }

  async getPropertyTraces(filters?: PropertyTraceFilters): Promise<ApiResponse<PropertyTrace[]>> {
    try {
      const params = {
        pageNumber: filters?.PageNumber || 1,
        pageSize: filters?.PageSize || 10,
        name: filters?.Name,
        minValue: filters?.MinValue,
        maxValue: filters?.MaxValue,
        minTax: filters?.MinTax,
        maxTax: filters?.MaxTax,
        startDate: filters?.StartDate,
        endDate: filters?.EndDate
      };
      return await this.api.get<ApiResponse<PropertyTrace[]>>('/PropertiesTrace', { params });
      } catch (error) {
      throw this.handleError(error);
    }
  }

  async createPropertyTrace(propertyTrace: CreatePropertyTraceRequestDto): Promise<PropertyTrace> {
    try {
      // PropertyTrace data is already in the correct format
      const request = propertyTrace;
      const result = await this.api.post<ApiResponse<PropertyTrace>>('/PropertiesTrace', request);
        notificationService.success('Property trace created successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async updatePropertyTrace(id: string, propertyTrace: CreatePropertyTraceRequestDto): Promise<PropertyTrace> {
    try {
      const request: UpdatePropertyTraceRequestDto = {
        ...propertyTrace,
        id
      };
      const result = await this.api.put<ApiResponse<PropertyTrace>>(`/PropertiesTrace/${id}`, request);
        notificationService.success('Property trace updated successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async deletePropertyTrace(id: string): Promise<void> {
    try {
  await this.api.delete<void>(`/PropertiesTrace/${id}`);
        notificationService.success('Property trace deleted successfully');
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async getPropertyTraceById(id: string): Promise<PropertyTrace> {
    try {
  const response = await this.api.get<ApiResponse<PropertyTrace>>(`/PropertiesTrace/${id}`);
        return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async getPropertyTracesByProperty(propertyId: string): Promise<PropertyTrace[]> {
    try {
  const response = await this.api.get<ApiResponse<PropertyTrace[]>>(`/PropertiesTrace/property/${propertyId}`);
        return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }
}

export const propertyTraceService = PropertyTraceService.getInstance();