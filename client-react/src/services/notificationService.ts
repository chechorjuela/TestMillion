import toast from 'react-hot-toast';
import { TOAST_DURATION } from '../config/constants';

type EntityType = 'property' | 'owner' | 'propertyTrace' | 'propertyImage';
type ActionType = 'create' | 'update' | 'delete';

const getEntityName = (entityType: EntityType): string => {
  switch (entityType) {
    case 'propertyTrace':
      return 'Property Trace';
    case 'propertyImage':
      return 'Property Image';
    default:
      return entityType.charAt(0).toUpperCase() + entityType.slice(1);
  }
};

const getActionMessage = (entityType: EntityType, action: ActionType, isSuccess: boolean): string => {
  const entityName = getEntityName(entityType);
  const actionPast = action === 'create' ? 'created' : action === 'update' ? 'updated' : 'deleted';
  
  if (isSuccess) {
    return `${entityName} ${actionPast} successfully`;
  }
  
  return `Failed to ${action} ${entityName.toLowerCase()}`;
};

type NotificationType = 'success' | 'error' | 'loading';

interface NotificationOptions {
  duration?: number;
  position?: 'top-right' | 'top-center' | 'top-left' | 'bottom-right' | 'bottom-center' | 'bottom-left';
}

const defaultOptions: NotificationOptions = {
  duration: 3000,
  position: 'top-right',
};

class NotificationService {
  success(message: string, options?: NotificationOptions) {
    toast.success(message, {
      ...defaultOptions,
      ...options,
      style: {
        background: '#10B981',
        color: '#FFFFFF',
      },
    });
  }

  error(message: string, options?: NotificationOptions) {
    toast.error(message, {
      ...defaultOptions,
      ...options,
      style: {
        background: '#EF4444',
        color: '#FFFFFF',
      },
    });
  }

  loading(message: string, options?: NotificationOptions) {
    return toast.loading(message, { ...defaultOptions, ...options });
  }

  dismiss(toastId?: string) {
    if (toastId) {
      toast.dismiss(toastId);
    } else {
      toast.dismiss();
    }
  }

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
      default:
        toast(message, { ...defaultOptions, ...options });
    }
  }

  entitySuccess(entityType: EntityType, action: ActionType) {
    this.success(getActionMessage(entityType, action, true));
  }

  entityError(entityType: EntityType, action: ActionType, error?: any) {
    const baseMessage = getActionMessage(entityType, action, false);
    const errorDetail = error?.message || '';
    this.error(`${baseMessage}${errorDetail ? `: ${errorDetail}` : ''}`);
  }

  networkError() {
    this.error('Network error. Please check your connection.');
  }

  serverError() {
    this.error('Server error occurred. Please try again later.');
  }

  validationError(message: string) {
    this.error(`Validation error: ${message}`);
  }
}

export const notificationService = new NotificationService();
