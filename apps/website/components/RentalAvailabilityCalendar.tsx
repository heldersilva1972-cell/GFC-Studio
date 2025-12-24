// [MODIFIED]
'use client';
import { useEffect, useState } from 'react';
import { getRentalAvailability } from '@/app/lib/api';
import { DayPicker } from 'react-day-picker';
import 'react-day-picker/dist/style.css';

const RentalAvailabilityCalendar = () => {
  const [bookedDates, setBookedDates] = useState([]);

  useEffect(() => {
    const fetchAvailability = async () => {
      const data = await getRentalAvailability();
      setBookedDates(data.bookedDates.map((dateString: string) => new Date(dateString)));
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
