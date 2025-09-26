import React, { useState, useEffect } from 'react';
import { useFormValidation } from '../../hooks/useFormValidation';
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

  const [formData, setFormData] = useState<any>({});

  // Validation rules for each entity type
  const validationRules = {
    propertyImage: {
      file: {
        required: true,
        pattern: /^https?:\/\/.+/,
        custom: (value: string) => {
          if (!value.match(/\.(jpg|jpeg|png|gif|webp)$/i)) {
            return 'URL must end with a valid image extension (.jpg, .png, .gif, etc)';
          }
        }
      },
      name: {
        required: true,
        minLength: 3,
        maxLength: 100
      },
      description: {
        required: true,
        minLength: 10,
        maxLength: 500
      },
      imagePath: {
        required: true,
        pattern: /^https?:\/\/.+/
      },
      idProperty: {
        required: true,
        pattern: /^[0-9a-fA-F]{24}$/,
        custom: (value: string) => {
          if (value && !value.match(/^[0-9a-fA-F]{24}$/)) {
            return 'Property ID must be a valid MongoDB ObjectId (24 hexadecimal characters)';
          }
        }
      }
    },
    owner: {
      name: {
        required: true,
        minLength: 3,
        maxLength: 100
      },
      address: {
        required: true,
        minLength: 5,
        maxLength: 200
      },
      birthday: {
        required: true,
        custom: (value: string) => {
          const date = new Date(value);
          if (isNaN(date.getTime())) {
            return 'Please enter a valid date';
          }
          if (date > new Date()) {
            return 'Birthday cannot be in the future';
          }
        }
      },
      photo: {
        pattern: /^https?:\/\/.+/
      }
    },
    propertyTrace: {
      dateSale: {
        required: true,
        custom: (value: string) => {
          const date = new Date(value);
          if (isNaN(date.getTime())) {
            return 'Please enter a valid date';
          }
        }
      },
      name: {
        required: true,
        minLength: 3,
        maxLength: 100
      },
      value: {
        required: true,
        custom: (value: string) => {
          const num = parseFloat(value);
          if (isNaN(num) || num <= 0) {
            return 'Value must be a positive number';
          }
        }
      },
      tax: {
        required: true,
        custom: (value: string) => {
          const num = parseFloat(value);
          if (isNaN(num) || num < 0 || num > 100) {
            return 'Tax must be a number between 0 and 100';
          }
        }
      },
      idProperty: {
        required: true,
        pattern: /^[0-9a-fA-F]{24}$/
      }
    }
  };

  const { errors, isValid, validateForm } = useFormValidation(
    formData,
    validationRules[entityType] || {}
  );

  useEffect(() => {
    if (data) {
      switch (entityType) {
        case 'propertyImage':
          const propertyImage = data as PropertyImage;
          setFormData({
            file: propertyImage.FileUrl,
            idProperty: propertyImage.Property?.Id || propertyImage.IdProperty,
            enabled: propertyImage.Enabled,
            name: propertyImage.Name,
            description: propertyImage.Description,
            imagePath: propertyImage.FileUrl,
            // Mantener los datos originales también
            ...data
          });
          break;
        default:
          setFormData(data);
          break;
      }
    } else {
      setFormData({});
    }
  }, [data, entityType]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const formatDate = (date: string) => {
    if (!date) return undefined;
    try {
      return date.split('T')[0];
    } catch (e) {
      return date;
    }
  };

  const formatNumberField = (value: string | undefined) => {
    if (!value) return undefined;
    const num = parseFloat(value);
    return isNaN(num) ? undefined : num;
  };

  const prepareOwnerData = () => {
    const values = {
      Name: formData.name || formData.Name,
      Address: formData.address || formData.Address,
      Birthdate: formatDate(formData.birthday || formData.Birthdate),
      Photo: formData.photo || formData.Photo
    };

    if (actionType === 'edit' && (data as Owner)?.Id) {
      values.Id = (data as Owner).Id;
    }

    return values;
  };

  const preparePropertyData = () => {
    const values = {
      Name: formData.name || formData.Name,
      Address: formData.address || formData.Address,
      Price: formatNumberField(formData.price || formData.Price?.toString()),
      Year: formatNumberField(formData.year || formData.Year?.toString()),
      CodeInternal: formData.codeInternal || formData.CodeInternal,
      IdOwner: formData.idOwner || formData.IdOwner,
      MainImage: formData.mainImage || formData.MainImage
    };

    if (actionType === 'edit' && (data as Property)?.Id) {
      values.Id = (data as Property).Id;
    }

    return values;
  };

  const preparePropertyTraceData = () => {
    const values = {
      Name: formData.name || formData.Name,
      Value: formatNumberField(formData.value || formData.Value?.toString()),
      Tax: formatNumberField(formData.tax || formData.Tax?.toString()),
      DateSale: formatDate(formData.dateSale || formData.DateSale),
      IdProperty: formData.idProperty || formData.IdProperty
    };

    if (actionType === 'edit' && (data as PropertyTrace)?.Id) {
      values.Id = (data as PropertyTrace).Id;
    }

    return values;
  };

  const preparePropertyImageData = () => {
    const values = actionType === 'create' ? {
      File: formData.file || '',
      Enabled: formData.enabled ?? true,
      IdProperty: formData.idProperty || '',
      Name: formData.name || 'Image ' + new Date().toISOString(),
      Description: formData.description || 'Property image description',
      ImagePath: formData.imagePath || formData.file || ''
    } : {
      FileUrl: formData.file || formData.FileUrl || '',
      Enabled: formData.enabled ?? formData.Enabled ?? true,
      IdProperty: formData.idProperty || formData.Property?.Id || '',
      Name: formData.name || 'Image ' + new Date().toISOString(),
      Description: formData.description || 'Property image description',
      ImagePath: formData.imagePath || formData.file || formData.FileUrl || ''
    };

    if (actionType === 'edit' && (data as PropertyImage)?.Id) {
      values.Id = (data as PropertyImage).Id;
    }

    return values;
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    
    if (!validateForm()) {
      return;
    }
    
    let values: any;
    
    switch (entityType) {
      case 'owner':
        values = prepareOwnerData();
        break;
      case 'property':
        values = preparePropertyData();
        break;
      case 'propertyTrace':
        values = preparePropertyTraceData();
        break;
      case 'propertyImage':
        values = preparePropertyImageData();
        break;
      default:
        return;
    }

    Object.keys(values).forEach(key => {
      if (values[key] === undefined || values[key] === '' || 
          (typeof values[key] === 'number' && isNaN(values[key]))) {
        delete values[key];
      }
    });

    onSubmit(values);
  };

  const renderForm = () => {
    switch (entityType) {
      case 'property':
        return (
          <PropertyForm
            onSubmit={onSubmit}
            onCancel={onClose}
            isLoading={isLoading}
            initialData={data ? {
              idOwner: (data as Property).IdOwner,
              name: (data as Property).Name || '',
              address: (data as Property).Address || '',
              price: ((data as Property).Price || '').toString(),
              codeInternal: (data as Property).CodeInternal || '',
              year: ((data as Property).Year || new Date().getFullYear()).toString(),
              mainImage: (data as Property).MainImage || ''
            } : undefined}
          />
        );

      case 'owner':
        return (
          <form onSubmit={handleSubmit} className="space-y-4">
            <Input
              label="Name"
              type="text"
              name="name"
              value={formData?.Name || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Address"
              type="text"
              name="address"
              value={formData?.Address || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Birthday"
              type="date"
              name="birthday"
              value={formData?.Birthdate?.split('T')[0] || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Photo URL"
              type="text"
              name="photo"
              value={formData?.Photo || ''}
              onChange={handleInputChange}
              placeholder="Enter photo URL"
            />
            <div className="flex justify-end space-x-2">
              <Button type="button" variant="outline" onClick={onClose}>
                Cancel
              </Button>
              <Button 
                type="submit" 
                disabled={isLoading || !isValid}
                title={!isValid ? 'Please fix form errors before submitting' : undefined}
              >
                {isLoading ? 'Saving...' : actionType === 'create' ? 'Create' : 'Update'}
              </Button>
            </div>
          </form>
        );

      case 'propertyTrace':
        return (
          <form onSubmit={handleSubmit} className="space-y-4">
            <Input
              label="Date Sale"
              type="date"
              name="dateSale"
              value={formData?.DateSale?.split('T')[0] || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Name"
              type="text"
              name="name"
              value={formData?.Name || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Value"
              type="number"
              name="value"
              value={formData?.Value?.toString() || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Tax (0 - 100)"
              type="number"
              name="tax"
              value={formData?.Tax?.toString() || ''}
              onChange={handleInputChange}
              required
            />
            <Input
              label="Property ID"
              type="text"
              name="idProperty"
              value={formData?.IdProperty || ''}
              onChange={handleInputChange}
              placeholder="24-char MongoDB ObjectId"
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
          <form onSubmit={handleSubmit} className="space-y-4">
            {actionType === 'edit' && (data as PropertyImage)?.FileUrl && (
              <div>
                <img 
                  src={(data as PropertyImage).FileUrl} 
                  alt="Current property image" 
                  className="w-full h-48 object-cover rounded mb-4"
                />
              </div>
            )}
            <Input
              label="Name"
              type="text"
              name="name"
              value={formData?.name || ''}
              onChange={handleInputChange}
              placeholder="Enter image name"
              error={errors.name}
              required
            />
            <Input
              label="Description"
              type="text"
              name="description"
              value={formData?.description || ''}
              onChange={handleInputChange}
              placeholder="Enter image description"
              error={errors.description}
              required
            />
            <Input
              label="Image URL"
              type="text"
              name="file"
              value={formData?.file || formData?.FileUrl || ''}
              onChange={handleInputChange}
              placeholder="Enter image URL"
              error={errors.file}
              required
            />
            <Input
              label="Image Path"
              type="text"
              name="imagePath"
              value={formData?.imagePath || formData?.FileUrl || ''}
              onChange={handleInputChange}
              placeholder="Enter image path"
              error={errors.imagePath}
              required
            />
            <Input
              label="Property ID"
              type="text"
              name="idProperty"
              value={formData?.idProperty || formData?.Property?.Id || formData?.IdProperty || ''}
              onChange={handleInputChange}
              error={errors.idProperty}
              required
            />
            <div className="flex items-center justify-between p-2 bg-gray-50 rounded-md">
              <div className="flex items-center space-x-2">
                <label className="relative inline-flex items-center cursor-pointer">
                  <input
                    type="checkbox"
                    name="enabled"
                    checked={formData?.enabled ?? formData?.Enabled ?? true}
                    onChange={handleInputChange}
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