// [NEW]
'use client';
import React from 'react';
import Modal from './Common/Modal';

interface DonationModalProps {
    isOpen: boolean;
    onClose: () => void;
}

const DonationModal: React.FC<DonationModalProps> = ({ isOpen, onClose }) => {
    return (
        <Modal isOpen={isOpen} onClose={onClose}>
            <h2>Support the GFC</h2>
            <p>Your generous donations help us continue our mission of fostering community and friendship.</p>

            <div style={{ marginTop: '1rem' }}>
                <h4>By Mail</h4>
                <p>Please make checks payable to the Gloucester Fraternity Club and mail to:</p>
                <p>
                    Gloucester Fraternity Club<br />
                    27 Webster St<br />
                    Gloucester, MA 01930
                </p>
            </div>

            <div style={{ marginTop: '2rem' }}>
                <h4>Online</h4>
                <p>Online donations are coming soon. Thank you for your patience!</p>
            </div>
        </Modal>
    );
};

export default DonationModal;
