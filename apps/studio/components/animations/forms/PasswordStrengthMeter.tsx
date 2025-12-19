"use client";

import React, { useState } from "react";
import { motion } from "framer-motion";

export function PasswordStrengthMeter() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const calculateStrength = (pwd: string): { level: number; label: string; color: string } => {
    if (pwd.length === 0) return { level: 0, label: "", color: "transparent" };
    if (pwd.length < 6) return { level: 1, label: "Weak", color: "#ef4444" };
    
    let score = 0;
    if (pwd.length >= 8) score++;
    if (/[a-z]/.test(pwd)) score++;
    if (/[A-Z]/.test(pwd)) score++;
    if (/[0-9]/.test(pwd)) score++;
    if (/[^a-zA-Z0-9]/.test(pwd)) score++;

    if (score <= 2) return { level: 2, label: "Medium", color: "#f59e0b" };
    if (score <= 4) return { level: 3, label: "Strong", color: "#10b981" };
    return { level: 4, label: "Very Strong", color: "#22c55e" };
  };

  const strength = calculateStrength(password);
  const segments = 4;
  const filledSegments = strength.level;

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="w-full max-w-md rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 shadow-xl"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <h2 className="mb-6 text-xl font-bold text-slate-100">Create Account</h2>

        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-slate-300">Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2.5 text-slate-100 placeholder-slate-500 focus:border-amber-500/50 focus:outline-none focus:ring-2 focus:ring-amber-500/20"
            placeholder="your@email.com"
          />
        </div>

        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-slate-300">Password</label>
          <div className="relative">
            <input
              type={showPassword ? "text" : "password"}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2.5 pr-10 text-slate-100 placeholder-slate-500 focus:border-amber-500/50 focus:outline-none focus:ring-2 focus:ring-amber-500/20"
              placeholder="Enter password"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-200"
            >
              <motion.svg
                width="20"
                height="20"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                animate={{ rotate: showPassword ? 180 : 0, scale: showPassword ? 1.1 : 1 }}
                transition={{ duration: 0.2 }}
              >
                {showPassword ? (
                  <>
                    <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24" />
                    <line x1="1" y1="1" x2="23" y2="23" />
                  </>
                ) : (
                  <>
                    <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                    <circle cx="12" cy="12" r="3" />
                  </>
                )}
              </motion.svg>
            </button>
          </div>
        </div>

        {password && (
          <div className="mb-4">
            <div className="mb-2 flex gap-1">
              {Array.from({ length: segments }).map((_, i) => (
                <motion.div
                  key={i}
                  className="h-1.5 flex-1 rounded-full bg-slate-700/50"
                  initial={{ scaleX: 0 }}
                  animate={{
                    scaleX: i < filledSegments ? 1 : 0,
                    backgroundColor: i < filledSegments ? strength.color : "#475569",
                  }}
                  transition={{ duration: 0.3, delay: i * 0.05 }}
                />
              ))}
            </div>
            <motion.p
              className="text-xs font-medium"
              animate={{ color: strength.color }}
              transition={{ duration: 0.3 }}
            >
              {strength.label}
            </motion.p>
          </div>
        )}

        <motion.button
          className="w-full rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-4 py-2.5 font-semibold text-slate-900 shadow-lg transition-all hover:from-amber-400 hover:to-amber-500"
          whileHover={{ scale: 1.02 }}
          whileTap={{ scale: 0.98 }}
        >
          Sign Up
        </motion.button>
      </motion.div>
    </div>
  );
}

