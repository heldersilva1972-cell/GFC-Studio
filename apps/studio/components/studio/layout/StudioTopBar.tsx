"use client";

import React from "react";

const StudioTopBar: React.FC = () => {
  return (
    <div className="h-14 min-h-[56px] flex items-center justify-between px-4 gap-4 border-b border-[color:var(--studio-color-border-subtle)] bg-[color:var(--studio-color-bg-elevated)]">
      <div className="flex flex-col leading-tight">
        <span className="text-[11px] uppercase tracking-[0.12em] text-[color:var(--studio-color-text-muted)]">
          GFC Studio
        </span>
        <span className="text-base font-medium text-[color:var(--studio-color-text-strong)]">
          Animation Workspace
        </span>
      </div>

      <div className="flex items-center gap-3">
        <span className="px-2.5 py-1 rounded-full text-xs font-medium border border-[color:var(--studio-color-border-subtle)] bg-[color:var(--studio-color-bg-surface, var(--studio-color-bg-elevated))] text-[color:var(--studio-color-text-strong)]">
          Draft
        </span>

        <div className="flex items-center rounded-full border border-[color:var(--studio-color-border-subtle)] bg-[color:var(--studio-color-bg-surface, var(--studio-color-bg-elevated))] p-0.5">
          <button
            type="button"
            className="px-3 py-1.5 text-xs font-medium rounded-full bg-[color:var(--studio-color-bg-elevated)] text-[color:var(--studio-color-text-strong)] shadow-sm"
          >
            Design
          </button>
          <button
            type="button"
            className="px-3 py-1.5 text-xs font-medium rounded-full text-[color:var(--studio-color-text-muted)] hover:text-[color:var(--studio-color-text-strong)] transition-colors"
          >
            Preview
          </button>
        </div>

        <button
          type="button"
          aria-label="Open studio settings"
          className="w-9 h-9 flex items-center justify-center rounded-full border border-[color:var(--studio-color-border-subtle)] bg-[color:var(--studio-color-bg-surface, var(--studio-color-bg-elevated))] text-[color:var(--studio-color-text-strong)] hover:text-[color:var(--studio-color-text-muted)] transition-colors"
        >
          ⋯
        </button>
      </div>
    </div>
  );
};

export default StudioTopBar;

