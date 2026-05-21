---
trigger: always_on
---

# TOKEN EFFICIENCY & CONTEXT CONSERVATION RULES
* **CONCISE CHAT RESPONSES:** Do not copy or re-summarize large blocks of text from artifact files (like implementation plans, tasks, or walkthroughs) into the chat. Reference the artifact files directly.
* **MINIMIZE FILE READS:** Do not re-read large files multiple times. When inspecting a file, use targeted line ranges (`StartLine`/`EndLine`) where possible instead of loading the entire file.
* **BATCH EDITS:** Propose changes in cohesive, batched edits using `multi_replace_file_content` instead of consecutive tiny changes.
* **RESTRICT SEARCH SCOPE:** Always restrict search paths and use specific glob filters when using search or grep tools to avoid loading irrelevant results.
* **QUIET TERMINAL OUTPUTS:** When proposing terminal commands, use quiet or minimal flags (e.g., `-q`, `--quiet`, or limiting output length) to prevent context pollution from long logs.
