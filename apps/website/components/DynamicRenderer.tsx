// [MODIFIED]
'use client';

import React from 'react';
import { motion } from 'framer-motion';
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

        return (
            <motion.div
                key={section.id}
                onClick={() => handleSectionClick(section)}
                style={selectionStyle}
                // ... animation props
            >
                <Component {...section.properties} />
            </motion.div>
        );
      })}
    </div>
  );
};

export default DynamicRenderer;
