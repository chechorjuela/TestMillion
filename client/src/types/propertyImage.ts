import type {Property} from './property';

export interface PropertyImage {
  Id: string;
  File: string;
  Enabled: boolean;
  IdProperty: string;
  Property?: Property;
}

export interface CreatePropertyImageRequestDto {
  file: string;
  enabled: boolean;
  idProperty: string;
}

export interface UpdatePropertyImageRequestDto {
  id: string;
  file: string;
  enabled: boolean;
  idProperty: string;
}

export interface PropertyImageFilters {
  Enabled?: boolean;
  PageNumber?: number;
  PageSize?: number;
}