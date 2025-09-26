import {createSlice, createAsyncThunk, type PayloadAction} from '@reduxjs/toolkit';
import type { Property, PropertyFilters } from '../../types';
import { propertyService } from './propertyService';

import type { ApiResponse } from '../../types/api';

interface PropertyState {
  properties: Property[];
  selectedProperty: Property | null;
  loading: boolean;
  error: string | null;
  metadata: ApiResponse<Property[]>['Metadata'];
}

const initialState: PropertyState = {
  properties: [],
  selectedProperty: null,
  loading: false,
  error: null,
  metadata: null,
};

export const fetchProperties = createAsyncThunk(
  'properties/fetchProperties',
  async (filters: PropertyFilters | undefined, { rejectWithValue }) => {
    try {
      return await propertyService.getProperties(filters);
    } catch (error) {
      return rejectWithValue('Failed to fetch properties');
    }
  }
);

export const fetchPropertyById = createAsyncThunk(
  'properties/fetchPropertyById',
  async (id: string, { rejectWithValue }) => {
    try {
      return await propertyService.getPropertyById(id);
    } catch (error) {
      return rejectWithValue('Failed to fetch property');
    }
  }
);

export const addProperty = createAsyncThunk(
  'properties/addProperty',
  async (propertyData: Partial<Property>, { rejectWithValue }) => {
    try {
      return await propertyService.createProperty(propertyData);
    } catch (error) {
      return rejectWithValue('Failed to create property');
    }
  }
);

const propertySlice = createSlice({
  name: 'properties',
  initialState,
  reducers: {
    clearSelectedProperty: (state) => {
      state.selectedProperty = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchProperties.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchProperties.fulfilled, (state, action: PayloadAction<ApiResponse<Property[]>>) => {
        state.loading = false;
        state.properties = [...action.payload.Data];
        state.metadata = { ...action.payload.Metadata };
        state.error = null;
      })
      .addCase(fetchProperties.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchPropertyById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchPropertyById.fulfilled, (state, action: PayloadAction<Property>) => {
        state.loading = false;
        state.selectedProperty = action.payload;
      })
      .addCase(fetchPropertyById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(addProperty.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(addProperty.fulfilled, (state, action: PayloadAction<Property>) => {
        state.loading = false;
        state.properties.push(action.payload);
      })
      .addCase(addProperty.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const { clearSelectedProperty } = propertySlice.actions;
export default propertySlice.reducer;