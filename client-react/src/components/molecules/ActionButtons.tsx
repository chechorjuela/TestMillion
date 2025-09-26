import React from 'react';
import { Button } from '../atoms/Button';
import { BsEye, BsPencil, BsTrash } from 'react-icons/bs';

interface ActionButtonProps {
  title: string;
  icon: React.ReactNode;
  onClick: (e: React.MouseEvent) => void;
  className?: string;
  variant?: 'outline' | 'solid';
}

const ActionButton: React.FC<ActionButtonProps> = ({
  title,
  icon,
  onClick,
  className = '',
  variant = 'outline'
}) => (
  <Button
    size="sm"
    variant={variant}
    onClick={onClick}
    className={`transition-all duration-200 ease-in-out p-1 ${className}`}
    title={title}
  >
    {icon}
  </Button>
);

interface ActionButtonsProps {
  onView?: (e: React.MouseEvent) => void;
  onEdit?: (e: React.MouseEvent) => void;
  onDelete?: (e: React.MouseEvent) => void;
  className?: string;
  disableView?: boolean;
  disableEdit?: boolean;
  disableDelete?: boolean;
}

export const ActionButtons: React.FC<ActionButtonsProps> = ({
  onView,
  onEdit,
  onDelete,
  className = '',
  disableView = false,
  disableEdit = false,
  disableDelete = false
}) => {
  const handleClick = (e: React.MouseEvent, action?: (e: React.MouseEvent) => void) => {
    e.stopPropagation();
    action?.(e);
  };

  return (
    <div className={`flex justify-end gap-2 ${className}`}>
      {onView && !disableView && (
        <ActionButton
          title="View"
          icon={<BsEye className="w-4 h-4" />}
          onClick={(e) => handleClick(e, onView)}
          className="hover:bg-theme-silver hover:text-theme-white hover:border-theme-silver"
        />
      )}
      {onEdit && !disableEdit && (
        <ActionButton
          title="Edit"
          icon={<BsPencil className="w-4 h-4" />}
          onClick={(e) => handleClick(e, onEdit)}
          className="hover:bg-theme-black hover:text-theme-white hover:border-theme-black"
        />
      )}
      {onDelete && !disableDelete && (
        <ActionButton
          title="Delete"
          icon={<BsTrash className="w-4 h-4" />}
          onClick={(e) => handleClick(e, onDelete)}
          className="hover:bg-red-500 hover:text-white hover:border-red-500"
        />
      )}
    </div>
  );
};