package com.gfc.connect.api

import android.content.Context
import android.content.Intent
import com.gfc.connect.BuildConfig
import com.gfc.connect.GfcConnectApp
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

object ApiClient {

    const val LIVE_URL = "https://gfc.lovanow.com/"
    const val LOCAL_USB_ADB_URL = "http://localhost:5207/" // Physical device via USB ADB Reverse
    const val LOCAL_EMULATOR_URL = "http://10.0.2.2:5207/" // Local AVD Emulator port
    const val LOCAL_IIS_URL = "http://10.0.2.2:62517/"     // Local IIS Express port

    private const val PREFS_NAME = "gfc_api_config"
    private const val KEY_BASE_URL = "custom_base_url"

    fun isRunningOnEmulator(): Boolean {
        return (android.os.Build.FINGERPRINT.startsWith("generic")
                || android.os.Build.FINGERPRINT.startsWith("unknown")
                || android.os.Build.MODEL.contains("google_sdk")
                || android.os.Build.MODEL.contains("Emulator")
                || android.os.Build.MODEL.contains("Android SDK built for x86")
                || android.os.Build.MANUFACTURER.contains("Genymotion")
                || (android.os.Build.BRAND.startsWith("generic") && android.os.Build.DEVICE.startsWith("generic"))
                || "google_sdk" == android.os.Build.PRODUCT
                || android.os.Build.HARDWARE.contains("goldfish")
                || android.os.Build.HARDWARE.contains("ranchu"))
    }

    fun getDefaultBaseUrl(): String {
        return if (BuildConfig.DEBUG) {
            if (isRunningOnEmulator()) LOCAL_EMULATOR_URL else LOCAL_USB_ADB_URL
        } else {
            LIVE_URL
        }
    }

    var currentBaseUrl: String = getDefaultBaseUrl()
        set(value) {
            val formatted = if (value.endsWith("/")) value else "$value/"
            field = formatted
            _service = null
        }

    fun getBaseUrl(): String = currentBaseUrl

    fun initBaseUrl(context: Context) {
        val prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
        val saved = prefs.getString(KEY_BASE_URL, null)
        if (!saved.isNullOrBlank()) {
            if (isRunningOnEmulator() && saved.contains("localhost:5207")) {
                currentBaseUrl = LOCAL_EMULATOR_URL
            } else if (!isRunningOnEmulator() && saved.contains("10.0.2.2:5207")) {
                currentBaseUrl = LOCAL_USB_ADB_URL
            } else {
                currentBaseUrl = saved
            }
        } else {
            currentBaseUrl = getDefaultBaseUrl()
        }
    }

    fun persistBaseUrl(context: Context, url: String) {
        currentBaseUrl = url
        val prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
        prefs.edit().putString(KEY_BASE_URL, currentBaseUrl).apply()
    }

    fun showServerSelectionDialog(context: Context, onServerChanged: (() -> Unit)? = null) {
        val options = arrayOf(
            "🌐 Production Host (gfc.lovanow.com)",
            "📱 Physical Device (USB Mirror / localhost:5207)",
            "💻 Android Studio Emulator (10.0.2.2:5207)",
            "📶 Local Wi-Fi LAN IP (Custom)"
        )

        com.google.android.material.dialog.MaterialAlertDialogBuilder(context)
            .setTitle("🌐 Select Server Target")
            .setItems(options) { _, which ->
                when (which) {
                    0 -> {
                        persistBaseUrl(context, LIVE_URL)
                        android.widget.Toast.makeText(context, "Target: Production (gfc.lovanow.com)", android.widget.Toast.LENGTH_SHORT).show()
                        onServerChanged?.invoke()
                    }
                    1 -> {
                        persistBaseUrl(context, LOCAL_USB_ADB_URL)
                        android.widget.Toast.makeText(context, "Target: USB Mirror (localhost:5207)", android.widget.Toast.LENGTH_SHORT).show()
                        onServerChanged?.invoke()
                    }
                    2 -> {
                        persistBaseUrl(context, LOCAL_EMULATOR_URL)
                        android.widget.Toast.makeText(context, "Target: AVD Emulator (10.0.2.2:5207)", android.widget.Toast.LENGTH_SHORT).show()
                        onServerChanged?.invoke()
                    }
                    3 -> {
                        promptCustomLanIpDialog(context, onServerChanged)
                    }
                }
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun promptCustomLanIpDialog(context: Context, onServerChanged: (() -> Unit)? = null) {
        val input = android.widget.EditText(context).apply {
            hint = "e.g. 192.168.1.150:5207"
            setText(currentBaseUrl.removePrefix("http://").removePrefix("https://").removeSuffix("/"))
            setPadding(40, 30, 40, 30)
        }

        com.google.android.material.dialog.MaterialAlertDialogBuilder(context)
            .setTitle("📶 Enter Local PC IP Address")
            .setMessage("Enter your computer's local Wi-Fi IP and port:")
            .setView(input)
            .setPositiveButton("Connect") { _, _ ->
                val text = input.text.toString().trim()
                if (text.isNotEmpty()) {
                    val fullUrl = if (text.startsWith("http://") || text.startsWith("https://")) text else "http://$text/"
                    persistBaseUrl(context, fullUrl)
                    android.widget.Toast.makeText(context, "Target: $fullUrl", android.widget.Toast.LENGTH_SHORT).show()
                    onServerChanged?.invoke()
                }
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private val authInterceptor = Interceptor { chain ->
        val original = chain.request()
        val builder = original.newBuilder()

        val token = GfcConnectApp.instance.tokenStorage.getDeviceToken()
        if (!token.isNullOrEmpty()) {
            builder.header("Authorization", "Bearer $token")
        }

        builder.header("User-Agent", "GFC-Connect-Android/${BuildConfig.VERSION_NAME}")
        val response = chain.proceed(builder.build())

        // If the server explicitly returns 401 Unauthorized for an authenticated endpoint
        if (response.code == 401 && !original.url.encodedPath.contains("auth/redeem-setup-code") && !original.url.encodedPath.contains("auth/login")) {
            // Invalidate local storage and return to setup code activation
            GfcConnectApp.instance.tokenStorage.clearAll()
            val intent = Intent(GfcConnectApp.instance, com.gfc.connect.ui.MainActivity::class.java).apply {
                flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                putExtra("AUTH_EXPIRED_REASON", "Device authorization expired (401). Please enter a new setup code.")
            }
            GfcConnectApp.instance.startActivity(intent)
        }

        response
    }

    private val okHttpClient: OkHttpClient by lazy {
        val logging = HttpLoggingInterceptor().apply {
            level = if (BuildConfig.DEBUG) HttpLoggingInterceptor.Level.BODY else HttpLoggingInterceptor.Level.NONE
        }

        OkHttpClient.Builder()
            .addInterceptor(authInterceptor)
            .addInterceptor(logging)
            .retryOnConnectionFailure(true)
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(25, TimeUnit.SECONDS)
            .writeTimeout(25, TimeUnit.SECONDS)
            .build()
    }

    private var _service: GfcApiService? = null

    val service: GfcApiService
        get() {
            if (_service == null) {
                _service = Retrofit.Builder()
                    .baseUrl(currentBaseUrl)
                    .client(okHttpClient)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build()
                    .create(GfcApiService::class.java)
            }
            return _service!!
        }
}
