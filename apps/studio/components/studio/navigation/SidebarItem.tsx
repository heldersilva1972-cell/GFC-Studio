import React from "react";
import { transitionFast } from "@/lib/theme/motionTokens";

export interface SidebarItemProps {
  icon: React.ReactNode;
  label: string;
  isActive?: boolean;
  badgeCount?: number;
  onClick?: () => void;
  collapsed?: boolean;
}

const SidebarItem: React.FC<SidebarItemProps> = ({
  icon,
  label,
  isActive = false,
  badgeCount = 0,
  onClick,
  collapsed = false,
}) => {
  const showBadge = typeof badgeCount === "number" && badgeCount > 0;
  const ariaLabel = collapsed
    ? `${label}${showBadge ? ` (${badgeCount} notifications)` : ""}`
    : undefined;

  const basePadding = collapsed ? "px-2 py-2 justify-center" : "px-3 py-2";
  const activeClasses = isActive
    ? "border border-[var(--studio-color-primary)] bg-[var(--studio-color-bg-elevated)]/90 text-[var(--studio-color-text-strong)] shadow-[0_4px_16px_rgba(0,0,0,0.28)]"
    : "border border-transparent text-[var(--studio-color-text-muted)] hover:border-[var(--studio-color-border-subtle)] hover:bg-[var(--studio-color-bg-elevated)]/70 hover:text-[var(--studio-color-text-strong)]";

  return (
    <button
      type="button"
      role="tab"
      aria-selected={isActive}
      aria-label={ariaLabel}
      onClick={onClick}
      className={`group relative flex w-full items-center gap-2 rounded-md text-sm font-medium outline-none focus-visible:ring-2 focus-visible:ring-[var(--studio-color-primary)] focus-visible:ring-offset-2 focus-visible:ring-offset-[var(--studio-color-bg-canvas)] ${basePadding} ${activeClasses}`}
      style={{
        transition: transitionFast(["background-color", "color", "border-color", "padding"]),
      }}
    >
      <span className="flex h-10 w-10 items-center justify-center text-base text-[var(--studio-color-text-strong)]">
        {icon}
      </span>

      <span className={collapsed ? "sr-only" : "ml-1 flex-1 truncate text-left"}>{label}</span>

      {showBadge && (
        <span
          className={`rounded-full bg-[var(--studio-color-danger)] text-[10px] font-semibold leading-none text-white ${
            collapsed ? "absolute right-2 top-2 h-2.5 w-2.5" : "ml-auto px-2 py-0.5"
          }`}
          aria-hidden
        >
          {collapsed ? "" : badgeCount}
        </span>
      )}
    </button>
  );
};

export default SidebarItem;


