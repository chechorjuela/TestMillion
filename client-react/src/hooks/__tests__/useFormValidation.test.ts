import { renderHook, act } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { useFormValidation } from '../useFormValidation';

describe('useFormValidation', () => {
  it('validates required fields', () => {
    const formData = {
      name: '',
      email: 'test@example.com'
    };

    const rules = {
      name: { required: true },
      email: { required: true }
    };

    const { result } = renderHook(() => useFormValidation(formData, rules));

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.name).toBe('name is required');
    expect(result.current.errors.email).toBeUndefined();
  });

  it('validates minimum length', () => {
    const formData = {
      username: 'ab',
      password: 'abcdef'
    };

    const rules = {
      username: { minLength: 3 },
      password: { minLength: 6 }
    };

    const { result } = renderHook(() => useFormValidation(formData, rules));

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.username).toBe('username must be at least 3 characters');
    expect(result.current.errors.password).toBeUndefined();
  });

  it('validates maximum length', () => {
    const formData = {
      title: 'This is a very long title that exceeds the maximum length',
      summary: 'Short summary'
    };

    const rules = {
      title: { maxLength: 20 },
      summary: { maxLength: 20 }
    };

    const { result } = renderHook(() => useFormValidation(formData, rules));

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.title).toBe('title must be no more than 20 characters');
    expect(result.current.errors.summary).toBeUndefined();
  });

  it('validates patterns', () => {
    const formData = {
      email: 'invalid-email',
      url: 'https://example.com'
    };

    const rules = {
      email: { pattern: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i },
      url: { pattern: /^https?:\/\/.+/ }
    };

    const { result } = renderHook(() => useFormValidation(formData, rules));

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.email).toBe('email has an invalid format');
    expect(result.current.errors.url).toBeUndefined();
  });

  it('validates using custom validation functions', () => {
    const formData = {
      age: '17',
      code: 'ABC123'
    };

    const rules = {
      age: {
        custom: (value: string) => {
          const age = parseInt(value);
          if (age < 18) return 'Must be 18 or older';
        }
      },
      code: {
        custom: (value: string) => {
          if (!value.startsWith('XYZ')) return 'Code must start with XYZ';
        }
      }
    };

    const { result } = renderHook(() => useFormValidation(formData, rules));

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.age).toBe('Must be 18 or older');
    expect(result.current.errors.code).toBe('Code must start with XYZ');
  });

  it('updates validation when form data changes', () => {
    const initialFormData = {
      name: '',
      email: 'invalid-email'
    };

    const rules = {
      name: { required: true },
      email: { pattern: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i }
    };

    const { result, rerender } = renderHook(
      ({ formData }) => useFormValidation(formData, rules),
      { initialProps: { formData: initialFormData } }
    );

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.name).toBeTruthy();
    expect(result.current.errors.email).toBeTruthy();

    // Update form data
    const updatedFormData = {
      name: 'John Doe',
      email: 'john@example.com'
    };

    rerender({ formData: updatedFormData });

    expect(result.current.isValid).toBe(true);
    expect(result.current.errors.name).toBeUndefined();
    expect(result.current.errors.email).toBeUndefined();
  });

  it('validates multiple rules for a single field', () => {
    const formData = {
      password: 'ab'
    };

    const rules = {
      password: {
        required: true,
        minLength: 8,
        pattern: /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/,
        custom: (value: string) => {
          if (!value.includes('!')) return 'Password must include !';
        }
      }
    };

    const { result } = renderHook(() => useFormValidation(formData, rules));

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.password).toBe('password must be at least 8 characters');
  });
});