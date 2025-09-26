import type {Property} from './property';

export interface PropertyTrace {
  Id: string;
  DateSale: string;
  Name: string;
  Value: number;
  Tax: number;
  IdProperty: string;
  Property?: Property;
}

export interface CreatePropertyTraceRequestDto {
  name: string;
  dateSale: string;
  value: number;
  tax: number;
  idProperty: string;
}

export interface UpdatePropertyTraceRequestDto extends CreatePropertyTraceRequestDto {
  id: string;
}

export interface PropertyTraceFilters {
  Name?: string;
  MinValue?: number;
  MaxValue?: number;
  MinTax?: number;
  MaxTax?: number;
  StartDate?: string;
  EndDate?: string;
  PageNumber?: number;
  PageSize?: number;
}