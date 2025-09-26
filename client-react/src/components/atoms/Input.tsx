import { InputHTMLAttributes, forwardRef, useState, useEffect } from 'react';
import { clsx } from 'clsx';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  value?: string | number | readonly string[];
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  name: string;
}

export const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ className, label, error, value: propValue, onChange, name, ...props }, ref) => {
    const [localValue, setLocalValue] = useState(propValue);

    useEffect(() => {
      setLocalValue(propValue);
    }, [propValue]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      setLocalValue(e.target.value);
      if (onChange) {
        onChange(e);
      }
    };
    return (
      <div className="w-full">
        {label && (
          <label 
            htmlFor={name}
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            {label}
          </label>
        )}
        <input
          ref={ref}
          id={name}
          name={name}
          className={clsx(
            'block w-full rounded-md border-gray-300 shadow-sm focus:border-primary-500 focus:ring-primary-500 sm:text-sm',
            error && 'border-red-300 focus:border-red-500 focus:ring-red-500',
            className
          )}
          value={localValue}
          onChange={handleChange}
          {...props}
        />
        {error && (
          <p className="mt-1 text-sm text-red-600">{error}</p>
        )}
      </div>
    );
  }
);