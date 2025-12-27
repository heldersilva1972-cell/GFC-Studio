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
    // Let the parent know the iframe is ready
    window.parent.postMessage({ type: 'PREVIEW_READY' }, '*');

    // Set up message listener
    const handleMessage = (event: MessageEvent) => {
        // TODO: Make this configurable for production environments
        if (event.origin !== 'http://localhost:5207') {
             console.warn('Message from untrusted origin ignored:', event.origin);
             return;
        }

      const { type, sections, component, sectionId, style } = event.data;

      switch (type) {
        case 'LOAD_SECTIONS':
          console.log('Preview received sections:', sections);
          setSections(sections as StudioSection[]);
          break;
        case 'ADD_COMPONENT':
          addSection(component as StudioSection);
          break;
        case 'UPDATE_STYLE':
          updateSectionStyle(sectionId, style);
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

  return (
    <div>
      <DynamicRenderer sections={sections} />
    </div>
  );
}
