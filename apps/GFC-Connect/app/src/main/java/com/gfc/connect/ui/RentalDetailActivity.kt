package com.gfc.connect.ui

import android.app.DatePickerDialog
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
import com.gfc.connect.data.models.AvailableMatrixTierDto
import com.gfc.connect.data.models.HallRentalDetailDto
import com.gfc.connect.data.models.UpdateRentalPayload
import com.gfc.connect.databinding.ActivityRentalDetailBinding
import com.google.android.material.button.MaterialButton
import com.google.android.material.card.MaterialCardView
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.text.NumberFormat
import java.text.SimpleDateFormat
import java.util.*

class RentalDetailActivity : AppCompatActivity() {

    companion object {
        const val EXTRA_RENTAL_ID = "extra_rental_id"
    }

    private lateinit var binding: ActivityRentalDetailBinding
    private lateinit var cacheManager: RentalCacheManager

    private var rentalId: Int = 0
    private var rentalDetail: HallRentalDetailDto? = null
    private var isEditMode: Boolean = false
    private var selectedMatrixTierTitle: String? = null
    private var selectedEventCalendar: Calendar = Calendar.getInstance()

    private val statusOptions = listOf("Pending", "Approved", "Denied", "Completed", "Cancelled")

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRentalDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        cacheManager = RentalCacheManager(this)

        rentalId = intent.getIntExtra(EXTRA_RENTAL_ID, 0)
        if (rentalId == 0) {
            Toast.makeText(this, "Invalid rental ID", Toast.LENGTH_SHORT).show()
            finish()
            return
        }

        setupToolbar()
        setupTabs()
        setupStatusSpinner()
        setupListeners()

        // 1. Instant Cache Load
        val cached = cacheManager.getRentalDetail(rentalId)
        if (cached != null) {
            rentalDetail = cached
            populateUI(cached)
        }

        // 2. Fresh Network Sync
        loadRentalDetail(silent = cached != null)
    }

    private fun setupToolbar() {
        binding.toolbarDetail.setNavigationOnClickListener {
            finish()
        }

        binding.btnToggleEdit.setOnClickListener {
            toggleEditMode(!isEditMode)
        }
    }

    private fun setupTabs() {
        val tabLayout = binding.tabLayoutSections
        tabLayout.removeAllTabs()
        tabLayout.addTab(tabLayout.newTab().setText("📅 Event"))
        tabLayout.addTab(tabLayout.newTab().setText("👤 Renter"))
        tabLayout.addTab(tabLayout.newTab().setText("💵 Pricing"))
        tabLayout.addTab(tabLayout.newTab().setText("🛡️ Admin"))

        tabLayout.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                showSection(tab?.position ?: 0)
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })
    }

    private fun showSection(position: Int) {
        binding.sectionEvent.visibility = if (position == 0) View.VISIBLE else View.GONE
        binding.sectionRenter.visibility = if (position == 1) View.VISIBLE else View.GONE
        binding.sectionFinancials.visibility = if (position == 2) View.VISIBLE else View.GONE
        binding.sectionAdmin.visibility = if (position == 3) View.VISIBLE else View.GONE
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
                selectedEventCalendar.set(year, month, dayOfMonth)
                val sdf = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)
                binding.txtEventDate.text = sdf.format(selectedEventCalendar.time)
            }, y, m, d).show()
        }

        // Phone call
        binding.btnCallRenter.setOnClickListener {
            val phone = binding.editPhone.text.toString().trim()
            if (phone.isNotEmpty()) {
                val intent = Intent(Intent.ACTION_DIAL, Uri.parse("tel:$phone"))
                startActivity(intent)
            } else {
                Toast.makeText(this, "No phone number available.", Toast.LENGTH_SHORT).show()
            }
        }

        // SMS
        binding.btnSmsRenter.setOnClickListener {
            val phone = binding.editPhone.text.toString().trim()
            if (phone.isNotEmpty()) {
                val intent = Intent(Intent.ACTION_SENDTO, Uri.parse("smsto:$phone"))
                startActivity(intent)
            } else {
                Toast.makeText(this, "No phone number available.", Toast.LENGTH_SHORT).show()
            }
        }

        // Email
        binding.btnEmailRenter.setOnClickListener {
            val email = binding.editEmail.text.toString().trim()
            if (email.isNotEmpty()) {
                val intent = Intent(Intent.ACTION_SENDTO, Uri.parse("mailto:$email"))
                intent.putExtra(Intent.EXTRA_SUBJECT, "Good Fellowship Club - Hall Rental Inquiry")
                startActivity(intent)
            } else {
                Toast.makeText(this, "No email address available.", Toast.LENGTH_SHORT).show()
            }
        }

        // Swipe to Refresh
        binding.swipeRefreshDetail.setColorSchemeResources(R.color.cyan_accent, R.color.blue_primary)
        binding.swipeRefreshDetail.setProgressBackgroundColorSchemeResource(R.color.surface_dark)
        binding.swipeRefreshDetail.setOnRefreshListener {
            loadRentalDetail(silent = true)
        }

        // Approve / Deny from detail
        binding.btnApproveDetail.setOnClickListener { promptApprove() }
        binding.btnDenyDetail.setOnClickListener { promptDeny() }

        // Save & Cancel
        binding.btnSaveDetail.setOnClickListener { promptSaveChanges() }
        binding.btnCancelEdit.setOnClickListener {
            toggleEditMode(false)
            rentalDetail?.let { populateUI(it) }
        }
    }

    private fun loadRentalDetail(silent: Boolean = false) {
        if (!silent) {
            showLoading("Syncing Booking Details...")
        }

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.getRentalDetail(rentalId)
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body() != null) {
                        rentalDetail = response.body()
                        rentalDetail?.let {
                            cacheManager.saveRentalDetail(it)
                            populateUI(it)
                        }
                    } else if (rentalDetail == null) {
                        Toast.makeText(this@RentalDetailActivity, "Failed to load booking details.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (rentalDetail == null) {
                        Toast.makeText(this@RentalDetailActivity, "Offline: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }

    private fun populateUI(item: HallRentalDetailDto) {
        // Header
        binding.txtHeaderEventType.text = item.eventType ?: "Hall Rental"
        binding.txtHeaderStatus.text = item.status
        updateStatusBadgeColor(item.status)

        // Parse Date
        val isoFormat = SimpleDateFormat("yyyy-MM-dd", Locale.US)
        val displayFormat = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)
        try {
            val dateStr = item.eventDate.substringBefore('T')
            val parsed = isoFormat.parse(dateStr)
            if (parsed != null) {
                selectedEventCalendar.time = parsed
                binding.txtEventDate.text = displayFormat.format(parsed)
                binding.txtHeaderSubtitle.text = "${item.applicantName} • ${displayFormat.format(parsed)}"
            } else {
                binding.txtEventDate.text = item.eventDate
                binding.txtHeaderSubtitle.text = "${item.applicantName} • ${item.eventDate}"
            }
        } catch (e: Exception) {
            binding.txtEventDate.text = item.eventDate
            binding.txtHeaderSubtitle.text = "${item.applicantName} • ${item.eventDate}"
        }

        // Event Fields
        binding.editEventType.setText(item.eventType ?: "Hall Rental")
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
        renderMatrixTiers(item)

        // Member Verification Card
        if (item.isVerifiedMember) {
            val idText = if (item.verifiedMemberId != null && item.verifiedMemberId > 0) " #${item.verifiedMemberId}" else ""
            binding.txtMemberVerifyBadgeDetail.text = "VERIFIED MEMBER$idText"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.emerald_accent))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_badge_emerald)
            binding.txtMemberVerifyDetails.text = item.memberVerificationText ?: "Active member in good standing."
        } else if (item.memberVerificationBadge == "UNVERIFIED_CLAIM") {
            binding.txtMemberVerifyBadgeDetail.text = "CLAIMED (NOT IN DIRECTORY)"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.status_yellow))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
            binding.txtMemberVerifyDetails.text = item.memberVerificationText ?: "Applicant selected Member pricing, but no matching record was found in the directory."
        } else {
            binding.txtMemberVerifyBadgeDetail.text = "NON-MEMBER"
            binding.txtMemberVerifyBadgeDetail.setTextColor(getColor(R.color.text_muted))
            binding.txtMemberVerifyBadgeDetail.setBackgroundResource(R.drawable.bg_pill_sync)
            binding.txtMemberVerifyDetails.text = "Applicant is booking under standard Non-Member pricing."
        }

        // Render Possible Candidate Matches
        renderPossibleCandidates(item)

        // Renter Fields
        binding.editApplicantName.setText(item.applicantName)
        binding.editPhone.setText(item.requesterPhone ?: "")
        binding.editEmail.setText(item.requesterEmail ?: "")
        
        val street = item.requesterAddress?.trim().orEmpty()
        val city = item.requesterCity?.trim().orEmpty()
        val state = item.requesterState?.trim().orEmpty()
        val zip = item.requesterZip?.trim().orEmpty()

        val fullAddress = if (street.isNotEmpty()) {
            val containsCity = city.isNotEmpty() && street.contains(city, ignoreCase = true)
            val containsState = state.isNotEmpty() && street.contains(state, ignoreCase = true)
            val containsZip = zip.isNotEmpty() && street.contains(zip, ignoreCase = true)

            if (containsCity || containsState || containsZip) {
                street
            } else {
                listOfNotNull(
                    street.ifEmpty { null },
                    city.ifEmpty { null },
                    state.ifEmpty { null },
                    zip.ifEmpty { null }
                ).joinToString(", ")
            }
        } else {
            listOfNotNull(
                city.ifEmpty { null },
                state.ifEmpty { null },
                zip.ifEmpty { null }
            ).joinToString(", ")
        }
        binding.editAddress.setText(if (fullAddress.isNotEmpty()) fullAddress else "")

        // Financials
        val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)
        binding.editTotalPrice.setText(item.totalPrice.toString())
        binding.editSecurityDeposit.setText(item.securityDepositAmount.toString())
        binding.editAmountPaid.setText(item.amountPaid.toString())
        binding.switchIsPaid.isChecked = item.isPaid || (item.amountPaid >= item.totalPrice && item.totalPrice > 0)

        binding.switchBar.isChecked = item.bartenderRequested
        binding.switchKitchen.isChecked = item.kitchenUsage
        binding.switchAv.isChecked = item.avEquipmentUsage

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
            if (isEmpty()) append("Pending administrative review.")
        }
        binding.txtDecisionInfo.text = decisionInfo.trimEnd()
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
                binding.layoutCandidatesContainer.visibility = View.GONE
                
                toggleEditMode(true)
                Toast.makeText(this, "Linked to ${candidate.fullName}. Tap 'Save Changes' to commit to server.", Toast.LENGTH_LONG).show()
            }
            .setNegativeButton("Cancel", null)
            .show()
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
                    } else {
                        text = "Select"
                        setTextColor(getColor(R.color.cyan_accent))
                        strokeColor = ColorStateList.valueOf(getColor(R.color.border_dark))
                    }
                }

                val onTierClick = View.OnClickListener {
                    applySelectedMatrixTier(tier)
                }
                selectBtn.setOnClickListener(onTierClick)
                setOnClickListener(onTierClick)

                rowLayout.addView(selectBtn)
                addView(rowLayout)
            }
            container.addView(card)
        }
    }

    private fun applySelectedMatrixTier(tier: AvailableMatrixTierDto) {
        selectedMatrixTierTitle = tier.title
        binding.editMatrixSelected.setText(tier.title)
        binding.txtCurrentMatrixBadge.text = tier.title

        // Recalculate price: base rate + bartender + kitchen + av
        var total = tier.rateForDate
        if (binding.switchBar.isChecked) total += 100.0
        if (binding.switchKitchen.isChecked) total += 50.0
        if (binding.switchAv.isChecked) total += 25.0
        binding.editTotalPrice.setText(String.format(Locale.US, "%.2f", total))

        if (!isEditMode) {
            toggleEditMode(true)
        }
        rentalDetail?.let { renderMatrixTiers(it) }
        Toast.makeText(this, "Updated to ${tier.title} ($${tier.rateForDate.toInt()} base). Tap 'Save Changes' to apply.", Toast.LENGTH_LONG).show()
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

    private fun toggleEditMode(enable: Boolean) {
        isEditMode = enable
        binding.btnToggleEdit.text = if (enable) "Done" else "Edit"
        binding.cardSaveBar.visibility = if (enable) View.VISIBLE else View.GONE
        binding.btnPickDate.visibility = if (enable) View.VISIBLE else View.GONE

        // Enable / Disable inputs
        binding.editEventType.isEnabled = enable
        binding.editStartTime.isEnabled = enable
        binding.editEndTime.isEnabled = enable
        binding.editRoomSelected.isEnabled = enable
        binding.editGuestCount.isEnabled = enable
        binding.editMatrixSelected.isEnabled = enable

        binding.editApplicantName.isEnabled = enable
        binding.editPhone.isEnabled = enable
        binding.editEmail.isEnabled = enable
        binding.editAddress.isEnabled = enable

        binding.editTotalPrice.isEnabled = enable
        binding.editSecurityDeposit.isEnabled = enable
        binding.editAmountPaid.isEnabled = enable
        binding.switchIsPaid.isEnabled = enable
        binding.switchBar.isEnabled = enable
        binding.switchKitchen.isEnabled = enable
        binding.switchAv.isEnabled = enable

        binding.spinnerStatus.isEnabled = enable
        binding.editInternalNotes.isEnabled = enable

        if (enable) {
            binding.editEventType.requestFocus()
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
        val newBar = binding.switchBar.isChecked
        val newKitchen = binding.switchKitchen.isChecked
        val newAv = binding.switchAv.isChecked
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
            }

            if (newGuests != null && cur.guestCount != newGuests) {
                diffs.add("👥 Guest Count: ${cur.guestCount ?: 0} ➔ $newGuests")
            }

            if (newRoom != null && !cur.roomSelected.equals(newRoom, ignoreCase = true)) {
                diffs.add("🚪 Room: ${cur.roomSelected ?: "Function Hall"} ➔ $newRoom")
            }

            if (cur.bartenderRequested != newBar) {
                diffs.add("🍸 Bartender Service: ${if (newBar) "Added" else "Removed"}")
            }
            if (cur.kitchenUsage != newKitchen) {
                diffs.add("🍳 Kitchen Access: ${if (newKitchen) "Added" else "Removed"}")
            }
            if (cur.avEquipmentUsage != newAv) {
                diffs.add("🔊 A/V Equipment: ${if (newAv) "Added" else "Removed"}")
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

        MaterialAlertDialogBuilder(this)
            .setTitle("📝 Review & Confirm Changes")
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
                        Toast.makeText(this@RentalDetailActivity, "Failed to save changes.", Toast.LENGTH_SHORT).show()
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

    private fun promptApprove() {
        val input = EditText(this).apply {
            hint = "Optional approval note..."
            setPadding(40, 24, 40, 24)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle("✅ Approve Rental Request")
            .setMessage("Confirm approval for this hall rental?")
            .setView(input)
            .setPositiveButton("Approve") { _, _ ->
                val note = input.text.toString().trim()
                executeApproval(note)
            }
            .setNegativeButton("Cancel", null)
            .show()
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
                        Toast.makeText(this@RentalDetailActivity, "Approval failed.", Toast.LENGTH_SHORT).show()
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

    private fun promptDeny() {
        val input = EditText(this).apply {
            hint = "Reason for denial..."
            setPadding(40, 24, 40, 24)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle("⛔ Deny Rental Request")
            .setMessage("Are you sure you want to deny this hall rental?")
            .setView(input)
            .setPositiveButton("Deny") { _, _ ->
                val note = input.text.toString().trim()
                executeDeny(note)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeDeny(note: String) {
        showLoading("Denying Booking...")
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.denyRental(rentalId, ApprovalActionRequest(note))
                withContext(Dispatchers.Main) {
                    hideLoading()
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalDetailActivity, "Rental denied.", Toast.LENGTH_SHORT).show()
                        loadRentalDetail()
                    } else {
                        Toast.makeText(this@RentalDetailActivity, "Denial failed.", Toast.LENGTH_SHORT).show()
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
}
