"use client";

import { useState } from "react";
import { animationRegistry, getCategories } from "./AnimationRegistry";
import type { AnimationConfig } from "./types";

interface AnimationSelectorProps {
  selectedId: string | null;
  onSelect: (id: string | null) => void;
  className?: string;
}

export default function AnimationSelector({
  selectedId,
  onSelect,
  className = "",
}: AnimationSelectorProps) {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const categories = getCategories();

  const filteredAnimations = selectedCategory
    ? animationRegistry.filter((anim) => anim.category === selectedCategory)
    : animationRegistry;

  return (
    <div className={`space-y-4 ${className}`}>
      {/* Category Filter */}
      <div className="flex flex-wrap gap-2">
        <button
          onClick={() => setSelectedCategory(null)}
          className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
            selectedCategory === null
              ? "bg-blue-600 text-white shadow-sm"
              : "bg-white text-gray-700 hover:bg-gray-50 border border-gray-200"
          }`}
        >
          All
        </button>
        {categories.map((category) => (
          <button
            key={category}
            onClick={() => setSelectedCategory(category)}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              selectedCategory === category
                ? "bg-blue-600 text-white shadow-sm"
                : "bg-white text-gray-700 hover:bg-gray-50 border border-gray-200"
            }`}
          >
            {category}
          </button>
        ))}
      </div>

      {/* Animation List */}
      <div className="space-y-2">
        {filteredAnimations.length === 0 ? (
          <p className="text-gray-500 text-sm text-center py-4">
            No animations found
          </p>
        ) : (
          filteredAnimations.map((animation) => (
            <AnimationCard
              key={animation.id}
              animation={animation}
              isSelected={selectedId === animation.id}
              onSelect={() => onSelect(animation.id)}
            />
          ))
        )}
      </div>
    </div>
  );
}

interface AnimationCardProps {
  animation: AnimationConfig;
  isSelected: boolean;
  onSelect: () => void;
}

function AnimationCard({ animation, isSelected, onSelect }: AnimationCardProps) {
  return (
    <button
      onClick={onSelect}
      className={`w-full text-left p-4 rounded-xl border-2 transition-all ${
        isSelected
          ? "border-blue-500 bg-blue-50 shadow-sm"
          : "border-gray-200 bg-white hover:border-gray-300 hover:shadow-sm"
      }`}
    >
      <div className="flex items-center justify-between">
        <div className="flex-1">
          <h3 className="font-semibold text-gray-900">{animation.name}</h3>
          <div className="flex items-center gap-3 mt-1">
            <span className="text-xs text-gray-500">{animation.category}</span>
            <span className="text-xs text-gray-400">•</span>
            <span className="text-xs text-gray-500">
              Complexity: {animation.complexity}
            </span>
          </div>
        </div>
        {isSelected && (
          <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
        )}
      </div>
    </button>
  );
}

