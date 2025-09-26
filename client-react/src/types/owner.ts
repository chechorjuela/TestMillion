export interface Owner {
  Id: string;
  Name: string;
  Address: string;
  Photo: string;
  Birthdate: string;
}

export interface OwnerFilters {
  Name?: string;
  Address?: string;
  StartBirthdate?: string;
  EndBirthdate?: string;
  PageNumber?: number;
  PageSize?: number;
}