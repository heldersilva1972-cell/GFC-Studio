// [MODIFIED]
'use client';

import React, { useState } from 'react';

interface SeoData {
  metaTitle: string;
  metaDescription: string;
  ogImageUrl: string;
}

interface SeoInspectorProps {
  pageId: number;
  initialData: SeoData;
}

const SeoInspector: React.FC<SeoInspectorProps> = ({ pageId, initialData }) => {
  const [data, setData] = useState<SeoData>(initialData);
  const [status, setStatus] = useState<'idle' | 'saving' | 'success' | 'error'>('idle');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setData(prevData => ({ ...prevData, [name]: value }));
  };

  const handleSave = async () => {
    setStatus('saving');
    try {
      const response = await fetch(`/api/Studio/pages/${pageId}/seo`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: pageId, ...data }),
      });

      if (!response.ok) {
        throw new Error('Failed to save SEO data');
      }
      setStatus('success');
      setTimeout(() => setStatus('idle'), 2000); // Reset status after 2 seconds
    } catch (error) {
      console.error(error);
      setStatus('error');
    }
  };

  return (
    <div className="p-4 border-l bg-white h-full">
      <h3 className="text-lg font-bold mb-4">SEO Inspector</h3>

      <div className="mb-4">
        <label htmlFor="metaTitle" className="block text-sm font-medium text-gray-700">Meta Title</label>
        <input
          type="text"
          name="metaTitle"
          id="metaTitle"
          value={data.metaTitle}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
        <p className="text-xs text-gray-500 mt-1">{data.metaTitle.length} / 60 characters</p>
      </div>

      <div className="mb-4">
        <label htmlFor="metaDescription" className="block text-sm font-medium text-gray-700">Meta Description</label>
        <textarea
          name="metaDescription"
          id="metaDescription"
          rows={4}
          value={data.metaDescription}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
        <p className="text-xs text-gray-500 mt-1">{data.metaDescription.length} / 160 characters</p>
      </div>

      <div className="mb-4">
        <label htmlFor="ogImageUrl" className="block text-sm font-medium text-gray-700">Open Graph Image</label>
        <input
          type="text"
          name="ogImageUrl"
          id="ogImageUrl"
          value={data.ogImageUrl}
          onChange={handleChange}
          placeholder="Paste image URL here..."
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>

      <button
        onClick={handleSave}
        disabled={status === 'saving'}
        className="w-full bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 disabled:bg-gray-400"
      >
        {status === 'saving' ? 'Saving...' : 'Save SEO Settings'}
      </button>

      {status === 'success' && <p className="text-green-500 text-sm mt-2">Saved successfully!</p>}
      {status === 'error' && <p className="text-red-500 text-sm mt-2">Failed to save. Please try again.</p>}
    </div>
  );
};

export default SeoInspector;
