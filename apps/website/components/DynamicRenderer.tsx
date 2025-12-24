// [NEW]
// [MODIFIED]
import React from 'react';
import { motion } from 'framer-motion';
import Hero from './Hero';
import FeatureGrid from './FeatureGrid';
import ContactSection from './ContactSection';
import styles from './DynamicRenderer.module.css';

// Helper function to get animation variants based on the effect type
const getAnimationVariants = (effect: string) => {
  const defaultVariants = {
    hidden: { opacity: 0, y: 50 },
    visible: { opacity: 1, y: 0 },
  };

  switch (effect) {
    case 'slideInFromLeft':
      return {
        hidden: { opacity: 0, x: -50 },
        visible: { opacity: 1, x: 0 },
      };
    case 'slideInFromRight':
      return {
        hidden: { opacity: 0, x: 50 },
        visible: { opacity: 1, x: 0 },
      };
    case 'fadeInUp':
    default:
      return defaultVariants;
  }
};

// Define a mapping from section type strings to React components
const sectionComponentMap: { [key: string]: React.ComponentType<any> } = {
  'Hero': Hero,
  'FeatureGrid': FeatureGrid,
  'Contact': ContactSection,
  // Add other section types here as they are created
};

// Define the expected structure of the props
interface AnimationSettings {
  effect: string;
  duration: number;
  delay: number;
}

interface Section {
  id: string;
  sectionType: string;
  content: Record<string, any>;
  animationSettings: AnimationSettings;
  sortOrder: number;
}

interface DynamicRendererProps {
  sections: Section[];
}

const DynamicRenderer: React.FC<DynamicRendererProps> = ({ sections }) => {
  if (!sections || sections.length === 0) {
    return <p>No content sections available.</p>;
  }

  // Sort sections by sortOrder
  const sortedSections = [...sections].sort((a, b) => a.sortOrder - b.sortOrder);

  return (
    <div className={styles.rendererContainer} data-testid="dynamic-renderer-container">
      {sortedSections.map((section) => {
        const Component = sectionComponentMap[section.sectionType];

        if (!Component) {
          console.warn(`No component found for section type: ${section.sectionType}`);
          return null; // Or render a placeholder/error component
        }

        try {
          const contentProps = section.content || {};
          const animationProps = section.animationSettings || { effect: 'fadeInUp', duration: 0.8, delay: 0.2 };

          const variants = getAnimationVariants(animationProps.effect);

          return (
            <motion.div
              key={section.id}
              initial="hidden"
              whileInView="visible"
              viewport={{ once: true, amount: 0.2 }}
              variants={variants}
              transition={{
                duration: animationProps.duration,
                delay: animationProps.delay || 0.2,
              }}
            >
              <Component {...contentProps} />
            </motion.div>
          );
        } catch (error) {
          console.error(`Error rendering component for section ${section.id}:`, error);
          return null; // Don't render components with invalid data
        }
      })}
    </div>
  );
};

export default DynamicRenderer;
