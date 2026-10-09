package com.gfc.connect.ui

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import android.content.Intent
import android.content.res.ColorStateList
import android.graphics.Typeface
import android.net.Uri
import android.os.Bundle
import android.view.Gravity
import android.view.View
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.cache.RentalCacheManager
import com.gfc.connect.data.models.ApprovalActionRequest
import androidx.core.widget.addTextChangedListener
import com.gfc.connect.data.models.ArchiveRentalPayload
import com.gfc.connect.data.models.AvailableAddonDto
import com.gfc.connect.data.models.AvailableMatrixTierDto
import com.gfc.connect.data.models.HallRentalDetailDto
import com.gfc.connect.data.models.LogCorrespondencePayload
import com.gfc.connect.data.models.PaymentReminderPayload
import com.gfc.connect.data.models.RecordPaymentPayload
import com.gfc.connect.data.models.UnavailableDateDto
import com.gfc.connect.data.models.UpdateRentalPayload
import com.gfc.connect.databinding.ActivityRentalDetailBinding
import com.google.android.material.button.MaterialButton
import com.google.android.material.card.MaterialCardView
import com.google.android.material.chip.Chip
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.text.NumberFormat
import java.text.SimpleDateFormat
import java.util.*

class RentalDetailActivity : AppCompatActivity() {

    companion object {
        const val EXTRA_RENTAL_ID = "extra_rental_id"
        const val EXTRA_IS_INQUIRY = "extra_is_inquiry"
    }

    private lateinit var binding: ActivityRentalDetailBinding
    private lateinit var cacheManager: RentalCacheManager

    private var rentalId: Int = 0
    private var rentalDetail: HallRentalDetailDto? = null
    private var isEditMode: Boolean = false
    private var isInquiryMode: Boolean = false
    private var selectedMatrixTierTitle: String? = null
    private var selectedEventCalendar: Calendar = Calendar.getInstance()
    private var originalEventCalendar: Calendar = Calendar.getInstance()
    private var originalStartTime: String? = null
    private var originalEndTime: String? = null
    private var originalDateStr: String? = null
    private var cachedUnavailableDates: List<UnavailableDateDto> = emptyList()
    private var hasActiveConflict: Boolean = false
    private val dynamicAddonSwitches = mutableMapOf<String, Pair<com.google.android.material.materialswitch.MaterialSwitch, AvailableAddonDto>>()

    private val statusOptions = listOf("Pending", "Approved", "Denied", "Completed", "Cancelled", "Inquiry", "Responded", "Archived")

    private lateinit var networkMonitor: com.gfc.connect.api.NetworkMonitor
    private var isOfflineMode: Boolean = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRentalDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        cacheManager = RentalCacheManager(this)
        networkMonitor = com.gfc.connect.api.NetworkMonitor(this)

        rentalId = intent.getIntExtra(EXTRA_RENTAL_ID, 0)
        if (rentalId == 0) {
            Toast.makeText(this, "Invalid rental ID", Toast.LENGTH_SHORT).show()
            finish()
            return
        }

        val initialIsInquiry = intent.getBooleanExtra(EXTRA_IS_INQUIRY, false)
        setupToolbar()
        setupTabs(isInquiry = initialIsInquiry)
        if (initialIsInquiry) {
            binding.btnToggleEdit.visibility = View.GONE
        }
        setupStatusSpinner()
        setupListeners()

        // 1. Instant Cache Load
        val cached = cacheManager.getRentalDetail(rentalId)
        if (cached != null) {
            rentalDetail = cached
            populateUI(cached)
        }
        updateLastSyncLabel()

        // 2. Fresh Network Sync
        loadRentalDetail(silent = cached != null)
        observeNetwork()
    }

    private fun observeNetwork() {
        lifecycleScope.launch {
            networkMonitor.observeNetworkState().collect { online ->
                if (online && isOfflineMode) {
                    loadRentalDetail(silent = true)
                }
            }
        }
    }

    private fun setupToolbar() {
        binding.toolbarDetail.setNavigationOnClickListener {
            handleBackNavigation()
        }

        onBackPressedDispatcher.addCallback(this, object : androidx.activity.OnBackPressedCallback(true) {
            override fun handleOnBackPressed() {
                handleBackNavigation()
            }
        })

        binding.btnToggleEdit.setOnClickListener {
            toggleEditMode(!isEditMode)
        }
    }

    private fun handleBackNavigation() {
        if (isEditMode) {
            com.google.android.material.dialog.MaterialAlertDialogBuilder(this)
                .setTitle("Unsaved Changes")
                .setMessage("You have unsaved edits to this rental booking. Do you want to discard them?")
                .setPositiveButton("Discard Changes") { _, _ ->
                    toggleEditMode(false)
                    finish()
                }
                .setNegativeButton("Keep Editing", null)
                .show()
        } else {
            finish()
        }
    }

    private fun setupTabs(isInquiry: Boolean) {
        isInquiryMode = isInquiry
        val tabLayout = binding.tabLayoutSections
        tabLayout.removeAllTabs()

        if (isInquiry) {
            tabLayout.addTab(tabLayout.newTab().setText("📋 Question"))
            tabLayout.addTab(tabLayout.newTab().setText("👤 Contact"))
            tabLayout.addTab(tabLayout.newTab().setText("🛡️ History & Notes"))
        } else {
            tabLayout.addTab(tabLayout.newTab().setText("📅 Event"))
            tabLayout.addTab(tabLayout.newTab().setText("👤 Applicant"))
            tabLayout.addTab(tabLayout.newTab().setText("💵 Pricing"))
            tabLayout.addTab(tabLayout.newTab().setText("🛡️ Admin"))
        }

        tabLayout.clearOnTabSelectedListeners()
        tabLayout.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                showSection(tab?.position ?: 0)
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })
    }

    private fun showSection(position: Int) {
        if (isInquiryMode) {
            binding.sectionEvent.visibility = if (position == 0) View.VISIBLE else View.GONE
            binding.sectionRenter.visibility = if (position == 1) View.VISIBLE else View.GONE
            binding.sectionFinancials.visibility = View.GONE
            binding.sectionAdmin.visibility = if (position == 2) View.VISIBLE else View.GONE
        } else {
            binding.sectionEvent.visibility = if (position == 0) View.VISIBLE else View.GONE
            binding.sectionRenter.visibility = if (position == 1) View.VISIBLE else View.GONE
            binding.sectionFinancials.visibility = if (position == 2) View.VISIBLE else View.GONE
            binding.sectionAdmin.visibility = if (position == 3) View.VISIBLE else View.GONE
        }
        binding.scrollContent.scrollTo(0, 0)
    }

    private fun setupStatusSpinner() {
        val adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, statusOptions)
        binding.spinnerStatus.adapter = adapter
    }

    private fun setupListeners() {
        // Date Picker
        binding.btnPickDate.setOnClickListener {
            val y = selectedEventCalendar.get(Calendar.YEAR)
            val m = selectedEventCalendar.get(Calendar.MONTH)
            val d = selectedEventCalendar.get(Calendar.DAY_OF_MONTH)

            DatePickerDialog(this, { _, year, month, dayOfMonth ->
                val newCal = Calendar.getInstance().apply { set(year, month, dayOfMonth) }
                val newDateStr = SimpleDateFormat("yyyy-MM-dd", Locale.US).format(newCal.time)
                selectedEventCalendar.set(year, month, dayOfMonth)
                val sdf = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)
                binding.txtEventDate.text = sdf.format(selectedEventCalendar.time)

                // If date changed: DO NOT prepopulate or preselect times on the new date!
                if (originalDateStr != null && newDateStr != originalDateStr) {
                    binding.editStartTime.setText("")
                    binding.editEndTime.setText("")
                } else if (originalDateStr != null && newDateStr == originalDateStr) {
                    // Restored back to original booking date: restore original times!
                    binding.editStartTime.setText(originalStartTime ?: "")
                    binding.editEndTime.setText(originalEndTime ?: "")
                }
                checkScheduleConflicts()
            }, y, m, d).show()
        }

        // Room text change listener for conflict checking
        binding.editRoomSelected.addTextChangedListener {
            if (isEditMode) {
                checkScheduleConflicts()
            }
        }

        // Time Pickers (Start & End Time)
        binding.editStartTime.setOnClickListener {
            if (isEditMode) {
                promptTimePicker(isStartTime = true)
            }
        }
        binding.editEndTime.setOnClickListener {
            if (isEditMode) {
                promptTimePicker(isStartTime = false)
            }
        }

        // Phone call (Renter section & Inquiry Quick Action)
        val onCallClick = View.OnClickListener {
            val phone = binding.editPhone.text.toString().trim()
            if (phone.isNotEmpty()) {
                val intent = Intent(Intent.ACTION_DIAL, Uri.parse("tel:$phone"))
                startActivity(intent)
                promptLogCallOutcome()
            } else {
                Toast.makeText(this, "No phone number available.", Toast.LENGTH_SHORT).show()
            }
        }
        binding.btnCallRenter.setOnClickListener(onCallClick)
        binding.btnInquiryQuickCall.setOnClickListener(onCallClick)

        // SMS (Renter section & Inquiry Quick Action)
        val onSmsClick = View.OnClickListener {
            val phone = binding.editPhone.text.toString().trim()
            if (phone.isNotEmpty()) {
                val intent = Intent(Intent.ACTION_SENDTO, Uri.parse("smsto:$phone"))
                startActivity(intent)
                promptLogCorrespondence(type = "sms")
            } else {
                Toast.makeText(this, "No phone number available.", Toast.LENGTH_SHORT).show()
            }
        }
        binding.btnSmsRenter.setOnClickListener(onSmsClick)
        binding.btnInquiryQuickSms.setOnClickListener(onSmsClick)

        // Email (Renter section & Inquiry Quick Action)
        val onEmailClick = View.OnClickListener {
            val email = binding.editEmail.text.toString().trim()
            if (email.isNotEmpty()) {
                val intent = Intent(Intent.ACTION_SENDTO, Uri.parse("mailto:$email"))
                val subject = if (isInquiryMode) "Gloucester Fraternity Club - Inquiry Response" else "Gloucester Fraternity Club - Hall Rental Inquiry"
                intent.putExtra(Intent.EXTRA_SUBJECT, subject)
                startActivity(intent)
                promptLogCorrespondence(type = "email")
            } else {
                Toast.makeText(this, "No email address available.", Toast.LENGTH_SHORT).show()
            }
        }
        binding.btnEmailRenter.setOnClickListener(onEmailClick)
        binding.btnInquiryQuickEmail.setOnClickListener(onEmailClick)

        // Inquiry Quick Action: Direct Log & Archive
        binding.btnInquiryQuickLog.setOnClickListener {
            promptLogCorrespondence(type = null)
        }
        binding.btnInquiryQuickArchive.setOnClickListener {
            promptArchiveInquiry()
        }
        binding.btnArchiveInquiry.setOnClickListener {
            promptArchiveInquiry()
        }

        // Swipe to Refresh
        binding.swipeRefreshDetail.setColorSchemeResources(R.color.cyan_accent, R.color.blue_primary)
        binding.swipeRefreshDetail.setProgressBackgroundColorSchemeResource(R.color.surface_dark)
        binding.swipeRefreshDetail.setOnRefreshListener {
            loadRentalDetail(silent = true)
        }

        // Approve / Cancel / Revert from detail
        binding.btnApproveDetail.setOnClickListener { promptApprove() }
        binding.btnDenyDetail.setOnClickListener { promptCancel() }
        binding.btnRevertToPending.setOnClickListener { promptRevertToPending() }

        // Cancel / Delete from detail
        binding.btnCancelBooking.setOnClickListener { promptCancel() }
        binding.btnDeleteBooking.setOnClickListener { promptDelete() }

        // Unlink Member
        binding.btnUnlinkMember.setOnClickListener { promptUnlinkMember() }

        // Record Payment, Waive Payment & Send Reminder
        binding.btnRecordPayment.setOnClickListener { promptRecordPayment() }
        binding.btnWaivePayment.setOnClickListener { promptWaivePayment() }
        binding.btnSendPaymentReminder.setOnClickListener { promptSendPaymentReminder() }

        // Save & Cancel
        binding.btnSaveDetail.setOnClickListener { promptSaveChanges() }
        binding.btnCancelEdit.setOnClickListener {
            toggleEditMode(false)
            rentalDetail?.let { populateUI(it) }
        }

        // Paid in Full Switch Styling & Guard
        val paidThumbTint = ColorStateList(
            arrayOf(
                intArrayOf(android.R.attr.state_checked),
                intArrayOf(-android.R.attr.state_checked)
            ),
            intArrayOf(
                getColor(R.color.emerald_accent),
                getColor(R.color.text_muted)
            )
        )
        val paidTrackTint = ColorStateList(
            arrayOf(
                intArrayOf(android.R.attr.state_checked),
                intArrayOf(-android.R.attr.state_checked)
            ),
            intArrayOf(
                getColor(R.color.blue_primary),
                getColor(R.color.surface_dark_muted)
            )
        )
        binding.switchIsPaid.thumbTintList = paidThumbTint
        binding.switchIsPaid.trackTintList = paidTrackTint
        binding.switchIsPaid.isEnabled = true
        binding.switchIsPaid.isClickable = false
        binding.switchIsPaid.isFocusable = false
        binding.switchIsPaid.setOnTouchListener { _, _ -> !isEditMode }
        binding.switchIsPaid.setOnCheckedChangeListener { _, isChecked ->
            binding.switchIsPaid.setTextColor(getColor(if (isChecked) R.color.text_primary else R.color.text_secondary))
        }
    }

    private fun updateLastSyncLabel() {
        val syncTime = cacheManager.getLastSyncTime()
        if (syncTime > 0) {
            val sdf = SimpleDateFormat("h:mm a", Locale.US)
            binding.toolbarDetail.subtitle = "Synced: ${sdf.format(Date(syncTime))}"
        } else {
            binding.toolbarDetail.subtitle = null
        }
    }

    private fun loadRentalDetail(silent: Boolean = false) {
        if (!silent) {
            showLoading("Syncing Booking Details...")
        }

        fetchUnavailableDates()

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.getRentalDetail(rentalId)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body() != null) {
                        isOfflineMode = false
                        rentalDetail = response.body()
                        rentalDetail?.let {
                            cacheManager.saveRentalDetail(it)
                            populateUI(it)
                        }
                        updateLastSyncLabel()
                    } else if (rentalDetail == null) {
                        isOfflineMode = true
                        updateLastSyncLabel()
                        Toast.makeText(this@RentalDetailActivity, "Failed to load booking details.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    isOfflineMode = true
                    updateLastSyncLabel()
                    if (rentalDetail == null) {
                        Toast.makeText(this@RentalDetailActivity, "Offline: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }

    private fun populateUI(item: HallRentalDetailDto) {
        val isInquiry = item.eventType?.contains("Inquiry", ignoreCase = true) == true ||
                        item.status.equals("Inquiry", ignoreCase = true) ||
                        item.status.equals("Responded", ignoreCase = true) ||
                        item.status.equals("Archived", ignoreCase = true)

        setupTabs(isInquiry)
        binding.btnToggleEdit.visibility = if (isInquiry) View.GONE else View.VISIBLE

        // Header
        binding.txtHeaderApplicantName.text = "👤 ${item.applicantName}"
        if (isInquiry) {
            binding.txtHeaderEventType.text = "📋 ${item.eventType ?: "General Inquiry"}"
            binding.txtHeaderEventType.setTextColor(getColor(R.color.status_yellow))
            binding.txtHeaderStatus.text = if (item.status.equals("Archived", ignoreCase = true)) "ARCHIVED INQUIRY" else "GENERAL INQUIRY"
            binding.txtHeaderStatus.setTextColor(getColor(R.color.cyan_accent))
            binding.txtHeaderStatus.setBackgroundResource(R.drawable.bg_badge_inquiry)

            // Dedicated Inquiry Card & Visibility
            binding.cardInquiryOverview.visibility = View.VISIBLE
            binding.layoutBookingOnlyEventDetails.visibility = View.GONE
            binding.lblEventType.text = "Occasion / Inquiry Reason"
            binding.lblEventDate.text = "Target / Preferred Date"

            // Bind Inquiry Question
            val question = item.eventDescription?.trim()?.ifEmpty { null } 
                ?: "No specific inquiry question text recorded."
            binding.txtInquiryQuestion.text = question

            // Bind Contact Preference
            val pref = item.preferredContactMethod?.trim().orEmpty()
            val contactStr = when {
                pref.equals("both", ignoreCase = true) -> "📞 Phone & ✉️ Email"
                pref.contains("phone", ignoreCase = true) || item.requestPhoneCall -> "📞 Phone Call"
                pref.contains("email", ignoreCase = true) -> "✉️ Email"
                item.requestPhoneCall -> "📞 Phone Call Requested"
                else -> "✉️ Email"
            }
            binding.txtInquiryContactMethodBadge.text = contactStr

            // Quick Archive Buttons
            val isArchived = item.status.equals("Archived", ignoreCase = true)
            binding.btnInquiryQuickArchive.text = if (isArchived) "Restore" else "Archive"
            binding.btnArchiveInquiry.visibility = View.VISIBLE
            binding.btnArchiveInquiry.text = if (isArchived) "📦 Restore to Active Inquiries" else "📦 Archive to FAQ Pool"
            binding.btnCancelBooking.visibility = View.GONE
        } else {
            binding.cardInquiryOverview.visibility = View.GONE
            binding.layoutBookingOnlyEventDetails.visibility = View.VISIBLE
            binding.lblEventType.text = "Event Type"
            binding.lblEventDate.text = "Event Date"
            binding.btnArchiveInquiry.visibility = View.GONE
            binding.btnCancelBooking.visibility = View.VISIBLE

            binding.txtHeaderEventType.text = item.eventType ?: "Hall Rental"
            binding.txtHeaderEventType.setTextColor(getColor(R.color.cyan_accent))
            binding.txtHeaderStatus.text = item.status
            updateStatusBadgeColor(item.status)
        }

        // Submission Age & Tracking Header
        val ageText = getSubmissionAgeText(item.createdAt ?: item.createdDate)
        val waivedTotal = item.amountWaived ?: 0.0
        val isPaid = item.isPaid || ((item.amountPaid + waivedTotal) >= item.totalPrice && item.totalPrice > 0)
        val remainingBal = Math.max(0.0, item.totalPrice - waivedTotal - item.amountPaid)
        
        if (!isInquiry && isPaid) {
            binding.txtHeaderTracking.text = if (waivedTotal >= item.totalPrice && item.amountPaid == 0.0) "• 🎁 Fee Waived" else "• ✓ Paid in Full"
            binding.txtHeaderTracking.setTextColor(if (waivedTotal >= item.totalPrice && item.amountPaid == 0.0) getColor(R.color.purple_accent) else getColor(R.color.emerald_accent))
            binding.txtHeaderTracking.visibility = View.VISIBLE
        } else if (!isInquiry && (item.amountPaid > 0 || waivedTotal > 0)) {
            binding.txtHeaderTracking.text = "• 💵 Bal: $${remainingBal.toInt()}"
            binding.txtHeaderTracking.setTextColor(getColor(R.color.cyan_accent))
            binding.txtHeaderTracking.visibility = View.VISIBLE
        } else if (!isInquiry) {
            binding.txtHeaderTracking.text = "• 🔴 Unpaid"
            binding.txtHeaderTracking.setTextColor(getColor(R.color.status_red))
            binding.txtHeaderTracking.visibility = View.VISIBLE
        } else if (ageText.isNotEmpty()) {
            binding.txtHeaderTracking.text = "• ⏱️ $ageText"
            binding.txtHeaderTracking.setTextColor(getColor(R.color.text_muted))
            binding.txtHeaderTracking.visibility = View.VISIBLE
        } else {
            binding.txtHeaderTracking.visibility = View.GONE
        }

        // Parse Date & Capture Original Booking Baseline
        val isoFormat = SimpleDateFormat("yyyy-MM-dd", Locale.US)
        val displayFormat = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)
        val dateStr = item.eventDate.substringBefore('T')
        originalDateStr = dateStr
        originalStartTime = item.startTime
        originalEndTime = item.endTime
        try {
            val parsed = isoFormat.parse(dateStr)
            if (parsed != null) {
                originalEventCalendar.time = parsed
                selectedEventCalendar.time = parsed
                binding.txtEventDate.text = displayFormat.format(parsed)
                binding.txtHeaderSubtitle.text = "•  ${displayFormat.format(parsed)}"
            } else {
                binding.txtEventDate.text = item.eventDate
                binding.txtHeaderSubtitle.text = "•  ${item.eventDate}"
            }
        } catch (e: Exception) {
            binding.txtEventDate.text = item.eventDate
            binding.txtHeaderSubtitle.text = "•  ${item.eventDate}"
        }

        // Event Fields
        binding.editEventType.setText(item.eventType ?: if (isInquiry) "General Inquiry" else "Hall Rental")
        binding.editStartTime.setText(item.startTime ?: "2:00 PM")
        binding.editEndTime.setText(item.endTime ?: "7:00 PM")
        binding.editRoomSelected.setText(item.roomSelected ?: "Function Hall")
        binding.editGuestCount.setText((item.guestCount ?: 50).toString())

        // Matrix Selected
        val matrix = item.matrixSelected ?: if (item.isVerifiedMember) "Members" else "Non-Members"
        selectedMatrixTierTitle = matrix
        binding.editMatrixSelected.setText(matrix)
        binding.txtCurrentMatrixBadge.text = matrix

        // Render Matrix Tiers in Pricing & Financials Tab
        if (!isInquiry) {
            renderMatrixTiers(item)
        }

        // Member Verification Card
        if (item.isVerifiedMember) {
            val idText = if (item.verifiedMemberId != null && item.verifiedMemberId > 0) " #${item.verifiedMemberId}" else ""
            binding.txtMemberVerifyBadgeDetail.text = "VERIFIED MEMBER$idText"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.emerald_accent))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_badge_emerald)
            binding.txtMemberVerifyDetails.text = item.memberVerificationText ?: "Active member in good standing."
            binding.btnUnlinkMember.visibility = View.VISIBLE
        } else if (item.memberVerificationBadge == "UNVERIFIED_CLAIM") {
            binding.txtMemberVerifyBadgeDetail.text = "CLAIMED (NOT IN DIRECTORY)"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.status_yellow))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
            binding.txtMemberVerifyDetails.text = item.memberVerificationText ?: "Applicant selected Member pricing, but no matching record was found in the directory."
            binding.btnUnlinkMember.visibility = View.GONE
        } else {
            binding.txtMemberVerifyBadgeDetail.text = "NON-MEMBER"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.text_muted))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
            binding.txtMemberVerifyDetails.text = "Applicant is booking under standard Non-Member pricing."
            binding.btnUnlinkMember.visibility = View.GONE
        }

        // Render Possible Candidate Matches
        if (!isInquiry) {
            renderPossibleCandidates(item)
        } else {
            binding.layoutCandidatesContainer.visibility = View.GONE
        }

        // Renter Fields
        binding.editApplicantName.setText(item.applicantName)
        binding.editPhone.setText(item.requesterPhone ?: "")
        binding.editEmail.setText(item.requesterEmail ?: "")
        
        val fullAddress = sanitizeAddress(
            rawStreet = item.requesterAddress,
            city = item.requesterCity,
            state = item.requesterState,
            zip = item.requesterZip
        )
        binding.editAddress.setText(fullAddress)

        // Financials
        val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)
        binding.editTotalPrice.setText(item.totalPrice.toString())
        if (item.requireSecurityDeposit) {
            binding.layoutSecurityDepositContainer.visibility = View.VISIBLE
            binding.editSecurityDeposit.setText(item.securityDepositAmount.toString())
        } else {
            binding.layoutSecurityDepositContainer.visibility = View.GONE
            binding.editSecurityDeposit.setText("0")
        }
        binding.editAmountPaid.setText(item.amountPaid.toString())
        val waived = item.amountWaived ?: 0.0
        val remaining = Math.max(0.0, item.totalPrice - waived - item.amountPaid)
        val balanceSummary = buildString {
            append("Original Amount Due: ${currencyFormat.format(item.totalPrice)}\n")
            if (waived > 0.0) {
                append("🎁 Amount Waived: -${currencyFormat.format(waived)}\n")
            }
            append("Payments Received: ${currencyFormat.format(item.amountPaid)}\n")
            append("Remaining Balance: ${currencyFormat.format(remaining)}")
        }
        binding.txtBalanceSummary.text = balanceSummary
        binding.switchIsPaid.isChecked = item.isPaid || (remaining <= 0.0 && item.totalPrice > 0.0)
        binding.switchIsPaid.setTextColor(getColor(if (binding.switchIsPaid.isChecked) R.color.text_primary else R.color.text_secondary))

        // Dynamic Add-on Amenities
        renderAddonSwitches(item)

        // Admin & Notes
        val statusIdx = statusOptions.indexOfFirst { it.equals(item.status, ignoreCase = true) }
        if (statusIdx >= 0) {
            binding.spinnerStatus.setSelection(statusIdx)
        }
        binding.editInternalNotes.setText(item.internalNotes ?: "")

        val decisionInfo = buildString {
            if (!item.createdDate.isNullOrBlank()) {
                append("• Created: ${item.createdDate.substringBefore('T')}\n")
            }
            if (!item.approvedBy.isNullOrBlank()) {
                append("• Approved by ${item.approvedBy}")
                if (!item.approvalDate.isNullOrBlank()) append(" on ${item.approvalDate.substringBefore('T')}")
                append("\n")
            }
            if (!item.deniedBy.isNullOrBlank()) {
                append("• Denied by ${item.deniedBy}")
                if (!item.denialDate.isNullOrBlank()) append(" on ${item.denialDate.substringBefore('T')}")
                append("\n")
            }
            if (!item.statusChangedBy.isNullOrBlank() && item.statusChangedBy != item.approvedBy && item.statusChangedBy != item.deniedBy) {
                append("• Last Modified by ${item.statusChangedBy}")
                if (!item.statusChangedDate.isNullOrBlank()) append(" on ${item.statusChangedDate.substringBefore('T')}")
                append("\n")
            }
            if (isEmpty()) append(if (isInquiry) "Inquiry active and awaiting response." else "Pending administrative review.")
        }
        binding.txtDecisionInfo.text = decisionInfo.trimEnd()

        // Quick Action buttons:
        // - Approve & Cancel buttons appear for actionable PENDING booking applications
        // - Revert to Pending button appears when a booking is APPROVED
        val isPending = !isInquiry && item.status.equals("Pending", ignoreCase = true)
        val isApproved = !isInquiry && item.status.equals("Approved", ignoreCase = true)

        binding.layoutQuickActions.visibility = if (isPending) View.VISIBLE else View.GONE
        binding.btnRevertToPending.visibility = if (isApproved) View.VISIBLE else View.GONE
    }

    private fun renderPossibleCandidates(item: HallRentalDetailDto) {
        val candidates = item.possibleMembers
        if (!item.isVerifiedMember && !candidates.isNullOrEmpty()) {
            binding.layoutCandidatesContainer.visibility = View.VISIBLE
            binding.layoutCandidatesList.removeAllViews()

            for (cand in candidates) {
                val candidateCard = com.google.android.material.card.MaterialCardView(this).apply {
                    layoutParams = android.widget.LinearLayout.LayoutParams(
                        android.widget.LinearLayout.LayoutParams.MATCH_PARENT,
                        android.widget.LinearLayout.LayoutParams.WRAP_CONTENT
                    ).apply {
                        topMargin = (6 * resources.displayMetrics.density).toInt()
                    }
                    setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    radius = 8 * resources.displayMetrics.density
                    strokeColor = getColor(R.color.border_dark)
                    strokeWidth = (1 * resources.displayMetrics.density).toInt()
                }

                val rowLayout = android.widget.LinearLayout(this).apply {
                    orientation = android.widget.LinearLayout.HORIZONTAL
                    gravity = android.view.Gravity.CENTER_VERTICAL
                    val pad = (12 * resources.displayMetrics.density).toInt()
                    setPadding(pad, pad, pad, pad)
                }

                val infoLayout = android.widget.LinearLayout(this).apply {
                    orientation = android.widget.LinearLayout.VERTICAL
                    layoutParams = android.widget.LinearLayout.LayoutParams(0, android.widget.LinearLayout.LayoutParams.WRAP_CONTENT, 1f)
                }

                val txtName = android.widget.TextView(this).apply {
                    text = "${cand.fullName} #${cand.memberId} (${cand.status})"
                    setTextColor(getColor(R.color.text_primary))
                    textSize = 13f
                    setTypeface(null, android.graphics.Typeface.BOLD)
                }

                val txtReason = android.widget.TextView(this).apply {
                    text = "Match: ${cand.matchReason}"
                    setTextColor(getColor(R.color.status_yellow))
                    textSize = 11f
                }

                infoLayout.addView(txtName)
                infoLayout.addView(txtReason)

                val btnSelect = com.google.android.material.button.MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
                    text = "Link"
                    textSize = 12f
                    setPadding(
                        (10 * resources.displayMetrics.density).toInt(),
                        (4 * resources.displayMetrics.density).toInt(),
                        (10 * resources.displayMetrics.density).toInt(),
                        (4 * resources.displayMetrics.density).toInt()
                    )
                    setOnClickListener {
                        linkCandidateMember(cand)
                    }
                }

                rowLayout.addView(infoLayout)
                rowLayout.addView(btnSelect)
                candidateCard.addView(rowLayout)
                binding.layoutCandidatesList.addView(candidateCard)
            }
        } else {
            binding.layoutCandidatesContainer.visibility = View.GONE
        }
    }

    private fun renderCandidateListDirect(candidates: List<com.gfc.connect.data.models.PossibleMemberDto>?) {
        val list = candidates ?: emptyList()
        if (list.isNotEmpty()) {
            binding.layoutCandidatesContainer.visibility = View.VISIBLE
            binding.layoutCandidatesList.removeAllViews()

            for (cand in list) {
                val candidateCard = com.google.android.material.card.MaterialCardView(this).apply {
                    layoutParams = android.widget.LinearLayout.LayoutParams(
                        android.widget.LinearLayout.LayoutParams.MATCH_PARENT,
                        android.widget.LinearLayout.LayoutParams.WRAP_CONTENT
                    ).apply {
                        topMargin = (6 * resources.displayMetrics.density).toInt()
                    }
                    setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    radius = 8 * resources.displayMetrics.density
                    strokeColor = getColor(R.color.border_dark)
                    strokeWidth = (1 * resources.displayMetrics.density).toInt()
                }

                val rowLayout = android.widget.LinearLayout(this).apply {
                    orientation = android.widget.LinearLayout.HORIZONTAL
                    gravity = android.view.Gravity.CENTER_VERTICAL
                    val pad = (12 * resources.displayMetrics.density).toInt()
                    setPadding(pad, pad, pad, pad)
                }

                val infoLayout = android.widget.LinearLayout(this).apply {
                    orientation = android.widget.LinearLayout.VERTICAL
                    layoutParams = android.widget.LinearLayout.LayoutParams(0, android.widget.LinearLayout.LayoutParams.WRAP_CONTENT, 1f)
                }

                val txtName = android.widget.TextView(this).apply {
                    text = "${cand.fullName} #${cand.memberId} (${cand.status})"
                    setTextColor(getColor(R.color.text_primary))
                    textSize = 13f
                    setTypeface(null, android.graphics.Typeface.BOLD)
                }

                val txtReason = android.widget.TextView(this).apply {
                    text = "Match: ${cand.matchReason}"
                    setTextColor(getColor(R.color.status_yellow))
                    textSize = 11f
                }

                infoLayout.addView(txtName)
                infoLayout.addView(txtReason)

                val btnSelect = com.google.android.material.button.MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
                    text = "Link"
                    textSize = 12f
                    setPadding(
                        (10 * resources.displayMetrics.density).toInt(),
                        (4 * resources.displayMetrics.density).toInt(),
                        (10 * resources.displayMetrics.density).toInt(),
                        (4 * resources.displayMetrics.density).toInt()
                    )
                    setOnClickListener {
                        linkCandidateMember(cand)
                    }
                }

                rowLayout.addView(infoLayout)
                rowLayout.addView(btnSelect)
                candidateCard.addView(rowLayout)
                binding.layoutCandidatesList.addView(candidateCard)
            }
        } else {
            binding.layoutCandidatesContainer.visibility = View.GONE
        }
    }

    private fun linkCandidateMember(candidate: com.gfc.connect.data.models.PossibleMemberDto) {
        MaterialAlertDialogBuilder(this)
            .setTitle("Link Applicant to Member")
            .setMessage("Link this rental booking to ${candidate.fullName} (#${candidate.memberId})?\n\nThis will set the Applicant Name to '${candidate.fullName}' and ensure Member pricing is verified.")
            .setPositiveButton("Confirm & Link") { _, _ ->
                binding.editApplicantName.setText(candidate.fullName)
                binding.editMatrixSelected.setText("Member")
                if (!candidate.phone.isNullOrBlank() && binding.editPhone.text.isNullOrBlank()) {
                    binding.editPhone.setText(candidate.phone)
                }
                if (!candidate.email.isNullOrBlank() && binding.editEmail.text.isNullOrBlank()) {
                    binding.editEmail.setText(candidate.email)
                }
                binding.txtMemberVerifyBadgeDetail.text = "VERIFIED: ${candidate.fullName} #${candidate.memberId}"
                binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.emerald_accent))
                binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_badge_emerald)
                binding.txtMemberVerifyDetails.text = "Linked to directory member #${candidate.memberId} (${candidate.status})."
                binding.btnUnlinkMember.visibility = View.VISIBLE
                binding.layoutCandidatesContainer.visibility = View.GONE
                
                toggleEditMode(true)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun promptUnlinkMember() {
        MaterialAlertDialogBuilder(this)
            .setTitle("Unlink Member Verification")
            .setMessage("Are you sure you want to unlink this member verification?\n\nThe booking will remain under Member pricing as an unverified claim, and directory candidate matches will be redisplayed for review.")
            .setPositiveButton("Unlink") { _, _ ->
                binding.txtMemberVerifyBadgeDetail.text = "CLAIMED (NOT IN DIRECTORY)"
                binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.status_yellow))
                binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
                binding.txtMemberVerifyDetails.text = "Member verification cleared. Review candidate matches below or tap 'Save Changes' to commit."
                binding.btnUnlinkMember.visibility = View.GONE
                
                val name = binding.editApplicantName.text.toString().trim()
                val email = binding.editEmail.text.toString().trim()
                val phone = binding.editPhone.text.toString().trim()
                verifyMemberStatusLive(name, email, phone)

                if (!isEditMode) {
                    toggleEditMode(true)
                }
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun verifyMemberStatusLive(name: String, email: String, phone: String) {
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val resp = ApiClient.service.verifyMemberLive(name, email, phone, true)
                withContext(Dispatchers.Main) {
                    if (resp.isSuccessful && resp.body() != null) {
                        val result = resp.body()!!
                        if (result.isVerified) {
                            val idText = if (result.memberId != null && result.memberId > 0) " #${result.memberId}" else ""
                            binding.txtMemberVerifyBadgeDetail.text = "VERIFIED MEMBER$idText"
                            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.emerald_accent))
                            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_badge_emerald)
                            binding.txtMemberVerifyDetails.text = result.statusText
                            binding.btnUnlinkMember.visibility = View.VISIBLE
                            binding.layoutCandidatesContainer.visibility = View.GONE
                        } else {
                            binding.txtMemberVerifyBadgeDetail.text = "CLAIMED (NOT IN DIRECTORY)"
                            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.status_yellow))
                            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
                            binding.txtMemberVerifyDetails.text = result.statusText
                            binding.btnUnlinkMember.visibility = View.GONE
                            renderCandidateListDirect(result.candidates)
                        }
                    } else {
                        binding.txtMemberVerifyBadgeDetail.text = "UNVERIFIED"
                        binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.status_yellow))
                        binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
                        binding.txtMemberVerifyDetails.text = "Could not verify member status with server (Code: ${resp.code()})."
                        binding.btnUnlinkMember.visibility = View.GONE
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    binding.txtMemberVerifyBadgeDetail.text = "OFFLINE"
                    binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.text_muted))
                    binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
                    binding.txtMemberVerifyDetails.text = "Directory check offline: ${e.localizedMessage}"
                    binding.btnUnlinkMember.visibility = View.GONE
                }
            }
        }
    }

    private fun renderMatrixTiers(item: HallRentalDetailDto) {
        val container = binding.containerMatrixTiers
        container.removeAllViews()

        val tiers = item.availableMatrixTiers ?: emptyList()
        if (tiers.isEmpty()) {
            val emptyTv = TextView(this).apply {
                text = "No matrix tiers configured on server."
                setTextColor(getColor(R.color.text_muted))
                textSize = 13f
            }
            container.addView(emptyTv)
            return
        }

        val activeSelected = selectedMatrixTierTitle ?: item.matrixSelected ?: if (item.isVerifiedMember) "Members" else "Non-Members"
        selectedMatrixTierTitle = activeSelected
        binding.txtCurrentMatrixBadge.text = activeSelected

        val dp6 = (6 * resources.displayMetrics.density).toInt()
        val dp8 = (8 * resources.displayMetrics.density).toInt()
        val dp12 = (12 * resources.displayMetrics.density).toInt()

        for (tier in tiers) {
            val isSelected = activeSelected.equals(tier.title, ignoreCase = true) ||
                             activeSelected.equals(tier.id, ignoreCase = true) ||
                             activeSelected.equals(tier.associatedRenterType, ignoreCase = true)

            val card = MaterialCardView(this).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    setMargins(0, 0, 0, dp8)
                }
                radius = 12 * resources.displayMetrics.density
                strokeWidth = if (isSelected) (2 * resources.displayMetrics.density).toInt() else (1 * resources.displayMetrics.density).toInt()
                strokeColor = if (isSelected) getColor(R.color.cyan_accent) else getColor(R.color.border_dark)
                setCardBackgroundColor(if (isSelected) getColor(R.color.surface_dark) else getColor(R.color.surface_dark_muted))
                isClickable = true
                isFocusable = true

                val rowLayout = LinearLayout(context).apply {
                    orientation = LinearLayout.HORIZONTAL
                    setPadding(dp12, dp12, dp12, dp12)
                    gravity = Gravity.CENTER_VERTICAL
                }

                // Info block
                val infoLayout = LinearLayout(context).apply {
                    orientation = LinearLayout.VERTICAL
                    layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f)
                }

                val titleRow = LinearLayout(context).apply {
                    orientation = LinearLayout.HORIZONTAL
                    gravity = Gravity.CENTER_VERTICAL
                }

                val titleTv = TextView(context).apply {
                    text = tier.title
                    setTextColor(if (isSelected) getColor(R.color.cyan_accent) else getColor(R.color.text_primary))
                    textSize = 14f
                    setTypeface(null, Typeface.BOLD)
                }
                titleRow.addView(titleTv)

                if (tier.isAvailableForDate) {
                    val rateBadge = TextView(context).apply {
                        text = "$${tier.rateForDate.toInt()} Base"
                        setTextColor(getColor(R.color.emerald_accent))
                        textSize = 12f
                        setTypeface(null, Typeface.BOLD)
                        setBackgroundResource(R.drawable.bg_badge_emerald)
                        setPadding(dp8, dp6 / 2, dp8, dp6 / 2)
                        val params = LinearLayout.LayoutParams(
                            LinearLayout.LayoutParams.WRAP_CONTENT,
                            LinearLayout.LayoutParams.WRAP_CONTENT
                        ).apply {
                            marginStart = dp8
                        }
                        layoutParams = params
                    }
                    titleRow.addView(rateBadge)
                } else {
                    val unavailBadge = TextView(context).apply {
                        text = "Unavailable on this day"
                        setTextColor(getColor(R.color.coral_red))
                        textSize = 11f
                        val params = LinearLayout.LayoutParams(
                            LinearLayout.LayoutParams.WRAP_CONTENT,
                            LinearLayout.LayoutParams.WRAP_CONTENT
                        ).apply {
                            marginStart = dp8
                        }
                        layoutParams = params
                    }
                    titleRow.addView(unavailBadge)
                }
                infoLayout.addView(titleRow)

                if (!tier.subtitle.isNullOrBlank()) {
                    val subTv = TextView(context).apply {
                        text = tier.subtitle
                        setTextColor(getColor(R.color.text_muted))
                        textSize = 12f
                        setPadding(0, dp6 / 2, 0, 0)
                    }
                    infoLayout.addView(subTv)
                }

                rowLayout.addView(infoLayout)

                // Select indicator button
                val selectBtn = MaterialButton(context, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
                    layoutParams = LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.WRAP_CONTENT,
                        (36 * resources.displayMetrics.density).toInt()
                    ).apply {
                        marginStart = dp8
                    }
                    textSize = 12f
                    cornerRadius = (8 * resources.displayMetrics.density).toInt()
                    if (isSelected) {
                        text = "Selected ✓"
                        setTextColor(getColor(R.color.bg_dark))
                        backgroundTintList = ColorStateList.valueOf(getColor(R.color.cyan_accent))
                        strokeColor = ColorStateList.valueOf(getColor(R.color.cyan_accent))
                        isEnabled = isEditMode
                    } else {
                        text = "Select"
                        setTextColor(if (isEditMode) getColor(R.color.cyan_accent) else getColor(R.color.text_muted))
                        strokeColor = ColorStateList.valueOf(getColor(R.color.border_dark))
                        visibility = if (isEditMode) View.VISIBLE else View.GONE
                        isEnabled = isEditMode
                    }
                }

                if (isEditMode) {
                    val onTierClick = View.OnClickListener {
                        applySelectedMatrixTier(tier)
                    }
                    selectBtn.setOnClickListener(onTierClick)
                    setOnClickListener(onTierClick)
                    isClickable = true
                    isFocusable = true
                } else {
                    isClickable = false
                    isFocusable = false
                    setOnClickListener(null)
                    selectBtn.setOnClickListener(null)
                }

                rowLayout.addView(selectBtn)
                addView(rowLayout)
            }
            container.addView(card)
        }
    }

    private fun applySelectedMatrixTier(tier: AvailableMatrixTierDto) {
        if (!isEditMode) return
        selectedMatrixTierTitle = tier.title
        binding.editMatrixSelected.setText(tier.title)
        binding.txtCurrentMatrixBadge.text = tier.title

        val isMemberTier = (!tier.title.contains("Non", ignoreCase = true) && tier.title.contains("Member", ignoreCase = true)) ||
                           (!tier.associatedRenterType.isNullOrEmpty() && !tier.associatedRenterType.contains("Non", ignoreCase = true) && tier.associatedRenterType.contains("Member", ignoreCase = true)) ||
                           (!tier.id.isNullOrEmpty() && !tier.id.contains("non", ignoreCase = true) && tier.id.contains("member", ignoreCase = true))

        if (isMemberTier) {
            val name = binding.editApplicantName.text.toString().trim().ifEmpty { rentalDetail?.applicantName.orEmpty() }
            val email = binding.editEmail.text.toString().trim().ifEmpty { rentalDetail?.requesterEmail.orEmpty() }
            val phone = binding.editPhone.text.toString().trim().ifEmpty { rentalDetail?.requesterPhone.orEmpty() }

            // Immediate visual feedback on the Pricing tab
            binding.txtMemberVerifyBadgeDetail.text = "CHECKING DIRECTORY..."
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.cyan_accent))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
            binding.txtMemberVerifyDetails.text = "Searching directory for '$name'..."
            binding.btnUnlinkMember.visibility = View.GONE
            binding.layoutCandidatesContainer.visibility = View.GONE

            verifyMemberStatusLive(name, email, phone)
        } else {
            binding.txtMemberVerifyBadgeDetail.text = "NON-MEMBER"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.text_muted))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
            binding.txtMemberVerifyDetails.text = "Applicant is booking under standard Non-Member pricing."
            binding.btnUnlinkMember.visibility = View.GONE
            binding.layoutCandidatesContainer.visibility = View.GONE
        }

        // Recalculate price dynamically with active add-on amenities
        recalculateDynamicPrice(tier)

        if (!isEditMode) {
            toggleEditMode(true)
        }
        rentalDetail?.let { renderMatrixTiers(it) }
    }

    private fun renderAddonSwitches(item: HallRentalDetailDto) {
        binding.layoutAddonsList.removeAllViews()
        dynamicAddonSwitches.clear()

        val addons = item.availableAddons?.filter { it.isActive } ?: emptyList()
        if (addons.isEmpty()) {
            binding.txtNoAddons.visibility = View.VISIBLE
            return
        }
        binding.txtNoAddons.visibility = View.GONE

        val dp4 = (4 * resources.displayMetrics.density).toInt()

        val addonThumbTint = ColorStateList(
            arrayOf(
                intArrayOf(android.R.attr.state_checked),
                intArrayOf(-android.R.attr.state_checked)
            ),
            intArrayOf(
                getColor(R.color.cyan_accent),
                getColor(R.color.text_muted)
            )
        )
        val addonTrackTint = ColorStateList(
            arrayOf(
                intArrayOf(android.R.attr.state_checked),
                intArrayOf(-android.R.attr.state_checked)
            ),
            intArrayOf(
                getColor(R.color.blue_primary),
                getColor(R.color.surface_dark_muted)
            )
        )

        for (addon in addons) {
            val isInitiallySelected = addon.isSelected || when {
                addon.id == "addon_bar" || addon.name.contains("Bar", ignoreCase = true) -> item.bartenderRequested
                addon.id == "addon_kitchen" || addon.name.contains("Kitchen", ignoreCase = true) -> item.kitchenUsage
                addon.id == "addon_av" || addon.name.contains("AV", ignoreCase = true) || addon.name.contains("Sound", ignoreCase = true) -> item.avEquipmentUsage
                else -> false
            }

            val switchView = com.google.android.material.materialswitch.MaterialSwitch(this).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    setMargins(0, dp4, 0, dp4)
                }
                text = "${addon.name} (+$${addon.fee.toInt()})"
                setTextColor(getColor(if (isInitiallySelected) R.color.text_primary else R.color.text_secondary))
                textSize = 14f
                thumbTintList = addonThumbTint
                trackTintList = addonTrackTint
                isChecked = isInitiallySelected
                isEnabled = true
                isClickable = isEditMode
                isFocusable = isEditMode
                setOnTouchListener { _, _ -> !isEditMode }
                setOnCheckedChangeListener { _, isChecked ->
                    setTextColor(getColor(if (isChecked) R.color.text_primary else R.color.text_secondary))
                    if (isEditMode) {
                        recalculateDynamicPrice()
                    }
                }
            }

            binding.layoutAddonsList.addView(switchView)
            dynamicAddonSwitches[addon.id] = Pair(switchView, addon)
        }
    }

    private fun recalculateDynamicPrice(selectedTier: AvailableMatrixTierDto? = null) {
        val tier = selectedTier ?: rentalDetail?.availableMatrixTiers?.firstOrNull { it.title.equals(selectedMatrixTierTitle, ignoreCase = true) }
        var total = tier?.rateForDate ?: (rentalDetail?.totalPrice ?: 0.0)

        for ((_, pair) in dynamicAddonSwitches) {
            val (switch, addon) = pair
            if (switch.isChecked) {
                total += addon.fee
            }
        }
        binding.editTotalPrice.setText(String.format(Locale.US, "%.2f", total))
    }

    private fun updateStatusBadgeColor(status: String) {
        when {
            status.contains("Approve", ignoreCase = true) || status.contains("Confirm", ignoreCase = true) -> {
                binding.txtHeaderStatus.setTextColor(getColor(R.color.emerald_accent))
                binding.txtHeaderStatus.setBackgroundResource(R.drawable.bg_badge_emerald)
            }
            status.contains("Den", ignoreCase = true) || status.contains("Cancel", ignoreCase = true) -> {
                binding.txtHeaderStatus.setTextColor(getColor(R.color.coral_red))
                binding.txtHeaderStatus.setBackgroundResource(R.drawable.bg_badge_emerald)
            }
            else -> {
                binding.txtHeaderStatus.setTextColor(getColor(R.color.cyan_accent))
                binding.txtHeaderStatus.setBackgroundResource(R.drawable.bg_badge_emerald)
            }
        }
    }

    private fun sanitizeAddress(rawStreet: String?, city: String?, state: String?, zip: String?): String {
        var street = rawStreet?.trim().orEmpty()
        if (street.isEmpty()) {
            val parts = listOfNotNull(
                city?.trim()?.ifEmpty { null },
                state?.trim()?.ifEmpty { null },
                zip?.trim()?.ifEmpty { null }
            )
            return parts.joinToString(", ")
        }

        // De-duplicate repeated chunks within the street string itself (e.g., "123 Main, Pawtucket, RI, Pawtucket, RI")
        val chunks = street.split(",").map { it.trim() }.filter { it.isNotEmpty() }
        val uniqueChunks = mutableListOf<String>()
        for (chunk in chunks) {
            if (!uniqueChunks.any { it.equals(chunk, ignoreCase = true) }) {
                uniqueChunks.add(chunk)
            }
        }
        street = uniqueChunks.joinToString(", ")

        // If city/state/zip are provided separately, only append them if they aren't already present in street
        val cleanCity = city?.trim().orEmpty()
        val cleanState = state?.trim().orEmpty()
        val cleanZip = zip?.trim().orEmpty()

        val needsCity = cleanCity.isNotEmpty() && !street.contains(cleanCity, ignoreCase = true)
        val needsState = cleanState.isNotEmpty() && !street.contains(cleanState, ignoreCase = true)
        val needsZip = cleanZip.isNotEmpty() && !street.contains(cleanZip, ignoreCase = true)

        val appendParts = mutableListOf<String>()
        if (needsCity) appendParts.add(cleanCity)
        if (needsState) appendParts.add(cleanState)
        if (needsZip) appendParts.add(cleanZip)

        return if (appendParts.isNotEmpty()) {
            "$street, ${appendParts.joinToString(", ")}"
        } else {
            street
        }
    }

    private fun toggleEditMode(enable: Boolean) {
        if (enable && (isOfflineMode || !networkMonitor.isOnline)) {
            com.google.android.material.dialog.MaterialAlertDialogBuilder(this)
                .setTitle("⚠️ Offline Mode (View Only)")
                .setMessage("You are currently disconnected from the server. Editing booking details is disabled while offline to avoid conflicting changes. Please connect to the network to edit.")
                .setPositiveButton("OK", null)
                .show()
            return
        }

        isEditMode = enable
        binding.btnToggleEdit.text = if (enable) "Done" else "Edit"
        binding.cardSaveBar.visibility = if (enable) View.VISIBLE else View.GONE
        binding.btnPickDate.visibility = if (enable) View.VISIBLE else View.GONE

        // Enable / Disable inputs
        binding.editEventType.isEnabled = enable
        binding.editStartTime.isClickable = enable
        binding.editEndTime.isClickable = enable
        binding.editStartTime.setTextColor(getColor(if (enable) R.color.cyan_accent else R.color.text_primary))
        binding.editEndTime.setTextColor(getColor(if (enable) R.color.cyan_accent else R.color.text_primary))
        binding.editRoomSelected.isEnabled = false
        binding.editRoomSelected.isFocusable = false
        binding.editGuestCount.isEnabled = enable
        binding.editMatrixSelected.isEnabled = false
        binding.editMatrixSelected.isFocusable = false

        binding.editApplicantName.isEnabled = enable
        binding.editPhone.isEnabled = enable
        binding.editEmail.isEnabled = enable
        binding.editAddress.isEnabled = enable

        binding.editTotalPrice.isEnabled = enable
        binding.editSecurityDeposit.isEnabled = enable
        binding.editAmountPaid.isEnabled = enable
        
        binding.switchIsPaid.isEnabled = true
        binding.switchIsPaid.isClickable = enable
        binding.switchIsPaid.isFocusable = enable
        binding.switchIsPaid.setOnTouchListener { _, _ -> !enable }
        
        for ((_, pair) in dynamicAddonSwitches) {
            val switch = pair.first
            switch.isEnabled = true
            switch.isClickable = enable
            switch.isFocusable = enable
            switch.setOnTouchListener { _, _ -> !enable }
        }

        binding.spinnerStatus.isEnabled = enable
        binding.editInternalNotes.isEnabled = enable

        rentalDetail?.let { renderMatrixTiers(it) }

        if (enable) {
            binding.editEventType.requestFocus()
            fetchUnavailableDates {
                checkScheduleConflicts()
            }
        } else {
            binding.cardConflictWarning.visibility = View.GONE
            binding.layoutQuickSelectSlots.visibility = View.GONE
        }
    }

    private fun showLoading(message: String = "Loading...") {
        binding.txtLoadingMessage.text = message
        binding.layoutLoadingOverlay.visibility = View.VISIBLE
    }

    private fun hideLoading() {
        binding.layoutLoadingOverlay.visibility = View.GONE
        binding.swipeRefreshDetail.isRefreshing = false
    }

    private fun promptSaveChanges() {
        val isoFormat = SimpleDateFormat("yyyy-MM-dd'T'00:00:00", Locale.US)
        val displayDateFormat = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)
        val eventDateStr = isoFormat.format(selectedEventCalendar.time)
        val eventDateDisplay = displayDateFormat.format(selectedEventCalendar.time)

        val newApplicantName = binding.editApplicantName.text.toString().trim().ifEmpty { null }
        val newPhone = binding.editPhone.text.toString().trim().ifEmpty { null }
        val newEmail = binding.editEmail.text.toString().trim().ifEmpty { null }
        val newAddress = binding.editAddress.text.toString().trim().ifEmpty { null }
        val newEventType = binding.editEventType.text.toString().trim().ifEmpty { null }
        val newStartTime = binding.editStartTime.text.toString().trim().ifEmpty { null }
        val newEndTime = binding.editEndTime.text.toString().trim().ifEmpty { null }
        val newRoom = binding.editRoomSelected.text.toString().trim().ifEmpty { null }
        val newGuests = binding.editGuestCount.text.toString().toIntOrNull()
        val newTotal = binding.editTotalPrice.text.toString().toDoubleOrNull()
        val newDeposit = binding.editSecurityDeposit.text.toString().toDoubleOrNull()
        val newAmountPaid = binding.editAmountPaid.text.toString().toDoubleOrNull()
        val newIsPaid = binding.switchIsPaid.isChecked
        
        var newBar = false
        var newKitchen = false
        var newAv = false
        for ((_, pair) in dynamicAddonSwitches) {
            val (switch, addon) = pair
            val isChecked = switch.isChecked
            if (addon.id == "addon_bar" || addon.name.contains("Bar", ignoreCase = true)) newBar = isChecked
            if (addon.id == "addon_kitchen" || addon.name.contains("Kitchen", ignoreCase = true)) newKitchen = isChecked
            if (addon.id == "addon_av" || addon.name.contains("AV", ignoreCase = true) || addon.name.contains("Sound", ignoreCase = true)) newAv = isChecked
        }

        val newStatus = binding.spinnerStatus.selectedItem?.toString()
        val newNotes = binding.editInternalNotes.text.toString().trim()
        val newMatrix = selectedMatrixTierTitle ?: binding.editMatrixSelected.text.toString().trim().ifEmpty { null }

        // Compute detected modifications
        val diffs = mutableListOf<String>()
        val cur = rentalDetail
        if (cur != null) {
            val oldDateStr = try {
                val parsed = SimpleDateFormat("yyyy-MM-dd", Locale.US).parse(cur.eventDate.substringBefore('T'))
                if (parsed != null) displayDateFormat.format(parsed) else cur.eventDate
            } catch (e: Exception) { cur.eventDate }

            if (cur.eventDate.substringBefore('T') != eventDateStr.substringBefore('T')) {
                diffs.add("📅 Event Date: $oldDateStr ➔ $eventDateDisplay")
            }

            val oldMatrix = cur.matrixSelected ?: (if (cur.memberStatus) "Members" else "Non-Members")
            if (!oldMatrix.equals(newMatrix, ignoreCase = true)) {
                diffs.add("📊 Pricing Tier: $oldMatrix ➔ $newMatrix")
            }

            if (newTotal != null && Math.abs(cur.totalPrice - newTotal) > 0.01) {
                diffs.add("💵 Total Quote: $${cur.totalPrice.toInt()} ➔ $${newTotal.toInt()}")
            }

            if (newStatus != null && !cur.status.equals(newStatus, ignoreCase = true)) {
                diffs.add("🛡️ Status: ${cur.status} ➔ $newStatus")
                if (newStatus.equals("Approved", ignoreCase = true) && !newIsPaid && ((newTotal ?: cur.totalPrice) > (newAmountPaid ?: cur.amountPaid))) {
                    val unpaidAmount = ((newTotal ?: cur.totalPrice) - (newAmountPaid ?: cur.amountPaid)).toInt()
                    diffs.add("⚠️ Approving with UNPAID balance of $$unpaidAmount")
                }
            }

            if (newGuests != null && cur.guestCount != newGuests) {
                diffs.add("👥 Guest Count: ${cur.guestCount ?: 0} ➔ $newGuests")
            }

            if (newRoom != null && !cur.roomSelected.equals(newRoom, ignoreCase = true)) {
                diffs.add("🚪 Room: ${cur.roomSelected ?: "Function Hall"} ➔ $newRoom")
            }

            for ((_, pair) in dynamicAddonSwitches) {
                val (switch, addon) = pair
                val isChecked = switch.isChecked
                val oldChecked = when {
                    addon.id == "addon_bar" || addon.name.contains("Bar", ignoreCase = true) -> cur.bartenderRequested
                    addon.id == "addon_kitchen" || addon.name.contains("Kitchen", ignoreCase = true) -> cur.kitchenUsage
                    addon.id == "addon_av" || addon.name.contains("AV", ignoreCase = true) || addon.name.contains("Sound", ignoreCase = true) -> cur.avEquipmentUsage
                    else -> addon.isSelected
                }
                if (oldChecked != isChecked) {
                    diffs.add("➕ ${addon.name}: ${if (isChecked) "Added (+$${addon.fee.toInt()})" else "Removed"}")
                }
            }

            if (cur.isPaid != newIsPaid) {
                diffs.add("💳 Payment Status: ${if (newIsPaid) "Marked as Paid" else "Unpaid"}")
            }
        }

        // Build Custom Review Dialog View
        val dp16 = (16 * resources.displayMetrics.density).toInt()
        val dp8 = (8 * resources.displayMetrics.density).toInt()
        val dp12 = (12 * resources.displayMetrics.density).toInt()

        val dialogContent = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp16, dp12, dp16, dp8)
        }

        val messageTv = TextView(this).apply {
            text = if (diffs.isNotEmpty()) {
                "The following ${diffs.size} modification(s) will be committed to the server:"
            } else {
                "No major field changes detected. Commit latest specifications?"
            }
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, 0, 0, dp8)
        }
        dialogContent.addView(messageTv)

        if (diffs.isNotEmpty()) {
            val diffsBox = LinearLayout(this).apply {
                orientation = LinearLayout.VERTICAL
                setBackgroundResource(R.drawable.bg_surface_card)
                setPadding(dp12, dp8, dp12, dp8)
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    bottomMargin = dp12
                }
            }
            for (diff in diffs) {
                val itemTv = TextView(this).apply {
                    text = "• $diff"
                    setTextColor(getColor(R.color.text_primary))
                    textSize = 13f
                    setTypeface(null, Typeface.BOLD)
                    setPadding(0, dp8 / 2, 0, dp8 / 2)
                }
                diffsBox.addView(itemTv)
            }
            dialogContent.addView(diffsBox)
        }

        // Modification Reason & Admin Note Quick-Picks
        val reasonLabel = TextView(this).apply {
            text = "Modification Reason / Staff Note (Optional):"
            setTextColor(getColor(R.color.cyan_accent))
            textSize = 12f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 0, 0, dp8 / 2)
        }
        dialogContent.addView(reasonLabel)

        val presetList = mutableListOf("-- Select a reason preset (Optional) --")
        val availablePresets = rentalDetail?.modificationReasonPresets
        if (!availablePresets.isNullOrEmpty()) {
            presetList.addAll(availablePresets)
        } else {
            presetList.addAll(
                listOf(
                    "Applied member pricing discount",
                    "Adjusted event time slot per applicant request",
                    "Updated room assignment / facility space",
                    "Added beverage & bartender services",
                    "Updated guest count & seating configuration",
                    "Modified security deposit / fee schedule",
                    "Booking details verified & approved",
                    "Other / Custom modification"
                )
            )
        }

        val reasonNoteInput = EditText(this).apply {
            hint = "Type a reason or note for this change..."
            setTextColor(getColor(R.color.text_primary))
            setHintTextColor(getColor(R.color.text_muted))
            textSize = 13f
            setBackgroundResource(R.drawable.bg_surface_card)
            setPadding(dp12, dp8, dp12, dp8)
            layoutParams = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
            ).apply {
                bottomMargin = dp12
            }
        }

        val reasonSpinner = Spinner(this).apply {
            layoutParams = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                (44 * resources.displayMetrics.density).toInt()
            ).apply {
                bottomMargin = dp8 / 2
            }
            val adapter = ArrayAdapter(this@RentalDetailActivity, android.R.layout.simple_spinner_dropdown_item, presetList)
            this.adapter = adapter
            onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
                override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                    if (position > 0) {
                        val selected = presetList[position]
                        if (!selected.contains("Other", ignoreCase = true)) {
                            reasonNoteInput.setText(selected)
                        }
                    }
                }
                override fun onNothingSelected(parent: AdapterView<*>?) {}
            }
        }
        dialogContent.addView(reasonSpinner)
        dialogContent.addView(reasonNoteInput)

        val recipientEmail = newEmail ?: rentalDetail?.requesterEmail
        val hasEmail = !recipientEmail.isNullOrBlank()

        val emailCheckBox = com.google.android.material.checkbox.MaterialCheckBox(this).apply {
            text = if (hasEmail) {
                "📧 Send updated booking confirmation email to applicant ($recipientEmail)"
            } else {
                "📧 Send confirmation email (No applicant email provided)"
            }
            isChecked = hasEmail
            isEnabled = hasEmail
            setTextColor(getColor(if (hasEmail) R.color.cyan_accent else R.color.text_muted))
            textSize = 12f
        }
        dialogContent.addView(emailCheckBox)

        if (hasActiveConflict) {
            val conflictBanner = TextView(this).apply {
                text = "⚠️ CALENDAR CONFLICT DETECTED:\n${binding.txtConflictDetails.text}"
                setTextColor(getColor(R.color.status_red))
                setBackgroundResource(R.drawable.bg_surface_card)
                setPadding(24, 20, 24, 20)
                setTypeface(null, Typeface.BOLD)
                textSize = 12f
                val lp = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT)
                lp.setMargins(0, 0, 0, 16)
                layoutParams = lp
            }
            dialogContent.addView(conflictBanner, 0)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle(if (hasActiveConflict) "⚠️ Review Changes (Conflict Warning)" else "📝 Review & Confirm Changes")
            .setView(dialogContent)
            .setPositiveButton("Confirm & Save") { _, _ ->
                val chosenReason = reasonNoteInput.text.toString().trim().ifEmpty { null }
                val payload = UpdateRentalPayload(
                    applicantName = newApplicantName,
                    requesterPhone = newPhone,
                    requesterEmail = newEmail,
                    requesterAddress = newAddress,
                    eventDate = eventDateStr,
                    eventType = newEventType,
                    startTime = newStartTime,
                    endTime = newEndTime,
                    roomSelected = newRoom,
                    guestCount = newGuests,
                    totalPrice = newTotal,
                    securityDepositAmount = newDeposit,
                    amountPaid = newAmountPaid,
                    isPaid = newIsPaid,
                    bartenderRequested = newBar,
                    kitchenUsage = newKitchen,
                    avEquipmentUsage = newAv,
                    status = newStatus,
                    internalNotes = newNotes,
                    matrixSelected = newMatrix,
                    sendUpdateEmail = emailCheckBox.isChecked,
                    changeReasonNote = chosenReason
                )
                executeSave(payload, recipientEmail, emailCheckBox.isChecked)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeSave(payload: UpdateRentalPayload, emailRecipient: String?, emailRequested: Boolean) {
        showLoading("Saving Changes & Syncing...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.updateRental(rentalId, payload)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        val emailMsg = if (emailRequested && !emailRecipient.isNullOrBlank()) " (Confirmation email sent to $emailRecipient)" else ""
                        Toast.makeText(this@RentalDetailActivity, "✅ Booking updated successfully!$emailMsg", Toast.LENGTH_LONG).show()
                        toggleEditMode(false)
                        loadRentalDetail()
                    } else {
                        val errBody = response.errorBody()?.string()
                        val errMsg = try {
                            if (!errBody.isNullOrBlank()) {
                                val json = JSONObject(errBody)
                                json.optString("error").ifEmpty { json.optString("message", "Failed to save changes.") }
                            } else "Failed to save changes."
                        } catch (_: Exception) {
                            errBody ?: "Failed to save changes."
                        }
                        MaterialAlertDialogBuilder(this@RentalDetailActivity)
                            .setTitle("⚠️ Cannot Save Changes")
                            .setMessage(errMsg)
                            .setPositiveButton("OK", null)
                            .show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Save error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun checkOfflineAndAlert(): Boolean {
        if (isOfflineMode || !networkMonitor.isOnline) {
            MaterialAlertDialogBuilder(this)
                .setTitle("⚠️ Offline Mode (View Only)")
                .setMessage("You are currently disconnected from the server. Booking actions, payments, and edits cannot be submitted while offline. Please connect to the network to perform this action.")
                .setPositiveButton("OK", null)
                .show()
            return true
        }
        return false
    }

    private fun promptApprove() {
        if (checkOfflineAndAlert()) return
        val item = rentalDetail
        val paidAmount = item?.amountPaid ?: 0.0
        val totalQuote = item?.totalPrice ?: 0.0
        val isPaid = item?.isPaid == true || (paidAmount >= totalQuote && totalQuote > 0)
        val balanceDue = Math.max(0.0, totalQuote - paidAmount)

        val input = EditText(this).apply {
            hint = "Optional approval note..."
            setPadding(40, 24, 40, 24)
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setTextColor(getColor(R.color.text_primary))
        }

        val dialogContent = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(48, 24, 48, 16)
        }

        if (!isPaid && balanceDue > 0) {
            val warningTv = TextView(this).apply {
                text = "⚠️ UNPAID BOOKING WARNING:\nThis rental has an outstanding balance of $${balanceDue.toInt()} (Paid: $${paidAmount.toInt()} of $${totalQuote.toInt()}).\n\nAre you sure you want to approve this booking before full payment is received?"
                setTextColor(getColor(R.color.gold_accent))
                textSize = 13f
                setTypeface(null, Typeface.BOLD)
                setPadding(0, 0, 0, 16)
            }
            dialogContent.addView(warningTv)
        } else {
            val infoTv = TextView(this).apply {
                text = "Confirm approval for this hall rental booking?"
                setTextColor(getColor(R.color.text_primary))
                textSize = 14f
                setPadding(0, 0, 0, 16)
            }
            dialogContent.addView(infoTv)
        }

        dialogContent.addView(input)

        MaterialAlertDialogBuilder(this)
            .setTitle(if (!isPaid && balanceDue > 0) "⚠️ Approve Unpaid Booking" else "✅ Approve Rental Request")
            .setView(dialogContent)
            .setPositiveButton(if (!isPaid && balanceDue > 0) "Approve Without Full Payment" else "Approve") { _, _ ->
                val note = input.text.toString().trim()
                executeApproval(note)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun promptTimePicker(isStartTime: Boolean) {
        val currentText = if (isStartTime) binding.editStartTime.text.toString().trim() else binding.editEndTime.text.toString().trim()
        val cal = Calendar.getInstance()
        try {
            val sdf = SimpleDateFormat("h:mm a", Locale.US)
            val parsed = sdf.parse(currentText)
            if (parsed != null) cal.time = parsed
        } catch (_: Exception) {
            cal.set(Calendar.HOUR_OF_DAY, if (isStartTime) 14 else 22)
            cal.set(Calendar.MINUTE, 0)
        }

        val hour = cal.get(Calendar.HOUR_OF_DAY)
        val minute = cal.get(Calendar.MINUTE)

        TimePickerDialog(this, { _, h, m ->
            val selected = Calendar.getInstance().apply {
                set(Calendar.HOUR_OF_DAY, h)
                set(Calendar.MINUTE, m)
            }
            val timeStr = SimpleDateFormat("h:mm a", Locale.US).format(selected.time)
            if (isStartTime) {
                binding.editStartTime.setText(timeStr)
            } else {
                binding.editEndTime.setText(timeStr)
            }
        }, hour, minute, false).show()
    }

    private fun executeApproval(note: String) {
        showLoading("Approving Booking...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.approveRental(rentalId, ApprovalActionRequest(note))
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "✅ Rental approved!", Toast.LENGTH_SHORT).show()
                        loadRentalDetail()
                    } else {
                        val errBody = response.errorBody()?.string()
                        val errMsg = try {
                            if (!errBody.isNullOrBlank()) {
                                val json = org.json.JSONObject(errBody)
                                json.optString("error").ifEmpty { json.optString("message", "Approval failed.") }
                            } else "Approval failed."
                        } catch (_: Exception) {
                            errBody ?: "Approval failed."
                        }
                        com.google.android.material.dialog.MaterialAlertDialogBuilder(this@RentalDetailActivity)
                            .setTitle("⚠️ Cannot Approve Rental")
                            .setMessage(errMsg)
                            .setPositiveButton("OK", null)
                            .show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptRevertToPending() {
        if (checkOfflineAndAlert()) return
        val applicantName = rentalDetail?.applicantName ?: "this booking"

        val input = EditText(this).apply {
            hint = "Optional reason note (e.g., approved by accident)..."
            setPadding(40, 24, 40, 24)
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setTextColor(getColor(R.color.text_primary))
        }

        val dialogContent = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(48, 24, 48, 16)
            addView(TextView(this@RentalDetailActivity).apply {
                text = "Are you sure you want to revert the approval for $applicantName?\n\nThis will return the booking status back to Pending, reset the approval record, and release the reservation on the calendar."
                setTextColor(getColor(R.color.text_secondary))
                textSize = 14f
                setPadding(0, 0, 0, 16)
            })
            addView(input)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle("↩️ Revert to Pending (Unapprove)")
            .setView(dialogContent)
            .setPositiveButton("Revert to Pending") { _, _ ->
                val reason = input.text.toString().trim().ifEmpty { "Reverted approval back to Pending" }
                executeRevertToPending(reason)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeRevertToPending(reason: String) {
        val current = rentalDetail ?: return
        showLoading("Reverting to Pending...")

        val payload = UpdateRentalPayload(
            applicantName = current.applicantName,
            requesterPhone = current.requesterPhone,
            requesterEmail = current.requesterEmail,
            requesterAddress = current.requesterAddress,
            eventDate = current.eventDate,
            eventType = current.eventType,
            startTime = current.startTime,
            endTime = current.endTime,
            roomSelected = current.roomSelected,
            guestCount = current.guestCount,
            totalPrice = current.totalPrice,
            securityDepositAmount = current.securityDepositAmount,
            amountPaid = current.amountPaid,
            isPaid = current.isPaid,
            bartenderRequested = current.bartenderRequested,
            kitchenUsage = current.kitchenUsage,
            avEquipmentUsage = current.avEquipmentUsage,
            status = "Pending",
            internalNotes = current.internalNotes,
            matrixSelected = current.renterType,
            sendUpdateEmail = false,
            changeReasonNote = reason
        )

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.updateRental(rentalId, payload)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "↩️ Booking reverted to Pending and calendar released.", Toast.LENGTH_LONG).show()
                        loadRentalDetail()
                    } else {
                        val errBody = response.errorBody()?.string()
                        val errMsg = try {
                            if (!errBody.isNullOrBlank()) {
                                val json = JSONObject(errBody)
                                json.optString("error").ifEmpty { json.optString("message", "Failed to revert booking.") }
                            } else "Failed to revert booking."
                        } catch (_: Exception) {
                            errBody ?: "Failed to revert booking."
                        }
                        MaterialAlertDialogBuilder(this@RentalDetailActivity)
                            .setTitle("⚠️ Cannot Revert Booking")
                            .setMessage(errMsg)
                            .setPositiveButton("OK", null)
                            .show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptCancel() {
        if (checkOfflineAndAlert()) return
        val input = EditText(this).apply {
            hint = "Optional cancellation reason..."
            setPadding(40, 24, 40, 24)
        }

        val applicantName = rentalDetail?.applicantName ?: "this applicant"

        MaterialAlertDialogBuilder(this)
            .setTitle("⚠️ Cancel Hall Booking")
            .setMessage("Are you sure you want to cancel the booking for $applicantName?\n\nThis will mark the request as Cancelled and release the reserved date on the club calendar.")
            .setView(input)
            .setPositiveButton("Cancel Booking") { _, _ ->
                val reason = input.text.toString().trim()
                executeCancel(reason)
            }
            .setNegativeButton("Keep Active", null)
            .show()
    }

    private fun executeCancel(reason: String) {
        showLoading("Cancelling Booking...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.cancelRental(rentalId, ApprovalActionRequest(reason))
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "Booking cancelled and calendar date released.", Toast.LENGTH_LONG).show()
                        loadRentalDetail()
                    } else {
                        Toast.makeText(this@RentalDetailActivity, "Failed to cancel booking.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptDelete() {
        if (checkOfflineAndAlert()) return
        val applicantName = rentalDetail?.applicantName ?: "this record"

        MaterialAlertDialogBuilder(this)
            .setTitle("🗑️ Permanently Delete Booking")
            .setMessage("⚠️ Are you sure you want to permanently delete the booking record for $applicantName?\n\nThis action CANNOT be undone and will permanently remove all booking records, payment history, and audit trails.")
            .setPositiveButton("Delete Permanently") { _, _ ->
                executeDelete()
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeDelete() {
        showLoading("Deleting Record...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.deleteRental(rentalId)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "Booking record permanently deleted.", Toast.LENGTH_LONG).show()
                        finish()
                    } else {
                        Toast.makeText(this@RentalDetailActivity, "Failed to delete record.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun getSubmissionAgeText(createdAtStr: String?): String {
        if (createdAtStr.isNullOrBlank()) return ""
        return try {
            val isoFormat = SimpleDateFormat("yyyy-MM-dd", Locale.US)
            val cleanDate = createdAtStr.substringBefore('T')
            val createdDate = isoFormat.parse(cleanDate) ?: return ""
            val diffMs = System.currentTimeMillis() - createdDate.time
            val days = (diffMs / (1000 * 60 * 60 * 24)).toInt()
            when {
                days > 1 -> "${days}d ago"
                days == 1 -> "Yesterday"
                days == 0 -> "Today"
                else -> ""
            }
        } catch (e: Exception) {
            ""
        }
    }

    private fun promptRecordPayment() {
        if (checkOfflineAndAlert()) return
        val item = rentalDetail ?: return
        val currentPaid = item.amountPaid
        val total = item.totalPrice
        val remaining = Math.max(0.0, total - currentPaid)
        val deposit = item.securityDepositAmount

        val dialogView = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(48, 24, 48, 16)
        }

        val summaryTv = TextView(this).apply {
            text = "Total: $${total.toInt()}  •  Paid: $${currentPaid.toInt()}  •  Balance Due: $${remaining.toInt()}"
            setTextColor(getColor(R.color.cyan_accent))
            textSize = 13f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 0, 0, 16)
        }
        dialogView.addView(summaryTv)

        val amountInput = EditText(this).apply {
            hint = "Payment Amount ($)"
            inputType = android.text.InputType.TYPE_CLASS_NUMBER or android.text.InputType.TYPE_NUMBER_FLAG_DECIMAL
            setText("")
            setPadding(20, 20, 20, 20)
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setTextColor(getColor(R.color.text_primary))
        }
        dialogView.addView(amountInput)

        // Presets buttons row
        val presetsLayout = LinearLayout(this).apply {
            orientation = LinearLayout.HORIZONTAL
            setPadding(0, 12, 0, 12)
        }

        if (remaining > 0) {
            val btnFull = com.google.android.material.button.MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
                text = "Full ($${remaining.toInt()})"
                textSize = 11f
                setOnClickListener { amountInput.setText(String.format(Locale.US, "%.2f", remaining)) }
            }
            presetsLayout.addView(btnFull)
        }

        if (item.requireSecurityDeposit && deposit > 0 && currentPaid < deposit) {
            val btnDep = com.google.android.material.button.MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
                text = "Deposit ($${deposit.toInt()})"
                textSize = 11f
                val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT).apply {
                    marginStart = 12
                }
                layoutParams = params
                setOnClickListener { amountInput.setText(String.format(Locale.US, "%.2f", deposit)) }
            }
            presetsLayout.addView(btnDep)
        }
        dialogView.addView(presetsLayout)

        // Payment Method Dropdown/Spinner
        val methodSpinner = android.widget.Spinner(this).apply {
            val methods = listOf("Cash", "Check", "Credit Card", "Venmo", "Online", "Waived / Complimentary", "Other")
            adapter = android.widget.ArrayAdapter(this@RentalDetailActivity, android.R.layout.simple_spinner_dropdown_item, methods)
            setBackgroundResource(R.drawable.bg_spinner_dark)
            setPadding(20, 20, 20, 20)
        }
        dialogView.addView(methodSpinner)

        val noteInput = EditText(this).apply {
            hint = "Optional reference / check # / note..."
            setPadding(20, 20, 20, 20)
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setTextColor(getColor(R.color.text_primary))
            val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT).apply {
                topMargin = 16
            }
            layoutParams = params
        }
        dialogView.addView(noteInput)

        val depositCheck = if (item.requireSecurityDeposit) {
            android.widget.CheckBox(this).apply {
                text = "Mark Security Deposit as Paid"
                isChecked = !item.securityDepositPaid
                setTextColor(getColor(R.color.text_primary))
                val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT).apply {
                    topMargin = 12
                }
                layoutParams = params
            }.also { dialogView.addView(it) }
        } else null

        MaterialAlertDialogBuilder(this)
            .setTitle("💵 Record Payment")
            .setView(dialogView)
            .setPositiveButton("Record Payment") { _, _ ->
                val amt = amountInput.text.toString().toDoubleOrNull() ?: 0.0
                if (amt <= 0.0) {
                    Toast.makeText(this, "Please enter a valid payment amount.", Toast.LENGTH_SHORT).show()
                    return@setPositiveButton
                }
                val method = methodSpinner.selectedItem?.toString() ?: "Cash"
                val note = noteInput.text.toString().trim().ifEmpty { null }
                val isDep = depositCheck?.isChecked ?: false

                if (method.contains("Waived", ignoreCase = true)) {
                    confirmWaivePayment(amt, note, isDep)
                } else {
                    executeRecordPayment(amt, method, note, isDep, isWaived = false)
                }
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun promptWaivePayment() {
        if (checkOfflineAndAlert()) return
        val item = rentalDetail ?: return
        val currentPaid = item.amountPaid
        val total = item.totalPrice
        val alreadyWaived = item.amountWaived ?: 0.0
        val remaining = Math.max(0.0, total - alreadyWaived - currentPaid)
        val deposit = item.securityDepositAmount

        val dialogView = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(48, 24, 48, 16)
        }

        val summaryTv = TextView(this).apply {
            val waiverNote = if (alreadyWaived > 0.0) "  •  Waived: $${alreadyWaived.toInt()}" else ""
            text = "Total Price: $${total.toInt()}$waiverNote  •  Paid: $${currentPaid.toInt()}  •  Remaining Balance: $${remaining.toInt()}"
            setTextColor(getColor(R.color.status_yellow))
            textSize = 13f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 0, 0, 16)
        }
        dialogView.addView(summaryTv)

        val lblAmt = TextView(this).apply {
            text = "Amount to Waive ($):"
            setTextColor(getColor(R.color.text_primary))
            textSize = 12f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 0, 0, 8)
        }
        dialogView.addView(lblAmt)

        val amountInput = EditText(this).apply {
            hint = "Waiver Amount ($)"
            inputType = android.text.InputType.TYPE_CLASS_NUMBER or android.text.InputType.TYPE_NUMBER_FLAG_DECIMAL
            setText(if (remaining > 0) String.format(Locale.US, "%.2f", remaining) else String.format(Locale.US, "%.2f", total))
            setPadding(20, 20, 20, 20)
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setTextColor(getColor(R.color.text_primary))
        }
        dialogView.addView(amountInput)

        // Presets row
        val presetsLayout = LinearLayout(this).apply {
            orientation = LinearLayout.HORIZONTAL
            setPadding(0, 12, 0, 12)
        }
        if (remaining > 0) {
            val btnFull = com.google.android.material.button.MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
                text = "Full Balance ($${remaining.toInt()})"
                textSize = 11f
                setOnClickListener { amountInput.setText(String.format(Locale.US, "%.2f", remaining)) }
            }
            presetsLayout.addView(btnFull)
        }
        dialogView.addView(presetsLayout)

        val lblReason = TextView(this).apply {
            text = "Waiver Reason (Required):"
            setTextColor(getColor(R.color.text_primary))
            textSize = 12f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 8, 0, 8)
        }
        dialogView.addView(lblReason)

        val reasonPresets = listOf(
            "-- Select reason preset (Optional) --",
            "Board of Directors / Officer Authorization",
            "Complimentary Member Booking",
            "Charity / Community Non-Profit Event",
            "Facility Maintenance / Schedule Compensation",
            "Dispute Resolution / Customer Goodwill",
            "Other / Custom Authorization"
        )
        val noteInput = EditText(this).apply {
            hint = "Type waiver reason & authorization notes..."
            setPadding(20, 20, 20, 20)
            minLines = 2
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setTextColor(getColor(R.color.text_primary))
            val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT).apply {
                topMargin = 12
            }
            layoutParams = params
        }

        val reasonSpinner = android.widget.Spinner(this).apply {
            adapter = android.widget.ArrayAdapter(this@RentalDetailActivity, android.R.layout.simple_spinner_dropdown_item, reasonPresets)
            setBackgroundResource(R.drawable.bg_spinner_dark)
            setPadding(20, 20, 20, 20)
            onItemSelectedListener = object : android.widget.AdapterView.OnItemSelectedListener {
                override fun onItemSelected(parent: android.widget.AdapterView<*>?, view: View?, position: Int, id: Long) {
                    if (position > 0 && !reasonPresets[position].contains("Other", ignoreCase = true)) {
                        noteInput.setText(reasonPresets[position])
                    }
                }
                override fun onNothingSelected(parent: android.widget.AdapterView<*>?) {}
            }
        }
        dialogView.addView(reasonSpinner)
        dialogView.addView(noteInput)

        val depositCheck = if (item.requireSecurityDeposit) {
            android.widget.CheckBox(this).apply {
                text = "Mark Security Deposit as Waived / Covered"
                isChecked = !item.securityDepositPaid
                setTextColor(getColor(R.color.text_primary))
                val params = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT).apply {
                    topMargin = 12
                }
                layoutParams = params
            }.also { dialogView.addView(it) }
        } else null

        MaterialAlertDialogBuilder(this)
            .setTitle("🎁 Waive Payment / Fees")
            .setView(dialogView)
            .setPositiveButton("Continue") { _, _ ->
                val amt = amountInput.text.toString().toDoubleOrNull() ?: 0.0
                if (amt <= 0.0) {
                    Toast.makeText(this, "Please enter a valid waiver amount.", Toast.LENGTH_SHORT).show()
                    return@setPositiveButton
                }
                val reason = noteInput.text.toString().trim()
                if (reason.isEmpty()) {
                    Toast.makeText(this, "A reason is required to waive payment.", Toast.LENGTH_SHORT).show()
                    return@setPositiveButton
                }
                val isDep = depositCheck?.isChecked ?: false
                confirmWaivePayment(amt, reason, isDep)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun confirmWaivePayment(amount: Double, reason: String?, markAsDeposit: Boolean) {
        val item = rentalDetail ?: return
        val applicant = item.applicantName
        val reasonText = if (!reason.isNullOrBlank()) reason else "Administrative waiver"

        val confirmationMessage = "Are you sure you want to WAIVE $${amount.toInt()} for $applicant?\n\n" +
                "• Amount: $${String.format(Locale.US, "%.2f", amount)}\n" +
                "• Reason: $reasonText\n" +
                (if (markAsDeposit) "• Security Deposit: Waived / Covered\n\n" else "\n") +
                "This action will record a fee waiver on this booking and update the payment status."

        MaterialAlertDialogBuilder(this)
            .setTitle("⚠️ Confirm Payment Waiver")
            .setMessage(confirmationMessage)
            .setPositiveButton("Yes, Waive Payment") { _, _ ->
                executeRecordPayment(amount, "Waived", reason, markAsDeposit, isWaived = true)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeRecordPayment(amount: Double, method: String, note: String?, markAsDeposit: Boolean, isWaived: Boolean = false) {
        showLoading(if (isWaived) "Applying Payment Waiver..." else "Recording Payment...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val payload = RecordPaymentPayload(amount, method, note, markAsDeposit, isWaived)
                val response = ApiClient.service.recordPayment(rentalId, payload)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        val msg = if (isWaived) "🎁 Payment waiver applied successfully!" else "💵 Payment recorded successfully!"
                        Toast.makeText(this@RentalDetailActivity, msg, Toast.LENGTH_LONG).show()
                        loadRentalDetail()
                    } else {
                        val errMsg = try {
                            val errJson = response.errorBody()?.string()
                            if (!errJson.isNullOrBlank()) {
                                val obj = org.json.JSONObject(errJson)
                                obj.optString("error", obj.optString("message", "Failed to process payment."))
                            } else {
                                response.body()?.message ?: "Failed to process payment."
                            }
                        } catch (e: Exception) {
                            response.body()?.message ?: "Failed to process payment."
                        }
                        Toast.makeText(this@RentalDetailActivity, "❌ $errMsg", Toast.LENGTH_LONG).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptSendPaymentReminder() {
        if (checkOfflineAndAlert()) return
        val item = rentalDetail ?: return
        val email = item.requesterEmail?.trim().orEmpty()
        if (email.isEmpty()) {
            Toast.makeText(this, "No email address found on this booking record.", Toast.LENGTH_LONG).show()
            return
        }

        val remaining = Math.max(0.0, item.totalPrice - item.amountPaid)
        val noteInput = EditText(this).apply {
            hint = "Optional custom note/instructions to include in email..."
            setPadding(40, 24, 40, 24)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle("✉️ Send Payment Reminder")
            .setMessage("Send customized balance reminder email to:\n$email\n\n• Outstanding Balance Due: $${remaining.toInt()}\n• Event Date: ${item.eventDate.substringBefore('T')}\n\nProceed with sending?")
            .setView(noteInput)
            .setPositiveButton("Send Reminder") { _, _ ->
                val customNote = noteInput.text.toString().trim().ifEmpty { null }
                executeSendPaymentReminder(customNote)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeSendPaymentReminder(customNote: String?) {
        showLoading("Sending Payment Reminder Email...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val payload = PaymentReminderPayload(customNote)
                val response = ApiClient.service.sendPaymentReminder(rentalId, payload)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "✉️ Payment reminder successfully sent!", Toast.LENGTH_LONG).show()
                        loadRentalDetail()
                    } else {
                        val errMsg = try {
                            val errJson = response.errorBody()?.string()
                            if (!errJson.isNullOrBlank()) {
                                val obj = org.json.JSONObject(errJson)
                                obj.optString("error", obj.optString("message", "Failed to send payment reminder."))
                            } else {
                                response.body()?.message ?: "Failed to send payment reminder."
                            }
                        } catch (e: Exception) {
                            response.body()?.message ?: "Failed to send payment reminder."
                        }
                        Toast.makeText(this@RentalDetailActivity, "❌ $errMsg", Toast.LENGTH_LONG).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptLogCallOutcome() {
        if (checkOfflineAndAlert()) return
        val outcomes = arrayOf("Spoke with Applicant", "Left Voicemail", "No Answer / Busy", "Follow-up Required")
        var selectedIndex = 0

        val dialogView = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(48, 24, 48, 16)
        }

        val noteInput = EditText(this).apply {
            hint = "Optional call summary / notes..."
            setTextColor(getColor(R.color.text_primary))
            setHintTextColor(getColor(R.color.text_muted))
            textSize = 14f
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setPadding(20, 20, 20, 20)
            val params = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
            ).apply {
                topMargin = 16
            }
            layoutParams = params
        }
        dialogView.addView(noteInput)

        MaterialAlertDialogBuilder(this)
            .setTitle("📞 Log Call Outcome")
            .setSingleChoiceItems(outcomes, selectedIndex) { _, which ->
                selectedIndex = which
            }
            .setView(dialogView)
            .setPositiveButton("Save Record") { _, _ ->
                val outcome = outcomes[selectedIndex]
                val note = noteInput.text.toString().trim().ifEmpty { null }
                executeLogCorrespondence("call", outcome, note)
            }
            .setNegativeButton("Skip", null)
            .show()
    }

    private fun promptLogCorrespondence(type: String? = null) {
        if (checkOfflineAndAlert()) return
        val types = arrayOf("Phone Call", "SMS / Text Message", "Email", "Internal Staff Note")
        val typeKeys = arrayOf("call", "sms", "email", "note")
        var selectedTypeIndex = if (type != null) typeKeys.indexOf(type).coerceAtLeast(0) else 0

        val dialogView = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(48, 24, 48, 16)
        }

        val outcomeInput = EditText(this).apply {
            hint = when (type) {
                "sms" -> "e.g., Sent inquiry response & availability details"
                "email" -> "e.g., Sent quote & pricing breakdown"
                else -> "Summary outcome / topic..."
            }
            setTextColor(getColor(R.color.text_primary))
            setHintTextColor(getColor(R.color.text_muted))
            textSize = 14f
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setPadding(20, 20, 20, 20)
        }
        dialogView.addView(outcomeInput)

        val noteInput = EditText(this).apply {
            hint = "Additional correspondence notes (optional)..."
            setTextColor(getColor(R.color.text_primary))
            setHintTextColor(getColor(R.color.text_muted))
            textSize = 14f
            minLines = 2
            setBackgroundResource(R.drawable.bg_edittext_dark)
            setPadding(20, 20, 20, 20)
            val params = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
            ).apply {
                topMargin = 16
            }
            layoutParams = params
        }
        dialogView.addView(noteInput)

        val title = when (type) {
            "sms" -> "💬 Log SMS / Text Message"
            "email" -> "✉️ Log Email Sent"
            else -> "📝 Log Correspondence & Reply"
        }

        val builder = MaterialAlertDialogBuilder(this)
            .setTitle(title)
            .setView(dialogView)
            .setPositiveButton("Save Record") { _, _ ->
                val chosenType = typeKeys[selectedTypeIndex]
                val outcome = outcomeInput.text.toString().trim().ifEmpty { 
                    when (chosenType) {
                        "sms" -> "SMS Message Sent"
                        "email" -> "Email Sent"
                        "call" -> "Phone Call"
                        else -> "Staff Note"
                    }
                }
                val note = noteInput.text.toString().trim().ifEmpty { null }
                executeLogCorrespondence(chosenType, outcome, note)
            }
            .setNegativeButton("Cancel", null)

        if (type == null) {
            builder.setSingleChoiceItems(types, selectedTypeIndex) { _, which ->
                selectedTypeIndex = which
            }
        }

        builder.show()
    }

    private fun executeLogCorrespondence(type: String, outcome: String?, notes: String?) {
        showLoading("Logging Correspondence...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val payload = LogCorrespondencePayload(type = type, outcome = outcome, notes = notes)
                val response = ApiClient.service.logCorrespondence(rentalId, payload)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "📝 Correspondence logged successfully!", Toast.LENGTH_SHORT).show()
                        loadRentalDetail()
                    } else {
                        Toast.makeText(this@RentalDetailActivity, "Failed to log correspondence.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptArchiveInquiry() {
        if (checkOfflineAndAlert()) return
        val isArchived = rentalDetail?.status.equals("Archived", ignoreCase = true)
        val title = if (isArchived) "📦 Restore Inquiry" else "📦 Archive Inquiry to FAQ Pool"
        val message = if (isArchived) {
            "Restore this inquiry back to active inquiries list?"
        } else {
            "Archive this inquiry into the FAQ Review & Reference Pool?\n\nThis marks the inquiry as answered/archived and moves it out of active inquiries."
        }
        val buttonText = if (isArchived) "Restore" else "Archive"

        MaterialAlertDialogBuilder(this)
            .setTitle(title)
            .setMessage(message)
            .setPositiveButton(buttonText) { _, _ ->
                executeArchiveInquiry(!isArchived)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeArchiveInquiry(archive: Boolean) {
        showLoading(if (archive) "Archiving Inquiry..." else "Restoring Inquiry...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val payload = ArchiveRentalPayload(archive = archive)
                val response = ApiClient.service.archiveRental(rentalId, payload)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        val msg = if (archive) "Inquiry archived to FAQ pool." else "Inquiry restored to active list."
                        Toast.makeText(this@RentalDetailActivity, msg, Toast.LENGTH_SHORT).show()
                        loadRentalDetail()
                    } else {
                        Toast.makeText(this@RentalDetailActivity, "Failed to update inquiry archive status.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    Toast.makeText(this@RentalDetailActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun fetchUnavailableDates(onComplete: (() -> Unit)? = null) {
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val resp = ApiClient.service.getUnavailableDates()
                if (resp.isSuccessful && resp.body() != null) {
                    cachedUnavailableDates = resp.body()!!
                    withContext(Dispatchers.Main) {
                        checkScheduleConflicts()
                        onComplete?.invoke()
                    }
                }
            } catch (_: Exception) {}
        }
    }

    enum class DaySlotStatus {
        ORIGINAL_BOOKING,
        AVAILABLE,
        BOOKED
    }

    data class DaySlotEvaluation(
        val name: String,
        val startTime: String,
        val endTime: String,
        val status: DaySlotStatus,
        val bookedReason: String? = null
    )

    private fun checkScheduleConflicts() {
        if (!isEditMode || isInquiryMode) {
            binding.cardConflictWarning.visibility = View.GONE
            binding.layoutQuickSelectSlots.visibility = View.GONE
            hasActiveConflict = false
            return
        }

        val targetDateStr = SimpleDateFormat("yyyy-MM-dd", Locale.US).format(selectedEventCalendar.time)
        val targetStart = binding.editStartTime.text.toString().trim()
        val targetEnd = binding.editEndTime.text.toString().trim()
        val targetRoom = binding.editRoomSelected.text.toString().trim().ifEmpty { "Function Hall" }

        val isSecondaryRoom = targetRoom.contains("Office", ignoreCase = true) ||
                targetRoom.contains("Board", ignoreCase = true) ||
                targetRoom.contains("Lounge", ignoreCase = true)

        val cur = rentalDetail
        val origDate = originalDateStr ?: cur?.eventDate?.substringBefore('T')
        val origTime = if (!originalStartTime.isNullOrBlank() && !originalEndTime.isNullOrBlank()) "$originalStartTime - $originalEndTime" else null
        val origApplicant = cur?.applicantName?.lowercase(Locale.US) ?: ""

        // Filter out self and get all other events on the same day for this room type
        val otherEventsOnDate = cachedUnavailableDates.filter { ev ->
            val evDate = ev.date.substringBefore('T')
            if (evDate != targetDateStr) return@filter false
            
            // 1. Check ID
            if (ev.id != 0 && ev.id == rentalId) return@filter false

            // 2. Check if this is the current booking itself represented in external feeds/calendars
            val desc = ev.eventType ?: ""
            if (origDate == targetDateStr && origApplicant.isNotBlank() && desc.lowercase(Locale.US).contains(origApplicant)) {
                return@filter false
            }
            if (origDate == targetDateStr && origTime != null && ev.eventTime == origTime && (desc.contains("Rental", ignoreCase = true) || desc.contains(cur?.eventType ?: "", ignoreCase = true))) {
                return@filter false
            }

            // 3. Room matching
            val evIsSecondary = desc.contains("Office", ignoreCase = true) ||
                    desc.contains("Board", ignoreCase = true) ||
                    desc.contains("Lounge", ignoreCase = true)

            if (isSecondaryRoom && !evIsSecondary) return@filter false
            if (!isSecondaryRoom && evIsSecondary) return@filter false

            true
        }

        // Calculate all slot states (Original Booking, Available, and Booked)
        val allDaySlots = computeAllDaySlotEvaluations(otherEventsOnDate)
        renderQuickSlotChips(allDaySlots)

        // Evaluate if the currently entered Start/End time has a collision
        if (targetStart.isBlank() || targetEnd.isBlank()) {
            hasActiveConflict = false
            binding.cardConflictWarning.visibility = View.GONE
            return
        }

        val startMin = parseTimeToMinutes(targetStart)
        val endMin = parseTimeToMinutes(targetEnd)

        val conflicts = otherEventsOnDate.filter { ev ->
            val evTime = ev.eventTime
            if (evTime.isNullOrBlank()) return@filter true

            val parts = evTime.split("-", "to", "–")
            if (parts.size == 2) {
                val evSt = parseTimeToMinutes(parts[0].trim())
                val evEt = parseTimeToMinutes(parts[1].trim())
                return@filter (startMin < evEt && endMin > evSt)
            }
            true
        }

        if (conflicts.isNotEmpty()) {
            hasActiveConflict = true
            binding.cardConflictWarning.setCardBackgroundColor(android.graphics.Color.parseColor("#2A1515"))
            binding.cardConflictWarning.strokeColor = getColor(R.color.status_red)
            binding.txtConflictTitle.text = "⚠️ Schedule Conflict"
            binding.txtConflictTitle.setTextColor(getColor(R.color.status_red))
            binding.txtConflictBadge.text = "BOOKED"
            binding.txtConflictBadge.setTextColor(getColor(R.color.status_red))
            binding.txtConflictDetails.setTextColor(android.graphics.Color.parseColor("#FFCCCC"))

            val sb = StringBuilder()
            conflicts.forEach { c ->
                val desc = c.eventType ?: "Booked Event / Rental"
                val timeInfo = if (!c.eventTime.isNullOrBlank()) " (${c.eventTime})" else " (Full Day)"
                sb.append("• $desc$timeInfo in $targetRoom is already scheduled.\n")
            }
            binding.txtConflictDetails.text = sb.toString().trim()
            binding.cardConflictWarning.visibility = View.VISIBLE
        } else {
            hasActiveConflict = false
            binding.cardConflictWarning.visibility = View.GONE
        }
    }

    private fun renderQuickSlotChips(evaluatedSlots: List<DaySlotEvaluation>) {
        binding.chipGroupQuickSlots.removeAllViews()
        if (!isEditMode || isInquiryMode || evaluatedSlots.isEmpty()) {
            binding.layoutQuickSelectSlots.visibility = View.GONE
            return
        }

        val targetDateStr = SimpleDateFormat("yyyy-MM-dd", Locale.US).format(selectedEventCalendar.time)
        val isOriginalDate = (originalDateStr != null && targetDateStr == originalDateStr)
        val dayName = SimpleDateFormat("EEEE", Locale.US).format(selectedEventCalendar.time)
        val displayFormat = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)

        binding.lblQuickSlotsTitle.text = "🕒 Daily Schedule ($dayName Slots - Tap to select):"

        if (!isOriginalDate && originalDateStr != null) {
            val origDateDisplay = displayFormat.format(originalEventCalendar.time)
            binding.txtOriginalBookingReminder.text = "📌 Original Booking was: $origDateDisplay (${originalStartTime ?: "N/A"} – ${originalEndTime ?: "N/A"})"
            binding.txtOriginalBookingReminder.visibility = View.VISIBLE
        } else {
            binding.txtOriginalBookingReminder.visibility = View.GONE
        }

        binding.layoutQuickSelectSlots.visibility = View.VISIBLE

        for (slot in evaluatedSlots) {
            val chip = Chip(this).apply {
                isCheckable = false
                textSize = 12f
                typeface = Typeface.DEFAULT_BOLD

                when (slot.status) {
                    DaySlotStatus.ORIGINAL_BOOKING -> {
                        text = "📌 ${slot.name}: ${slot.startTime} – ${slot.endTime} (Original Booking)"
                        chipBackgroundColor = ColorStateList.valueOf(android.graphics.Color.parseColor("#142938"))
                        chipStrokeColor = ColorStateList.valueOf(getColor(R.color.cyan_accent))
                        chipStrokeWidth = 2f
                        setTextColor(getColor(R.color.cyan_accent))
                        isClickable = true
                        setOnClickListener {
                            binding.editStartTime.setText(slot.startTime)
                            binding.editEndTime.setText(slot.endTime)
                            checkScheduleConflicts()
                            Toast.makeText(this@RentalDetailActivity, "Restored original slot: ${slot.startTime} – ${slot.endTime}", Toast.LENGTH_SHORT).show()
                        }
                    }
                    DaySlotStatus.AVAILABLE -> {
                        text = "🟢 ${slot.name}: ${slot.startTime} – ${slot.endTime} (Available)"
                        chipBackgroundColor = ColorStateList.valueOf(android.graphics.Color.parseColor("#153326"))
                        chipStrokeColor = ColorStateList.valueOf(getColor(R.color.emerald_accent))
                        chipStrokeWidth = 2f
                        setTextColor(getColor(R.color.emerald_accent))
                        isClickable = true
                        setOnClickListener {
                            binding.editStartTime.setText(slot.startTime)
                            binding.editEndTime.setText(slot.endTime)
                            checkScheduleConflicts()
                            Toast.makeText(this@RentalDetailActivity, "Selected: ${slot.startTime} – ${slot.endTime}", Toast.LENGTH_SHORT).show()
                        }
                    }
                    DaySlotStatus.BOOKED -> {
                        val reason = slot.bookedReason ?: "Existing Booking"
                        text = "🔴 ${slot.name}: ${slot.startTime} – ${slot.endTime} (Booked: $reason)"
                        chipBackgroundColor = ColorStateList.valueOf(android.graphics.Color.parseColor("#2A1515"))
                        chipStrokeColor = ColorStateList.valueOf(getColor(R.color.status_red))
                        chipStrokeWidth = 1.5f
                        setTextColor(android.graphics.Color.parseColor("#FFAAAA"))
                        isClickable = true
                        setOnClickListener {
                            Toast.makeText(this@RentalDetailActivity, "⛔ Unavailable: Slot is booked for '$reason'.", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            }
            binding.chipGroupQuickSlots.addView(chip)
        }
    }

    private fun computeAllDaySlotEvaluations(bookedEvents: List<UnavailableDateDto>): List<DaySlotEvaluation> {
        val targetDateStr = SimpleDateFormat("yyyy-MM-dd", Locale.US).format(selectedEventCalendar.time)
        val isOriginalDate = (originalDateStr != null && targetDateStr == originalDateStr)
        val origSt = originalStartTime
        val origEt = originalEndTime

        // 1. Get Day-of-Week (0 = Sunday, 1 = Monday ... 6 = Saturday)
        val dayOfWeek = selectedEventCalendar.get(Calendar.DAY_OF_WEEK) - 1
        val configuredDay = rentalDetail?.availableDaySchedules?.firstOrNull { it.dayOfWeek == dayOfWeek }

        val candidateSlots = mutableListOf<Triple<String, String, String>>()
        if (configuredDay != null && !configuredDay.slots.isNullOrEmpty()) {
            for (s in configuredDay.slots.filter { it.isActive }) {
                candidateSlots.add(Triple(s.name ?: "Block", s.startTime.trim(), s.endTime.trim()))
            }
        }

        if (candidateSlots.isEmpty()) {
            candidateSlots.add(Triple("Morning Block", "9:00 AM", "1:00 PM"))
            candidateSlots.add(Triple("Afternoon Block", "12:00 PM", "5:00 PM"))
            candidateSlots.add(Triple("Evening Block", "6:00 PM", "11:00 PM"))
        }

        val bookedRanges = bookedEvents.mapNotNull { ev ->
            val evTime = ev.eventTime
            if (evTime.isNullOrBlank() || (!evTime.contains("-") && !evTime.contains("to") && !evTime.contains("–"))) {
                Pair(Pair(0, 24 * 60), ev.eventType ?: "Booked Event")
            } else {
                val parts = evTime.split("-", "to", "–")
                if (parts.size == 2) {
                    Pair(Pair(parseTimeToMinutes(parts[0].trim()), parseTimeToMinutes(parts[1].trim())), ev.eventType ?: "Booked Event")
                } else null
            }
        }

        val results = mutableListOf<DaySlotEvaluation>()
        for ((name, start, end) in candidateSlots) {
            val candStart = parseTimeToMinutes(start)
            val candEnd = parseTimeToMinutes(end)

            // Check if this slot matches the original booking
            val isOriginalSlot = isOriginalDate && (
                start.equals(origSt, ignoreCase = true) ||
                (origSt != null && origEt != null &&
                 candStart == parseTimeToMinutes(origSt) &&
                 candEnd == parseTimeToMinutes(origEt))
            )

            if (isOriginalSlot) {
                results.add(DaySlotEvaluation(name, start, end, DaySlotStatus.ORIGINAL_BOOKING))
            } else {
                val collision = bookedRanges.firstOrNull { b -> candStart < b.first.second && candEnd > b.first.first }
                if (collision != null) {
                    results.add(DaySlotEvaluation(name, start, end, DaySlotStatus.BOOKED, collision.second))
                } else {
                    results.add(DaySlotEvaluation(name, start, end, DaySlotStatus.AVAILABLE))
                }
            }
        }
        return results
    }

    private fun formatMinutesToTime(minutes: Int): String {
        val h = (minutes / 60) % 24
        val m = minutes % 60
        val isPm = h >= 12
        val displayH = if (h == 0) 12 else if (h > 12) h - 12 else h
        return if (m == 0) {
            String.format(Locale.US, "%d:00 %s", displayH, if (isPm) "PM" else "AM")
        } else {
            String.format(Locale.US, "%d:%02d %s", displayH, m, if (isPm) "PM" else "AM")
        }
    }

    private fun parseTimeToMinutes(timeStr: String): Int {
        try {
            val clean = timeStr.trim().uppercase(Locale.US)
            val isPm = clean.contains("PM")
            val isAm = clean.contains("AM")
            val digits = clean.replace("AM", "").replace("PM", "").trim()
            val parts = digits.split(":")
            if (parts.isNotEmpty()) {
                var hours = parts[0].toInt()
                val minutes = if (parts.size > 1) parts[1].toInt() else 0
                if (isPm && hours < 12) hours += 12
                if (isAm && hours == 12) hours = 0
                return hours * 60 + minutes
            }
        } catch (_: Exception) {}
        return 0
    }
}
