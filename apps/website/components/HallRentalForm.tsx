// [MODIFIED]
'use client';

import { useState, FormEvent, useEffect } from 'react';
import { motion } from 'framer-motion';
import { useForm, Controller } from 'react-hook-form';
import { DateSplitInput } from './DateSplitInput';
import { TimeSplitInput } from './TimeSplitInput';

interface HallRentalFormProps {
  selectedDate: Date;
  onSuccess: (data: any) => void;
  initialData?: any;
  onChange?: (data: any) => void;
}

interface PricingSettings {
  memberRate: number;
  nonMemberRate: number;
  nonProfitRate: number;
  kitchenFee: number;
  avEquipmentFee: number;
  securityDepositAmount: number;
}

const renderField = (field, control, formData) => {
  const commonProps = {
    id: field.name,
    className: "w-full bg-midnight-blue border border-white/20 rounded-md px-3 py-2 focus:border-burnished-gold outline-none",
    ...field.validation
  };

  if (field.conditional && formData[field.conditional.field] !== field.conditional.value) {
    return null;
  }

  return (
    <div key={field.name} className={`md:col-span-${field.gridCols || 2}`}>
      <label htmlFor={field.name} className="block text-sm font-medium mb-1">{field.label} {field.required && '*'}</label>
      {field.description && <p className="text-xs text-white/50 mb-1">{field.description}</p>}
      <Controller
        name={field.name}
        control={control}
        defaultValue={field.defaultValue || ""}
        rules={{ required: field.required }}
        render={({ field: controllerField }) => {
          switch (field.type) {
            case 'text':
            case 'email':
            case 'tel':
            case 'number':
              return <input type={field.type} {...controllerField} {...commonProps} placeholder={field.placeholder} />;
            case 'textarea':
              return <textarea {...controllerField} {...commonProps} rows={field.rows || 3}></textarea>;
            case 'select':
              return (
                <select {...controllerField} {...commonProps}>
                  {field.options.map(option => <option key={option} value={option}>{option}</option>)}
                </select>
              );
            case 'checkbox':
              return (
                <div className="flex items-start gap-3">
                  <input type="checkbox" {...controllerField} className="mt-1 w-5 h-5 accent-burnished-gold" />
                  <label htmlFor={field.name} className="cursor-pointer text-sm">{field.label}</label>
                </div>
              );
            case 'date-split':
                return <DateSplitInput control={control} name={field.name} required={field.required} />;
            case 'time-split':
                return <TimeSplitInput control={control} name={field.name} required={field.required} />;
            default:
              return null;
          }
        }}
      />
    </div>
  );
};


export default function HallRentalForm({ selectedDate, onSuccess, initialData, onChange }: HallRentalFormProps) {
  const { control, handleSubmit, watch, formState: { errors } } = useForm({
    defaultValues: initialData || {}
  });
  const formData = watch();
  const [schema, setSchema] = useState(null);
  const [pricing, setPricing] = useState<PricingSettings | null>(null);
  const [status, setStatus] = useState<'idle' | 'submitting' | 'error'>('idle');
  const [message, setMessage] = useState('');
  const [showConfirmation, setShowConfirmation] = useState(false);

  useEffect(() => {
    const fetchSchema = async () => {
        try {
            const res = await fetch('/api/forms/hall-rental');
            if (!res.ok) {
                throw new Error('Failed to load form.');
            }
            const data = await res.json();
            setSchema(data.schemaJson ? JSON.parse(data.schemaJson) : data);
        } catch (error) {
            setStatus('error');
            setMessage(error.message);
        }
    };

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
    fetchSchema();
    fetchPricing();
  }, []);

  const calculateEstimate = () => {
    if (!pricing) return 0;

    let total = 0;

    if (formData.memberStatus === 'Member') total += pricing.memberRate;
    else if (formData.memberStatus === 'Non-Profit Organization') total += pricing.nonProfitRate;
    else total += pricing.nonMemberRate;

    if (formData.kitchenUse === 'Yes') total += pricing.kitchenFee;
    if (formData.avEquipment === 'Yes') total += pricing.avEquipmentFee;

    return total;
  };

  const estimatedTotal = calculateEstimate();

  const onSubmit = (data: any) => {
    setShowConfirmation(true);
  };

  const handleFinalSubmit = async () => {
    setShowConfirmation(false);
    setStatus('submitting');
    setMessage('');

    try {
        const data = formData;
      const finalEventType = data.eventType === 'Other' ? `Other: ${data.otherEventType}` : data.eventType;

      const res = await fetch('/api/HallRentalInquiry', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          ...data,
          eventDate: selectedDate,
          status: 'Pending',
          eventType: finalEventType,
          totalPrice: estimatedTotal
        }),
      });

      if (!res.ok) {
        const text = await res.text();
        let errorMsg = 'Something went wrong';
        try {
          const errorData = JSON.parse(text);
          errorMsg = errorData.message || errorMsg;
        } catch {
          errorMsg = `Server Error: ${text.substring(0, 200)}`;
        }
        throw new Error(errorMsg);
      }

      setStatus('idle');
      onSuccess(data);

    } catch (err) {
      setStatus('error');
      setMessage(err instanceof Error ? err.message : 'An unknown error occurred');
    }
  };

  if (!schema) {
    return <div className="text-center p-8">Loading form...</div>;
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-8 text-pure-white max-w-4xl mx-auto">
      {schema.fields.map(section => (
        <div key={section.section} className="bg-white/5 p-6 rounded-xl border border-white/10">
          <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 border-b border-burnished-gold/20 pb-2">{section.section}</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {section.fields.map(field => renderField(field, control, formData))}
          </div>
        </div>
      ))}

      <div className="bg-burnished-gold/10 p-6 rounded-xl border border-burnished-gold/30 shadow-inner">
        <h3 className="text-xl font-display font-bold text-burnished-gold mb-4 uppercase tracking-widest">Rental Estimate</h3>
        <div className="space-y-4">
          <div className="flex justify-between items-center text-lg">
            <span>Base Rate ({formData.memberStatus})</span>
            <span className="font-mono text-xl">
              ${formData.memberStatus === 'Member' ? pricing?.memberRate : formData.memberStatus === 'Non-Profit' ? pricing?.nonProfitRate : pricing?.nonMemberRate}
            </span>
          </div>

          {formData.kitchenUse === 'Yes' && (
            <div className="flex justify-between items-center text-gray-300">
              <span>Kitchen Usage Fee</span>
              <span className="font-mono">+${pricing?.kitchenFee}</span>
            </div>
          )}

          {formData.avEquipment === 'Yes' && (
            <div className="flex justify-between items-center text-gray-300">
              <span>Special Equipment Fee</span>
              <span className="font-mono">+${pricing?.avEquipmentFee}</span>
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
            <div className="relative z-10">
              <h2 className="text-3xl font-display font-bold text-burnished-gold mb-2 text-center">Review Your Application</h2>
              <p className="text-pure-white/70 text-center mb-6">Please confirm all details are correct before submitting</p>

              <div className="space-y-4 bg-black/30 p-6 rounded-xl border border-white/10">
                {Object.entries(formData).map(([key, value]) => (
                  <div key={key}><span className="text-pure-white/60">{key}:</span> <span className="text-pure-white font-semibold">{String(value)}</span></div>
                ))}
                <div className="pt-4 border-t border-white/10">
                  <div className="text-center">
                    <span className="text-pure-white/60">Estimated Total:</span>
                    <div className="text-3xl font-bold text-burnished-gold mt-1">${estimatedTotal}</div>
                  </div>
                </div>
              </div>

              <div className="flex gap-4 mt-6">
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
              </div>
            </div>
          </motion.div>
        </motion.div>
      )}
    </form>
  );
}
