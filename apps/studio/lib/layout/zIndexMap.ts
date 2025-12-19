export const zIndexMap = {
  base: 0,
  stage: 10,
  sidebar: 20,
  drawers: 30,
  topBar: 40,
  alerts: 50,
  modals: 60,
} as const;

export const getZIndex = (layer: keyof typeof zIndexMap) => zIndexMap[layer];

export type ZIndexLayer = keyof typeof zIndexMap;

