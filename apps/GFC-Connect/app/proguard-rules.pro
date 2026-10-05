# ====================================================================
# GFC Connect ProGuard / R8 Obfuscation & Security Hardening Rules
# ====================================================================

# Code Shrinking & Name Obfuscation
-repackageclasses 'com.gfc.connect.internal'
-allowaccessmodification

# Strip verbose debug logs in release builds for anti-tamper security
-assumenosideeffects class android.util.Log {
    public static boolean isLoggable(java.lang.String, int);
    public static int v(...);
    public static int d(...);
}

# Keep Data Models & DTOs for JSON Serialization
-keepclassmembers class * {
    @com.google.gson.annotations.SerializedName <fields>;
}
-keep class com.gfc.connect.data.models.** { *; }

# Retrofit & OkHttp
-dontwarn okio.**
-dontwarn javax.annotation.**
-keepattributes Signature, InnerClasses, EnclosingMethod
-keepclassmembers,allowobfuscation interface * {
    @retrofit2.http.* <methods>;
}

# AndroidX Biometric & Security Crypto
-keep class androidx.biometric.** { *; }
-keep class androidx.security.crypto.** { *; }

# Firebase Messaging
-keep class com.google.firebase.messaging.** { *; }
-keep class com.gfc.connect.notifications.FcmService { *; }

# Room SQLite Database
-keepclassmembers class * extends androidx.room.RoomDatabase {
    public abstract <methods>;
}
-keep class * extends androidx.room.RoomDatabase
-dontwarn androidx.room.paging.**
