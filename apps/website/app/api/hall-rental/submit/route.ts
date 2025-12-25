// [NEW]
import { NextResponse } from 'next/server';

export async function POST(request: Request) {
  try {
    const body = await request.json();
    const res = await fetch('http://localhost:5207/api/HallRentalRequest', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    });

    if (res.ok) {
      return new NextResponse(null, { status: 200 });
    } else {
      return new NextResponse(
        JSON.stringify({ message: 'Failed to submit rental request' }),
        { status: 500, headers: { 'Content-Type': 'application/json' } }
      );
    }
  } catch (error) {
    console.error('Failed to submit rental request:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to submit rental request' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
