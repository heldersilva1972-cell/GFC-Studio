// [NEW]
import { NextResponse } from 'next/server';

// Opt out of caching for this route
export const dynamic = 'force-dynamic';

// Revalidate every 10 seconds
export const revalidate = 10;

// Mock function to simulate fetching from the Web App API
// In a real scenario, this would be an actual fetch call:
// const res = await fetch('http://localhost:5000/api/v1/club/status');
// const data = await res.json();
const getClubStatusFromWebApp = async () => {
  // For now, we'll return a mock status.
  // This will be replaced with a real fetch call once the Web App API is available.
  const isMorning = new Date().getHours() < 12;
  return { status: isMorning ? 'Open' : 'Closed' };
};

export async function GET() {
  try {
    const data = await getClubStatusFromWebApp();

    // Validate the data from the API
    if (!data || typeof data.status !== 'string') {
      throw new Error('Invalid data format from Web App API');
    }

    return NextResponse.json(data);
  } catch (error) {
    console.error('Failed to fetch club status:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to fetch club status' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
