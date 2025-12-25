// [NEW]
'use client';

import { useState, useEffect } from 'react';

type StatusData = {
  status: 'Open' | 'Closed' | string;
};

export default function LiveStatusIndicator() {
  const [statusData, setStatusData] = useState<StatusData | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStatus = async () => {
      try {
        const res = await fetch('/api/club-status');
        if (!res.ok) {
          throw new Error('Failed to fetch status');
        }
        const data: StatusData = await res.json();
        setStatusData(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'An unknown error occurred');
      }
    };

    fetchStatus();
    // Fetch status every 10 seconds to keep it live
    const interval = setInterval(fetchStatus, 10000);

    return () => clearInterval(interval);
  }, []);

  if (error) {
    return (
        <div className="hidden md:flex items-center space-x-2">
            <div className="w-3 h-3 bg-yellow-500 rounded-full"></div>
            <span className="text-sm font-medium text-yellow-400">Status: Unknown</span>
        </div>
    );
  }

  if (!statusData) {
    return (
        <div className="hidden md:flex items-center space-x-2">
            <div className="w-3 h-3 bg-gray-500 rounded-full animate-pulse"></div>
            <span className="text-sm font-medium text-gray-400">Loading...</span>
        </div>
    );
  }

  const isOpen = statusData.status === 'Open';

  return (
    <div className="hidden md:flex items-center space-x-2">
      <div className="relative flex items-center justify-center w-3 h-3">
        {isOpen && <div className="absolute inline-flex w-full h-full bg-green-400 rounded-full opacity-75 animate-ping"></div>}
        <div className={`relative inline-flex w-2 h-2 ${isOpen ? 'bg-green-500' : 'bg-red-500'} rounded-full`}></div>
      </div>
      <span className={`text-sm font-medium ${isOpen ? 'text-green-400' : 'text-red-400'}`}>
        Club Status: {statusData.status}
      </span>
    </div>
  );
}
