package com.gfc.connect.security

import android.content.Context
import android.content.SharedPreferences
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import com.gfc.connect.data.models.MobilePermissionDto
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

class SecureTokenStorage(context: Context) {

    private val masterKey = MasterKey.Builder(context)
        .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
        .build()

    private val prefs: SharedPreferences = EncryptedSharedPreferences.create(
        context,
        PREFS_NAME,
        masterKey,
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )

    private val gson = Gson()

    fun saveAuthData(token: String, userId: Int, username: String, memberName: String?, permissions: List<MobilePermissionDto>?) {
        val permissionsJson = if (permissions != null) gson.toJson(permissions) else null
        prefs.edit()
            .putString(KEY_DEVICE_TOKEN, token)
            .putInt(KEY_USER_ID, userId)
            .putString(KEY_USERNAME, username)
            .putString(KEY_MEMBER_NAME, memberName)
            .putString(KEY_PERMISSIONS_JSON, permissionsJson)
            .apply()
    }

    fun getDeviceToken(): String? = prefs.getString(KEY_DEVICE_TOKEN, null)

    fun getUserId(): Int = prefs.getInt(KEY_USER_ID, -1)

    fun getUsername(): String? = prefs.getString(KEY_USERNAME, null)

    fun getMemberName(): String? = prefs.getString(KEY_MEMBER_NAME, null)

    fun getPermissions(): List<MobilePermissionDto> {
        val json = prefs.getString(KEY_PERMISSIONS_JSON, null) ?: return emptyList()
        val type = object : TypeToken<List<MobilePermissionDto>>() {}.type
        return try {
            gson.fromJson(json, type) ?: emptyList()
        } catch (e: Exception) {
            emptyList()
        }
    }

    fun hasPermission(routeKeyword: String): Boolean {
        val permissions = getPermissions()
        return permissions.any { it.canAccess && it.pageRoute.contains(routeKeyword, ignoreCase = true) }
    }

    fun isBiometricEnabled(): Boolean = prefs.getBoolean(KEY_BIOMETRIC_ENABLED, true)

    fun setBiometricEnabled(enabled: Boolean) {
        prefs.edit().putBoolean(KEY_BIOMETRIC_ENABLED, enabled).apply()
    }

    fun isPaired(): Boolean = !getDeviceToken().isNullOrEmpty()

    fun clearAll() {
        prefs.edit().clear().apply()
    }

    companion object {
        private const val PREFS_NAME = "gfc_secure_vault_prefs"
        private const val KEY_DEVICE_TOKEN = "enc_dev_token"
        private const val KEY_USER_ID = "enc_user_id"
        private const val KEY_USERNAME = "enc_username"
        private const val KEY_MEMBER_NAME = "enc_member_name"
        private const val KEY_PERMISSIONS_JSON = "enc_perms_json"
        private const val KEY_BIOMETRIC_ENABLED = "enc_biometric_active"
    }
}
