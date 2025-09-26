import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { ActionModal } from '../ActionModal';

describe('ActionModal', () => {
  const mockOnClose = vi.fn();
  const mockOnSubmit = vi.fn();

  const defaultProps = {
    isOpen: true,
    onClose: mockOnClose,
    entityType: 'propertyImage' as const,
    actionType: 'create' as const,
    onSubmit: mockOnSubmit,
    isLoading: false,
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders property image form correctly', () => {
    render(<ActionModal {...defaultProps} />);

    expect(screen.getByLabelText('Name')).toBeInTheDocument();
    expect(screen.getByLabelText('Description')).toBeInTheDocument();
    expect(screen.getByLabelText('Image URL')).toBeInTheDocument();
    expect(screen.getByLabelText('Image Path')).toBeInTheDocument();
    expect(screen.getByLabelText('Property ID')).toBeInTheDocument();
  });

  it('shows validation errors for empty required fields', async () => {
    render(<ActionModal {...defaultProps} />);

    const submitButton = screen.getByRole('button', { name: /create/i });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('name is required')).toBeInTheDocument();
      expect(screen.getByText('description is required')).toBeInTheDocument();
      expect(screen.getByText('file is required')).toBeInTheDocument();
      expect(screen.getByText('imagePath is required')).toBeInTheDocument();
      expect(screen.getByText('idProperty is required')).toBeInTheDocument();
    });

    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  it('validates property ID format', async () => {
    render(<ActionModal {...defaultProps} />);
    
    const propertyIdInput = screen.getByLabelText('Property ID');
    await userEvent.type(propertyIdInput, 'invalid-id');

    await waitFor(() => {
      expect(screen.getByText('idProperty has an invalid format')).toBeInTheDocument();
    });
  });

  it('validates image URL format', async () => {
    render(<ActionModal {...defaultProps} />);
    
    const imageUrlInput = screen.getByLabelText('Image URL');
    await userEvent.type(imageUrlInput, 'invalid-url');

    await waitFor(() => {
      expect(screen.getByText('file has an invalid format')).toBeInTheDocument();
    });
  });

  it('submits form with valid data', async () => {
    render(<ActionModal {...defaultProps} />);

    // Fill in valid data
    await userEvent.type(screen.getByLabelText('Name'), 'Test Image');
    await userEvent.type(screen.getByLabelText('Description'), 'This is a test description');
    await userEvent.type(screen.getByLabelText('Image URL'), 'https://example.com/image.jpg');
    await userEvent.type(screen.getByLabelText('Image Path'), 'https://example.com/image.jpg');
    await userEvent.type(screen.getByLabelText('Property ID'), '507f1f77bcf86cd799439011');

    const submitButton = screen.getByRole('button', { name: /create/i });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(mockOnSubmit).toHaveBeenCalledWith({
        File: 'https://example.com/image.jpg',
        Name: 'Test Image',
        Description: 'This is a test description',
        ImagePath: 'https://example.com/image.jpg',
        IdProperty: '507f1f77bcf86cd799439011',
        Enabled: true,
      });
    });
  });

  it('disables submit button when loading', () => {
    render(<ActionModal {...defaultProps} isLoading={true} />);
    
    const submitButton = screen.getByRole('button', { name: /saving/i });
    expect(submitButton).toBeDisabled();
  });

  it('handles edit mode correctly', () => {
    const editData = {
      Id: '123',
      FileUrl: 'https://example.com/existing.jpg',
      Name: 'Existing Image',
      Description: 'Existing description',
      Property: {
        Id: '507f1f77bcf86cd799439011',
        Name: 'Test Property'
      },
      Enabled: true
    };

    render(
      <ActionModal
        {...defaultProps}
        actionType="edit"
        data={editData}
      />
    );

    const nameInput = screen.getByLabelText('Name');
    const descInput = screen.getByLabelText('Description');
    const fileInput = screen.getByLabelText('Image URL');
    const propertyIdInput = screen.getByLabelText('Property ID');
    
    expect(nameInput).toHaveValue('Existing Image');
    expect(descInput).toHaveValue('Existing description');
    expect(fileInput).toHaveValue('https://example.com/existing.jpg');
    expect(propertyIdInput).toHaveValue('507f1f77bcf86cd799439011');
  });
});