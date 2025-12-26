// [NEW]
import { NextResponse } from 'next/server';

export const dynamic = 'force-dynamic';

const getFinancialSettings = async () => {
  // Mock data - replace with actual fetch to http://localhost:5000/api/v1/settings/financial
  return {
    showDonateButton: true,
    showMemberDuesButton: true,
  };
};

export async function GET() {
  try {
    const data = await getFinancialSettings();
    return NextResponse.json(data);
  } catch (error) {
    console.error('Failed to fetch financial settings:', error);
    return new NextResponse(
      JSON.stringify({ message: 'Failed to fetch financial settings' }),
      { status: 500, headers: { 'Content-Type': 'application/json' } }
    );
  }
}
