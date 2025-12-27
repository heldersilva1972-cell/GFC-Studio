// [MODIFIED]
import { create } from 'zustand';
import { StudioSection, AnimationKeyframe } from '@/app/lib/types';

interface PageState {
  sections: StudioSection[];
  animationKeyframes: AnimationKeyframe[];
  selectedSectionId: string | null;
  setSections: (sections: StudioSection[]) => void;
  addSection: (section: StudioSection) => void;
  updateSectionStyle: (sectionId: string, style: any) => void;
  updateAnimation: (keyframes: AnimationKeyframe[]) => void;
  setSelectedSectionId: (sectionId: string | null) => void;
}

export const usePageStore = create<PageState>((set) => ({
  sections: [],
  animationKeyframes: [],
  selectedSectionId: null,
  setSections: (sections) => set({ sections }),
  addSection: (section) => set((state) => ({ sections: [...state.sections, section] })),
  updateSectionStyle: (sectionId, style) => set((state) => ({
    sections: state.sections.map(s =>
      s.clientId === sectionId
        ? { ...s, properties: { ...s.properties, ...style } }
        : s
    ),
  })),
  updateAnimation: (keyframes) => set({ animationKeyframes: keyframes }),
  setSelectedSectionId: (sectionId) => set({ selectedSectionId: sectionId }),
}));
