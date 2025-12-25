// [NEW]
import { NextResponse } from 'next/server';

// Opt out of caching for this route
export const dynamic = 'force-dynamic';

// Revalidate every 10 seconds
export const revalidate = 10;

const getClubStatusFromWebApp = async () => {
  const res = await fetch('http://localhost:5207/api/WebsiteSettings');
  const data = await res.json();
  return { status: data.isClubOpen ? 'Open' : 'Closed' };
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
