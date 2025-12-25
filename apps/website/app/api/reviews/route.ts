// [NEW]
import { NextResponse } from 'next/server';

export async function POST(request: Request) {
  try {
    const body = await request.json();
    const { name, rating, comment } = body;

    // Basic validation
    if (!name || !rating || !comment) {
      return new NextResponse(
        JSON.stringify({ message: 'Missing required fields' }),
        { status: 400, headers: { 'Content-Type': 'application/json' } }
      );
    }

    const res = await fetch('http://localhost:5207/api/Reviews', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, rating, comment }),
    });

    if (res.ok) {
        return new NextResponse(
            JSON.stringify({ message: 'Review submitted successfully!' }),
            { status: 201, headers: { 'Content-Type': 'application/json' } }
        );
    } else {
        return new NextResponse(
            JSON.stringify({ message: 'Failed to submit review' }),
            { status: 500, headers: { 'Content-Type': 'application/json' } }
        );
    }
  } catch (error) {
    console.error('Failed to process review submission:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to process review submission' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}

export async function GET() {
    try {
        const res = await fetch('http://localhost:5207/api/Reviews');
        const data = await res.json();
        return NextResponse.json(data);
    } catch (error) {
        console.error('Failed to fetch reviews:', error);
        return new NextResponse(
            JSON.stringify({ message: 'Failed to fetch reviews' }),
            { status: 500, headers: { 'Content-Type': 'application/json' } }
        );
    }
}
