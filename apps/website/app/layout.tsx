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

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" className={`${inter.variable} ${outfit.variable}`}>
      <body className="bg-midnight-blue text-pure-white font-sans antialiased">
        <UpdateBar />
        <Header />
        <main className="min-h-screen">{children}</main>
        <Footer />
        <BackToWebAppButton />
      </body>
    </html>
  );
}
