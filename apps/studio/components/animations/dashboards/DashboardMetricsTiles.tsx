"use client";

import React from "react";
import { motion } from "framer-motion";

type Metric = {
  label: string;
  value: string;
  delta: string;
  isPositive: boolean;
};

export function DashboardMetricsTiles() {
  const metrics: Metric[] = [
    { label: "Revenue", value: "$45,231", delta: "+12.5%", isPositive: true },
    { label: "Active Users", value: "2,341", delta: "+8.2%", isPositive: true },
    { label: "Conversion", value: "3.2%", delta: "-2.1%", isPositive: false },
    { label: "Retention", value: "89.4%", delta: "+5.3%", isPositive: true },
  ];

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <div className="grid w-full max-w-2xl grid-cols-2 gap-4">
        {metrics.map((metric, index) => (
          <motion.div
            key={metric.label}
            className="rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-6 shadow-lg"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: index * 0.1 }}
            whileHover={{
              y: -4,
              boxShadow: "0 20px 40px rgba(0,0,0,0.3)",
            }}
          >
            <div className="mb-2 text-sm font-medium text-slate-400">{metric.label}</div>
            <div className="mb-2 text-2xl font-bold text-slate-100">{metric.value}</div>
            <motion.div
              className="flex items-center gap-1 text-xs font-medium"
              animate={{
                color: metric.isPositive ? "#10b981" : "#ef4444",
              }}
              whileHover={{ scale: 1.1 }}
            >
              <motion.span
                animate={{
                  y: [0, -2, 0],
                }}
                transition={{
                  duration: 2,
                  repeat: Infinity,
                  ease: "easeInOut",
                }}
              >
                {metric.isPositive ? "↑" : "↓"}
              </motion.span>
              {metric.delta}
            </motion.div>
          </motion.div>
        ))}
      </div>
    </div>
  );
}

