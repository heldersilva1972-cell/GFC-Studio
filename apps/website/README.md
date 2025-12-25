# GFC Website - Phase 0

Ultra-modern public-facing website for the Gloucester Fraternity Club.

## 🚀 Quick Start

### Prerequisites
- Node.js 18+ installed
- npm or yarn package manager

### Installation

```bash
# Install dependencies
npm install

# Run development server
npm run dev

# Build for production
npm run build

# Start production server
npm start
```

The website will be available at **http://localhost:3000**

## 📁 Project Structure

```
website/
├── app/                    # Next.js app directory
│   ├── layout.tsx         # Root layout
│   ├── page.tsx           # Home page
│   └── globals.css        # Global styles
├── components/            # React components
│   ├── Header.tsx         # Navigation header
│   ├── Hero.tsx           # Hero section
│   ├── FeatureGrid.tsx    # Feature cards
│   ├── ContactSection.tsx # Contact info
│   ├── Footer.tsx         # Footer
│   └── BackToWebAppButton.tsx  # Admin navigation
├── public/                # Static assets
└── package.json           # Dependencies
```

## 🎨 Features

### Phase 0 (Current)
- ✅ Ultra-modern, premium design
- ✅ Fully responsive (mobile/tablet/desktop)
- ✅ Smooth animations with Framer Motion
- ✅ Gradient backgrounds and glassmorphism effects
- ✅ Content from current gloucesterfraternityclub.com
- ✅ Navigation to/from GFC Web App
- ✅ SEO-friendly structure

### Future Phases
- 🔮 Studio integration for content editing
- 🔮 Database-driven content
- 🔮 Event calendar
- 🔮 Hall rental application forms
- 🔮 Photo gallery with uploads
- 🔮 Member portal integration

## 🔗 Integration with GFC Web App

### Navigation Flow
- **Web App → Website**: Click "Public Website" in the Web App navigation
- **Website → Web App**: Click the floating "Back to Admin" button (bottom right)

### URLs
- **Website**: http://localhost:3000
- **Web App**: http://localhost:5207

## 🎨 Design System

### Colors
- **Primary**: Deep Blue (#1e3a8a) - Trust, tradition
- **Secondary**: Gold (#f59e0b) - Warmth, community
- **Accent**: Teal (#0d9488) - Modern, fresh

### Typography
- **Headings**: Outfit (Google Fonts)
- **Body**: Inter (Google Fonts)

### Components
- Premium button system with hover effects
- Card components with elevation
- Responsive grid layouts
- Animated hero sections
- Glassmorphism effects

## 📱 Responsive Breakpoints

- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

## 🛠️ Technology Stack

- **Framework**: Next.js 14+ (App Router)
- **Language**: TypeScript
- **Styling**: Vanilla CSS with CSS Modules
- **Animations**: Framer Motion
- **Icons**: Lucide React
- **Fonts**: Google Fonts (Inter, Outfit)

## 📝 Content

Content is currently static and based on the live gloucesterfraternityclub.com website. Future phases will integrate with GFC Studio for dynamic content management.

## 🚧 Development Notes

### Phase 0 Scope
This is a **proof-of-concept** implementation focused on:
1. Modern, premium UI/UX
2. Responsive design
3. Bi-directional navigation with Web App
4. Foundation for future Studio integration

### Not Included in Phase 0
- Content management system
- Database integration
- Form submissions
- User authentication
- Real-time data
- Production deployment

## 📄 License

© 2025 Gloucester Fraternity Club. All rights reserved.

---

**Built with ❤️ for the GFC community**
