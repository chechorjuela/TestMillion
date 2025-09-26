export interface ApiResponse<T> {
  Data: T;
  Status: number;
  Message: string;
  Metadata: {
    CurrentPage: number;
    TotalPages: number;
    PageSize: number;
    TotalCount: number;
    HasPrevious: boolean;
    HasNext: boolean;
  } | null;
}