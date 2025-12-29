// [NEW]
export interface StudioSection {
    id: string;
    clientId: string;
    sectionType: string;
    properties: {
      [key: string]: any;
    };
    animationSettingsJson?: string;
    sortOrder: number;
  }
