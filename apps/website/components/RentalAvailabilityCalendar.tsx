// [MODIFIED]
'use client';
import { useEffect, useState } from 'react';
import { getUnavailableDates } from '@/app/lib/api';
import { DayPicker } from 'react-day-picker';
import 'react-day-picker/dist/style.css';

interface RentalAvailabilityCalendarProps {
    onDateSelect: (date: Date) => void;
}

const RentalAvailabilityCalendar = ({ onDateSelect }: RentalAvailabilityCalendarProps) => {
  const [unavailableDates, setUnavailableDates] = useState<Date[]>([]);

  useEffect(() => {
    const fetchAvailability = async () => {
      const dates = await getUnavailableDates();
      setUnavailableDates(dates);
    };

    fetchAvailability();
  }, []);

  return (
    <div className="bg-midnight-blue p-6 rounded-lg shadow-lg">
      <h2 className="text-2xl font-bold text-center mb-4 text-pure-white">Select a Date</h2>
        <DayPicker
            className="text-pure-white"
            onDayClick={onDateSelect}
            disabled={unavailableDates}
            styles={{
                root: {  },
                head: { color: '#FDB813' }, // Burnished Gold for day names
                day: { color: '#FFFFFF' }, // White for day numbers

            }}
            modifiers={{
                disabled: unavailableDates
            }}
            modifiersStyles={{
                disabled: { color: '#a0a0a0', textDecoration: 'line-through' }
            }}
      />
    </div>
  );
};

export default RentalAvailabilityCalendar;
