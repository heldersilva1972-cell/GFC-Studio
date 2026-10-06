package com.gfc.connect.ui

import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.Bundle
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
import com.google.android.material.dialog.MaterialAlertDialogBuilder

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

        binding.btnTestBiometrics.setOnClickListener {
            if (!canAuth) {
                Toast.makeText(this, "⚠️ Please enroll a fingerprint or screen lock in device Settings first.", Toast.LENGTH_LONG).show()
                return@setOnClickListener
            }

            biometricManager.promptBiometricAuthentication(
                onSuccess = {
                    Toast.makeText(this, "✅ Biometric authentication successful!", Toast.LENGTH_LONG).show()
                },
                onError = { code, err ->
                    Toast.makeText(this, "❌ Biometric verification error ($code): $err", Toast.LENGTH_LONG).show()
                },
                onFailed = {
                    Toast.makeText(this, "⚠️ Biometric not recognized. Please try again.", Toast.LENGTH_SHORT).show()
                }
            )
        }
    }

    private fun bindAlertSettings() {
        // Switch bindings
        binding.switchAlertRentals.isChecked = appSettings.alertRentalInquiries
        binding.switchAlertRentals.setOnCheckedChangeListener { _, isChecked ->
            appSettings.alertRentalInquiries = isChecked
        }

        binding.switchAlertPayments.isChecked = appSettings.alertRentalPayments
        binding.switchAlertPayments.setOnCheckedChangeListener { _, isChecked ->
            appSettings.alertRentalPayments = isChecked
        }

        binding.switchAlertClubEvents.isChecked = appSettings.alertClubEvents
        binding.switchAlertClubEvents.setOnCheckedChangeListener { _, isChecked ->
            appSettings.alertClubEvents = isChecked
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

        binding.rowUnpairDevice.setOnClickListener {
            MaterialAlertDialogBuilder(this)
                .setTitle("Unpair & Reset Device?")
                .setMessage("This will disconnect this device from your club account and remove local encrypted credentials. You will need a new 8-digit setup code to pair again.")
                .setPositiveButton("Unpair Device") { _, _ ->
                    tokenStorage.clearAll()
                    appSettings.resetToDefaults()
                    Toast.makeText(this, "Device unpaired successfully.", Toast.LENGTH_LONG).show()

                    val intent = Intent(this, MainActivity::class.java).apply {
                        flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                    }
                    startActivity(intent)
                    finish()
                }
                .setNegativeButton("Cancel", null)
                .show()
        }
    }
}
