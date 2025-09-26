export const PAGE_SIZES = [10, 25, 50, 100] as const;

export const DATE_FORMAT = 'YYYY-MM-DD' as const;

export const API_ENDPOINTS = {
  OWNERS: '/Owner',
  PROPERTIES: '/Properties',
  PROPERTY_TRACES: '/PropertiesTrace',
  PROPERTY_IMAGES: '/PropertiesImages',
} as const;

export const TABLE_DEFAULTS = {
  PAGE_SIZE: 10,
  MIN_SEARCH_LENGTH: 3,
  DEBOUNCE_TIME: 300,
} as const;

export const MODAL_TITLES = {
  CREATE: 'Create',
  EDIT: 'Edit',
  DELETE: 'Delete',
  VIEW: 'View',
} as const;

export const ERROR_MESSAGES = {
  REQUIRED_FIELD: 'This field is required',
  INVALID_DATE: 'Please enter a valid date',
  INVALID_NUMBER: 'Please enter a valid number',
  INVALID_EMAIL: 'Please enter a valid email address',
  INVALID_URL: 'Please enter a valid URL',
  NETWORK_ERROR: 'Network error occurred. Please try again.',
  UNKNOWN_ERROR: 'An unknown error occurred. Please try again.',
} as const;

export const TOAST_DURATION = 3000;

export const IMAGE_DEFAULTS = {
  FALLBACK: 'https://via.placeholder.com/150',
  MAX_SIZE: 5 * 1024 * 1024, // 5MB
  ACCEPTED_TYPES: ['image/jpeg', 'image/png', 'image/gif'],
} as const;

export const THEME = {
  colors: {
    primary: '#3B82F6',
    secondary: '#6B7280',
    success: '#10B981',
    danger: '#EF4444',
    warning: '#F59E0B',
    info: '#3B82F6',
  },
  spacing: {
    xs: '0.25rem',
    sm: '0.5rem',
    md: '1rem',
    lg: '1.5rem',
    xl: '2rem',
  },
} as const;