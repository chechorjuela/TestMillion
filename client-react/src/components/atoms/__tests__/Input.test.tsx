import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Input } from '../Input';

describe('Input', () => {
  it('renders correctly with label', () => {
    render(<Input label="Username" name="username" />);
    expect(screen.getByLabelText('Username')).toBeInTheDocument();
  });

  it('renders without label', () => {
    render(<Input placeholder="Enter username" name="username" />);
    expect(screen.getByPlaceholderText('Enter username')).toBeInTheDocument();
  });

  it('shows error message when provided', () => {
    render(<Input label="Username" name="username" error="This field is required" />);
    expect(screen.getByText('This field is required')).toBeInTheDocument();
  });

  it('applies error styles when error is present', () => {
    render(<Input label="Username" name="username" error="This field is required" />);
    const input = screen.getByLabelText('Username');
    expect(input).toHaveClass('border-red-300');
  });

  it('handles value changes', async () => {
    const handleChange = vi.fn();
    render(<Input label="Username" name="username" onChange={handleChange} />);
    
    const input = screen.getByLabelText('Username');
    await userEvent.type(input, 'test');
    
    expect(handleChange).toHaveBeenCalledTimes(4); // Once for each character
  });

  it('handles controlled value updates', () => {
    const { rerender } = render(<Input label="Username" name="username" value="initial" />);
    expect(screen.getByLabelText('Username')).toHaveValue('initial');

    rerender(<Input label="Username" name="username" value="updated" />);
    expect(screen.getByLabelText('Username')).toHaveValue('updated');
  });

  it('forwards ref correctly', () => {
    const ref = vi.fn();
    render(<Input ref={ref} name="test" />);
    expect(ref).toHaveBeenCalled();
  });

  it('handles different input types', () => {
    render(<Input type="number" label="Age" name="age" />);
    const input = screen.getByLabelText('Age');
    expect(input).toHaveAttribute('type', 'number');
  });

  it('handles disabled state', () => {
    render(<Input label="Username" name="username" disabled />);
    expect(screen.getByLabelText('Username')).toBeDisabled();
  });

  it('handles required attribute', () => {
    render(<Input label="Username" name="username" required />);
    expect(screen.getByLabelText('Username')).toBeRequired();
  });

  it('passes through additional props', () => {
    render(<Input label="Username" name="username" data-testid="custom-input" />);
    expect(screen.getByLabelText('Username')).toHaveAttribute('data-testid', 'custom-input');
  });

  it('maintains value on blur', async () => {
    const { container } = render(<Input label="Username" name="username" />);
    const input = screen.getByLabelText('Username');
    
    await userEvent.type(input, 'test value');
    fireEvent.blur(input);
    
    expect(input).toHaveValue('test value');
  });
});