import { Link } from 'react-router-dom';
import { motion } from 'framer-motion';
import { clsx } from 'clsx';

interface NavLinkProps {
  to: string;
  isActive: boolean;
  children: React.ReactNode;
}

export const NavLink = ({ to, isActive, children }: NavLinkProps) => {
  return (
    <Link
      to={to}
      className={clsx(
        'px-3 py-2 rounded-md text-sm font-medium transition-colors relative',
        isActive
          ? 'text-theme-black'
          : 'text-theme-medium-gray hover:text-theme-black hover:bg-theme-silver/10'
      )}
    >
      {children}
      {isActive && (
        <motion.div
          layoutId="navigation-underline"
          className="absolute bottom-0 left-0 right-0 h-0.5 bg-theme-silver"
          initial={false}
        />
      )}
    </Link>
  );
};