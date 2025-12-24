// [NEW]
'use client';
import { useEffect, useState } from 'react';
import { DayPicker } from 'react-day-picker';
import 'react-day-picker/dist/style.css';

const RentalAvailabilityCalendar = () => {
  const [bookedDates, setBookedDates] = useState([]);

  useEffect(() => {
    const fetchAvailability = async () => {
      const res = await fetch('/api/content/rental-availability');
      const data = await res.json();
      setBookedDates(data.map((item: any) => new Date(item.date)));
    };

    fetchAvailability();
  }, []);

  return (
    <div>
      <h2>Rental Availability</h2>
      <DayPicker
        modifiers={{ booked: bookedDates }}
        modifiersStyles={{ booked: { color: 'red' } }}
      />
    </div>
  );
};

export default RentalAvailabilityCalendar;
