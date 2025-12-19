"use client";

import { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { getAnimationById } from "@/app/animations/core/AnimationRegistry";

interface CodeDisplayProps {
  animationId: string | null;
  className?: string;
}

export default function CodeDisplay({
  animationId,
  className = "",
}: CodeDisplayProps) {
  const [copied, setCopied] = useState(false);
  const [isExpanded, setIsExpanded] = useState(true);
  const [isMobile, setIsMobile] = useState(false);

  useEffect(() => {
    const checkMobile = () => {
      setIsMobile(window.innerWidth < 768);
      if (window.innerWidth >= 768) {
        setIsExpanded(true);
      }
    };
    checkMobile();
    window.addEventListener("resize", checkMobile);
    return () => window.removeEventListener("resize", checkMobile);
  }, []);

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
    <div className={`w-full ${className}`}>
      {/* Mobile: Accordion Header */}
      {isMobile && (
        <button
          onClick={() => setIsExpanded(!isExpanded)}
          className="w-full bg-white rounded-t-xl border border-gray-200 px-4 py-3 flex items-center justify-between shadow-sm"
        >
          <span className="font-medium text-gray-900">Code</span>
          <motion.svg
            animate={{ rotate: isExpanded ? 180 : 0 }}
            transition={{ duration: 0.2 }}
            className="w-5 h-5 text-gray-500"
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
        </button>
      )}

      {/* Code Display */}
      <AnimatePresence>
        {(!isMobile || isExpanded) && (
          <motion.div
            initial={isMobile ? { height: 0, opacity: 0 } : false}
            animate={{ height: "auto", opacity: 1 }}
            exit={isMobile ? { height: 0, opacity: 0 } : undefined}
            transition={{ duration: 0.3 }}
            className={`bg-white ${isMobile ? "rounded-b-xl" : "rounded-xl"} border border-gray-200 shadow-sm overflow-hidden`}
          >
            {/* Header with Copy Button */}
            <div className="bg-gray-50 border-b border-gray-200 px-4 py-3 flex items-center justify-between">
              <span className="text-sm font-medium text-gray-700">
                {animation?.name || "No Animation"} - Source Code
              </span>
              <button
                onClick={handleCopy}
                className={`px-4 py-2 rounded-lg text-sm font-medium transition-all ${
                  copied
                    ? "bg-green-600 text-white"
                    : "bg-green-500 text-white hover:bg-green-600"
                } shadow-sm`}
              >
                {copied ? (
                  <span className="flex items-center gap-2">
                    <svg
                      className="w-4 h-4"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M5 13l4 4L19 7"
                      />
                    </svg>
                    Copied!
                  </span>
                ) : (
                  "Copy Code"
                )}
              </button>
            </div>

            {/* Code Content */}
            <div className="relative overflow-x-auto">
              <pre className="p-4 text-sm bg-gray-900 text-gray-100 font-mono overflow-x-auto">
                <code>{code}</code>
              </pre>
            </div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}

