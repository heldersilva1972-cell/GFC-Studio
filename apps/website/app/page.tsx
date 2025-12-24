import Hero from '@/components/Hero'
import FeatureGrid from '@/components/FeatureGrid'
import EventsCalendar from '@/components/EventsCalendar'
import RentalAvailabilityCalendar from '@/components/RentalAvailabilityCalendar'
import ContactForm from '@/components/ContactForm'

export default function HomePage() {
    return (
        <>
            <Hero />
            <FeatureGrid />
            <EventsCalendar />
            <RentalAvailabilityCalendar />
            <ContactForm />
        </>
    )
}
