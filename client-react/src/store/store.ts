import { configureStore } from '@reduxjs/toolkit';
import propertyReducer from '../features/properties/propertySlice';
import ownerReducer from '../features/owners/ownerSlice';
import propertyTraceReducer from '../features/propertyTraces/propertyTraceSlice';
import propertyImageReducer from '../features/propertyImages/propertyImageSlice';

export const store = configureStore({
  reducer: {
    properties: propertyReducer,
    owners: ownerReducer,
    propertyTraces: propertyTraceReducer,
    propertyImages: propertyImageReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;