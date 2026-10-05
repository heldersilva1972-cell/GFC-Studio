package com.gfc.connect.ui

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.view.View
import android.view.inputmethod.InputMethodManager
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.lifecycle.lifecycleScope
import com.gfc.connect.BuildConfig
import com.gfc.connect.GfcConnectApp
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.models.DeviceRegistrationPayload
import com.gfc.connect.data.models.SetupCodeRequest
import com.gfc.connect.data.models.VersionResponse
import com.gfc.connect.databinding.ActivityMainBinding
import com.gfc.connect.security.BiometricAuthManager
import com.gfc.connect.update.AppUpdateManager
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

        setupListeners()
        evaluateAppState()
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

        // Server Endpoint Switcher (Localhost 5207 vs IIS Express vs Live)
        val serverOptions = listOf(
            "Localhost (5207)" to ApiClient.LOCAL_EMULATOR_URL,
            "Live Cloud" to ApiClient.LIVE_URL,
            "IIS Express (62517)" to ApiClient.LOCAL_IIS_URL
        )
        var currentServerIndex = 0
        ApiClient.currentBaseUrl = serverOptions[currentServerIndex].second
        binding.btnToggleServer.text = "🌐 Server: ${serverOptions[currentServerIndex].first}"

        binding.btnToggleServer.setOnClickListener {
            currentServerIndex = (currentServerIndex + 1) % serverOptions.size
            ApiClient.currentBaseUrl = serverOptions[currentServerIndex].second
            binding.btnToggleServer.text = "🌐 Server: ${serverOptions[currentServerIndex].first}"
            Toast.makeText(this, "Target: ${serverOptions[currentServerIndex].second}", Toast.LENGTH_SHORT).show()
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

        binding.cardDoorAccess.setOnClickListener {
            showFeatureToast("Door Access", "Accessing digital key & facility door controls...")
        }

        binding.cardClubNews.setOnClickListener {
            showFeatureToast("Club News", "Loading bulletins, announcements, and events...")
        }

        binding.cardMemberDues.setOnClickListener {
            showFeatureToast("Member Dues", "Opening dues payment & renewal portal...")
        }

        binding.cardShiftReports.setOnClickListener {
            showFeatureToast("Shift Reports", "Opening daily bartender shift reporting...")
        }

        binding.cardSettings.setOnClickListener {
            showFeatureToast("Settings", "Opening biometrics & security settings...")
        }

        binding.btnNotificationBell.setOnClickListener {
            showFeatureToast("Notifications", "You have no unread notifications.")
        }
    }

    private fun evaluateAppState() {
        val storage = GfcConnectApp.instance.tokenStorage

        when {
            // State 1: Device Not Yet Paired -> Show 6-digit Setup Code screen
            !storage.isPaired() -> {
                showView(OnboardingState.UNPAIRED)
            }

            // State 2: Device Paired & Biometrics Available -> Lock screen & prompt Biometrics
            storage.isBiometricEnabled() && biometricManager.canAuthenticate() -> {
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

    private fun unlockToMainHub() {
        showView(OnboardingState.UNLOCKED)
        populateMainHubData()
        checkNotificationPermissions()
        checkForAppUpdates()
        syncFcmToken()
    }

    private fun populateMainHubData() {
        val storage = GfcConnectApp.instance.tokenStorage
        val memberName = storage.getMemberName() ?: storage.getUsername() ?: "Member"
        val userId = storage.getUserId()

        binding.txtWelcomeHeader.text = "Welcome back, $memberName"
        binding.txtMemberIdSubtitle.text = "Member ID: #${userId.toString().padStart(7, '0')}"

        // Filter Feature Cards by User's Permissions
        applyPermissionVisibility()
    }

    private fun applyPermissionVisibility() {
        val storage = GfcConnectApp.instance.tokenStorage

        // Check if user has specific permissions (or fallback to visible for general cards)
        binding.cardHallRentals.visibility = if (storage.hasPermission("hall-rentals")) View.VISIBLE else View.GONE
        binding.cardDoorAccess.visibility = if (storage.hasPermission("door-access")) View.VISIBLE else View.GONE
        binding.cardShiftReports.visibility = if (storage.hasPermission("shift-reports")) View.VISIBLE else View.GONE
        binding.cardMemberDues.visibility = if (storage.hasPermission("member-dues")) View.VISIBLE else View.GONE
        binding.cardClubNews.visibility = if (storage.hasPermission("club-news")) View.VISIBLE else View.VISIBLE
    }

    private fun pairWithSetupCode(code: String) {
        binding.progressOnboarding.visibility = View.VISIBLE
        binding.txtOnboardingError.visibility = View.GONE
        binding.btnVerifyCode.isEnabled = false

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.redeemSetupCode(SetupCodeRequest(code))
                if (response.isSuccessful && response.body()?.token != null) {
                    val token = response.body()!!.token!!

                    // Fetch user info with this token
                    val userResponse = ApiClient.service.getCurrentUser(token)
                    if (userResponse.isSuccessful && userResponse.body()?.user != null) {
                        val result = userResponse.body()!!
                        val user = result.user!!
                        val memberFullName = "${user.firstName ?: ""} ${user.lastName ?: ""}".trim().ifEmpty { user.username }

                        // Save securely in Hardware-Backed Keystore
                        GfcConnectApp.instance.tokenStorage.saveAuthData(
                            token = token,
                            userId = user.userId,
                            username = user.username,
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

    private fun showFeatureToast(featureName: String, detail: String) {
        Toast.makeText(this, "[$featureName] $detail", Toast.LENGTH_SHORT).show()
    }

    enum class OnboardingState {
        UNPAIRED,
        BIOMETRIC_LOCKED,
        UNLOCKED
    }
}
