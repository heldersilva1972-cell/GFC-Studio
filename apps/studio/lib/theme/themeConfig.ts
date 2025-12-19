export const studioTheme = {
  colors: {
    primary: "#6366f1",
    primarySoft: "#e0e7ff",
    accent: "#22d3ee",
    accentSoft: "#cffafe",
    bgCanvas: "#0b0e1a",
    bgElevated: "#11172b",
    borderSubtle: "#1f2a44",
    textStrong: "#e5e7eb",
    textMuted: "#94a3b8",
    danger: "#ef4444",
    success: "#22c55e",
    warning: "#f59e0b",
  },
  radius: {
    none: "0px",
    sm: "4px",
    md: "8px",
    lg: "12px",
    xl: "16px",
    full: "9999px",
  },
  spacing: {
    xs: "0.25rem",
    sm: "0.5rem",
    md: "0.75rem",
    lg: "1rem",
    xl: "1.5rem",
    "2xl": "2rem",
  },
  shadow: {
    soft: "0 4px 12px rgba(0, 0, 0, 0.18)",
    medium: "0 8px 24px rgba(0, 0, 0, 0.22)",
    strong: "0 12px 40px rgba(0, 0, 0, 0.32)",
  },
  typography: {
    fontFamily:
      '"Inter", "Inter var", system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", "Helvetica Neue", Arial, sans-serif',
    sizes: {
      xs: "0.75rem",
      sm: "0.875rem",
      base: "1rem",
      lg: "1.125rem",
      xl: "1.25rem",
      "2xl": "1.5rem",
    },
  },
} as const;

export type StudioThemeTokens = typeof studioTheme;

export default studioTheme;

