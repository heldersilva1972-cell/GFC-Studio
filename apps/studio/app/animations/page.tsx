"use client";

import { useState } from "react";
import Link from "next/link";
import { ArrowLeft } from "lucide-react";
import { animations, getAnimationRoute } from "./registry";

export default function AnimationsPage() {
  const [selectedId, setSelectedId] = useState<string | null>(null);

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div className="mb-8">
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
          <p className="text-lg text-gray-600">
            Browse and select animations to preview and explore.
          </p>
        </div>

        <div className="space-y-2">
          {animations.map((animation) => {
            const route = getAnimationRoute(animation.id);
            const isSelected = selectedId === animation.id;

            return (
              <Link
                key={animation.id}
                href={route}
                onClick={() => setSelectedId(animation.id)}
                className={`block p-3 rounded-lg border transition-all ${
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
              </Link>
            );
          })}
        </div>
      </div>
    </main>
  );
}
