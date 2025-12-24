import type { Metadata } from 'next'
import './globals.css'
import Header from '@/components/Header'
import Footer from '@/components/Footer'
import BackToWebAppButton from '@/components/BackToWebAppButton'

export const metadata: Metadata = {
    title: 'Gloucester Fraternity Club | Building Community Since 1923',
    description: 'Welcome to the Gloucester Fraternity Club — a place where friendship, family, and community come together. Hall rentals, events, and membership available.',
    keywords: 'Gloucester Fraternity Club, GFC, hall rentals, community events, Gloucester MA, membership',
}

async function getWebsiteSettings() {
  const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/WebsiteSettings`, {
    cache: 'no-store',
  });
  if (!res.ok) {
    console.error('Failed to fetch website settings');
    return null;
  }
  return res.json();
}

export default async function RootLayout({
    children,
}: {
    children: React.ReactNode
}) {
    const settings = await getWebsiteSettings();

    const a11yClass = settings?.highAccessibilityMode ? 'high-accessibility' : '';

    return (
        <html lang="en" suppressHydrationWarning>
            <body className={a11yClass} data-motion-reduced={settings?.highAccessibilityMode ? 'true' : 'false'}>
                <style jsx global>{`
                    :root {
                        --primary-color: ${settings?.primaryColor || '#0D1B2A'};
                        --secondary-color: ${settings?.secondaryColor || '#FFD700'};
                        --font-heading: '${settings?.headingFont || 'Outfit'}', sans-serif;
                        --font-body: '${settings?.bodyFont || 'Inter'}', sans-serif;
                    }
                `}</style>
                <Header />
                <main>{children}</main>
                <Footer />
                <BackToWebAppButton />
            </body>
        </html>
    )
}
