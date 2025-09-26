import { useState, useCallback } from 'react';
import type { ActionMode } from '../types/common';

interface UseModalProps<T> {
  onSuccess?: () => void;
  onError?: (error: any) => void;
}

export const useModal = <T,>({ onSuccess, onError }: UseModalProps<T> = {}) => {
  const [isOpen, setIsOpen] = useState(false);
  const [mode, setMode] = useState<ActionMode>('create');
  const [data, setData] = useState<T | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleOpen = useCallback((newMode: ActionMode, item?: T) => {
    setMode(newMode);
    setData(item || null);
    setIsOpen(true);
  }, []);

  const handleClose = useCallback(() => {
    setData(null);
    setIsOpen(false);
    setIsLoading(false);
  }, []);

  const handleAction = useCallback(async (action: () => Promise<void>) => {
    setIsLoading(true);
    try {
      await action();
      onSuccess?.();
      handleClose();
    } catch (error) {
      onError?.(error);
      console.error('Modal action error:', error);
    } finally {
      setIsLoading(false);
    }
  }, [handleClose, onSuccess, onError]);

  return {
    isOpen,
    mode,
    data,
    isLoading,
    handleOpen,
    handleClose,
    handleAction,
  };
};