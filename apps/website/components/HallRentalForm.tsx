// [MODIFIED]
'use client';

import { useState, FormEvent, useEffect } from 'react';
import { motion } from 'framer-motion';

interface HallRentalFormProps {
  selectedDate: Date;
  onSuccess: (data: any) => void;
  initialData?: any;
  onChange?: (data: any) => void;
}

const EVENT_TYPES = [
  "Wedding",
  "Birthday Party",
  "Baby Shower",
  "Fundraiser",
  "Bereavement / Collation",
  "Meeting / Seminar",
  "Holiday Party",
  "Anniversary",
  "Show / Performance",
  "Other"
];

export default function HallRentalForm({ selectedDate, onSuccess, initialData, onChange }: HallRentalFormProps) {
  const [formData, setFormData] = useState(initialData || {
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
    let newFormData;

    // Checkbox handling
    if ((e.target as HTMLInputElement).type === 'checkbox') {
      newFormData = { ...formData, [id]: (e.target as HTMLInputElement).checked };
    } else {
      newFormData = { ...formData, [id]: value };
    }

    setFormData(newFormData);

    // Propagate changes for auto-save/draft
    if (onChange) onChange(newFormData);
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
          eventType: finalEventType,
          startTime: formData.startTime,
          endTime: formData.endTime,
        }),
      });

      if (!res.ok) {
        // [FIX] Read text once, then parse
        const text = await res.text();
        let errorMsg = 'Something went wrong';
        try {
          const data = JSON.parse(text);
          errorMsg = data.message || errorMsg;
          if (data.details) errorMsg += `: ${data.details}`;
        } catch {
          console.error('Server Error:', text);
          errorMsg = `Server Error: ${text.substring(0, 200)}`;
        }
        throw new Error(errorMsg);
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
            <label className="block text-sm font-medium mb-1">Date of Birth (Must be 21+) *</label>
            <div className="flex gap-2">
              <select
                id="dobMonth"
                value={formData.dob.split('-')[1] || ''}
                onChange={(e) => {
                  const month = e.target.value;
                  const day = formData.dob.split('-')[2] || '01';
                  const year = formData.dob.split('-')[0] || '';
                  if (year) setFormData({ ...formData, dob: `${year}-${month}-${day}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="">Month</option>
                <option value="01">January</option>
                <option value="02">February</option>
                <option value="03">March</option>
                <option value="04">April</option>
                <option value="05">May</option>
                <option value="06">June</option>
                <option value="07">July</option>
                <option value="08">August</option>
                <option value="09">September</option>
                <option value="10">October</option>
                <option value="11">November</option>
                <option value="12">December</option>
              </select>
              <select
                id="dobDay"
                value={formData.dob.split('-')[2] || ''}
                onChange={(e) => {
                  const month = formData.dob.split('-')[1] || '01';
                  const day = e.target.value;
                  const year = formData.dob.split('-')[0] || '';
                  if (year) setFormData({ ...formData, dob: `${year}-${month}-${day}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="">Day</option>
                {[...Array(31)].map((_, i) => {
                  const day = (i + 1).toString().padStart(2, '0');
                  return <option key={day} value={day}>{day}</option>;
                })}
              </select>
              <select
                id="dobYear"
                value={formData.dob.split('-')[0] || ''}
                onChange={(e) => {
                  const month = formData.dob.split('-')[1] || '01';
                  const day = formData.dob.split('-')[2] || '01';
                  const year = e.target.value;
                  setFormData({ ...formData, dob: `${year}-${month}-${day}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="">Year</option>
                {(() => {
                  const currentYear = new Date().getFullYear();
                  const minYear = currentYear - 100; // 100 years ago
                  const maxYear = currentYear - 21; // Must be 21+
                  const years = [];
                  for (let year = maxYear; year >= minYear; year--) {
                    years.push(<option key={year} value={year}>{year}</option>);
                  }
                  return years;
                })()}
              </select>
            </div>
            <p className="text-xs text-white/50 mt-1">You must be at least 21 years old to rent the hall</p>
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
                <input type="text" id="otherEventType" value={formData.otherEventType} onChange={handleChange} required className="w-full bg-midnight-blue border border-burnished-gold rounded-md px-3 py-2 outline-none" placeholder="e.g. Retirement Party" />
              </div>
            )}
          </div>

          {/* Start Time - Dropdown Selects */}
          <div>
            <label className="block text-sm font-medium mb-1">Start Time *</label>
            <div className="flex gap-2">
              <select
                id="startHour"
                value={formData.startTime.split(':')[0] || ''}
                onChange={(e) => {
                  const hour = e.target.value;
                  const minute = formData.startTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = formData.startTime.split(' ')[1] || 'PM';
                  setFormData({ ...formData, startTime: `${hour}:${minute} ${period}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="">Hour</option>
                {[...Array(12)].map((_, i) => {
                  const hour = i + 1;
                  return <option key={hour} value={hour}>{hour}</option>;
                })}
              </select>
              <select
                id="startMinute"
                value={formData.startTime.split(':')[1]?.split(' ')[0] || '00'}
                onChange={(e) => {
                  const hour = formData.startTime.split(':')[0] || '1';
                  const minute = e.target.value;
                  const period = formData.startTime.split(' ')[1] || 'PM';
                  setFormData({ ...formData, startTime: `${hour}:${minute} ${period}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="00">00</option>
                <option value="15">15</option>
                <option value="30">30</option>
                <option value="45">45</option>
              </select>
              <select
                id="startPeriod"
                value={formData.startTime.split(' ')[1] || 'PM'}
                onChange={(e) => {
                  const hour = formData.startTime.split(':')[0] || '1';
                  const minute = formData.startTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = e.target.value;
                  setFormData({ ...formData, startTime: `${hour}:${minute} ${period}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="AM">AM</option>
                <option value="PM">PM</option>
              </select>
            </div>
          </div>

          {/* End Time - Dropdown Selects */}
          <div>
            <label className="block text-sm font-medium mb-1">End Time *</label>
            <div className="flex gap-2">
              <select
                id="endHour"
                value={formData.endTime.split(':')[0] || ''}
                onChange={(e) => {
                  const hour = e.target.value;
                  const minute = formData.endTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = formData.endTime.split(' ')[1] || 'PM';
                  setFormData({ ...formData, endTime: `${hour}:${minute} ${period}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="">Hour</option>
                {[...Array(12)].map((_, i) => {
                  const hour = i + 1;
                  return <option key={hour} value={hour}>{hour}</option>;
                })}
              </select>
              <select
                id="endMinute"
                value={formData.endTime.split(':')[1]?.split(' ')[0] || '00'}
                onChange={(e) => {
                  const hour = formData.endTime.split(':')[0] || '1';
                  const minute = e.target.value;
                  const period = formData.endTime.split(' ')[1] || 'PM';
                  setFormData({ ...formData, endTime: `${hour}:${minute} ${period}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="00">00</option>
                <option value="15">15</option>
                <option value="30">30</option>
                <option value="45">45</option>
              </select>
              <select
                id="endPeriod"
                value={formData.endTime.split(' ')[1] || 'PM'}
                onChange={(e) => {
                  const hour = formData.endTime.split(':')[0] || '1';
                  const minute = formData.endTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = e.target.value;
                  setFormData({ ...formData, endTime: `${hour}:${minute} ${period}` });
                }}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="AM">AM</option>
                <option value="PM">PM</option>
              </select>
            </div>
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
