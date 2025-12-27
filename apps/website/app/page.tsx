// [MODIFIED]
import React from 'react';
import AnimationRenderer from '@/components/AnimationRenderer';
import FeatureGrid from '@/components/FeatureGrid';
import { getAnimationById, getHomePageContent } from '@/app/lib/api';

const HomePage = async () => {
    const animation = await getAnimationById('home-hero');
    const content = await getHomePageContent();

    return (
        <main>
            <AnimationRenderer animation={animation} content={content} />
            <FeatureGrid />
        </main>
    );
};

export default HomePage;
