'use client'

import { useState } from 'react'
import Link from 'next/link'
import { Menu, X } from 'lucide-react'
import styles from './Header.module.css'

export default function Header() {
    const [isMenuOpen, setIsMenuOpen] = useState(false)

    const toggleMenu = () => setIsMenuOpen(!isMenuOpen)

    return (
        <header className={styles.header}>
            <div className="container">
                <nav className={styles.nav}>
                    <Link href="/" className={styles.logo}>
                        <span className={styles.logoText}>GFC</span>
                        <span className={styles.logoSubtext}>Gloucester Fraternity Club</span>
                    </Link>

                    {/* Desktop Navigation */}
                    <ul className={`${styles.navList} ${styles.desktop}`}>
                        <li><Link href="/" className={styles.navLink}>Home</Link></li>
                        <li><Link href="/hall-rentals" className={styles.navLink}>Hall Rentals</Link></li>
                        <li><Link href="/events" className={styles.navLink}>Events</Link></li>
                        <li><Link href="/membership" className={styles.navLink}>Membership</Link></li>
                        <li><Link href="/gallery" className={styles.navLink}>Gallery</Link></li>
                        <li><Link href="/contact" className={styles.navLink}>Contact</Link></li>
                    </ul>

                    {/* Mobile Menu Button */}
                    <button
                        className={styles.menuButton}
                        onClick={toggleMenu}
                        aria-label="Toggle menu"
                    >
                        {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
                    </button>
                </nav>

                {/* Mobile Navigation */}
                {isMenuOpen && (
                    <ul className={`${styles.navList} ${styles.mobile}`}>
                        <li><Link href="/" className={styles.navLink} onClick={toggleMenu}>Home</Link></li>
                        <li><Link href="/hall-rentals" className={styles.navLink} onClick={toggleMenu}>Hall Rentals</Link></li>
                        <li><Link href="/events" className={styles.navLink} onClick={toggleMenu}>Events</Link></li>
                        <li><Link href="/membership" className={styles.navLink} onClick={toggleMenu}>Membership</Link></li>
                        <li><Link href="/gallery" className={styles.navLink} onClick={toggleMenu}>Gallery</Link></li>
                        <li><Link href="/contact" className={styles.navLink} onClick={toggleMenu}>Contact</Link></li>
                    </ul>
                )}
            </div>
        </header>
    )
}
