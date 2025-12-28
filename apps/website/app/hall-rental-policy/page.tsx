'use client';

import { Metadata } from 'next';
import { useEffect, useState } from 'react';

export default function HallRentalPolicyPage() {
    const [fromApplication, setFromApplication] = useState(false);
    const [pricing, setPricing] = useState<any>(null);

    useEffect(() => {
        // Check if user came from the rental application
        const referrer = document.referrer;
        const isFromApp = referrer.includes('/hall-rentals') || window.opener !== null;
        setFromApplication(isFromApp);

        // Fetch pricing from API
        fetch('/api/hall-rental/pricing')
            .then(res => res.json())
            .then(data => setPricing(data))
            .catch(err => console.error('Error fetching pricing:', err));
    }, []);

    return (
        <div className="min-h-screen bg-gradient-to-b from-midnight-blue via-midnight-blue to-deep-navy">
            {/* Header */}
            <div className="bg-midnight-blue border-b border-burnished-gold/20">
                <div className="max-w-5xl mx-auto px-4 py-12">
                    <div className="text-center">
                        <h1 className="text-4xl md:text-5xl font-display font-bold text-burnished-gold mb-4">
                            Hall Rental Policy
                        </h1>
                        <p className="text-pure-white/70 text-lg">
                            Gloucester Fraternity Club, Inc. • 27 Webster Street • Gloucester, MA 01930
                        </p>
                    </div>
                </div>
            </div>

            {/* Content */}
            <div className="max-w-5xl mx-auto px-4 py-12">
                <div className="bg-white/5 backdrop-blur-sm rounded-2xl border border-white/10 overflow-hidden">

                    {/* Table of Contents */}
                    <div className="bg-burnished-gold/10 border-b border-burnished-gold/20 p-6">
                        <h2 className="text-2xl font-display font-bold text-burnished-gold mb-4">Table of Contents</h2>
                        <nav className="grid md:grid-cols-2 gap-3">
                            <a href="#rental-details" className="text-pure-white hover:text-burnished-gold transition-colors">
                                → Rental Details & Pricing
                            </a>
                            <a href="#eligibility" className="text-pure-white hover:text-burnished-gold transition-colors">
                                → Eligibility Requirements
                            </a>
                            <a href="#kitchen" className="text-pure-white hover:text-burnished-gold transition-colors">
                                → Kitchen Usage Policy
                            </a>
                            <a href="#additional" className="text-pure-white hover:text-burnished-gold transition-colors">
                                → Additional Policies
                            </a>
                        </nav>
                    </div>

                    <div className="p-8 space-y-12 text-pure-white">

                        {/* Rental Details & Pricing */}
                        <section id="rental-details">
                            <h2 className="text-3xl font-display font-bold text-burnished-gold mb-6 border-b border-burnished-gold/20 pb-3">
                                Rental Details & Pricing
                            </h2>

                            <div className="space-y-6">
                                <div className="bg-black/30 p-6 rounded-xl border border-burnished-gold/20">
                                    <h3 className="text-xl font-bold text-burnished-gold mb-3">Function Time</h3>
                                    <ul className="space-y-2 text-pure-white/90">
                                        <li className="flex items-start gap-2">
                                            <span className="text-burnished-gold mt-1">•</span>
                                            <span><strong className="text-burnished-gold">Standard rental includes up to {pricing?.baseFunctionHours || 5} hours of function time</strong></span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-burnished-gold mt-1">•</span>
                                            <span>Additional hours available at <strong className="text-burnished-gold">${pricing?.additionalHourRate || 50} per hour</strong></span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-red-400 mt-1">⚠</span>
                                            <span className="text-red-400"><strong>Important:</strong> Setup and cleanup time are <strong>NOT included</strong> in your {pricing?.baseFunctionHours || 5} hours of event time</span>
                                        </li>
                                    </ul>
                                </div>

                                <div>
                                    <h3 className="text-xl font-bold text-burnished-gold mb-4">Pricing Structure</h3>

                                    <div className="grid md:grid-cols-2 gap-4 mb-6">
                                        <div className="bg-gradient-to-br from-green-900/30 to-green-800/20 p-5 rounded-xl border border-green-500/30">
                                            <h4 className="font-bold text-green-400 mb-3">Function Hall Rentals</h4>
                                            <table className="w-full text-sm">
                                                <tbody className="space-y-2">
                                                    <tr className="border-b border-green-500/20">
                                                        <td className="py-2 text-pure-white/80">Members</td>
                                                        <td className="py-2 text-right font-bold text-green-400">${pricing?.functionHallMemberRate || 300}</td>
                                                    </tr>
                                                    <tr className="border-b border-green-500/20">
                                                        <td className="py-2 text-pure-white/80">Non-Members</td>
                                                        <td className="py-2 text-right font-bold text-green-400">${pricing?.functionHallNonMemberRate || 400}</td>
                                                    </tr>
                                                    <tr className="border-b border-green-500/20">
                                                        <td className="py-2 text-pure-white/80">Coalition (Non-Member)</td>
                                                        <td className="py-2 text-right font-bold text-green-400">${pricing?.coalitionNonMemberRate || 200}</td>
                                                    </tr>
                                                    <tr className="border-b border-green-500/20">
                                                        <td className="py-2 text-pure-white/80">Coalition (Member)</td>
                                                        <td className="py-2 text-right font-bold text-green-400">${pricing?.coalitionMemberRate || 100}</td>
                                                    </tr>
                                                    <tr>
                                                        <td className="py-2 text-pure-white/80">Youth Organizations</td>
                                                        <td className="py-2 text-right font-bold text-green-400">${pricing?.youthOrganizationNonMemberRate || 100}</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <p className="text-xs text-pure-white/50 mt-3 italic">*Rates do not include bartender service</p>
                                        </div>

                                        <div className="bg-gradient-to-br from-burnished-gold/20 to-yellow-800/20 p-5 rounded-xl border border-burnished-gold/30">
                                            <h4 className="font-bold text-burnished-gold mb-3">Add-On Services</h4>
                                            <table className="w-full text-sm">
                                                <tbody className="space-y-2">
                                                    <tr className="border-b border-burnished-gold/20">
                                                        <td className="py-2 text-pure-white/80">Bar Service</td>
                                                        <td className="py-2 text-right font-bold text-burnished-gold">${pricing?.bartenderServiceFee || 100}</td>
                                                    </tr>
                                                    <tr className="border-b border-burnished-gold/20">
                                                        <td className="py-2 text-pure-white/80">Kitchen Use</td>
                                                        <td className="py-2 text-right font-bold text-burnished-gold">${pricing?.kitchenFee || 100}</td>
                                                    </tr>
                                                    <tr className="border-b border-burnished-gold/20">
                                                        <td className="py-2 text-pure-white/80">A/V Equipment</td>
                                                        <td className="py-2 text-right font-bold text-burnished-gold">
                                                            {pricing?.avEquipmentFee ? `$${pricing.avEquipmentFee}` : 'Contact'}
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td className="py-2 text-pure-white/80">Security Deposit</td>
                                                        <td className="py-2 text-right font-bold text-burnished-gold">
                                                            {pricing?.securityDepositAmount ? `$${pricing.securityDepositAmount}` : 'Required'}
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <p className="text-xs text-pure-white/50 mt-3 italic">*Security deposit is refundable</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>



                        {/* Eligibility Requirements */}
                        <section id="eligibility">
                            <h2 className="text-3xl font-display font-bold text-burnished-gold mb-6 border-b border-burnished-gold/20 pb-3">
                                Eligibility Requirements
                            </h2>

                            <div className="space-y-4">
                                <h3 className="text-2xl font-bold text-pure-white mb-4">Who May Rent the Hall</h3>

                                <div className="bg-green-900/20 p-6 rounded-xl border border-green-500/30">
                                    <h4 className="text-lg font-bold text-green-400 mb-3">1. Club Members</h4>
                                    <p className="text-pure-white/90">
                                        Any <strong>Regular, Guest, or Life member</strong> in good standing may rent the hall at member rates.
                                    </p>
                                </div>

                                <div className="bg-blue-900/20 p-6 rounded-xl border border-blue-500/30">
                                    <h4 className="text-lg font-bold text-blue-400 mb-3">2. Friends & Family of Members</h4>
                                    <ul className="space-y-2 text-pure-white/90">
                                        <li className="flex items-start gap-2">
                                            <span className="text-blue-400">•</span>
                                            <span>Friends or family members of a member in good standing may rent the hall</span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-blue-400">•</span>
                                            <span><strong>Pricing:</strong> Charged at non-member rates</span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-blue-400">•</span>
                                            <span><strong>Application Process:</strong> Same as "Management of the Hall" section</span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-blue-400">•</span>
                                            <span><strong>Note:</strong> Family members eligible to join as Regular members should be encouraged to do so, but this does not affect rental approval—only pricing</span>
                                        </li>
                                    </ul>
                                </div>

                                <div className="bg-purple-900/20 p-6 rounded-xl border border-purple-500/30">
                                    <h4 className="text-lg font-bold text-purple-400 mb-3">3. Outside Organizations</h4>
                                    <p className="text-pure-white/90">
                                        Any outside organization or group may rent the hall at <strong className="text-purple-400">non-member</strong> or <strong className="text-purple-400">organization meeting</strong> rates.
                                    </p>
                                </div>
                            </div>
                        </section>

                        {/* Kitchen Usage Policy */}
                        <section id="kitchen">
                            <h2 className="text-3xl font-display font-bold text-burnished-gold mb-6 border-b border-burnished-gold/20 pb-3">
                                Kitchen Usage Policy
                            </h2>

                            <div className="space-y-6">
                                <div className="grid md:grid-cols-2 gap-4">
                                    <div className="bg-green-900/20 p-6 rounded-xl border border-green-500/30">
                                        <h3 className="text-lg font-bold text-green-400 mb-3">Members (Regular, Guest, or Life)</h3>
                                        <ul className="space-y-2 text-pure-white/90 text-sm">
                                            <li className="flex items-start gap-2">
                                                <span className="text-green-400">✓</span>
                                                <span><strong>Full kitchen use at no additional charge</strong></span>
                                            </li>
                                            <li className="flex items-start gap-2">
                                                <span className="text-green-400">✓</span>
                                                <span>Must provide all details to Hall Rental Committee</span>
                                            </li>
                                            <li className="flex items-start gap-2">
                                                <span className="text-green-400">✓</span>
                                                <span>Committee must approve plans</span>
                                            </li>
                                        </ul>
                                    </div>

                                    <div className="bg-blue-900/20 p-6 rounded-xl border border-blue-500/30">
                                        <h3 className="text-lg font-bold text-blue-400 mb-3">Non-Members & Organizations</h3>
                                        <div className="space-y-3 text-sm">
                                            <div>
                                                <h4 className="font-bold text-pure-white mb-1">Light Use (No Additional Fee)</h4>
                                                <ul className="space-y-1 text-pure-white/80 ml-4">
                                                    <li>• Heating meatballs, light appetizers</li>
                                                    <li>• Preparing pastry trays</li>
                                                    <li>• Warming food in pizza ovens</li>
                                                </ul>
                                            </div>
                                            <div>
                                                <h4 className="font-bold text-burnished-gold mb-1">Full Use (${pricing?.kitchenFee || 100} Fee)</h4>
                                                <p className="text-pure-white/80">
                                                    Full kitchen use permitted <strong>only with committee approval</strong> for preparing full meals on premises or commercial caterer cooking on-site.
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div className="bg-red-900/20 p-6 rounded-xl border border-red-500/30">
                                    <h3 className="text-xl font-bold text-red-400 mb-4">Kitchen Rules & Requirements</h3>
                                    <div className="grid md:grid-cols-2 gap-4">
                                        <div>
                                            <h4 className="font-bold text-pure-white mb-2">General Rules</h4>
                                            <ul className="space-y-2 text-pure-white/90 text-sm">
                                                <li className="flex items-start gap-2">
                                                    <span className="text-red-400">❌</span>
                                                    <span><strong>NO CLUB UTENSILS</strong> provided (applies to members and non-members)</span>
                                                </li>
                                                <li className="flex items-start gap-2">
                                                    <span className="text-green-400">✓</span>
                                                    <span>Refrigerator available for storage (remove all perishables after event)</span>
                                                </li>
                                                <li className="flex items-start gap-2">
                                                    <span className="text-green-400">✓</span>
                                                    <span>Dumpster available for use</span>
                                                </li>
                                                <li className="flex items-start gap-2">
                                                    <span className="text-green-400">✓</span>
                                                    <span>Coffee pot available by arrangement</span>
                                                </li>
                                                <li className="flex items-start gap-2">
                                                    <span className="text-red-400">⚠</span>
                                                    <span><strong>Lower ovens on gas stove are NOT to be used under any circumstances</strong></span>
                                                </li>
                                                <li className="flex items-start gap-2">
                                                    <span className="text-green-400">✓</span>
                                                    <span>Pizza ovens may be used for warming food</span>
                                                </li>
                                            </ul>
                                        </div>
                                        <div>
                                            <h4 className="font-bold text-pure-white mb-2">Caterer Requirements</h4>
                                            <ul className="space-y-2 text-pure-white/90 text-sm">
                                                <li className="flex items-start gap-2">
                                                    <span className="text-yellow-400">⚠</span>
                                                    <span><strong>Proof of LIABILITY INSURANCE</strong> must be on file with the club</span>
                                                </li>
                                                <li className="flex items-start gap-2">
                                                    <span className="text-yellow-400">⚠</span>
                                                    <span>If caterer has no insurance, <strong>Board of Directors must approve</strong> (via Vice President) before use</span>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>

                        {/* Additional Policies */}
                        <section id="additional">
                            <h2 className="text-3xl font-display font-bold text-burnished-gold mb-6 border-b border-burnished-gold/20 pb-3">
                                Additional Policies
                            </h2>

                            <div className="space-y-4">
                                <div className="bg-black/30 p-6 rounded-xl border border-white/10">
                                    <h3 className="text-lg font-bold text-burnished-gold mb-2">Janitorial Services</h3>
                                    <ul className="space-y-2 text-pure-white/90">
                                        <li className="flex items-start gap-2">
                                            <span className="text-green-400">•</span>
                                            <span>Standard cleaning is included in rental fee</span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-yellow-400">•</span>
                                            <span><strong>Additional janitorial effort required:</strong> Cost deducted from Security Deposit</span>
                                        </li>
                                    </ul>
                                </div>

                                <div className="bg-gradient-to-br from-purple-900/30 to-pink-900/20 p-6 rounded-xl border border-purple-500/30">
                                    <h3 className="text-lg font-bold text-purple-400 mb-2">50th Anniversary Special</h3>
                                    <p className="text-pure-white/90 mb-3">
                                        Any Regular, Guest, or Life member celebrating their <strong className="text-purple-400">50th anniversary</strong> may have the hall at <strong className="text-purple-400">no charge</strong>:
                                    </p>
                                    <ul className="space-y-2 text-pure-white/90">
                                        <li className="flex items-start gap-2">
                                            <span className="text-purple-400">•</span>
                                            <span>Application may be presented by family member or friend</span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-purple-400">•</span>
                                            <span><strong>Only janitorial fee required</strong></span>
                                        </li>
                                        <li className="flex items-start gap-2">
                                            <span className="text-purple-400">•</span>
                                            <span>If bar service requested, <strong>bartender cost covered by club</strong></span>
                                        </li>
                                    </ul>
                                </div>

                                <div className="bg-black/30 p-6 rounded-xl border border-white/10">
                                    <h3 className="text-lg font-bold text-burnished-gold mb-2">Advertising Restrictions</h3>
                                    <p className="text-pure-white/90">
                                        When the hall is rented by a group or organization: <strong className="text-red-400">No outside advertising</strong> unless approved by Hall Rental Committee. All advertising content must be reviewed and approved in advance.
                                    </p>
                                </div>


                            </div>
                        </section>

                        {/* Contact */}
                        <div className="bg-gradient-to-r from-burnished-gold/20 to-yellow-800/20 p-8 rounded-xl border border-burnished-gold/30 text-center">
                            <h3 className="text-2xl font-display font-bold text-burnished-gold mb-4">Contact Information</h3>
                            <p className="text-pure-white/90 mb-2">
                                <strong>Gloucester Fraternity Club, Inc.</strong>
                            </p>
                            <p className="text-pure-white/80 mb-4">
                                27 Webster Street<br />
                                Gloucester, Massachusetts 01930
                            </p>
                            <p className="text-pure-white/70 text-sm italic">
                                For hall rental inquiries, please submit an application through our website or contact the Hall Rental Committee.
                            </p>
                            <div className="mt-6">
                                {fromApplication ? (
                                    <button
                                        onClick={() => window.close()}
                                        className="inline-block bg-burnished-gold hover:bg-yellow-600 text-midnight-blue font-bold px-8 py-3 rounded-lg transition-colors shadow-lg"
                                    >
                                        ← Return to Hall Rental Application
                                    </button>
                                ) : (
                                    <a
                                        href="/hall-rentals"
                                        className="inline-block bg-burnished-gold hover:bg-yellow-600 text-midnight-blue font-bold px-8 py-3 rounded-lg transition-colors shadow-lg"
                                    >
                                        Apply for Hall Rental
                                    </a>
                                )}
                            </div>
                        </div>

                        {/* Footer Note */}
                        <div className="text-center text-pure-white/50 text-sm italic pt-8 border-t border-white/10">
                            This policy is subject to change by membership vote. Last updated: December 2025
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
