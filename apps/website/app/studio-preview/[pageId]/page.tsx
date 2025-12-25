// [NEW]
'use client';

import { useEffect } from 'react';
import { usePageStore } from '@/app/lib/store';
import { getPageBySlug } from '@/app/lib/api';
import DynamicRenderer from '@/components/DynamicRenderer';
import { StudioSection } from '@/app/lib/types';

interface PreviewPageProps {
    params: {
        pageId: string;
    };
}

export default function StudioPreviewPage({ params }: PreviewPageProps) {
    const { setSections, addSection, updateSectionStyle } = usePageStore();

    useEffect(() => {
        // Fetch initial page data
        const fetchPageData = async () => {
            // We treat pageId as slug here for now to match the API
            const pageData = await getPageBySlug(params.pageId);
            if (pageData && pageData.sections) {
                setSections(pageData.sections);
            }
        };
        fetchPageData();

        // Set up message listener
        const handleMessage = (event: MessageEvent) => {
            const { type, component, sectionId, style } = event.data;

            switch (type) {
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
    }, [params.pageId, setSections, addSection, updateSectionStyle]);

    const sections = usePageStore((state) => state.sections);

    return (
        <div>
            <DynamicRenderer sections={sections} />
        </div>
    );
}
