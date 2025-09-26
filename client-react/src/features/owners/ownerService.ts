import { BaseService } from '../../services/BaseService';
import { API_ENDPOINTS } from '../../config/constants';
import type { Owner, OwnerFilters } from '../../types';
import type { ApiResponse } from '../../types/api';

class OwnerService extends BaseService<Owner, OwnerFilters> {
  private static instance: OwnerService;
  private constructor() {
    super(API_ENDPOINTS.OWNERS);
  }

  public static getInstance(): OwnerService {
    if (!OwnerService.instance) {
      OwnerService.instance = new OwnerService();
    }
    return OwnerService.instance;
  }

  async getOwners(filters?: OwnerFilters): Promise<ApiResponse<Owner[]>> {
    return this.getAll(filters);
  }

  async getOwnerById(id: string): Promise<Owner> {
    return this.getById(id);
  }

  async createOwner(owner: Partial<Owner>): Promise<Owner> {
    const formatted = {
      ...owner,
      Birthdate: owner.Birthdate ? owner.Birthdate.split('T')[0] : undefined,
    };
    
    Object.keys(formatted).forEach(key => {
      if (formatted[key] === undefined || formatted[key] === null || formatted[key] === '') {
        delete formatted[key];
      }
    });

    return this.create(formatted);
  }

  async updateOwner(id: string, owner: Partial<Owner>): Promise<Owner> {
    const formatted = {
      Id: id,
      ...owner,
      Birthdate: owner.Birthdate ? owner.Birthdate.split('T')[0] : undefined,
    };

    Object.keys(formatted).forEach(key => {
      if (formatted[key] === undefined || formatted[key] === null || formatted[key] === '') {
        delete formatted[key];
      }
    });

    return this.update(id, formatted);
  }

  async deleteOwner(id: string): Promise<void> {
    return this.delete(id);
  }

  async getOwnersByBirthDateRange(startDate: string, endDate: string): Promise<Owner[]> {
    try {
      const response = await this.api.get<ApiResponse<Owner[]>>(
        `${this.endpoint}/birthdate-range`,
        { params: { startDate, endDate } }
      );
      return response.Data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  async getOwnerDetails(id: string): Promise<Owner> {
    try {
      const response = await this.api.get<ApiResponse<Owner>>(`${this.endpoint}/${id}`);
      return response.Data;
    } catch (error) {
      throw this.handleError(error);
    }
  }
}

export const ownerService = OwnerService.getInstance();