import React from 'react';
import EventsCalendar from '@/components/EventsCalendar';

const EventsPage = () => {
    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold text-center mb-6 text-midnight-blue">Events Calendar</h1>
            <p className="text-center text-gray-600 mb-8 max-w-2xl mx-auto">
                Stay up to date with the latest happenings at the Gloucester Fraternity Club.
                From community gatherings to special celebrations, there's always something going on.
            </p>
            <div className="max-w-4xl mx-auto">
                <EventsCalendar />
            </div>
        </div>
    );
};

export default EventsPage;
