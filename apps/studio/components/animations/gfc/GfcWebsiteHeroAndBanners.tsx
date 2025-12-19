"use client";

import React from "react";
import { motion } from "framer-motion";

// 1. GFC Hero Simple Fade
export function GfcHeroSimpleFade({
  title = "Gloucester Fraternity Club",
  subtitle = "A welcoming community space for events, gatherings, and celebrations",
  ctaText = "Rent the Hall",
}: {
  title?: string;
  subtitle?: string;
  ctaText?: string;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative w-full max-w-4xl rounded-2xl border border-slate-700/50 bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 p-12 shadow-2xl"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.6, ease: "easeOut" }}
      >
        <motion.h1
          className="mb-4 text-4xl font-bold tracking-tight text-slate-50 md:text-5xl lg:text-6xl"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, delay: 0.2, ease: "easeOut" }}
        >
          {title}
        </motion.h1>
        <motion.p
          className="mb-8 text-lg text-slate-300 md:text-xl"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, delay: 0.4, ease: "easeOut" }}
        >
          {subtitle}
        </motion.p>
        <motion.button
          className="rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-8 py-3 text-lg font-semibold text-slate-900 shadow-lg transition-all hover:from-amber-400 hover:to-amber-500"
          initial={{ scale: 0, opacity: 0 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{
            duration: 0.5,
            delay: 0.6,
            type: "spring",
            stiffness: 200,
          }}
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
        >
          {ctaText}
        </motion.button>
        <motion.div
          className="absolute inset-0 rounded-2xl bg-gradient-to-r from-amber-500/10 via-transparent to-amber-500/10"
          animate={{
            opacity: [0.3, 0.5, 0.3],
          }}
          transition={{
            duration: 3,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        />
      </motion.div>
    </div>
  );
}

// 2. GFC Hero Diagonal Split
export function GfcHeroDiagonalSplit({
  title = "Hall Rentals",
  subtitle = "Perfect for weddings, parties, and community events",
  ctaText = "See Availability",
}: {
  title?: string;
  subtitle?: string;
  ctaText?: string;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative w-full max-w-5xl overflow-hidden rounded-2xl border border-slate-700/50 bg-slate-900 shadow-2xl"
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.5 }}
      >
        <div className="grid min-h-[400px] grid-cols-1 md:grid-cols-2">
          {/* Left: Text Block */}
          <motion.div
            className="flex flex-col justify-center bg-gradient-to-br from-slate-800 to-slate-900 p-8 md:p-12"
            initial={{ opacity: 0, x: -30 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.7, delay: 0.2, ease: "easeOut" }}
          >
            <h1 className="mb-4 text-3xl font-bold text-slate-50 md:text-4xl lg:text-5xl">
              {title}
            </h1>
            <p className="mb-8 text-slate-300 md:text-lg">{subtitle}</p>
            <motion.button
              className="w-fit rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-6 py-3 font-semibold text-slate-900 shadow-lg transition-all hover:from-amber-400 hover:to-amber-500"
              initial={{ y: 30, opacity: 0 }}
              animate={{ y: 0, opacity: 1 }}
              transition={{
                duration: 0.6,
                delay: 0.8,
                type: "spring",
                stiffness: 200,
                damping: 15,
              }}
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
            >
              {ctaText}
            </motion.button>
          </motion.div>

          {/* Right: Diagonal Panel */}
          <motion.div
            className="relative bg-gradient-to-br from-amber-500 via-amber-600 to-amber-700"
            initial={{ x: 100, skewX: -5, opacity: 0 }}
            animate={{ x: 0, skewX: 0, opacity: 1 }}
            transition={{ duration: 0.8, delay: 0.3, ease: "easeOut" }}
            style={{
              clipPath: "polygon(15% 0%, 100% 0%, 100% 100%, 0% 100%)",
            }}
          >
            <div className="flex h-full items-center justify-center p-8">
              <motion.div
                className="h-48 w-full max-w-xs rounded-lg border-4 border-white/20 bg-slate-800/30 backdrop-blur-sm shadow-xl"
                initial={{ scale: 0.9, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                transition={{ duration: 0.5, delay: 0.9 }}
              >
                <div className="flex h-full items-center justify-center text-slate-200/50">
                  <span className="text-sm">Hall Photo</span>
                </div>
              </motion.div>
            </div>
          </motion.div>
        </div>
      </motion.div>
    </div>
  );
}

// 3. GFC Announcement Ribbon
export function GfcAnnouncementRibbon({
  message = "Special Event: Annual Gala on December 15th",
  linkText = "Learn more",
}: {
  message?: string;
  linkText?: string;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative w-full max-w-4xl overflow-hidden rounded-lg border border-amber-500/30 bg-gradient-to-r from-amber-500/10 via-amber-500/5 to-amber-500/10"
        initial={{ y: -100, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        transition={{
          duration: 0.6,
          ease: "easeOut",
        }}
      >
        <div className="flex items-center gap-4 p-4 md:p-6">
          <motion.div
            initial={{ scale: 0, rotate: -180 }}
            animate={{ scale: 1, rotate: 0 }}
            transition={{
              duration: 0.5,
              delay: 0.3,
              type: "spring",
              stiffness: 200,
            }}
          >
            <svg
              className="h-6 w-6 text-amber-400 md:h-8 md:w-8"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9"
              />
            </svg>
          </motion.div>
          <motion.p
            className="flex-1 text-sm font-medium text-slate-200 md:text-base"
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.5, delay: 0.4 }}
          >
            {message}
          </motion.p>
          {linkText && (
            <motion.a
              href="#"
              className="text-sm font-semibold text-amber-400 underline transition-colors hover:text-amber-300"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ duration: 0.5, delay: 0.5 }}
            >
              {linkText}
            </motion.a>
          )}
        </div>
        {/* Shimmer effect */}
        <motion.div
          className="absolute left-0 top-0 h-full w-1 bg-gradient-to-b from-transparent via-amber-400/50 to-transparent"
          animate={{
            opacity: [0.3, 0.8, 0.3],
          }}
          transition={{
            duration: 2,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        />
      </motion.div>
    </div>
  );
}

// 4. GFC Section Header Gold Underline
export function GfcSectionHeaderGoldUnderline({
  overline = "Hall Rentals",
  title = "Available Spaces",
}: {
  overline?: string;
  title?: string;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <div className="w-full max-w-3xl">
        {overline && (
          <motion.p
            className="mb-2 text-sm font-semibold uppercase tracking-wider text-amber-400"
            initial={{ opacity: 0, y: -10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.4 }}
          >
            {overline}
          </motion.p>
        )}
        <motion.h2
          className="mb-4 text-3xl font-bold text-slate-50 md:text-4xl lg:text-5xl"
          initial={{ opacity: 0, y: 10 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5, delay: 0.2 }}
        >
          {title}
        </motion.h2>
        <div className="relative">
          <motion.div
            className="h-1 bg-gradient-to-r from-amber-500 via-amber-400 to-amber-500"
            initial={{ scaleX: 0 }}
            animate={{ scaleX: 1 }}
            transition={{
              duration: 0.8,
              delay: 0.4,
              ease: "easeOut",
            }}
            style={{ originX: 0 }}
          />
          {/* Spark effect */}
          <motion.div
            className="absolute -top-1 h-3 w-3 rounded-full bg-amber-400 shadow-lg shadow-amber-400/50"
            initial={{ x: 0, opacity: 0, scale: 0 }}
            animate={{
              x: ["0%", "100%"],
              opacity: [0, 1, 1, 0],
              scale: [0, 1, 1, 0],
            }}
            transition={{
              duration: 1.2,
              delay: 0.6,
              ease: "easeInOut",
            }}
          />
        </div>
      </div>
    </div>
  );
}

