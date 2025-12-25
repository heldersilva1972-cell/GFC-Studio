// [NEW]
'use client';

import { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

export default function PreviewPage({ params }: { params: { pageId: string } }) {
  const [pageContent, setPageContent] = useState('');

  useEffect(() => {
    const fetchPageContent = async () => {
      try {
        const res = await fetch(`http://localhost:5000/api/pages/${params.pageId}`);
        if (res.ok) {
          const data = await res.json();
          // This is a placeholder for rendering logic.
          // In a real scenario, you would parse the sections and render components.
          setPageContent(JSON.stringify(data.sections, null, 2));
        } else {
          setPageContent('Error fetching page content.');
        }
      } catch (error) {
        setPageContent('Error fetching page content.');
      }
    };

    fetchPageContent();

    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/studiopreviewhub')
      .build();

    connection.on('ReceivePreviewUpdate', (content) => {
      // This is a placeholder for rendering logic.
      // In a real scenario, you would parse the sections and render components.
      setPageContent(content);
    });

    connection.start().catch(err => console.error('SignalR Connection Error: ', err));

    return () => {
      connection.stop();
    };
  }, [params.pageId]);

  return (
    <div>
      <h1>Page Preview</h1>
      <pre>{pageContent}</pre>
    </div>
  );
}
