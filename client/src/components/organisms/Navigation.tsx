import { useState } from 'react';
import { Logo } from '../atoms/Logo';
import { HamburgerButton } from '../atoms/HamburgerButton';
import { NavLinks } from '../molecules/NavLinks';
import { MobileMenu } from '../molecules/MobileMenu';

export const Navigation = () => {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  return (
    <nav className="bg-theme-white shadow-md border-b border-theme-silver/10">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          <Logo />
          
          <NavLinks />
          
          <div className="md:hidden">
            <HamburgerButton onClick={() => setIsMobileMenuOpen(true)} />
          </div>
        </div>
      </div>

      <MobileMenu
        isOpen={isMobileMenuOpen}
        onClose={() => setIsMobileMenuOpen(false)}
      />
    </nav>
  );
};