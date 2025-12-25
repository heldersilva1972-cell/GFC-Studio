// [NEW]
import { NextResponse } from 'next/server';

export const dynamic = 'force-dynamic';
export const revalidate = 60; // Revalidate every 60 seconds

const getEventsFromWebApp = async () => {
  // Mock data - replace with actual fetch to http://localhost:5000/api/v1/events
  return [
    { id: 1, title: 'Annual Charity Gala', date: '2025-12-31T19:00:00', description: 'Our biggest event of the year! Dress to impress.' },
    { id: 2, title: 'Family Fun Day', date: '2026-01-15T12:00:00', description: 'Games, food, and fun for the whole family.' },
    { id: 3, title: 'Live Music Night', date: '2026-01-22T20:00:00', description: 'Featuring local band "The Rovers".' },
  ];
};

export async function GET() {
  try {
    const data = await getEventsFromWebApp();
    if (!Array.isArray(data)) {
        throw new Error('Invalid data format for events');
    }
    return NextResponse.json(data);
  } catch (error) {
    console.error('Failed to fetch events:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to fetch events' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
