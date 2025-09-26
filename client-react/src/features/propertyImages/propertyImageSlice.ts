import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { propertyImageService } from './propertyImageService';
import type { PropertyImage, PropertyImageFilters } from '../../types';
import type { ApiResponse } from '../../types/api';

interface PropertyImagesState {
  propertyImages: PropertyImage[];
  loading: boolean;
  error: string | null;
  metadata: {
    TotalCount: number;
    TotalPages: number;
    CurrentPage: number;
    PageSize: number;
  } | null;
}

const initialState: PropertyImagesState = {
  propertyImages: [],
  loading: false,
  error: null,
  metadata: null,
};

export const fetchPropertyImages = createAsyncThunk(
  'propertyImages/fetchPropertyImages',
  async (filters?: PropertyImageFilters) => {
    const response = await propertyImageService.getPropertyImages(filters);
    return response;
  }
);

const propertyImageSlice = createSlice({
  name: 'propertyImages',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchPropertyImages.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchPropertyImages.fulfilled, (state, action) => {
        state.loading = false;
        state.propertyImages = action.payload.Data;
        state.metadata = {
          TotalCount: action.payload.TotalCount,
          TotalPages: action.payload.TotalPages,
          CurrentPage: action.payload.CurrentPage,
          PageSize: action.payload.PageSize,
        };
      })
      .addCase(fetchPropertyImages.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch property images';
      });
  },
});

export default propertyImageSlice.reducer;