import { useForm } from 'react-hook-form';
import { FormInput } from '../atoms/FormInput';

interface PropertyFormData {
  idOwner: string;
  name: string;
  address: string;
  price: string;
  codeInternal: string;
  year: string;
  mainImage?: string;
}

interface PropertyFormProps {
  onSubmit: (data: PropertyFormData) => void;
  onCancel?: () => void;
  isLoading?: boolean;
}

export const PropertyForm = ({ onSubmit, onCancel, isLoading }: PropertyFormProps) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<PropertyFormData>({    
    defaultValues: {
      idOwner: '',
      name: '',
      address: '',
      price: '',
      codeInternal: '',
      year: new Date().getFullYear().toString(),
      mainImage: ''
    }
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <FormInput
        label="Property Name"
        name="name"
        register={register('name', {
          required: 'Property name is required',
          minLength: { value: 3, message: 'Name must be at least 3 characters' },
          maxLength: { value: 100, message: 'Name must not exceed 100 characters' }
        })}
        error={errors.name?.message}
        placeholder="Enter property name"
      />
      
      <FormInput
        label="Internal Code"
        name="codeInternal"
        register={register('codeInternal', {
          required: 'Internal code is required',
          minLength: { value: 3, message: 'Code must be at least 3 characters' }
        })}
        error={errors.codeInternal?.message}
        placeholder="Enter internal code"
      />
      
      <div className="grid grid-cols-2 gap-4">
        <FormInput
          label="Price"
          name="price"
          type="number"
          register={register('price', {
            required: 'Price is required',
            min: { value: 0, message: 'Price must be greater than 0' },
            pattern: { value: /^\d+(\.\d{1,2})?$/, message: 'Price must be a valid decimal number' }
          })}
          error={errors.price?.message}
          placeholder="Enter price"
        />
        
        <FormInput
          label="Address"
          name="address"
          register={register('address', {
            required: 'Address is required',
            minLength: { value: 5, message: 'Address must be at least 5 characters' }
          })}
          error={errors.address?.message}
          placeholder="Enter property address"
        />
      </div>
      
      <div className="grid grid-cols-2 gap-4">
        <FormInput
          label="Year"
          name="year"
          type="number"
          register={register('year', {
            required: 'Year is required',
            min: { value: 1800, message: 'Year must be after 1800' },
            max: { value: new Date().getFullYear(), message: 'Year cannot be in the future' },
            pattern: { value: /^\d{4}$/, message: 'Please enter a valid year' }
          })}
          error={errors.year?.message}
          placeholder="Construction year"
        />
        
        <FormInput
          label="Main Image URL"
          name="mainImage"
          register={register('mainImage')}
          error={errors.mainImage?.message}
          placeholder="Enter image URL (optional)"
        />
      </div>
      
      <FormInput
        label="Owner ID"
        name="idOwner"
        register={register('idOwner', {
          required: 'Owner ID is required'
        })}
        error={errors.idOwner?.message}
        placeholder="Enter owner ID"
      />

      <div className="flex justify-end space-x-3 mt-6">
        <button
          type="button"
          onClick={onCancel}
          className="btn btn-outline"
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={isLoading}
          className="btn btn-primary"
        >
          {isLoading ? 'Creating...' : 'Create Property'}
        </button>
      </div>
    </form>
  );
};