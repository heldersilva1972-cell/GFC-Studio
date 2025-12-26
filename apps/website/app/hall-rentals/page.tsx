// [MODIFIED]
'use client';
import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import FeatureGrid from '../../components/FeatureGrid';
import RentalAvailabilityCalendar from '../../components/RentalAvailabilityCalendar';
import HallRentalForm from '../../components/HallRentalForm';

const variants = {
  enter: (direction: number) => ({
    x: direction > 0 ? 1000 : -1000,
    opacity: 0,
    scale: 0.95,
  }),
  center: {
    zIndex: 1,
    x: 0,
    opacity: 1,
    scale: 1,
  },
  exit: (direction: number) => ({
    zIndex: 0,
    x: direction < 0 ? 1000 : -1000,
    opacity: 0,
    scale: 0.95,
  }),
};

const DRAFT_KEY = 'gfc_rental_draft';

const HallRentalPage = () => {
  const [step, setStep] = useState(1);
  const [direction, setDirection] = useState(0);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [newlyBookedDate, setNewlyBookedDate] = useState<Date | null>(null);
  // Replaced toast with modal for clearer confirmation
  const [showSuccessModal, setShowSuccessModal] = useState(false);

  // Draft State
  const [showDraftPrompt, setShowDraftPrompt] = useState(false);
  const [showSavePrompt, setShowSavePrompt] = useState(false);
  const [draftData, setDraftData] = useState<any>(null);
  const [formData, setFormData] = useState<any>(null); // To pass into form if resuming
  const [pendingNavigation, setPendingNavigation] = useState<number | null>(null);

  useEffect(() => {
    // Check for draft on mount
    const saved = localStorage.getItem(DRAFT_KEY);
    if (saved) {
      try {
        const parsed = JSON.parse(saved);
        setDraftData(parsed);
      } catch (e) { console.error("Draft parse error", e); }
    }
  }, []);

  const navigate = (newStep: number) => {
    // If going BACK from Form (Step 3), prompt to save
    if (step === 3 && newStep < 3) {
      setPendingNavigation(newStep);
      setShowSavePrompt(true);
      return;
    }
    setDirection(newStep > step ? 1 : -1);
    setStep(newStep);
  };

  const forceNavigate = (newStep: number) => {
    setDirection(newStep > step ? 1 : -1);
    setStep(newStep);
    setShowSavePrompt(false);
    setPendingNavigation(null);
  };

  const handleDateSelect = (date: Date) => {
    setSelectedDate(date);
    // If we have a draft, prompt user how they want to proceed when entering form
    if (draftData) {
      setShowDraftPrompt(true); // Don't navigate yet, show overlay logic in render
    } else {
      navigate(3);
    }
  };

  const handleDraftChoice = (choice: 'resume_original' | 'use_new_date' | 'discard') => {
    if (choice === 'resume_original') {
      // Use saved date
      if (draftData.selectedDate) setSelectedDate(new Date(draftData.selectedDate));
      setFormData(draftData.formData);
    } else if (choice === 'use_new_date') {
      // Use NEW date (already set in selectedDate), but load form data
      setFormData(draftData.formData);
    } else {
      // Discard
      localStorage.removeItem(DRAFT_KEY);
      setDraftData(null);
      setFormData(null);
    }
    setShowDraftPrompt(false);
    navigate(3);
  };

  const handleSaveAndExit = () => {
    // Logic assumes Form child pushes to LS. We simply proceed.
    setShowSavePrompt(false);
    forceNavigate(pendingNavigation ?? 2);
  };

  const handleDiscardAndExit = () => {
    localStorage.removeItem(DRAFT_KEY);
    setDraftData(null);
    setShowSavePrompt(false);
    forceNavigate(pendingNavigation ?? 2);
  };

  const handleSuccess = (data: any) => {
    // Clear draft on success
    localStorage.removeItem(DRAFT_KEY);
    setDraftData(null);
    setFormData(null);

    setNewlyBookedDate(selectedDate);
    setShowSuccessModal(true);
    // We stay on Step 3 (or effectively overlay) until they click "Continue"
  };

  const handleCloseSuccess = () => {
    setShowSuccessModal(false);
    forceNavigate(2); // Go to calendar directly, bypassing "Save Draft" prompt
  };

  // Helper... (unchanged)
  const onFormChange = (data: any) => {
    const payload = {
      formData: data,
      selectedDate: selectedDate,
      lastUpdated: new Date().toISOString()
    };
    localStorage.setItem(DRAFT_KEY, JSON.stringify(payload));
  };


  return (
    <div className="container mx-auto px-4 py-8 relative overflow-hidden min-h-screen">

      {/* Progress Bar (Static) */}
      <div className="w-full mb-8 relative z-10">
        <div className="flex items-center justify-center">
          <div className={`w-1/3 text-center transition-colors duration-500 ${step >= 1 ? 'text-burnished-gold' : 'text-pure-white/50'}`}>
            <div className="font-bold">Step 1</div>
            <div className="text-sm">Gallery</div>
          </div>
          <div className={`w-1/3 text-center transition-colors duration-500 ${step >= 2 ? 'text-burnished-gold' : 'text-pure-white/50'}`}>
            <div className="font-bold">Step 2</div>
            <div className="text-sm">Calendar</div>
          </div>
          <div className={`w-1/3 text-center transition-colors duration-500 ${step >= 3 ? 'text-burnished-gold' : 'text-pure-white/50'}`}>
            <div className="font-bold">Step 3</div>
            <div className="text-sm">Form</div>
          </div>
        </div>
        <div className="w-full bg-pure-white/10 rounded-full h-1 mt-2 overflow-hidden">
          <motion.div
            className="bg-burnished-gold h-1 rounded-full"
            initial={{ width: 0 }}
            animate={{ width: `${((step - 1) / 2) * 100}%` }}
            transition={{ duration: 0.5, ease: "easeInOut" }}
          />
        </div>
      </div>

      {/* OVERLAYS */}
      <AnimatePresence>
        {/* Success Modal (Replaces Toast) */}
        {showSuccessModal && (
          <motion.div
            className="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-md bg-black/80 p-4"
            initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }}
          >
            <motion.div
              className="bg-midnight-blue border-2 border-emerald-500 p-8 rounded-xl shadow-[0_0_50px_rgba(16,185,129,0.2)] max-w-lg w-full text-center relative overflow-hidden"
              initial={{ scale: 0.9, y: 20 }} animate={{ scale: 1, y: 0 }}
            >
              {/* Success Icon */}
              <div className="w-16 h-16 bg-emerald-500 rounded-full flex items-center justify-center mx-auto mb-6 shadow-lg shadow-emerald-500/40">
                <svg className="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M5 13l4 4L19 7"></path></svg>
              </div>

              <h3 className="text-3xl font-display font-bold text-white mb-2">Application Received!</h3>
              <div className="bg-emerald-900/20 border border-emerald-500/30 p-4 rounded-lg mb-6">
                <p className="text-emerald-100/70 text-xs uppercase tracking-widest mb-1">Reserved Date</p>
                <p className="text-2xl font-bold text-white">
                  {selectedDate?.toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
                </p>
              </div>

              <div className="bg-white/5 rounded-lg p-6 text-left mb-6 space-y-4 border border-white/10">
                <div>
                  <h4 className="text-burnished-gold font-bold mb-1 flex items-center gap-2">
                    <span className="text-lg">⚠️</span> Important: Payment Required
                  </h4>
                  <p className="text-white/80 text-sm leading-relaxed">
                    You have <span className="font-bold text-white underline decoration-burnished-gold">4 days</span> to submit payment to secure your date. If payment is not received, the date will be released.
                  </p>
                </div>

                <div className="border-t border-white/10 pt-4">
                  <h4 className="text-white font-bold text-sm mb-2">Payment Options:</h4>
                  <ul className="text-white/70 text-sm space-y-2 list-disc list-inside">
                    <li>Drop off a check at the Club (Bar or Office)</li>
                    <li>Mail a check to the Club address</li>
                    {/* Placeholder for Online Payment Feature */}
                    {/* TODO: Check 'EnableOnlineRentalsPayment' setting here later */}
                    <li className="flex items-center gap-2">
                      <span>Pay Online</span>
                      <span className="text-[10px] bg-emerald-500/20 text-emerald-400 px-2 py-0.5 rounded border border-emerald-500/30">Coming Soon</span>
                    </li>
                  </ul>
                </div>
              </div>

              <button
                onClick={handleCloseSuccess}
                className="w-full py-3 bg-emerald-600 hover:bg-emerald-500 text-white font-bold rounded-lg shadow-lg hover:shadow-emerald-500/20 transition-all transform hover:-translate-y-1"
              >
                I Understand - View Calendar
              </button>
            </motion.div>
          </motion.div>
        )}

        {/* Draft Found Prompt */}
        {showDraftPrompt && (
          <motion.div
            className="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-black/60 p-4"
            initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }}
          >
            <div className="bg-midnight-blue border border-burnished-gold p-8 rounded-xl shadow-2xl max-w-lg w-full">
              <h3 className="text-2xl font-bold text-white mb-2">Unfinished Application Found</h3>
              <p className="text-white/70 mb-6">
                We found a saved application from {draftData?.lastUpdated ? new Date(draftData.lastUpdated).toLocaleDateString() : 'earlier'}.
                How would you like to proceed?
              </p>
              <div className="flex flex-col gap-3">
                <button onClick={() => handleDraftChoice('resume_original')} className="w-full py-3 bg-emerald-600 hover:bg-emerald-500 text-white font-bold rounded">
                  Resume Application for {draftData?.selectedDate ? new Date(draftData.selectedDate).toLocaleDateString() : 'Original Date'}
                </button>
                <button onClick={() => handleDraftChoice('use_new_date')} className="w-full py-3 bg-burnished-gold hover:bg-yellow-500 text-midnight-blue font-bold rounded">
                  Use Details for New Date ({selectedDate?.toLocaleDateString()}) ({selectedDate?.toLocaleDateString(undefined, { weekday: 'long' })})
                </button>
                <button onClick={() => handleDraftChoice('discard')} className="w-full py-3 border border-red-500/50 text-red-400 hover:bg-red-500/10 rounded">
                  Delete Draft & Start Fresh
                </button>
              </div>
            </div>
          </motion.div>
        )}

        {/* Save on Exit Prompt */}
        {showSavePrompt && (
          <motion.div
            className="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-black/60 p-4"
            initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }}
          >
            <div className="bg-midnight-blue border border-white/20 p-8 rounded-xl shadow-2xl max-w-md w-full">
              <h3 className="text-xl font-bold text-white mb-2">Save Progress?</h3>
              <p className="text-white/70 mb-4 font-bold">
                Application Date: {selectedDate?.toLocaleDateString()}
              </p>
              <p className="text-white/70 mb-6">
                You are navigating away without submitting. Would you like to keep your details as a draft?
              </p>
              <div className="flex gap-4">
                <button onClick={handleDiscardAndExit} className="flex-1 py-2 border border-white/20 text-white/70 hover:bg-white/10 rounded">Discard</button>
                <button onClick={handleSaveAndExit} className="flex-1 py-2 bg-burnished-gold text-midnight-blue font-bold hover:bg-yellow-500 rounded">Save Draft</button>
              </div>
            </div>
          </motion.div>
        )}
      </AnimatePresence>


      <AnimatePresence custom={direction} mode="wait">

        {step === 1 && (
          <motion.div
            key="step1"
            custom={direction}
            variants={variants}
            initial="enter"
            animate="center"
            exit="exit"
            transition={{ type: "spring", stiffness: 300, damping: 30 }}
            className="space-y-8"
          >
            <div className="bg-white/5 p-8 rounded-xl border border-white/10 text-center">
              <h2 className="text-4xl font-display text-burnished-gold mb-6">Our Historic Venue</h2>
              <p className="text-xl text-pure-white/80 max-w-2xl mx-auto mb-8">
                Host your event in true Gloucester style. Our hall features a full-service bar,
                commercial kitchen, and capacity for up to 200 guests.
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
                onClick={() => navigate(2)}
                className="bg-burnished-gold text-midnight-blue font-bold text-xl px-12 py-4 rounded-full hover:bg-burnished-gold/90 transition-all shadow-lg hover:shadow-burnished-gold/20 transform hover:-translate-y-1"
              >
                Check Availability &rarr;
              </button>
            </div>
          </motion.div>
        )}

        {step === 2 && (
          <motion.div
            key="step2"
            custom={direction}
            variants={variants}
            initial="enter"
            animate="center"
            exit="exit"
            transition={{ type: "spring", stiffness: 300, damping: 30 }}
          >
            <RentalAvailabilityCalendar
              onDateSelect={handleDateSelect}
              newlyBookedDate={newlyBookedDate}
            />
            <div className="flex justify-between mt-8">
              <button
                onClick={() => navigate(1)}
                className="bg-pure-white/10 text-pure-white font-bold px-6 py-3 rounded-full hover:bg-pure-white/20 transition-colors"
              >
                &larr; Back to Gallery
              </button>
            </div>
          </motion.div>
        )}

        {step === 3 && (
          <motion.div
            key="step3"
            custom={direction}
            variants={variants}
            initial="enter"
            animate="center"
            exit="exit"
            transition={{ type: "spring", stiffness: 300, damping: 30 }}
          >
            <div className="bg-midnight-blue p-6 rounded-2xl border border-burnished-gold/30 shadow-2xl">
              <div className="text-center mb-8 border-b border-white/10 pb-6">
                <p className="text-pure-white/60 mb-1 uppercase tracking-widest text-sm">Applying for date</p>
                <h2 className="text-4xl font-display font-bold text-burnished-gold">
                  {selectedDate?.toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
                </h2>
              </div>
              <HallRentalForm
                selectedDate={selectedDate!}
                onSuccess={handleSuccess}
                initialData={formData}
                onChange={onFormChange}
              />
            </div>

            <div className="flex justify-between mt-8">
              <button
                onClick={() => navigate(2)}
                className="bg-pure-white/10 text-pure-white font-bold px-6 py-3 rounded-full hover:bg-pure-white/20 transition-colors"
              >
                &larr; Change Date
              </button>
            </div>
          </motion.div>
        )}

      </AnimatePresence>
    </div>
  );
};

export default HallRentalPage;
