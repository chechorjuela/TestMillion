import { useLocation } from 'react-router-dom';
import { motion, AnimatePresence } from 'framer-motion';
import { NavLink } from '../atoms/NavLink';

interface MobileMenuProps {
  isOpen: boolean;
  onClose: () => void;
}

const navigationItems = [
  { path: '/', label: 'Properties' },
  { path: '/owners', label: 'Owners' },
  { path: '/property-traces', label: 'Property Traces' },
];

export const MobileMenu = ({ isOpen, onClose }: MobileMenuProps) => {
  const location = useLocation();

  return (
    <AnimatePresence>
      {isOpen && (
        <>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className="fixed inset-0 bg-black bg-opacity-25 md:hidden"
            onClick={onClose}
          />
          <motion.div
            initial={{ x: '100%' }}
            animate={{ x: 0 }}
            exit={{ x: '100%' }}
            transition={{ type: 'tween' }}
            className="fixed right-0 top-0 bottom-0 w-64 bg-white shadow-lg md:hidden"
          >
            <div className="pt-5 pb-6 px-5">
              <div className="flex items-center justify-between mb-6">
                <h2 className="text-lg font-medium text-gray-900">Menu</h2>
                <button
                  type="button"
                  className="rounded-md p-2 text-gray-400 hover:text-gray-500 hover:bg-gray-100"
                  onClick={onClose}
                >
                  <span className="sr-only">Close menu</span>
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
                      d="M6 18L18 6M6 6l12 12"
                    />
                  </svg>
                </button>
              </div>
              <div className="flex flex-col space-y-1">
                {navigationItems.map((item) => {
                  const isActive = location.pathname === item.path ||
                    (item.path !== '/' && location.pathname.startsWith(item.path));
                  
                  return (
                    <NavLink
                      key={item.path}
                      to={item.path}
                      isActive={isActive}
                    >
                      {item.label}
                    </NavLink>
                  );
                })}
              </div>
            </div>
          </motion.div>
        </>
      )}
    </AnimatePresence>
  );
};