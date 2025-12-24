// [MODIFIED]
'use client';

import { motion } from 'framer-motion';
import { Calendar, Home, Users, Heart, Camera, MapPin } from 'lucide-react';
import Link from 'next/link';
import styles from './FeatureGrid.module.css';

const features = [
    {
        icon: Calendar,
        title: 'Upcoming Events',
        description: 'Join us for great local events throughout the year! From family gatherings to monthly dances, there\'s something for everyone.',
        highlights: ['Easter Bunny Breakfast', 'Pancakes with Santa', 'Special Needs Dance', 'Thursday Night Dart League'],
        link: '/events',
        color: 'primary',
    },
    {
        icon: Home,
        title: 'Hall Rentals',
        description: 'Hosting a celebration, meeting, or fundraiser? Our upstairs and downstairs halls are available for rent.',
        highlights: ['Flexible rates', 'Full kitchen and bar', 'Ample parking', 'Special nonprofit pricing'],
        link: '/hall-rentals',
        color: 'secondary',
    },
    {
        icon: Users,
        title: 'Membership',
        description: 'Become part of a proud Gloucester tradition. Membership brings neighbors together through friendship and service.',
        highlights: ['Community events', 'Exclusive benefits', 'Networking opportunities', 'Support local causes'],
        link: '/membership',
        color: 'accent',
    },
    {
        icon: Heart,
        title: 'Support the Club',
        description: 'Our events, outreach, and programs are made possible through the generosity of our members and supporters.',
        highlights: ['Volunteer opportunities', 'Make a donation', 'Sponsor an event', 'Join our mission'],
        link: '/membership',
        color: 'primary',
    },
    {
        icon: Camera,
        title: 'Community in Action',
        description: 'Check out photos from our recent events — from Breakfast with the Easter Bunny to our annual Family Picnic.',
        highlights: ['Event galleries', 'Member spotlights', 'Historical photos', 'Community moments'],
        link: '/gallery',
        color: 'secondary',
    },
    {
        icon: MapPin,
        title: 'Visit Us',
        description: '27 Webster Street, Gloucester, MA. Open Sunday–Thursday: 11 AM – 10 PM, Friday–Saturday: 11 AM – Midnight.',
        highlights: ['Central location', 'Easy parking', 'Accessible facility', 'Welcoming atmosphere'],
        link: '/contact',
        color: 'accent',
    },
];

const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
        opacity: 1,
        transition: {
            staggerChildren: 0.1,
        },
    },
};

const itemVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
        opacity: 1,
        y: 0,
        transition: {
            duration: 0.5,
        },
    },
};

export default function FeatureGrid() {
    return (
        <section className="section">
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                    transition={{ duration: 0.6 }}
                    className="text-center mb-xl"
                >
                    <h2 className={styles.sectionTitle}>What We Offer</h2>
                    <p className={styles.sectionSubtitle}>
                        Discover the many ways to connect with the Gloucester Fraternity Club
                    </p>
                </motion.div>

                <motion.div
                    variants={containerVariants}
                    initial="hidden"
                    whileInView="visible"
                    viewport={{ once: true }}
                    className={styles.grid}
                >
                    {features.map((feature, index) => {
                        const Icon = feature.icon;
                        return (
                            <motion.div
                                key={index}
                                variants={itemVariants}
                                className={`${styles.featureCard} ${styles[feature.color]}`}
                            >
                                <div className={styles.iconWrapper}>
                                    <Icon className={styles.icon} size={32} />
                                </div>
                                <h3 className={styles.featureTitle}>{feature.title}</h3>
                                <p className={styles.featureDescription}>{feature.description}</p>
                                <ul className={styles.highlightList}>
                                    {feature.highlights.map((highlight, idx) => (
                                        <li key={idx} className={styles.highlightItem}>
                                            <span className={styles.highlightDot}></span>
                                            {highlight}
                                        </li>
                                    ))}
                                </ul>
                                <Link href={feature.link} className={styles.featureLink}>
                                    Learn More →
                                </Link>
                            </motion.div>
                        );
                    })}
                </motion.div>
            </div>
        </section>
    );
}
