// [NEW]
export interface AnimationKeyframe {
    id: string;
    target: string;
    effect: string;
    duration: number;
    delay: number;
    easing: string;
    trigger: string;
}

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

export interface WebsiteSettings {
    primaryColor: string;
    secondaryColor: string;
    headingFont: string;
    bodyFont: string;
}
