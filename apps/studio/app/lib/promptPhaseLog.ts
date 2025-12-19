/**
 * Prompt/Phase Tracking Log
 * 
 * This module provides typed access to the prompt phase log.
 * The log tracks Cursor prompt/phase installs and does NOT reflect app versioning.
 */

import rawPromptPhaseLog from "@/data/promptPhaseLog.json";

export type PromptPhaseEntry = {
  phase: string;
  promptRevision?: string;
  installedOn: string;
  description: string;
};

export type PromptPhaseLog = {
  project: string;
  notes: string;
  entries: PromptPhaseEntry[];
};

export const promptPhaseLog = rawPromptPhaseLog as PromptPhaseLog;

