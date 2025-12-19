/**
 * App Metadata
 * 
 * This module provides typed access to the app metadata.
 * The metadata tracks app version and revision for Animation Playground only.
 */

import rawAppMeta from "@/data/appMeta.json";

export type AppMeta = {
  project: string;
  appVersion: string;
  revision: number;
  lastUpdated: string;
  notes: string;
};

export const appMeta = rawAppMeta as AppMeta;

