package com.gfc.connect

import android.app.Application
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.os.Build
import com.gfc.connect.security.SecureTokenStorage

class GfcConnectApp : Application() {

    lateinit var tokenStorage: SecureTokenStorage
        private set

    override fun onCreate() {
        super.onCreate()
        instance = this

        // Initialize Hardware-Backed Secure Token Storage
        tokenStorage = SecureTokenStorage(this)

        // Create System Notification Channels
        createNotificationChannels()
    }

    private fun createNotificationChannels() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

            // 1. Urgent Club Alerts Channel (High Importance, Sound & Vibration)
            val urgentChannel = NotificationChannel(
                CHANNEL_URGENT_ALERTS,
                getString(R.string.notif_channel_alerts),
                NotificationManager.IMPORTANCE_HIGH
            ).apply {
                description = "Urgent club security and facility alerts"
                enableVibration(true)
                enableLights(true)
            }

            // 2. Hall Rentals Updates Channel
            val rentalsChannel = NotificationChannel(
                CHANNEL_HALL_RENTALS,
                getString(R.string.notif_channel_rentals),
                NotificationManager.IMPORTANCE_DEFAULT
            ).apply {
                description = "Hall rental bookings, approvals, and contract notifications"
            }

            // 3. General Announcements Channel
            val generalChannel = NotificationChannel(
                CHANNEL_GENERAL,
                getString(R.string.notif_channel_general),
                NotificationManager.IMPORTANCE_DEFAULT
            ).apply {
                description = "Club bulletins, event updates, and news"
            }

            notificationManager.createNotificationChannels(listOf(urgentChannel, rentalsChannel, generalChannel))
        }
    }

    companion object {
        const val CHANNEL_URGENT_ALERTS = "gfc_channel_urgent"
        const val CHANNEL_HALL_RENTALS = "gfc_channel_rentals"
        const val CHANNEL_GENERAL = "gfc_channel_general"

        lateinit var instance: GfcConnectApp
            private set
    }
}
