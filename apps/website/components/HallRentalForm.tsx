// [NEW]
'use client';

import { useState, FormEvent } from 'react';
import { motion } from 'framer-motion';

interface HallRentalFormProps {
    selectedDate: Date;
    onSuccess: () => void;
}

export default function HallRentalForm({ selectedDate, onSuccess }: HallRentalFormProps) {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [details, setDetails] = useState('');
  const [status, setStatus] = useState<'idle' | 'submitting' | 'success' | 'error'>('idle');
  const [message, setMessage] = useState('');

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setStatus('submitting');
    setMessage('');

    try {
      const res = await fetch('/api/hall-rental/submit', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          requesterName: name,
          requesterEmail: email,
          requesterPhone: phone,
          eventDetails: details,
          eventDate: selectedDate,
          status: 'Pending',
        }),
      });

      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message || 'Something went wrong');
      }

      setStatus('success');
      onSuccess();

    } catch (err) {
      setStatus('error');
      setMessage(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  if (status === 'success') {
    return (
        <div className="text-center p-8 bg-green-500/10 border border-green-500 rounded-lg">
            <h3 className="font-display text-2xl font-bold text-green-400">Thank You!</h3>
            <p className="mt-2 text-green-300">Your request has been submitted successfully.</p>
        </div>
    );
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <div>
        <label htmlFor="name" className="block text-sm font-medium text-pure-white/80 mb-2">
          Your Name
        </label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
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
          value={email}
          onChange={(e) => setEmail(e.target.value)}
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
          value={phone}
          onChange={(e) => setPhone(e.target.value)}
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
          value={details}
          onChange={(e) => setDetails(e.target.value)}
          required
          className="w-full bg-midnight-blue/50 border border-burnished-gold/30 rounded-md px-4 py-2 focus:ring-burnished-gold focus:border-burnished-gold transition-colors"
        ></textarea>
      </div>

      <div>
        <motion.button
          type="submit"
          disabled={status === 'submitting'}
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          className="w-full bg-burnished-gold text-midnight-blue font-bold px-6 py-3 rounded-md hover:bg-burnished-gold/90 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {status === 'submitting' ? 'Submitting...' : 'Submit Application'}
        </motion.button>
      </div>
      {status === 'error' && <p className="text-red-400 text-sm mt-2">{message}</p>}
    </form>
  );
}
