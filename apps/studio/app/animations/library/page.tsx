import Link from "next/link";
import { ArrowLeft } from "lucide-react";

export default function AnimationLibraryPage() {
  return (
    <main className="min-h-screen bg-gray-50">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <Link
          href="/"
          className="inline-flex items-center gap-2 text-sm font-medium text-gray-600 hover:text-gray-900 mb-4 transition-colors"
        >
          <ArrowLeft className="h-4 w-4" />
          <span>Back to Home</span>
        </Link>
        <div className="bg-white rounded-xl shadow-sm p-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">
            Animation Library
          </h1>
          <p className="text-lg text-gray-600 mb-6">
            Explore the collection of available animations. Select an animation
            to preview and customize it.
          </p>
          <div className="mt-8">
            <p className="text-gray-500 italic">
              Animation library grid and preview features will be added in
              future phases.
            </p>
          </div>
        </div>
      </div>
    </main>
  );
}

