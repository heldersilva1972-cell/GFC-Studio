export const breakpoints = {
  sm: 640,
  md: 768,
  lg: 1024,
  xl: 1280,
} as const;

export type BreakpointKey = keyof typeof breakpoints;

export const isNarrow = (width: number) => width < breakpoints.md;

export const isMedium = (width: number) => width >= breakpoints.md && width < breakpoints.lg;

export const isWide = (width: number) => width >= breakpoints.lg;

