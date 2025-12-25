// [NEW]
'use client';

import { useEffect, useState } from 'react';

export function useTheme() {
  const [theme, setTheme] = useState({
    colors: {
        primary: '#0D1B2A',
        secondary: '#E0E1DD',
        accent: '#FFD700',
    },
    typography: {
        headingFont: 'Outfit',
        bodyFont: 'Inter',
    },
    accessibility: {
        highContrast: false,
    }
  });

  useEffect(() => {
    const handleMessage = (event) => {
      // TODO: Add origin check for security
      const { type, payload } = event.data;

      if (type === 'THEME_UPDATE' && payload) {
        setTheme(payload);

        // Apply theme to the document root
        const root = document.documentElement;
        root.style.setProperty('--primary-color', payload.colors.primary);
        root.style.setProperty('--secondary-color', payload.colors.secondary);
        root.style.setProperty('--accent-color', payload.colors.accent);
        root.style.setProperty('--font-heading', payload.typography.headingFont);
        root.style.setProperty('--font-body', payload.typography.bodyFont);

        if (payload.accessibility.highContrast) {
            document.body.classList.add('high-contrast');
        } else {
            document.body.classList.remove('high-contrast');
        }
      }
    };

    window.addEventListener('message', handleMessage);

    return () => {
      window.removeEventListener('message', handleMessage);
    };
  }, []);

  return theme;
}
