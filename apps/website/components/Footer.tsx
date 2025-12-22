import Link from 'next/link'
import { Facebook, Instagram, Twitter, Heart } from 'lucide-react'
import styles from './Footer.module.css'

export default function Footer() {
    const currentYear = new Date().getFullYear()

    return (
        <footer className={styles.footer}>
            <div className="container">
                <div className={styles.footerGrid}>
                    <div className={styles.footerSection}>
                        <h3 className={styles.footerTitle}>Gloucester Fraternity Club</h3>
                        <p className={styles.footerText}>
                            Building community, friendship, and tradition since 1923. Proudly serving the Cape Ann area.
                        </p>
                        <div className={styles.socialLinks}>
                            <a href="https://www.facebook.com/GloucesterFraternityClub/" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>
                                <Facebook size={20} />
                            </a>
                            <a href="https://instagram.com/gfc9651/" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>
                                <Instagram size={20} />
                            </a>
                            <a href="https://twitter.com/GFC_club/" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>
                                <Twitter size={20} />
                            </a>
                        </div>
                    </div>

                    <div className={styles.footerSection}>
                        <h4 className={styles.footerHeading}>Quick Links</h4>
                        <ul className={styles.footerLinks}>
                            <li><Link href="/">Home</Link></li>
                            <li><Link href="/hall-rentals">Hall Rentals</Link></li>
                            <li><Link href="/events">Events</Link></li>
                            <li><Link href="/membership">Membership</Link></li>
                        </ul>
                    </div>

                    <div className={styles.footerSection}>
                        <h4 className={styles.footerHeading}>More</h4>
                        <ul className={styles.footerLinks}>
                            <li><Link href="/gallery">Photo Gallery</Link></li>
                            <li><Link href="/contact">Contact Us</Link></li>
                            <li><Link href="/">About Us</Link></li>
                            <li><Link href="/">History</Link></li>
                        </ul>
                    </div>

                    <div className={styles.footerSection}>
                        <h4 className={styles.footerHeading}>Contact</h4>
                        <ul className={styles.footerContact}>
                            <li>27 Webster Street</li>
                            <li>Gloucester, MA 01930</li>
                            <li><a href="tel:+19782832889">(978) 283-2889</a></li>
                            <li><a href="mailto:gfc@gloucesterfraternityclub.com">gfc@gloucesterfraternityclub.com</a></li>
                        </ul>
                    </div>
                </div>

                <div className={styles.footerBottom}>
                    <p className={styles.copyright}>
                        © {currentYear} Gloucester Fraternity Club. All rights reserved.
                    </p>
                    <p className={styles.madeWith}>
                        Made with <Heart size={16} className={styles.heart} /> for our community
                    </p>
                </div>
            </div>
        </footer>
    )
}
