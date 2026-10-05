package com.gfc.connect.update

import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Build
import androidx.core.content.FileProvider
import com.gfc.connect.BuildConfig
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.models.VersionResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.io.File
import java.io.FileOutputStream
import java.io.InputStream

class AppUpdateManager(private val context: Context) {

    suspend fun checkForUpdates(): VersionResponse? = withContext(Dispatchers.IO) {
        try {
            val response = ApiClient.service.getAppVersion()
            if (response.isSuccessful && response.body() != null) {
                val versionInfo = response.body()!!
                if (versionInfo.latestVersionCode > BuildConfig.VERSION_CODE) {
                    return@withContext versionInfo
                }
            }
            null
        } catch (e: Exception) {
            null
        }
    }

    suspend fun downloadAndInstallApk(
        onProgress: (Int) -> Unit,
        onError: (String) -> Unit
    ) = withContext(Dispatchers.IO) {
        try {
            val response = ApiClient.service.downloadApk()
            if (!response.isSuccessful || response.body() == null) {
                withContext(Dispatchers.Main) { onError("Failed to stream APK from host server.") }
                return@withContext
            }

            val body = response.body()!!
            val apkFile = File(context.cacheDir, "gfc-connect-update.apk")
            if (apkFile.exists()) apkFile.delete()

            val inputStream: InputStream = body.byteStream()
            val outputStream = FileOutputStream(apkFile)
            val totalBytes = body.contentLength()
            var bytesReadTotal = 0L
            val buffer = ByteArray(8192)
            var bytesRead: Int

            while (inputStream.read(buffer).also { bytesRead = it } != -1) {
                outputStream.write(buffer, 0, bytesRead)
                bytesReadTotal += bytesRead
                if (totalBytes > 0) {
                    val progress = ((bytesReadTotal * 100) / totalBytes).toInt()
                    withContext(Dispatchers.Main) { onProgress(progress) }
                }
            }

            outputStream.flush()
            outputStream.close()
            inputStream.close()

            // Launch Native Android Package Installer via FileProvider
            withContext(Dispatchers.Main) {
                launchPackageInstaller(apkFile)
            }
        } catch (e: Exception) {
            withContext(Dispatchers.Main) {
                onError("Download error: ${e.localizedMessage}")
            }
        }
    }

    private fun launchPackageInstaller(apkFile: File) {
        val authority = "${context.packageName}.fileprovider"
        val contentUri: Uri = FileProvider.getUriForFile(context, authority, apkFile)

        val installIntent = Intent(Intent.ACTION_VIEW).apply {
            setDataAndType(contentUri, "application/vnd.android.package-archive")
            addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION)
            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
        }

        context.startActivity(installIntent)
    }
}
