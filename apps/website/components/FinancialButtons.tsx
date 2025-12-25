// [NEW]
'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';

type FinancialSettings = {
  showDonateButton: boolean;
  showMemberDuesButton: boolean;
};

export default function FinancialButtons() {
  const [settings, setSettings] = useState<FinancialSettings | null>(null);

  useEffect(() => {
    const fetchSettings = async () => {
      try {
        const res = await fetch('/api/financial-settings');
        if (res.ok) {
          const data: FinancialSettings = await res.json();
          setSettings(data);
        }
      } catch (error) {
        console.error('Failed to fetch financial settings:', error);
      }
    };

    fetchSettings();
  }, []);

  if (!settings) {
    return null; // Don't render anything until settings are loaded
  }

  return (
    <div className="flex items-center space-x-4">
      {settings.showMemberDuesButton && (
        <Link href="/dues" passHref>
          <span className="bg-pure-white text-midnight-blue font-semibold px-4 py-2 rounded-md text-sm hover:bg-pure-white/90 transition-colors">
            Member Dues
          </span>
        </Link>
      )}
      {settings.showDonateButton && (
        <Link href="/donate" passHref>
          <span className="bg-burnished-gold text-midnight-blue font-semibold px-4 py-2 rounded-md text-sm hover:bg-burnished-gold/90 transition-colors">
            Donate
          </span>
        </Link>
      )}
    </div>
  );
}
