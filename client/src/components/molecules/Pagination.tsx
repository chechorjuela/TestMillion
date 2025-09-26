import { useMemo } from 'react';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  className?: string;
}

export const Pagination = ({
  currentPage,
  totalPages,
  onPageChange,
  className = '',
}: PaginationProps) => {
  const handleDotsClick = (position: 'left' | 'right') => {
    if (position === 'left') {
      const targetPage = Math.floor((currentPage - 1) / 2) + 1;
      onPageChange(targetPage);
    } else {
      const targetPage = Math.floor((totalPages - currentPage) / 2) + currentPage;
      onPageChange(targetPage);
    }
  };

  const handlePageClick = (page: number | string, position?: 'left' | 'right') => {
    if (typeof page === 'number' && page !== currentPage) {
      onPageChange(page);
    } else if (page === '...' && position) {
      handleDotsClick(position);
    }
  };
  const pages = useMemo(() => {
    const items: (number | string)[] = [];
    const DOTS = '...';
    const siblingCount = 1;
    const totalNumbers = 7;

    if (totalPages <= totalNumbers) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }

    const leftSiblingIndex = Math.max(currentPage - siblingCount, 1);
    const rightSiblingIndex = Math.min(currentPage + siblingCount, totalPages);

    const shouldShowLeftDots = leftSiblingIndex > 2;
    const shouldShowRightDots = rightSiblingIndex < totalPages - 1;

    if (!shouldShowLeftDots && shouldShowRightDots) {
      const leftItemCount = 3 + 2 * siblingCount;
      const leftRange = Array.from({ length: leftItemCount }, (_, i) => i + 1);

      items.push(...leftRange);
      items.push(DOTS);
      items.push(totalPages);
      return items;
    }

    if (shouldShowLeftDots && !shouldShowRightDots) {
      const rightItemCount = 3 + 2 * siblingCount;
      const rightRange = Array.from(
        { length: rightItemCount },
        (_, i) => totalPages - rightItemCount + i + 1
      );
      items.push(1);
      items.push(DOTS);
      items.push(...rightRange);
      return items;
    }

    if (shouldShowLeftDots && shouldShowRightDots) {
      const middleRange = Array.from(
        { length: rightSiblingIndex - leftSiblingIndex + 1 },
        (_, i) => leftSiblingIndex + i
      );
      items.push(1);
      items.push(DOTS);
      items.push(...middleRange);
      items.push(DOTS);
      items.push(totalPages);
      return items;
    }

    return items;
  }, [currentPage, totalPages]);

  if (totalPages <= 1) return null;

  return (
    <nav className={`flex items-center justify-center space-x-1 select-none ${className}`}>
      <button
        type="button"
        onClick={(e) => {
          e.preventDefault();
          e.stopPropagation();
          if (currentPage > 1) handlePageClick(currentPage - 1);
        }}
        disabled={currentPage === 1}
        className={`px-3 py-2 rounded-md text-sm font-medium border ${
          currentPage === 1
            ? 'text-theme-medium-gray/50 cursor-not-allowed border-theme-silver/20 bg-theme-silver/5'
            : 'text-theme-medium-gray hover:bg-theme-silver/10 border-theme-silver/30'
        }`}
      >
        &lt;
      </button>

      {pages.map((page, index, array) => {
        const isLeftDots = page === '...' && array[index - 1] === 1;
        const isRightDots = page === '...' && array[index + 1] === totalPages;
        const position = isLeftDots ? 'left' : isRightDots ? 'right' : undefined;

        return (
          <button
            type="button"
            key={index}
            onClick={(e) => {
              e.preventDefault();
              e.stopPropagation();
              handlePageClick(page, position);
            }}
            disabled={typeof page === 'number' && page === currentPage}
          className={`px-3 py-2 rounded-md text-sm font-medium border min-w-[40px] ${
            typeof page === 'number' && page === currentPage
              ? 'bg-theme-silver text-theme-white border-theme-silver cursor-default'
              : typeof page === 'string'
              ? 'text-theme-medium-gray/50 cursor-default border-transparent'
              : 'text-theme-medium-gray hover:bg-theme-silver/10 border-theme-silver/30'
          }`}
        >
          {page}
        </button>
        );
      })}

      {/* Botón Next */}
      <button
        type="button"
        onClick={(e) => {
          e.preventDefault();
          e.stopPropagation();
          if (currentPage < totalPages) handlePageClick(currentPage + 1);
        }}
        disabled={currentPage === totalPages}
        className={`px-3 py-2 rounded-md text-sm font-medium border ${
          currentPage === totalPages
            ? 'text-theme-medium-gray/50 cursor-not-allowed border-theme-silver/20 bg-theme-silver/5'
            : 'text-theme-medium-gray hover:bg-theme-silver/10 border-theme-silver/30'
        }`}
      >
        &gt;
      </button>
    </nav>
  );
};
