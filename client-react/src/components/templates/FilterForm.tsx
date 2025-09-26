import { useForm, type UseFormRegister } from 'react-hook-form';
import { motion } from 'framer-motion';
import { Button } from '../atoms/Button';

interface FilterField {
  name: string;
  label: string;
  type: 'text' | 'number' | 'select' | 'date';
  options?: { value: string; label: string }[];
  placeholder?: string;
}

interface FilterFormProps<T extends object> {
  fields: FilterField[];
  onSubmit: (data: T) => void;
  loading?: boolean;
}

export const FilterForm = <T extends object>({
  fields,
  onSubmit,
  loading = false,
}: FilterFormProps<T>) => {
  const { register, handleSubmit, reset } = useForm<T>();

  const renderField = (field: FilterField, register: UseFormRegister<T>) => {
    const commonClasses = 'block w-full rounded-md border-theme-silver/30 bg-theme-white text-theme-black shadow-sm focus:border-theme-silver focus:ring-theme-silver sm:text-sm placeholder-theme-medium-gray/50';

    switch (field.type) {
      case 'select':
        return (
          <select
            {...register(field.name as any)}
            className={commonClasses}
          >
            <option value="">All</option>
            {field.options?.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        );

      case 'number':
        return (
          <input
            type="number"
            {...register(field.name as any)}
            placeholder={field.placeholder}
            className={commonClasses}
          />
        );

      default:
        return (
          <input
            type={field.type}
            {...register(field.name as any)}
            placeholder={field.placeholder}
            className={commonClasses}
          />
        );
    }
  };

  return (
    <motion.form
      initial={{ opacity: 0, y: -20 }}
      animate={{ opacity: 1, y: 0 }}
      onSubmit={handleSubmit(onSubmit)}
      className="bg-theme-white p-6 rounded-lg shadow-lg border border-theme-silver/20"
    >
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {fields.map((field) => (
          <div key={field.name}>
            <label
              htmlFor={field.name}
              className="block text-sm font-medium text-theme-medium-gray mb-1"
            >
              {field.label}
            </label>
            {renderField(field, register)}
          </div>
        ))}
      </div>

      <div className="mt-4 flex gap-3">
        <Button type="submit" isLoading={loading}>
          Apply Filters
        </Button>
        <Button
          type="button"
          variant="outline"
          onClick={() => reset()}
        >
          Clear
        </Button>
      </div>
    </motion.form>
  );
};