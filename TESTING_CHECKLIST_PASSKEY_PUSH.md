# Post-Migration Testing Checklist

## ✅ Database Migration Complete

The following tables should now exist:
- `UserNotificationPreferences` - Stores user notification channel preferences (Email, SMS, Push)
- `PushSubscriptions` - Stores Web Push subscription endpoints

---

## 🧪 Testing Steps

### 1. **Test Passkey Registration** (My Security Page)
1. Navigate to `/my-security`
2. Click "Add New Passkey" in the Passkeys section
3. Enter a friendly name (e.g., "Windows Hello", "iPhone FaceID")
4. Click "Register Passkey"
5. Complete the browser's biometric prompt
6. ✅ Verify the passkey appears in the list

### 2. **Test Passkey Login** (Login Page)
1. Log out of the application
2. Navigate to `/login`
3. Click the **"Passkey"** tab
4. Enter your username
5. Click "Use Passkey"
6. Complete the biometric authentication
7. ✅ Verify successful login and redirect to dashboard

### 3. **Test Push Notification Subscription** (My Security Page)
1. Navigate to `/my-security`
2. Scroll to "Mobile & Browser Notifications" section
3. Click "Enable Push"
4. Grant notification permission when prompted
5. ✅ Verify status changes to "Active"
6. Check browser DevTools > Application > Service Workers (should be registered)

### 4. **Test Notification Preferences** (Admin Page)
1. Navigate to `/admin/notification-preferences`
2. Select a user to expand their preferences
3. Toggle Email, SMS, and/or Push for different event types
4. Click "Save Preferences"
5. ✅ Verify success message appears
6. Refresh page and confirm settings persist

### 5. **Test Device Management**
1. Navigate to `/my-security`
2. View "Trusted Devices" section
3. Click "Sign Out of All Devices"
4. Confirm the prompt
5. ✅ Verify you're logged out and redirected to login

### 6. **Test Audit Logging**
1. Perform a Passkey login
2. Register a new Passkey
3. Revoke a Passkey
4. Navigate to audit logs (if accessible)
5. ✅ Verify these actions appear:
   - `LoginSuccessPasskey`
   - `PasskeyRegistered`
   - `PasskeyRevoked`

---

## 🔍 Verification Queries

Run these to verify data is being stored correctly:

```sql
-- Check if notification preferences exist
SELECT COUNT(*) as PreferenceCount FROM UserNotificationPreferences;

-- Check if push subscriptions exist
SELECT COUNT(*) as SubscriptionCount FROM PushSubscriptions;

-- View all passkeys
SELECT Id, UserId, FriendlyName, CreatedAtUtc FROM UserPasskeys;

-- View active device sessions
SELECT UserId, UserAgent, IpAddress, LastUsedUtc 
FROM TrustedDevices 
WHERE IsRevoked = 0 AND ExpiresAtUtc > GETUTCDATE();
```

---

## ⚠️ Known Limitations

1. **VAPID Key**: Using placeholder - real push notifications won't work until proper VAPID keys are configured
2. **HTTPS Required**: Push notifications require HTTPS in production
3. **Browser Support**: Passkeys require modern browsers (Chrome 67+, Edge 18+, Safari 13+)

---

## 🐛 Troubleshooting

### Passkey Registration Fails
- Ensure you're on HTTPS or localhost
- Check browser console for errors
- Verify FIDO2 service is configured in `Program.cs`

### Push Subscription Fails
- Verify Service Worker is registered (`/service-worker.js`)
- Check browser supports Push API
- Ensure notifications.js is loaded

### Login Page Doesn't Show Passkey Tab
- Clear browser cache
- Verify `Login.razor` was updated correctly
- Check for JavaScript errors in console

---

## 📊 Success Criteria

- ✅ Can register a Passkey from My Security page
- ✅ Can login using Passkey from Login page
- ✅ Can enable/disable Push notifications
- ✅ Admin can configure user notification preferences
- ✅ "Sign Out of All Devices" works efficiently
- ✅ All security actions are audit-logged

---

**Ready for Production?** 
- ⚠️ Generate real VAPID keys
- ⚠️ Configure HTTPS for production
- ⚠️ Test on mobile devices (iOS Safari, Android Chrome)
- ⚠️ Verify email/SMS integration (currently simulated)
