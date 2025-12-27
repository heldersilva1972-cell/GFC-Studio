// [NEW]
// [MODIFIED]
'use client';

import React from 'react';
import { motion } from 'framer-motion';
import Hero from './Hero';
import styles from './AnimationRenderer.module.css';

const getAnimationVariants = (keyframe) => {
    if (!keyframe) return {};
    const { effect, duration, delay, easing } = keyframe;
    let initial = {};
    let animate = {};

    switch (effect) {
        case 'fade-in':
            initial = { opacity: 0 };
            animate = { opacity: 1 };
            break;
        case 'slide-in-up':
            initial = { opacity: 0, y: 50 };
            animate = { opacity: 1, y: 0 };
            break;
        case 'scale-in':
            initial = { scale: 0.5, opacity: 0 };
            animate = { scale: 1, opacity: 1 };
            break;
        default:
            initial = { opacity: 1 };
            animate = { opacity: 1 };
    }

    return {
        initial,
        animate,
        transition: {
            duration: duration || 1,
            delay: delay || 0,
            ease: easing || 'easeInOut',
        },
    };
};

const AnimationRenderer = ({ animation, content, fallbackImage = '/images/hero-bg.jpg' }) => {
    if (!animation || !content) {
        // Graceful fallback to a static image
        return (
            <div className={styles.fallbackContainer} style={{ backgroundImage: `url(${fallbackImage})` }}>
                <div className={styles.fallbackOverlay}></div>
                 <Hero {...content} />
            </div>
        );
    }

    const { keyframes } = animation;

    // This assumes the renderer is for a "Hero" type component for now.
    // A more dynamic implementation would map targets to components.
    return (
        <div className={styles.rendererWrapper}>
             <motion.div {...getAnimationVariants(keyframes.find(k => k.target === 'background-image'))}>
                <Hero
                    title={
                        <motion.span {...getAnimationVariants(keyframes.find(k => k.target === 'headline'))}>
                            {content.title}
                        </motion.span>
                    }
                    subtitle={
                        <motion.span {...getAnimationVariants(keyframes.find(k => k.target === 'subtitle'))}>
                            {content.subtitle}
                        </motion.span>
                    }
                    primaryCtaText={
                         <motion.div {...getAnimationVariants(keyframes.find(k => k.target === 'primary-cta'))}>
                           {content.primaryCtaText}
                         </motion.div>
                    }
                     secondaryCtaText={
                         <motion.div {...getAnimationVariants(keyframes.find(k => k.target === 'secondary-cta'))}>
                           {content.secondaryCtaText}
                         </motion.div>
                    }
                    primaryCtaLink={content.primaryCtaLink}
                    secondaryCtaLink={content.secondaryCtaLink}
                    backgroundImage={content.backgroundImage}
                    stats={content.stats}
                />
            </motion.div>
        </div>
    );
};

export default AnimationRenderer;
