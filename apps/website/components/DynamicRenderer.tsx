// [MODIFIED]
'use client';

import React, { useEffect, useState } from 'react';
import { motion, useAnimation } from 'framer-motion';
import Hero from './Hero';
import FeatureGrid from './FeatureGrid';
import ContactSection from './ContactSection';
import RichTextBlock from './RichTextBlock';
import ButtonCTA from './ButtonCTA';
import LayoutGrid from './LayoutGrid';
import { usePageStore } from '@/app/lib/store';
import { StudioSection, AnimationKeyframe } from '@/app/lib/types';
import styles from './DynamicRenderer.module.css';

const getAnimationVariants = (effect: string) => {
    // ... animation logic from previous steps
};

const sectionComponentMap: { [key: string]: React.ComponentType<any> } = {
  'Hero': Hero,
  'RichTextBlock': RichTextBlock,
  'ButtonCTA': ButtonCTA,
  'LayoutGrid': LayoutGrid,
  'FeatureGrid': FeatureGrid,
  'Contact': ContactSection,
};

interface DynamicRendererProps {
  sections: StudioSection[];
  animationKeyframes?: AnimationKeyframe[];
}

const DynamicRenderer: React.FC<DynamicRendererProps> = ({ sections, animationKeyframes }) => {
  const { selectedSectionId, setSelectedSectionId } = usePageStore();
  const controls = useAnimation();
  const [isInStudio, setIsInStudio] = useState(false);

  useEffect(() => {
    // Check if running inside an iframe (the studio)
    if (window.self !== window.top) {
      setIsInStudio(true);
    }

    const handleMessage = (event) => {
      if (event.data.type === 'ANIMATION_SCRUB' && isInStudio) {
        controls.set('visible');
        controls.start({
          opacity: event.data.position,
          y: 50 * (1 - event.data.position),
          transition: { duration: 0 },
        });
      }
    };

    window.addEventListener('message', handleMessage);
    return () => window.removeEventListener('message', handleMessage);
  }, [controls, isInStudio]);

  const handleSectionClick = (section: StudioSection) => {
    setSelectedSectionId(section.clientId);
    const targetOrigin = document.referrer;
    if (targetOrigin) {
        window.parent.postMessage({
            type: 'SECTION_SELECTED',
            section: section,
        }, targetOrigin);
    }
  };

  if (!sections || sections.length === 0) {
    return <p>No content sections available.</p>;
  }

  const sortedSections = [...sections].sort((a, b) => a.sortOrder - b.sortOrder);

  return (
    <div className={styles.rendererContainer}>
      {sortedSections.map((section) => {
        const Component = sectionComponentMap[section.sectionType];
        if (!Component) return null;

        const isSelected = section.clientId === selectedSectionId;
        const selectionStyle = isSelected ? { border: '2px solid #007bff', boxShadow: '0 0 10px #007bff' } : {};

        const keyframe = animationKeyframes?.find(k => k.target === section.sectionType);

        const getInitialState = (effect) => {
            switch (effect) {
                case 'fadeIn': return { opacity: 0 };
                case 'slideUp': return { opacity: 0, y: 50 };
                case 'slideDown': return { opacity: 0, y: -50 };
                case 'slideLeft': return { opacity: 0, x: -50 };
                case 'slideRight': return { opacity: 0, x: 50 };
                case 'scaleIn': return { opacity: 0, scale: 0.5 };
                case 'bounceIn': return { opacity: 0, scale: 0.5 };
                default: return { opacity: 1 };
            }
        };

        const variants = keyframe ? {
          hidden: getInitialState(keyframe.effect),
          visible: {
            opacity: 1,
            y: 0,
            x: 0,
            scale: 1,
            transition: {
              duration: keyframe.duration || 1,
              delay: keyframe.delay || 0,
              ease: keyframe.easing || 'easeInOut'
            }
          }
        } : {};

        return (
            <motion.div
                key={section.id}
                onClick={() => handleSectionClick(section)}
                style={selectionStyle}
                initial="hidden"
                animate={isInStudio ? controls : undefined}
                whileInView={!isInStudio ? "visible" : undefined}
                viewport={{ once: true }}
                variants={variants}
            >
                <Component {...section.properties} />
            </motion.div>
        );
      })}
    </div>
  );
};

export default DynamicRenderer;
