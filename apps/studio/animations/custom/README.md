# Custom Animations

This directory contains user-created custom animations that are not part of the predefined Phase animations.

## Naming Convention

Custom animations are automatically numbered sequentially:
- `Custom1`, `Custom2`, `Custom3`, etc.
- Numbers are **never reused**, even if an animation is deleted
- Each custom animation gets its own directory: `/animations/custom/CustomX/`

## Structure

Each custom animation must include:

```
/animations/custom/CustomX/
  ├── CustomX.tsx          # Main component file
  ├── code.ts              # Code export for copy functionality
  └── index.ts             # Configuration export
```

## Required Content

Each custom animation must include:

1. **Title**: "CustomX – [Description]"
2. **Description**: Full descriptive concept
3. **Technical Breakdown**: Implementation details
4. **SVG Path List** (if applicable): Path definitions
5. **Framer Motion Plan**: Animation strategy
6. **Full React Component**: Complete, working code

## Registration

Custom animations must be registered in:
- `app/animations/core/AnimationRegistry.ts`
- `app/animations/registry.ts`
- `app/AnimationPlaygroundClient.tsx` (AnimationPresetId type)

## Next Custom Animation

The next available custom animation number will be: **Custom1**

