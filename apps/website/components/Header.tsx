// [MODIFIED]
'use client';

import { useState } from 'react';
import Link from 'next/link';
import { Menu, X } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import LiveStatusIndicator from './LiveStatusIndicator';
import FinancialButtons from './FinancialButtons';

const navItems = [
  { name: 'Home', href: '/' },
  { name: 'Hall Rentals', href: '/hall-rentals' },
  { name: 'Events', href: '/events' },
  { name: 'Membership', href: '/membership' },
  { name: 'Gallery', href: '/gallery' },
  { name: 'Contact', href: '/contact' },
];

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);

  return (
    <>
      <header className="sticky top-0 z-50 bg-midnight-blue/80 backdrop-blur-sm border-b border-burnished-gold/20">
        <div className="container mx-auto px-4 sm:px-6 lg:px-8">
          <nav className="flex items-center justify-between h-20">
            {/* Logo */}
            <Link href="/" className="flex items-center space-x-2">
              <span className="font-display text-3xl font-bold text-burnished-gold">
                GFC
              </span>
              <span className="hidden sm:block font-sans text-sm text-pure-white/80">
                Gloucester Fraternity Club
              </span>
            </Link>

            {/* Live Status Indicator */}
            <LiveStatusIndicator />

            {/* Desktop Navigation */}
            <ul className="hidden lg:flex items-center space-x-8">
              {navItems.map((item) => (
                <li key={item.name}>
                  <Link
                    href={item.href}
                    className="font-sans text-sm font-medium text-pure-white/80 hover:text-burnished-gold transition-colors duration-300 relative after:content-[''] after:absolute after:left-0 after:bottom-[-4px] after:w-0 after:h-[2px] after:bg-burnished-gold after:transition-all after:duration-300 hover:after:w-full"
                  >
                    {item.name}
                  </Link>
                </li>
              ))}
            </ul>

            <div className="hidden lg:flex">
              <FinancialButtons />
            </div>

            {/* Mobile Menu Button */}
            <button
              className="lg:hidden text-pure-white"
              onClick={toggleMenu}
              aria-label="Toggle menu"
            >
              {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
            </button>
          </nav>
        </div>
      </header>

      {/* Mobile Navigation Drawer */}
      <AnimatePresence>
        {isMenuOpen && (
          <motion.div
            initial={{ opacity: 0, y: -50 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -50 }}
            className="fixed inset-0 z-40 bg-midnight-blue lg:hidden"
          >
            <div className="container mx-auto px-4 sm:px-6 lg:px-8 pt-24 pb-8 h-full flex flex-col">
              <ul className="flex flex-col items-center justify-center flex-grow space-y-8">
                {navItems.map((item) => (
                  <li key={item.name}>
                    <Link
                      href={item.href}
                      className="font-display text-3xl font-semibold text-pure-white hover:text-burnished-gold transition-colors duration-300"
                      onClick={toggleMenu}
                    >
                      {item.name}
                    </Link>
                  </li>
                ))}
              </ul>
              <div className="lg:hidden flex justify-center pb-8">
                  <FinancialButtons />
              </div>
            </div>
          </motion.div>
        )}
      </AnimatePresence>
    </>
  );
}
