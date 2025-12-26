// [NEW]
import { NextResponse } from 'next/server';

export const dynamic = 'force-dynamic';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207';

export async function GET() {
    try {
        const res = await fetch(`${API_URL}/api/WebsiteSettings`, {
            next: { revalidate: 60 } // Cache for 1 minute
        });

        if (!res.ok) {
            throw new Error('Failed to fetch from backend');
        }

        const settings = await res.json();

        // Only return public-safe pricing info
        const pricing = {
            memberRate: settings.memberRate,
            nonMemberRate: settings.nonMemberRate,
            nonProfitRate: settings.nonProfitRate,
            kitchenFee: settings.kitchenFee,
            avEquipmentFee: settings.avEquipmentFee,
            securityDepositAmount: settings.securityDepositAmount,
            maxHallRentalDurationHours: settings.maxHallRentalDurationHours
        };

        return NextResponse.json(pricing);
    } catch (error) {
        console.error('Failed to fetch hall rental pricing:', error);
        // Fallback to baseline defaults if backend is down
        return NextResponse.json({
            memberRate: 250,
            nonMemberRate: 500,
            nonProfitRate: 350,
            kitchenFee: 50,
            avEquipmentFee: 25,
            securityDepositAmount: 100,
            maxHallRentalDurationHours: 8
        });
    }
}
