package com.gfc.connect.notifications

import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import androidx.core.app.NotificationCompat
import com.gfc.connect.BuildConfig
import com.gfc.connect.GfcConnectApp
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.models.DeviceRegistrationPayload
import com.gfc.connect.ui.MainActivity
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class FcmService : FirebaseMessagingService() {

    override fun onNewToken(token: String) {
        super.onNewToken(token)
        syncTokenToServer(token)
    }

    override fun onMessageReceived(remoteMessage: RemoteMessage) {
        super.onMessageReceived(remoteMessage)

        val title = remoteMessage.notification?.title ?: remoteMessage.data["title"] ?: "GFC Connect"
        val body = remoteMessage.notification?.body ?: remoteMessage.data["body"] ?: "You have a new update from Gloucester Fraternity Club"
        val targetCategory = remoteMessage.data["category"]?.lowercase()

        val appSettings = GfcConnectApp.instance.appSettings
        if (targetCategory?.contains("inquiry") == true && !appSettings.alertRentalInquiries) {
            return
        }
        if (targetCategory?.contains("rental_payment") == true && !appSettings.alertRentalPayments) {
            return
        }
        if (targetCategory?.contains("rental") == true && !appSettings.alertRentalInquiries && !appSettings.alertRentalPayments) {
            return
        }
        if (targetCategory?.contains("event") == true && !appSettings.alertClubEvents) {
            return
        }
        if (targetCategory?.contains("door") == true && !appSettings.alertDoorAccess) {
            return
        }
        if (targetCategory?.contains("shift") == true && !appSettings.alertShiftReports) {
            return
        }
        if (targetCategory?.contains("bulletin") == true && !appSettings.alertBulletins) {
            return
        }
        if (targetCategory?.contains("lottery") == true) {
            if (!appSettings.alertLotteryVariance) return

            // Check if payload variance magnitude meets user threshold
            val varianceVal = remoteMessage.data["variance"]?.toDoubleOrNull()
            if (varianceVal != null && Math.abs(varianceVal) < appSettings.lotteryVarianceThreshold) {
                return
            }

            // Quiet Hours Check (Default 9:00 PM – 8:00 AM)
            if (appSettings.lotteryQuietHoursEnabled) {
                val currentHour = java.util.Calendar.getInstance().get(java.util.Calendar.HOUR_OF_DAY)
                val startHour = appSettings.lotteryQuietHoursStartHour // 21 (9 PM)
                val endHour = appSettings.lotteryQuietHoursEndHour // 8 (8 AM)
                val isQuiet = if (startHour > endHour) {
                    currentHour >= startHour || currentHour < endHour
                } else {
                    currentHour in startHour until endHour
                }

                if (isQuiet) {
                    // During quiet hours, do not play audible chime / turn on screen.
                    // The variance remains saved and appears immediately on the app dashboard.
                    return
                }
            }
        }

        val channelId = when {
            targetCategory?.contains("rental") == true || targetCategory?.contains("inquiry") == true -> GfcConnectApp.CHANNEL_HALL_RENTALS
            targetCategory?.contains("door") == true -> GfcConnectApp.CHANNEL_DOOR_ACCESS
            targetCategory?.contains("urgent") == true || targetCategory?.contains("alert") == true -> GfcConnectApp.CHANNEL_URGENT_ALERTS
            else -> GfcConnectApp.CHANNEL_GENERAL
        }

        showSystemNotification(title, body, channelId, remoteMessage.data)
    }

    private fun showSystemNotification(title: String, body: String, channelId: String, data: Map<String, String>) {
        val appSettings = GfcConnectApp.instance.appSettings
        val intent = Intent(this, MainActivity::class.java).apply {
            flags = Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_SINGLE_TOP
            for ((key, value) in data) {
                putExtra("fcm_$key", value)
            }
        }

        val pendingIntent = PendingIntent.getActivity(
            this,
            System.currentTimeMillis().toInt(),
            intent,
            PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
        )

        val builder = NotificationCompat.Builder(this, channelId)
            .setSmallIcon(R.drawable.ic_shield_badge)
            .setContentTitle(title)
            .setContentText(body)
            .setStyle(NotificationCompat.BigTextStyle().bigText(body))
            .setPriority(NotificationCompat.PRIORITY_HIGH)
            .setAutoCancel(true)
            .setContentIntent(pendingIntent)

        if (!appSettings.alertSoundEnabled) {
            builder.setSilent(true)
        }
        if (!appSettings.alertVibrateEnabled) {
            builder.setVibrate(longArrayOf(0))
        }

        val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        notificationManager.notify(System.currentTimeMillis().toInt(), builder.build())
    }

    private fun syncTokenToServer(fcmToken: String) {
        val storage = GfcConnectApp.instance.tokenStorage
        val deviceToken = storage.getDeviceToken() ?: return

        CoroutineScope(Dispatchers.IO).launch {
            try {
                val payload = DeviceRegistrationPayload(
                    deviceToken = deviceToken,
                    fcmToken = fcmToken,
                    deviceModel = "${Build.MANUFACTURER} ${Build.MODEL}",
                    osVersion = "Android ${Build.VERSION.RELEASE} (API ${Build.VERSION.SDK_INT})",
                    appVersion = BuildConfig.VERSION_NAME,
                    platform = "Android"
                )
                ApiClient.service.registerDevice(payload)
            } catch (e: Exception) {
                // Token sync can be retried on next app launch
            }
        }
    }
}
