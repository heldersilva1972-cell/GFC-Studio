/** @type {import('tailwindcss').Config} */
const config = {
  content: [
    "./app/**/*.{ts,tsx,js,jsx,mdx}",
    "./components/**/*.{ts,tsx,js,jsx,mdx}",
    "./lib/**/*.{ts,tsx,js,jsx}",
    "./pages/**/*.{ts,tsx,js,jsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: "var(--studio-color-primary)",
        "primary-soft": "var(--studio-color-primary-soft)",
        accent: "var(--studio-color-accent)",
        "accent-soft": "var(--studio-color-accent-soft)",
        "bg-canvas": "var(--studio-color-bg-canvas)",
        "bg-elevated": "var(--studio-color-bg-elevated)",
        "border-subtle": "var(--studio-color-border-subtle)",
        "text-strong": "var(--studio-color-text-strong)",
        "text-muted": "var(--studio-color-text-muted)",
        danger: "var(--studio-color-danger)",
        success: "var(--studio-color-success)",
        warning: "var(--studio-color-warning)",
      },
      borderRadius: {
        none: "var(--studio-radius-none)",
        sm: "var(--studio-radius-sm)",
        md: "var(--studio-radius-md)",
        lg: "var(--studio-radius-lg)",
        xl: "var(--studio-radius-xl)",
        full: "var(--studio-radius-full)",
      },
      boxShadow: {
        soft: "var(--studio-shadow-soft)",
        medium: "var(--studio-shadow-medium)",
        strong: "var(--studio-shadow-strong)",
      },
      fontFamily: {
        sans: [
          "Inter",
          "Inter var",
          "system-ui",
          "-apple-system",
          "BlinkMacSystemFont",
          '"Segoe UI"',
          '"Helvetica Neue"',
          "Arial",
          "sans-serif",
        ],
      },
    },
  },
  plugins: [],
};

module.exports = config;

