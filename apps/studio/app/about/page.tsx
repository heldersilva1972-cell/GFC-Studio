import Link from "next/link";
import { ArrowLeft } from "lucide-react";

export default function AboutPage() {
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
          <h1 className="text-3xl font-bold text-gray-900 mb-4">About</h1>
          <p className="text-lg text-gray-600 mb-6">
            Animation Playground is a standalone project for creating,
            previewing, and exporting animations.
          </p>
          <div className="mt-8 space-y-4">
            <div>
              <h2 className="text-xl font-semibold text-gray-900 mb-2">
                Built With
              </h2>
              <ul className="list-disc list-inside text-gray-600 space-y-1">
                <li>Next.js 15 (App Router)</li>
                <li>React & TypeScript</li>
                <li>Tailwind CSS</li>
                <li>Framer Motion</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}

