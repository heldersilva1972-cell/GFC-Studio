import Link from 'next/link';
import styles from './Hero.module.css';

interface HeroProps {
  badge?: string;
  title: string;
  subtitle: string;
  primaryCtaText?: string;
  primaryCtaLink?: string;
  secondaryCtaText?: string;
  secondaryCtaLink?: string;
  stats?: { label: string; value: string }[];
}

export default function Hero({
  badge = "Est. 1923",
  title,
  subtitle,
  primaryCtaText,
  primaryCtaLink,
  secondaryCtaText,
  secondaryCtaLink,
  stats
}: HeroProps) {
  return (
    <section className={styles.hero}>
      <div className={styles.heroBackground}>
        <div className={styles.gradientOverlay}></div>
        <div className={styles.patternOverlay}></div>
      </div>

      <div className={`container ${styles.heroContent}`}>
        {badge && (
          <div className={styles.badge}>
            <span className={styles.badgeText}>{badge}</span>
          </div>
        )}

        <h1 className={styles.heroTitle}>{title}</h1>
        <p className={styles.heroSubtitle}>{subtitle}</p>

        <div className={styles.heroActions}>
          {primaryCtaText && primaryCtaLink && (
            <Link href={primaryCtaLink} className="btn btn-primary">
              {primaryCtaText}
            </Link>
          )}
          {secondaryCtaText && secondaryCtaLink && (
            <Link href={secondaryCtaLink} className="btn btn-primary">
              {secondaryCtaText}
            </Link>
          )}
        </div>

        {stats && stats.length > 0 && (
          <div className={styles.heroStats}>
            {stats.map((stat, index) => (
              <div key={index} className={styles.statWrapper} style={{ display: 'contents' }}>
                <div className={styles.stat}>
                  <div className={styles.statNumber}>{stat.value}</div>
                  <div className={styles.statLabel}>{stat.label}</div>
                </div>
                {index < stats.length - 1 && <div className={styles.statDivider}></div>}
              </div>
            ))}
          </div>
        )}
      </div>

      <div className={styles.heroWave}>
        <svg viewBox="0 0 1440 120" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M0 64.9524L60 69.4604C120 73.9684 240 82.9844 360 83.2504C480 83.5164 600 75.0324 720 71.0404C840 67.0484 960 67.5484 1080 73.0804C1200 78.6124 1320 89.1764 1380 94.4584L1440 99.7404V120H1380C1320 120 1200 120 1080 120C960 120 840 120 720 120C600 120 480 120 360 120C240 120 120 120 60 120H0V64.9524Z" fill="white" />
        </svg>
      </div>
    </section>
  );
}
