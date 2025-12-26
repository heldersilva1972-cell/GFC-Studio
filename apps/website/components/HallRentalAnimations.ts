// Advanced Animation Configurations for Hall Rental Modals
// These are cutting-edge, multi-layered animations

export const confirmationModalVariants = {
    backdrop: {
        initial: { opacity: 0 },
        animate: {
            opacity: 1,
            transition: { duration: 0.3 }
        },
        exit: { opacity: 0 }
    },

    container: {
        initial: {
            scale: 0,
            rotateY: -180,
            rotateX: -45,
            z: -2000,
            opacity: 0
        },
        animate: {
            scale: 1,
            rotateY: 0,
            rotateX: 0,
            z: 0,
            opacity: 1,
            transition: {
                type: 'spring',
                damping: 15,
                stiffness: 100,
                duration: 0.8
            }
        },
        exit: {
            scale: 0.8,
            opacity: 0,
            transition: { duration: 0.3 }
        }
    }
};

export const successModalVariants = {
    backdrop: {
        initial: { opacity: 0, scale: 3 },
        animate: {
            opacity: 1,
            scale: 1,
            transition: {
                duration: 1.2,
                ease: [0.6, 0.01, 0.05, 0.95]
            }
        }
    },

    container: {
        initial: {
            scale: 0,
            rotateZ: -180,
            opacity: 0
        },
        animate: {
            scale: 1,
            rotateZ: 0,
            opacity: 1,
            transition: {
                type: 'spring',
                damping: 12,
                stiffness: 80,
                delay: 0.5
            }
        }
    }
};
