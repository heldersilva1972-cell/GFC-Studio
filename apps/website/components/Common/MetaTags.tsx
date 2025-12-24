// [NEW]
'use client';
import Head from 'next/head';

interface MetaTagsProps {
  title: string;
  description: string;
  ogImage?: string;
  ogType?: string;
  twitterCard?: string;
}

const MetaTags = ({
  title,
  description,
  ogImage = '/default-og-image.png',
  ogType = 'website',
  twitterCard = 'summary_large_image',
}: MetaTagsProps) => {
  return (
    <Head>
      <title>{title}</title>
      <meta name="description" content={description} />

      {/* Open Graph */}
      <meta property="og:title" content={title} />
      <meta property="og:description" content={description} />
      <meta property="og:image" content={ogImage} />
      <meta property="og:type" content={ogType} />

      {/* Twitter Card */}
      <meta name="twitter:card" content={twitterCard} />
      <meta name="twitter:title" content={title} />
      <meta name="twitter:description" content={description} />
      <meta name="twitter:image" content={ogImage} />
    </Head>
  );
};

export default MetaTags;
