// [NEW]
'use client';

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';

type Announcement = {
  id: number;
  type: 'alert' | 'news';
  message?: string;
  title?: string;
  content?: string;
};

const cardVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: (i: number) => ({
      opacity: 1,
      y: 0,
      transition: {
        delay: i * 0.1,
        duration: 0.5,
        ease: 'easeOut',
      },
    }),
};

export default function HomepageNewsModule() {
  const [news, setNews] = useState<Announcement[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchNews = async () => {
      try {
        const res = await fetch('/api/announcements');
        if (!res.ok) {
          throw new Error('Failed to fetch news');
        }
        const data: Announcement[] = await res.json();
        const newsItems = data.filter(item => item.type === 'news');
        setNews(newsItems);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'An unknown error occurred');
      } finally {
        setIsLoading(false);
      }
    };

    fetchNews();
  }, []);

  if (isLoading) {
    return (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {[...Array(3)].map((_, i) => (
                <div key={i} className="bg-midnight-blue/50 border border-burnished-gold/20 p-6 rounded-lg animate-pulse">
                    <div className="h-6 bg-burnished-gold/20 rounded w-3/4 mb-4"></div>
                    <div className="h-4 bg-pure-white/20 rounded w-full mb-2"></div>
                    <div className="h-4 bg-pure-white/20 rounded w-5/6"></div>
                </div>
            ))}
        </div>
    );
  }

  if (error) {
    return <p className="text-red-400">Failed to load news: {error}</p>;
  }

  if (news.length === 0) {
    return <p>No news to display at the moment.</p>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
      {news.map((item, i) => (
        <motion.div
            key={item.id}
            custom={i}
            variants={cardVariants}
            initial="hidden"
            animate="visible"
            className="bg-midnight-blue/50 border border-burnished-gold/20 p-6 rounded-lg shadow-lg hover:shadow-burnished-gold/10 hover:border-burnished-gold/50 transition-all duration-300"
        >
          <h3 className="font-display text-xl font-bold text-burnished-gold mb-2">
            {item.title}
          </h3>
          <p className="text-pure-white/80 text-sm">{item.content}</p>
        </motion.div>
      ))}
    </div>
  );
}
