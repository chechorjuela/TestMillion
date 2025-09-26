import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { ownerService } from './ownerService';
import type { Owner, OwnerFilters } from '../../types';

interface OwnersState {
  owners: Owner[];
  loading: boolean;
  error: string | null;
  metadata: {
    TotalCount: number;
    TotalPages: number;
    CurrentPage: number;
    PageSize: number;
  } | null;
}

const initialState: OwnersState = {
  owners: [],
  loading: false,
  error: null,
  metadata: null,
};

export const fetchOwners = createAsyncThunk(
  'owners/fetchOwners',
  async (filters?: OwnerFilters) => {
    const response = await ownerService.getOwners(filters);
    return response;
  }
);

const ownerSlice = createSlice({
  name: 'owners',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchOwners.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchOwners.fulfilled, (state, action) => {
        state.loading = false;
        state.owners = action.payload.Data;
        state.metadata = {
          TotalCount: action.payload.TotalCount,
          TotalPages: action.payload.TotalPages,
          CurrentPage: action.payload.CurrentPage,
          PageSize: action.payload.PageSize,
        };
      })
      .addCase(fetchOwners.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch owners';
      });
  },
});

export default ownerSlice.reducer;