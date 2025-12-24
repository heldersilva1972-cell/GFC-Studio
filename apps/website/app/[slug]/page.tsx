// [NEW]
import DynamicRenderer from '@/components/DynamicRenderer';
import { getPageBySlug } from '@/app/lib/api';
import { notFound } from 'next/navigation';
import Header from '@/components/Header';
import Footer from '@/components/Footer';

interface PageProps {
  params: {
    slug: string;
  };
}

export default async function Page({ params }: PageProps) {
  try {
    const pageData = await getPageBySlug(params.slug);

    if (!pageData || !pageData.sections) {
      notFound();
    }

    return (
      <>
        <Header />
        <main>
          <DynamicRenderer sections={pageData.sections} />
        </main>
        <Footer />
      </>
    );
  } catch (error) {
    console.error(`Failed to fetch page for slug "${params.slug}":`, error);
    notFound();
  }
}
