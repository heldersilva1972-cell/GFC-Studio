// [MODIFIED]
'use client';

import MediaBrowser from '@/components/media/MediaBrowser';
import SeoInspector from '@/components/studio/SeoInspector';

export default function StudioPage() {
  // In a real app, you would fetch this data based on the current page
  const initialSeoData = {
    metaTitle: 'Welcome to GFC',
    metaDescription: 'The official website for the GFC.',
    ogImageUrl: '',
  };

  // Hardcode pageId for demonstration purposes
  const pageId = 1;

  return (
    <div className="h-screen w-full flex flex-col">
      <header className="bg-white border-b p-4">
        <h1 className="text-2xl font-bold">GFC Studio</h1>
      </header>
      <div className="flex flex-grow overflow-hidden">
        <main className="flex-grow">
          <MediaBrowser />
        </main>
        <aside className="w-1/4">
          <SeoInspector pageId={pageId} initialData={initialSeoData} />
        </aside>
      </div>
    </div>
  );
}


