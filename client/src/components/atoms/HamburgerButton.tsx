interface HamburgerButtonProps {
  onClick: () => void;
}

export const HamburgerButton = ({ onClick }: HamburgerButtonProps) => {
  return (
    <button
      onClick={onClick}
      className="p-2 rounded-md text-gray-600 hover:text-primary-500 hover:bg-gray-50"
    >
      <span className="sr-only">Open menu</span>
      <svg
        className="h-6 w-6"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth={2}
          d="M4 6h16M4 12h16M4 18h16"
        />
      </svg>
    </button>
  );
};