// [MODIFIED]
import Hero from '@/components/Hero'
import FeatureGrid from '@/components/FeatureGrid'
import HomepageNewsModule from '@/components/HomepageNewsModule'
import EventsCalendar from '@/components/EventsCalendar'
import RentalAvailabilityCalendar from '@/components/RentalAvailabilityCalendar'
import ContactSection from '@/components/ContactSection'
import PublicReviewSubmissionForm from '@/components/PublicReviewSubmissionForm'

export default function HomePage() {
    return (
        <>
            <Hero />
            <FeatureGrid />

            {/* News Section */}
            <section className="py-20 bg-midnight-blue">
                <div className="container mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="text-center mb-12">
                        <h2 className="text-3xl md:text-4xl font-display font-bold text-pure-white">
                            Latest <span className="text-burnished-gold">Updates</span>
                        </h2>
                        <p className="mt-4 text-lg text-pure-white/80 max-w-2xl mx-auto">
                            Stay up-to-date with the latest news and announcements from the club.
                        </p>
                    </div>
                    <HomepageNewsModule />
                </div>
            </section>

            <EventsCalendar />
            <RentalAvailabilityCalendar />

            {/* Review Submission Section */}
            <section className="py-20 bg-midnight-blue/70">
                <div className="container mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="max-w-2xl mx-auto">
                        <div className="text-center mb-12">
                            <h2 className="text-3xl md:text-4xl font-display font-bold text-pure-white">
                                Share Your <span className="text-burnished-gold">Experience</span>
                            </h2>
                            <p className="mt-4 text-lg text-pure-white/80">
                                We value your feedback. Let us know about your experience at the GFC.
                            </p>
                        </div>
                        <PublicReviewSubmissionForm />
                    </div>
                </div>
            </section>

            <ContactSection />
        </>
    )
}
