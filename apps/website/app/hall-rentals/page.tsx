// [MODIFIED]
'use client';
import { useState } from 'react';
import FeatureGrid from '../../components/FeatureGrid';
import RentalAvailabilityCalendar from '../../components/RentalAvailabilityCalendar';
import HallRentalForm from '../../components/HallRentalForm';
import ApplicationSuccess from '../../components/ApplicationSuccess';

const HallRentalPage = () => {
  const [step, setStep] = useState(1);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);
  const [formData, setFormData] = useState({ name: '', email: '', phone: '', details: '' });

  const handleDateSelect = (date: Date) => {
    setSelectedDate(date);
    setStep(3);
  };

  const handleSuccess = (data: any) => {
    setFormData(data);
    setIsSuccess(true);
  };

  if (isSuccess) {
    return <ApplicationSuccess formData={formData} selectedDate={selectedDate!} />;
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
        <div className="space-y-8">
          <div className="bg-white/5 p-8 rounded-xl border border-white/10 text-center">
            <h2 className="text-4xl font-display text-burnished-gold mb-6">Our Historic Venue</h2>
            <p className="text-xl text-pure-white/80 max-w-2xl mx-auto mb-8">
              Host your event in true Gloucester style. Our hall features a full-service bar,
              commercial kitchen, and capacity for up to 200 guests.
              Perfect for weddings, fundraisers, and family reunions.
            </p>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 text-left mb-8">
              <div className="bg-black/20 p-6 rounded-lg">
                <h3 className="text-burnished-gold font-bold mb-2">Capacity</h3>
                <p className="text-white">Up to 200 Guests</p>
              </div>
              <div className="bg-black/20 p-6 rounded-lg">
                <h3 className="text-burnished-gold font-bold mb-2">Amenities</h3>
                <p className="text-white">Full Bar & Kitchen</p>
              </div>
              <div className="bg-black/20 p-6 rounded-lg">
                <h3 className="text-burnished-gold font-bold mb-2">Parking</h3>
                <p className="text-white">Private Lot Available</p>
              </div>
            </div>
          </div>

          <div className="flex justify-center">
            <button
              onClick={() => setStep(2)}
              className="bg-burnished-gold text-midnight-blue font-bold text-xl px-12 py-4 rounded-full hover:bg-burnished-gold/90 transition-all shadow-lg hover:shadow-burnished-gold/20 transform hover:-translate-y-1"
            >
              Check Availability &rarr;
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
