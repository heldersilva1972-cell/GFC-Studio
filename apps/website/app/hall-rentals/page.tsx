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
  const [pricing, setPricing] = useState<any>(null);

  useEffect(() => {
    console.log('Fetching pricing from API...');
    fetch('/api/hall-rental/pricing')
      .then(res => {
        console.log('Pricing API response status:', res.status);
        return res.json();
      })
      .then(data => {
        console.log('Pricing data received:', data);
        setPricing(data);
      })
      .catch(err => console.error('Error fetching pricing:', err));
  }, []);

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
        {/* SUCCESS MODAL - INSANE CINEMATIC ANIMATIONS */}
        {showSuccessModal && (
          <motion.div
            className="fixed inset-0 z-50 flex items-center justify-center p-4 overflow-hidden"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
          >
            {/* CINEMATIC ZOOM BACKGROUND */}
            <motion.div
              className="absolute inset-0 bg-black"
              initial={{ scale: 3 }}
              animate={{ scale: 1 }}
              transition={{ duration: 1.2, ease: [0.6, 0.01, 0.05, 0.95] }}
            >
              <motion.div
                className="absolute inset-0"
                style={{
                  background: 'radial-gradient(circle at center, rgba(212,175,55,0.4) 0%, rgba(0,0,0,1) 70%)'
                }}
                animate={{
                  scale: [1, 1.2, 1],
                  opacity: [0.5, 0.8, 0.5]
                }}
                transition={{ duration: 3, repeat: Infinity }}
              />
            </motion.div>

            {/* CONFETTI EXPLOSION - 150 PARTICLES */}
            <div className="absolute inset-0 pointer-events-none">
              {[...Array(150)].map((_, i) => {
                const colors = ['#D4AF37', '#FFD700', '#FFF', '#FF6B6B', '#4ECDC4', '#A78BFA', '#FB923C'];
                const angle = Math.random() * Math.PI * 2;
                const velocity = 200 + Math.random() * 500;
                const rotation = Math.random() * 1080 - 540;
                const size = Math.random() * 12 + 4;
                return (
                  <motion.div
                    key={i}
                    className="absolute"
                    style={{
                      left: '50%',
                      top: '50%',
                      width: size,
                      height: size,
                      backgroundColor: colors[Math.floor(Math.random() * colors.length)],
                      borderRadius: Math.random() > 0.5 ? '50%' : '0%',
                      boxShadow: `0 0 ${size}px ${colors[Math.floor(Math.random() * colors.length)]}`
                    }}
                    initial={{ x: 0, y: 0, opacity: 1, rotate: 0, scale: 0 }}
                    animate={{
                      x: Math.cos(angle) * velocity,
                      y: Math.sin(angle) * velocity - 300,
                      opacity: 0,
                      rotate: rotation,
                      scale: [0, 1.5, 0.5]
                    }}
                    transition={{
                      duration: 2.5 + Math.random() * 1.5,
                      ease: 'easeOut'
                    }}
                  />
                );
              })}
            </div>

            {/* PULSING ENERGY RINGS - 5 RINGS */}
            {[...Array(5)].map((_, i) => (
              <motion.div
                key={i}
                className="absolute rounded-full"
                style={{
                  left: '50%',
                  top: '50%',
                  transform: 'translate(-50%, -50%)',
                  border: `${3 - i * 0.4}px solid rgba(212, 175, 55, ${0.8 - i * 0.15})`
                }}
                initial={{ width: 0, height: 0, opacity: 0.8 }}
                animate={{
                  width: 1000,
                  height: 1000,
                  opacity: 0
                }}
                transition={{
                  duration: 2.5,
                  delay: i * 0.2,
                  repeat: Infinity,
                  repeatDelay: 1
                }}
              />
            ))}

            {/* MAIN SUCCESS CARD WITH 3D ROTATION */}
            <motion.div
              initial={{
                scale: 0,
                rotateZ: -180,
                rotateY: -90,
                opacity: 0
              }}
              animate={{
                scale: 1,
                rotateZ: 0,
                rotateY: 0,
                opacity: 1
              }}
              transition={{
                type: 'spring',
                damping: 12,
                stiffness: 80,
                delay: 0.5
              }}
              className="relative bg-gradient-to-br from-midnight-blue via-deep-navy to-black border-4 border-burnished-gold rounded-3xl p-10 max-w-2xl w-full shadow-2xl max-h-[90vh] overflow-y-auto"
              style={{ perspective: '1000px', transformStyle: 'preserve-3d' }}
            >
              {/* LENS FLARE EFFECTS */}
              <motion.div
                className="absolute -top-20 -right-20 w-40 h-40 bg-burnished-gold rounded-full blur-3xl opacity-50"
                animate={{
                  scale: [1, 1.5, 1],
                  opacity: [0.3, 0.6, 0.3]
                }}
                transition={{ duration: 2, repeat: Infinity }}
              />
              <motion.div
                className="absolute -bottom-20 -left-20 w-40 h-40 bg-emerald-500 rounded-full blur-3xl opacity-30"
                animate={{
                  scale: [1.2, 1, 1.2],
                  opacity: [0.2, 0.4, 0.2]
                }}
                transition={{ duration: 3, repeat: Infinity }}
              />

              {/* GLITCH OVERLAY EFFECT */}
              <motion.div
                className="absolute inset-0 bg-burnished-gold/10 rounded-3xl pointer-events-none mix-blend-overlay"
                animate={{
                  opacity: [0, 0.3, 0, 0.5, 0],
                  x: [0, -5, 5, -3, 0],
                }}
                transition={{ duration: 0.3, times: [0, 0.2, 0.4, 0.6, 1], repeat: 2, delay: 0.8 }}
              />

              {/* HOLOGRAPHIC SCANNING LINES */}
              <motion.div
                className="absolute inset-0 pointer-events-none overflow-hidden rounded-3xl"
                initial={{ opacity: 0 }}
                animate={{ opacity: [0, 0.3, 0] }}
                transition={{ duration: 2, repeat: Infinity, delay: 1 }}
              >
                <motion.div
                  className="absolute w-full h-1 bg-gradient-to-r from-transparent via-burnished-gold to-transparent"
                  animate={{ y: ['0%', '100%'] }}
                  transition={{ duration: 2, repeat: Infinity, ease: 'linear' }}
                />
              </motion.div>

              {/* SUCCESS ICON WITH ANIMATED CHECKMARK */}
              <motion.div
                initial={{ scale: 0, rotate: -180 }}
                animate={{ scale: 1, rotate: 0 }}
                transition={{ delay: 0.8, type: 'spring', damping: 10 }}
                className="text-center mb-6 relative z-10"
              >
                <motion.div
                  className="w-24 h-24 mx-auto mb-4 rounded-full bg-gradient-to-r from-green-400 to-emerald-600 flex items-center justify-center relative"
                  animate={{
                    boxShadow: [
                      '0 0 30px rgba(52, 211, 153, 0.5)',
                      '0 0 60px rgba(52, 211, 153, 0.8)',
                      '0 0 30px rgba(52, 211, 153, 0.5)'
                    ]
                  }}
                  transition={{ duration: 1.5, repeat: Infinity }}
                >
                  {/* Rotating ring behind checkmark */}
                  <motion.div
                    className="absolute inset-0 rounded-full border-4 border-white/30"
                    animate={{ rotate: 360 }}
                    transition={{ duration: 3, repeat: Infinity, ease: 'linear' }}
                  />
                  <motion.svg
                    className="w-12 h-12 text-white relative z-10"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                    initial={{ pathLength: 0 }}
                    animate={{ pathLength: 1 }}
                    transition={{ delay: 1.2, duration: 0.5, ease: 'easeInOut' }}
                  >
                    <motion.path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="3"
                      d="M5 13l4 4L19 7"
                    />
                  </motion.svg>
                </motion.div>

                <motion.h2
                  initial={{ y: 20, opacity: 0 }}
                  animate={{ y: 0, opacity: 1 }}
                  transition={{ delay: 1 }}
                  className="text-5xl font-display font-bold mb-3"
                  style={{
                    background: 'linear-gradient(90deg, #D4AF37, #FFD700, #D4AF37)',
                    backgroundSize: '200% auto',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent',
                    backgroundClip: 'text'
                  }}
                >
                  <motion.span
                    animate={{ backgroundPosition: ['0%', '100%', '0%'] }}
                    transition={{ duration: 3, repeat: Infinity }}
                    style={{ display: 'inline-block' }}
                  >
                    Application Received!
                  </motion.span>
                </motion.h2>

                <motion.div
                  initial={{ opacity: 0, scale: 0.8 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ delay: 1.2 }}
                  className="bg-black/40 backdrop-blur-sm p-4 rounded-xl border border-burnished-gold/30 mb-6"
                >
                  <p className="text-pure-white/60 text-sm mb-1">RESERVED DATE</p>
                  <motion.p
                    className="text-2xl font-bold text-pure-white"
                    animate={{ scale: [1, 1.05, 1] }}
                    transition={{ duration: 2, repeat: Infinity }}
                  >
                    {selectedDate?.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
                  </motion.p>
                </motion.div>
              </motion.div>

              {/* PAYMENT INFORMATION - CRITICAL */}
              <motion.div
                initial={{ opacity: 0, y: 30 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 1.4 }}
                className="bg-yellow-900/20 p-6 rounded-xl border-2 border-yellow-600/50 mb-6 relative z-10"
              >
                <div className="flex items-start gap-3 mb-4">
                  <motion.span
                    className="text-3xl"
                    animate={{ scale: [1, 1.2, 1], rotate: [0, -10, 10, 0] }}
                    transition={{ duration: 1, repeat: Infinity }}
                  >
                    ⚠️
                  </motion.span>
                  <div>
                    <h3 className="text-xl font-bold text-yellow-400 mb-2">Important: Payment Required</h3>
                    <p className="text-pure-white/90">
                      You have <strong className="text-yellow-400">4 days</strong> to submit payment to secure your date.
                      If payment is not received, the date will be released.
                    </p>
                  </div>
                </div>

                <div className="space-y-3">
                  <p className="font-bold text-pure-white">Payment Options:</p>
                  <ul className="space-y-2 text-pure-white/90">
                    <li className="flex items-center gap-2">
                      <span className="text-green-400">•</span>
                      <span>Drop off a check at the Club (Bar or Office)</span>
                    </li>
                    <li className="flex items-center gap-2">
                      <span className="text-green-400">•</span>
                      <span>Mail a check to the Club address</span>
                    </li>
                    <li className="flex items-center gap-2">
                      <span className="text-blue-400">•</span>
                      <span>Pay Online <span className="bg-green-600 text-white text-xs px-2 py-1 rounded ml-2">Coming Soon</span></span>
                    </li>
                  </ul>
                </div>
              </motion.div>

              {/* ACTION BUTTON */}
              <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ delay: 1.6 }}
                className="relative z-10"
              >
                <motion.button
                  onClick={handleCloseSuccess}
                  whileHover={{
                    scale: 1.05,
                    boxShadow: '0 0 50px rgba(212, 175, 55, 0.6)'
                  }}
                  whileTap={{ scale: 0.95 }}
                  className="w-full bg-gradient-to-r from-burnished-gold to-yellow-600 text-midnight-blue font-bold px-8 py-4 rounded-xl shadow-lg transition-all text-lg relative overflow-hidden group"
                >
                  <motion.div
                    className="absolute inset-0 bg-white/20"
                    initial={{ x: '-100%' }}
                    whileHover={{ x: '100%' }}
                    transition={{ duration: 0.5 }}
                  />
                  <span className="relative z-10">Return to Hall Rentals</span>
                </motion.button>
              </motion.div>
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
                <div className="bg-black/20 p-6 rounded-lg border border-white/5">
                  <h3 className="text-burnished-gold font-bold mb-2">Member Rate</h3>
                  <p className="text-3xl font-display text-white">${pricing?.functionHallMemberRate || 300}</p>
                  <p className="text-xs text-white/50 mt-1">For active club members</p>
                </div>
                <div className="bg-black/20 p-6 rounded-lg border border-burnished-gold/20">
                  <h3 className="text-burnished-gold font-bold mb-2">Non-Member Rate</h3>
                  <p className="text-3xl font-display text-white">${pricing?.functionHallNonMemberRate || 400}</p>
                  <p className="text-xs text-white/50 mt-1">For guests and public events</p>
                </div>
                <div className="bg-black/20 p-6 rounded-lg border border-white/5">
                  <h3 className="text-burnished-gold font-bold mb-2">Youth Organization</h3>
                  <p className="text-3xl font-display text-white">${pricing?.youthOrganizationNonMemberRate || 100}</p>
                  <p className="text-xs text-white/50 mt-1">For youth events</p>
                </div>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-6 text-left mb-8">
                <div className="bg-white/5 p-4 rounded-lg flex items-center space-x-4">
                  <div className="bg-burnished-gold/20 p-3 rounded-full text-burnished-gold">
                    <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"></path></svg>
                  </div>
                  <div>
                    <h4 className="text-white font-bold">Full Amenities</h4>
                    <p className="text-sm text-white/60">Commercial kitchen & full-service bar access.</p>
                  </div>
                </div>
                <div className="bg-white/5 p-4 rounded-lg flex items-center space-x-4">
                  <div className="bg-burnished-gold/20 p-3 rounded-full text-burnished-gold">
                    <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                  </div>
                  <div>
                    <h4 className="text-white font-bold">Flexible Hours</h4>
                    <p className="text-sm text-white/60">Available for day or evening functions.</p>
                  </div>
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
