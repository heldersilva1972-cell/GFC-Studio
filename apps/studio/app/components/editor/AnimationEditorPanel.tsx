"use client";

import React from "react";
import type { AnimationConfig } from "@/app/animations/core/types";
import { CodeViewer } from "./CodeViewer";

interface AnimationEditorPanelProps {
  animation?: AnimationConfig | null;
}

export function AnimationEditorPanel({ animation }: AnimationEditorPanelProps) {
  if (!animation) {
    return (
      <div className="rounded-2xl border border-slate-700/70 bg-slate-900/80 p-4 text-sm text-slate-300 shadow-lg">
        Select an animation tile to view details and code.
      </div>
    );
  }

  const { name, category, complexity, editor } = animation;
  const isAdvanced = complexity >= 3;

  return (
    <div className="flex flex-col gap-3 rounded-2xl border border-slate-700/70 bg-slate-900/80 p-3 shadow-xl md:p-4">
      {/* Header */}
      <div className="flex flex-wrap items-center justify-between gap-2">
        <div className="space-y-0.5">
          <div className="flex items-center gap-2">
            <h2 className="text-sm font-semibold text-slate-50 md:text-base">
              {name}
            </h2>
            {isAdvanced && (
              <span className="rounded-full bg-fuchsia-600/30 px-2 py-0.5 text-[0.65rem] font-semibold uppercase tracking-wide text-fuchsia-200">
                Advanced
              </span>
            )}
          </div>
          <div className="flex flex-wrap items-center gap-2 text-[0.7rem] text-slate-400 md:text-xs">
            <span className="rounded-full border border-slate-600/70 px-2 py-0.5">
              Category: {category}
            </span>
          </div>
        </div>
      </div>

      {/* Description / notes */}
      {(editor?.notes || editor?.suggestedUse) && (
        <div className="grid gap-3 md:grid-cols-[minmax(0,1.4fr)_minmax(0,1.6fr)]">
          <div className="space-y-1.5 text-xs text-slate-300 md:text-sm">
            {editor?.notes && (
              <p className="text-slate-200">
                <span className="font-semibold text-slate-100">Notes: </span>
                {editor.notes}
              </p>
            )}
          </div>
          {editor?.suggestedUse && (
            <div className="rounded-xl bg-slate-950/80 px-3 py-2 text-[0.7rem] text-slate-200 md:text-xs">
              <p className="font-semibold text-slate-100">Suggested use</p>
              <p className="mt-0.5 text-slate-300">
                {editor.suggestedUse}
              </p>
            </div>
          )}
        </div>
      )}

      {/* Code viewer */}
      <div className="min-h-[180px] md:min-h-[220px]">
        <CodeViewer
          sourceCode={editor?.sourceCode}
          filePathHint={editor?.filePathHint}
        />
      </div>
    </div>
  );
}

export default AnimationEditorPanel;

