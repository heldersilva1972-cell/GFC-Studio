package com.gfc.connect.api

import com.gfc.connect.data.models.*
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.*

interface GfcApiService {

    @POST("api/mobile-auth/redeem-setup-code")
    suspend fun redeemSetupCode(@Body request: SetupCodeRequest): Response<SetupCodeResponse>

    @GET("api/mobile-auth/user")
    suspend fun getCurrentUser(@Query("token") token: String? = null): Response<GfcLoginResult>

    @GET("api/app/version")
    suspend fun getAppVersion(): Response<VersionResponse>

    @Streaming
    @GET("api/app/download/latest.apk")
    suspend fun downloadApk(@Query("token") token: String? = null): Response<ResponseBody>

    @POST("api/devices/register")
    suspend fun registerDevice(@Body payload: DeviceRegistrationPayload): Response<ResponseBody>

    @GET("api/rentals/mobile/list")
    suspend fun getRentalsList(): Response<List<HallRentalDto>>

    @GET("api/rentals/mobile/unavailable-dates")
    suspend fun getUnavailableDates(): Response<List<UnavailableDateDto>>

    @POST("api/rentals/mobile/inquire")
    suspend fun submitInquiry(@Body payload: MobileInquiryRequest): Response<InquiryResponse>

    @POST("api/rentals/mobile/approve/{id}")
    suspend fun approveRental(@Path("id") id: Int, @Body request: ApprovalActionRequest? = null): Response<ApprovalResponse>

    @POST("api/rentals/mobile/deny/{id}")
    suspend fun denyRental(@Path("id") id: Int, @Body request: ApprovalActionRequest? = null): Response<ApprovalResponse>

    @POST("api/rentals/mobile/cancel/{id}")
    suspend fun cancelRental(@Path("id") id: Int, @Body request: ApprovalActionRequest? = null): Response<ApprovalResponse>

    @DELETE("api/rentals/mobile/delete/{id}")
    suspend fun deleteRental(@Path("id") id: Int): Response<ApprovalResponse>

    @GET("api/rentals/mobile/detail/{id}")
    suspend fun getRentalDetail(@Path("id") id: Int): Response<HallRentalDetailDto>

    @GET("api/rentals/mobile/verify-member")
    suspend fun verifyMemberLive(
        @Query("name") name: String?,
        @Query("email") email: String?,
        @Query("phone") phone: String?,
        @Query("isMember") isMember: Boolean = true
    ): Response<VerifyMemberResponse>

    @POST("api/rentals/mobile/update/{id}")
    suspend fun updateRental(@Path("id") id: Int, @Body payload: UpdateRentalPayload): Response<RentalUpdateResponse>

    @POST("api/rentals/mobile/record-payment/{id}")
    suspend fun recordPayment(@Path("id") id: Int, @Body payload: RecordPaymentPayload): Response<ApprovalResponse>

    @POST("api/rentals/mobile/payment-reminder/{id}")
    suspend fun sendPaymentReminder(@Path("id") id: Int, @Body payload: PaymentReminderPayload): Response<ApprovalResponse>

    @POST("api/rentals/mobile/club-event")
    suspend fun createClubEvent(@Body payload: CreateClubEventPayload): Response<ApprovalResponse>
}
