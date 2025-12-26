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
    const res = await fetch('http://localhost:5207/api/WebsiteSettings', { cache: 'no-store' });

    if (!res.ok) {
      const text = await res.text();
      console.error(`Backend returned ${res.status}:`, text);
      return new NextResponse(JSON.stringify({ message: `Backend error: ${res.status}`, detail: text }), {
        status: res.status,
        headers: { 'Content-Type': 'application/json' }
      });
    }

    const data = await res.json();
    return NextResponse.json({ status: data.isClubOpen ? 'Open' : 'Closed' });

  } catch (error) {
    console.error('Failed to fetch club status:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to fetch club status', error: String(error) }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
