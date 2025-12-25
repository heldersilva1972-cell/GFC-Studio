const { fontFamily } = require('tailwindcss/defaultTheme');

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './app/**/*.{js,ts,jsx,tsx,mdx}',
    './components/**/*.{js,ts,jsx,tsx,mdx}',
  ],
  theme: {
    extend: {
      colors: {
        'midnight-blue': '#0A192F', // Primary background
        'burnished-gold': '#D4AF37', // Highlights and CTAs
        'pure-white': '#FFFFFF',     // Crisp content
      },
      fontFamily: {
        sans: ['var(--font-inter)', ...fontFamily.sans],
        display: ['var(--font-outfit)'],
      },
    },
  },
  plugins: [],
}
