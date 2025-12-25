// [NEW]
'use client';

import { useState, useEffect } from 'react';
import { X, Megaphone } from 'lucide-react';
import { AnimatePresence, motion } from 'framer-motion';

type Announcement = {
  id: number;
  type: 'alert' | 'news';
  message?: string;
  title?: string;
};

export default function UpdateBar() {
  const [alert, setAlert] = useState<Announcement | null>(null);
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    const fetchAnnouncements = async () => {
      try {
        const res = await fetch('/api/announcements');
        if (!res.ok) {
          throw new Error('Failed to fetch announcements');
        }
        const data: Announcement[] = await res.json();
        const latestAlert = data.find(item => item.type === 'alert') || null;

        if (latestAlert) {
            setAlert(latestAlert);
            setIsVisible(true);
        }

      } catch (error) {
        console.error('UpdateBar Error:', error);
      }
    };

    fetchAnnouncements();
  }, []);

  const handleClose = () => {
    setIsVisible(false);
  }

  return (
    <AnimatePresence>
      {isVisible && alert && (
        <motion.div
            initial={{ opacity: 0, height: 0 }}
            animate={{ opacity: 1, height: 'auto' }}
            exit={{ opacity: 0, height: 0 }}
            transition={{ duration: 0.3, ease: 'easeInOut' }}
            className="bg-burnished-gold text-midnight-blue"
        >
          <div className="container mx-auto px-4 sm:px-6 lg:px-8">
            <div className="flex items-center justify-between h-12">
              <div className="flex items-center space-x-3">
                <Megaphone className="h-5 w-5" />
                <p className="text-sm font-medium">
                  {alert.message}
                </p>
              </div>
              <button
                onClick={handleClose}
                aria-label="Dismiss announcement"
                className="p-1 rounded-md hover:bg-black/10 transition-colors"
              >
                <X className="h-5 w-5" />
              </button>
            </div>
          </div>
        </motion.div>
      )}
    </AnimatePresence>
  );
}
