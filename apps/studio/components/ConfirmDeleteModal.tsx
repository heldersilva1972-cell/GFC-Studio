"use client";

import React from "react";
import { motion, AnimatePresence } from "framer-motion";
import { AlertTriangle, X } from "lucide-react";

interface ConfirmDeleteModalProps {
  isOpen: boolean;
  animationName: string;
  onConfirm: () => void;
  onCancel: () => void;
}

export function ConfirmDeleteModal({
  isOpen,
  animationName,
  onConfirm,
  onCancel,
}: ConfirmDeleteModalProps) {
  return (
    <AnimatePresence>
      {isOpen && (
        <>
          {/* Backdrop */}
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.2 }}
            className="fixed inset-0 z-[100] bg-black/60 backdrop-blur-sm"
            onClick={onCancel}
          />
          {/* Modal */}
          <motion.div
            initial={{ opacity: 0, scale: 0.95, y: 20 }}
            animate={{ opacity: 1, scale: 1, y: 0 }}
            exit={{ opacity: 0, scale: 0.95, y: 20 }}
            transition={{ duration: 0.2, ease: "easeOut" }}
            className="fixed left-1/2 top-1/2 z-[101] w-full max-w-md -translate-x-1/2 -translate-y-1/2"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="mx-4 rounded-2xl border border-red-500/30 bg-gradient-to-br from-slate-900/95 to-slate-950/95 p-6 shadow-2xl shadow-black/50 backdrop-blur-xl">
              {/* Header */}
              <div className="mb-4 flex items-start justify-between">
                <div className="flex items-center gap-3">
                  <div className="flex h-10 w-10 items-center justify-center rounded-full border border-red-500/30 bg-red-500/10">
                    <AlertTriangle className="h-5 w-5 text-red-400" />
                  </div>
                  <div>
                    <h3 className="text-lg font-bold text-slate-100">
                      Delete Animation
                    </h3>
                    <p className="text-sm text-slate-400">This action cannot be undone</p>
                  </div>
                </div>
                <button
                  onClick={onCancel}
                  className="rounded-lg p-1.5 text-slate-400 transition-colors hover:bg-slate-800/50 hover:text-slate-200"
                >
                  <X className="h-4 w-4" />
                </button>
              </div>

              {/* Content */}
              <div className="mb-6">
                <p className="text-sm text-slate-300">
                  Are you sure you want to delete{" "}
                  <span className="font-semibold text-slate-100">
                    &ldquo;{animationName}&rdquo;
                  </span>
                  ?
                </p>
                <p className="mt-2 text-xs text-slate-400">
                  This will remove the animation from the registry and delete its files.
                </p>
              </div>

              {/* Actions */}
              <div className="flex gap-3">
                <button
                  onClick={onCancel}
                  className="flex-1 rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2.5 text-sm font-medium text-slate-200 transition-all hover:border-slate-600/50 hover:bg-slate-800"
                >
                  Cancel
                </button>
                <button
                  onClick={onConfirm}
                  className="flex-1 rounded-lg border border-red-500/50 bg-red-500/10 px-4 py-2.5 text-sm font-medium text-red-400 transition-all hover:border-red-500/70 hover:bg-red-500/20"
                >
                  Delete
                </button>
              </div>
            </div>
          </motion.div>
        </>
      )}
    </AnimatePresence>
  );
}

