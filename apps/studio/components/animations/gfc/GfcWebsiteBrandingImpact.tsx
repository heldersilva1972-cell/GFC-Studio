"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";

// 1. Donation Impact Meter
export function GfcBrandDonationImpactMeter() {
  const [progress, setProgress] = useState(0);
  const targetProgress = 65;

  useEffect(() => {
    const timer = setTimeout(() => {
      setProgress(targetProgress);
    }, 300);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-2 text-xl font-bold text-slate-100">Your Donation's Impact</h3>
        <p className="mb-6 text-sm text-slate-400">
          Help us reach our annual goal and support the Gloucester community
        </p>

        {/* Progress bar */}
        <div className="relative mb-6">
          <div className="h-3 w-full overflow-hidden rounded-full bg-slate-800/50">
            <motion.div
              className="relative h-full bg-gradient-to-r from-amber-500 to-amber-600"
              initial={{ width: 0 }}
              animate={{ width: `${progress}%` }}
              transition={{ duration: 1.5, ease: "easeOut" }}
            >
              {/* Glow effect */}
              <motion.div
                className="absolute inset-0 bg-amber-400/50"
                animate={{ opacity: [0.5, 1, 0.5] }}
                transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
              />
            </motion.div>
          </div>
          {/* Indicator bubble */}
          <motion.div
            className="absolute -top-8 h-6 rounded-full bg-amber-500 px-2 text-xs font-bold text-slate-900 shadow-lg"
            initial={{ left: "0%" }}
            animate={{ left: `${progress}%` }}
            transition={{ duration: 1.5, ease: "easeOut" }}
            style={{ transform: "translateX(-50%)" }}
          >
            {progress}%
          </motion.div>
        </div>

        {/* Impact bullets */}
        <div className="space-y-2">
          <div className="flex items-center gap-2 text-sm text-slate-300">
            <div className="h-1.5 w-1.5 rounded-full bg-amber-500" />
            <span>Supports community dinners</span>
          </div>
          <div className="flex items-center gap-2 text-sm text-slate-300">
            <div className="h-1.5 w-1.5 rounded-full bg-amber-500" />
            <span>Keeps the hall maintained</span>
          </div>
          <div className="flex items-center gap-2 text-sm text-slate-300">
            <div className="h-1.5 w-1.5 rounded-full bg-amber-500" />
            <span>Funds youth programs</span>
          </div>
        </div>
      </div>
    </div>
  );
}

// 2. Animated GFC Crest / Logo Highlight
export function GfcBrandCrestHighlight() {
  const [shinePosition, setShinePosition] = useState(-100);

  useEffect(() => {
    const interval = setInterval(() => {
      setShinePosition(100);
      setTimeout(() => setShinePosition(-100), 1000);
    }, 10000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="relative w-full max-w-md">
        {/* Radial glow background */}
        <motion.div
          className="absolute inset-0 rounded-full"
          style={{
            background: "radial-gradient(circle, rgba(251, 191, 36, 0.2) 0%, rgba(251, 191, 36, 0.05) 50%, transparent 100%)",
            transform: "translate(-50%, -50%)",
            left: "50%",
            top: "50%",
          }}
          animate={{ scale: [1, 1.1, 1], opacity: [0.3, 0.5, 0.3] }}
          transition={{ duration: 4, repeat: Infinity, ease: "easeInOut" }}
        />

        {/* Crest card */}
        <motion.div
          className="relative rounded-2xl border border-slate-700/50 bg-slate-900/80 p-8 shadow-2xl backdrop-blur-sm"
          initial={{ opacity: 0, scale: 0.8 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.6, ease: "easeOut" }}
        >
          {/* Crest circle */}
          <div className="relative mx-auto mb-4 flex h-32 w-32 items-center justify-center rounded-full border-4 border-amber-500/50 bg-gradient-to-br from-amber-500/20 to-amber-600/20">
            <div className="text-center">
              <div className="text-3xl font-bold text-amber-400">GFC</div>
              <div className="text-xs font-medium text-amber-500">EST. 1930</div>
            </div>

            {/* Shine sweep */}
            <motion.div
              className="absolute inset-0 rounded-full"
              style={{
                background: `linear-gradient(90deg, transparent 0%, rgba(251, 191, 36, 0.4) 50%, transparent 100%)`,
                clipPath: "inset(0 0 0 0)",
              }}
              animate={{ x: `${shinePosition}%` }}
              transition={{ duration: 1, ease: "easeInOut" }}
            />
          </div>

          <h3 className="mb-2 text-center text-2xl font-bold text-slate-100">
            Gloucester Fraternity Club
          </h3>
          <p className="text-center text-sm text-slate-400">
            A cornerstone of the Gloucester community since 1930
          </p>
        </motion.div>
      </div>
    </div>
  );
}

// 3. "Perfect For…" Badge Cluster
export function GfcBrandPerfectForBadges() {
  const badges = ["Weddings", "Birthdays", "Fundraisers", "Reunions", "Corporate Events", "Anniversaries"];

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-2xl">
        <h3 className="mb-6 text-center text-2xl font-bold text-slate-100">Perfect for…</h3>
        <div className="flex flex-wrap justify-center gap-3">
          {badges.map((badge, index) => (
            <motion.div
              key={badge}
              className="group relative"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.4, delay: index * 0.1 }}
            >
              <motion.div
                className="rounded-full border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-sm font-medium text-slate-300 transition-all"
                animate={{
                  y: [0, -2, 0],
                }}
                transition={{
                  duration: 3 + index * 0.5,
                  repeat: Infinity,
                  ease: "easeInOut",
                  delay: index * 0.2,
                }}
                whileHover={{
                  scale: 1.1,
                  backgroundColor: "rgba(251, 191, 36, 0.6)",
                  color: "#0f172a",
                  borderColor: "rgba(251, 191, 36, 0.8)",
                  boxShadow: "0 10px 25px rgba(251, 191, 36, 0.3)",
                }}
              >
                {badge}
              </motion.div>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 4. Testimonials Slider
export function GfcBrandTestimonialsSlider() {
  const testimonials = [
    {
      quote: "The GFC hall was the perfect venue for our wedding reception. Beautiful space and excellent service!",
      name: "Sarah & James",
      event: "Wedding Reception",
    },
    {
      quote: "We've been hosting our annual fundraiser here for 5 years. The staff is always accommodating and helpful.",
      name: "Michael Chen",
      event: "Annual Fundraiser",
    },
    {
      quote: "Our family reunion was a huge success thanks to the welcoming atmosphere and great facilities at GFC.",
      name: "Patricia Williams",
      event: "Family Reunion",
    },
  ];

  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % testimonials.length);
    }, 5000);
    return () => clearInterval(interval);
  }, [testimonials.length]);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-2xl rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-6 text-xl font-bold text-slate-100">What our guests say</h3>

        {/* Testimonial content */}
        <div className="relative min-h-[200px]">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentIndex}
              initial={{ opacity: 0, x: 16 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -16 }}
              transition={{ duration: 0.4 }}
              className="absolute inset-0"
            >
              <blockquote className="mb-4 text-lg italic text-slate-200">
                "{testimonials[currentIndex].quote}"
              </blockquote>
              <div className="mt-6">
                <div className="font-semibold text-amber-400">{testimonials[currentIndex].name}</div>
                <div className="text-sm text-slate-400">{testimonials[currentIndex].event}</div>
              </div>
            </motion.div>
          </AnimatePresence>
        </div>

        {/* Navigation dots */}
        <div className="mt-6 flex justify-center gap-2">
          {testimonials.map((_, index) => (
            <button
              key={index}
              onClick={() => setCurrentIndex(index)}
              className={`h-2 rounded-full transition-all ${
                index === currentIndex
                  ? "w-8 bg-amber-500"
                  : "w-2 bg-slate-700 hover:bg-slate-600"
              }`}
              aria-label={`Go to testimonial ${index + 1}`}
            />
          ))}
        </div>
      </div>
    </div>
  );
}

// 5. Membership Types Flip Cards
export function GfcBrandMembershipTypeFlipCards() {
  const membershipTypes = [
    {
      title: "Regular Member",
      front: {
        bullets: ["Monthly meetings", "Event discounts", "Community voting rights"],
      },
      back: {
        title: "How to qualify",
        text: "Open to all Gloucester residents. Annual membership fee applies. Attend monthly meetings and participate in club activities.",
      },
    },
    {
      title: "Life Member",
      front: {
        bullets: ["Lifetime access", "All member benefits", "Recognition plaque"],
      },
      back: {
        title: "How to qualify",
        text: "Awarded after 25+ years of active membership. Includes all regular member benefits plus lifetime status and special recognition.",
      },
    },
    {
      title: "Guest / Visitor",
      front: {
        bullets: ["Event access", "Temporary membership", "Welcome orientation"],
      },
      back: {
        title: "How to qualify",
        text: "Perfect for those interested in joining. Attend events as a guest, meet members, and learn about the club before committing.",
      },
    },
  ];

  const [flippedIndex, setFlippedIndex] = useState<number | null>(null);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-4xl">
        <div className="grid gap-4 md:grid-cols-3">
          {membershipTypes.map((type, index) => (
            <motion.div
              key={type.title}
              className="relative h-64"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.4, delay: index * 0.1 }}
              onMouseEnter={() => setFlippedIndex(index)}
              onMouseLeave={() => setFlippedIndex(null)}
              style={{ perspective: "1000px" }}
            >
              <motion.div
                className="relative h-full w-full"
                animate={{
                  rotateY: flippedIndex === index ? 180 : 0,
                }}
                transition={{ duration: 0.4, ease: "easeInOut" }}
                style={{ transformStyle: "preserve-3d" }}
              >
                {/* Front face */}
                <div
                  className="absolute inset-0 rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 shadow-lg"
                  style={{ backfaceVisibility: "hidden", transform: "rotateY(0deg)" }}
                >
                  <h4 className="mb-4 text-lg font-bold text-amber-400">{type.title}</h4>
                  <ul className="space-y-2">
                    {type.front.bullets.map((bullet, i) => (
                      <li key={i} className="flex items-start gap-2 text-sm text-slate-300">
                        <span className="mt-1 h-1.5 w-1.5 shrink-0 rounded-full bg-amber-500" />
                        <span>{bullet}</span>
                      </li>
                    ))}
                  </ul>
                </div>

                {/* Back face */}
                <div
                  className="absolute inset-0 rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-800/95 to-slate-900/95 p-6 shadow-lg"
                  style={{
                    backfaceVisibility: "hidden",
                    transform: "rotateY(180deg)",
                  }}
                >
                  <h4 className="mb-3 text-lg font-bold text-amber-400">{type.back.title}</h4>
                  <p className="text-sm leading-relaxed text-slate-300">{type.back.text}</p>
                </div>
              </motion.div>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

