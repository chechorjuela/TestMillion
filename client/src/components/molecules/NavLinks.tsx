import { useLocation } from 'react-router-dom';
import { NavLink } from '../atoms/NavLink';

const navigationItems = [
  { path: '/properties', label: 'Properties' },
  { path: '/owners', label: 'Owners' },
  { path: '/property-traces', label: 'Property Traces' },
  { path: '/property-images', label: 'Property Images' },
];

export const NavLinks = () => {
  const location = useLocation();

  return (
    <div className="hidden md:flex items-center space-x-4">
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
  );
};