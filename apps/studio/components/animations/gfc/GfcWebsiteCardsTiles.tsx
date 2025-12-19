"use client";

import React, { useState, useEffect } from "react";
import { motion, useMotionValue, useSpring, useTransform, useAnimationControls } from "framer-motion";

// 1. Floating Membership Stats Panel
export function GfcCardMembershipStats({
  stat = 327,
  label = "Members",
  icon = "👥",
}: {
  stat?: number;
  label?: string;
  icon?: string;
}) {
  const [displayStat, setDisplayStat] = useState(0);
  const y = useMotionValue(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setDisplayStat((prev) => {
        if (prev < stat) {
          const increment = Math.ceil((stat - prev) / 10);
          return Math.min(prev + increment, stat);
        }
        return prev;
      });
    }, 50);
    return () => clearInterval(interval);
  }, [stat]);

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-8 shadow-xl"
        animate={{ y: [0, -8, 0] }}
        transition={{
          duration: 3,
          repeat: Infinity,
          ease: "easeInOut",
        }}
        whileHover={{ scale: 1.05, boxShadow: "0 20px 40px rgba(0,0,0,0.4)" }}
      >
        <div className="absolute right-4 top-4 text-3xl opacity-30">{icon}</div>
        <motion.div
          className="text-5xl font-bold text-amber-400"
          initial={{ opacity: 0, scale: 0.5 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.5 }}
        >
          {displayStat}
        </motion.div>
        <div className="mt-2 text-sm font-medium text-slate-300">{label}</div>
      </motion.div>
    </div>
  );
}

// 2. Hall Feature Carousel Strip
export function GfcCardHallFeatureStrip() {
  const features = [
    { icon: "🍽️", label: "Kitchen" },
    { icon: "🍺", label: "Bar" },
    { icon: "🅿️", label: "Parking" },
    { icon: "🪑", label: "Seating" },
    { icon: "🎵", label: "Sound System" },
    { icon: "💡", label: "Lighting" },
  ];

  const [isHovered, setIsHovered] = useState(false);
  const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);
  const controls = useAnimationControls();

  useEffect(() => {
    if (isHovered) {
      controls.stop();
    } else {
      controls.start({
        x: [0, -600],
        transition: {
          duration: 20,
          repeat: Infinity,
          ease: "linear",
        },
      });
    }
  }, [isHovered, controls]);

  useEffect(() => {
    controls.start({
      x: [0, -600],
      transition: {
        duration: 20,
        repeat: Infinity,
        ease: "linear",
      },
    });
  }, [controls]);

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative w-full max-w-2xl overflow-hidden rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6"
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        <motion.div
          className="flex gap-6"
          animate={controls}
          style={{ width: "200%" }}
        >
          {[...features, ...features].map((feature, idx) => (
            <motion.div
              key={idx}
              className="flex min-w-[120px] flex-col items-center gap-2 rounded-lg border border-slate-700/30 bg-slate-800/50 px-4 py-3"
              onMouseEnter={() => setHoveredIndex(idx)}
              onMouseLeave={() => setHoveredIndex(null)}
              animate={{
                scale: hoveredIndex === idx ? 1.1 : 1,
              }}
              transition={{ duration: 0.2 }}
            >
              <div className="text-2xl">{feature.icon}</div>
              <div className="text-xs font-medium text-slate-300">{feature.label}</div>
            </motion.div>
          ))}
        </motion.div>
      </motion.div>
    </div>
  );
}

// 3. Hover-Reveal Hall Floorplan Card
export function GfcCardFloorplanReveal() {
  const [isHovered, setIsHovered] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative h-64 w-full max-w-md overflow-hidden rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95"
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        {/* Photo layer */}
        <motion.div
          className="absolute inset-0 bg-gradient-to-br from-blue-900/60 to-slate-800/80"
          animate={{
            x: isHovered ? -20 : 0,
            opacity: isHovered ? 0.3 : 1,
          }}
          transition={{ duration: 0.4 }}
        />

        {/* Floorplan layer */}
        <motion.div
          className="absolute inset-0 flex items-center justify-center"
          animate={{
            opacity: isHovered ? 1 : 0,
            scale: isHovered ? 1 : 0.9,
          }}
          transition={{ duration: 0.4 }}
        >
          <div className="relative h-48 w-48">
            {/* Simplified floorplan */}
            <svg viewBox="0 0 200 200" className="h-full w-full">
              <rect x="20" y="20" width="160" height="160" fill="none" stroke="#fbbf24" strokeWidth="2" />
              <rect x="40" y="40" width="50" height="40" fill="none" stroke="#fbbf24" strokeWidth="1.5" />
              <rect x="110" y="40" width="50" height="40" fill="none" stroke="#fbbf24" strokeWidth="1.5" />
              <rect x="40" y="100" width="120" height="60" fill="none" stroke="#fbbf24" strokeWidth="1.5" />
            </svg>

            {/* Icons */}
            {["🍺", "🍽️", "💃"].map((icon, idx) => (
              <motion.div
                key={idx}
                className="absolute text-xl"
                style={{
                  left: idx === 0 ? "30%" : idx === 1 ? "60%" : "45%",
                  top: idx === 0 ? "30%" : idx === 1 ? "30%" : "70%",
                }}
                initial={{ opacity: 0, scale: 0 }}
                animate={{
                  opacity: isHovered ? 1 : 0,
                  scale: isHovered ? 1 : 0,
                }}
                transition={{ duration: 0.3, delay: idx * 0.1 }}
              >
                {icon}
              </motion.div>
            ))}
          </div>
        </motion.div>
      </motion.div>
    </div>
  );
}

// 4. Photo Mosaic Hover Overlay
export function GfcCardPhotoMosaic() {
  const [isHovered, setIsHovered] = useState(false);
  const gradients = [
    "from-blue-900/80 to-slate-800/80",
    "from-amber-900/80 to-slate-800/80",
    "from-violet-900/80 to-slate-800/80",
    "from-emerald-900/80 to-slate-800/80",
    "from-rose-900/80 to-slate-800/80",
    "from-cyan-900/80 to-slate-800/80",
  ];

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative grid h-64 w-full max-w-md grid-cols-3 gap-2 rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-4"
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        {gradients.map((gradient, idx) => (
          <motion.div
            key={idx}
            className={`rounded-lg bg-gradient-to-br ${gradient}`}
            animate={{
              scale: isHovered ? 1 : 1.1,
              filter: isHovered ? "saturate(1)" : "saturate(0.3)",
            }}
            transition={{ duration: 0.3 }}
          />
        ))}

        {/* Overlay */}
        <motion.div
          className="absolute inset-0 flex items-end justify-center rounded-xl bg-gradient-to-t from-black/80 via-black/40 to-transparent p-4"
          animate={{
            opacity: isHovered ? 1 : 0,
          }}
          transition={{ duration: 0.3 }}
        >
          <motion.div
            className="flex items-center gap-2 text-slate-100"
            animate={{
              y: isHovered ? 0 : 10,
            }}
            transition={{ duration: 0.3 }}
          >
            <span className="text-lg">🔍</span>
            <span className="text-sm font-medium">View Gallery</span>
          </motion.div>
        </motion.div>
      </motion.div>
    </div>
  );
}

// 5. Event Card Hover Shadow
export const GfcCardEventHover: React.FC = () => {
  const date = "23";
  const month = "MAR";
  const title = "Community Dinner Night";
  const description = "Join us for a night of great food, friends, and community at the GFC hall.";
  const cta = "More Details";

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <motion.article
        initial={{ opacity: 0, y: 12 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.35, ease: "easeOut" }}
        whileHover={{
          y: -4,
          boxShadow: "0 18px 45px rgba(0,0,0,0.55)",
        }}
        className="relative max-w-md w-full rounded-2xl border border-slate-700 bg-slate-900/80 p-4 shadow-lg backdrop-blur-sm"
      >
        {/* Date badge */}
        <motion.div
          whileHover={{ y: -2 }}
          className="absolute -top-4 left-4 rounded-xl bg-gradient-to-b from-amber-400 to-amber-600 px-3 py-2 text-center text-xs font-bold text-slate-900 shadow-md"
        >
          <motion.div layout className="text-lg leading-none">
            {date}
          </motion.div>
          <div className="text-[0.6rem] tracking-wide">{month}</div>
        </motion.div>

        {/* Content */}
        <div className="mt-4 space-y-2">
          <h3 className="text-lg font-semibold text-slate-50">{title}</h3>
          <p className="text-sm text-slate-400">{description}</p>
        </div>

        {/* Meta row */}
        <div className="mt-4 flex items-center justify-between text-xs text-slate-400">
          <div className="flex flex-col">
            <span>Gloucester Fraternity Club Hall</span>
            <span className="text-[0.7rem] text-slate-500">Doors open at 6:00 PM</span>
          </div>
          {/* CTA */}
          <motion.button
            whileHover={{ x: 2 }}
            className="inline-flex items-center gap-1 rounded-full bg-slate-800 px-3 py-1 text-[0.7rem] font-medium text-amber-300"
          >
            <span>{cta}</span>
            <motion.span
              initial={false}
              animate={{ x: [0, 2, 0] }}
              transition={{
                repeat: Infinity,
                repeatDelay: 1.4,
                duration: 0.5,
                ease: "easeInOut",
              }}
              className="inline-block"
            >
              →
            </motion.span>
          </motion.button>
        </div>
      </motion.article>
    </div>
  );
};

// 6. Team Member Card Fade-Up
export function GfcCardTeamMember({
  name = "John Smith",
  role = "President",
  caption = "Leading the club since 2020",
  initials = "JS",
}: {
  name?: string;
  role?: string;
  caption?: string;
  initials?: string;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative w-full max-w-xs rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 text-center shadow-lg"
        initial={{ opacity: 0, y: 10 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
        whileHover={{
          scale: 1.05,
          borderColor: "rgba(251, 191, 36, 0.5)",
          backgroundColor: "rgba(15, 23, 42, 0.98)",
        }}
      >
        {/* Avatar */}
        <motion.div
          className="mx-auto mb-4 flex h-20 w-20 items-center justify-center rounded-full bg-gradient-to-br from-amber-500/20 to-amber-600/20 text-2xl font-bold text-amber-300"
          initial={{ scale: 0 }}
          animate={{ scale: 1 }}
          transition={{ duration: 0.5, delay: 0.2, type: "spring" }}
        >
          {initials}
        </motion.div>

        <h3 className="mb-1 text-lg font-bold text-slate-100">{name}</h3>
        <p className="mb-2 text-sm font-medium text-amber-400">{role}</p>
        <p className="text-xs text-slate-400">{caption}</p>
      </motion.div>
    </div>
  );
}

