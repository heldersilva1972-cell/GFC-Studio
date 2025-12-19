import MorphingShapeFlow from "./MorphingShapeFlow";
import { morphingShapeFlowCode } from "./code";

export const morphingShapeFlowConfig = {
  id: "morphingShapeFlow",
  name: "Morphing Shape Flow",
  category: "Advanced",
  complexity: 5,
  previewSize: "md" as const,
  component: MorphingShapeFlow,
  code: morphingShapeFlowCode,
  editor: {
    filePathHint: "animations/advanced/MorphingShapeFlow/MorphingShapeFlow.tsx",
    sourceCode: `"use client";

import React from "react";
import { motion } from "framer-motion";

export default function MorphingShapeFlow({
  className = "",
  size = 200,
}: {
  className?: string;
  size?: number;
}) {
  return (
    <div className={\`flex items-center justify-center \${className}\`}>
      <motion.div
        className="h-48 w-48 bg-gradient-to-tr from-violet-500 via-fuchsia-500 to-amber-400"
        animate={{
          borderRadius: [
            "40% 60% 60% 40%",
            "70% 30% 50% 50%",
            "30% 70% 40% 60%",
            "40% 60% 60% 40%",
          ],
          rotate: [0, 12, -8, 0],
        }}
        transition={{
          duration: 7,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}`,
    notes: "Advanced SVG morphing with gradient colors. Requires Framer Motion.",
    suggestedUse: "Perfect for hero sections, landing pages, or as a background element.",
  },
};

export { MorphingShapeFlow };
export default morphingShapeFlowConfig;

