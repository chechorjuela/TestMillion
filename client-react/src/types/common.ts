export interface BaseEntity {
  Id: string;
  CreatedAt?: Date;
  UpdatedAt?: Date;
}

export interface PaginationParams {
  PageNumber: number;
  PageSize: number;
}

export interface FilterParams extends PaginationParams {
  [key: string]: any;
}

export interface ApiResponse<T> {
  Data: T;
  Success: boolean;
  Message?: string;
  Metadata?: {
    TotalItems: number;
    TotalPages: number;
    CurrentPage: number;
    PageSize: number;
  };
}

export interface BaseFilters extends FilterParams {
  StartDate?: string;
  EndDate?: string;
  SortBy?: string;
  SortDirection?: 'asc' | 'desc';
}

export type ActionMode = 'create' | 'edit' | 'delete' | 'view';
export type EntityType = 'property' | 'owner' | 'propertyTrace' | 'propertyImage';