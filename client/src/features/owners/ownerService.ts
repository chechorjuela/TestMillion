import { BaseService } from '@/lib/api/BaseService';
import { notificationService } from '../../services/notificationService';
import type { Owner, OwnerFilters, CreateOwnerRequestDto, UpdateOwnerRequestDto } from '../../types';
import type { ApiResponse } from '../../types/api';

class OwnerService extends BaseService {
  private static instance: OwnerService;
  private constructor() {
    super();
  }

  public static getInstance(): OwnerService {
    if (!OwnerService.instance) {
      OwnerService.instance = new OwnerService();
    }
    return OwnerService.instance;
  }

  async getOwners(filters?: OwnerFilters): Promise<ApiResponse<Owner[]>> {
    try {
      const params = {
        pageNumber: filters?.PageNumber || 1,
        pageSize: filters?.PageSize || 10,
        name: filters?.Name,
        address: filters?.Address,
        startBirthdate: filters?.StartBirthdate,
        endBirthdate: filters?.EndBirthdate
      };
      return await this.api.get<ApiResponse<Owner[]>>('/Owner', { params });
      } catch (error) {
      throw this.handleError(error);
    }
  }

  async createOwner(owner: CreateOwnerRequestDto): Promise<Owner> {
    try {
      // Owner data is already in the correct format
      const request = owner;
      const result = await this.api.post<ApiResponse<Owner>>('/Owner', request);
        notificationService.success('Owner created successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async updateOwner(id: string, owner: CreateOwnerRequestDto): Promise<Owner> {
    try {
      const request: UpdateOwnerRequestDto = {
        ...owner,
        id
      };
      const result = await this.api.put<ApiResponse<Owner>>(`/Owner/${id}`, request);
        notificationService.success('Owner updated successfully');
      return result.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async deleteOwner(id: string): Promise<void> {
    try {
  await this.api.delete<void>(`/Owner/${id}`);
        notificationService.success('Owner deleted successfully');
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async getOwnerById(id: string): Promise<Owner> {
    try {
  const response = await this.api.get<ApiResponse<Owner>>(`/Owner/${id}`);
        return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }
}

export const ownerService = OwnerService.getInstance();