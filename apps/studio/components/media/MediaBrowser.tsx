// [NEW]
'use client';

import React, { useState, useEffect } from 'react';

// Define types for our data
interface MediaAsset {
  id: number;
  fileName: string;
  fileType: string;
  filePath_sm: string; // Using the small variant for previews
  createdAt: string;
}

interface AssetFolder {
  id: number;
  name: string;
  subFolders: AssetFolder[];
}

const MediaBrowser: React.FC = () => {
  const [folders, setFolders] = useState<AssetFolder[]>([]);
  const [assets, setAssets] = useState<MediaAsset[]>([]);
  const [currentFolderId, setCurrentFolderId] = useState<number | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const API_BASE_URL = '/api/AssetManager'; // Using relative path for proxying

  useEffect(() => {
    fetchFolders();
    fetchAssets(null); // Fetch root assets initially
  }, []);

  const fetchFolders = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/folders`);
      if (!response.ok) throw new Error('Failed to fetch folders');
      const data = await response.json();
      setFolders(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  const fetchAssets = async (folderId: number | null) => {
    setIsLoading(true);
    setCurrentFolderId(folderId);
    const url = folderId ? `${API_BASE_URL}/assets/${folderId}` : `${API_BASE_URL}/assets`;
    try {
      const response = await fetch(url);
      if (!response.ok) throw new Error('Failed to fetch assets');
      const data = await response.json();
      setAssets(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An unknown error occurred');
    } finally {
      setIsLoading(false);
    }
  };

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    const url = currentFolderId
      ? `${API_BASE_URL}/upload/${currentFolderId}`
      : `${API_BASE_URL}/upload`;

    try {
      const response = await fetch(url, {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        throw new Error('Upload failed');
      }

      // Refresh assets after upload
      fetchAssets(currentFolderId);

    } catch (err) {
        setError(err instanceof Error ? err.message : 'An unknown error occurred during upload');
    }
  };

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Folder Tree Sidebar */}
      <div className="w-1/4 bg-white p-4 border-r">
        <h2 className="text-xl font-bold mb-4">Folders</h2>
        {/* Simplified folder view for now */}
        <ul>
          <li className="p-2 hover:bg-gray-200 cursor-pointer" onClick={() => fetchAssets(null)}>Root</li>
          {folders.map(folder => (
            <li key={folder.id} className="p-2 hover:bg-gray-200 cursor-pointer" onClick={() => fetchAssets(folder.id)}>
              {folder.name}
            </li>
          ))}
        </ul>
      </div>

      {/* Asset Grid */}
      <div className="w-3/4 p-4">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-bold">Assets</h2>
          <input
            type="file"
            onChange={handleFileUpload}
            className="block w-full text-sm text-slate-500
              file:mr-4 file:py-2 file:px-4
              file:rounded-full file:border-0
              file:text-sm file:font-semibold
              file:bg-violet-50 file:text-violet-700
              hover:file:bg-violet-100"
          />
        </div>

        {error && <p className="text-red-500">{error}</p>}

        {isLoading ? (
          <p>Loading...</p>
        ) : (
          <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 gap-4">
            {assets.map(asset => (
              <div key={asset.id} className="border rounded-lg overflow-hidden">
                <img src={asset.filePath_sm} alt={asset.fileName} className="w-full h-32 object-cover" />
                <div className="p-2">
                  <p className="text-sm truncate">{asset.fileName}</p>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default MediaBrowser;
