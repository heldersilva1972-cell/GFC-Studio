'use client'

import { ArrowLeft } from 'lucide-react'
import styles from './BackToWebAppButton.module.css'

export default function BackToWebAppButton() {
    return (
        <a
            href="http://localhost:5000"
            className={styles.backButton}
            title="Return to Admin Panel"
        >
            <ArrowLeft size={20} />
            <span className={styles.buttonText}>Back to Admin</span>
        </a>
    )
}
