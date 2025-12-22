# QUICK SYNC GUIDE - After Jules Merges a PR

## 🚨 EVERY TIME Jules completes work and you merge the PR:

**Run these 3 commands:**

```powershell
git checkout master
git fetch origin
git reset --hard origin/master
```

**That's it!** Your local files will now match what's on GitHub.

---

## Why This Happens

Jules works on **feature branches** like:
- `feat/camera-system-phase-0-1997011709796585676`
- `feat/camera-system-phase-2-video-streaming-6163926092052941278`

When you merge Jules's PR on GitHub:
1. ✅ The code goes into `master` on GitHub
2. ❌ Your local repository is still on the feature branch
3. ❌ You don't see the new files

**The fix:** Switch to master and sync with GitHub.

---

## The 3-Command Fix (Copy & Paste)

```powershell
git checkout master && git fetch origin && git reset --hard origin/master
```

**Bookmark this file!** You'll need it after every Jules PR merge.

---

## Alternative: Use GitHub Desktop

If you prefer a GUI:
1. Open GitHub Desktop
2. Switch to `master` branch
3. Click "Fetch origin"
4. Click "Pull origin"

---

## Prevent Future Issues

**Before starting new work:**
1. Always check you're on `master`: `git branch`
2. Always sync first: `git pull origin master`
3. Then create your feature branch if needed

---

**Save this file to your desktop for quick access!**
