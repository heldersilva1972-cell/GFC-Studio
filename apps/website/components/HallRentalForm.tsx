// [MODIFIED]
'use client';

import { useState, FormEvent, useEffect } from 'react';
import { motion } from 'framer-motion';

interface HallRentalFormProps {
  selectedDate: Date;
  onSuccess: (data: any) => void;
}

const EVENT_TYPES = [
  "Wedding",
  "Birthday Party",
  "Fundraiser",
  "Bereavement / Collation",
  "Meeting / Seminar",
  "Holiday Party",
  "Anniversary",
  "Show / Performance",
  "Other"
];

export default function HallRentalForm({ selectedDate, onSuccess }: HallRentalFormProps) {
  const [formData, setFormData] = useState({
    name: '',
    address: '',
    city: '',
    zip: '',
    phone: '',
    email: '',
    dob: '',
    memberStatus: 'Non-Member',
    sponsoringMember: '',
    eventType: '', // Default to empty
    otherEventType: '', // Extra field for "Other" generic input
    guestCount: '',
    startTime: '',
    endTime: '',
    setupNeeded: 'No',
    kitchenUse: 'No',
    catererName: '',
    barService: 'No',
    chargingAdmission: false,
    details: '',

    // Policies
    policyAge: false,
    policyAlcohol: false,
    policyDecorations: false,
    policyPayment: false,
    policyCancellation: false,
    policyDamage: false,
  });

  const [status, setStatus] = useState<'idle' | 'submitting' | 'saving' | 'success' | 'error' | 'saved'>('idle');
  const [message, setMessage] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { id, value } = e.target;
    if ((e.target as HTMLInputElement).type === 'checkbox') {
      setFormData((prev) => ({ ...prev, [id]: (e.target as HTMLInputElement).checked }));
    } else {
      setFormData((prev) => ({ ...prev, [id]: value }));
    }
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();

    // 1. Strict Validation
    const requiredFields = [
      formData.name, formData.address, formData.city, formData.zip,
      formData.phone, formData.email, formData.dob, formData.guestCount,
      formData.startTime, formData.endTime, formData.eventType
    ];

    if (requiredFields.some(field => !field || field.trim() === '')) {
      setStatus('error');
      setMessage('Please fill in all required fields.');
      return;
    }

    // Validate "Other" description if selected
    if (formData.eventType === 'Other' && !formData.otherEventType.trim()) {
      setStatus('error');
      setMessage('Please describe your event type.');
      return;
    }

    const requiredPolicies = ['policyAge', 'policyAlcohol', 'policyDecorations', 'policyPayment', 'policyCancellation', 'policyDamage'];
    if (requiredPolicies.some(p => !formData[p as keyof typeof formData])) {
      setStatus('error');
      setMessage('You must acknowledge ALL club policies to proceed.');
      return;
    }

    // 2. Age Check 
    const birthDate = new Date(formData.dob);
    const ageDifMs = Date.now() - birthDate.getTime();
    if (new Date(ageDifMs).getUTCFullYear() - 1970 < 21) {
      setStatus('error');
      setMessage('You must be at least 21 years of age to rent the hall.');
      return;
    }

    setStatus('submitting');
    setMessage('');

    try {
      // Determine final event type string
      const finalEventType = formData.eventType === 'Other' ? `Other: ${formData.otherEventType}` : formData.eventType;

      const compiledDetails = `
        --- APPLICANT INFO ---
        DOB: ${formData.dob}
        Address: ${formData.address}, ${formData.city} ${formData.zip}
        
        --- EVENT DETAILS ---
        Type: ${finalEventType}
        Time: ${formData.startTime} to ${formData.endTime}
        Guests: ${formData.guestCount}
        Admission Charged: ${formData.chargingAdmission ? 'Yes' : 'No'}
        
        --- SERVICES ---
        Bar Service: ${formData.barService}
        Kitchen: ${formData.kitchenUse}
        Caterer: ${formData.catererName || 'None'}
        Setup Needed: ${formData.setupNeeded}
        
        --- MEMBERSHIP ---
        Status: ${formData.memberStatus}
        Sponsor: ${formData.sponsoringMember || 'N/A'}
        
        --- NOTES ---
        ${formData.details}
      `;

      const res = await fetch('/api/HallRentalInquiry', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          requesterName: formData.name,
          requesterEmail: formData.email,
          requesterPhone: formData.phone,
          eventDetails: compiledDetails,
          requestedDate: selectedDate,
          status: 'Pending',
        }),
      });

      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message || 'Something went wrong');
      }

      setStatus('success');
      onSuccess(formData);

    } catch (err) {
      setStatus('error');
      setMessage(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-8 text-pure-white max-w-4xl mx-auto">

      {/* 1. APPLICANT INFORMATION */}
      <div className="bg-white/5 p-6 rounded-xl border border-white/10">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 border-b border-burnished-gold/20 pb-2">1. Applicant & Membership</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label htmlFor="name" className="block text-sm font-medium mb-1">Full Name *</label>
            <input type="text" id="name" value={formData.name} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 focus:border-burnished-gold outline-none" />
          </div>
          <div>
            <label htmlFor="memberStatus" className="block text-sm font-medium mb-1">Membership Status *</label>
            <select id="memberStatus" value={formData.memberStatus} onChange={handleChange} className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
              <option value="Non-Member">Non-Member</option>
              <option value="Member">Member</option>
            </select>
          </div>
          {formData.memberStatus === 'Member' && (
            <div className="md:col-span-2 bg-burnished-gold/10 p-3 rounded">
              <p className="text-sm text-burnished-gold font-bold">Please note: Membership status will be verified against club records.</p>
            </div>
          )}
          <div>
            <label htmlFor="dob" className="block text-sm font-medium mb-1">Date of Birth (Must be 21+) *</label>
            <input type="date" id="dob" value={formData.dob} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 focus:border-burnished-gold outline-none text-white text-sm" />
          </div>
          <div className="md:col-span-2">
            <label htmlFor="address" className="block text-sm font-medium mb-1">Street Address *</label>
            <input type="text" id="address" value={formData.address} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
          </div>
          <div className="md:col-span-2 grid grid-cols-2 gap-4">
            <div>
              <label htmlFor="city" className="block text-sm font-medium mb-1">City *</label>
              <input type="text" id="city" value={formData.city} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
            </div>
            <div>
              <label htmlFor="zip" className="block text-sm font-medium mb-1">Zip Code *</label>
              <input type="text" id="zip" value={formData.zip} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
            </div>
          </div>
          <div>
            <label htmlFor="phone" className="block text-sm font-medium mb-1">Phone Number *</label>
            <input type="tel" id="phone" value={formData.phone} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
          </div>
          <div>
            <label htmlFor="email" className="block text-sm font-medium mb-1">Email Address *</label>
            <input type="email" id="email" value={formData.email} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
          </div>
        </div>
      </div>

      {/* 2. EVENT DETAILS */}
      <div className="bg-white/5 p-6 rounded-xl border border-white/10">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 border-b border-burnished-gold/20 pb-2">2. Event Details</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="md:col-span-2">
            <label htmlFor="eventType" className="block text-sm font-medium mb-1">Type of Function *</label>
            <select id="eventType" value={formData.eventType} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
              <option value="" disabled>Select Function Type...</option>
              {EVENT_TYPES.map(type => (
                <option key={type} value={type}>{type}</option>
              ))}
            </select>
            {formData.eventType === 'Other' && (
              <div className="mt-3 animate-fade-in">
                <label htmlFor="otherEventType" className="block text-sm font-medium mb-1 text-burnished-gold">Please describe the function *</label>
                <input type="text" id="otherEventType" value={formData.otherEventType} onChange={handleChange} required className="w-full bg-midnight-blue border border-burnished-gold rounded-md px-3 py-2 outline-none" placeholder="e.g. Retirement Party, Baby Shower" />
              </div>
            )}
          </div>

          {/* Simplified Time Entry */}
          <div>
            <label htmlFor="startTime" className="block text-sm font-medium mb-1">Start Time *</label>
            <input type="text" id="startTime" value={formData.startTime} onChange={handleChange} placeholder="e.g. 6:00 PM" required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
          </div>
          <div>
            <label htmlFor="endTime" className="block text-sm font-medium mb-1">End Time *</label>
            <input type="text" id="endTime" value={formData.endTime} onChange={handleChange} placeholder="e.g. 11:00 PM" required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
          </div>

          <div>
            <label htmlFor="guestCount" className="block text-sm font-medium mb-1">Number of Guests (Max 200) *</label>
            <input type="number" id="guestCount" max="200" value={formData.guestCount} onChange={handleChange} required className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none" />
          </div>
          <div className="flex items-center pt-6">
            <input type="checkbox" id="chargingAdmission" checked={formData.chargingAdmission} onChange={handleChange} className="w-5 h-5 text-burnished-gold" />
            <label htmlFor="chargingAdmission" className="ml-2 text-sm">Will admission be charged?</label>
          </div>
        </div>
      </div>

      {/* 3. LOGISTICS & POLICIES */}
      <div className="bg-white/5 p-6 rounded-xl border border-white/10 space-y-8">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 border-b border-burnished-gold/20 pb-2">3. Logistics & Services</h3>

        {/* SETUP TIME */}
        <div>
          <label htmlFor="setupNeeded" className="block text-lg font-bold mb-2">Do you require setup time prior to your event start time listed above? *</label>
          <div className="bg-black/20 p-4 rounded mb-2 text-gray-300 text-sm leading-relaxed border-l-2 border-burnished-gold">
            Please note: The hall will be empty upon your arrival. Tables and chairs are available for your use, but setup and takedown are the responsibility of the renter. The hall must be returned to its original clean and empty condition at the end of your rental. Extra setup time is subject to availability; we will contact you to review details and confirm what’s possible.
          </div>
          <select id="setupNeeded" value={formData.setupNeeded} onChange={handleChange} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
            <option value="No">No</option>
            <option value="Yes">Yes</option>
          </select>
        </div>

        {/* BAR SERVICE */}
        <div>
          <label htmlFor="barService" className="block text-lg font-bold mb-2">Bar Service (alcohol & bartender – additional cost)? *</label>
          <div className="bg-black/20 p-4 rounded mb-2 text-gray-300 text-sm leading-relaxed border-l-2 border-burnished-gold">
            Selecting this option provides a staffed bar with alcoholic beverages. If not selected, guests are strictly prohibited from bringing alcohol onto the premises under any circumstances.
          </div>
          <select id="barService" value={formData.barService} onChange={handleChange} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
            <option value="No">No</option>
            <option value="Yes">Yes</option>
          </select>
        </div>

        {/* KITCHEN POLICY */}
        <div>
          <label htmlFor="kitchenUse" className="block text-lg font-bold mb-2">Kitchen Access Requested? *</label>
          <div className="bg-black/20 p-4 rounded mb-2 text-gray-300 text-sm leading-relaxed border-l-2 border-burnished-gold">
            <strong>GLOUCESTER FRATERNITY CLUB KITCHEN POLICY:</strong><br />
            The kitchen may not be used for cooking meals. Caterers may use the kitchen for cooking but only with proof of <strong>LIABILITY INSURANCE</strong>. The lower ovens on the gas stove are <strong>NOT</strong> to be used under any circumstances. The pizza ovens may be used for warming food. The refrigerator may be used for storage but please take all perishable items with you. The Dumpster is available for your use. The Gloucester Fraternity Club provides no utensils, pots or pans for use. Arrangements can be made for the use of our coffee pot.
          </div>
          <select id="kitchenUse" value={formData.kitchenUse} onChange={handleChange} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
            <option value="No">No</option>
            <option value="Yes">Yes</option>
          </select>

          {formData.kitchenUse === 'Yes' && (
            <div className="mt-4 animate-fade-in">
              <label htmlFor="catererName" className="block text-sm font-medium mb-1 text-burnished-gold">Caterer Name & Insurance Info (Required for cooking)</label>
              <input type="text" id="catererName" value={formData.catererName} onChange={handleChange} placeholder="Enter 'Self' or Caterer Name" className="w-full bg-midnight-blue border border-burnished-gold rounded-md px-3 py-2 outline-none" />
            </div>
          )}
        </div>

        <div>
          <label htmlFor="details" className="block text-sm font-medium mb-1">Additional Notes / Special Requests</label>
          <textarea id="details" rows={3} value={formData.details} onChange={handleChange} className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none"></textarea>
        </div>
      </div>

      {/* 4. MANDATORY CONTRACT */}
      <div className="bg-black/40 p-6 rounded-xl border border-white/10 space-y-4">
        <h3 className="text-xl font-display font-bold text-red-400 mb-2">Usage Agreement</h3>

        <div className="flex items-start gap-3">
          <input type="checkbox" id="policyAge" checked={formData.policyAge} onChange={handleChange} className="mt-1 w-5 h-5 accent-burnished-gold" />
          <label htmlFor="policyAge" className="text-sm cursor-pointer">I attest that I am at least <strong>21 years of age</strong>.</label>
        </div>

        <div className="flex items-start gap-3">
          <input type="checkbox" id="policyAlcohol" checked={formData.policyAlcohol} onChange={handleChange} className="mt-1 w-5 h-5 accent-burnished-gold" />
          <label htmlFor="policyAlcohol" className="text-sm cursor-pointer">I understand that <strong>NO OUTSIDE ALCOHOL</strong> is permitted on the premises. Violation results in immediate termination of the event.</label>
        </div>

        <div className="flex items-start gap-3">
          <input type="checkbox" id="policyDecorations" checked={formData.policyDecorations} onChange={handleChange} className="mt-1 w-5 h-5 accent-burnished-gold" />
          <label htmlFor="policyDecorations" className="text-sm cursor-pointer">I agree NOT to use scotch tape, tacks, or nails on any walls, ceilings, or equipment.</label>
        </div>

        <div className="flex items-start gap-3">
          <input type="checkbox" id="policyCancellation" checked={formData.policyCancellation} onChange={handleChange} className="mt-1 w-5 h-5 accent-burnished-gold" />
          <label htmlFor="policyCancellation" className="text-sm cursor-pointer">I understand that cancellations made less than <strong>30 days</strong> prior to the date will forfeit the rental fee.</label>
        </div>

        <div className="flex items-start gap-3">
          <input type="checkbox" id="policyPayment" checked={formData.policyPayment} onChange={handleChange} className="mt-1 w-5 h-5 accent-burnished-gold" />
          <label htmlFor="policyPayment" className="text-sm cursor-pointer">I agree to pay the Hall Rental Fee in full within <strong>2 business days</strong> of approval to reserve the date.</label>
        </div>

        <div className="flex items-start gap-3">
          <input type="checkbox" id="policyDamage" checked={formData.policyDamage} onChange={handleChange} className="mt-1 w-5 h-5 accent-burnished-gold" />
          <label htmlFor="policyDamage" className="text-sm cursor-pointer">I accept full responsibility for any damage to the building, equipment, or fixtures caused by my guests.</label>
        </div>
      </div>

      <motion.button
        type="submit"
        disabled={status === 'submitting'}
        whileHover={{ scale: 1.02 }}
        whileTap={{ scale: 0.98 }}
        className="w-full bg-gradient-to-r from-burnished-gold to-yellow-600 text-midnight-blue font-bold px-6 py-5 rounded-lg hover:shadow-lg hover:shadow-burnished-gold/20 transition-all disabled:opacity-50 text-xl"
      >
        {status === 'submitting' ? 'Submitting Application...' : 'Submit Rental Application'}
      </motion.button>

      {status === 'error' && <p className="text-red-400 text-center font-bold bg-red-900/20 p-4 rounded border border-red-500/50">{message}</p>}
    </form>
  );
}
