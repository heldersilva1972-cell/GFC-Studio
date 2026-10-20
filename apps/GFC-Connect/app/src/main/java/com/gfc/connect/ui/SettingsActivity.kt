package com.gfc.connect.ui

import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.Bundle
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.NotificationCompat
import com.gfc.connect.BuildConfig
import com.gfc.connect.GfcConnectApp
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.cache.RentalCacheManager
import com.gfc.connect.databinding.ActivitySettingsBinding
import com.gfc.connect.security.BiometricAuthManager
import androidx.lifecycle.lifecycleScope
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.gfc.connect.data.models.RentalNotificationPreferencesDto
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class SettingsActivity : AppCompatActivity() {

    private lateinit var binding: ActivitySettingsBinding
    private val appSettings by lazy { GfcConnectApp.instance.appSettings }
    private val tokenStorage by lazy { GfcConnectApp.instance.tokenStorage }
    private lateinit var biometricManager: BiometricAuthManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySettingsBinding.inflate(layoutInflater)
        setContentView(binding.root)

        biometricManager = BiometricAuthManager(this)

        setupToolbar()
        bindAccountInfo()
        bindBiometricSettings()
        bindAlertSettings()
        bindSystemActions()
    }

    private fun setupToolbar() {
        binding.toolbarSettings.setNavigationOnClickListener {
            finish()
        }
    }

    private fun bindAccountInfo() {
        val memberName = tokenStorage.getMemberName()
        val username = tokenStorage.getUsername() ?: "Club Staff"
        val permissions = tokenStorage.getPermissions()

        val displayName = if (!memberName.isNullOrBlank()) memberName else username
        val roleDesc = when {
            permissions.any { it.pageRoute.contains("admin", ignoreCase = true) || it.pageRoute.contains("all", ignoreCase = true) } -> "Club Administrator • Active"
            permissions.any { it.pageRoute.contains("rental", ignoreCase = true) } -> "Hall Rental Manager • Active"
            else -> "Staff Member • Active"
        }

        binding.txtAccountName.text = displayName
        binding.txtAccountRole.text = roleDesc
        binding.txtServerEndpoint.text = ApiClient.getBaseUrl().trimEnd('/')
        binding.txtAppVersionTag.text = "v${BuildConfig.VERSION_NAME}"
        binding.txtBuildFooter.text = "GFC Connect Native Companion v${BuildConfig.VERSION_NAME} (Build ${BuildConfig.VERSION_CODE})\nGloucester Fraternity Club • All Rights Reserved"

        // Developer 5-tap gesture on version tag or build footer to switch server target
        var devTapCount = 0
        var lastDevTapTime = 0L
        val devClickListener = View.OnClickListener {
            val now = System.currentTimeMillis()
            if (now - lastDevTapTime < 2500) {
                devTapCount++
            } else {
                devTapCount = 1
            }
            lastDevTapTime = now

            if (devTapCount >= 5) {
                devTapCount = 0
                ApiClient.showServerSelectionDialog(this) {
                    binding.txtServerEndpoint.text = ApiClient.getBaseUrl().trimEnd('/')
                }
            } else if (devTapCount >= 3) {
                val remaining = 5 - devTapCount
                Toast.makeText(this, "Tap $remaining more times for server picker", Toast.LENGTH_SHORT).show()
            }
        }
        binding.txtAppVersionTag.setOnClickListener(devClickListener)
        binding.txtBuildFooter.setOnClickListener(devClickListener)
    }

    private fun bindBiometricSettings() {
        val canAuth = biometricManager.canAuthenticate()
        val hardwareAvailable = biometricManager.isHardwareAvailable()

        binding.txtBiometricStatus.text = when {
            canAuth -> "Fingerprint & Face Unlock active on device"
            hardwareAvailable -> "Hardware sensor detected. Set up fingerprint in Android Settings"
            else -> "Biometric hardware unavailable on this device"
        }

        binding.switchBiometricAppLock.isChecked = appSettings.biometricAppLockEnabled && canAuth
        binding.switchBiometricAppLock.isEnabled = canAuth

        binding.switchBiometricAppLock.setOnCheckedChangeListener { _, isChecked ->
            appSettings.biometricAppLockEnabled = isChecked
            tokenStorage.setBiometricEnabled(isChecked)
            val stateText = if (isChecked) "enabled" else "disabled"
            Toast.makeText(this, "Biometric App Lock $stateText", Toast.LENGTH_SHORT).show()
        }

        binding.switchBiometricSensitive.isChecked = appSettings.biometricRequireForSensitiveActions && canAuth
        binding.switchBiometricSensitive.isEnabled = canAuth

        binding.switchBiometricSensitive.setOnCheckedChangeListener { _, isChecked ->
            appSettings.biometricRequireForSensitiveActions = isChecked
            val stateText = if (isChecked) "enabled" else "disabled"
            Toast.makeText(this, "Sensitive action biometric confirmation $stateText", Toast.LENGTH_SHORT).show()
        }
    }

    private fun bindAlertSettings() {
        // Prevent listeners from firing auto-saves during initial UI setup
        _isUpdatingPrefs = true

        // Initial switch bindings from local settings
        binding.switchAlertRentals.isChecked = appSettings.alertRentalNewSubmissions
        binding.switchRentalInquiryPush.isChecked = appSettings.alertRentalInquiries
        binding.switchAlertPayments.isChecked = appSettings.alertRentalPayments
        binding.switchRentalReplyPush.isChecked = appSettings.alertRentalReplies

        // Explicit Email Save Action
        binding.btnSaveRentalEmail.setOnClickListener {
            saveRentalNotificationPreferences()
        }

        // Email Alert Checkboxes with strict validation: Empty email is blocked
        val emailCheckListener = android.widget.CompoundButton.OnCheckedChangeListener { buttonView, isChecked ->
            if (_isUpdatingPrefs) return@OnCheckedChangeListener
            if (isChecked) {
                val email = binding.editRentalNotifyEmail.text?.toString()?.trim()
                if (email.isNullOrBlank()) {
                    buttonView.isChecked = false
                    Toast.makeText(this, "⚠️ Email address is required to enable email notifications. Please enter your email address first.", Toast.LENGTH_LONG).show()
                    return@OnCheckedChangeListener
                }
            }
            saveRentalNotificationPreferences()
        }

        binding.chkRentalNewEmail.setOnCheckedChangeListener(emailCheckListener)
        binding.chkRentalInquiryEmail.setOnCheckedChangeListener(emailCheckListener)
        binding.chkRentalPaymentEmail.setOnCheckedChangeListener(emailCheckListener)
        binding.chkRentalReplyEmail.setOnCheckedChangeListener(emailCheckListener)

        // Switch bindings for granular rental push alerts
        binding.switchAlertRentals.setOnCheckedChangeListener { _, isChecked ->
            if (_isUpdatingPrefs) return@setOnCheckedChangeListener
            appSettings.alertRentalNewSubmissions = isChecked
            saveRentalNotificationPreferences()
        }

        binding.switchRentalInquiryPush.setOnCheckedChangeListener { _, isChecked ->
            if (_isUpdatingPrefs) return@setOnCheckedChangeListener
            appSettings.alertRentalInquiries = isChecked
            saveRentalNotificationPreferences()
        }

        binding.switchAlertPayments.setOnCheckedChangeListener { _, isChecked ->
            if (_isUpdatingPrefs) return@setOnCheckedChangeListener
            appSettings.alertRentalPayments = isChecked
            saveRentalNotificationPreferences()
        }

        binding.switchRentalReplyPush.setOnCheckedChangeListener { _, isChecked ->
            if (_isUpdatingPrefs) return@setOnCheckedChangeListener
            appSettings.alertRentalReplies = isChecked
            saveRentalNotificationPreferences()
        }

        // Lottery Shift Variance Alert & Threshold
        binding.switchAlertLotteryVariance.isChecked = appSettings.alertLotteryVariance
        binding.layoutLotteryThresholdConfig.visibility = if (appSettings.alertLotteryVariance) View.VISIBLE else View.GONE
        binding.editLotteryThreshold.setText(String.format(java.util.Locale.US, "%.2f", appSettings.lotteryVarianceThreshold))
        binding.chkLotteryQuietHours.isChecked = appSettings.lotteryQuietHoursEnabled

        binding.switchAlertLotteryVariance.setOnCheckedChangeListener { _, isChecked ->
            appSettings.alertLotteryVariance = isChecked
            binding.layoutLotteryThresholdConfig.visibility = if (isChecked) View.VISIBLE else View.GONE
        }

        binding.editLotteryThreshold.addTextChangedListener(object : android.text.TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {}
            override fun afterTextChanged(s: android.text.Editable?) {
                val value = s?.toString()?.toFloatOrNull()
                if (value != null && value > 0f) {
                    appSettings.lotteryVarianceThreshold = value
                }
            }
        })

        binding.chkLotteryQuietHours.setOnCheckedChangeListener { _, isChecked ->
            appSettings.lotteryQuietHoursEnabled = isChecked
        }

        // Sound & Vibration Checkboxes
        binding.chkAlertSound.isChecked = appSettings.alertSoundEnabled
        binding.chkAlertSound.setOnCheckedChangeListener { _, isChecked ->
            appSettings.alertSoundEnabled = isChecked
        }

        binding.chkAlertVibrate.isChecked = appSettings.alertVibrateEnabled
        binding.chkAlertVibrate.setOnCheckedChangeListener { _, isChecked ->
            appSettings.alertVibrateEnabled = isChecked
        }

        // Test Push Notification Action
        binding.btnTestNotification.setOnClickListener {
            sendTestNotification()
        }

        _isUpdatingPrefs = false

        // Fetch explicit server preferences for rental notifications
        loadRentalNotificationPreferences()
    }

    private var _isUpdatingPrefs = false

    private fun loadRentalNotificationPreferences() {
        // Pre-populate with local cache if present
        if (appSettings.rentalNotifyEmail.isNotBlank()) {
            binding.editRentalNotifyEmail.setText(appSettings.rentalNotifyEmail)
        }

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val resp = ApiClient.service.getRentalNotificationPreferences()
                if (resp.isSuccessful && resp.body() != null) {
                    val prefs = resp.body()!!
                    withContext(Dispatchers.Main) {
                        _isUpdatingPrefs = true
                        val effectiveEmail = if (!prefs.email.isNullOrBlank()) prefs.email else appSettings.rentalNotifyEmail
                        binding.editRentalNotifyEmail.setText(effectiveEmail)
                        if (!prefs.email.isNullOrBlank()) {
                            appSettings.rentalNotifyEmail = prefs.email
                        }
                        binding.chkRentalNewEmail.isChecked = prefs.rentalNewSubmissionEmail
                        binding.switchAlertRentals.isChecked = prefs.rentalNewSubmissionPush
                        appSettings.alertRentalNewSubmissions = prefs.rentalNewSubmissionPush

                        binding.chkRentalInquiryEmail.isChecked = prefs.rentalInquiryEmail
                        binding.switchRentalInquiryPush.isChecked = prefs.rentalInquiryPush
                        appSettings.alertRentalInquiries = prefs.rentalInquiryPush

                        binding.chkRentalPaymentEmail.isChecked = prefs.rentalPaymentEmail
                        binding.switchAlertPayments.isChecked = prefs.rentalPaymentPush
                        appSettings.alertRentalPayments = prefs.rentalPaymentPush

                        binding.chkRentalReplyEmail.isChecked = prefs.rentalReplyEmail
                        binding.switchRentalReplyPush.isChecked = prefs.rentalReplyPush
                        appSettings.alertRentalReplies = prefs.rentalReplyPush
                        _isUpdatingPrefs = false
                    }
                }
            } catch (ex: Exception) {
                // Silently keep local defaults if offline
            }
        }
    }

    private fun saveRentalNotificationPreferences() {
        if (_isUpdatingPrefs) return

        val email = binding.editRentalNotifyEmail.text?.toString()?.trim() ?: ""
        val wantsNewEmail = binding.chkRentalNewEmail.isChecked
        val wantsInqEmail = binding.chkRentalInquiryEmail.isChecked
        val wantsPayEmail = binding.chkRentalPaymentEmail.isChecked
        val wantsRepEmail = binding.chkRentalReplyEmail.isChecked

        // Strict Check: If any email alert is checked and email is empty, revert and warn
        if ((wantsNewEmail || wantsInqEmail || wantsPayEmail || wantsRepEmail) && email.isBlank()) {
            Toast.makeText(this, "⚠️ Please enter your email address to receive rental email alerts.", Toast.LENGTH_LONG).show()
            binding.chkRentalNewEmail.isChecked = false
            binding.chkRentalInquiryEmail.isChecked = false
            binding.chkRentalPaymentEmail.isChecked = false
            binding.chkRentalReplyEmail.isChecked = false
            return
        }

        // Save email locally immediately so user input is never lost
        if (email.isNotBlank()) {
            appSettings.rentalNotifyEmail = email
        }

        val dto = RentalNotificationPreferencesDto(
            email = email,
            rentalNewSubmissionEmail = wantsNewEmail,
            rentalNewSubmissionPush = binding.switchAlertRentals.isChecked,
            rentalInquiryEmail = wantsInqEmail,
            rentalInquiryPush = binding.switchRentalInquiryPush.isChecked,
            rentalPaymentEmail = wantsPayEmail,
            rentalPaymentPush = binding.switchAlertPayments.isChecked,
            rentalReplyEmail = wantsRepEmail,
            rentalReplyPush = binding.switchRentalReplyPush.isChecked
        )

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val resp = ApiClient.service.updateRentalNotificationPreferences(dto)
                withContext(Dispatchers.Main) {
                    if (resp.isSuccessful) {
                        Toast.makeText(this@SettingsActivity, "✅ Notification preferences saved", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(this@SettingsActivity, "⚠️ Saved locally (Server sync issue)", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (ex: Exception) {
                withContext(Dispatchers.Main) {
                    Toast.makeText(this@SettingsActivity, "⚠️ Saved locally (Offline)", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun sendTestNotification() {
        val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

        val intent = Intent(this, MainActivity::class.java).apply {
            flags = Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_SINGLE_TOP
        }

        val pendingIntent = PendingIntent.getActivity(
            this,
            9999,
            intent,
            PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
        )

        val builder = NotificationCompat.Builder(this, GfcConnectApp.CHANNEL_URGENT_ALERTS)
            .setSmallIcon(R.drawable.ic_shield_badge)
            .setContentTitle("🔔 GFC Connect Alert Test")
            .setContentText("Notifications and alerts are operating correctly for Gloucester Fraternity Club.")
            .setStyle(NotificationCompat.BigTextStyle().bigText("🔔 Notifications and alerts are operating correctly for Gloucester Fraternity Club. Sound: ${if (appSettings.alertSoundEnabled) "ON" else "OFF"}, Vibrate: ${if (appSettings.alertVibrateEnabled) "ON" else "OFF"}."))
            .setPriority(NotificationCompat.PRIORITY_HIGH)
            .setAutoCancel(true)
            .setContentIntent(pendingIntent)

        if (!appSettings.alertSoundEnabled) {
            builder.setSilent(true)
        }
        if (!appSettings.alertVibrateEnabled) {
            builder.setVibrate(longArrayOf(0))
        }

        notificationManager.notify(System.currentTimeMillis().toInt(), builder.build())
        Toast.makeText(this, "🔔 Test notification sent to notification drawer!", Toast.LENGTH_SHORT).show()
    }

    private fun bindSystemActions() {
        binding.rowClearCache.setOnClickListener {
            val cacheManager = RentalCacheManager(this)
            cacheManager.clearCache()
            Toast.makeText(this, "🧹 Offline rental cache cleared successfully.", Toast.LENGTH_SHORT).show()
        }

        binding.rowCheckUpdates.setOnClickListener {
            Toast.makeText(this, "Checking for latest GFC Connect companion update...", Toast.LENGTH_SHORT).show()
            // Launch main update check
            val intent = Intent(this, MainActivity::class.java).apply {
                putExtra("EXTRA_CHECK_UPDATE", true)
            }
            startActivity(intent)
        }
    }
}
