package com.gfc.connect.ui

import android.Manifest
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.view.View
import android.view.inputmethod.InputMethodManager
import android.widget.EditText
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.lifecycle.lifecycleScope
import com.gfc.connect.BuildConfig
import com.gfc.connect.GfcConnectApp
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.models.DeviceRegistrationPayload
import com.gfc.connect.data.models.SetupCodeRequest
import com.gfc.connect.data.models.VersionResponse
import com.gfc.connect.databinding.ActivityMainBinding
import com.gfc.connect.security.BiometricAuthManager
import com.gfc.connect.update.AppUpdateManager
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.firebase.messaging.FirebaseMessaging
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding
    private lateinit var biometricManager: BiometricAuthManager
    private lateinit var updateManager: AppUpdateManager

    private var availableUpdate: VersionResponse? = null

    // Android 13+ Notification Permission Launcher
    private val requestNotificationPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { isGranted ->
        if (isGranted) {
            syncFcmToken()
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        biometricManager = BiometricAuthManager(this)
        updateManager = AppUpdateManager(this)
        ApiClient.initBaseUrl(this)

        setupListeners()
        val isDeepLinking = handleDeepLink(intent) || handleNotificationIntent(intent)
        if (!isDeepLinking) {
            evaluateAppState()
        }
    }

    override fun onNewIntent(intent: Intent) {
        super.onNewIntent(intent)
        setIntent(intent)
        if (!handleDeepLink(intent)) {
            handleNotificationIntent(intent)
        }
    }

    private fun handleNotificationIntent(intent: Intent?): Boolean {
        if (intent == null) return false
        val category = intent.getStringExtra("fcm_category")?.lowercase() ?: ""
        if (category.isEmpty()) return false

        if (category.contains("lottery")) {
            val target = Intent(this, LotteryVarianceActivity::class.java)
            startActivity(target)
            return true
        }

        if (category.contains("rental") || category.contains("inquiry")) {
            val rentalId = intent.getStringExtra("fcm_rental_id")?.toIntOrNull()
            if (rentalId != null && rentalId > 0) {
                val target = Intent(this, RentalDetailActivity::class.java).apply {
                    putExtra(RentalDetailActivity.EXTRA_RENTAL_ID, rentalId)
                    putExtra(RentalDetailActivity.EXTRA_IS_INQUIRY, category.contains("inquiry"))
                }
                startActivity(target)
                return true
            } else {
                val target = Intent(this, HallRentalsActivity::class.java)
                startActivity(target)
                return true
            }
        }
        return false
    }

    private fun handleDeepLink(intent: Intent?): Boolean {
        val data = intent?.data ?: return false
        if (data.scheme.equals("gfcconnect", ignoreCase = true) && data.host.equals("pair", ignoreCase = true)) {
            val code = data.getQueryParameter("code")
            if (!code.isNullOrBlank()) {
                val clean = code.filter { it.isDigit() }
                if (clean.length == 8) {
                    val formatted = "${clean.substring(0, 4)}-${clean.substring(4, 8)}"
                    binding.editSetupCode.setText(formatted)
                    pairWithSetupCode(formatted)
                    return true
                }
            }
        }
        return false
    }

    override fun onResume() {
        super.onResume()
        if (GfcConnectApp.instance.tokenStorage.isPaired()) {
            checkActiveLotteryVariances()
            checkRentalPendingCount()
            checkForAppUpdates()
        }
    }

    private fun setupListeners() {
        // Auto-format 8-digit code as XXXX-XXXX while typing on numeric keypad
        binding.editSetupCode.addTextChangedListener(object : android.text.TextWatcher {
            private var isFormatting = false

            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {}
            override fun afterTextChanged(s: android.text.Editable?) {
                if (isFormatting || s == null) return
                isFormatting = true

                val digits = s.toString().filter { it.isDigit() }
                val formatted = StringBuilder()
                for (i in digits.indices) {
                    if (i == 4) formatted.append("-")
                    formatted.append(digits[i])
                    if (i >= 7) break // Cap at 8 digits
                }

                s.replace(0, s.length, formatted.toString())
                isFormatting = false

                // Show clear button when input has text
                binding.btnClearCode.visibility = if (digits.isNotEmpty()) View.VISIBLE else View.GONE

                // Auto-clear error when user types
                binding.txtOnboardingError.visibility = View.GONE

                // Close soft keyboard immediately when the 8th digit is entered
                if (digits.length == 8) {
                    val imm = getSystemService(Context.INPUT_METHOD_SERVICE) as? InputMethodManager
                    imm?.hideSoftInputFromWindow(binding.editSetupCode.windowToken, 0)
                    binding.editSetupCode.clearFocus()
                }
            }
        })

        // 1-Tap Clear Button
        binding.btnClearCode.setOnClickListener {
            binding.editSetupCode.setText("")
            binding.txtOnboardingError.visibility = View.GONE
            binding.editSetupCode.requestFocus()
            val imm = getSystemService(Context.INPUT_METHOD_SERVICE) as? InputMethodManager
            imm?.showSoftInput(binding.editSetupCode, InputMethodManager.SHOW_IMPLICIT)
        }

        // Auto-select text on tap so typing replaces existing code
        binding.editSetupCode.setOnClickListener {
            binding.editSetupCode.selectAll()
        }

        // Pairing / Onboarding Button
        binding.btnVerifyCode.setOnClickListener {
            val rawCode = binding.editSetupCode.text.toString().trim()
            val cleanCode = rawCode.filter { it.isDigit() }
            if (cleanCode.length == 8) {
                pairWithSetupCode(rawCode)
            } else {
                binding.txtOnboardingError.text = "Please enter the full 8-digit setup code."
                binding.txtOnboardingError.visibility = View.VISIBLE
            }
        }

        // Server Endpoint Switcher
        binding.btnToggleServer.visibility = if (BuildConfig.DEBUG) View.VISIBLE else View.GONE
        updateServerButtonLabel()
        binding.btnToggleServer.setOnClickListener {
            ApiClient.showServerSelectionDialog(this) {
                updateServerButtonLabel()
            }
        }

        // Hidden 5-tap gesture on badge to reveal server picker in any build
        var badgeTapCount = 0
        var lastBadgeTapTime = 0L
        binding.imgOnboardingBadge.setOnClickListener {
            val now = System.currentTimeMillis()
            if (now - lastBadgeTapTime < 2500) {
                badgeTapCount++
            } else {
                badgeTapCount = 1
            }
            lastBadgeTapTime = now

            if (badgeTapCount >= 5) {
                badgeTapCount = 0
                binding.btnToggleServer.visibility = View.VISIBLE
                ApiClient.showServerSelectionDialog(this) {
                    updateServerButtonLabel()
                }
            } else if (badgeTapCount >= 3) {
                val remaining = 5 - badgeTapCount
                Toast.makeText(this, "Tap $remaining more times for server picker", Toast.LENGTH_SHORT).show()
            }
        }

        // Biometric Retry Button
        binding.btnRetryBiometric.setOnClickListener {
            promptBiometrics()
        }

        // In-App Update Apply Button
        binding.btnApplyUpdate.setOnClickListener {
            startInAppUpdate()
        }

        // Feature Card Click Handlers
        binding.cardHallRentals.setOnClickListener {
            val intent = android.content.Intent(this, HallRentalsActivity::class.java)
            startActivity(intent)
        }

        binding.cardRentalCalendar.setOnClickListener {
            val intent = Intent(this, RentalCalendarActivity::class.java).apply {
                putExtra(RentalCalendarActivity.EXTRA_IS_VIEW_ONLY, true)
            }
            startActivity(intent)
        }

        binding.cardLotteryVariance.setOnClickListener {
            val intent = Intent(this, LotteryVarianceActivity::class.java)
            startActivity(intent)
        }

        binding.btnSettings.setOnClickListener {
            val intent = Intent(this, SettingsActivity::class.java)
            startActivity(intent)
        }
    }

    private fun evaluateAppState() {
        val storage = GfcConnectApp.instance.tokenStorage
        val appSettings = GfcConnectApp.instance.appSettings

        val expiredReason = intent.getStringExtra("AUTH_EXPIRED_REASON")
        if (!expiredReason.isNullOrEmpty()) {
            showView(OnboardingState.UNPAIRED)
            binding.txtOnboardingError.text = expiredReason
            binding.txtOnboardingError.visibility = View.VISIBLE
            return
        }

        when {
            // State 1: Device Not Yet Paired -> Show 6-digit Setup Code screen
            !storage.isPaired() -> {
                showView(OnboardingState.UNPAIRED)
            }

            // State 2: Device Paired & Biometrics Available -> Lock screen & prompt Biometrics
            storage.isBiometricEnabled() && appSettings.biometricAppLockEnabled && biometricManager.canAuthenticate() -> {
                showView(OnboardingState.BIOMETRIC_LOCKED)
                binding.txtBiometricUserGreeting.text = "Welcome back, ${storage.getMemberName() ?: storage.getUsername() ?: "Member"}.\nTouch fingerprint sensor to unlock."
                promptBiometrics()
            }

            // State 3: Device Paired & Biometrics Not Required / Fallback -> Direct to Main Hub
            else -> {
                unlockToMainHub()
            }
        }
    }

    private fun showView(state: OnboardingState) {
        binding.layoutOnboarding.visibility = if (state == OnboardingState.UNPAIRED) View.VISIBLE else View.GONE
        binding.layoutBiometricLock.visibility = if (state == OnboardingState.BIOMETRIC_LOCKED) View.VISIBLE else View.GONE
        binding.layoutMainHub.visibility = if (state == OnboardingState.UNLOCKED) View.VISIBLE else View.GONE
    }

    private fun promptBiometrics() {
        biometricManager.promptBiometricAuthentication(
            onSuccess = {
                unlockToMainHub()
            },
            onError = { _, errString ->
                Toast.makeText(this, "Authentication: $errString", Toast.LENGTH_SHORT).show()
            },
            onFailed = {
                Toast.makeText(this, "Fingerprint not recognized. Try again.", Toast.LENGTH_SHORT).show()
            }
        )
    }

    private var updateCheckJob: kotlinx.coroutines.Job? = null

    private fun unlockToMainHub() {
        showView(OnboardingState.UNLOCKED)
        populateMainHubData()
        checkNotificationPermissions()
        checkForAppUpdates()
        startPeriodicUpdateCheck()
        syncFcmToken()
    }

    private fun startPeriodicUpdateCheck() {
        updateCheckJob?.cancel()
        updateCheckJob = lifecycleScope.launch {
            while (true) {
                // Poll for updates every 15 minutes while active
                kotlinx.coroutines.delay(15 * 60 * 1000L)
                checkForAppUpdates()
            }
        }
    }

    private fun populateMainHubData() {
        binding.txtWelcomeHeader.text = "GFC Connect"
        binding.txtMemberIdSubtitle.visibility = View.GONE

        // Filter Feature Cards by User's Permissions
        applyPermissionVisibility()
    }

    private fun applyPermissionVisibility() {
        val storage = GfcConnectApp.instance.tokenStorage

        // Check if user has hall-rentals permission (or fallback to visible)
        binding.cardHallRentals.visibility = if (storage.hasPermission("hall-rentals")) View.VISIBLE else View.GONE

        // Query active lottery variances matching user's threshold
        checkActiveLotteryVariances()
    }

    private fun checkActiveLotteryVariances() {
        val appSettings = GfcConnectApp.instance.appSettings
        val threshold = appSettings.lotteryVarianceThreshold

        // Card is always displayed
        binding.cardLotteryVariance.visibility = View.VISIBLE

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.getActiveLotteryVariances(threshold)
                withContext(Dispatchers.Main) {
                    if (response.isSuccessful && response.body() != null) {
                        val activeList = response.body()!!.filter { !appSettings.isLotteryShiftDismissed(it.shiftId) }
                        if (activeList.isNotEmpty()) {
                            val count = activeList.size
                            binding.txtLotteryVarianceBadge.text = if (count > 1) "⚠️ $count Active" else "⚠️ 1 Active"
                            binding.txtLotteryVarianceBadge.setTextColor(ContextCompat.getColor(this@MainActivity, R.color.status_yellow))
                            binding.txtLotteryVarianceBadge.setBackgroundResource(R.drawable.bg_badge_gray)
                            binding.imgLotteryVarianceIcon.setColorFilter(ContextCompat.getColor(this@MainActivity, R.color.status_yellow))
                            binding.cardLotteryVariance.strokeColor = ContextCompat.getColor(this@MainActivity, R.color.status_yellow)

                            binding.txtLotteryVarianceSummary.text = if (count > 1) {
                                "$count shifts require review"
                            } else {
                                val topItem = activeList.first()
                                "1 shift requires review (${topItem.employeeName})"
                            }
                        } else {
                            binding.txtLotteryVarianceBadge.text = "✓ All Balanced"
                            binding.txtLotteryVarianceBadge.setTextColor(ContextCompat.getColor(this@MainActivity, R.color.emerald_accent))
                            binding.txtLotteryVarianceBadge.setBackgroundResource(R.drawable.bg_badge_emerald)
                            binding.imgLotteryVarianceIcon.setColorFilter(ContextCompat.getColor(this@MainActivity, R.color.emerald_accent))
                            binding.cardLotteryVariance.strokeColor = ContextCompat.getColor(this@MainActivity, R.color.border_dark)

                            binding.txtLotteryVarianceSummary.text = "No active shift variances"
                        }
                    } else {
                        binding.txtLotteryVarianceBadge.text = "✓ All Balanced"
                        binding.txtLotteryVarianceBadge.setTextColor(ContextCompat.getColor(this@MainActivity, R.color.emerald_accent))
                        binding.txtLotteryVarianceBadge.setBackgroundResource(R.drawable.bg_badge_emerald)
                        binding.imgLotteryVarianceIcon.setColorFilter(ContextCompat.getColor(this@MainActivity, R.color.emerald_accent))
                        binding.cardLotteryVariance.strokeColor = ContextCompat.getColor(this@MainActivity, R.color.border_dark)
                        binding.txtLotteryVarianceSummary.text = "No active shift variances"
                    }
                }
            } catch (_: Exception) {
                withContext(Dispatchers.Main) {
                    binding.txtLotteryVarianceBadge.text = "✓ All Balanced"
                    binding.txtLotteryVarianceBadge.setTextColor(ContextCompat.getColor(this@MainActivity, R.color.emerald_accent))
                    binding.txtLotteryVarianceBadge.setBackgroundResource(R.drawable.bg_badge_emerald)
                    binding.imgLotteryVarianceIcon.setColorFilter(ContextCompat.getColor(this@MainActivity, R.color.emerald_accent))
                    binding.cardLotteryVariance.strokeColor = ContextCompat.getColor(this@MainActivity, R.color.border_dark)
                    binding.txtLotteryVarianceSummary.text = "No active shift variances"
                }
            }
        }
    }

    private fun checkRentalPendingCount() {
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.getRentalsList()
                withContext(Dispatchers.Main) {
                    if (response.isSuccessful && response.body() != null) {
                        val rentals = response.body()!!
                        val pendingCount = rentals.count { r ->
                            !r.status.equals("Archived", ignoreCase = true) &&
                            !r.status.equals("Denied", ignoreCase = true) &&
                            !r.status.equals("Cancelled", ignoreCase = true) &&
                            (r.status.equals("Pending", ignoreCase = true) || 
                             r.status.equals("Inquiry", ignoreCase = true) ||
                             r.status.equals("Responded", ignoreCase = true) ||
                             r.eventType?.contains("Inquiry", ignoreCase = true) == true)
                        }

                        if (pendingCount > 0) {
                            binding.txtRentalPendingBadge.visibility = View.VISIBLE
                            binding.txtRentalPendingBadge.text = pendingCount.toString()
                        } else {
                            binding.txtRentalPendingBadge.visibility = View.GONE
                        }
                        binding.txtRentalPendingSummary.text = "Manage inquiries, approve requests & record payments"
                    } else {
                        binding.txtRentalPendingBadge.visibility = View.GONE
                        binding.txtRentalPendingSummary.text = "Manage inquiries, approve requests & record payments"
                    }
                }
            } catch (_: Exception) {
                withContext(Dispatchers.Main) {
                    binding.txtRentalPendingBadge.visibility = View.GONE
                    binding.txtRentalPendingSummary.text = "Manage inquiries, approve requests & record payments"
                }
            }
        }
    }

    private fun pairWithSetupCode(code: String) {
        binding.progressOnboarding.visibility = View.VISIBLE
        binding.txtOnboardingError.visibility = View.GONE
        binding.btnVerifyCode.isEnabled = false

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.redeemSetupCode(SetupCodeRequest(code))
                if (response.isSuccessful && response.body()?.token != null) {
                    val body = response.body()!!
                    val token = body.token!!

                    val user = body.user
                    val permissions = body.permissions

                    if (user != null) {
                        val memberFullName = "${user.firstName ?: ""} ${user.lastName ?: ""}".trim().ifEmpty { user.username }

                        // Save securely in Hardware-Backed Keystore
                        GfcConnectApp.instance.tokenStorage.saveAuthData(
                            token = token,
                            userId = user.userId,
                            username = user.username,
                            memberName = memberFullName,
                            permissions = permissions
                        )

                        withContext(Dispatchers.Main) {
                            binding.progressOnboarding.visibility = View.GONE
                            binding.btnVerifyCode.isEnabled = true
                            Toast.makeText(this@MainActivity, "Device paired successfully as $memberFullName!", Toast.LENGTH_LONG).show()
                            evaluateAppState()
                        }
                    } else {
                        // Fallback: Fetch user info with this token
                        val userResponse = ApiClient.service.getCurrentUser(token)
                        if (userResponse.isSuccessful && userResponse.body()?.user != null) {
                            val result = userResponse.body()!!
                            val u = result.user!!
                            val memberFullName = "${u.firstName ?: ""} ${u.lastName ?: ""}".trim().ifEmpty { u.username }

                            GfcConnectApp.instance.tokenStorage.saveAuthData(
                                token = token,
                                userId = u.userId,
                                username = u.username,
                                memberName = memberFullName,
                                permissions = result.permissions
                            )

                            withContext(Dispatchers.Main) {
                                binding.progressOnboarding.visibility = View.GONE
                                binding.btnVerifyCode.isEnabled = true
                                Toast.makeText(this@MainActivity, "Device paired successfully as $memberFullName!", Toast.LENGTH_LONG).show()
                                evaluateAppState()
                            }
                        } else {
                            showPairingError("Setup code verified, but failed to retrieve user account.")
                        }
                    }
                } else {
                    val rawErr = response.errorBody()?.string() ?: ""
                    val errorMsg = if (rawErr.contains("error")) {
                        rawErr.substringAfter("\"error\":\"").substringBefore("\"")
                    } else if (rawErr.isNotEmpty()) {
                        rawErr
                    } else {
                        "Invalid or expired setup code."
                    }
                    showPairingError(errorMsg)
                }
            } catch (e: Exception) {
                showPairingError("Connection failed to ${ApiClient.currentBaseUrl}: ${e.localizedMessage ?: "Check server status"}")
            }
        }
    }

    private suspend fun showPairingError(message: String) {
        withContext(Dispatchers.Main) {
            binding.progressOnboarding.visibility = View.GONE
            binding.btnVerifyCode.isEnabled = true
            binding.txtOnboardingError.text = message
            binding.txtOnboardingError.visibility = View.VISIBLE
        }
    }

    private fun checkForAppUpdates() {
        lifecycleScope.launch {
            val update = updateManager.checkForUpdates()
            if (update != null) {
                availableUpdate = update
                binding.cardUpdateBanner.visibility = View.VISIBLE
                binding.txtUpdateTitle.text = "Update Available (v${update.latestVersionName})"
                binding.txtUpdateSubtitle.text = update.releaseNotes ?: "Tap Update to get the latest GFC Connect."
            } else {
                binding.cardUpdateBanner.visibility = View.GONE
            }
        }
    }

    private fun startInAppUpdate() {
        binding.btnApplyUpdate.isEnabled = false
        binding.txtUpdateSubtitle.text = "Downloading APK..."

        lifecycleScope.launch {
            updateManager.downloadAndInstallApk(
                onProgress = { percent ->
                    binding.txtUpdateSubtitle.text = "Downloading... $percent%"
                },
                onError = { err ->
                    binding.btnApplyUpdate.isEnabled = true
                    binding.txtUpdateSubtitle.text = "Update error: $err"
                    Toast.makeText(this@MainActivity, err, Toast.LENGTH_LONG).show()
                }
            )
        }
    }

    private fun checkNotificationPermissions() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
            if (ContextCompat.checkSelfPermission(this, Manifest.permission.POST_NOTIFICATIONS) != PackageManager.PERMISSION_GRANTED) {
                requestNotificationPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
            }
        }
    }

    private fun syncFcmToken() {
        val storage = GfcConnectApp.instance.tokenStorage
        val deviceToken = storage.getDeviceToken() ?: return

        // 1. Immediate device registration with hardware specs
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val payload = DeviceRegistrationPayload(
                    deviceToken = deviceToken,
                    fcmToken = null,
                    deviceModel = "${Build.MANUFACTURER.replaceFirstChar { it.uppercase() }} ${Build.MODEL}",
                    osVersion = "Android ${Build.VERSION.RELEASE}",
                    appVersion = BuildConfig.VERSION_NAME,
                    platform = "Android"
                )
                ApiClient.service.registerDevice(payload)
            } catch (e: Exception) {
                // Log or ignore network retry
            }
        }

        // 2. Attach FCM token if Firebase services available
        try {
            FirebaseMessaging.getInstance().token.addOnCompleteListener { task ->
                if (task.isSuccessful && task.result != null) {
                    val fcmToken = task.result
                    lifecycleScope.launch(Dispatchers.IO) {
                        try {
                            val payload = DeviceRegistrationPayload(
                                deviceToken = deviceToken,
                                fcmToken = fcmToken,
                                deviceModel = "${Build.MANUFACTURER.replaceFirstChar { it.uppercase() }} ${Build.MODEL}",
                                osVersion = "Android ${Build.VERSION.RELEASE}",
                                appVersion = BuildConfig.VERSION_NAME,
                                platform = "Android"
                            )
                            ApiClient.service.registerDevice(payload)
                        } catch (e: Exception) {
                            // Silent retry on next session
                        }
                    }
                }
            }
        } catch (e: Exception) {
            // Firebase optional on dev emulator
        }
    }

    private fun updateServerButtonLabel() {
        val current = ApiClient.currentBaseUrl
        val label = when {
            current.contains("localhost") || current.contains("127.0.0.1") -> "USB Mirror (localhost:5207)"
            current.contains("10.0.2.2:5207") -> "Emulator (10.0.2.2:5207)"
            current.contains("lovanow.com") -> "Production Host"
            else -> "Custom ($current)"
        }
        binding.btnToggleServer.text = "🌐 Target: $label"
    }

    private fun showFeatureToast(featureName: String, detail: String) {
        Toast.makeText(this, "[$featureName] $detail", Toast.LENGTH_SHORT).show()
    }

    enum class OnboardingState {
        UNPAIRED,
        BIOMETRIC_LOCKED,
        UNLOCKED
    }
}
