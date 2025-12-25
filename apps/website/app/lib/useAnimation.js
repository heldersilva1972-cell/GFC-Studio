// [MODIFIED]
'use client';

import { useEffect } from 'react';
import { useAnimation, useMotionValue } from 'framer-motion';

export function useStudioAnimations() {
  const controls = useAnimation();
  const time = useMotionValue(0);

  useEffect(() => {
    const handleMessage = (event) => {
      // TODO: Add origin check for security in production
      const { type, payload } = event.data;

      if (type === 'ANIMATION_PREVIEW' && payload) {
        const { keyframes } = payload;
        const sequence = keyframes.map(kf => {
            const transition = {
                duration: kf.duration,
                delay: kf.delay,
                ease: kf.easing || 'easeInOut'
            };

            let animationProps = {};
            switch (kf.effect) {
                case 'fadeIn':
                    animationProps = { opacity: [0, 1] };
                    break;
                case 'slideInUp':
                    animationProps = { y: ['100%', '0%'], opacity: [0, 1] };
                    break;
                case 'bounceIn':
                    transition.type = 'spring';
                    transition.damping = 10;
                    transition.stiffness = 300;
                    animationProps = { scale: [0.5, 1], opacity: [0, 1] };
                    break;
                default:
                    animationProps = { opacity: [0, 1] };
            }

            return { ...animationProps, transition };
        });

        controls.start(sequence);
      } else if (type === 'SCRUB_UPDATE' && payload) {
        const { position } = payload;
        const totalDuration = 5; // 5s total time
        const newTime = (position / 100) * totalDuration;
        time.set(newTime);
      }
    };

    window.addEventListener('message', handleMessage);

    return () => {
      window.removeEventListener('message', handleMessage);
    };
  }, [controls, time]);

  return { controls, time };
}
