"use client";

import { useState } from "react";
import Link from "next/link";
import { ArrowLeft } from "lucide-react";
import AnimationPreview from "@/components/AnimationPreview";
import CodeDisplay from "@/components/CodeDisplay";
import AnimationSelector from "@/components/AnimationSelector";
import { useAnimationEngine } from "@/app/animations/core/useAnimationEngine";

export default function PlaygroundPage() {
  const { state, actions } = useAnimationEngine();
  const [showSelector, setShowSelector] = useState(true);

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-6">
          <Link
            href="/"
            className="inline-flex items-center gap-2 text-sm font-medium text-gray-600 hover:text-gray-900 mb-4 transition-colors"
          >
            <ArrowLeft className="h-4 w-4" />
            <span>Back to Home</span>
          </Link>
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            Animation Playground
          </h1>
          <p className="text-gray-600">
            Select an animation to preview and copy its code
          </p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Left Sidebar - Animation Selector */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-4 lg:sticky lg:top-4">
              <div className="flex items-center justify-between mb-4">
                <h2 className="text-lg font-semibold text-gray-900">
                  Animations
                </h2>
                <button
                  onClick={() => setShowSelector(!showSelector)}
                  className="lg:hidden text-gray-500 hover:text-gray-700"
                  aria-label="Toggle animation selector"
                >
                  <svg
                    className="w-5 h-5"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    {showSelector ? (
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M6 18L18 6M6 6l12 12"
                      />
                    ) : (
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M4 6h16M4 12h16M4 18h16"
                      />
                    )}
                  </svg>
                </button>
              </div>
              {showSelector && (
                <AnimationSelector
                  selectedId={state.selectedAnimationId}
                  onSelect={actions.setSelectedAnimation}
                  variant="list"
                />
              )}
            </div>
          </div>

          {/* Main Content - Preview and Code */}
          <div className="lg:col-span-2 space-y-6">
            {/* Animation Preview */}
            <AnimationPreview
              animationId={state.selectedAnimationId}
              size={state.size}
              speed={state.speed}
              colors={state.colors}
            />

            {/* Code Display */}
            <CodeDisplay animationId={state.selectedAnimationId} />
          </div>
        </div>
      </div>
    </main>
  );
}

