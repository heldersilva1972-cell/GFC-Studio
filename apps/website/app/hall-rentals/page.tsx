// [NEW]
'use client';
import { useState } from 'react';
import FeatureGrid from '../../components/FeatureGrid';
import RentalAvailabilityCalendar from '../../components/RentalAvailabilityCalendar';
import HallRentalForm from '../../components/HallRentalForm';

const HallRentalPage = () => {
  const [step, setStep] = useState(1);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);

  const handleDateSelect = (date: Date) => {
    setSelectedDate(date);
    setStep(3);
  };

  const handleSuccess = () => {
    setIsSuccess(true);
  };

  if (isSuccess) {
    return (
        <div className="text-center p-8 bg-green-500/10 border border-green-500 rounded-lg">
            <h3 className="font-display text-2xl font-bold text-green-400">Thank You!</h3>
            <p className="mt-2 text-green-300">Your request has been submitted successfully.</p>
        </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Progress Bar */}
      <div className="w-full mb-8">
          <div className="flex items-center justify-center">
              <div className={`w-1/3 text-center ${step >= 1 ? 'text-burnished-gold' : 'text-pure-white/50'}`}>
                  <div className="font-bold">Step 1</div>
                  <div className="text-sm">Gallery</div>
              </div>
              <div className={`w-1/3 text-center ${step >= 2 ? 'text-burnished-gold' : 'text-pure-white/50'}`}>
                  <div className="font-bold">Step 2</div>
                  <div className="text-sm">Calendar</div>
              </div>
              <div className={`w-1/3 text-center ${step >= 3 ? 'text-burnished-gold' : 'text-pure-white/50'}`}>
                  <div className="font-bold">Step 3</div>
                  <div className="text-sm">Form</div>
              </div>
          </div>
          <div className="w-full bg-pure-white/20 rounded-full h-1 mt-2">
              <div
                  className="bg-burnished-gold h-1 rounded-full"
                  style={{ width: `${((step - 1) / 2) * 100}%` }}
              ></div>
          </div>
      </div>


      {step === 1 && (
        <div>
          <FeatureGrid />
          <div className="flex justify-end mt-4">
            <button
              onClick={() => setStep(2)}
              className="bg-burnished-gold text-midnight-blue font-bold px-6 py-3 rounded-md hover:bg-burnished-gold/90 transition-colors"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {step === 2 && (
        <div>
          <RentalAvailabilityCalendar onDateSelect={handleDateSelect} />
          <div className="flex justify-between mt-4">
            <button
              onClick={() => setStep(1)}
              className="bg-pure-white/20 text-pure-white font-bold px-6 py-3 rounded-md hover:bg-pure-white/30 transition-colors"
            >
              Back
            </button>
          </div>
        </div>
      )}

      {step === 3 && (
        <div>
          <h2 className="text-2xl font-bold text-center mb-4">Application Form for {selectedDate?.toLocaleDateString()}</h2>
          <HallRentalForm selectedDate={selectedDate!} onSuccess={handleSuccess} />
          <div className="flex justify-between mt-4">
            <button
              onClick={() => setStep(2)}
              className="bg-pure-white/20 text-pure-white font-bold px-6 py-3 rounded-md hover:bg-pure-white/30 transition-colors"
            >
              Back
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default HallRentalPage;
