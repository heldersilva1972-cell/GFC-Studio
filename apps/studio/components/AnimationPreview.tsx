// REV: AnimationPlayground / Phase 4B / AnimationPreview Component
"use client";

import { motion, AnimatePresence } from "framer-motion";
import AnimationContainer from "@/app/animations/core/AnimationContainer";

interface AnimationPreviewProps {
  animationId: string | null;
  size?: number;
  speed?: number;
  colors?: string[];
  className?: string;
}

export default function AnimationPreview({
  animationId,
  size = 300,
  speed = 1.0,
  colors,
  className = "",
}: AnimationPreviewProps) {
  return (
    <motion.div
      initial={{ x: 100, opacity: 0 }}
      animate={{ x: 0, opacity: 1 }}
      exit={{ x: 100, opacity: 0 }}
      transition={{ duration: 0.3, ease: "easeOut" }}
      className={`w-full bg-white rounded-xl shadow-lg border border-gray-200 overflow-y-auto ${className}`}
    >
      <div className="p-8">
        <div className="flex items-center justify-center min-h-[400px] max-w-5xl mx-auto">
          <AnimatePresence mode="wait">
            {animationId ? (
              <motion.div
                key={animationId}
                initial={{ opacity: 0, scale: 0.95 }}
                animate={{ opacity: 1, scale: 1 }}
                exit={{ opacity: 0, scale: 0.95 }}
                transition={{ duration: 0.3 }}
                className="w-full h-full flex items-center justify-center"
              >
                <AnimationContainer
                  animationId={animationId}
                  size={size}
                  speed={speed}
                  colors={colors}
                />
              </motion.div>
            ) : (
              <motion.div
                key="placeholder"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="text-center text-gray-500"
              >
                <p className="text-lg mb-2">No animation selected</p>
                <p className="text-sm">
                  Select an animation from the list to preview it here
                </p>
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </div>
    </motion.div>
  );
}

