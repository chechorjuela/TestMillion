import type {Property} from './property';

export interface PropertyImage {
  Id: string;
  FileUrl: string;
  Enabled: boolean;
  Property?: Property;
  Name: string;
  Description: string;
  ImagePath: string;
}

export interface CreatePropertyImageRequestDto {
  File: string;
  Enabled: boolean;
  IdProperty: string;
  Name: string;
  Description: string;
  ImagePath: string;
}

export interface UpdatePropertyImageRequestDto {
  id: string;
  FileUrl: string;
  Enabled: boolean;
  IdProperty: string;
  Name: string;
  Description: string;
  ImagePath: string;
}

export interface PropertyImageFilters {
  Enabled?: boolean;
  PageNumber?: number;
  PageSize?: number;
}