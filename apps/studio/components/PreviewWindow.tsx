export default function PreviewWindow() {
  return (
    <div className="w-full">
      <div className="bg-gray-100 rounded-lg border-2 border-dashed border-gray-300 p-12 flex items-center justify-center min-h-[400px]">
        <div className="text-center">
          <div className="w-24 h-24 bg-gray-300 rounded-lg mx-auto mb-4 flex items-center justify-center">
            <svg
              className="w-12 h-12 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M15 10l4.553-2.276A1 1 0 0121 8.618v6.764a1 1 0 01-1.447.894L15 14M5 18h8a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z"
              />
            </svg>
          </div>
          <p className="text-gray-500 font-medium">Animation Preview Window</p>
          <p className="text-sm text-gray-400 mt-2">
            Animation preview will appear here
          </p>
        </div>
      </div>
    </div>
  );
}

