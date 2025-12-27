// [MODIFIED]
import type { Metadata } from 'next';
import { Inter, Outfit } from 'next/font/google';
import './globals.css';
import Header from '@/components/Header';
import Footer from '@/components/Footer';
import BackToWebAppButton from '@/components/BackToWebAppButton';
import UpdateBar from '@/components/UpdateBar';

// Setup Fonts
const inter = Inter({
  subsets: ['latin'],
  variable: '--font-inter',
  display: 'swap',
});

const outfit = Outfit({
  subsets: ['latin'],
  variable: '--font-outfit',
  display: 'swap',
});

export async function generateMetadata(): Promise<Metadata> {
  const settings = await getWebsiteSettings();

  const defaultTitle = 'Gloucester Fraternity Club | Building Community Since 1923';
  const defaultDescription = 'Welcome to the Gloucester Fraternity Club — a place where friendship, family, and community come together. Hall rentals, events, and membership available.';

  return {
    title: settings?.seoTitle || defaultTitle,
    description: settings?.seoDescription || defaultDescription,
    keywords: settings?.seoKeywords || 'Gloucester Fraternity Club, GFC, hall rentals, community events, Gloucester MA, membership',
    openGraph: {
        title: settings?.seoTitle || defaultTitle,
        description: settings?.seoDescription || defaultDescription,
        url: process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:3000',
        siteName: 'Gloucester Fraternity Club',
        images: [
            {
                url: `${process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:3000'}/og-image.png`,
                width: 1200,
                height: 630,
            },
        ],
        locale: 'en_US',
        type: 'website',
    },
  };
}

async function getWebsiteSettings() {
    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL || process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207';
    try {
        const res = await fetch(`${baseUrl}/api/WebsiteSettings`, {
            cache: 'no-store', // Opt-out of caching
        });
        if (!res.ok) {
            console.error(`Failed to fetch website settings from ${baseUrl}, status: ${res.status}`);
            return null;
        }
        return res.json();
    } catch (error) {
        console.warn('Could not fetch website settings, using defaults:', error);
        return null;
    }
}

export default async function RootLayout({
    children,
}: {
  children: React.ReactNode;
}) {
    const settings = await getWebsiteSettings();

    const a11yClass = settings?.highAccessibilityMode ? 'high-accessibility' : '';
    const largeTextClass = settings?.largeTextMode ? 'large-text' : '';

    const globalStyles = {
        '--primary-color': settings?.primaryColor || '#0D1B2A',
        '--secondary-color': settings?.secondaryColor || '#FFD700',
        '--font-heading': `'${settings?.headingFont || 'Outfit'}', sans-serif`,
        '--font-body': `'${settings?.bodyFont || 'Inter'}', sans-serif`,
    } as React.CSSProperties;

    return (
        <html lang="en" suppressHydrationWarning>
            <head>
                <link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon.png" />
                <link rel="icon" type="image/png" sizes="32x32" href="/favicon-32x32.png" />
                <link rel="icon" type="image/png" sizes="16x16" href="/favicon-16x16.png" />
                <link rel="manifest" href="/site.webmanifest" />
            </head>
            <body
                className={`${a11yClass} ${largeTextClass}`}
                data-motion-reduced={settings?.highAccessibilityMode ? 'true' : 'false'}
                style={globalStyles}
            >
                <Header />
                <main>{children}</main>
                <Footer highAccessibilityMode={settings?.highAccessibilityMode} largeTextMode={settings?.largeTextMode} />
                <BackToWebAppButton />
            </body>
        </html>
    )
}
