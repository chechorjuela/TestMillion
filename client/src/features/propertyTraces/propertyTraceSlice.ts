import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { propertyTraceService } from './propertyTraceService';
import type { PropertyTrace, PropertyTraceFilters } from '../../types';
import type { ApiResponse } from '../../types/api';

interface PropertyTracesState {
  propertyTraces: PropertyTrace[];
  loading: boolean;
  error: string | null;
  metadata: {
    TotalCount: number;
    TotalPages: number;
    CurrentPage: number;
    PageSize: number;
  } | null;
}

const initialState: PropertyTracesState = {
  propertyTraces: [],
  loading: false,
  error: null,
  metadata: null,
};

export const fetchPropertyTraces = createAsyncThunk(
  'propertyTraces/fetchPropertyTraces',
  async (filters?: PropertyTraceFilters) => {
    const response = await propertyTraceService.getPropertyTraces(filters);
    return response;
  }
);

const propertyTraceSlice = createSlice({
  name: 'propertyTraces',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchPropertyTraces.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchPropertyTraces.fulfilled, (state, action) => {
        state.loading = false;
        state.propertyTraces = action.payload.Data;
        state.metadata = {
          TotalCount: action.payload.TotalCount,
          TotalPages: action.payload.TotalPages,
          CurrentPage: action.payload.CurrentPage,
          PageSize: action.payload.PageSize,
        };
      })
      .addCase(fetchPropertyTraces.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch property traces';
      });
  },
});

export default propertyTraceSlice.reducer;