import toast from 'react-hot-toast';

type NotificationType = 'success' | 'error' | 'loading';

interface NotificationOptions {
  duration?: number;
  position?: 'top-right' | 'top-center' | 'top-left' | 'bottom-right' | 'bottom-center' | 'bottom-left';
}

const defaultOptions: NotificationOptions = {
  duration: 3000,
  position: 'top-right',
};

export const notificationService = {
  success(message: string, options?: NotificationOptions) {
    toast.success(message, { ...defaultOptions, ...options });
  },

  error(message: string, options?: NotificationOptions) {
    toast.error(message, { ...defaultOptions, ...options });
  },

  loading(message: string, options?: NotificationOptions) {
    return toast.loading(message, { ...defaultOptions, ...options });
  },

  dismiss(toastId?: string) {
    if (toastId) {
      toast.dismiss(toastId);
    } else {
      toast.dismiss();
    }
  },

  custom(message: string, type: NotificationType, options?: NotificationOptions) {
    switch (type) {
      case 'success':
        this.success(message, options);
        break;
      case 'error':
        this.error(message, options);
        break;
      case 'loading':
        return this.loading(message, options);
        break;
      default:
        toast(message, { ...defaultOptions, ...options });
    }
  },
};