# Camera System - What to Commit to GitHub

**Purpose:** Clear guide on what's safe vs unsafe to commit  
**Last Updated:** 2025-12-21

---

## ✅ SAFE to Commit (Documentation & Templates)

### **Documentation Files:**
- ✅ `CAMERA_*.md` - All planning and setup guides
- ✅ `CAMERA_DDNS_CONFIG.md` - DDNS configuration (no secrets)
- ✅ `CAMERA_IP_ADDRESS_PLAN.md` - IP addressing plan
- ✅ `CAMERA_ARCHITECTURE_DIAGRAM.md` - System architecture

### **Template Files:**
- ✅ `update-duckdns.TEMPLATE.ps1` - Script template with placeholders
- ✅ `appsettings.TEMPLATE.json` - Config template with placeholders
- ✅ `wg0.TEMPLATE.conf` - VPN config template with placeholders

### **Configuration Info (Non-Secret):**
- ✅ Hostname: `yourclub.duckdns.org`
- ✅ Subdomain: `yourclub`
- ✅ Local IPs: `192.168.1.200`, `192.168.1.64`
- ✅ VPN network: `10.0.0.0/24`
- ✅ Ports: `51820`, `5100`, `554`, `8000`

---

## ❌ NEVER Commit (Secrets & Keys)

### **DDNS Secrets:**
- ❌ DuckDNS token
- ❌ `update-duckdns.ps1` (actual script with token)
- ❌ Any file with "token" in the name

### **VPN Keys:**
- ❌ `server_private.key`
- ❌ `client_private.key`
- ❌ `webapp_private.key`
- ❌ `wg0.conf` (actual config with private key)
- ❌ `webapp-client.conf` (actual config with private key)
- ❌ Any `*.key` files
- ❌ Any `*.conf` files (unless they're templates)

### **Credentials:**
- ❌ NVR username/password
- ❌ `credentials.dat`
- ❌ `appsettings.json` (contains NVR password)
- ❌ API keys
- ❌ Any password files

### **Runtime Data:**
- ❌ Log files (`*.log`)
- ❌ Stream files (`*.ts`, `*.m3u8`)
- ❌ `logs/` folder
- ❌ `streams/` folder

---

## 🛡️ Protection Strategy

### **1. Use .gitignore**

**Merge `.gitignore-camera-secrets` into your main `.gitignore`:**

```bash
# Add camera secrets to main .gitignore
cat .gitignore-camera-secrets >> .gitignore
```

**Or manually add these patterns:**
```gitignore
# Camera System Secrets
**/update-duckdns.ps1
**/*_private.key
**/wg0.conf
**/webapp-client.conf
**/credentials.dat
**/VideoAgent/appsettings.json
**/logs/
**/*.log
```

---

### **2. Use Template Files**

**For any config with secrets:**

1. Create template version: `filename.TEMPLATE.ext`
2. Replace secrets with placeholders: `YOUR_TOKEN_HERE`
3. Commit template only
4. Add actual file to `.gitignore`

**Example:**
```
✅ Commit: update-duckdns.TEMPLATE.ps1
❌ Ignore: update-duckdns.ps1
```

---

### **3. Document Non-Secret Info**

**In `CAMERA_DDNS_CONFIG.md`, store:**
- ✅ Hostname
- ✅ Subdomain
- ✅ Provider name
- ✅ Account email (optional)
- ✅ File locations
- ✅ Recovery procedures

**DO NOT store:**
- ❌ Actual token
- ❌ Private keys
- ❌ Passwords

---

## 🚨 If You Accidentally Commit a Secret

### **Immediate Actions:**

1. **Rotate the secret immediately:**
   ```powershell
   # For DDNS: Create new subdomain on DuckDNS
   # For VPN: Generate new keys
   # For passwords: Change them
   ```

2. **Remove from Git history:**
   ```bash
   # Using BFG Repo-Cleaner (recommended)
   java -jar bfg.jar --delete-files update-duckdns.ps1
   git reflog expire --expire=now --all
   git gc --prune=now --aggressive
   
   # Or using git filter-branch
   git filter-branch --force --index-filter \
     "git rm --cached --ignore-unmatch path/to/secret-file" \
     --prune-empty --tag-name-filter cat -- --all
   ```

3. **Force push:**
   ```bash
   git push origin --force --all
   git push origin --force --tags
   ```

4. **Update all systems with new secret**

---

## 📋 Pre-Commit Checklist

**Before every commit, verify:**

- [ ] No `*.key` files
- [ ] No `*.conf` files (unless `.TEMPLATE.conf`)
- [ ] No `appsettings.json` (unless `.TEMPLATE.json`)
- [ ] No files with "token" or "password" in name
- [ ] No log files
- [ ] Only documentation and templates

**Quick check:**
```bash
git status
# Review list carefully before committing!
```

---

## 🔄 Recovery from GitHub

**What you CAN recover from GitHub if you lose local files:**

✅ **Documentation:**
- All setup guides
- IP address plan
- Architecture diagrams
- DDNS configuration (non-secret parts)

✅ **Templates:**
- Script templates
- Config templates
- You'll need to fill in secrets manually

❌ **What you CANNOT recover:**
- Actual secrets (tokens, keys, passwords)
- You'll need to regenerate these

---

## 📊 File Organization

```
GFC-Studio V2/
├── docs/
│   ├── in-process/
│   │   ├── CAMERA_*.md              ✅ Commit
│   │   └── CAMERA_DDNS_CONFIG.md    ✅ Commit
│   └── templates/
│       ├── update-duckdns.TEMPLATE.ps1  ✅ Commit
│       └── appsettings.TEMPLATE.json    ✅ Commit
├── .gitignore                       ✅ Commit (with camera secrets)
└── .gitignore-camera-secrets        ✅ Commit (reference)

Video Agent PC (NOT in Git):
C:\Scripts\
├── update-duckdns.ps1               ❌ Local only
└── duckdns-update.log               ❌ Local only

C:\Program Files\WireGuard\
├── wg0.conf                         ❌ Local only
├── server_private.key               ❌ Local only
└── webapp_private.key               ❌ Local only

C:\GFC\VideoAgent\
├── appsettings.json                 ❌ Local only
├── credentials.dat                  ❌ Local only
└── logs/                            ❌ Local only
```

---

## ✅ Summary

**Commit to GitHub:**
- 📄 Documentation
- 📋 Templates with placeholders
- 🗺️ Architecture and plans
- 📊 Non-secret configuration info

**Keep Local Only:**
- 🔑 Tokens and keys
- 🔐 Passwords and credentials
- 📝 Logs and runtime data
- ⚙️ Actual config files with secrets

**Golden Rule:**
> If it has a secret, it stays local. If it's documentation or a template, it goes to GitHub.

---

**This approach gives you:**
- ✅ Full documentation in GitHub for recovery
- ✅ Templates for quick setup
- ✅ No secrets exposed
- ✅ Easy to rebuild if needed
