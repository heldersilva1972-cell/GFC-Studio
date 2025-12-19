// REV: AnimationPlayground / Phase 4B / AnimationList Component
"use client";

import { animations, getAnimationRoute } from "@/app/animations/registry";

interface AnimationListProps {
  selectedId: string | null;
  onSelect: (id: string) => void;
  className?: string;
}

export default function AnimationList({
  selectedId,
  onSelect,
  className = "",
}: AnimationListProps) {
  return (
    <div className={`space-y-2 ${className}`}>
      {animations.map((animation) => {
        const isSelected = selectedId === animation.id;

        return (
          <button
            key={animation.id}
            onClick={() => onSelect(animation.id)}
            className={`w-full text-left p-3 rounded-lg border transition-all ${
              isSelected
                ? "border-blue-500 bg-blue-50"
                : "border-gray-200 bg-white hover:bg-slate-50 hover:border-gray-300"
            }`}
          >
            <div className="flex items-start justify-between">
              <div className="flex-1">
                <h3 className="font-semibold text-gray-900 mb-1">
                  {animation.name}
                </h3>
                <p className="text-sm text-gray-600">
                  {animation.description}
                </p>
                <div className="flex items-center gap-3 mt-2">
                  {animation.category && (
                    <span className="text-xs text-gray-500 capitalize">
                      {animation.category}
                    </span>
                  )}
                  {animation.difficulty && (
                    <>
                      <span className="text-xs text-gray-400">•</span>
                      <span className="text-xs text-gray-500">
                        Difficulty: {animation.difficulty}/3
                      </span>
                    </>
                  )}
                </div>
              </div>
            </div>
          </button>
        );
      })}
    </div>
  );
}

