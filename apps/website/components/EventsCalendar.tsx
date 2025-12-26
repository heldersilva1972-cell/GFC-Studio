// [MODIFIED]
'use client';
import { useEffect, useState } from 'react';
import { getEvents } from '@/app/lib/api';

interface Event {
  title: string;
  eventDate: string;
  description: string;
}

const EventsCalendar = () => {
  const [events, setEvents] = useState<Event[]>([]);

  useEffect(() => {
    const fetchEvents = async () => {
      const data = await getEvents();
      setEvents(data);
    };

    fetchEvents();
  }, []);

  return (
    <div className="bg-white/5 p-6 rounded-lg shadow-lg border border-white/10">
      <h2 className="text-2xl font-bold mb-6 text-burnished-gold font-display">Upcoming Events</h2>
      <div className="space-y-6">
        {events.length === 0 ? (
          <p className="text-pure-white/60 italic">No upcoming events scheduled.</p>
        ) : (
          events.map((event, index) => (
            <div key={index} className="border-b border-white/10 pb-4 last:border-0 last:pb-0">
              <div className="flex justify-between items-start mb-2">
                <h3 className="text-xl font-bold text-pure-white">{event.title}</h3>
                <span className="text-success-green text-sm font-mono py-1 px-2 bg-success-green/10 rounded">
                  {new Date(event.eventDate).toLocaleDateString()}
                </span>
              </div>
              <p className="text-pure-white/80 leading-relaxed font-sans">{event.description}</p>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default EventsCalendar;
