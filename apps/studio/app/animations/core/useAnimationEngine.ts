"use client";

import { useState, useCallback } from "react";
import type {
  AnimationEngineState,
  AnimationEngineActions,
  AnimationEngineContextValue,
} from "./types";

const DEFAULT_STATE: AnimationEngineState = {
  selectedAnimationId: null,
  speed: 1.0,
  size: 200,
  colors: ["#3b82f6", "#8b5cf6", "#ec4899"],
  theme: "light",
};

export function useAnimationEngine(): AnimationEngineContextValue {
  const [state, setState] = useState<AnimationEngineState>(DEFAULT_STATE);

  const setSelectedAnimation = useCallback((id: string | null) => {
    setState((prev) => ({ ...prev, selectedAnimationId: id }));
  }, []);

  const setSpeed = useCallback((speed: number) => {
    setState((prev) => ({ ...prev, speed: Math.max(0.1, Math.min(3.0, speed)) }));
  }, []);

  const setSize = useCallback((size: number) => {
    setState((prev) => ({ ...prev, size: Math.max(50, Math.min(500, size)) }));
  }, []);

  const setColors = useCallback((colors: string[]) => {
    setState((prev) => ({ ...prev, colors }));
  }, []);

  const setTheme = useCallback((theme: "light" | "dark") => {
    setState((prev) => ({ ...prev, theme }));
  }, []);

  const reset = useCallback(() => {
    setState(DEFAULT_STATE);
  }, []);

  const actions: AnimationEngineActions = {
    setSelectedAnimation,
    setSpeed,
    setSize,
    setColors,
    setTheme,
    reset,
  };

  return { state, actions };
}

