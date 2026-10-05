package com.gfc.connect.data.db

import android.content.Context
import androidx.room.*

@Entity(tableName = "cached_records")
data class CachedRecord(
    @PrimaryKey val key: String,
    val jsonPayload: String,
    val lastUpdated: Long = System.currentTimeMillis()
)

@Dao
interface CachedRecordDao {
    @Query("SELECT * FROM cached_records WHERE `key` = :key LIMIT 1")
    suspend fun getRecord(key: String): CachedRecord?

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun saveRecord(record: CachedRecord)

    @Query("DELETE FROM cached_records WHERE `key` = :key")
    suspend fun deleteRecord(key: String)

    @Query("DELETE FROM cached_records")
    suspend fun clearAll()
}

@Database(entities = [CachedRecord::class], version = 1, exportSchema = false)
abstract class GfcDatabase : RoomDatabase() {
    abstract fun recordDao(): CachedRecordDao

    companion object {
        @Volatile
        private var INSTANCE: GfcDatabase? = null

        fun getDatabase(context: Context): GfcDatabase {
            return INSTANCE ?: synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    GfcDatabase::class.java,
                    "gfc_connect_offline.db"
                ).build()
                INSTANCE = instance
                instance
            }
        }
    }
}
