# 🚀 Quick Start Guide - GFC Website Phase 0

## Step-by-Step Instructions

### 1. Install Website Dependencies

Open a terminal and run:

```bash
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\website"
npm install
```

Wait for installation to complete (may take 2-3 minutes).

---

### 2. Start the Website

In the same terminal:

```bash
npm run dev
```

You should see:
```
▲ Next.js 14.x.x
- Local:        http://localhost:3000
✓ Ready in X.Xs
```

**Keep this terminal open!**

---

### 3. Start the Web App

Open a **NEW** terminal and run:

```bash
cd "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"
dotnet run
```

You should see:
```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shutdown.
```

**Keep this terminal open too!**

---

### 4. Test the Integration

#### From Web App → Website:
1. Open browser to http://localhost:5000
2. Log in to the Web App
3. Look for "Public Website" in the navigation (globe icon)
4. Click it → Website opens in new tab

#### From Website → Web App:
1. On the website (http://localhost:3000)
2. Look for floating "Back to Admin" button (bottom right)
3. Click it → Returns to Web App

---

## 🎨 What You'll See

### Website Home Page
- **Hero Section**: Gradient background with "Building Community..." headline
- **Stats**: 100+ Years, 500+ Members, 50+ Events
- **Feature Cards**: 6 cards for Events, Hall Rentals, Membership, etc.
- **Contact Section**: Address, phone, email, hours
- **Footer**: Links and social media

### Design Features
- Smooth animations on scroll
- Hover effects on cards and buttons
- Responsive mobile menu
- Premium gradient backgrounds
- Glassmorphism effects

---

## ⚠️ Troubleshooting

### Port Already in Use

**Website (Port 3000):**
```bash
npm run dev -- -p 3001
```
Then update Web App link to http://localhost:3001

**Web App (Port 5000):**
Edit `launchSettings.json` to use different port

### Dependencies Not Installing

Try:
```bash
npm cache clean --force
npm install
```

### Styles Not Loading

Hard refresh browser:
- Windows: `Ctrl + Shift + R`
- Mac: `Cmd + Shift + R`

### Website Won't Start

Check for errors in terminal. Common issues:
- Node.js version too old (need 18+)
- npm not installed
- Syntax errors in files

---

## 📱 Testing on Mobile

1. Find your computer's IP address:
   ```bash
   ipconfig
   ```
   Look for "IPv4 Address"

2. Start website with:
   ```bash
   npm run dev -- -H 0.0.0.0
   ```

3. On mobile browser, visit:
   ```
   http://YOUR_IP_ADDRESS:3000
   ```

---

## 🛑 Stopping the Applications

### Stop Website:
In the website terminal, press `Ctrl + C`

### Stop Web App:
In the Web App terminal, press `Ctrl + C`

---

## 📝 Quick Reference

| Application | URL | Command |
|-------------|-----|---------|
| Website | http://localhost:3000 | `npm run dev` |
| Web App | http://localhost:5000 | `dotnet run` |

---

## ✅ Success Checklist

- [ ] npm install completed without errors
- [ ] Website starts on port 3000
- [ ] Web App starts on port 5000
- [ ] Can navigate from Web App to Website
- [ ] Can navigate from Website to Web App
- [ ] Hero section displays correctly
- [ ] Feature cards render properly
- [ ] Mobile menu works
- [ ] Animations are smooth

---

## 🎯 Next Steps After Testing

1. Review the design and provide feedback
2. Test on different browsers (Chrome, Edge, Firefox)
3. Test on mobile devices
4. Check the implementation plan for future phases
5. Decide on additional pages to add

---

**Need Help?** Check the full documentation:
- Implementation Plan: `docs/PHASE_0_GFC_WEBSITE_IMPLEMENTATION_PLAN.md`
- Summary: `docs/PHASE_0_GFC_WEBSITE_SUMMARY.md`
- Website README: `apps/website/README.md`

---

**Ready to start? Run the commands above! 🚀**
