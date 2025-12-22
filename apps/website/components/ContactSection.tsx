'use client'

import { motion } from 'framer-motion'
import { Phone, Mail, MapPin, Clock, Facebook, Instagram, Twitter } from 'lucide-react'
import styles from './ContactSection.module.css'

export default function ContactSection() {
    return (
        <section className={styles.contactSection}>
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                    transition={{ duration: 0.6 }}
                    className="text-center mb-xl"
                >
                    <h2 className={styles.title}>Visit Us</h2>
                    <p className={styles.subtitle}>
                        We're located in the heart of Gloucester and welcome visitors
                    </p>
                </motion.div>

                <div className={styles.grid}>
                    <motion.div
                        initial={{ opacity: 0, x: -30 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                        transition={{ duration: 0.6 }}
                        className={styles.infoCard}
                    >
                        <div className={styles.infoItem}>
                            <div className={styles.iconWrapper}>
                                <MapPin size={24} />
                            </div>
                            <div>
                                <h3 className={styles.infoTitle}>Address</h3>
                                <p className={styles.infoText}>
                                    27 Webster Street<br />
                                    Gloucester, MA 01930
                                </p>
                            </div>
                        </div>

                        <div className={styles.infoItem}>
                            <div className={styles.iconWrapper}>
                                <Phone size={24} />
                            </div>
                            <div>
                                <h3 className={styles.infoTitle}>Phone</h3>
                                <a href="tel:+19782832889" className={styles.infoLink}>
                                    (978) 283-2889
                                </a>
                            </div>
                        </div>

                        <div className={styles.infoItem}>
                            <div className={styles.iconWrapper}>
                                <Mail size={24} />
                            </div>
                            <div>
                                <h3 className={styles.infoTitle}>Email</h3>
                                <a href="mailto:gfc@gloucesterfraternityclub.com" className={styles.infoLink}>
                                    gfc@gloucesterfraternityclub.com
                                </a>
                            </div>
                        </div>

                        <div className={styles.infoItem}>
                            <div className={styles.iconWrapper}>
                                <Clock size={24} />
                            </div>
                            <div>
                                <h3 className={styles.infoTitle}>Hours</h3>
                                <p className={styles.infoText}>
                                    Sunday–Thursday: 11:00 AM – 10:00 PM<br />
                                    Friday–Saturday: 11:00 AM – 12:00 Midnight
                                </p>
                            </div>
                        </div>

                        <div className={styles.socialLinks}>
                            <a href="https://www.facebook.com/GloucesterFraternityClub/" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>
                                <Facebook size={24} />
                            </a>
                            <a href="https://instagram.com/gfc9651/" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>
                                <Instagram size={24} />
                            </a>
                            <a href="https://twitter.com/GFC_club/" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>
                                <Twitter size={24} />
                            </a>
                        </div>
                    </motion.div>

                    <motion.div
                        initial={{ opacity: 0, x: 30 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                        transition={{ duration: 0.6 }}
                        className={styles.mapPlaceholder}
                    >
                        <div className={styles.mapContent}>
                            <MapPin size={48} className={styles.mapIcon} />
                            <p className={styles.mapText}>Interactive Map Coming Soon</p>
                            <a
                                href="https://www.google.com/maps/search/?api=1&query=27+Webster+Street+Gloucester+MA+01930"
                                target="_blank"
                                rel="noopener noreferrer"
                                className="btn btn-primary"
                            >
                                Open in Google Maps
                            </a>
                        </div>
                    </motion.div>
                </div>
            </div>
        </section>
    )
}
