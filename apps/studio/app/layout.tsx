import "./globals.css";

export const metadata = {
  title: "Animation Playground",
  description: "Interactive animation playground built with Next.js, TypeScript, and Tailwind CSS.",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" className="dark">
      <body className="h-screen overflow-hidden bg-[var(--studio-color-bg-canvas)] text-[var(--studio-color-text-strong)] antialiased">
        {children}
      </body>
    </html>
  );
}

