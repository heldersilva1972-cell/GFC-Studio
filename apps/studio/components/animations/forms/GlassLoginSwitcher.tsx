"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";

export function GlassLoginSwitcher() {
  const [isSignUp, setIsSignUp] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative w-full max-w-2xl overflow-hidden rounded-2xl border border-white/10 bg-white/5 backdrop-blur-xl shadow-2xl"
        initial={{ opacity: 0, scale: 0.95 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.5 }}
      >
        <div className="grid md:grid-cols-2">
          {/* Left: Form */}
          <div className="p-8">
            <div className="mb-6 flex gap-2">
              <button
                onClick={() => setIsSignUp(false)}
                className={`relative px-4 py-2 text-sm font-medium transition-colors ${
                  !isSignUp ? "text-slate-100" : "text-slate-400"
                }`}
              >
                Sign In
                {!isSignUp && (
                  <motion.div
                    className="absolute bottom-0 left-0 right-0 h-0.5 bg-amber-400"
                    layoutId="activeTab"
                  />
                )}
              </button>
              <button
                onClick={() => setIsSignUp(true)}
                className={`relative px-4 py-2 text-sm font-medium transition-colors ${
                  isSignUp ? "text-slate-100" : "text-slate-400"
                }`}
              >
                Sign Up
                {isSignUp && (
                  <motion.div
                    className="absolute bottom-0 left-0 right-0 h-0.5 bg-amber-400"
                    layoutId="activeTab"
                  />
                )}
              </button>
            </div>

            <AnimatePresence mode="wait">
              <motion.div
                key={isSignUp ? "signup" : "signin"}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                exit={{ opacity: 0, x: 20 }}
                transition={{ duration: 0.3 }}
              >
                <div className="space-y-4">
                  <div>
                    <label className="mb-1.5 block text-xs font-medium text-slate-300">
                      Email
                    </label>
                    <input
                      type="email"
                      className="w-full rounded-lg border border-white/10 bg-white/5 px-4 py-2.5 text-sm text-slate-100 placeholder-slate-500 backdrop-blur-sm focus:border-amber-400/50 focus:outline-none"
                      placeholder="your@email.com"
                    />
                  </div>
                  <div>
                    <label className="mb-1.5 block text-xs font-medium text-slate-300">
                      Password
                    </label>
                    <input
                      type="password"
                      className="w-full rounded-lg border border-white/10 bg-white/5 px-4 py-2.5 text-sm text-slate-100 placeholder-slate-500 backdrop-blur-sm focus:border-amber-400/50 focus:outline-none"
                      placeholder="••••••••"
                    />
                  </div>
                  {isSignUp && (
                    <motion.div
                      initial={{ opacity: 0, height: 0 }}
                      animate={{ opacity: 1, height: "auto" }}
                      exit={{ opacity: 0, height: 0 }}
                    >
                      <label className="mb-1.5 block text-xs font-medium text-slate-300">
                        Confirm Password
                      </label>
                      <input
                        type="password"
                        className="w-full rounded-lg border border-white/10 bg-white/5 px-4 py-2.5 text-sm text-slate-100 placeholder-slate-500 backdrop-blur-sm focus:border-amber-400/50 focus:outline-none"
                        placeholder="••••••••"
                      />
                    </motion.div>
                  )}
                  <motion.button
                    className="w-full rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-4 py-2.5 text-sm font-semibold text-slate-900 shadow-lg"
                    whileHover={{ scale: 1.02 }}
                    whileTap={{ scale: 0.98 }}
                  >
                    {isSignUp ? "Create Account" : "Sign In"}
                  </motion.button>
                </div>
              </motion.div>
            </AnimatePresence>
          </div>

          {/* Right: Decorative Panel */}
          <div className="hidden bg-gradient-to-br from-amber-500/20 via-violet-500/20 to-blue-500/20 p-8 md:block">
            <AnimatePresence mode="wait">
              <motion.div
                key={isSignUp ? "signup-panel" : "signin-panel"}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -20 }}
                transition={{ duration: 0.3 }}
                className="flex h-full flex-col justify-center text-center"
              >
                <h3 className="mb-4 text-2xl font-bold text-slate-100">
                  {isSignUp ? "Join Us Today" : "Welcome Back"}
                </h3>
                <p className="text-slate-300">
                  {isSignUp
                    ? "Start your journey with us and unlock exclusive features."
                    : "Sign in to continue to your account and access all features."}
                </p>
              </motion.div>
            </AnimatePresence>
          </div>
        </div>
      </motion.div>
    </div>
  );
}

