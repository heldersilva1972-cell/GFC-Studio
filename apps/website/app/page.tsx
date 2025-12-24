// [MODIFIED]
'use client';

import Hero from '@/components/Hero'
import FeatureGrid from '@/components/FeatureGrid'
import EventsCalendar from '@/components/EventsCalendar'
import RentalAvailabilityCalendar from '@/components/RentalAvailabilityCalendar'
import ContactSection from '@/components/ContactSection'
import { AnimatedComponent } from '@/components/AnimatedComponent'

export default function HomePage() {
    return (
        <>
            <Hero />
            <FeatureGrid />
            <EventsCalendar />
            <RentalAvailabilityCalendar />
            <ContactSection />
            <AnimatedComponent />
        </>
    )
}
