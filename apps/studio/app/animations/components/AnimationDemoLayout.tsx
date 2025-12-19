import Link from "next/link";
import { ArrowLeft } from "lucide-react";
import { getAnimationMetaById } from "../registry";

interface AnimationDemoLayoutProps {
  animationId?: string;
  children: React.ReactNode;
  title?: string;
}

export default function AnimationDemoLayout({
  animationId,
  children,
  title,
}: AnimationDemoLayoutProps) {
  // Try to get metadata from registry, fallback to provided title or default
  const meta = animationId ? getAnimationMetaById(animationId) : null;
  const displayTitle = title || meta?.name || "Animation Demo";

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-6">
          <div className="flex items-center gap-4 mb-4">
            <Link
              href="/"
              className="inline-flex items-center gap-2 text-sm font-medium text-gray-600 hover:text-gray-900 transition-colors"
            >
              <ArrowLeft className="h-4 w-4" />
              <span>Back to Home</span>
            </Link>
            <Link
              href="/animations"
              className="inline-flex items-center text-sm text-gray-500 hover:text-gray-700 transition-colors"
            >
              <span>Library</span>
            </Link>
          </div>
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            {displayTitle}
          </h1>
          {meta?.description && (
            <p className="text-lg text-gray-600">{meta.description}</p>
          )}
        </div>

        {/* Demo Area */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-8">
          {children}
        </div>
      </div>
    </main>
  );
}

