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

export default function RootLayout({
    children,
}: {
    children: React.ReactNode
}) {
    return (
        <html lang="en">
            <body>
                <Header />
                <main>{children}</main>
                <Footer />
                <BackToWebAppButton />
            </body>
        </html>
    )
}
