import type {UseFormRegisterReturn} from 'react-hook-form';
import clsx from 'clsx';

interface FormInputProps {
  label: string;
  name: string;
  type?: string;
  register: UseFormRegisterReturn;
  error?: string;
  placeholder?: string;
}

export const FormInput = ({
  label,
  name,
  type = 'text',
  register,
  error,
  placeholder,
}: FormInputProps) => {
  return (
    <div className="w-full">
      <label htmlFor={name} className="label">
        {label}
      </label>
      <input
        type={type}
        id={name}
        placeholder={placeholder}
        {...register}
        className={clsx(
          'input',
          error && 'border-red-300 focus:border-red-500 focus:ring-red-500'
        )}
      />
      {error && (
        <p className="mt-1 text-sm text-red-600">{error}</p>
      )}
    </div>
  );
};