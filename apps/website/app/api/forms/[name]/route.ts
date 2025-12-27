// [NEW]
import { NextResponse } from 'next/server';

export async function GET(
  request: Request,
  { params }: { params: { name: string } }
) {
  const formName = params.name;
  const backendUrl = `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/forms/${formName}`;

  try {
    const response = await fetch(backendUrl);

    if (!response.ok) {
      throw new Error(`Failed to fetch form schema: ${response.statusText}`);
    }

    const data = await response.json();
    return NextResponse.json(data);
  } catch (error) {
    return NextResponse.json({ message: error.message }, { status: 500 });
  }
}
