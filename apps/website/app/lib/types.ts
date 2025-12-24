// [NEW]
export interface StudioSection {
    id: string;
    clientId: string;
    sectionType: string;
    properties: {
      [key: string]: any;
    };
    animationSettings?: {
      effect: string;
      duration: number;
      delay: number;
    };
    sortOrder: number;
  }
