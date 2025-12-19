"use client";

import { useState } from "react";
import AnimationSelectorCore from "@/app/animations/core/AnimationSelector";
import { animationRegistry } from "@/app/animations/core/AnimationRegistry";

interface AnimationSelectorProps {
  selectedId: string | null;
  onSelect: (id: string | null) => void;
  className?: string;
  variant?: "dropdown" | "list";
}

export default function AnimationSelector({
  selectedId,
  onSelect,
  className = "",
  variant = "list",
}: AnimationSelectorProps) {
  const [isOpen, setIsOpen] = useState(false);

  // Dropdown variant for simple selection
  if (variant === "dropdown") {
    const selectedAnimation = selectedId
      ? animationRegistry.find((anim) => anim.id === selectedId)
      : null;

    return (
      <div className={`relative ${className}`}>
        <button
          onClick={() => setIsOpen(!isOpen)}
          className="w-full bg-white border border-gray-300 rounded-lg px-4 py-2 text-left flex items-center justify-between hover:border-gray-400 transition-colors"
        >
          <span className="text-gray-900">
            {selectedAnimation?.name || "Select an animation"}
          </span>
          <svg
            className={`w-5 h-5 text-gray-500 transition-transform ${
              isOpen ? "rotate-180" : ""
            }`}
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
          </svg>
        </button>

        {isOpen && (
          <>
            <div
              className="fixed inset-0 z-10"
              onClick={() => setIsOpen(false)}
            />
            <div className="absolute z-20 w-full mt-1 bg-white border border-gray-200 rounded-lg shadow-lg max-h-60 overflow-y-auto">
              {animationRegistry.map((animation) => (
                <button
                  key={animation.id}
                  onClick={() => {
                    onSelect(animation.id);
                    setIsOpen(false);
                  }}
                  className={`w-full text-left px-4 py-2 hover:bg-gray-50 transition-colors ${
                    selectedId === animation.id ? "bg-blue-50" : ""
                  }`}
                >
                  <div className="font-medium text-gray-900">
                    {animation.name}
                  </div>
                  <div className="text-xs text-gray-500">{animation.category}</div>
                </button>
              ))}
            </div>
          </>
        )}
      </div>
    );
  }

  // List variant (default) - uses the core AnimationSelector
  return (
    <AnimationSelectorCore
      selectedId={selectedId}
      onSelect={onSelect}
      className={className}
    />
  );
}

