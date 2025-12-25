// [NEW]
import { NextResponse } from 'next/server';

export const dynamic = 'force-dynamic';
export const revalidate = 15; // Revalidate every 15 seconds

const getAnnouncementsFromWebApp = async () => {
  // Mock data - replace with actual fetch to http://localhost:5000/api/v1/announcements
  return [
    { id: 1, type: 'alert', message: 'Urgent: The club will be closed this Friday for a private event.' },
    { id: 2, type: 'news', title: 'New Member Mixer', content: 'Join us next month for a special event to welcome our new members.' },
    { id: 3, type: 'news', title: 'Summer Hours Update', content: 'Our summer hours are now in effect. The club will be open until 10 PM on weekends.' },
  ];
};

export async function GET() {
  try {
    const data = await getAnnouncementsFromWebApp();
    if (!Array.isArray(data)) {
        throw new Error('Invalid data format for announcements');
    }
    return NextResponse.json(data);
  } catch (error) {
    console.error('Failed to fetch announcements:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to fetch announcements' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
