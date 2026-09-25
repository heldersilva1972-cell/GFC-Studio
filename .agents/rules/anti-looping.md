# ANTI-LOOPING & DECISIVE EXECUTION RULE

## 1. Max Repeated File Read Limit
- **NEVER** view or inspect the exact same file range or boundary more than **TWICE** in a single turn.
- If inspection yields the necessary context on the 1st or 2nd read, immediately proceed to editing or answering.
- If you find yourself hesitating or about to read the same lines a 3rd time, STOP calling inspection tools, state your current understanding to the user, and propose or execute the edit directly.

## 2. Decisive Action Over Verification Loops
- Razor tag balance errors (`RZ9980`, `RZ1006`, `CS1513`) should be resolved by targeted inspection of the opened/closed blocks, followed immediately by applying the corrected structure.
- Never enter a cycle of re-reading surrounding lines without making progress.
