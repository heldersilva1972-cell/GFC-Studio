"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";

type Option = { id: string; label: string; color: string };

export function ProductConfiguratorCard() {
  const [selectedFrame, setSelectedFrame] = useState("black");
  const [selectedSeat, setSelectedSeat] = useState("red");
  const [selectedWheels, setSelectedWheels] = useState("silver");

  const frames: Option[] = [
    { id: "black", label: "Black", color: "#1f2937" },
    { id: "blue", label: "Blue", color: "#3b82f6" },
    { id: "red", label: "Red", color: "#ef4444" },
  ];

  const seats: Option[] = [
    { id: "red", label: "Red", color: "#ef4444" },
    { id: "black", label: "Black", color: "#1f2937" },
    { id: "brown", label: "Brown", color: "#92400e" },
  ];

  const wheels: Option[] = [
    { id: "silver", label: "Silver", color: "#9ca3af" },
    { id: "black", label: "Black", color: "#1f2937" },
    { id: "gold", label: "Gold", color: "#fbbf24" },
  ];

  const price = 1299;

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="w-full max-w-md rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 shadow-xl"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        {/* Product Image */}
        <div className="mb-6 flex h-48 items-center justify-center rounded-lg bg-gradient-to-br from-slate-800 to-slate-900">
          <AnimatePresence mode="wait">
            <motion.div
              key={`${selectedFrame}-${selectedSeat}-${selectedWheels}`}
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0, scale: 0.9 }}
              transition={{ duration: 0.3 }}
              className="relative h-32 w-32"
            >
              {/* Simplified bike representation */}
              <svg viewBox="0 0 100 100" className="h-full w-full">
                {/* Frame */}
                <path
                  d="M30 60 L50 30 L70 60 L70 80 L30 80 Z"
                  fill={frames.find((f) => f.id === selectedFrame)?.color || "#1f2937"}
                />
                {/* Seat */}
                <ellipse
                  cx="50"
                  cy="30"
                  rx="8"
                  ry="5"
                  fill={seats.find((s) => s.id === selectedSeat)?.color || "#ef4444"}
                />
                {/* Wheels */}
                <circle
                  cx="30"
                  cy="80"
                  r="12"
                  fill={wheels.find((w) => w.id === selectedWheels)?.color || "#9ca3af"}
                />
                <circle
                  cx="70"
                  cy="80"
                  r="12"
                  fill={wheels.find((w) => w.id === selectedWheels)?.color || "#9ca3af"}
                />
              </svg>
            </motion.div>
          </AnimatePresence>
        </div>

        {/* Options */}
        <div className="mb-6 space-y-4">
          <div>
            <label className="mb-2 block text-sm font-medium text-slate-300">Frame</label>
            <div className="flex gap-2">
              {frames.map((frame) => (
                <motion.button
                  key={frame.id}
                  onClick={() => setSelectedFrame(frame.id)}
                  className={`rounded-lg border-2 px-3 py-1.5 text-xs font-medium transition-all ${
                    selectedFrame === frame.id
                      ? "border-amber-500 bg-amber-500/20 text-amber-300"
                      : "border-slate-700 bg-slate-800/50 text-slate-400 hover:border-slate-600"
                  }`}
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                >
                  {frame.label}
                </motion.button>
              ))}
            </div>
          </div>

          <div>
            <label className="mb-2 block text-sm font-medium text-slate-300">Seat</label>
            <div className="flex gap-2">
              {seats.map((seat) => (
                <motion.button
                  key={seat.id}
                  onClick={() => setSelectedSeat(seat.id)}
                  className={`rounded-lg border-2 px-3 py-1.5 text-xs font-medium transition-all ${
                    selectedSeat === seat.id
                      ? "border-amber-500 bg-amber-500/20 text-amber-300"
                      : "border-slate-700 bg-slate-800/50 text-slate-400 hover:border-slate-600"
                  }`}
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                >
                  {seat.label}
                </motion.button>
              ))}
            </div>
          </div>

          <div>
            <label className="mb-2 block text-sm font-medium text-slate-300">Wheels</label>
            <div className="flex gap-2">
              {wheels.map((wheel) => (
                <motion.button
                  key={wheel.id}
                  onClick={() => setSelectedWheels(wheel.id)}
                  className={`rounded-lg border-2 px-3 py-1.5 text-xs font-medium transition-all ${
                    selectedWheels === wheel.id
                      ? "border-amber-500 bg-amber-500/20 text-amber-300"
                      : "border-slate-700 bg-slate-800/50 text-slate-400 hover:border-slate-600"
                  }`}
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                >
                  {wheel.label}
                </motion.button>
              ))}
            </div>
          </div>
        </div>

        {/* Price and CTA */}
        <div className="flex items-center justify-between">
          <div>
            <div className="text-2xl font-bold text-slate-100">${price}</div>
            <div className="text-xs text-slate-400">Starting price</div>
          </div>
          <motion.button
            className="rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-6 py-2.5 font-semibold text-slate-900 shadow-lg"
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
          >
            Customize
          </motion.button>
        </div>
      </motion.div>
    </div>
  );
}

