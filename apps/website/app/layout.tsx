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

export const metadata: Metadata = {
  title: 'Gloucester Fraternity Club | Building Community Since 1923',
  description:
    'Welcome to the Gloucester Fraternity Club — a place where friendship, family, and community come together. Hall rentals, events, and membership available.',
  keywords:
    'Gloucester Fraternity Club, GFC, hall rentals, community events, Gloucester MA, membership',
};

async function getWebsiteSettings() {
    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL || process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207';
    try {
        const res = await fetch(`${baseUrl}/api/WebsiteSettings`, {
            cache: 'no-store',
        });
        if (!res.ok) {
            console.error(`Failed to fetch website settings from ${baseUrl}`);
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

    const globalStyles = {
        '--primary-color': settings?.primaryColor || '#0D1B2A',
        '--secondary-color': settings?.secondaryColor || '#FFD700',
        '--font-heading': `'${settings?.headingFont || 'Outfit'}', sans-serif`,
        '--font-body': `'${settings?.bodyFont || 'Inter'}', sans-serif`,
    } as React.CSSProperties;

    return (
        <html lang="en" suppressHydrationWarning>
            <body
                className={a11yClass}
                data-motion-reduced={settings?.highAccessibilityMode ? 'true' : 'false'}
                style={globalStyles}
            >
                <Header />
                <main>{children}</main>
                <Footer />
                <BackToWebAppButton />
            </body>
        </html>
    )
}
