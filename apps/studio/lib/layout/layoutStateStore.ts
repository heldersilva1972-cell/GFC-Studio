import { useMemo, useSyncExternalStore } from "react";

export type ThemeMode = "light" | "dark";

export interface StudioLayoutState {
  isSidebarCollapsed: boolean;
  openDrawerId: string | null;
  themeMode: ThemeMode;
}

export interface StudioLayoutActions {
  toggleSidebar: () => void;
  openDrawer: (id: string) => void;
  closeDrawer: () => void;
  setThemeMode: (mode: ThemeMode) => void;
}

type StoreSubscriber = () => void;

const initialState: StudioLayoutState = {
  isSidebarCollapsed: false,
  openDrawerId: null,
  themeMode: "dark",
};

let state: StudioLayoutState = initialState;
const subscribers = new Set<StoreSubscriber>();

const notify = () => {
  subscribers.forEach((listener) => listener());
};

const setState = (
  updater: Partial<StudioLayoutState> | ((current: StudioLayoutState) => Partial<StudioLayoutState>)
) => {
  const nextPartial = typeof updater === "function" ? updater(state) : updater;
  state = { ...state, ...nextPartial };
  notify();
};

const getSnapshot = () => state;

const subscribe = (listener: StoreSubscriber) => {
  subscribers.add(listener);
  return () => subscribers.delete(listener);
};

const actions: StudioLayoutActions = {
  toggleSidebar: () => setState((current) => ({ isSidebarCollapsed: !current.isSidebarCollapsed })),
  openDrawer: (id: string) => setState({ openDrawerId: id }),
  closeDrawer: () => setState({ openDrawerId: null }),
  setThemeMode: (mode: ThemeMode) => setState({ themeMode: mode }),
};

export const useStudioLayoutState = (): StudioLayoutState & StudioLayoutActions => {
  const snapshot = useSyncExternalStore(subscribe, getSnapshot, getSnapshot);

  return useMemo(
    () => ({
      ...snapshot,
      ...actions,
    }),
    [snapshot]
  );
};

export default useStudioLayoutState;

