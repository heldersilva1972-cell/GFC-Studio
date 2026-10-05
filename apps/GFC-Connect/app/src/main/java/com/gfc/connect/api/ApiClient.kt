package com.gfc.connect.api

import android.content.Context
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
    const val LOCAL_EMULATOR_URL = "http://10.0.2.2:5207/" // Local Kestrel / Visual Studio port
    const val LOCAL_IIS_URL = "http://10.0.2.2:62517/"     // Local IIS Express port

    var currentBaseUrl: String = LIVE_URL
        set(value) {
            val formatted = if (value.endsWith("/")) value else "$value/"
            field = formatted
            _service = null
        }

    private val authInterceptor = Interceptor { chain ->
        val original = chain.request()
        val builder = original.newBuilder()

        val token = GfcConnectApp.instance.tokenStorage.getDeviceToken()
        if (!token.isNullOrEmpty()) {
            builder.header("Authorization", "Bearer $token")
        }

        builder.header("User-Agent", "GFC-Connect-Android/${BuildConfig.VERSION_NAME}")
        chain.proceed(builder.build())
    }

    private val okHttpClient: OkHttpClient by lazy {
        val logging = HttpLoggingInterceptor().apply {
            level = if (BuildConfig.DEBUG) HttpLoggingInterceptor.Level.BODY else HttpLoggingInterceptor.Level.NONE
        }

        OkHttpClient.Builder()
            .addInterceptor(authInterceptor)
            .addInterceptor(logging)
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(60, TimeUnit.SECONDS)
            .writeTimeout(60, TimeUnit.SECONDS)
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
