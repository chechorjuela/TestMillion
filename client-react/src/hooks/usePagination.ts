import { useState, useCallback, useEffect } from 'react';
import type { BaseFilters, PaginationParams } from '../types/common';

interface UsePaginationProps<T, F extends BaseFilters> {
  fetchData: (params: F) => Promise<any>;
  initialFilters?: Partial<F>;
  defaultPageSize?: number;
}

export const usePagination = <T, F extends BaseFilters>({
  fetchData,
  initialFilters = {},
  defaultPageSize = 10
}: UsePaginationProps<T, F>) => {
  const [pageSize, setPageSize] = useState(defaultPageSize);
  const [currentPage, setCurrentPage] = useState(1);
  const [currentFilters, setCurrentFilters] = useState<F>({ 
    ...initialFilters as F,
    PageNumber: 1,
    PageSize: defaultPageSize
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadData = useCallback(async (params: F) => {
    setIsLoading(true);
    setError(null);
    try {
      await fetchData(params);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An error occurred');
      console.error('Error fetching data:', err);
    } finally {
      setIsLoading(false);
    }
  }, [fetchData]);

  const handlePageChange = useCallback(async (page: number) => {
    if (page === currentPage) return;
    
    setCurrentPage(page);
    const newParams = {
      ...currentFilters,
      PageNumber: page,
    } as F;
    setCurrentFilters(newParams);
    await loadData(newParams);
  }, [currentFilters, currentPage, loadData]);

  const handleItemsPerPageChange = useCallback(async (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
    const newParams = {
      ...currentFilters,
      PageNumber: 1,
      PageSize: newPageSize,
    } as F;
    setCurrentFilters(newParams);
    await loadData(newParams);
  }, [currentFilters, loadData]);

  const handleFilterChange = useCallback(async (filters: Partial<F>) => {
    const newParams = {
      ...currentFilters,
      ...filters,
      PageNumber: 1,
    } as F;
    setCurrentFilters(newParams);
    setCurrentPage(1);
    await loadData(newParams);
  }, [currentFilters, loadData]);

  useEffect(() => {
    loadData(currentFilters);
  }, []); // Solo cargar al montar

  return {
    pageSize,
    currentPage,
    currentFilters,
    isLoading,
    error,
    handlePageChange,
    handleItemsPerPageChange,
    handleFilterChange,
    setCurrentFilters,
    reload: () => loadData(currentFilters),
  };
};