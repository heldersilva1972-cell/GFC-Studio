// [NEW]
'use client';

import { motion } from 'framer-motion';
import { useStudioAnimations } from '../app/lib/useAnimation';

export function AnimatedComponent() {
  const { controls, time } = useStudioAnimations();

  return (
    <motion.div
      className="animated-box"
      animate={controls}
      style={{
        width: 100,
        height: 100,
        backgroundColor: 'gold',
      }}
    >
      <motion.div style={{ width: '100%', height: '100%', x: time }} />
    </motion.div>
  );
}
