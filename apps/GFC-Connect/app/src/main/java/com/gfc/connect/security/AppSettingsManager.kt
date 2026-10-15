package com.gfc.connect.security

import android.content.Context
import android.content.SharedPreferences

class AppSettingsManager(context: Context) {

    private val prefs: SharedPreferences = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)

    // ==========================================
    // NOTIFICATION & ALERT PREFERENCES
    // ==========================================
    var alertRentalInquiries: Boolean
        get() = prefs.getBoolean(KEY_ALERT_RENTAL_INQUIRIES, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_RENTAL_INQUIRIES, value).apply()

    var alertRentalPayments: Boolean
        get() = prefs.getBoolean(KEY_ALERT_RENTAL_PAYMENTS, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_RENTAL_PAYMENTS, value).apply()

    var alertClubEvents: Boolean
        get() = prefs.getBoolean(KEY_ALERT_CLUB_EVENTS, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_CLUB_EVENTS, value).apply()

    var alertDoorAccess: Boolean
        get() = prefs.getBoolean(KEY_ALERT_DOOR_ACCESS, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_DOOR_ACCESS, value).apply()

    var alertShiftReports: Boolean
        get() = prefs.getBoolean(KEY_ALERT_SHIFT_REPORTS, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_SHIFT_REPORTS, value).apply()

    var alertBulletins: Boolean
        get() = prefs.getBoolean(KEY_ALERT_BULLETINS, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_BULLETINS, value).apply()

    var alertLotteryVariance: Boolean
        get() = prefs.getBoolean(KEY_ALERT_LOTTERY_VARIANCE, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_LOTTERY_VARIANCE, value).apply()

    var lotteryVarianceThreshold: Float
        get() = prefs.getFloat(KEY_LOTTERY_VARIANCE_THRESHOLD, 5.00f)
        set(value) = prefs.edit().putFloat(KEY_LOTTERY_VARIANCE_THRESHOLD, value).apply()

    var lotteryQuietHoursEnabled: Boolean
        get() = prefs.getBoolean(KEY_LOTTERY_QUIET_HOURS_ENABLED, true)
        set(value) = prefs.edit().putBoolean(KEY_LOTTERY_QUIET_HOURS_ENABLED, value).apply()

    var lotteryQuietHoursStartHour: Int
        get() = prefs.getInt(KEY_LOTTERY_QUIET_HOURS_START, 21) // 9:00 PM
        set(value) = prefs.edit().putInt(KEY_LOTTERY_QUIET_HOURS_START, value).apply()

    var lotteryQuietHoursEndHour: Int
        get() = prefs.getInt(KEY_LOTTERY_QUIET_HOURS_END, 8) // 8:00 AM
        set(value) = prefs.edit().putInt(KEY_LOTTERY_QUIET_HOURS_END, value).apply()

    var alertSoundEnabled: Boolean
        get() = prefs.getBoolean(KEY_ALERT_SOUND, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_SOUND, value).apply()

    var alertVibrateEnabled: Boolean
        get() = prefs.getBoolean(KEY_ALERT_VIBRATE, true)
        set(value) = prefs.edit().putBoolean(KEY_ALERT_VIBRATE, value).apply()

    // ==========================================
    // LOCAL DISMISSED SHIFTS (DEVICE-SPECIFIC)
    // ==========================================
    fun isLotteryShiftDismissed(shiftId: Int): Boolean {
        val set = prefs.getStringSet(KEY_DISMISSED_LOTTERY_SHIFTS, emptySet()) ?: emptySet()
        return set.contains(shiftId.toString())
    }

    fun dismissLotteryShift(shiftId: Int) {
        val currentSet = prefs.getStringSet(KEY_DISMISSED_LOTTERY_SHIFTS, emptySet()) ?: emptySet()
        val newSet = HashSet(currentSet)
        newSet.add(shiftId.toString())
        prefs.edit().putStringSet(KEY_DISMISSED_LOTTERY_SHIFTS, newSet).apply()
    }

    // Lightweight offline JSON cache (avoids Room code generation issues)
    fun getCachedLotteryJson(key: String): String? {
        return prefs.getString(key, null)
    }

    fun saveCachedLotteryJson(key: String, json: String) {
        prefs.edit().putString(key, json).apply()
    }

    // ==========================================
    // BIOMETRICS & SECURITY PREFERENCES
    // ==========================================
    var biometricAppLockEnabled: Boolean
        get() = prefs.getBoolean(KEY_BIOMETRIC_APP_LOCK, true)
        set(value) = prefs.edit().putBoolean(KEY_BIOMETRIC_APP_LOCK, value).apply()

    var biometricRequireForSensitiveActions: Boolean
        get() = prefs.getBoolean(KEY_BIOMETRIC_SENSITIVE_ACTIONS, true)
        set(value) = prefs.edit().putBoolean(KEY_BIOMETRIC_SENSITIVE_ACTIONS, value).apply()

    fun resetToDefaults() {
        prefs.edit()
            .putBoolean(KEY_ALERT_RENTAL_INQUIRIES, true)
            .putBoolean(KEY_ALERT_RENTAL_PAYMENTS, true)
            .putBoolean(KEY_ALERT_CLUB_EVENTS, true)
            .putBoolean(KEY_ALERT_DOOR_ACCESS, true)
            .putBoolean(KEY_ALERT_SHIFT_REPORTS, true)
            .putBoolean(KEY_ALERT_BULLETINS, true)
            .putBoolean(KEY_ALERT_LOTTERY_VARIANCE, true)
            .putFloat(KEY_LOTTERY_VARIANCE_THRESHOLD, 5.00f)
            .putBoolean(KEY_LOTTERY_QUIET_HOURS_ENABLED, true)
            .putInt(KEY_LOTTERY_QUIET_HOURS_START, 21)
            .putInt(KEY_LOTTERY_QUIET_HOURS_END, 8)
            .putBoolean(KEY_ALERT_SOUND, true)
            .putBoolean(KEY_ALERT_VIBRATE, true)
            .putBoolean(KEY_BIOMETRIC_APP_LOCK, true)
            .putBoolean(KEY_BIOMETRIC_SENSITIVE_ACTIONS, true)
            .apply()
    }

    companion object {
        private const val PREFS_NAME = "gfc_connect_app_settings"
        private const val KEY_ALERT_RENTAL_INQUIRIES = "alert_rental_inquiries"
        private const val KEY_ALERT_RENTAL_PAYMENTS = "alert_rental_payments"
        private const val KEY_ALERT_CLUB_EVENTS = "alert_club_events"
        private const val KEY_ALERT_DOOR_ACCESS = "alert_door_access"
        private const val KEY_ALERT_SHIFT_REPORTS = "alert_shift_reports"
        private const val KEY_ALERT_BULLETINS = "alert_bulletins"
        private const val KEY_ALERT_LOTTERY_VARIANCE = "alert_lottery_variance"
        private const val KEY_LOTTERY_VARIANCE_THRESHOLD = "lottery_variance_threshold"
        private const val KEY_LOTTERY_QUIET_HOURS_ENABLED = "lottery_quiet_hours_enabled"
        private const val KEY_LOTTERY_QUIET_HOURS_START = "lottery_quiet_hours_start"
        private const val KEY_LOTTERY_QUIET_HOURS_END = "lottery_quiet_hours_end"
        private const val KEY_ALERT_SOUND = "alert_sound"
        private const val KEY_ALERT_VIBRATE = "alert_vibrate"
        private const val KEY_DISMISSED_LOTTERY_SHIFTS = "dismissed_lottery_shifts"
        private const val KEY_BIOMETRIC_APP_LOCK = "biometric_app_lock"
        private const val KEY_BIOMETRIC_SENSITIVE_ACTIONS = "biometric_sensitive_actions"
    }
}
