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
import { StudioSection } from '@/app/lib/types';
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
}

const DynamicRenderer: React.FC<DynamicRendererProps> = ({ sections }) => {
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

  const handleMoveUp = (sectionId: string) => {
    window.parent.postMessage({ type: 'MOVE_SECTION', payload: { sectionId, direction: -1 } }, document.referrer);
  };

  const handleMoveDown = (sectionId: string) => {
    window.parent.postMessage({ type: 'MOVE_SECTION', payload: { sectionId, direction: 1 } }, document.referrer);
  };

  const handleDelete = (sectionId: string) => {
    window.parent.postMessage({ type: 'DELETE_SECTION', payload: { sectionId } }, document.referrer);
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

        const animationSettings = section.animationSettingsJson ? JSON.parse(section.animationSettingsJson) : {};

        const getInitialState = (effect) => {
            switch (effect) {
                case 'FadeIn': return { opacity: 0 };
                case 'SlideUp': return { opacity: 0, y: 50 };
                case 'Scale': return { opacity: 0, scale: 0.5 };
                case 'Rotate': return { opacity: 0, rotate: -45 };
                default: return { opacity: 1 };
            }
        };

        const variants = {
          hidden: getInitialState(animationSettings.Effect),
          visible: {
            opacity: 1,
            y: 0,
            scale: 1,
            rotate: 0,
            transition: {
              duration: animationSettings.Duration || 1,
              delay: animationSettings.Delay || 0,
              ease: animationSettings.Easing || 'easeInOut'
            }
          }
        };

        return (
            <motion.div
                key={section.id}
                style={selectionStyle}
                initial="hidden"
                animate={isInStudio ? controls : undefined}
                whileInView={!isInStudio ? "visible" : undefined}
                viewport={{ once: true }}
                variants={variants}
                className={styles.sectionWrapper}
            >
                {isInStudio && isSelected && (
                    <div className={styles.sectionToolbar}>
                        <button onClick={() => handleMoveUp(section.clientId)}>↑</button>
                        <button onClick={() => handleMoveDown(section.clientId)}>↓</button>
                        <button onClick={() => handleDelete(section.clientId)}>🗑️</button>
                    </div>
                )}
                <div onClick={() => handleSectionClick(section)} className={styles.sectionContent}>
                    <Component {...section.properties} />
                </div>
            </motion.div>
        );
      })}
    </div>
  );
};

export default DynamicRenderer;
