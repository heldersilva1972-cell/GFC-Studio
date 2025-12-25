// [NEW]
'use client';

import { useEffect, useState } from 'react';
import ComponentRegistry from '@/components/studio/ComponentRegistry';

interface StudioSection {
    ComponentType: string;
    PropertiesJson: string;
    ClientId: string;
}

export default function StudioPreviewPage({ params }: { params: { pageId: string } }) {
    const [sections, setSections] = useState<StudioSection[]>([]);
    const [error, setError] = useState<string | null>(null);
    const { pageId } = params;

    useEffect(() => {
        if (!pageId) return;

        const fetchSections = async () => {
            try {
                const res = await fetch(`/api/studio/page/${pageId}`);
                if (!res.ok) {
                    throw new Error(`Failed to fetch page data: ${res.statusText}`);
                }
                const data = await res.json();
                setSections(data);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchSections();
    }, [pageId]);

    if (error) {
        return <div style={{ color: 'red', padding: '20px' }}>Error: {error}</div>;
    }

    if (!sections.length) {
        return <div style={{ padding: '20px' }}>Loading sections or page is empty...</div>;
    }

    return (
        <div>
            {sections.map((section, index) => {
                const Component = ComponentRegistry[section.ComponentType];
                if (!Component) {
                    return <div key={index} style={{ color: 'orange', padding: '10px' }}>Unknown component type: {section.ComponentType}</div>;
                }
                const props = section.PropertiesJson ? JSON.parse(section.PropertiesJson) : {};
                return <Component key={index} ClientId={section.ClientId} {...props} />;
            })}
        </div>
    );
}
