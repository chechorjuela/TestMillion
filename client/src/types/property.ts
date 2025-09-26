export interface Owner {
  Id: string;
  Name: string;
  Address: string;
  Photo: string;
  Birthdate: string;
}

export interface Property {
  Id: string;
  Name: string;
  Address: string;
  Price: number;
  CodeInternal: string;
  Year: number;
  IdOwner: string;
  Owner: Owner;
  MainImage: string | null;
}

export interface PropertyFilters {
  Name?: string;
  Address?: string;
  MinPrice?: number;
  MaxPrice?: number;
  PageNumber?: number;
  PageSize?: number;
}

export interface CreateOwnerRequestDto {
  name: string;
  address: string;
  photo: string;
  birthdate: string;
}

export interface UpdateOwnerRequestDto extends CreateOwnerRequestDto {
  id: string;
}

export interface OwnerFilters {
  Name?: string;
  Address?: string;
  StartBirthdate?: string;
  EndBirthdate?: string;
  PageNumber?: number;
  PageSize?: number;
}
