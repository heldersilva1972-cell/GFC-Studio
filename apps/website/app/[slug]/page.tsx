// [MODIFIED]
import DynamicRenderer from '@/components/DynamicRenderer';
import { getPageBySlug } from '@/app/lib/api';
import { notFound } from 'next/navigation';
import type { Metadata } from 'next';

interface PageProps {
  params: {
    slug: string;
  };
}

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
  const pageData = await getPageBySlug(params.slug);

  if (!pageData) {
    return {
      title: 'Page Not Found',
      description: 'The page you are looking for does not exist.',
    };
  }

  return {
    title: pageData.title,
    description: pageData.metaDescription,
    openGraph: {
      title: pageData.title,
      description: pageData.metaDescription,
      images: [
        {
          url: pageData.ogImageUrl || '/default-og-image.png',
          width: 1200,
          height: 630,
          alt: pageData.title,
        },
      ],
    },
  };
}


export default async function Page({ params }: PageProps) {
  const pageData = await getPageBySlug(params.slug);

  if (!pageData || !pageData.sections) {
    notFound();
  }

  return (
    <main>
      <DynamicRenderer sections={pageData.sections} />
    </main>
  );
}
