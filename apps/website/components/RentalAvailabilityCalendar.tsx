'use client';

import { useEffect, useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { getUnavailableDates } from '@/app/lib/api';
import { ChevronLeft, ChevronRight, Lock, Check, X } from 'lucide-react';
import {
  format, addMonths, subMonths, startOfMonth,
  endOfMonth, startOfWeek, endOfWeek, isSameMonth,
  isSameDay, addDays, isBefore
} from 'date-fns';

interface RentalAvailabilityCalendarProps {
  onDateSelect: (date: Date) => void;
  newlyBookedDate?: Date | null;
}

// --- MOTION VARIANTS ---

const ringVariants = {
  idle: {
    scale: 1,
    opacity: 0.5,
    boxShadow: "0 0 0 1px rgba(16,185,129,0.3)",
    transition: {
      duration: 3,
      repeat: Infinity,
      repeatType: "reverse" as const,
      ease: "easeInOut"
    }
  },
  hover: {
    scale: 1.05,
    opacity: 1,
    borderColor: "rgba(16,185,129,1)",
    boxShadow: "0 0 10px 0 rgba(16,185,129,0.5)",
    transition: { duration: 0.3 }
  },
  selected: {
    scale: 0.95,
    opacity: 1,
    borderColor: "rgba(16,185,129,1)",
    boxShadow: "0 0 0 2px rgba(16,185,129,0.8)",
  }
};

const iconVariants = {
  idle: { rotate: 0 },
  hover: { rotate: 90, scale: 1.1, transition: { type: "spring", stiffness: 300 } }
};

const lockVariants = {
  idle: { opacity: 0.6 },
  hover: {
    x: [0, -2, 2, -2, 2, 0],
    transition: { duration: 0.4 }
  }
};

const rippleVariants = {
  initial: { scale: 0, opacity: 0.8 },
  animate: {
    scale: 4,
    opacity: 0,
    transition: { duration: 1.5, ease: "easeOut" }
  }
};

// --- SUB-COMPONENTS ---

const DayTile = ({ date, status, onClick, isCurrentMonth, isNewlyBooked, eventType, eventTime, isPast }: any) => {
  const formattedDay = format(date, 'd');

  if (!isCurrentMonth) {
    return <div className="h-28 sm:h-36 bg-black/40 border border-white/5 opacity-20"></div>;
  }

  // 1. PAST DATES (Muted, Disabled, No Scary Red/Lock)
  if (status === 'past') {
    return (
      <div className="relative h-28 sm:h-36 bg-black/20 border border-white/5 p-3 opacity-40 cursor-not-allowed">
        <div className="absolute top-2 right-2 text-white/20 text-lg font-display">{formattedDay}</div>
        <div className="flex flex-col items-center justify-center h-full gap-2">
          <span className="text-[10px] uppercase tracking-widest text-white/20 font-medium">Past</span>
        </div>
      </div>
    );
  }

  // Handle Booked dates that are also "past" - show details but keep it muted?
  // Or just show them as booked. The user wants to see the event and time.
  const isPastBooking = (status === 'pending' || status === 'booked') && isPast;
  const opacityClass = isPastBooking ? 'opacity-50' : '';

  // 2. NEWLY BOOKED (Success State) - PULSING ANIMATION!
  if (isNewlyBooked) {
    return (
      <motion.div
        className="relative h-28 sm:h-36 bg-gradient-to-br from-emerald-500/30 to-emerald-600/20 border-2 border-emerald-400 p-3 overflow-hidden cursor-default"
        animate={{
          scale: [1, 1.05, 1],
          boxShadow: [
            '0 0 20px rgba(52, 211, 153, 0.6)',
            '0 0 40px rgba(52, 211, 153, 0.9)',
            '0 0 20px rgba(52, 211, 153, 0.6)'
          ],
          borderColor: [
            'rgba(52, 211, 153, 0.8)',
            'rgba(52, 211, 153, 1)',
            'rgba(52, 211, 153, 0.8)'
          ]
        }}
        transition={{
          duration: 2,
          repeat: Infinity,
          ease: 'easeInOut'
        }}
      >
        {/* Ripple Effect */}
        <motion.div
          className="absolute inset-0 bg-emerald-500/20"
          variants={rippleVariants}
          initial="initial"
          animate="animate"
          style={{ originX: 0.5, originY: 0.5 }}
        />

        {/* Shimmer overlay */}
        <motion.div
          className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent"
          animate={{ x: ['-100%', '200%'] }}
          transition={{ duration: 2, repeat: Infinity, ease: 'linear' }}
        />

        {/* Particle burst */}
        {[...Array(8)].map((_, i) => {
          const angle = (i / 8) * Math.PI * 2;
          return (
            <motion.div
              key={i}
              className="absolute w-1 h-1 bg-emerald-400 rounded-full"
              style={{
                left: '50%',
                top: '50%'
              }}
              animate={{
                x: Math.cos(angle) * 30,
                y: Math.sin(angle) * 30,
                opacity: [0, 1, 0],
                scale: [0, 1.5, 0]
              }}
              transition={{
                duration: 2,
                repeat: Infinity,
                delay: i * 0.1
              }}
            />
          );
        })}

        <div className="absolute top-2 right-2 text-emerald-400 text-lg font-display font-bold">{formattedDay}</div>
        <div className="flex flex-col items-center justify-center h-full gap-2 relative z-10">
          <motion.div
            initial={{ scale: 0 }}
            animate={{ scale: 1 }}
            transition={{ type: "spring", delay: 0.5 }}
            className="relative"
          >
            {/* Rotating ring */}
            <motion.div
              className="absolute inset-0 rounded-full border-2 border-emerald-400/50"
              animate={{ rotate: 360 }}
              transition={{ duration: 3, repeat: Infinity, ease: 'linear' }}
              style={{ width: 40, height: 40, left: -4, top: -4 }}
            />
            <div className="w-8 h-8 rounded-full bg-emerald-500 flex items-center justify-center shadow-lg shadow-emerald-500/50 relative z-10">
              <Check size={18} className="text-white font-bold" />
            </div>
          </motion.div>
          <motion.span
            className="text-[10px] font-bold text-emerald-400 uppercase tracking-widest mt-1"
            animate={{ opacity: [0.7, 1, 0.7] }}
            transition={{ duration: 1.5, repeat: Infinity }}
          >
            Your Booking
          </motion.span>
          <span className="text-[9px] text-emerald-200/60">(Pending Review)</span>
        </div>
      </motion.div>
    );
  }

  // 3. PENDING (Awaiting Approval) - NEW !
  if (status === 'pending') {
    return (
      <motion.div
        className={`relative h-28 sm:h-36 bg-white/5 border border-burnished-gold/20 p-2 overflow-hidden ${opacityClass} group`}
        initial="idle"
        whileHover="hover"
      >
        <div className="absolute top-1 right-1 text-burnished-gold/20 text-sm font-display font-bold">
          {formattedDay}
        </div>
        <div className="flex flex-col items-center justify-center h-full gap-1">
          <motion.div variants={lockVariants}>
            <div className="w-8 h-8 rounded-full bg-burnished-gold/5 flex items-center justify-center mb-1 border border-burnished-gold/10 group-hover:border-burnished-gold/40 transition-colors">
              <Lock size={14} className="text-burnished-gold/40" />
            </div>
          </motion.div>
          <span className="text-[9px] uppercase tracking-widest text-burnished-gold/40 font-bold">Pending</span>
          <div className="text-center mt-1 w-full">
            <p className="text-[11px] text-burnished-gold/80 font-bold leading-tight px-1 line-clamp-2">
              {eventType || 'Private Event'}
            </p>
            {eventTime && (
              <p className="text-[10px] text-burnished-gold/40 font-medium mt-1">
                <span className="opacity-70">🕒</span> {eventTime}
              </p>
            )}
          </div>
        </div>
      </motion.div>
    );
  }

  // 4. BLACKOUT (Club Closure / Maintenance)
  if (status === 'blackout') {
    return (
      <motion.div
        className={`relative h-28 sm:h-36 bg-blue-900/10 border border-blue-500/30 p-2 overflow-hidden ${opacityClass} group`}
        initial="idle"
        whileHover="hover"
      >
        <div className="absolute top-1 right-1 text-blue-400/30 text-sm font-display font-bold">{formattedDay}</div>
        <div className="flex flex-col items-center justify-center h-full gap-1">
          <motion.div variants={lockVariants}>
            <div className="w-9 h-9 rounded-full bg-blue-500/10 flex items-center justify-center mb-1 border border-blue-500/20 group-hover:border-blue-500/50 transition-all">
              <svg className="w-4 h-4 text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z"></path></svg>
            </div>
          </motion.div>
          <span className="text-[10px] uppercase tracking-widest text-blue-400/60 font-bold text-center">Club Event</span>
          <div className="text-center mt-1 w-full">
            <p className="text-[12px] text-blue-100 font-bold leading-tight px-1 line-clamp-2">
              {eventType || 'Private Event'}
            </p>
            {eventTime && (
              <p className="text-[10px] text-blue-300/70 font-medium mt-1">
                <span className="opacity-70">🕒</span> {eventTime}
              </p>
            )}
          </div>
        </div>
      </motion.div>
    );
  }

  // 5. BOOKED (Confirmed/Unavailable)
  if (status === 'booked') {
    return (
      <motion.div
        className={`relative h-28 sm:h-36 bg-gradient-to-br from-burnished-gold/5 to-transparent border border-burnished-gold/40 p-2 overflow-hidden ${opacityClass} shadow-[inset_0_0_20px_rgba(212,175,55,0.05)] group`}
        initial="idle"
        whileHover="hover"
      >
        <div className="absolute top-1 right-1 text-burnished-gold/40 text-sm font-display font-bold">{formattedDay}</div>
        <div className="flex flex-col items-center justify-center h-full gap-1">
          <motion.div variants={lockVariants}>
            <div className="w-9 h-9 rounded-full bg-burnished-gold/10 flex items-center justify-center mb-1 border border-burnished-gold/30 shadow-[0_0_15px_rgba(212,175,55,0.1)] group-hover:shadow-[0_0_20px_rgba(212,175,55,0.2)] transition-all">
              <Lock size={16} className="text-burnished-gold" />
            </div>
          </motion.div>
          <span className="text-[10px] uppercase tracking-widest text-burnished-gold/80 font-bold">Reserved</span>
          <div className="text-center mt-1 w-full">
            <p className="text-[12px] text-white font-bold leading-tight px-1 line-clamp-2">
              {eventType || 'Private Event'}
            </p>
            {eventTime && (
              <p className="text-[10px] text-burnished-gold/70 font-medium mt-1">
                <span className="opacity-70">🕒</span> {eventTime}
              </p>
            )}
          </div>
        </div>
      </motion.div>
    );
  }

  // 5. AVAILABLE (Standard)
  return (
    <motion.div
      // ... (rest of available block, no changes) ...
      className="relative h-28 sm:h-36 bg-white/5 backdrop-blur-sm cursor-pointer border border-white/10 overflow-hidden group"
      initial="idle"
      whileHover="hover"
      whileTap="selected"
      onClick={() => onClick(date)}
    >
      {/* Breathing Ring */}
      <motion.div
        className="absolute inset-0 border-2 border-emerald-500/30"
        variants={ringVariants}
      />

      <div className="absolute top-2 right-2 text-white/80 text-lg font-display font-medium group-hover:text-white transition-colors">
        {formattedDay}
      </div>

      <div className="flex flex-col items-center justify-center h-full gap-2 z-10 relative">
        <motion.div variants={iconVariants}>
          <div className="w-5 h-5 rounded-full bg-emerald-500 flex items-center justify-center shadow-[0_0_10px_rgba(16,185,129,0.5)]">
            <Check size={12} className="text-black font-bold" />
          </div>
        </motion.div>
        <span className="text-[10px] font-bold text-emerald-400 uppercase tracking-widest group-hover:text-emerald-300 transition-colors">Available</span>
      </div>
    </motion.div>
  );
};


// --- MAIN COMPONENT ---

const RentalAvailabilityCalendar = ({ onDateSelect, newlyBookedDate }: RentalAvailabilityCalendarProps) => {
  const [currentMonth, setCurrentMonth] = useState(new Date());
  // [FIX] State now includes eventType and eventTime
  const [unavailableDates, setUnavailableDates] = useState<{
    date: Date;
    status: string;
    eventType?: string;
    eventTime?: string;
  }[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [confirmDate, setConfirmDate] = useState<Date | null>(null);

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

    // Set up polling every 60 seconds
    const pollInterval = setInterval(() => {
      // Only poll if page is visible
      if (!document.hidden) {
        fetchAvailability();
      }
    }, 60000); // 60 seconds

    // Cleanup interval on unmount
    return () => clearInterval(pollInterval);
  }, [currentMonth, newlyBookedDate]);

  useEffect(() => {
    // Auto-scroll to newly booked date
    if (newlyBookedDate && !isSameMonth(newlyBookedDate, currentMonth)) {
      setCurrentMonth(startOfMonth(newlyBookedDate));
    }
  }, [newlyBookedDate]); // Triggers when a new date is booked

  const getDayStatus = (date: Date) => {
    // 1. Newly Booked Check
    if (newlyBookedDate && isSameDay(newlyBookedDate, date)) {
      return { status: 'newly_booked', eventType: undefined, eventTime: undefined, isPast: false };
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    // 2. Past Date Check
    const isPast = isBefore(date, today);

    // 3. Unavailable Check (Robust Comparison)
    // We treat UTC dates from server as local "dates" to avoid timezone shifts
    const match = unavailableDates.find(u => {
      const uDate = new Date(u.date);
      // Use string comparison of YYYY-MM-DD for robustness
      const dateStr = format(date, 'yyyy-MM-dd');
      const uDateStr = format(uDate, 'yyyy-MM-dd');

      // If server sent UTC, but it's now local, it might have shifted.
      // If we suspect it shifted, we can check uDate.toISOString().split('T')[0]
      const uDateUtcStr = uDate.toISOString().split('T')[0];

      return dateStr === uDateStr || dateStr === uDateUtcStr;
    });

    if (match) {
      // Return status even if it's "past" so we can see WHAT was booked in the past
      return {
        status: match.status.toLowerCase(),
        eventType: match.eventType,
        eventTime: match.eventTime,
        isPast: isPast
      };
    }

    if (isPast) return { status: 'past', eventType: undefined, eventTime: undefined, isPast: true };

    return { status: 'available', eventType: undefined, eventTime: undefined, isPast: false };
  };

  const handleDayClick = (date: Date) => {
    setConfirmDate(date);
  };

  const handleConfirm = () => {
    if (confirmDate) {
      onDateSelect(confirmDate);
      setConfirmDate(null);
    }
  };

  const renderDays = () => {
    const monthStart = startOfMonth(currentMonth);
    const monthEnd = endOfMonth(monthStart);
    const startDate = startOfWeek(monthStart);
    const endDate = endOfWeek(monthEnd);

    const rows = [];
    let days = [];
    let day = startDate;

    while (day <= endDate) {
      for (let i = 0; i < 7; i++) {
        const cloneDay = day;
        const dayInfo = getDayStatus(cloneDay);
        const isCurrentMonth = isSameMonth(cloneDay, monthStart);
        const isNewlyBooked = newlyBookedDate ? isSameDay(cloneDay, newlyBookedDate) : false;

        days.push(
          <DayTile
            key={day.toString()}
            date={cloneDay}
            status={dayInfo.status}
            isCurrentMonth={isCurrentMonth}
            isNewlyBooked={isNewlyBooked}
            eventType={dayInfo.eventType}
            eventTime={dayInfo.eventTime}
            isPast={dayInfo.isPast}
            onClick={() => dayInfo.status === 'available' && handleDayClick(cloneDay)}
          />
        );
        day = addDays(day, 1);
      }
      rows.push(
        <div className="grid grid-cols-7 gap-px lg:gap-1 bg-white/5 border border-white/5 rounded-xl overflow-hidden shadow-2xl mb-1" key={day.toString()}>
          {days}
        </div>
      );
      days = [];
    }
    return <div className="animate-fade-in relative z-0">{rows}</div>;
  };

  if (isLoading) {
    return <div className="h-96 flex items-center justify-center text-burnished-gold animate-pulse">Loading calendar...</div>;
  }

  return (
    <div className="relative">
      <div className="bg-midnight-blue p-4 sm:p-8 rounded-2xl shadow-2xl border border-white/10 max-w-5xl mx-auto min-h-[600px]">
        {/* Header */}
        <div className="flex items-center justify-between mb-8 px-4">
          <button onClick={() => setCurrentMonth(subMonths(currentMonth, 1))} className="p-2 rounded-full hover:bg-white/10 text-burnished-gold"><ChevronLeft size={32} /></button>
          <div className="text-center">
            <h2 className="text-3xl font-display font-bold text-pure-white">{format(currentMonth, 'MMMM yyyy')}</h2>
            <p className="text-pure-white/50 text-sm mt-1">Select a green available date below</p>
          </div>
          <button onClick={() => setCurrentMonth(addMonths(currentMonth, 1))} className="p-2 rounded-full hover:bg-white/10 text-burnished-gold"><ChevronRight size={32} /></button>
        </div>

        {/* Week Days */}
        <div className="grid grid-cols-7 mb-2">
          {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map(d => (
            <div key={d} className="text-center font-display font-bold text-burnished-gold/70 text-sm uppercase tracking-wider py-2">{d}</div>
          ))}
        </div>

        {renderDays()}
      </div>

      {/* CONFIRMATION OVERLAY */}
      <AnimatePresence>
        {confirmDate && (
          <motion.div
            className="absolute inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-black/40 rounded-2xl"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
          >
            <motion.div
              className="bg-midnight-blue border border-burnished-gold p-8 rounded-xl shadow-2xl max-w-md w-full mx-4"
              initial={{ scale: 0.9, y: 20 }}
              animate={{ scale: 1, y: 0 }}
              exit={{ scale: 0.9, y: 20 }}
            >
              <h3 className="text-2xl font-bold text-white mb-2">Select this date?</h3>
              <p className="text-white/70 mb-6 text-lg">
                Do you want to start a rental application for <br />
                <span className="text-burnished-gold font-bold">{format(confirmDate, 'EEEE, MMMM do, yyyy')}</span>?
              </p>

              <div className="flex gap-4">
                <button
                  onClick={() => setConfirmDate(null)}
                  className="flex-1 py-3 px-4 rounded-lg border border-white/20 text-white font-medium hover:bg-white/10 transition-colors"
                >
                  Cancel
                </button>
                <button
                  onClick={handleConfirm}
                  className="flex-1 py-3 px-4 rounded-lg bg-emerald-600 text-white font-bold hover:bg-emerald-500 transition-colors shadow-[0_0_15px_rgba(16,185,129,0.4)]"
                >
                  Yes, Select Date
                </button>
              </div>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
};

export default RentalAvailabilityCalendar;
