/**
 * App Revision History Log
 * 
 * This module provides typed access to the app revision history log.
 * The log tracks detailed revision history with change summaries.
 */

import rawRevisionLog from "@/data/appRevisionLog.json";

export type RevisionLogEntry = {
  revision: number;
  date: string;
  phase: string;
  changes: string[];
};

export type AppRevisionLog = {
  project: string;
  log: RevisionLogEntry[];
};

export const appRevisionLog = rawRevisionLog as AppRevisionLog;

