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
    <div>
      <h2>Upcoming Events</h2>
      <div>
        {events.map((event, index) => (
          <div key={index}>
            <h3>{event.title}</h3>
            <p>{new Date(event.eventDate).toLocaleDateString()}</p>
            <p>{event.description}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default EventsCalendar;
