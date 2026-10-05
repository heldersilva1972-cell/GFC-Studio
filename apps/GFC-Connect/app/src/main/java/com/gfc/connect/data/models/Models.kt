package com.gfc.connect.data.models

import com.google.gson.annotations.SerializedName

data class SetupCodeRequest(
    @SerializedName("code") val code: String
)

data class SetupCodeResponse(
    @SerializedName("token") val token: String?,
    @SerializedName("deviceToken") val deviceToken: String? = null,
    @SerializedName("user") val user: UserDto? = null,
    @SerializedName("permissions") val permissions: List<MobilePermissionDto>? = null,
    @SerializedName("allowedRoutes") val allowedRoutes: List<String>? = null,
    @SerializedName("error") val error: String? = null
)

data class GfcLoginResult(
    @SerializedName("code") val code: Int,
    @SerializedName("success") val success: Boolean,
    @SerializedName("user") val user: UserDto?,
    @SerializedName("deviceToken") val deviceToken: String?,
    @SerializedName("permissions") val permissions: List<MobilePermissionDto>?,
    @SerializedName("allowedRoutes") val allowedRoutes: List<String>?,
    @SerializedName("errorMessageForLog") val errorMessageForLog: String?
)

data class UserDto(
    @SerializedName("userId") val userId: Int,
    @SerializedName("username") val username: String,
    @SerializedName("firstName") val firstName: String?,
    @SerializedName("lastName") val lastName: String?,
    @SerializedName("isAdmin") val isAdmin: Boolean,
    @SerializedName("memberId") val memberId: Int?
)

data class MobilePermissionDto(
    @SerializedName("pageId") val pageId: Int,
    @SerializedName("pageName") val pageName: String,
    @SerializedName("pageRoute") val pageRoute: String,
    @SerializedName("category") val category: String?,
    @SerializedName("canAccess") val canAccess: Boolean,
    @SerializedName("canEdit") val canEdit: Boolean
)

data class VersionResponse(
    @SerializedName("latestVersionCode") val latestVersionCode: Int,
    @SerializedName("latestVersionName") val latestVersionName: String,
    @SerializedName("downloadUrl") val downloadUrl: String,
    @SerializedName("mandatoryUpdate") val mandatoryUpdate: Boolean,
    @SerializedName("releaseNotes") val releaseNotes: String?
)

data class DeviceRegistrationPayload(
    @SerializedName("deviceToken") val deviceToken: String,
    @SerializedName("fcmToken") val fcmToken: String?,
    @SerializedName("deviceModel") val deviceModel: String,
    @SerializedName("osVersion") val osVersion: String,
    @SerializedName("appVersion") val appVersion: String,
    @SerializedName("platform") val platform: String = "Android"
)

data class HallRentalDto(
    @SerializedName("id") val id: Int,
    @SerializedName("applicantName") val applicantName: String,
    @SerializedName("requesterPhone") val requesterPhone: String?,
    @SerializedName("requesterEmail") val requesterEmail: String?,
    @SerializedName("eventDate") val eventDate: String,
    @SerializedName("eventType") val eventType: String?,
    @SerializedName("startTime") val startTime: String?,
    @SerializedName("endTime") val endTime: String?,
    @SerializedName("roomSelected") val roomSelected: String?,
    @SerializedName("guestCount") val guestCount: Int,
    @SerializedName("totalPrice") val totalPrice: Double,
    @SerializedName("securityDepositAmount") val securityDepositAmount: Double,
    @SerializedName("requireSecurityDeposit") val requireSecurityDeposit: Boolean = true,
    @SerializedName("amountPaid") val amountPaid: Double? = 0.0,
    @SerializedName("isPaid") val isPaid: Boolean? = false,
    @SerializedName("createdAt") val createdAt: String? = null,
    @SerializedName("status") val status: String,
    @SerializedName("bartenderRequested") val bartenderRequested: Boolean,
    @SerializedName("kitchenUsage") val kitchenUsage: Boolean,
    @SerializedName("avEquipmentUsage") val avEquipmentUsage: Boolean,
    @SerializedName("adminNotes") val adminNotes: String?,
    @SerializedName("matrixSelected") val matrixSelected: String?,
    @SerializedName("isVerifiedMember") val isVerifiedMember: Boolean = false,
    @SerializedName("verifiedMemberId") val verifiedMemberId: Int?,
    @SerializedName("memberVerificationText") val memberVerificationText: String?,
    @SerializedName("memberVerificationBadge") val memberVerificationBadge: String?,
    @SerializedName("possibleMembers") val possibleMembers: List<PossibleMemberDto>? = emptyList()
)

data class PossibleMemberDto(
    @SerializedName("memberId") val memberId: Int,
    @SerializedName("fullName") val fullName: String,
    @SerializedName("status") val status: String,
    @SerializedName("matchReason") val matchReason: String,
    @SerializedName("phone") val phone: String?,
    @SerializedName("email") val email: String?
)

data class RecordPaymentPayload(
    @SerializedName("amount") val amount: Double,
    @SerializedName("paymentMethod") val paymentMethod: String?,
    @SerializedName("note") val note: String?,
    @SerializedName("markAsDeposit") val markAsDeposit: Boolean = false
)

data class PaymentReminderPayload(
    @SerializedName("customNote") val customNote: String?
)

data class CreateClubEventPayload(
    @SerializedName("date") val date: String,
    @SerializedName("reason") val reason: String,
    @SerializedName("location") val location: String = "Function Hall",
    @SerializedName("startTime") val startTime: String?,
    @SerializedName("endTime") val endTime: String?,
    @SerializedName("isFullDay") val isFullDay: Boolean = true
)

data class ApprovalActionRequest(
    @SerializedName("notes") val notes: String?
)

data class ApprovalResponse(
    @SerializedName("success") val success: Boolean,
    @SerializedName("message") val message: String?
)

data class MobileInquiryRequest(
    @SerializedName("eventDate") val eventDate: String,
    @SerializedName("eventType") val eventType: String,
    @SerializedName("roomSelected") val roomSelected: String,
    @SerializedName("guestCount") val guestCount: Int,
    @SerializedName("startTime") val startTime: String,
    @SerializedName("endTime") val endTime: String,
    @SerializedName("barService") val barService: Boolean,
    @SerializedName("kitchenAccess") val kitchenAccess: Boolean,
    @SerializedName("avEquipment") val avEquipment: Boolean,
    @SerializedName("notes") val notes: String?
)

data class InquiryResponse(
    @SerializedName("success") val success: Boolean,
    @SerializedName("id") val id: Int?,
    @SerializedName("status") val status: String?,
    @SerializedName("totalPrice") val totalPrice: Double?,
    @SerializedName("message") val message: String?
)

data class UnavailableDateDto(
    @SerializedName("date") val date: String,
    @SerializedName("status") val status: String? = "Booked",
    @SerializedName("eventType") val eventType: String? = null,
    @SerializedName("eventTime") val eventTime: String? = null,
    @SerializedName("reason") val reason: String? = null,
    @SerializedName("isFullDay") val isFullDay: Boolean = true
)

data class HallRentalDetailDto(
    @SerializedName("id") val id: Int,
    @SerializedName("applicantName") val applicantName: String,
    @SerializedName("requesterName") val requesterName: String?,
    @SerializedName("requesterEmail") val requesterEmail: String?,
    @SerializedName("requesterPhone") val requesterPhone: String?,
    @SerializedName("requesterAddress") val requesterAddress: String?,
    @SerializedName("requesterCity") val requesterCity: String?,
    @SerializedName("requesterState") val requesterState: String?,
    @SerializedName("requesterZip") val requesterZip: String?,
    @SerializedName("eventDate") val eventDate: String,
    @SerializedName("alternateEventDate") val alternateEventDate: String?,
    @SerializedName("roomSelected") val roomSelected: String?,
    @SerializedName("eventType") val eventType: String?,
    @SerializedName("eventDescription") val eventDescription: String?,
    @SerializedName("startTime") val startTime: String?,
    @SerializedName("endTime") val endTime: String?,
    @SerializedName("renterType") val renterType: String?,
    @SerializedName("memberStatus") val memberStatus: Boolean,
    @SerializedName("guestCount") val guestCount: Int?,
    @SerializedName("rulesAgreed") val rulesAgreed: Boolean,
    @SerializedName("bartenderRequested") val bartenderRequested: Boolean,
    @SerializedName("kitchenUsage") val kitchenUsage: Boolean,
    @SerializedName("avEquipmentUsage") val avEquipmentUsage: Boolean,
    @SerializedName("securityDepositPaid") val securityDepositPaid: Boolean,
    @SerializedName("securityDepositAmount") val securityDepositAmount: Double,
    @SerializedName("requireSecurityDeposit") val requireSecurityDeposit: Boolean = true,
    @SerializedName("totalPrice") val totalPrice: Double,
    @SerializedName("amountPaid") val amountPaid: Double,
    @SerializedName("isPaid") val isPaid: Boolean,
    @SerializedName("paymentMethod") val paymentMethod: String?,
    @SerializedName("status") val status: String,
    @SerializedName("approvedBy") val approvedBy: String?,
    @SerializedName("approvalDate") val approvalDate: String?,
    @SerializedName("deniedBy") val deniedBy: String?,
    @SerializedName("denialDate") val denialDate: String?,
    @SerializedName("statusChangedBy") val statusChangedBy: String?,
    @SerializedName("statusChangedDate") val statusChangedDate: String?,
    @SerializedName("internalNotes") val internalNotes: String?,
    @SerializedName("createdDate") val createdDate: String?,
    @SerializedName("createdAt") val createdAt: String? = null,
    @SerializedName("matrixSelected") val matrixSelected: String?,
    @SerializedName("isVerifiedMember") val isVerifiedMember: Boolean = false,
    @SerializedName("verifiedMemberId") val verifiedMemberId: Int?,
    @SerializedName("memberVerificationText") val memberVerificationText: String?,
    @SerializedName("memberVerificationBadge") val memberVerificationBadge: String?,
    @SerializedName("possibleMembers") val possibleMembers: List<PossibleMemberDto>? = emptyList(),
    @SerializedName("availableMatrixTiers") val availableMatrixTiers: List<AvailableMatrixTierDto>? = emptyList(),
    @SerializedName("modificationReasonPresets") val modificationReasonPresets: List<String>? = emptyList()
)

data class AvailableMatrixTierDto(
    @SerializedName("id") val id: String,
    @SerializedName("title") val title: String,
    @SerializedName("subtitle") val subtitle: String?,
    @SerializedName("associatedRenterType") val associatedRenterType: String?,
    @SerializedName("themeColor") val themeColor: String?,
    @SerializedName("rateForDate") val rateForDate: Double,
    @SerializedName("isAvailableForDate") val isAvailableForDate: Boolean,
    @SerializedName("isSelected") val isSelected: Boolean
)

data class UpdateRentalPayload(
    @SerializedName("applicantName") val applicantName: String?,
    @SerializedName("requesterPhone") val requesterPhone: String?,
    @SerializedName("requesterEmail") val requesterEmail: String?,
    @SerializedName("requesterAddress") val requesterAddress: String?,
    @SerializedName("eventDate") val eventDate: String?,
    @SerializedName("eventType") val eventType: String?,
    @SerializedName("startTime") val startTime: String?,
    @SerializedName("endTime") val endTime: String?,
    @SerializedName("roomSelected") val roomSelected: String?,
    @SerializedName("guestCount") val guestCount: Int?,
    @SerializedName("totalPrice") val totalPrice: Double?,
    @SerializedName("securityDepositAmount") val securityDepositAmount: Double?,
    @SerializedName("amountPaid") val amountPaid: Double?,
    @SerializedName("isPaid") val isPaid: Boolean?,
    @SerializedName("bartenderRequested") val bartenderRequested: Boolean?,
    @SerializedName("kitchenUsage") val kitchenUsage: Boolean?,
    @SerializedName("avEquipmentUsage") val avEquipmentUsage: Boolean?,
    @SerializedName("status") val status: String?,
    @SerializedName("internalNotes") val internalNotes: String?,
    @SerializedName("matrixSelected") val matrixSelected: String? = null,
    @SerializedName("sendUpdateEmail") val sendUpdateEmail: Boolean = false,
    @SerializedName("changeReasonNote") val changeReasonNote: String? = null
)

data class RentalUpdateResponse(
    @SerializedName("success") val success: Boolean,
    @SerializedName("message") val message: String?,
    @SerializedName("error") val error: String?
)

data class VerifyMemberResponse(
    @SerializedName("isVerified") val isVerified: Boolean,
    @SerializedName("memberId") val memberId: Int?,
    @SerializedName("statusText") val statusText: String?,
    @SerializedName("badgeType") val badgeType: String?,
    @SerializedName("candidates") val candidates: List<PossibleMemberDto>? = emptyList()
)

