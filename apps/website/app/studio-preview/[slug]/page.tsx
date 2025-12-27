// [NEW]
'use client';

import { useEffect } from 'react';
import { usePageStore } from '@/app/lib/store';
import { getPageBySlug } from '@/app/lib/api';
import DynamicRenderer from '@/components/DynamicRenderer';
import { StudioSection } from '@/app/lib/types';

interface PreviewPageProps {
  params: {
    slug: string;
  };
}

export default function StudioPreviewPage({ params }: PreviewPageProps) {
  const { setSections, addSection, updateSectionStyle } = usePageStore();

  useEffect(() => {
    // Fetch initial page data
    const fetchPageData = async () => {
      const pageData = await getPageBySlug(params.slug);
      if (pageData && pageData.sections) {
        setSections(pageData.sections);
      }
    };
    fetchPageData();

    // Set up message listener
    const handleMessage = (event: MessageEvent) => {
      const { type, component, sectionId, style, payload } = event.data;

      switch (type) {
        case 'ADD_COMPONENT':
          addSection(component as StudioSection);
          break;
        case 'UPDATE_STYLE':
          updateSectionStyle(sectionId, style);
          break;
        case 'UPDATE_ANIMATION':
          usePageStore.getState().updateAnimation(payload);
          break;
        case 'REQUEST_STATE':
            const currentState = usePageStore.getState().sections;
            event.source?.postMessage({ type: 'CURRENT_STATE', payload: currentState }, event.origin as any);
            break;
      }
    };

    window.addEventListener('message', handleMessage);

    return () => {
      window.removeEventListener('message', handleMessage);
    };
  }, [params.slug, setSections, addSection, updateSectionStyle]);

  const sections = usePageStore((state) => state.sections);
  const animationKeyframes = usePageStore((state) => state.animationKeyframes);

  return (
    <div>
      <DynamicRenderer sections={sections} animationKeyframes={animationKeyframes} />
    </div>
  );
}
