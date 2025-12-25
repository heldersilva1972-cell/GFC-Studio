import React from 'react';
import Hero from '@/components/Hero';
import FeatureGrid from '@/components/FeatureGrid';

const HomePage = () => {
    return (
        <main>
            <Hero
                title="Building Community, Friendship, and Tradition"
                subtitle="Welcome to the Gloucester Fraternity Club — a place where friendship, family, and community come together. Since our founding in the early 1920s, we've proudly served the Cape Ann area through fellowship, fun, and service."
                primaryCtaText="Rent Our Hall"
                primaryCtaLink="/hall-rentals"
                secondaryCtaText="View Events"
                secondaryCtaLink="/events"
                stats={[
                    { label: "Years of Service", value: "100+" },
                    { label: "Active Members", value: "500+" },
                    { label: "Events Yearly", value: "50+" }
                ]}
            />
            <FeatureGrid />
        </main>
    );
};

export default HomePage;
