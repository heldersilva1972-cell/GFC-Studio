"use client";

import React, { useState, useRef } from "react";
import { motion, useScroll, useTransform, useInView } from "framer-motion";

// 1. Parallax Background Section
export function GfcSectionParallaxHero() {
  const [isHovered, setIsHovered] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);
  const { scrollYProgress } = useScroll({
    container: containerRef,
  });
  const backgroundY = useTransform(scrollYProgress, [0, 1], [0, 50]);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div
        ref={containerRef}
        className="relative h-full w-full max-w-4xl overflow-hidden rounded-xl"
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        {/* Background layer */}
        <motion.div
          className="absolute inset-0 bg-gradient-to-br from-blue-900/40 via-slate-800/60 to-slate-900/80"
          animate={{
            y: isHovered ? -10 : 0,
            scale: isHovered ? 1.05 : 1,
          }}
          transition={{ duration: 0.4, ease: "easeOut" }}
          style={{ y: backgroundY }}
        >
          {/* Subtle pattern */}
          <div
            className="absolute inset-0 opacity-10"
            style={{
              backgroundImage:
                "linear-gradient(45deg, rgba(255,255,255,0.1) 25%, transparent 25%), linear-gradient(-45deg, rgba(255,255,255,0.1) 25%, transparent 25%)",
              backgroundSize: "20px 20px",
            }}
          />
        </motion.div>

        {/* Foreground card */}
        <motion.div
          className="relative z-10 flex h-full items-center justify-center p-8"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, ease: "easeOut" }}
        >
          <motion.div
            className="w-full max-w-2xl rounded-2xl border border-slate-700/50 bg-slate-900/90 p-8 shadow-2xl backdrop-blur-sm"
            animate={{
              scale: isHovered ? 1.02 : 1,
            }}
            transition={{ duration: 0.3 }}
          >
            <h2 className="mb-4 text-4xl font-bold text-slate-50 md:text-5xl">
              Gloucester Fraternity Club
            </h2>
            <p className="mb-6 text-lg text-slate-300">
              A welcoming community space for events, gatherings, and celebrations in the heart of Gloucester.
            </p>
            <motion.button
              className="rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-6 py-3 font-semibold text-slate-900 shadow-lg"
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
            >
              Discover the Hall
            </motion.button>
          </motion.div>
        </motion.div>
      </div>
    </div>
  );
}

// 2. History Timeline Section
export function GfcSectionHistoryTimeline() {
  const timelineItems = [
    {
      year: "1930",
      title: "Foundation",
      description: "The Gloucester Fraternity Club was established as a community gathering place.",
    },
    {
      year: "1965",
      title: "Expansion",
      description: "Major renovations expanded the hall to accommodate larger events and celebrations.",
    },
    {
      year: "2000",
      title: "Modernization",
      description: "Updated facilities and amenities to serve the community for generations to come.",
    },
    {
      year: "2024",
      title: "Today",
      description: "Continuing to be a cornerstone of the Gloucester community.",
    },
  ];

  const containerRef = useRef<HTMLDivElement>(null);
  const isInView = useInView(containerRef, { once: true, amount: 0.2 });

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div ref={containerRef} className="relative w-full max-w-2xl">
        {/* Vertical line */}
        <motion.div
          className="absolute left-8 top-0 bottom-0 w-0.5 bg-gradient-to-b from-amber-500 via-amber-400 to-amber-500"
          initial={{ scaleY: 0 }}
          animate={{ scaleY: isInView ? 1 : 0 }}
          transition={{ duration: 1, ease: "easeOut" }}
          style={{ originY: 0 }}
        />

        {/* Timeline items */}
        <div className="space-y-8">
          {timelineItems.map((item, index) => (
            <motion.div
              key={item.year}
              className="relative flex items-start gap-6"
              initial={{ opacity: 0, y: 20 }}
              animate={isInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 20 }}
              transition={{ duration: 0.5, delay: index * 0.15, ease: "easeOut" }}
            >
              {/* Year badge */}
              <div className="relative z-10 flex h-16 w-16 shrink-0 items-center justify-center rounded-full border-2 border-amber-500 bg-slate-900 shadow-lg">
                <span className="text-sm font-bold text-amber-400">{item.year}</span>
              </div>

              {/* Content */}
              <div className="flex-1 rounded-lg border border-slate-700/50 bg-slate-900/80 p-4">
                <h3 className="mb-1 text-lg font-semibold text-slate-100">{item.title}</h3>
                <p className="text-sm text-slate-400">{item.description}</p>
              </div>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 3. Rental Steps Section
export function GfcSectionRentalSteps() {
  const steps = [
    {
      number: "1",
      title: "Check Availability",
      description: "Browse our calendar and find the perfect date for your event.",
    },
    {
      number: "2",
      title: "Submit Request",
      description: "Fill out our simple rental form with your event details.",
    },
    {
      number: "3",
      title: "Confirm & Celebrate",
      description: "Receive confirmation and enjoy your special day at the GFC hall.",
    },
  ];

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-4xl">
        <h2 className="mb-8 text-center text-3xl font-bold text-slate-100">Rental Process</h2>
        <div className="grid gap-6 md:grid-cols-3">
          {steps.map((step, index) => (
            <motion.div
              key={step.number}
              className="group relative rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 shadow-lg"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: index * 0.1, ease: "easeOut" }}
              whileHover={{
                scale: 1.05,
                borderColor: "rgba(251, 191, 36, 0.5)",
                boxShadow: "0 20px 40px rgba(0,0,0,0.3)",
              }}
            >
              {/* Step number badge */}
              <motion.div
                className="mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-gradient-to-br from-amber-500 to-amber-600 text-xl font-bold text-slate-900 shadow-md"
                whileHover={{ scale: [1, 1.2, 1], rotate: [0, 5, -5, 0] }}
                transition={{ duration: 0.4 }}
              >
                {step.number}
              </motion.div>

              <h3 className="mb-2 text-xl font-semibold text-slate-100">{step.title}</h3>
              <p className="text-sm text-slate-400">{step.description}</p>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 4. Wave Section Divider
export function GfcSectionWaveDivider() {
  const ref = useRef<HTMLDivElement>(null);
  const isInView = useInView(ref, { once: true, amount: 0.3 });

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <motion.div
        ref={ref}
        className="relative w-full max-w-4xl overflow-hidden rounded-xl"
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.6 }}
      >
        {/* Top section */}
        <div className="h-24 bg-gradient-to-br from-slate-900 to-slate-800" />

        {/* Wave divider */}
        <div className="relative h-32 bg-gradient-to-b from-slate-800 to-slate-700">
          <svg
            className="absolute inset-0 h-full w-full"
            viewBox="0 0 1200 120"
            preserveAspectRatio="none"
          >
            <defs>
              <linearGradient id="waveGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stopColor="#fbbf24" stopOpacity="0.3" />
                <stop offset="50%" stopColor="#f59e0b" stopOpacity="0.5" />
                <stop offset="100%" stopColor="#fbbf24" stopOpacity="0.3" />
              </linearGradient>
            </defs>
            <motion.path
              d="M0,60 Q300,20 600,60 T1200,60 L1200,120 L0,120 Z"
              fill="url(#waveGradient)"
              initial={{ pathLength: 0, opacity: 0 }}
              animate={isInView ? { pathLength: 1, opacity: 1 } : { pathLength: 0, opacity: 0 }}
              transition={{ duration: 1.5, ease: "easeInOut" }}
            />
            <motion.path
              d="M0,80 Q300,40 600,80 T1200,80 L1200,120 L0,120 Z"
              fill="rgba(251, 191, 36, 0.2)"
              initial={{ pathLength: 0, opacity: 0 }}
              animate={isInView ? { pathLength: 1, opacity: 1 } : { pathLength: 0, opacity: 0 }}
              transition={{ duration: 1.5, delay: 0.2, ease: "easeInOut" }}
            />
          </svg>
        </div>

        {/* Bottom section */}
        <div className="flex h-24 items-center justify-center bg-gradient-to-br from-slate-700 to-slate-800">
          <span className="text-sm font-medium text-slate-300">Next section</span>
        </div>
      </motion.div>
    </div>
  );
}

// 5. Sticky Mini Nav Demo
export function GfcSectionStickyMiniNavDemo() {
  const [activeSection, setActiveSection] = useState("overview");
  const containerRef = useRef<HTMLDivElement>(null);
  const { scrollYProgress } = useScroll({
    container: containerRef,
  });

  const sections = [
    { id: "overview", label: "Overview", content: "Learn about the Gloucester Fraternity Club and our rich history in the community." },
    { id: "photos", label: "Photos", content: "Browse photos of our beautiful hall, event spaces, and community gatherings." },
    { id: "pricing", label: "Pricing", content: "View our rental rates and packages for different event types and durations." },
    { id: "faqs", label: "FAQs", content: "Find answers to common questions about renting the hall and planning your event." },
  ];

  // Simulate active section based on scroll progress
  React.useEffect(() => {
    const unsubscribe = scrollYProgress.on("change", (latest) => {
      if (latest < 0.25) setActiveSection("overview");
      else if (latest < 0.5) setActiveSection("photos");
      else if (latest < 0.75) setActiveSection("pricing");
      else setActiveSection("faqs");
    });
    return () => unsubscribe();
  }, [scrollYProgress]);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="relative h-full w-full max-w-3xl overflow-hidden rounded-xl border border-slate-700/50 bg-slate-900/80">
        {/* Sticky nav */}
        <motion.nav
          className="sticky top-0 z-20 border-b border-slate-700/50 bg-slate-900/95 backdrop-blur-sm"
          initial={{ y: -10, opacity: 0 }}
          animate={{ y: 0, opacity: 1 }}
          transition={{ duration: 0.4 }}
        >
          <div className="flex gap-1 p-2">
            {sections.map((section) => (
              <button
                key={section.id}
                onClick={() => {
                  if (containerRef.current) {
                    const element = containerRef.current.querySelector(`#${section.id}`);
                    element?.scrollIntoView({ behavior: "smooth", block: "start" });
                  }
                }}
                className="relative px-4 py-2 text-sm font-medium text-slate-400 transition-colors hover:text-slate-200"
              >
                {section.label}
                {activeSection === section.id && (
                  <motion.div
                    className="absolute bottom-0 left-0 right-0 h-0.5 bg-amber-400"
                    layoutId="activeNav"
                    transition={{ duration: 0.3 }}
                  />
                )}
              </button>
            ))}
          </div>
        </motion.nav>

        {/* Scrollable content */}
        <div
          ref={containerRef}
          className="h-[calc(100%-60px)] overflow-y-auto"
        >
          {sections.map((section, index) => (
            <motion.section
              key={section.id}
              id={section.id}
              className="min-h-[300px] border-b border-slate-800/50 p-8"
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true, amount: 0.3 }}
              transition={{ duration: 0.5, delay: index * 0.1 }}
            >
              <h3 className="mb-4 text-2xl font-bold text-slate-100">{section.label}</h3>
              <p className="text-slate-300">{section.content}</p>
              <div className="mt-6 space-y-3">
                {Array.from({ length: 3 }).map((_, i) => (
                  <div
                    key={i}
                    className="h-20 rounded-lg bg-slate-800/50 border border-slate-700/30"
                  />
                ))}
              </div>
            </motion.section>
          ))}
        </div>
      </div>
    </div>
  );
}

