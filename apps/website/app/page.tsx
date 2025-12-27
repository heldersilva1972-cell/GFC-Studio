// [MODIFIED]
import React from 'react';
import AnimationRenderer from '@/components/AnimationRenderer';
import FeatureGrid from '@/components/FeatureGrid';
import { getAnimationById, getHomePageContent } from '@/app/lib/api';

const HomePage = async () => {
    let animation;
    try {
        animation = await getAnimationById('home-hero');
    } catch (error) {
        console.error("Failed to fetch animation, using default:", error);
        animation = { keyframes: [] };
    }
    const content = await getHomePageContent();

    return (
        <main>
            <AnimationRenderer animation={animation} content={content} />
            <FeatureGrid />
        </main>
    );
};

export default HomePage;
