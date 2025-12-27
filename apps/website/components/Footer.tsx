
'use client';
import { useState, useEffect } from 'react';
import Link from 'next/link';
import { Facebook, Instagram, Twitter, Mail, Phone, MapPin } from 'lucide-react';

const socialLinks = [
  { icon: Facebook, href: 'https://www.facebook.com/GloucesterFraternityClub/', name: 'Facebook' },
  { icon: Instagram, href: 'https://instagram.com/gfc9651/', name: 'Instagram' },
  { icon: Twitter, href: 'https://twitter.com/GFC_club/', name: 'Twitter' },
];

const quickLinks = [
  { name: 'Hall Rentals', href: '/hall-rentals' },
  { name: 'Events', href: '/events' },
  { name: 'Membership', href: '/membership' },
  { name: 'Contact Us', href: '/contact' },
];

const moreLinks = [
  { name: 'Photo Gallery', href: '/gallery' },
  { name: 'About Us', href: '/about' },
  { name: 'Member Login', href: 'http://localhost:5000' }, // Link to Web App
  { name: 'Privacy Policy', href: '/privacy-policy' },
  { name: 'Terms of Service', href: '/terms-of-service' },
  { name: 'Hall Rental Rules', href: '/hall-rental-rules' },
]

interface FooterProps {
  highAccessibilityMode?: boolean;
  largeTextMode?: boolean;
}

export default function Footer({ highAccessibilityMode, largeTextMode }: FooterProps) {
  const currentYear = new Date().getFullYear();

  const toggleA11yMode = async () => {
    const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207';
    try {
      await fetch(`${API_URL}/api/WebsiteSettings/toggle-accessibility`, {
        method: 'POST',
      });
      window.location.reload();
    } catch (error) {
      console.error('Failed to toggle accessibility mode:', error);
    }
  };

  const toggleLargeTextMode = async () => {
    const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207';
    try {
      await fetch(`${API_URL}/api/WebsiteSettings/toggle-large-text`, {
        method: 'POST',
      });
      window.location.reload();
    } catch (error) {
      console.error('Failed to toggle large text mode:', error);
    }
  };

  return (
    <footer className="bg-midnight-blue border-t border-burnished-gold/20 text-pure-white/60">
      <div className="container mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {/* About Section */}
          <div className="md:col-span-2 lg:col-span-1">
            <h3 className="font-display text-2xl font-bold text-burnished-gold mb-4">
              GFC
            </h3>
            <p className="text-sm">
              Building community, friendship, and tradition since 1923. Proudly serving the Cape Ann area.
            </p>
            <div className="flex space-x-4 mt-6">
              {socialLinks.map((social) => (
                <a
                  key={social.name}
                  href={social.href}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="hover:text-burnished-gold transition-colors duration-300"
                >
                  <social.icon size={20} />
                  <span className="sr-only">{social.name}</span>
                </a>
              ))}
            </div>
          </div>

          {/* Quick Links */}
          <div>
            <h4 className="font-display text-lg font-semibold text-pure-white mb-4">
              Quick Links
            </h4>
            <ul className="space-y-2">
              {quickLinks.map((link) => (
                <li key={link.name}>
                  <Link href={link.href} className="text-sm hover:text-burnished-gold transition-colors duration-300">
                    {link.name}
                  </Link>
                </li>
              ))}
            </ul>
          </div>

          {/* More Links */}
          <div>
            <h4 className="font-display text-lg font-semibold text-pure-white mb-4">
              More
            </h4>
            <ul className="space-y-2">
              {moreLinks.map((link) => (
                <li key={link.name}>
                  <a href={link.href} className="text-sm hover:text-burnished-gold transition-colors duration-300">
                    {link.name}
                  </a>
                </li>
              ))}
            </ul>
          </div>

          {/* Contact Section */}
          <div>
            <h4 className="font-display text-lg font-semibold text-pure-white mb-4">
              Contact
            </h4>
            <ul className="space-y-3 text-sm">
              <li className="flex items-start space-x-3">
                <MapPin size={16} className="mt-1 flex-shrink-0" />
                <span>27 Webster Street<br />Gloucester, MA 01930</span>
              </li>
              <li className="flex items-center space-x-3">
                <Phone size={16} />
                <a href="tel:+19782832889" className="hover:text-burnished-gold transition-colors duration-300">(978) 283-2889</a>
              </li>
              <li className="flex items-center space-x-3">
                <Mail size={16} />
                <a href="mailto:gfc@gloucesterfraternityclub.com" className="hover:text-burnished-gold transition-colors duration-300">
                  gfc@gloucesterfraternityclub.com
                </a>
              </li>
            </ul>
          </div>
        </div>

        <div className="mt-12 border-t border-burnished-gold/20 pt-8 flex flex-col sm:flex-row justify-between items-center">
          <p className="text-sm text-center sm:text-left">
            © {currentYear} Gloucester Fraternity Club. All rights reserved.
          </p>
          <div className="flex flex-col sm:flex-row sm:space-x-4 mt-4 sm:mt-0">
            <button onClick={toggleA11yMode} className="text-sm underline hover:text-burnished-gold transition-colors duration-300">
              {highAccessibilityMode ? 'Disable High Accessibility Mode' : 'Enable High Accessibility Mode'}
            </button>
            <button onClick={toggleLargeTextMode} className="text-sm underline hover:text-burnished-gold transition-colors duration-300 mt-2 sm:mt-0">
              {largeTextMode ? 'Disable Senior-Friendly Mode' : 'Enable Senior-Friendly Mode'}
            </button>
          </div>
          <p className="text-sm mt-4 sm:mt-0">
            Website designed with modern legacy.
          </p>
        </div>
      </div>
    </footer>
  );
}
