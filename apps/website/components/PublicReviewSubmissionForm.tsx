// [NEW]
'use client';

import { useState, FormEvent } from 'react';
import { motion } from 'framer-motion';
import { Star } from 'lucide-react';

export default function PublicReviewSubmissionForm() {
  const [name, setName] = useState('');
  const [rating, setRating] = useState(0);
  const [hoverRating, setHoverRating] = useState(0);
  const [comment, setComment] = useState('');
  const [status, setStatus] = useState<'idle' | 'submitting' | 'success' | 'error'>('idle');
  const [message, setMessage] = useState('');

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setStatus('submitting');
    setMessage('');

    try {
      const res = await fetch('/api/reviews', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, rating, comment }),
      });

      const data = await res.json();

      if (!res.ok) {
        throw new Error(data.message || 'Something went wrong');
      }

      setStatus('success');
      setMessage(data.message);
      setName('');
      setRating(0);
      setComment('');

    } catch (err) {
      setStatus('error');
      setMessage(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  if (status === 'success') {
    return (
        <div className="text-center p-8 bg-green-500/10 border border-green-500 rounded-lg">
            <h3 className="font-display text-2xl font-bold text-green-400">Thank You!</h3>
            <p className="mt-2 text-green-300">{message}</p>
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
        <label className="block text-sm font-medium text-pure-white/80 mb-2">Rating</label>
        <div className="flex space-x-1">
          {[1, 2, 3, 4, 5].map((star) => (
            <Star
              key={star}
              className={`cursor-pointer transition-colors ${
                (hoverRating || rating) >= star ? 'text-burnished-gold' : 'text-pure-white/30'
              }`}
              onClick={() => setRating(star)}
              onMouseEnter={() => setHoverRating(star)}
              onMouseLeave={() => setHoverRating(0)}
            />
          ))}
        </div>
      </div>

      <div>
        <label htmlFor="comment" className="block text-sm font-medium text-pure-white/80 mb-2">
          Comment
        </label>
        <textarea
          id="comment"
          rows={4}
          value={comment}
          onChange={(e) => setComment(e.target.value)}
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
          {status === 'submitting' ? 'Submitting...' : 'Submit Review'}
        </motion.button>
      </div>
      {status === 'error' && <p className="text-red-400 text-sm mt-2">{message}</p>}
    </form>
  );
}
