'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';

interface Section {
  id: string;
  type: string;
  content: any;
  styles?: any;
  animation?: any;
}

export default function StudioPreviewPage() {
  const params = useParams();
  const slug = params.slug as string;
  const [sections, setSections] = useState<Section[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Listen for messages from the Blazor Studio editor
    const handleMessage = (event: MessageEvent) => {
      // Only accept messages from the Blazor app
      if (event.origin !== 'http://localhost:5207') return;

      const { type, payload } = event.data;

      switch (type) {
        case 'UPDATE_SECTIONS':
          setSections(payload);
          break;
        case 'UPDATE_SECTION':
          setSections(prev => 
            prev.map(s => s.id === payload.id ? payload : s)
          );
          break;
        case 'ADD_SECTION':
          setSections(prev => [...prev, payload]);
          break;
        case 'DELETE_SECTION':
          setSections(prev => prev.filter(s => s.id !== payload.id));
          break;
      }
    };

    window.addEventListener('message', handleMessage);
    
    // Notify parent that preview is ready
    window.parent.postMessage({ type: 'PREVIEW_READY', slug }, 'http://localhost:5207');
    
    setIsLoading(false);

    return () => window.removeEventListener('message', handleMessage);
  }, [slug]);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-gray-50">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading preview...</p>
        </div>
      </div>
    );
  }

  if (sections.length === 0) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-gray-50">
        <div className="text-center max-w-md p-8">
          <div className="text-6xl mb-4">📄</div>
          <h2 className="text-2xl font-bold text-gray-800 mb-2">Empty Page</h2>
          <p className="text-gray-600 mb-4">
            This page doesn't have any sections yet. 
          </p>
          <p className="text-sm text-gray-500">
            Click "Add Section" or drag a template from the left panel to get started.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-white">
      {sections.map((section) => (
        <div
          key={section.id}
          className="studio-section"
          data-section-id={section.id}
          style={section.styles}
        >
          {renderSection(section)}
        </div>
      ))}
    </div>
  );
}

function renderSection(section: Section) {
  switch (section.type) {
    case 'Hero':
    case 'HeroSection':
      return (
        <div className="relative h-96 flex items-center justify-center bg-gradient-to-r from-blue-600 to-purple-600 text-white">
          <div className="text-center z-10">
            <h1 className="text-5xl font-bold mb-4">
              {section.content?.headline || 'Welcome'}
            </h1>
            <p className="text-xl">
              {section.content?.subtitle || 'Your subtitle here'}
            </p>
          </div>
        </div>
      );

    case 'RichTextBlock':
    case 'TextBlock':
      return (
        <div className="max-w-4xl mx-auto py-12 px-4">
          <div 
            className="prose prose-lg"
            dangerouslySetInnerHTML={{ __html: section.content?.content || '<p>Enter your text here...</p>' }}
          />
        </div>
      );

    case 'ButtonCTA':
      return (
        <div className="text-center py-8">
          <a
            href={section.content?.link || '#'}
            className="inline-block px-8 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
            style={{ backgroundColor: section.content?.backgroundColor }}
          >
            {section.content?.text || 'Click Me'}
          </a>
        </div>
      );

    case 'ImageGallery':
      return (
        <div className="grid grid-cols-3 gap-4 p-8">
          {(section.content?.images || []).map((img: any, idx: number) => (
            <img
              key={idx}
              src={img.url}
              alt={img.alt || `Gallery image ${idx + 1}`}
              className="w-full h-64 object-cover rounded-lg"
            />
          ))}
        </div>
      );

    default:
      return (
        <div className="p-8 bg-gray-100 border-2 border-dashed border-gray-300 text-center">
          <p className="text-gray-600">
            Unknown section type: <strong>{section.type}</strong>
          </p>
          <pre className="mt-4 text-xs text-left bg-white p-4 rounded overflow-auto">
            {JSON.stringify(section.content, null, 2)}
          </pre>
        </div>
      );
  }
}
