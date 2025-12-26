import { NextResponse } from 'next/server';

export async function GET() {
  try {
    // Fetch from backend
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207'}/api/WebsiteSettings`);

    if (!response.ok) {
      return NextResponse.json({ isOpen: true }, { status: 200 }); // Default to open
    }

    const settings = await response.json();
    return NextResponse.json({ isOpen: settings.isClubOpen ?? true });
  } catch (error) {
    // If backend is down, default to open
    return NextResponse.json({ isOpen: true }, { status: 200 });
  }
}
