// [NEW]
'use client';
import { useEffect, useState } from 'react';

const EventsCalendar = () => {
  const [events, setEvents] = useState([]);

  useEffect(() => {
    const fetchEvents = async () => {
      const res = await fetch('/api/content/events');
      const data = await res.json();
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
