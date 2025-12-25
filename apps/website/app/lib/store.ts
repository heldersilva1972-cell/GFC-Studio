// [MODIFIED]
import { create } from 'zustand';
import { StudioSection } from '@/app/lib/types';

interface PageState {
  sections: StudioSection[];
  selectedSectionId: string | null;
  setSections: (sections: StudioSection[]) => void;
  addSection: (section: StudioSection) => void;
  updateSectionStyle: (sectionId: string, style: any) => void;
  setSelectedSectionId: (sectionId: string | null) => void;
}

export const usePageStore = create<PageState>((set) => ({
  sections: [],
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
  setSelectedSectionId: (sectionId) => set({ selectedSectionId: sectionId }),
}));
