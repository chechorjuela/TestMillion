import { useState, useEffect } from 'react';

type ValidationRule = {
  required?: boolean;
  minLength?: number;
  maxLength?: number;
  pattern?: RegExp;
  custom?: (value: any) => string | undefined;
};

type ValidationRules = {
  [key: string]: ValidationRule;
};

type FormErrors = {
  [key: string]: string;
};

export const useFormValidation = (formData: any, rules: ValidationRules) => {
  const [errors, setErrors] = useState<FormErrors>({});
  const [isValid, setIsValid] = useState(false);

  const validateField = (name: string, value: any, rule: ValidationRule): string | undefined => {
    if (rule.required && (!value || value.toString().trim() === '')) {
      return `${name} is required`;
    }

    if (value) {
      if (rule.minLength && value.length < rule.minLength) {
        return `${name} must be at least ${rule.minLength} characters`;
      }

      if (rule.maxLength && value.length > rule.maxLength) {
        return `${name} must be no more than ${rule.maxLength} characters`;
      }

      if (rule.pattern && !rule.pattern.test(value)) {
        return `${name} has an invalid format`;
      }

      if (rule.custom) {
        return rule.custom(value);
      }
    }

    return undefined;
  };

  const validateForm = () => {
    const newErrors: FormErrors = {};
    let formIsValid = true;

    Object.keys(rules).forEach((fieldName) => {
      const value = formData[fieldName];
      const error = validateField(fieldName, value, rules[fieldName]);
      
      if (error) {
        newErrors[fieldName] = error;
        formIsValid = false;
      }
    });

    setErrors(newErrors);
    setIsValid(formIsValid);

    return formIsValid;
  };

  useEffect(() => {
    validateForm();
  }, [formData]);

  return {
    errors,
    isValid,
    validateForm,
  };
};