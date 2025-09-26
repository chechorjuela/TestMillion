import { CachedApiClient } from './CachedApiClient';
import { notificationService } from '../../services/notificationService';

export abstract class BaseService {
  protected api: CachedApiClient;

  constructor() {
    this.api = CachedApiClient.getInstance();
  }

  protected handleError(error: unknown): never {
    let errorMessage: string;

    if (error instanceof Error) {
      errorMessage = error.message;
    } else if (typeof error === 'string') {
      errorMessage = error;
    } else {
      errorMessage = 'An unexpected error occurred';
    }

    notificationService.error(errorMessage);
    throw new Error(errorMessage);
  }
}

export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface BaseFilter {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}