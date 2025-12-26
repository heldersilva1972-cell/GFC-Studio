'use client';

import { motion } from 'framer-motion';

interface ApplicationSuccessProps {
    formData: {
        name: string;
        email: string;
        phone: string;
        details: string;
        eventDate?: string;
    };
    selectedDate: Date;
}

export default function ApplicationSuccess({ formData, selectedDate }: ApplicationSuccessProps) {
    return (
        <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            className="fixed inset-0 z-50 flex items-center justify-center p-4 overflow-hidden"
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

            {/* CONFETTI EXPLOSION */}
            <div className="absolute inset-0 pointer-events-none">
                {[...Array(100)].map((_, i) => {
                    const colors = ['#D4AF37', '#FFD700', '#FFF', '#FF6B6B', '#4ECDC4'];
                    const angle = Math.random() * Math.PI * 2;
                    const velocity = 200 + Math.random() * 400;
                    const rotation = Math.random() * 720 - 360;
                    return (
                        <motion.div
                            key={i}
                            className="absolute"
                            style={{
                                left: '50%',
                                top: '50%',
                                width: Math.random() * 10 + 5,
                                height: Math.random() * 10 + 5,
                                backgroundColor: colors[Math.floor(Math.random() * colors.length)],
                                borderRadius: Math.random() > 0.5 ? '50%' : '0%'
                            }}
                            initial={{ x: 0, y: 0, opacity: 1, rotate: 0 }}
                            animate={{
                                x: Math.cos(angle) * velocity,
                                y: Math.sin(angle) * velocity - 200,
                                opacity: 0,
                                rotate: rotation
                            }}
                            transition={{
                                duration: 2 + Math.random(),
                                ease: 'easeOut'
                            }}
                        />
                    );
                })}
            </div>

            {/* PULSING ENERGY RINGS */}
            {[...Array(3)].map((_, i) => (
                <motion.div
                    key={i}
                    className="absolute rounded-full border-2 border-burnished-gold"
                    style={{
                        left: '50%',
                        top: '50%',
                        transform: 'translate(-50%, -50%)'
                    }}
                    initial={{ width: 0, height: 0, opacity: 0.8 }}
                    animate={{
                        width: 800,
                        height: 800,
                        opacity: 0
                    }}
                    transition={{
                        duration: 2,
                        delay: i * 0.3,
                        repeat: Infinity,
                        repeatDelay: 0.6
                    }}
                />
            ))}

            {/* MAIN SUCCESS CARD WITH LENS FLARE */}
            <motion.div
                initial={{
                    scale: 0,
                    rotateZ: -180,
                    opacity: 0
                }}
                animate={{
                    scale: 1,
                    rotateZ: 0,
                    opacity: 1
                }}
                transition={{
                    type: 'spring',
                    damping: 12,
                    stiffness: 80,
                    delay: 0.5
                }}
                className="relative bg-gradient-to-br from-midnight-blue via-deep-navy to-black border-4 border-burnished-gold rounded-3xl p-10 max-w-2xl w-full shadow-2xl max-h-[90vh] overflow-y-auto"
            >
                {/* LENS FLARE EFFECT */}
                <motion.div
                    className="absolute -top-20 -right-20 w-40 h-40 bg-burnished-gold rounded-full blur-3xl opacity-50"
                    animate={{
                        scale: [1, 1.5, 1],
                        opacity: [0.3, 0.6, 0.3]
                    }}
                    transition={{ duration: 2, repeat: Infinity }}
                />

                {/* SUCCESS ICON WITH ANIMATED CHECKMARK */}
                <motion.div
                    initial={{ scale: 0, rotate: -180 }}
                    animate={{ scale: 1, rotate: 0 }}
                    transition={{ delay: 0.8, type: 'spring', damping: 10 }}
                    className="text-center mb-6"
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
                        <motion.svg
                            className="w-12 h-12 text-white"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                            initial={{ pathLength: 0 }}
                            animate={{ pathLength: 1 }}
                            transition={{ delay: 1.2, duration: 0.5 }}
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
                        className="text-5xl font-display font-bold text-burnished-gold mb-3"
                    >
                        Application Received!
                    </motion.h2>

                    <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 1.2 }}
                        className="bg-black/40 backdrop-blur-sm p-4 rounded-xl border border-burnished-gold/30 mb-6"
                    >
                        <p className="text-pure-white/60 text-sm mb-1">RESERVED DATE</p>
                        <p className="text-2xl font-bold text-pure-white">
                            {formData.eventDate || selectedDate.toLocaleDateString('en-US', {
                                weekday: 'long',
                                year: 'numeric',
                                month: 'long',
                                day: 'numeric'
                            })}
                        </p>
                    </motion.div>
                </motion.div>

                {/* PAYMENT INFORMATION - CRITICAL */}
                <motion.div
                    initial={{ opacity: 0, y: 30 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 1.4 }}
                    className="bg-yellow-900/20 p-6 rounded-xl border-2 border-yellow-600/50 mb-6"
                >
                    <div className="flex items-start gap-3 mb-4">
                        <motion.span
                            className="text-3xl"
                            animate={{ scale: [1, 1.2, 1] }}
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

                {/* APPLICATION SUMMARY */}
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 1.6 }}
                    className="bg-black/40 backdrop-blur-sm p-6 rounded-xl border border-white/10 mb-6"
                >
                    <h4 className="font-bold text-xl mb-4 text-burnished-gold">Application Summary</h4>
                    <div className="space-y-2 text-pure-white/80">
                        <p><strong>Name:</strong> {formData.name}</p>
                        <p><strong>Email:</strong> {formData.email}</p>
                        <p><strong>Phone:</strong> {formData.phone}</p>
                        {formData.details && (
                            <>
                                <p><strong>Event Details:</strong></p>
                                <p className="whitespace-pre-wrap text-sm">{formData.details}</p>
                            </>
                        )}
                    </div>
                </motion.div>

                {/* ACTION BUTTON */}
                <motion.div
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: 1.8 }}
                >
                    <motion.a
                        href="/hall-rentals"
                        whileHover={{
                            scale: 1.05,
                            boxShadow: '0 0 50px rgba(212, 175, 55, 0.6)'
                        }}
                        whileTap={{ scale: 0.95 }}
                        className="block w-full bg-gradient-to-r from-burnished-gold to-yellow-600 text-midnight-blue font-bold px-8 py-4 rounded-xl shadow-lg transition-all text-lg text-center"
                    >
                        Return to Hall Rentals
                    </motion.a>
                </motion.div>
            </motion.div>
        </motion.div>
    );
}
