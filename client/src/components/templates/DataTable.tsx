import { motion } from 'framer-motion';
import React from 'react';
import { Pagination } from '../molecules/Pagination';

interface Column<T> {
  header: string;
  accessor: keyof T | ((item: T) => React.ReactNode);
  className?: string;
}

interface DataTableProps<T> {
  columns: Column<T>[];
  data?: T[];
  loading?: boolean;
  onRowClick?: (item: T) => void;
  keyExtractor: (item: T) => string;
  pagination: {
    currentPage: number;
    totalPages: number;
    onPageChange: (page: number) => void;
    itemsPerPage: number;
    onItemsPerPageChange: (itemsPerPage: number) => void;
    totalItems: number;
  };
}

export const DataTable = <T extends object>(props: DataTableProps<T>) => {
  const {
    columns,
    data = [],
    loading = false,
    onRowClick,
    keyExtractor,
    pagination
  } = props;
  if (loading) {
    return (
      <div className="p-8">
        <div className="flex justify-center">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
        </div>
      </div>
    );
  }

  if (data.length === 0) {
    return (
      <motion.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        className="text-center py-12 text-theme-medium-gray"
      >
        No data available
      </motion.div>
    );
  }

  return (
    <div>
      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-theme-silver/20">
          <thead className="bg-theme-silver/10">
            <tr>
              {columns.map((column, index) => (
                <th
                  key={index}
                  scope="col"
                  className={`px-6 py-3 text-left text-xs font-medium text-theme-medium-gray uppercase tracking-wider ${column.className || ''}`}
                >
                  {column.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-theme-silver/20">
            {data.map((item) => (
              <motion.tr
                key={keyExtractor(item)}
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                whileHover={{ backgroundColor: 'rgba(0,0,0,0.02)' }}
                onClick={() => onRowClick?.(item)}
                className={onRowClick ? 'cursor-pointer' : ''}
              >
                {columns.map((column, index) => (
                  <td
                    key={index}
                    className={`px-6 py-4 whitespace-nowrap text-sm text-theme-black ${column.className || ''}`}
                  >
                    {typeof column.accessor === 'function'
                      ? column.accessor(item)
                      : item[column.accessor]}
                  </td>
                ))}
              </motion.tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="sticky bottom-0 bg-theme-white py-4 flex items-center justify-between border-t border-theme-silver/20 px-4 rounded-b-lg">
        <div className="flex items-center gap-2">
          <select
            value={pagination.itemsPerPage}
            onChange={(e) => pagination.onItemsPerPageChange(Number(e.target.value))}
            className="rounded border border-theme-silver/30 bg-theme-white text-theme-black text-sm py-1 px-2 focus:outline-none focus:ring-2 focus:ring-theme-silver focus:border-theme-silver"
          >
            <option value={5}>5 items</option>
            <option value={10}>10 items</option>
            <option value={15}>15 items</option>
          </select>
          <span className="text-sm text-theme-medium-gray">
            {pagination.totalItems > 0 ? (
              <>Showing {Math.min(((pagination.currentPage - 1) * pagination.itemsPerPage) + 1, pagination.totalItems)} to {Math.min(pagination.currentPage * pagination.itemsPerPage, pagination.totalItems)} of {pagination.totalItems} entries</>
            ) : (
              'No entries to show'
            )}
          </span>
        </div>
        <Pagination
          currentPage={pagination.currentPage}
          totalPages={pagination.totalPages}
          onPageChange={pagination.onPageChange}
          className="ml-4"
        />
      </div>
    </div>
  );
};
