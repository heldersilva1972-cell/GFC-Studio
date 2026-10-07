package com.gfc.connect.data.cache

import android.content.Context
import android.content.SharedPreferences
import com.gfc.connect.data.models.HallRentalDetailDto
import com.gfc.connect.data.models.HallRentalDto
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

class RentalCacheManager(context: Context) {

    private val prefs: SharedPreferences = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
    private val gson = Gson()

    fun saveRentals(items: List<HallRentalDto>) {
        val json = gson.toJson(items)
        prefs.edit()
            .putString(KEY_RENTALS_LIST, json)
            .putLong(KEY_LAST_SYNC, System.currentTimeMillis())
            .apply()
    }

    fun getRentals(): List<HallRentalDto> {
        val json = prefs.getString(KEY_RENTALS_LIST, null) ?: return emptyList()
        val type = object : TypeToken<List<HallRentalDto>>() {}.type
        return try {
            gson.fromJson(json, type) ?: emptyList()
        } catch (e: Exception) {
            emptyList()
        }
    }

    fun saveRentalDetail(detail: HallRentalDetailDto) {
        val key = "$KEY_RENTAL_DETAIL_PREFIX${detail.id}"
        val json = gson.toJson(detail)
        prefs.edit()
            .putString(key, json)
            .apply()
    }

    fun getRentalDetail(id: Int): HallRentalDetailDto? {
        val key = "$KEY_RENTAL_DETAIL_PREFIX$id"
        val json = prefs.getString(key, null) ?: return null
        return try {
            gson.fromJson(json, HallRentalDetailDto::class.java)
        } catch (e: Exception) {
            null
        }
    }

    fun saveUnavailableDates(items: List<com.gfc.connect.data.models.UnavailableDateDto>) {
        val json = gson.toJson(items)
        prefs.edit()
            .putString(KEY_UNAVAILABLE_DATES, json)
            .apply()
    }

    fun getUnavailableDates(): List<com.gfc.connect.data.models.UnavailableDateDto> {
        val json = prefs.getString(KEY_UNAVAILABLE_DATES, null) ?: return emptyList()
        val type = object : TypeToken<List<com.gfc.connect.data.models.UnavailableDateDto>>() {}.type
        return try {
            gson.fromJson(json, type) ?: emptyList()
        } catch (e: Exception) {
            emptyList()
        }
    }

    fun getLastSyncTime(): Long = prefs.getLong(KEY_LAST_SYNC, 0L)

    fun clearCache() {
        prefs.edit().clear().apply()
    }

    companion object {
        private const val PREFS_NAME = "gfc_rentals_local_cache"
        private const val KEY_RENTALS_LIST = "cache_rentals_list"
        private const val KEY_UNAVAILABLE_DATES = "cache_unavailable_dates"
        private const val KEY_RENTAL_DETAIL_PREFIX = "cache_rental_detail_"
        private const val KEY_LAST_SYNC = "cache_last_sync_timestamp"
    }
}
