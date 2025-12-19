"use client";

import React, { useState, useRef } from "react";
import { motion, AnimatePresence } from "framer-motion";

export function ImageUploaderGallery() {
  const [files, setFiles] = useState<File[]>([]);
  const [isDragging, setIsDragging] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    setIsDragging(false);
    const droppedFiles = Array.from(e.dataTransfer.files).filter((file) =>
      file.type.startsWith("image/")
    );
    setFiles((prev) => [...prev, ...droppedFiles]);
  };

  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      const selectedFiles = Array.from(e.target.files);
      setFiles((prev) => [...prev, ...selectedFiles]);
    }
  };

  const removeFile = (index: number) => {
    setFiles((prev) => prev.filter((_, i) => i !== index));
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="w-full max-w-2xl rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 shadow-xl"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <h2 className="mb-4 text-xl font-bold text-slate-100">Upload Images</h2>

        {/* Dropzone */}
        <motion.div
          className="mb-6 rounded-lg border-2 border-dashed p-8 text-center transition-colors"
          animate={{
            borderColor: isDragging ? "#fbbf24" : "rgba(148, 163, 184, 0.3)",
            backgroundColor: isDragging ? "rgba(251, 191, 36, 0.1)" : "transparent",
          }}
          onDragOver={(e) => {
            e.preventDefault();
            setIsDragging(true);
          }}
          onDragLeave={() => setIsDragging(false)}
          onDrop={handleDrop}
          onClick={() => fileInputRef.current?.click()}
        >
          <input
            ref={fileInputRef}
            type="file"
            multiple
            accept="image/*"
            onChange={handleFileSelect}
            className="hidden"
          />
          <motion.div
            animate={{ scale: isDragging ? 1.05 : 1 }}
            transition={{ duration: 0.2 }}
          >
            <div className="mb-2 text-4xl">📸</div>
            <p className="text-slate-300">
              Drag and drop images here, or click to select
            </p>
          </motion.div>
        </motion.div>

        {/* Gallery */}
        {files.length > 0 && (
          <div className="grid grid-cols-3 gap-3">
            <AnimatePresence>
              {files.map((file, index) => (
                <motion.div
                  key={`${file.name}-${index}`}
                  className="group relative aspect-square overflow-hidden rounded-lg bg-slate-800"
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  exit={{ opacity: 0, scale: 0.9 }}
                  transition={{ duration: 0.3 }}
                >
                  <img
                    src={URL.createObjectURL(file)}
                    alt={file.name}
                    className="h-full w-full object-cover"
                  />
                  <button
                    onClick={() => removeFile(index)}
                    className="absolute right-2 top-2 rounded-full bg-red-500/80 p-1.5 opacity-0 transition-opacity group-hover:opacity-100"
                  >
                    <svg
                      width="16"
                      height="16"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="white"
                      strokeWidth="2"
                    >
                      <line x1="18" y1="6" x2="6" y2="18" />
                      <line x1="6" y1="6" x2="18" y2="18" />
                    </svg>
                  </button>
                </motion.div>
              ))}
            </AnimatePresence>
          </div>
        )}
      </motion.div>
    </div>
  );
}

