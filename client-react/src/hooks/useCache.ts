import { CachedApiClient } from '../lib/api/CachedApiClient';

export const useCache = () => {
  const clearCache = () => {
    CachedApiClient.getInstance().clearCache();
  };

  return {
    clearCache,
  };
};