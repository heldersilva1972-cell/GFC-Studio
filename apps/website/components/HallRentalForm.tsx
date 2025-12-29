// [MODIFIED]
'use client';

import { useState, FormEvent, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';

interface HallRentalFormProps {
  selectedDate: Date;
  onSuccess: (data: any) => void;
  initialData?: any;
  onChange?: (data: any) => void;
}

const EVENT_TYPES = [
  "Wedding", "Birthday Party", "Baby Shower", "Fundraiser",
  "Bereavement / Collation", "Meeting / Seminar", "Holiday Party",
  "Anniversary", "Graduation Party", "Show / Performance", "Youth Organization Function", "Other"
];

const YOUTH_FUNCTIONS = [
  "Sports Team Celebration",
  "Cheerleading Event",
  "Dance Event",
  "Youth Group Meeting",
  "Scouts Event",
  "Academic Achievement Celebration",
  "Other Youth Function"
];

interface PricingSettings {
  functionHallMemberRate: number;
  functionHallNonMemberRate: number;
  coalitionMemberRate: number;
  coalitionNonMemberRate: number;
  youthOrganizationMemberRate: number;
  youthOrganizationNonMemberRate: number;
  bartenderServiceFee: number;
  kitchenFee: number;
  avEquipmentFee: number;
  securityDepositAmount: number;
  baseFunctionHours: number;
  additionalHourRate: number;
}

export default function HallRentalForm({ selectedDate, onSuccess, initialData, onChange }: HallRentalFormProps) {
  const [pricing, setPricing] = useState<PricingSettings | null>(null);
  const [formData, setFormData] = useState(initialData || {
    name: '',
    address: '',
    city: '',
    zip: '',
    phone: '',
    email: '',
    dob: '',
    memberStatus: 'Non-Member', // Values: 'Member', 'Non-Member', 'Non-Profit'
    sponsoringMember: '',
    eventType: '',
    otherEventType: '',
    youthFunctionType: '',
    guestCount: '',
    startTime: '',
    endTime: '',
    setupNeeded: 'No',
    kitchenUse: 'No',
    avEquipment: 'No',
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

    // Important Rental Policies
    policyFunctionTime: false,
    policyCommitteeApproval: false,
    policyKitchenRules: false,
    policySecurityDeposit: false,
  });

  useEffect(() => {
    const fetchPricing = async () => {
      try {
        const res = await fetch('/api/hall-rental/pricing');
        if (res.ok) {
          const data = await res.json();
          setPricing(data);
        }
      } catch (error) {
        console.error('Failed to fetch pricing:', error);
      }
    };
    fetchPricing();
  }, []);

  const getDurationHours = (start: string, end: string) => {
    if (!start || !end || start.includes('Hour') || end.includes('Hour')) return 0;

    const parseTime = (timeStr: string) => {
      const parts = timeStr.split(' ');
      if (parts.length < 2) return 0;
      const [time, period] = parts;
      let [hours, minutes] = time.split(':').map(Number);
      if (period === 'PM' && hours < 12) hours += 12;
      if (period === 'AM' && hours === 12) hours = 0;
      return hours + minutes / 60;
    };

    const startTime = parseTime(start);
    const endTime = parseTime(end);

    let duration = endTime - startTime;
    if (duration <= 0) duration += 24; // Handle overnight
    return duration;
  };

  const calculateEstimate = () => {
    if (!pricing) return 0;

    let total = 0;

    // Determine hall type based on event type
    let baseRate = 0;
    const isMember = formData.memberStatus === 'Member';

    if (formData.eventType === 'Youth Organization Function') {
      // Youth Organization rates
      baseRate = isMember ? pricing.youthOrganizationMemberRate : pricing.youthOrganizationNonMemberRate;
    } else if (formData.eventType === 'Bereavement / Collation') {
      // Coalition rates
      baseRate = isMember ? pricing.coalitionMemberRate : pricing.coalitionNonMemberRate;
    } else {
      // Function Hall rates (default for all other events)
      baseRate = isMember ? pricing.functionHallMemberRate : pricing.functionHallNonMemberRate;
    }

    total += baseRate;

    // Additional Hours (using settings)
    const baseFunctionHours = pricing.baseFunctionHours || 5;
    const additionalHourRate = pricing.additionalHourRate || 50;
    const duration = getDurationHours(formData.startTime, formData.endTime);

    if (duration > baseFunctionHours) {
      const extraHours = Math.ceil(duration - baseFunctionHours);
      total += extraHours * additionalHourRate;
    }

    // Add-ons
    if (formData.kitchenUse === 'Yes') total += (pricing.kitchenFee || 50);
    if (formData.avEquipment === 'Yes') total += (pricing.avEquipmentFee || 25);
    if (formData.barService === 'Yes') total += (pricing.bartenderServiceFee || 100);

    return total;
  };

  const estimatedTotal = calculateEstimate();
  const eventDuration = getDurationHours(formData.startTime, formData.endTime);

  const [status, setStatus] = useState<'idle' | 'submitting' | 'saving' | 'success' | 'error' | 'saved'>('idle');
  const [message, setMessage] = useState('');
  const [showConfirmation, setShowConfirmation] = useState(false);
  const [validationErrors, setValidationErrors] = useState<Record<string, string>>({});

  const validateForm = () => {
    const errors: Record<string, string> = {};

    // Required base fields
    if (!formData.name?.trim()) errors.name = "Full name is required";
    if (!formData.address?.trim()) errors.address = "Address is required";
    if (!formData.city?.trim()) errors.city = "City is required";
    if (!formData.zip?.trim()) errors.zip = "Zip code is required";
    if (!formData.phone?.trim()) errors.phone = "Phone number is required";
    if (!formData.email?.trim()) errors.email = "Email is required";
    if (!formData.eventType) errors.eventType = "Event type is required";
    if (!formData.guestCount?.trim()) errors.guestCount = "Guest count is required";

    // Timing
    if (!formData.startTime || formData.startTime.includes('Hour')) errors.startTime = "Start time is required";
    if (!formData.endTime || formData.endTime.includes('Hour')) errors.endTime = "End time is required";

    // Date of Birth / Age
    if (!formData.dob || formData.dob.includes('--')) {
      errors.dob = "Full date of birth is required";
    } else {
      const birthDate = new Date(formData.dob);
      const ageDifMs = Date.now() - birthDate.getTime();
      if (new Date(ageDifMs).getUTCFullYear() - 1970 < 21) {
        errors.dob = "Must be at least 21 years old";
      }
    }

    if (formData.eventType === 'Youth Organization Function' && !formData.youthFunctionType) {
      errors.youthFunctionType = "Youth function type required";
    }

    if (formData.eventType === 'Other' && !formData.otherEventType.trim()) {
      errors.otherEventType = "Description required";
    }

    // Policies
    const policies = [
      'policyAge', 'policyAlcohol', 'policyDecorations', 'policyCancellation', 'policyPayment', 'policyDamage',
      'policyFunctionTime', 'policyCommitteeApproval', 'policyKitchenRules', 'policySecurityDeposit'
    ];

    policies.forEach(p => {
      if (!formData[p as keyof typeof formData]) {
        errors[p] = "Agreement required";
      }
    });

    return errors;
  };

  const ValidationError = ({ message, className = "" }: { message?: string, className?: string }) => (
    <AnimatePresence>
      {message && (
        <motion.div
          initial={{ opacity: 0, x: -10 }}
          animate={{ opacity: 1, x: 0 }}
          exit={{ opacity: 0, x: -10 }}
          className={`absolute z-20 left-full ml-4 top-1/2 -translate-y-1/2 bg-red-600 text-white text-xs font-bold py-1.5 px-3 rounded shadow-xl whitespace-nowrap pointer-events-none before:content-[''] before:absolute before:right-full before:top-1/2 before:-translate-y-1/2 before:border-[6px] before:border-transparent before:border-r-red-600 ${className}`}
        >
          {message}
        </motion.div>
      )}
    </AnimatePresence>
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { id, value } = e.target;
    let newFormData;

    // Clear error for this field
    if (validationErrors[id]) {
      const newErrors = { ...validationErrors };
      delete newErrors[id];
      setValidationErrors(newErrors);
    }

    // Checkbox handling
    if ((e.target as HTMLInputElement).type === 'checkbox') {
      newFormData = { ...formData, [id]: (e.target as HTMLInputElement).checked };
    } else {
      newFormData = { ...formData, [id]: value };
    }

    setFormData(newFormData);

    // Auto-blur select elements after selection to prevent accidental scroll changes
    if (e.target.tagName === 'SELECT') {
      setTimeout(() => e.target.blur(), 100);
    }

    // Propagate changes for auto-save/draft
    if (onChange) onChange(newFormData);
  };

  // Prevent scroll wheel from changing select values when not actively focused
  const handleSelectWheel = (e: React.WheelEvent<HTMLSelectElement>) => {
    e.currentTarget.blur();
    e.preventDefault();
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // Clear global error
    setStatus('idle');
    setMessage('');

    const errors = validateForm();
    setValidationErrors(errors);

    if (Object.keys(errors).length > 0) {
      // Find first visible error and scroll to it
      const firstErrorId = Object.keys(errors)[0];
      const element = document.getElementById(firstErrorId);
      if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
        element.focus();
      }
      setStatus('error');
      setMessage('Please correct the highlighted fields and policies.');
      return;
    }

    // All validation passed - show confirmation modal
    setShowConfirmation(true);
  };

  // Actual submission function called from confirmation modal
  const handleFinalSubmit = async () => {
    setShowConfirmation(false);
    setStatus('submitting');
    setMessage('');

    try {
      // Determine final event type string
      let finalEventType = formData.eventType;
      if (formData.eventType === 'Youth Organization Function') {
        finalEventType = `Youth Organization Function: ${formData.youthFunctionType}`;
      } else if (formData.eventType === 'Other') {
        finalEventType = `Other: ${formData.otherEventType}`;
      }

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
          eventDate: selectedDate, // Ensure mapped to EventDate
          status: 'Pending',
          eventType: finalEventType,
          startTime: formData.startTime,
          endTime: formData.endTime,
          renterType: formData.memberStatus,
          memberStatus: formData.memberStatus === 'Member',
          kitchenUsage: formData.kitchenUse === 'Yes',
          avEquipmentUsage: formData.avEquipment === 'Yes',
          totalPrice: estimatedTotal,
          guestCount: parseInt(formData.guestCount) || 0,
          rulesAgreed: true
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
          <div className="relative">
            <label htmlFor="name" className="block text-sm font-medium mb-1">Full Name *</label>
            <input type="text" id="name" value={formData.name} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 focus:border-burnished-gold outline-none ${validationErrors.name ? 'border-red-500' : 'border-white/20'}`} />
            <ValidationError message={validationErrors.name} />
          </div>
          <div>
            <label htmlFor="memberStatus" className="block text-sm font-medium mb-1">Membership Status *</label>
            <select id="memberStatus" value={formData.memberStatus} onChange={handleChange} onWheel={handleSelectWheel} className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
              <option value="Non-Member">Non-Member</option>
              <option value="Member">Member</option>
              <option value="Non-Profit">Non-Profit Organization</option>
            </select>
          </div>
          {formData.memberStatus === 'Member' && (
            <div className="md:col-span-2 bg-burnished-gold/10 p-3 rounded">
              <p className="text-sm text-burnished-gold font-bold">Please note: Membership status will be verified against club records.</p>
            </div>
          )}
          <div className="relative">
            <label className="block text-sm font-medium mb-1">Date of Birth (Must be 21+) *</label>
            <div className={`flex gap-2 rounded-md ${validationErrors.dob ? 'border border-red-500 p-1' : ''}`}>
              <select
                id="dobMonth"
                value={formData.dob.split('-')[1] || ''}
                onChange={(e) => {
                  if (validationErrors.dob) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.dob;
                    setValidationErrors(newErrors);
                  }
                  const month = e.target.value;
                  const day = formData.dob.split('-')[2] || '';
                  const year = formData.dob.split('-')[0] || '';
                  const newDob = `${year}-${month}-${day}`;
                  setFormData(prev => {
                    const next = { ...prev, dob: newDob };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
                  if (validationErrors.dob) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.dob;
                    setValidationErrors(newErrors);
                  }
                  const month = formData.dob.split('-')[1] || '';
                  const day = e.target.value;
                  const year = formData.dob.split('-')[0] || '';
                  const newDob = `${year}-${month}-${day}`;
                  setFormData(prev => {
                    const next = { ...prev, dob: newDob };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
                  if (validationErrors.dob) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.dob;
                    setValidationErrors(newErrors);
                  }
                  const month = formData.dob.split('-')[1] || '01';
                  const day = formData.dob.split('-')[2] || '01';
                  const year = e.target.value;
                  const newDob = `${year}-${month}-${day}`;
                  setFormData(prev => {
                    const next = { ...prev, dob: newDob };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
            <ValidationError message={validationErrors.dob} />
            <p className="text-xs text-white/50 mt-1">You must be at least 21 years old to rent the hall</p>
          </div>
          <div className="md:col-span-2 relative">
            <label htmlFor="address" className="block text-sm font-medium mb-1">Street Address *</label>
            <input type="text" id="address" value={formData.address} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.address ? 'border-red-500' : 'border-white/20'}`} />
            <ValidationError message={validationErrors.address} />
          </div>
          <div className="md:col-span-2 grid grid-cols-2 gap-4">
            <div className="relative">
              <label htmlFor="city" className="block text-sm font-medium mb-1">City *</label>
              <input type="text" id="city" value={formData.city} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.city ? 'border-red-500' : 'border-white/20'}`} />
              <ValidationError message={validationErrors.city} />
            </div>
            <div className="relative">
              <label htmlFor="zip" className="block text-sm font-medium mb-1">Zip Code *</label>
              <input type="text" id="zip" value={formData.zip} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.zip ? 'border-red-500' : 'border-white/20'}`} />
              <ValidationError message={validationErrors.zip} />
            </div>
          </div>
          <div className="relative">
            <label htmlFor="phone" className="block text-sm font-medium mb-1">Phone Number *</label>
            <input type="tel" id="phone" value={formData.phone} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.phone ? 'border-red-500' : 'border-white/20'}`} />
            <ValidationError message={validationErrors.phone} />
          </div>
          <div className="relative">
            <label htmlFor="email" className="block text-sm font-medium mb-1">Email Address *</label>
            <input type="email" id="email" value={formData.email} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.email ? 'border-red-500' : 'border-white/20'}`} />
            <ValidationError message={validationErrors.email} />
          </div>
        </div>
      </div>

      {/* 2. EVENT DETAILS */}
      <div className="bg-white/5 p-6 rounded-xl border border-white/10">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 border-b border-burnished-gold/20 pb-2">2. Event Details</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="md:col-span-2 relative">
            <label htmlFor="eventType" className="block text-sm font-medium mb-1">Type of Function *</label>
            <select id="eventType" value={formData.eventType} onChange={handleChange} onWheel={handleSelectWheel} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 text-white outline-none ${validationErrors.eventType ? 'border-red-500' : 'border-white/20'}`}>
              <option value="" disabled>Select Function Type...</option>
              {EVENT_TYPES.map(type => (
                <option key={type} value={type}>{type}</option>
              ))}
            </select>
            <ValidationError message={validationErrors.eventType} />
            {formData.eventType === 'Youth Organization Function' && (
              <div className="mt-3 animate-fade-in relative">
                <label htmlFor="youthFunctionType" className="block text-sm font-medium mb-1 text-burnished-gold">Select Youth Function Type *</label>
                <select id="youthFunctionType" value={formData.youthFunctionType} onChange={handleChange} onWheel={handleSelectWheel} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 text-white outline-none ${validationErrors.youthFunctionType ? 'border-red-500' : 'border-burnished-gold'}`}>
                  <option value="" disabled>Select Youth Function...</option>
                  {YOUTH_FUNCTIONS.map(func => (
                    <option key={func} value={func}>{func}</option>
                  ))}
                </select>
                <ValidationError message={validationErrors.youthFunctionType} />
              </div>
            )}
            {formData.eventType === 'Other' && (
              <div className="mt-3 animate-fade-in relative">
                <label htmlFor="otherEventType" className="block text-sm font-medium mb-1 text-burnished-gold">Please describe the function *</label>
                <input type="text" id="otherEventType" value={formData.otherEventType} onChange={handleChange} required className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.otherEventType ? 'border-red-500' : 'border-burnished-gold'}`} placeholder="e.g. Retirement Party" />
                <ValidationError message={validationErrors.otherEventType} />
              </div>
            )}
          </div>

          <div className="relative">
            <label className="block text-sm font-medium mb-1">Start Time *</label>
            <div className={`flex gap-2 rounded-md ${validationErrors.startTime ? 'border border-red-500 p-1' : ''}`}>
              <select
                id="startHour"
                value={formData.startTime.split(':')[0] || ''}
                onChange={(e) => {
                  if (validationErrors.startTime) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.startTime;
                    setValidationErrors(newErrors);
                  }
                  const hour = e.target.value;
                  const minute = formData.startTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = formData.startTime.split(' ')[1] || 'PM';
                  const newTime = `${hour}:${minute} ${period}`;
                  setFormData(prev => {
                    const next = { ...prev, startTime: newTime };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
                  if (validationErrors.startTime) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.startTime;
                    setValidationErrors(newErrors);
                  }
                  const hour = formData.startTime.split(':')[0] || '1';
                  const minute = e.target.value;
                  const period = formData.startTime.split(' ')[1] || 'PM';
                  const newTime = `${hour}:${minute} ${period}`;
                  setFormData(prev => {
                    const next = { ...prev, startTime: newTime };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
                  if (validationErrors.startTime) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.startTime;
                    setValidationErrors(newErrors);
                  }
                  const hour = formData.startTime.split(':')[0] || '1';
                  const minute = formData.startTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = e.target.value;
                  const newTime = `${hour}:${minute} ${period}`;
                  setFormData(prev => {
                    const next = { ...prev, startTime: newTime };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="AM">AM</option>
                <option value="PM">PM</option>
              </select>
            </div>
            <ValidationError message={validationErrors.startTime} />
          </div>

          <div className="relative">
            <label className="block text-sm font-medium mb-1">End Time *</label>
            <div className={`flex gap-2 rounded-md ${validationErrors.endTime ? 'border border-red-500 p-1' : ''}`}>
              <select
                id="endHour"
                value={formData.endTime.split(':')[0] || ''}
                onChange={(e) => {
                  if (validationErrors.endTime) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.endTime;
                    setValidationErrors(newErrors);
                  }
                  const hour = e.target.value;
                  const minute = formData.endTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = formData.endTime.split(' ')[1] || 'PM';
                  const newTime = `${hour}:${minute} ${period}`;
                  setFormData(prev => {
                    const next = { ...prev, endTime: newTime };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
                  if (validationErrors.endTime) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.endTime;
                    setValidationErrors(newErrors);
                  }
                  const hour = formData.endTime.split(':')[0] || '1';
                  const minute = e.target.value;
                  const period = formData.endTime.split(' ')[1] || 'PM';
                  const newTime = `${hour}:${minute} ${period}`;
                  setFormData(prev => {
                    const next = { ...prev, endTime: newTime };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
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
                  if (validationErrors.endTime) {
                    const newErrors = { ...validationErrors };
                    delete newErrors.endTime;
                    setValidationErrors(newErrors);
                  }
                  const hour = formData.endTime.split(':')[0] || '1';
                  const minute = formData.endTime.split(':')[1]?.split(' ')[0] || '00';
                  const period = e.target.value;
                  const newTime = `${hour}:${minute} ${period}`;
                  setFormData(prev => {
                    const next = { ...prev, endTime: newTime };
                    if (onChange) onChange(next);
                    return next;
                  });
                  setTimeout(() => e.target.blur(), 100);
                }}
                onWheel={handleSelectWheel}
                required
                className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
              >
                <option value="AM">AM</option>
                <option value="PM">PM</option>
              </select>
            </div>
            <ValidationError message={validationErrors.endTime} />
          </div>

          <div className="relative">
            <label htmlFor="guestCount" className="block text-sm font-medium mb-1">Number of Guests (Max 200) *</label>
            <input type="text" id="guestCount" inputMode="numeric" pattern="[0-9]*" value={formData.guestCount} onChange={handleChange} required placeholder="Enter number of guests (max 200)" className={`w-full bg-midnight-blue border rounded-md px-3 py-2 outline-none ${validationErrors.guestCount ? 'border-red-500' : 'border-white/20'}`} />
            <ValidationError message={validationErrors.guestCount} />
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
          <select id="setupNeeded" value={formData.setupNeeded} onChange={handleChange} onWheel={handleSelectWheel} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
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
          <select id="barService" value={formData.barService} onChange={handleChange} onWheel={handleSelectWheel} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
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
          <select id="kitchenUse" value={formData.kitchenUse} onChange={handleChange} onWheel={handleSelectWheel} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
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

        {/* EQUIPMENT POLICY */}
        <div>
          <label htmlFor="avEquipment" className="block text-lg font-bold mb-2">Special Equipment (A/V, Microphone, Projector)?</label>
          <div className="bg-black/20 p-4 rounded mb-2 text-gray-300 text-sm leading-relaxed border-l-2 border-burnished-gold">
            Check this if you require use of the club&apos;s sound system, microphone, or projection equipment.
            {pricing && <span className="text-burnished-gold ml-1">Fee: ${pricing.avEquipmentFee}</span>}
          </div>
          <select id="avEquipment" value={formData.avEquipment} onChange={handleChange} onWheel={handleSelectWheel} required className="w-full md:w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 text-white outline-none">
            <option value="No">No</option>
            <option value="Yes">Yes</option>
          </select>
        </div>

        <div>
          <label htmlFor="details" className="block text-sm font-medium mb-1">Additional Notes / Special Requests</label>
          <textarea id="details" rows={3} value={formData.details} onChange={handleChange} className="w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none"></textarea>
        </div>
      </div>

      {/* PRICING SUMMARY */}
      <div className="bg-burnished-gold/10 p-6 rounded-xl border border-burnished-gold/30 shadow-inner">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 uppercase tracking-widest">Rental Estimate</h3>
        <div className="space-y-4">
          <div className="flex justify-between items-center text-lg">
            <span>Base Rate ({formData.memberStatus})</span>
            <span className="font-mono text-xl">
              ${formData.memberStatus === 'Member' ? pricing?.memberRate || 250 : formData.memberStatus === 'Non-Profit' ? pricing?.nonProfitRate || 350 : pricing?.nonMemberRate || 500}
            </span>
          </div>

          {eventDuration > 5 && (
            <div className="flex justify-between items-center text-gray-300">
              <div className="flex flex-col">
                <span>Extra Time ({Math.ceil(eventDuration - 5)} hours)</span>
                <span className="text-[10px] opacity-50">First 5 hours included</span>
              </div>
              <span className="font-mono">+${Math.ceil(eventDuration - 5) * 50}</span>
            </div>
          )}

          {formData.kitchenUse === 'Yes' && (
            <div className="flex justify-between items-center text-gray-300">
              <span>Kitchen Usage Fee</span>
              <span className="font-mono">+${pricing?.kitchenFee || 50}</span>
            </div>
          )}

          {formData.avEquipment === 'Yes' && (
            <div className="flex justify-between items-center text-gray-300">
              <span>Special Equipment Fee</span>
              <span className="font-mono">+${pricing?.avEquipmentFee || 25}</span>
            </div>
          )}

          <div className="border-t border-burnished-gold/30 pt-4 flex justify-between items-center">
            <div className="text-lg font-bold text-burnished-gold">Estimated Total</div>
            <div className="text-3xl font-display font-bold text-burnished-gold">${estimatedTotal}</div>
          </div>

          <div className="bg-black/40 p-3 rounded text-xs text-white/60 text-center italic">
            * Final price will be confirmed by the Hall Manager upon review.
            {pricing && <span> Includes ${pricing.securityDepositAmount} refundable security deposit.</span>}
          </div>
        </div>
      </div>

      {/* IMPORTANT RENTAL POLICIES */}
      <div className="bg-midnight-blue/60 p-6 rounded-xl border-2 border-burnished-gold/40 space-y-4">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 flex items-center gap-2">
          <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
          </svg>
          Important Rental Policies
        </h3>

        <div className="space-y-3 text-sm">
          <div className="flex items-start gap-3 bg-black/30 p-3 rounded relative">
            <input type="checkbox" id="policyFunctionTime" checked={formData.policyFunctionTime} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyFunctionTime ? 'ring-2 ring-red-500' : ''}`} />
            <label htmlFor="policyFunctionTime" className="cursor-pointer">
              I understand that the rental includes <strong className="text-burnished-gold">up to 5 hours of function time</strong>. Additional hours are available at <strong className="text-burnished-gold">$50 per hour</strong>. <span className="text-red-400 font-bold">Setup and cleanup time are NOT included</span> in the 5-hour function time.
            </label>
            <ValidationError message={validationErrors.policyFunctionTime} className="!-top-2 !left-auto !right-0 translate-x-0" />
          </div>

          <div className="flex items-start gap-3 bg-black/30 p-3 rounded relative">
            <input type="checkbox" id="policyCommitteeApproval" checked={formData.policyCommitteeApproval} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyCommitteeApproval ? 'ring-2 ring-red-500' : ''}`} />
            <label htmlFor="policyCommitteeApproval" className="cursor-pointer">
              I understand that all rental requests are subject to <strong className="text-burnished-gold">Hall Rental Committee approval</strong>. The committee has the authority to set attendance limits and may require police presence for certain events (cost added to rental fee).
            </label>
            <ValidationError message={validationErrors.policyCommitteeApproval} className="!-top-2 !left-auto !right-0 translate-x-0" />
          </div>

          <div className="flex items-start gap-3 bg-black/30 p-3 rounded relative">
            <input type="checkbox" id="policyKitchenRules" checked={formData.policyKitchenRules} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyKitchenRules ? 'ring-2 ring-red-500' : ''}`} />
            <label htmlFor="policyKitchenRules" className="cursor-pointer">
              I understand that <strong className="text-burnished-gold">NO CLUB UTENSILS</strong> are provided. If using a caterer for cooking, <strong className="text-red-400">proof of liability insurance</strong> must be on file. The <strong className="text-red-400">lower ovens on the gas stove are NOT to be used</strong> under any circumstances.
            </label>
            <ValidationError message={validationErrors.policyKitchenRules} className="!-top-2 !left-auto !right-0 translate-x-0" />
          </div>

          <div className="flex items-start gap-3 bg-black/30 p-3 rounded relative">
            <input type="checkbox" id="policySecurityDeposit" checked={formData.policySecurityDeposit} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policySecurityDeposit ? 'ring-2 ring-red-500' : ''}`} />
            <label htmlFor="policySecurityDeposit" className="cursor-pointer">
              I understand that a <strong className="text-burnished-gold">refundable security deposit</strong> is required. Any additional janitorial effort or damages will be deducted from this deposit. The hall must be returned to its original clean and empty condition.
            </label>
            <ValidationError message={validationErrors.policySecurityDeposit} className="!-top-2 !left-auto !right-0 translate-x-0" />
          </div>

          <div className="mt-4 p-3 bg-burnished-gold/10 rounded border border-burnished-gold/30 text-center">
            <a href="/hall-rental-policy" target="_blank" className="text-burnished-gold hover:text-yellow-400 font-bold underline">
              📄 View Complete Hall Rental Policy
            </a>
          </div>
        </div>
      </div>

      {/* 4. MANDATORY CONTRACT */}
      <div className="bg-black/40 p-6 rounded-xl border border-white/10 space-y-4">
        <h3 className="text-xl font-display font-bold text-red-400 mb-2">Usage Agreement</h3>

        <div className="flex items-start gap-3 relative">
          <input type="checkbox" id="policyAge" checked={formData.policyAge} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyAge ? 'ring-2 ring-red-500' : ''}`} />
          <label htmlFor="policyAge" className="text-sm cursor-pointer">I attest that I am at least <strong>21 years of age</strong>.</label>
          <ValidationError message={validationErrors.policyAge} className="!-top-4 !left-auto !right-0 translate-x-0" />
        </div>

        <div className="flex items-start gap-3 relative">
          <input type="checkbox" id="policyAlcohol" checked={formData.policyAlcohol} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyAlcohol ? 'ring-2 ring-red-500' : ''}`} />
          <label htmlFor="policyAlcohol" className="text-sm cursor-pointer">I understand that <strong>NO OUTSIDE ALCOHOL</strong> is permitted on the premises. Violation results in immediate termination of the event.</label>
          <ValidationError message={validationErrors.policyAlcohol} className="!-top-4 !left-auto !right-0 translate-x-0" />
        </div>

        <div className="flex items-start gap-3 relative">
          <input type="checkbox" id="policyDecorations" checked={formData.policyDecorations} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyDecorations ? 'ring-2 ring-red-500' : ''}`} />
          <label htmlFor="policyDecorations" className="text-sm cursor-pointer">I agree NOT to use scotch tape, tacks, or nails on any walls, ceilings, or equipment.</label>
          <ValidationError message={validationErrors.policyDecorations} className="!-top-4 !left-auto !right-0 translate-x-0" />
        </div>

        <div className="flex items-start gap-3 relative">
          <input type="checkbox" id="policyCancellation" checked={formData.policyCancellation} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyCancellation ? 'ring-2 ring-red-500' : ''}`} />
          <label htmlFor="policyCancellation" className="text-sm cursor-pointer">I understand that cancellations made less than <strong>30 days</strong> prior to the date will forfeit the rental fee.</label>
          <ValidationError message={validationErrors.policyCancellation} className="!-top-4 !left-auto !right-0 translate-x-0" />
        </div>

        <div className="flex items-start gap-3 relative">
          <input type="checkbox" id="policyPayment" checked={formData.policyPayment} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyPayment ? 'ring-2 ring-red-500' : ''}`} />
          <label htmlFor="policyPayment" className="text-sm cursor-pointer">I agree to pay the Hall Rental Fee in full within <strong>2 business days</strong> of approval to reserve the date.</label>
          <ValidationError message={validationErrors.policyPayment} className="!-top-4 !left-auto !right-0 translate-x-0" />
        </div>

        <div className="flex items-start gap-3 relative">
          <input type="checkbox" id="policyDamage" checked={formData.policyDamage} onChange={handleChange} required className={`mt-1 w-5 h-5 accent-burnished-gold ${validationErrors.policyDamage ? 'ring-2 ring-red-500' : ''}`} />
          <label htmlFor="policyDamage" className="text-sm cursor-pointer">I accept full responsibility for any damage to the building, equipment, or fixtures caused by my guests.</label>
          <ValidationError message={validationErrors.policyDamage} className="!-top-4 !left-auto !right-0 translate-x-0" />
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

      {/* CONFIRMATION MODAL - Review Before Submission */}
      {showConfirmation && (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          exit={{ opacity: 0 }}
          className="fixed inset-0 z-50 flex items-center justify-center p-4"
          style={{ backdropFilter: 'blur(10px)', background: 'rgba(0,0,0,0.8)' }}
        >
          <motion.div
            initial={{ scale: 0.5, rotateX: -15, opacity: 0 }}
            animate={{ scale: 1, rotateX: 0, opacity: 1 }}
            exit={{ scale: 0.8, opacity: 0 }}
            transition={{ type: 'spring', damping: 20, stiffness: 300 }}
            className="bg-gradient-to-br from-midnight-blue via-deep-navy to-midnight-blue border-2 border-burnished-gold/40 rounded-2xl p-8 max-w-3xl w-full max-h-[90vh] overflow-y-auto shadow-2xl shadow-burnished-gold/20 relative"
          >
            {/* Animated particles */}
            <div className="absolute inset-0 overflow-hidden rounded-2xl pointer-events-none">
              {[...Array(20)].map((_, i) => (
                <motion.div
                  key={i}
                  className="absolute w-1 h-1 bg-burnished-gold rounded-full"
                  initial={{ x: Math.random() * 100 + '%', y: '-10%', opacity: 0 }}
                  animate={{
                    y: '110%',
                    opacity: [0, 1, 0],
                    scale: [0, 1.5, 0]
                  }}
                  transition={{
                    duration: 3 + Math.random() * 2,
                    repeat: Infinity,
                    delay: Math.random() * 2
                  }}
                />
              ))}
            </div>

            <div className="relative z-10">
              <motion.div
                initial={{ y: -20, opacity: 0 }}
                animate={{ y: 0, opacity: 1 }}
                transition={{ delay: 0.2 }}
                className="text-center mb-6"
              >
                <motion.div
                  animate={{ rotate: [0, 360] }}
                  transition={{ duration: 20, repeat: Infinity, ease: 'linear' }}
                  className="w-16 h-16 mx-auto mb-4 rounded-full bg-gradient-to-r from-burnished-gold to-yellow-600 flex items-center justify-center"
                >
                  <svg className="w-8 h-8 text-midnight-blue" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                  </svg>
                </motion.div>
                <h2 className="text-3xl font-display font-bold text-burnished-gold mb-2">Review Your Application</h2>
                <p className="text-pure-white/70">Please confirm all details are correct before submitting</p>
              </motion.div>

              <motion.div
                initial={{ y: 20, opacity: 0 }}
                animate={{ y: 0, opacity: 1 }}
                transition={{ delay: 0.3 }}
                className="space-y-4 bg-black/30 p-6 rounded-xl border border-white/10"
              >
                <div className="grid md:grid-cols-2 gap-4 text-sm">
                  <div><span className="text-pure-white/60">Name:</span> <span className="text-pure-white font-semibold">{formData.name}</span></div>
                  <div><span className="text-pure-white/60">Membership:</span> <span className="text-burnished-gold font-semibold">{formData.memberStatus}</span></div>
                  {formData.memberStatus === 'Member' && formData.sponsoringMember && (
                    <div className="md:col-span-2"><span className="text-pure-white/60">Sponsoring Member:</span> <span className="text-pure-white">{formData.sponsoringMember}</span></div>
                  )}
                  <div><span className="text-pure-white/60">Email:</span> <span className="text-pure-white">{formData.email}</span></div>
                  <div><span className="text-pure-white/60">Phone:</span> <span className="text-pure-white">{formData.phone}</span></div>
                  <div className="md:col-span-2"><span className="text-pure-white/60">Address:</span> <span className="text-pure-white">{formData.address}, {formData.city} {formData.zip}</span></div>

                  <div className="md:col-span-2 h-px bg-white/10 my-1"></div>

                  <div><span className="text-pure-white/60">Function Type:</span> <span className="text-burnished-gold font-semibold">{formData.eventType === 'Other' ? formData.otherEventType : formData.eventType}</span></div>
                  <div><span className="text-pure-white/60">Event Date:</span> <span className="text-pure-white font-semibold">{selectedDate?.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}</span></div>
                  <div><span className="text-pure-white/60">Function Times:</span> <span className="text-pure-white font-semibold">{formData.startTime} - {formData.endTime}</span></div>
                  <div><span className="text-pure-white/60">Duration:</span> <span className="text-burnished-gold font-bold">{eventDuration.toFixed(1)} hours</span></div>
                  <div><span className="text-pure-white/60">Guests:</span> <span className="text-pure-white font-semibold">{formData.guestCount}</span></div>

                  <div className="md:col-span-2 h-px bg-white/10 my-1"></div>

                  <div><span className="text-pure-white/60">Setup Time:</span> <span className="text-pure-white">{formData.setupNeeded}</span></div>
                  <div><span className="text-pure-white/60">Bar Service:</span> <span className="text-pure-white">{formData.barService}</span></div>
                  <div><span className="text-pure-white/60">Kitchen Access:</span> <span className="text-pure-white">{formData.kitchenUse}</span></div>
                  <div><span className="text-pure-white/60">A/V Equipment:</span> <span className="text-pure-white">{formData.avEquipment}</span></div>
                  {formData.kitchenUse === 'Yes' && formData.catererName && (
                    <div className="md:col-span-2"><span className="text-pure-white/60">Caterer:</span> <span className="text-pure-white">{formData.catererName}</span></div>
                  )}

                  {formData.details && (
                    <div className="md:col-span-2 mt-2">
                      <span className="text-pure-white/60 block mb-1">Additional Notes:</span>
                      <div className="bg-white/5 p-3 rounded text-pure-white/80 lowercase italic leading-relaxed border border-white/5">
                        {formData.details}
                      </div>
                    </div>
                  )}
                </div>

                <div className="pt-4 border-t border-white/10">
                  <div className="text-center">
                    <span className="text-pure-white/60">Estimated Total:</span>
                    <div className="text-3xl font-bold text-burnished-gold mt-1">${estimatedTotal}</div>
                    <p className="text-xs text-pure-white/50 mt-1">+ ${pricing?.securityDepositAmount} refundable security deposit</p>
                  </div>
                </div>
              </motion.div>

              <motion.div
                initial={{ y: 20, opacity: 0 }}
                animate={{ y: 0, opacity: 1 }}
                transition={{ delay: 0.4 }}
                className="flex gap-4 mt-6"
              >
                <motion.button
                  whileHover={{ scale: 1.05, boxShadow: '0 0 30px rgba(212, 175, 55, 0.4)' }}
                  whileTap={{ scale: 0.95 }}
                  onClick={handleFinalSubmit}
                  className="flex-1 bg-gradient-to-r from-burnished-gold to-yellow-600 text-midnight-blue font-bold px-8 py-4 rounded-lg shadow-lg transition-all text-lg"
                >
                  ✓ Submit Application
                </motion.button>
                <motion.button
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                  onClick={() => setShowConfirmation(false)}
                  className="flex-1 bg-white/10 hover:bg-white/20 text-pure-white font-bold px-8 py-4 rounded-lg border border-white/20 transition-all text-lg"
                >
                  ← Return to Application
                </motion.button>
              </motion.div>
            </div>
          </motion.div>
        </motion.div>
      )}
    </form>
  );
}
