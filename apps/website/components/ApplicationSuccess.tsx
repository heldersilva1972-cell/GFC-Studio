// [NEW]
'use client';

import { motion } from 'framer-motion';

interface ApplicationSuccessProps {
    formData: {
        name: string;
        email: string;
        phone: string;
        details: string;
    };
    selectedDate: Date;
}

export default function ApplicationSuccess({ formData, selectedDate }: ApplicationSuccessProps) {
    const handlePrint = () => {
        window.print();
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
            className="text-center p-8 bg-green-500/10 border border-green-500 rounded-lg max-w-2xl mx-auto"
        >
            <h3 className="font-display text-3xl font-bold text-green-400">Application Received!</h3>
            <p className="mt-2 text-green-300">Thank you for your interest in renting our hall. We have received your application and will be in touch shortly.</p>

            <div className="mt-6 text-left bg-midnight-blue/20 p-6 rounded-lg">
                <h4 className="font-bold text-xl mb-4 text-pure-white">Application Summary</h4>
                <div className="space-y-2 text-pure-white/80">
                    <p><strong>Name:</strong> {formData.name}</p>
                    <p><strong>Email:</strong> {formData.email}</p>
                    <p><strong>Phone:</strong> {formData.phone}</p>
                    <p><strong>Requested Date:</strong> {selectedDate.toLocaleDateString()}</p>
                    <p><strong>Event Details:</strong></p>
                    <p className="whitespace-pre-wrap">{formData.details}</p>
                </div>
            </div>

            <motion.button
                onClick={handlePrint}
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                className="mt-6 bg-burnished-gold text-midnight-blue font-bold px-6 py-3 rounded-md hover:bg-burnished-gold/90 transition-colors"
            >
                Print Summary
            </motion.button>
        </motion.div>
    );
}
