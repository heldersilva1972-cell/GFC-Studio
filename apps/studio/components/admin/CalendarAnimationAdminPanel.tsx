"use client";

import React, { useState, useMemo, useCallback } from "react";
import { motion, AnimatePresence } from "framer-motion";
import {
  Search,
  Filter,
  Edit,
  Save,
  X,
  Check,
  Settings,
  Eye,
  EyeOff,
} from "lucide-react";
import {
  calendarAnimationConfigs,
  type CalendarAnimationConfig,
  type CalendarAnimationCategory,
  updateCalendarAnimationConfig,
  getCalendarAnimationConfigsByCategory,
} from "@/app/lib/calendarAnimationConfig";

interface CalendarAnimationAdminPanelProps {
  onConfigChange?: () => void;
}

export default function CalendarAnimationAdminPanel({
  onConfigChange,
}: CalendarAnimationAdminPanelProps) {
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<CalendarAnimationCategory | "all">("all");
  const [showEnabledOnly, setShowEnabledOnly] = useState(false);
  const [selectedConfig, setSelectedConfig] = useState<CalendarAnimationConfig | null>(null);
  const [editingConfig, setEditingConfig] = useState<CalendarAnimationConfig | null>(null);
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState("");

  // Filter and search configs
  const filteredConfigs = useMemo(() => {
    let filtered = [...calendarAnimationConfigs];

    // Category filter
    if (selectedCategory !== "all") {
      filtered = filtered.filter((config) => config.category === selectedCategory);
    }

    // Enabled filter
    if (showEnabledOnly) {
      filtered = filtered.filter((config) => config.enabled);
    }

    // Search filter
    if (searchQuery.trim()) {
      const query = searchQuery.toLowerCase();
      filtered = filtered.filter(
        (config) =>
          config.displayName.toLowerCase().includes(query) ||
          config.description.toLowerCase().includes(query) ||
          config.tags.some((tag) => tag.toLowerCase().includes(query)) ||
          config.id.toLowerCase().includes(query)
      );
    }

    // Sort by category, then name
    filtered.sort((a, b) => {
      if (a.category !== b.category) {
        return a.category.localeCompare(b.category);
      }
      return a.displayName.localeCompare(b.displayName);
    });

    return filtered;
  }, [searchQuery, selectedCategory, showEnabledOnly]);

  const categories: CalendarAnimationCategory[] = [
    "Calendar Availability",
    "Calendar Booked Days",
    "Calendar Add Event",
    "Calendar Status Indicators",
    "Calendar Navigation",
    "Calendar Loading & Feedback",
  ];

  const handleEdit = useCallback((config: CalendarAnimationConfig) => {
    setSelectedConfig(config);
    setEditingConfig({ ...config });
  }, []);

  const handleSave = useCallback(() => {
    if (!editingConfig) return;

    updateCalendarAnimationConfig(editingConfig.id, editingConfig);
    setSelectedConfig(editingConfig);
    setEditingConfig(null);
    setToastMessage("Configuration saved successfully");
    setShowToast(true);
    setTimeout(() => setShowToast(false), 3000);
    onConfigChange?.();
  }, [editingConfig, onConfigChange]);

  const handleCancel = useCallback(() => {
    setEditingConfig(null);
    if (selectedConfig) {
      setEditingConfig({ ...selectedConfig });
    }
  }, [selectedConfig]);

  const handleToggleEnabled = useCallback((config: CalendarAnimationConfig) => {
    updateCalendarAnimationConfig(config.id, { enabled: !config.enabled });
    setToastMessage(`${config.displayName} ${!config.enabled ? "enabled" : "disabled"}`);
    setShowToast(true);
    setTimeout(() => setShowToast(false), 3000);
    onConfigChange?.();
  }, [onConfigChange]);

  const getDefaultUsageSummary = (config: CalendarAnimationConfig): string => {
    const usages: string[] = [];
    if (config.isDefaultFor.hoverDay) usages.push("Hover");
    if (config.isDefaultFor.clickDay) usages.push("Click");
    if (config.isDefaultFor.selectWeek) usages.push("Week");
    if (config.isDefaultFor.monthChange) usages.push("Month");
    if (config.isDefaultFor.bookedDay) usages.push("Booked");
    if (config.isDefaultFor.blockedDay) usages.push("Blocked");
    if (config.isDefaultFor.availableDay) usages.push("Available");
    if (config.isDefaultFor.loadingState) usages.push("Loading");
    if (config.isDefaultFor.statusIndicator) usages.push("Status");
    return usages.length > 0 ? usages.join(", ") : "None";
  };

  return (
    <div className="flex h-full w-full gap-4 overflow-hidden">
      {/* Left: Table View */}
      <div className="flex flex-1 flex-col overflow-hidden rounded-xl border border-slate-800/80 bg-gradient-to-br from-slate-900/90 to-slate-950/90">
        {/* Header */}
        <div className="flex-shrink-0 border-b border-slate-800/50 p-4">
          <div className="mb-4 flex items-center gap-2">
            <Settings className="h-5 w-5 text-slate-300" />
            <h2 className="text-lg font-bold text-slate-100">Calendar Animation Config</h2>
          </div>

          {/* Filters */}
          <div className="flex flex-wrap gap-3">
            {/* Search */}
            <div className="relative flex-1 min-w-[200px]">
              <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
              <input
                type="text"
                placeholder="Search animations..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-2 pl-9 text-sm text-slate-100 placeholder:text-slate-500 focus:border-cyan-500/60 focus:outline-none"
              />
            </div>

            {/* Category Filter */}
            <select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value as CalendarAnimationCategory | "all")}
              className="rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-2 text-sm text-slate-100 focus:border-cyan-500/60 focus:outline-none"
            >
              <option value="all">All Categories</option>
              {categories.map((cat) => (
                <option key={cat} value={cat}>
                  {cat}
                </option>
              ))}
            </select>

            {/* Enabled Filter */}
            <button
              onClick={() => setShowEnabledOnly(!showEnabledOnly)}
              className={`flex items-center gap-2 rounded-lg border px-3 py-2 text-sm font-medium transition-colors ${
                showEnabledOnly
                  ? "border-cyan-500/60 bg-cyan-500/20 text-cyan-200"
                  : "border-slate-700/50 bg-slate-800/50 text-slate-400 hover:bg-slate-800/70"
              }`}
            >
              <Filter className="h-4 w-4" />
              Enabled Only
            </button>
          </div>
        </div>

        {/* Table */}
        <div className="flex-1 overflow-y-auto p-4">
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-slate-800/50">
                  <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wider text-slate-400">
                    Display Name
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wider text-slate-400">
                    Category
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wider text-slate-400">
                    Enabled
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wider text-slate-400">
                    Tags
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wider text-slate-400">
                    Default Usage
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wider text-slate-400">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-800/50">
                {filteredConfigs.map((config) => (
                  <motion.tr
                    key={config.id}
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    className={`cursor-pointer transition-colors hover:bg-slate-800/30 ${
                      selectedConfig?.id === config.id ? "bg-slate-800/50" : ""
                    }`}
                    onClick={() => handleEdit(config)}
                  >
                    <td className="px-4 py-3 text-sm font-medium text-slate-100">
                      {config.displayName}
                    </td>
                    <td className="px-4 py-3 text-xs text-slate-400">{config.category}</td>
                    <td className="px-4 py-3">
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          handleToggleEnabled(config);
                        }}
                        className={`flex items-center gap-1 rounded-full px-2 py-1 text-xs font-medium ${
                          config.enabled
                            ? "bg-emerald-500/20 text-emerald-300"
                            : "bg-slate-700/50 text-slate-500"
                        }`}
                      >
                        {config.enabled ? (
                          <>
                            <Eye className="h-3 w-3" />
                            Enabled
                          </>
                        ) : (
                          <>
                            <EyeOff className="h-3 w-3" />
                            Disabled
                          </>
                        )}
                      </button>
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex flex-wrap gap-1">
                        {config.tags.slice(0, 3).map((tag) => (
                          <span
                            key={tag}
                            className="rounded-full bg-slate-800/50 px-2 py-0.5 text-xs text-slate-400"
                          >
                            {tag}
                          </span>
                        ))}
                        {config.tags.length > 3 && (
                          <span className="text-xs text-slate-500">+{config.tags.length - 3}</span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3 text-xs text-slate-400">
                      {getDefaultUsageSummary(config)}
                    </td>
                    <td className="px-4 py-3">
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          handleEdit(config);
                        }}
                        className="rounded-lg p-1.5 text-slate-400 transition-colors hover:bg-slate-700/50 hover:text-slate-100"
                      >
                        <Edit className="h-4 w-4" />
                      </button>
                    </td>
                  </motion.tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {/* Right: Editor Panel */}
      <AnimatePresence>
        {editingConfig && (
          <motion.div
            initial={{ x: 400, opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            exit={{ x: 400, opacity: 0 }}
            transition={{ duration: 0.3 }}
            className="flex w-[400px] flex-shrink-0 flex-col overflow-hidden rounded-xl border border-slate-800/80 bg-gradient-to-br from-slate-900/90 to-slate-950/90"
          >
            {/* Editor Header */}
            <div className="flex-shrink-0 border-b border-slate-800/50 p-4">
              <div className="flex items-center justify-between">
                <h3 className="text-base font-bold text-slate-100">Edit Animation</h3>
                <div className="flex gap-2">
                  <button
                    onClick={handleSave}
                    className="flex items-center gap-1 rounded-lg bg-cyan-500/20 px-3 py-1.5 text-sm font-medium text-cyan-200 transition-colors hover:bg-cyan-500/30"
                  >
                    <Save className="h-4 w-4" />
                    Save
                  </button>
                  <button
                    onClick={handleCancel}
                    className="flex items-center gap-1 rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-1.5 text-sm font-medium text-slate-400 transition-colors hover:bg-slate-800/70"
                  >
                    <X className="h-4 w-4" />
                    Cancel
                  </button>
                </div>
              </div>
            </div>

            {/* Editor Content */}
            <div className="flex-1 overflow-y-auto p-4 space-y-4">
              {/* Display Name */}
              <div>
                <label className="mb-1 block text-xs font-medium text-slate-400">Display Name</label>
                <input
                  type="text"
                  value={editingConfig.displayName}
                  onChange={(e) =>
                    setEditingConfig({ ...editingConfig, displayName: e.target.value })
                  }
                  className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-2 text-sm text-slate-100 focus:border-cyan-500/60 focus:outline-none"
                />
              </div>

              {/* Description */}
              <div>
                <label className="mb-1 block text-xs font-medium text-slate-400">Description</label>
                <textarea
                  value={editingConfig.description}
                  onChange={(e) =>
                    setEditingConfig({ ...editingConfig, description: e.target.value })
                  }
                  rows={3}
                  className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-2 text-sm text-slate-100 focus:border-cyan-500/60 focus:outline-none"
                />
              </div>

              {/* Category */}
              <div>
                <label className="mb-1 block text-xs font-medium text-slate-400">Category</label>
                <select
                  value={editingConfig.category}
                  onChange={(e) =>
                    setEditingConfig({
                      ...editingConfig,
                      category: e.target.value as CalendarAnimationCategory,
                    })
                  }
                  className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-2 text-sm text-slate-100 focus:border-cyan-500/60 focus:outline-none"
                >
                  {categories.map((cat) => (
                    <option key={cat} value={cat}>
                      {cat}
                    </option>
                  ))}
                </select>
              </div>

              {/* Tags */}
              <div>
                <label className="mb-1 block text-xs font-medium text-slate-400">Tags (comma-separated)</label>
                <input
                  type="text"
                  value={editingConfig.tags.join(", ")}
                  onChange={(e) =>
                    setEditingConfig({
                      ...editingConfig,
                      tags: e.target.value.split(",").map((t) => t.trim()).filter(Boolean),
                    })
                  }
                  className="w-full rounded-lg border border-slate-700/50 bg-slate-800/50 px-3 py-2 text-sm text-slate-100 focus:border-cyan-500/60 focus:outline-none"
                />
              </div>

              {/* Enabled */}
              <div>
                <label className="mb-2 flex items-center gap-2">
                  <input
                    type="checkbox"
                    checked={editingConfig.enabled}
                    onChange={(e) =>
                      setEditingConfig({ ...editingConfig, enabled: e.target.checked })
                    }
                    className="h-4 w-4 rounded border-slate-700/50 bg-slate-800/50 text-cyan-500 focus:ring-cyan-500/50"
                  />
                  <span className="text-xs font-medium text-slate-400">Enabled (visible in tiles)</span>
                </label>
              </div>

              {/* Default Usage */}
              <div>
                <label className="mb-2 block text-xs font-medium text-slate-400">Default Usage</label>
                <div className="space-y-2 rounded-lg border border-slate-700/50 bg-slate-800/30 p-3">
                  {[
                    { key: "hoverDay" as const, label: "Hover Day" },
                    { key: "clickDay" as const, label: "Click Day" },
                    { key: "selectWeek" as const, label: "Select Week" },
                    { key: "monthChange" as const, label: "Month Change" },
                    { key: "bookedDay" as const, label: "Booked Day" },
                    { key: "blockedDay" as const, label: "Blocked Day" },
                    { key: "availableDay" as const, label: "Available Day" },
                    { key: "loadingState" as const, label: "Loading State" },
                    { key: "statusIndicator" as const, label: "Status Indicator" },
                  ].map(({ key, label }) => (
                    <label key={key} className="flex items-center gap-2">
                      <input
                        type="checkbox"
                        checked={editingConfig.isDefaultFor[key] || false}
                        onChange={(e) =>
                          setEditingConfig({
                            ...editingConfig,
                            isDefaultFor: {
                              ...editingConfig.isDefaultFor,
                              [key]: e.target.checked,
                            },
                          })
                        }
                        className="h-4 w-4 rounded border-slate-700/50 bg-slate-800/50 text-cyan-500 focus:ring-cyan-500/50"
                      />
                      <span className="text-xs text-slate-300">{label}</span>
                    </label>
                  ))}
                </div>
              </div>

              {/* NEW Indicator Override */}
              <div>
                <label className="mb-2 block text-xs font-medium text-slate-400">NEW Indicator Override</label>
                <div className="space-y-2">
                  <label className="flex items-center gap-2">
                    <input
                      type="radio"
                      name="newIndicator"
                      checked={
                        !editingConfig.newIndicatorOverride?.forceNew &&
                        !editingConfig.newIndicatorOverride?.neverNew
                      }
                      onChange={() =>
                        setEditingConfig({
                          ...editingConfig,
                          newIndicatorOverride: undefined,
                        })
                      }
                      className="h-4 w-4 border-slate-700/50 bg-slate-800/50 text-cyan-500 focus:ring-cyan-500/50"
                    />
                    <span className="text-xs text-slate-300">Use global rules</span>
                  </label>
                  <label className="flex items-center gap-2">
                    <input
                      type="radio"
                      name="newIndicator"
                      checked={editingConfig.newIndicatorOverride?.forceNew === true}
                      onChange={() =>
                        setEditingConfig({
                          ...editingConfig,
                          newIndicatorOverride: { forceNew: true, neverNew: false },
                        })
                      }
                      className="h-4 w-4 border-slate-700/50 bg-slate-800/50 text-cyan-500 focus:ring-cyan-500/50"
                    />
                    <span className="text-xs text-slate-300">Always NEW</span>
                  </label>
                  <label className="flex items-center gap-2">
                    <input
                      type="radio"
                      name="newIndicator"
                      checked={editingConfig.newIndicatorOverride?.neverNew === true}
                      onChange={() =>
                        setEditingConfig({
                          ...editingConfig,
                          newIndicatorOverride: { forceNew: false, neverNew: true },
                        })
                      }
                      className="h-4 w-4 border-slate-700/50 bg-slate-800/50 text-cyan-500 focus:ring-cyan-500/50"
                    />
                    <span className="text-xs text-slate-300">Never NEW</span>
                  </label>
                </div>
              </div>

              {/* ID (read-only) */}
              <div>
                <label className="mb-1 block text-xs font-medium text-slate-400">ID (read-only)</label>
                <input
                  type="text"
                  value={editingConfig.id}
                  disabled
                  className="w-full rounded-lg border border-slate-700/50 bg-slate-900/50 px-3 py-2 text-sm text-slate-500"
                />
              </div>
            </div>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Toast Notification */}
      <AnimatePresence>
        {showToast && (
          <motion.div
            initial={{ y: 50, opacity: 0 }}
            animate={{ y: 0, opacity: 1 }}
            exit={{ y: 50, opacity: 0 }}
            className="fixed bottom-4 right-4 z-50 flex items-center gap-2 rounded-lg bg-slate-800 border border-slate-700/50 px-4 py-3 shadow-lg"
          >
            <Check className="h-4 w-4 text-emerald-400" />
            <span className="text-sm text-slate-100">{toastMessage}</span>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}

