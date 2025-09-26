import React from 'react';
import { Modal } from '../atoms/Modal';
import { Button } from '../atoms/Button';
import { Input } from '../atoms/Input';
import { PropertyForm } from '../molecules/PropertyForm';
import type { Property, Owner, PropertyImage, PropertyTrace } from '../../types';

type EntityType = 'property' | 'owner' | 'propertyTrace' | 'propertyImage';
type ActionType = 'create' | 'edit' | 'delete';

interface ActionModalProps {
  isOpen: boolean;
  onClose: () => void;
  entityType: EntityType;
  actionType: ActionType;
  data?: Property | Owner | PropertyImage | PropertyTrace;
  onSubmit: (data: any) => Promise<void>;
  isLoading?: boolean;
}

export const ActionModal = ({
  isOpen,
  onClose,
  entityType,
  actionType,
  data,
  onSubmit,
  isLoading = false
}: ActionModalProps) => {
  const getModalTitle = () => {
    const action = actionType === 'create' ? 'Add' : actionType === 'edit' ? 'Edit' : 'Delete';
    const entity = entityType.charAt(0).toUpperCase() + entityType.slice(1).replace(/([A-Z])/g, ' $1');
    return `${action} ${entity}`;
  };

  const renderForm = () => {
    switch (entityType) {
      case 'property':
        return (
          <PropertyForm
            onSubmit={onSubmit}
            onCancel={onClose}
            isLoading={isLoading}
            initialData={data as Property}
          />
        );

      case 'owner':
        return (
          <form onSubmit={onSubmit} className="space-y-4">
            <Input
              label="Name"
              type="text"
              name="name"
              defaultValue={(data as Owner)?.Name}
              required
            />
            <Input
              label="Address"
              type="text"
              name="address"
              defaultValue={(data as Owner)?.Address}
              required
            />
            <Input
              label="Birthday"
              type="date"
              name="birthday"
              defaultValue={(data as Owner)?.Birthdate?.split('T')[0]}
              required
            />
            <Input
              label="Photo URL"
              type="text"
              name="photo"
              defaultValue={(data as Owner)?.Photo}
              placeholder="Enter photo URL"
            />
            <div className="flex justify-end space-x-2">
              <Button type="button" variant="outline" onClick={onClose}>
                Cancel
              </Button>
              <Button type="submit" disabled={isLoading}>
                {isLoading ? 'Saving...' : actionType === 'create' ? 'Create' : 'Update'}
              </Button>
            </div>
          </form>
        );

      case 'propertyTrace':
        return (
          <form onSubmit={onSubmit} className="space-y-4">
            <Input
              label="Date Sale"
              type="date"
              name="dateSale"
              defaultValue={(data as PropertyTrace)?.DateSale?.split('T')[0]}
              required
            />
            <Input
              label="Name"
              type="text"
              name="name"
              defaultValue={(data as PropertyTrace)?.Name}
              required
            />
            <Input
              label="Value"
              type="number"
              name="value"
              defaultValue={(data as PropertyTrace)?.Value?.toString()}
              required
            />
            <Input
              label="Tax"
              type="number"
              name="tax"
              defaultValue={(data as PropertyTrace)?.Tax?.toString()}
              required
            />
            <div className="flex justify-end space-x-2">
              <Button type="button" variant="outline" onClick={onClose}>
                Cancel
              </Button>
              <Button type="submit" disabled={isLoading}>
                {isLoading ? 'Saving...' : actionType === 'create' ? 'Create' : 'Update'}
              </Button>
            </div>
          </form>
        );

      case 'propertyImage':
        return (
          <form onSubmit={onSubmit} className="space-y-4">
            {actionType === 'edit' && (data as PropertyImage)?.File && (
              <div>
                <img 
                  src={(data as PropertyImage).File} 
                  alt="Current property image" 
                  className="w-full h-48 object-cover rounded mb-4"
                />
              </div>
            )}
            <Input
              label="Image URL"
              type="text"
              name="file"
              defaultValue={(data as PropertyImage)?.File}
              placeholder="Enter image URL"
              required
            />
            <Input
              label="Property ID"
              type="text"
              name="idProperty"
              defaultValue={(data as PropertyImage)?.IdProperty}
              required
            />
            <div className="flex items-center justify-between p-2 bg-gray-50 rounded-md">
              <div className="flex items-center space-x-2">
                <label className="relative inline-flex items-center cursor-pointer">
                  <input
                    type="checkbox"
                    name="enabled"
                    defaultChecked={(data as PropertyImage)?.Enabled}
                    className="sr-only peer"
                  />
                  <div className="w-11 h-6 bg-gray-200 rounded-full peer peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600"></div>
                </label>
                <span className="text-sm font-medium text-gray-900">Enable this image</span>
              </div>
            </div>
            <div className="flex justify-end space-x-2">
              <Button type="button" variant="outline" onClick={onClose}>
                Cancel
              </Button>
              <Button type="submit" disabled={isLoading}>
                {isLoading ? 'Saving...' : actionType === 'create' ? 'Create' : 'Update'}
              </Button>
            </div>
          </form>
        );

      default:
        return null;
    }
  };

  const renderDeleteConfirmation = () => (
    <div className="space-y-4">
      <p className="text-gray-600">
        Are you sure you want to delete this {entityType.replace(/([A-Z])/g, ' $1').toLowerCase()}?
      </p>
      <div className="flex justify-end space-x-2">
        <Button type="button" variant="outline" onClick={onClose}>
          Cancel
        </Button>
        <Button
          type="button"
          variant="danger"
          onClick={() => onSubmit(data)}
          disabled={isLoading}
        >
          {isLoading ? 'Deleting...' : 'Delete'}
        </Button>
      </div>
    </div>
  );

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={getModalTitle()}
    >
      {actionType === 'delete' ? renderDeleteConfirmation() : renderForm()}
    </Modal>
  );
};