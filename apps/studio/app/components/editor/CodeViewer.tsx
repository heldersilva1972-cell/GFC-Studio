"use client";

import React from "react";

interface CodeViewerProps {
  sourceCode?: string;
  filePathHint?: string;
}

export function CodeViewer({ sourceCode, filePathHint }: CodeViewerProps) {
  const [copied, setCopied] = React.useState(false);

  const handleCopy = async () => {
    if (!sourceCode) return;
    try {
      await navigator.clipboard.writeText(sourceCode);
      setCopied(true);
      window.setTimeout(() => setCopied(false), 1500);
    } catch {
      // Fail silently; no runtime crash
    }
  };

  const hasCode = Boolean(sourceCode && sourceCode.trim().length > 0);

  return (
    <div className="flex h-full flex-col rounded-2xl border border-slate-700/70 bg-slate-900/80 shadow-lg">
      <div className="flex items-center justify-between border-b border-slate-700/70 px-3 py-2 text-xs md:text-sm">
        <div className="flex flex-col">
          <span className="font-medium text-slate-100">Code Viewer</span>
          {filePathHint && (
            <span className="text-[0.7rem] text-slate-400 md:text-xs">
              {filePathHint}
            </span>
          )}
        </div>
        <button
          type="button"
          onClick={handleCopy}
          disabled={!hasCode}
          className="rounded-full border border-slate-600/80 px-3 py-1 text-xs font-medium text-slate-100 transition hover:bg-slate-700/80 disabled:cursor-not-allowed disabled:opacity-40"
        >
          {copied ? "Copied!" : "Copy"}
        </button>
      </div>

      <div className="relative flex-1 overflow-hidden rounded-b-2xl bg-slate-950/90">
        {hasCode ? (
          <pre className="h-full w-full overflow-auto p-3 text-[0.7rem] leading-relaxed text-slate-100 md:p-4 md:text-xs">
            <code>{sourceCode}</code>
          </pre>
        ) : (
          <div className="flex h-full items-center justify-center px-4 py-6 text-center text-xs text-slate-400 md:text-sm">
            <div className="space-y-1">
              <p>No source code is attached to this animation yet.</p>
              {filePathHint && (
                <p className="text-[0.7rem] text-slate-500 md:text-xs">
                  Open this file in Cursor to view and edit the full implementation.
                </p>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default CodeViewer;

