"use client";

import React from "react";
import { motion, AnimatePresence } from "framer-motion";
import { X } from "lucide-react";
import type { NewSettings, NewIndicatorMode } from "@/app/lib/newSettings";
import { appMeta } from "@/app/lib/appMeta";
import { appRevisionLog } from "@/app/lib/appRevisionLog";
import { promptPhaseLog } from "@/app/lib/promptPhaseLog";

interface PlaygroundSettingsDialogProps {
  isOpen: boolean;
  onClose: () => void;
  newSettings: NewSettings;
  onChangeNewSettings: (settings: NewSettings) => void;
}

export function PlaygroundSettingsDialog({
  isOpen,
  onClose,
  newSettings,
  onChangeNewSettings,
}: PlaygroundSettingsDialogProps) {
  const handleModeChange = (mode: NewIndicatorMode) => {
    onChangeNewSettings({ ...newSettings, mode });
  };

  const handleDurationChange = (durationDays: number) => {
    const clamped = Math.max(1, Math.min(365, durationDays));
    onChangeNewSettings({ ...newSettings, durationDays: clamped });
  };

  return (
    <AnimatePresence>
      {isOpen && (
        <>
          {/* Overlay */}
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={onClose}
            className="fixed inset-0 z-50 bg-black/60 backdrop-blur-sm"
          />

          {/* Dialog */}
          <motion.div
            initial={{ opacity: 0, scale: 0.95, y: 20 }}
            animate={{ opacity: 1, scale: 1, y: 0 }}
            exit={{ opacity: 0, scale: 0.95, y: 20 }}
            className="fixed left-1/2 top-4 z-50 w-full max-w-2xl max-h-[calc(100vh-2rem)] -translate-x-1/2 rounded-2xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-950/95 shadow-2xl flex flex-col"
          >
            {/* Header */}
            <div className="flex-shrink-0 flex items-center justify-between p-6 pb-4 border-b border-slate-800/50">
              <h2 className="text-xl font-bold text-slate-100">Settings</h2>
              <button
                onClick={onClose}
                className="rounded-lg p-1.5 text-slate-400 transition-colors hover:bg-slate-800/50 hover:text-slate-100"
              >
                <X className="h-5 w-5" />
              </button>
            </div>

            {/* Content - Scrollable */}
            <div className="flex-1 overflow-y-auto p-6 space-y-6">
              {/* App Version & Revision */}
              <div>
                <label className="mb-3 block text-sm font-semibold text-slate-200">
                  App Version & Revision
                </label>
                <div className="rounded-lg border border-slate-700/50 bg-slate-800/30 p-4 space-y-3">
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-slate-400">App Version</span>
                    <span className="text-sm font-medium text-slate-200">{appMeta.appVersion}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-slate-400">Revision</span>
                    <span className="text-sm font-medium text-slate-200">{appMeta.revision}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-slate-400">Last Updated</span>
                    <span className="text-sm font-medium text-slate-200">
                      {new Date(appMeta.lastUpdated).toLocaleDateString()}
                    </span>
                  </div>
                </div>
                <p className="mt-2 text-xs text-slate-500">
                  This version and revision apply only to the Animation Playground app and are tracked separately from any global project or package.json version.
                </p>
              </div>

              {/* Revision History */}
              <div className="border-t border-slate-800/50 pt-6">
                <label className="mb-3 block text-sm font-semibold text-slate-200">
                  Revision History
                </label>
                {appRevisionLog.log.length === 0 ? (
                  <div className="rounded-lg border border-slate-700/50 bg-slate-800/30 p-4 text-center">
                    <p className="text-sm text-slate-400">
                      No revision history yet. Future Cursor prompts can append entries to data/appRevisionLog.json.
                    </p>
                  </div>
                ) : (
                  <div className="rounded-lg border border-slate-700/50 bg-slate-800/30 overflow-hidden max-h-[400px] overflow-y-auto">
                    <div className="p-4 space-y-6">
                      {appRevisionLog.log.map((entry, index) => (
                        <div key={index} className="border-b border-slate-700/30 last:border-b-0 pb-4 last:pb-0">
                          <div className="mb-2">
                            <h4 className="text-sm font-semibold text-slate-200">
                              Revision {entry.revision} – Phase: {entry.phase} – {new Date(entry.date).toLocaleDateString()}
                            </h4>
                          </div>
                          <ul className="list-disc list-inside space-y-1 ml-2">
                            {entry.changes.map((change, changeIndex) => (
                              <li key={changeIndex} className="text-sm text-slate-400">
                                {change}
                              </li>
                            ))}
                          </ul>
                        </div>
                      ))}
                    </div>
                  </div>
                )}
              </div>

              {/* New Indicator Mode */}
              <div>
                <label className="mb-3 block text-sm font-semibold text-slate-200">
                  New Indicator Mode
                </label>
                <div className="space-y-2">
                  {[
                    { value: "duration" as const, label: "Duration only" },
                    { value: "latest" as const, label: "Latest only" },
                    { value: "duration-or-latest" as const, label: "Duration OR latest" },
                  ].map((option) => (
                    <label
                      key={option.value}
                      className="flex cursor-pointer items-center gap-3 rounded-lg border border-slate-700/50 bg-slate-800/30 p-3 transition-colors hover:bg-slate-800/50"
                    >
                      <input
                        type="radio"
                        name="newMode"
                        value={option.value}
                        checked={newSettings.mode === option.value}
                        onChange={() => handleModeChange(option.value)}
                        className="h-4 w-4 cursor-pointer accent-amber-500"
                      />
                      <span className="text-sm text-slate-300">{option.label}</span>
                    </label>
                  ))}
                </div>
              </div>

              {/* New Duration */}
              <div>
                <label className="mb-3 block text-sm font-semibold text-slate-200">
                  New Duration (days)
                </label>
                <div className="flex items-center gap-3">
                  <input
                    type="number"
                    min={1}
                    max={365}
                    value={newSettings.durationDays}
                    onChange={(e) => handleDurationChange(parseInt(e.target.value) || 30)}
                    className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-slate-100 placeholder-slate-500 focus:border-amber-500/50 focus:outline-none focus:ring-2 focus:ring-amber-500/20"
                  />
                  <span className="text-sm text-slate-400">days</span>
                </div>
                <p className="mt-2 text-xs text-slate-500">
                  Presets added within this duration will be marked as "New"
                </p>
              </div>

              {/* Prompt / Phase Tracking */}
              <div className="border-t border-slate-800/50 pt-6">
                <label className="mb-3 block text-sm font-semibold text-slate-200">
                  Prompt / Phase Tracking
                </label>
                <p className="mb-4 text-xs text-slate-500">
                  This log tracks Cursor prompt/phase installs only. It does NOT reflect the app's official version number.
                </p>
                {promptPhaseLog.entries.length === 0 ? (
                  <div className="rounded-lg border border-slate-700/50 bg-slate-800/30 p-4 text-center">
                    <p className="text-sm text-slate-400">
                      No phases recorded yet. Future Cursor phase prompts can append entries to data/promptPhaseLog.json.
                    </p>
                  </div>
                ) : (
                  <div className="rounded-lg border border-slate-700/50 bg-slate-800/30 overflow-hidden">
                    <div className="overflow-x-auto">
                      <table className="w-full text-sm">
                        <thead>
                          <tr className="border-b border-slate-700/50 bg-slate-800/50">
                            <th className="px-4 py-3 text-left text-xs font-semibold text-slate-300 uppercase tracking-wider">
                              Phase
                            </th>
                            <th className="px-4 py-3 text-left text-xs font-semibold text-slate-300 uppercase tracking-wider">
                              Prompt Revision
                            </th>
                            <th className="px-4 py-3 text-left text-xs font-semibold text-slate-300 uppercase tracking-wider">
                              Installed On
                            </th>
                            <th className="px-4 py-3 text-left text-xs font-semibold text-slate-300 uppercase tracking-wider">
                              Description
                            </th>
                          </tr>
                        </thead>
                        <tbody className="divide-y divide-slate-700/30">
                          {promptPhaseLog.entries.map((entry, index) => (
                            <tr key={index} className="hover:bg-slate-800/40 transition-colors">
                              <td className="px-4 py-3 text-slate-200 font-medium">
                                {entry.phase}
                              </td>
                              <td className="px-4 py-3 text-slate-400">
                                {entry.promptRevision || "-"}
                              </td>
                              <td className="px-4 py-3 text-slate-400">
                                {entry.installedOn}
                              </td>
                              <td className="px-4 py-3 text-slate-400">
                                {entry.description}
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  </div>
                )}
              </div>
            </div>

            {/* Footer */}
            <div className="flex-shrink-0 flex justify-end p-6 pt-4 border-t border-slate-800/50">
              <button
                onClick={onClose}
                className="rounded-lg bg-amber-500/20 px-4 py-2 text-sm font-semibold text-amber-300 transition-colors hover:bg-amber-500/30"
              >
                Close
              </button>
            </div>
          </motion.div>
        </>
      )}
    </AnimatePresence>
  );
}

