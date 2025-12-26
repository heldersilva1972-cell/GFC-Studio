'use client';
import { useEffect, useState } from 'react';
import { getUnavailableDates } from '@/app/lib/api';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { format, addMonths, subMonths, startOfMonth, endOfMonth, startOfWeek, endOfWeek, isSameMonth, isSameDay, addDays, isBefore } from 'date-fns';

interface RentalAvailabilityCalendarProps {
  onDateSelect: (date: Date) => void;
}

const RentalAvailabilityCalendar = ({ onDateSelect }: RentalAvailabilityCalendarProps) => {
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [unavailableDates, setUnavailableDates] = useState<Date[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAvailability = async () => {
      setIsLoading(true);
      try {
        const dates = await getUnavailableDates();
        setUnavailableDates(dates);
      } catch (error) {
        console.error("Failed to load dates", error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchAvailability();
  }, []);

  // Helper to check availability status
  const getDayStatus = (date: Date) => {
    // Normalize "today" to start of day for accurate comparison
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const isPast = isBefore(date, today);
    const isUnavailableInDb = unavailableDates.some(unavailable => isSameDay(unavailable, date));

    // Status Determination logic requested by user:
    // 1. Past dates -> Disabled (opacity) but NOT "Booked"
    // 2. Future + Unavailable -> "Booked" (Red)
    // 3. Future + Available -> "Available" (Green)

    if (isPast) return 'past';
    if (isUnavailableInDb) return 'booked';
    return 'available';
  };

  const renderHeader = () => {
    return (
      <div className="flex items-center justify-between mb-8 px-4">
        <button
          onClick={() => setCurrentMonth(subMonths(currentMonth, 1))}
          className="p-2 rounded-full hover:bg-white/10 text-burnished-gold transition-colors"
        >
          <ChevronLeft size={32} />
        </button>
        <div className="text-center">
          <h2 className="text-3xl font-display font-bold text-pure-white">
            {format(currentMonth, 'MMMM yyyy')}
          </h2>
          <p className="text-pure-white/50 text-sm mt-1">Select a green available date below</p>
        </div>
        <button
          onClick={() => setCurrentMonth(addMonths(currentMonth, 1))}
          className="p-2 rounded-full hover:bg-white/10 text-burnished-gold transition-colors"
        >
          <ChevronRight size={32} />
        </button>
      </div>
    );
  };

  const renderDays = () => {
    const days = [];
    const dateFormat = "EEE";
    let startDate = startOfWeek(currentMonth);

    for (let i = 0; i < 7; i++) {
      days.push(
        <div className="text-center font-display font-bold text-burnished-gold/70 text-sm uppercase tracking-wider py-2" key={i}>
          {format(addDays(startDate, i), dateFormat)}
        </div>
      );
    }
    return <div className="grid grid-cols-7 mb-2">{days}</div>;
  };

  const renderCells = () => {
    const monthStart = startOfMonth(currentMonth);
    const monthEnd = endOfMonth(monthStart);
    const startDate = startOfWeek(monthStart);
    const endDate = endOfWeek(monthEnd);

    const dateFormat = "d";
    const rows = [];
    let days = [];
    let day = startDate;
    let formattedDate = "";

    while (day <= endDate) {
      for (let i = 0; i < 7; i++) {
        formattedDate = format(day, dateFormat);
        const cloneDay = day;

        const status = getDayStatus(day);
        const isSelected = selectedDate ? isSameDay(day, selectedDate) : false;
        const isCurrentMonth = isSameMonth(day, monthStart);

        // Styling based on status
        let cellClasses = "relative h-28 sm:h-36 border border-white/5 p-3 transition-all duration-300 group ";

        // Backgrounds
        if (!isCurrentMonth) {
          cellClasses += "bg-black/60 opacity-30 ";
        } else if (status === 'past') {
          cellClasses += "bg-black/40 opacity-40 cursor-not-allowed ";
        } else if (status === 'booked') {
          cellClasses += "bg-red-900/10 cursor-not-allowed border-red-500/10 ";
        } else {
          // Available
          cellClasses += "bg-white/5 backdrop-blur-sm cursor-pointer hover:bg-emerald-900/10 hover:border-emerald-500/30 hover:shadow-[0_0_15px_rgba(16,185,129,0.1)] ";
        }

        // Selected State
        if (isSelected) {
          cellClasses += "ring-2 ring-emerald-500 bg-emerald-900/20 z-10 shadow-[0_0_20px_rgba(16,185,129,0.3)] ";
        }

        days.push(
          <div
            key={day.toString()}
            className={cellClasses}
            onClick={() => {
              if (status === 'available') {
                setSelectedDate(cloneDay);
                onDateSelect(cloneDay);
              }
            }}
          >
            {/* Day Number */}
            <div className={`text-right font-display font-medium text-lg mb-2 ${!isCurrentMonth || status === 'past' ? 'text-white/20' : 'text-white/80'}`}>
              {formattedDate}
            </div>

            {/* Status Indicator */}
            <div className="flex flex-col items-center justify-center h-[calc(100%-32px)]">
              {isCurrentMonth && status !== 'past' && (
                <>
                  {status === 'booked' ? (
                    <span className="flex items-center gap-1 text-xs font-bold text-red-400 bg-red-500/10 border border-red-500/20 px-3 py-1.5 rounded-full uppercase tracking-wider">
                      Booked
                    </span>
                  ) : (
                    <div className="flex flex-col items-center gap-2">
                      <div className={`w-3 h-3 rounded-full bg-emerald-500 shadow-[0_0_8px_rgba(16,185,129,0.8)] ${isSelected ? 'scale-125' : 'group-hover:scale-110'} transition-transform`}></div>
                      <span className="text-xs font-bold text-emerald-400 uppercase tracking-widest opacity-80 group-hover:opacity-100">
                        Available
                      </span>
                    </div>
                  )}
                </>
              )}
            </div>
          </div>
        );
        day = addDays(day, 1);
      }
      rows.push(
        <div className="grid grid-cols-7 gap-px lg:gap-1 bg-white/5 border border-white/5 rounded-xl overflow-hidden shadow-2xl" key={day.toString()}>
          {days}
        </div>
      );
      days = [];
    }
    return <div className="animate-fade-in">{rows}</div>;
  };

  if (isLoading) {
    return (
      <div className="h-96 flex items-center justify-center text-burnished-gold animate-pulse">
        Loading calendar...
      </div>
    );
  }

  return (
    <div className="bg-midnight-blue p-4 sm:p-8 rounded-2xl shadow-2xl border border-white/10 max-w-5xl mx-auto">
      {renderHeader()}
      {renderDays()}
      {renderCells()}
    </div>
  );
};

export default RentalAvailabilityCalendar;
