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

    // In a real application, you would save this to a database
    // or forward it to a moderation queue in the Web App.
    console.log('New Review Submitted:');
    console.log({ name, rating, comment });

    return new NextResponse(
      JSON.stringify({ message: 'Review submitted successfully!' }),
      { status: 201, headers: { 'Content-Type': 'application/json' } }
    );
  } catch (error) {
    console.error('Failed to process review submission:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to process review submission' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
