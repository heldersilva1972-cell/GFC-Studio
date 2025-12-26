// [MODIFIED]
'use client';

import { useState, FormEvent, useEffect } from 'react';
import { motion } from 'framer-motion';

interface HallRentalFormProps {
    selectedDate: Date;
    onSuccess: (data: { name: string; email: string; phone: string; details: string; }) => void;
}

export default function HallRentalForm({ selectedDate, onSuccess }: HallRentalFormProps) {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phone: '',
    details: '',
  });
  const [status, setStatus] = useState<'idle' | 'submitting' | 'saving' | 'success' | 'error' | 'saved'>('idle');
  const [message, setMessage] = useState('');

  useEffect(() => {
    // Check for a resume token in the URL
    const urlParams = new URLSearchParams(window.location.search);
    const resumeToken = urlParams.get('resume');
    if (resumeToken) {
      const fetchInquiry = async () => {
        try {
          const res = await fetch(`/api/HallRentalInquiry/resume/${resumeToken}`);
          if (res.ok) {
            const data = await res.json();
            setFormData(data);
          }
        } catch (error) {
          console.error('Failed to fetch inquiry data:', error);
        }
      };
      fetchInquiry();
    }
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { id, value } = e.target;
    setFormData((prev) => ({ ...prev, [id]: value }));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setStatus('submitting');
    setMessage('');

    try {
      const res = await fetch('/api/HallRentalInquiry', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          requesterName: formData.name,
          requesterEmail: formData.email,
          requesterPhone: formData.phone,
          eventDetails: formData.details,
          requestedDate: selectedDate,
          status: 'Pending',
        }),
      });

      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message || 'Something went wrong');
      }

      setStatus('success');
      onSuccess(formData);

    } catch (err) {
      setStatus('error');
      setMessage(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  const handleSave = async () => {
    setStatus('saving');
    setMessage('');

    try {
      const res = await fetch('/api/HallRentalInquiry/save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
      });

      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message || 'Something went wrong');
      }

      setStatus('saved');
      setMessage('Your application has been saved. A link to resume has been sent to your email.');

    } catch (err) {
      setStatus('error');
      setMessage(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <div>
        <label htmlFor="name" className="block text-sm font-medium text-pure-white/80 mb-2">
          Your Name
        </label>
        <input
          type="text"
          id="name"
          value={formData.name}
          onChange={handleChange}
          required
          className="w-full bg-midnight-blue/50 border border-burnished-gold/30 rounded-md px-4 py-2 focus:ring-burnished-gold focus:border-burnished-gold transition-colors"
        />
      </div>

      <div>
        <label htmlFor="email" className="block text-sm font-medium text-pure-white/80 mb-2">
          Email
        </label>
        <input
          type="email"
          id="email"
          value={formData.email}
          onChange={handleChange}
          required
          className="w-full bg-midnight-blue/50 border border-burnished-gold/30 rounded-md px-4 py-2 focus:ring-burnished-gold focus:border-burnished-gold transition-colors"
        />
      </div>

      <div>
        <label htmlFor="phone" className="block text-sm font-medium text-pure-white/80 mb-2">
          Phone
        </label>
        <input
          type="tel"
          id="phone"
          value={formData.phone}
          onChange={handleChange}
          required
          className="w-full bg-midnight-blue/50 border border-burnished-gold/30 rounded-md px-4 py-2 focus:ring-burnished-gold focus:border-burnished-gold transition-colors"
        />
      </div>

      <div>
        <label htmlFor="details" className="block text-sm font-medium text-pure-white/80 mb-2">
          Event Details
        </label>
        <textarea
          id="details"
          rows={4}
          value={formData.details}
          onChange={handleChange}
          required
          className="w-full bg-midnight-blue/50 border border-burnished-gold/30 rounded-md px-4 py-2 focus:ring-burnished-gold focus:border-burnished-gold transition-colors"
        ></textarea>
      </div>

      <div className="flex space-x-4">
        <motion.button
          type="button"
          onClick={handleSave}
          disabled={status === 'saving' || status === 'submitting'}
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          className="w-full bg-gray-500 text-white font-bold px-6 py-3 rounded-md hover:bg-gray-600 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {status === 'saving' ? 'Saving...' : 'Save & Resume Later'}
        </motion.button>
        <motion.button
          type="submit"
          disabled={status === 'submitting' || status === 'saving'}
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          className="w-full bg-burnished-gold text-midnight-blue font-bold px-6 py-3 rounded-md hover:bg-burnished-gold/90 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {status === 'submitting' ? 'Submitting...' : 'Submit Application'}
        </motion.button>
      </div>
      {status === 'error' && <p className="text-red-400 text-sm mt-2">{message}</p>}
      {status === 'saved' && <p className="text-green-400 text-sm mt-2">{message}</p>}
    </form>
  );
}
