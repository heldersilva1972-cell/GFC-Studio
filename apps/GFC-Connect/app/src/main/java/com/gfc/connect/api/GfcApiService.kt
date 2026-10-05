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

    @GET("api/rentals/mobile/detail/{id}")
    suspend fun getRentalDetail(@Path("id") id: Int): Response<HallRentalDetailDto>

    @POST("api/rentals/mobile/update/{id}")
    suspend fun updateRental(@Path("id") id: Int, @Body payload: UpdateRentalPayload): Response<RentalUpdateResponse>
}
