// REV: AnimationPlayground / Phase 4B / CodeViewer Component
"use client";

import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { getAnimationById } from "@/app/animations/core/AnimationRegistry";

interface CodeViewerProps {
  animationId: string | null;
  className?: string;
}

export default function CodeViewer({
  animationId,
  className = "",
}: CodeViewerProps) {
  const [copied, setCopied] = useState(false);
  const [isExpanded, setIsExpanded] = useState(true);

  const animation = animationId ? getAnimationById(animationId) : null;
  const code = animation?.code || "// No animation selected";

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(code);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error("Failed to copy code:", err);
    }
  };

  return (
    <motion.div
      initial={{ y: 100, opacity: 0 }}
      animate={{ y: 0, opacity: 1 }}
      exit={{ y: 100, opacity: 0 }}
      transition={{ duration: 0.3, ease: "easeOut" }}
      className={`w-full ${className}`}
    >
      <div className="bg-white rounded-t-xl border border-gray-200 shadow-lg overflow-hidden">
        {/* Header with Toggle and Copy Button */}
        <div className="bg-gray-50 border-b border-gray-200 px-4 py-3 flex items-center justify-between">
          <button
            onClick={() => setIsExpanded(!isExpanded)}
            className="flex items-center gap-2 text-sm font-medium text-gray-700 hover:text-gray-900 transition-colors"
          >
            <motion.svg
              animate={{ rotate: isExpanded ? 180 : 0 }}
              transition={{ duration: 0.2 }}
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M19 9l-7 7-7-7"
              />
            </motion.svg>
            <span>Code</span>
          </button>
          <button
            onClick={handleCopy}
            className={`px-3 py-1.5 rounded text-xs font-medium transition-all ${
              copied
                ? "bg-green-600 text-white"
                : "bg-blue-600 text-white hover:bg-blue-700"
            } shadow-sm`}
          >
            {copied ? "Copied!" : "Copy Code"}
          </button>
        </div>

        {/* Code Content */}
        <AnimatePresence>
          {isExpanded && (
            <motion.div
              initial={{ height: 0, opacity: 0 }}
              animate={{ height: "auto", opacity: 1 }}
              exit={{ height: 0, opacity: 0 }}
              transition={{ duration: 0.3 }}
              className="overflow-hidden"
            >
              <div className="prose prose-sm max-w-none">
                <pre className="m-0 p-4 bg-gray-900 text-gray-100 font-mono text-sm overflow-x-auto">
                  <code>{code}</code>
                </pre>
              </div>
            </motion.div>
          )}
        </AnimatePresence>
      </div>
    </motion.div>
  );
}

