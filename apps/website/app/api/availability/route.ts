// [NEW]
import { NextResponse } from 'next/server';

export const dynamic = 'force-dynamic';
export const revalidate = 10;

const getAvailabilityFromWebApp = async () => {
  const res = await fetch('http://localhost:5207/api/Availability');
  const data = await res.json();
  return data;
};

export async function GET() {
  try {
    const data = await getAvailabilityFromWebApp();
    return NextResponse.json(data);
  } catch (error) {
    console.error('Failed to fetch availability:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to fetch availability' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
