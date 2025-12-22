'use client'

import { motion } from 'framer-motion'
import { ArrowRight, Calendar, Home } from 'lucide-react'
import Link from 'next/link'
import styles from './Hero.module.css'

export default function Hero() {
    return (
        <section className={styles.hero}>
            <div className={styles.heroBackground}>
                <div className={styles.gradientOverlay}></div>
                <div className={styles.patternOverlay}></div>
            </div>

            <div className="container">
                <div className={styles.heroContent}>
                    <motion.div
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.8, delay: 0.2 }}
                        className={styles.badge}
                    >
                        <span className={styles.badgeText}>Est. 1923</span>
                    </motion.div>

                    <motion.h1
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.8, delay: 0.4 }}
                        className={styles.heroTitle}
                    >
                        Building Community, Friendship, and Tradition
                    </motion.h1>

                    <motion.p
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.8, delay: 0.6 }}
                        className={styles.heroSubtitle}
                    >
                        Welcome to the Gloucester Fraternity Club — a place where friendship, family, and community come together.
                        Since our founding in the early 1920s, we've proudly served the Cape Ann area through fellowship, fun, and service.
                    </motion.p>

                    <motion.div
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.8, delay: 0.8 }}
                        className={styles.heroActions}
                    >
                        <Link href="/hall-rentals" className="btn btn-primary btn-lg">
                            <Home size={20} />
                            Rent Our Hall
                            <ArrowRight size={20} />
                        </Link>
                        <Link href="/events" className="btn btn-outline btn-lg">
                            <Calendar size={20} />
                            View Events
                        </Link>
                    </motion.div>

                    <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ duration: 1, delay: 1 }}
                        className={styles.heroStats}
                    >
                        <div className={styles.stat}>
                            <div className={styles.statNumber}>100+</div>
                            <div className={styles.statLabel}>Years of Service</div>
                        </div>
                        <div className={styles.statDivider}></div>
                        <div className={styles.stat}>
                            <div className={styles.statNumber}>500+</div>
                            <div className={styles.statLabel}>Active Members</div>
                        </div>
                        <div className={styles.statDivider}></div>
                        <div className={styles.stat}>
                            <div className={styles.statNumber}>50+</div>
                            <div className={styles.statLabel}>Events Yearly</div>
                        </div>
                    </motion.div>
                </div>
            </div>

            <div className={styles.heroWave}>
                <svg viewBox="0 0 1440 120" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path
                        d="M0 0L60 10C120 20 240 40 360 46.7C480 53 600 47 720 43.3C840 40 960 40 1080 46.7C1200 53 1320 67 1380 73.3L1440 80V120H1380C1320 120 1200 120 1080 120C960 120 840 120 720 120C600 120 480 120 360 120C240 120 120 120 60 120H0V0Z"
                        fill="white"
                    />
                </svg>
            </div>
        </section>
    )
}
