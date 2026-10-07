package com.gfc.connect.ui

import android.content.Intent
import android.os.Bundle
import android.graphics.Typeface
import android.view.Gravity
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.cache.RentalCacheManager
import com.gfc.connect.data.models.HallRentalDto
import com.gfc.connect.data.models.UnavailableDateDto
import android.app.TimePickerDialog
import android.widget.EditText
import android.widget.RadioButton
import android.widget.RadioGroup
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import com.gfc.connect.data.models.CreateClubEventPayload
import com.gfc.connect.data.models.UpdateClubEventPayload
import com.gfc.connect.databinding.ActivityRentalCalendarBinding
import com.google.android.material.button.MaterialButton
import com.google.android.material.card.MaterialCardView
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.text.NumberFormat
import java.text.SimpleDateFormat
import java.util.*

class RentalCalendarActivity : AppCompatActivity() {

    private lateinit var binding: ActivityRentalCalendarBinding
    private lateinit var cacheManager: RentalCacheManager
    private lateinit var networkMonitor: com.gfc.connect.api.NetworkMonitor
    private var isOfflineMode: Boolean = false

    private val rentalsList = mutableListOf<HallRentalDto>()
    private val unavailableDatesList = mutableListOf<UnavailableDateDto>()
    private var displayMonthCalendar: Calendar = Calendar.getInstance()
    private var selectedDateString: String = ""

    private data class CalendarDayModel(
        val dayNumber: Int,
        val dateString: String,
        val isCurrentMonth: Boolean,
        val hasBooked: Boolean,
        val hasPending: Boolean,
        val hasClubEvent: Boolean,
        val hasBarService: Boolean = false
    )

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRentalCalendarBinding.inflate(layoutInflater)
        setContentView(binding.root)

        cacheManager = RentalCacheManager(this)
        networkMonitor = com.gfc.connect.api.NetworkMonitor(this)

        setupToolbar()
        setupSwipeRefresh()
        setupCalendar()
        setupFab()
        loadData()
        observeNetwork()
    }

    private fun observeNetwork() {
        lifecycleScope.launch {
            networkMonitor.observeNetworkState().collect { online ->
                if (online && isOfflineMode) {
                    loadData()
                }
            }
        }
    }

    private fun setupSwipeRefresh() {
        binding.swipeRefreshCalendar.setColorSchemeResources(R.color.cyan_accent, R.color.blue_primary, R.color.purple_accent)
        binding.swipeRefreshCalendar.setProgressBackgroundColorSchemeResource(R.color.surface_dark)
        binding.swipeRefreshCalendar.setOnRefreshListener {
            loadData()
        }
    }

    override fun onResume() {
        super.onResume()
        loadData()
    }

    private fun setupFab() {
        binding.fabAddClubEvent.setOnClickListener {
            promptCreateClubEvent()
        }
    }

    private fun setupToolbar() {
        binding.toolbarCalendar.setNavigationOnClickListener {
            finish()
        }
    }

    private fun setupCalendar() {
        val today = Calendar.getInstance()
        val isoFormat = SimpleDateFormat("yyyy-MM-dd", Locale.US)
        selectedDateString = isoFormat.format(today.time)
        updateDateHeader(today.time)

        binding.recyclerCalendarGrid.layoutManager = GridLayoutManager(this, 7)

        binding.btnPrevMonth.setOnClickListener {
            displayMonthCalendar.add(Calendar.MONTH, -1)
            renderCalendarMonth()
        }

        binding.btnNextMonth.setOnClickListener {
            displayMonthCalendar.add(Calendar.MONTH, 1)
            renderCalendarMonth()
        }

        renderCalendarMonth()
    }

    private fun updateDateHeader(date: Date) {
        val displayFormat = SimpleDateFormat("EEE, MMM dd, yyyy", Locale.US)
        binding.txtSelectedDateHeader.text = "📅 ${displayFormat.format(date)}"
    }

    private fun renderCalendarMonth() {
        val monthYearFormat = SimpleDateFormat("MMMM yyyy", Locale.US)
        binding.txtCurrentMonthYear.text = monthYearFormat.format(displayMonthCalendar.time)

        val cal = displayMonthCalendar.clone() as Calendar
        cal.set(Calendar.DAY_OF_MONTH, 1)
        val firstDayOfWeek = cal.get(Calendar.DAY_OF_WEEK) - 1 // 0 for Sunday
        val maxDays = cal.getActualMaximum(Calendar.DAY_OF_MONTH)

        val isoFormat = SimpleDateFormat("yyyy-MM-dd", Locale.US)
        val days = mutableListOf<CalendarDayModel?>()

        // Leading blank days
        for (i in 0 until firstDayOfWeek) {
            days.add(null)
        }

        // Current month days
        for (day in 1..maxDays) {
            cal.set(Calendar.DAY_OF_MONTH, day)
            val dateStr = isoFormat.format(cal.time)

            val matchingRentals = rentalsList.filter {
                !it.status.equals("Archived", ignoreCase = true) &&
                !it.status.equals("Cancelled", ignoreCase = true) &&
                !it.status.equals("Denied", ignoreCase = true) &&
                !it.status.equals("Inquiry", ignoreCase = true) &&
                !it.status.equals("Responded", ignoreCase = true) &&
                it.eventType?.contains("Inquiry", ignoreCase = true) != true &&
                it.eventDate.substringBefore('T') == dateStr
            }

            val matchingUnavailable = unavailableDatesList.filter { unavail ->
                unavail.date.substringBefore('T') == dateStr &&
                !unavail.status.equals("Inquiry", ignoreCase = true) &&
                !unavail.status.equals("Responded", ignoreCase = true) &&
                !unavail.status.equals("Archived", ignoreCase = true) &&
                unavail.eventType?.contains("Inquiry", ignoreCase = true) != true &&
                unavail.reason?.contains("Inquiry", ignoreCase = true) != true &&
                !matchingRentals.any { r ->
                    (unavail.eventType != null && unavail.eventType.equals(r.eventType, ignoreCase = true)) ||
                    (unavail.eventTime != null && unavail.eventTime.contains(r.startTime ?: "", ignoreCase = true))
                }
            }

            val hasBooked = matchingRentals.any {
                it.status.equals("Approved", ignoreCase = true) ||
                it.status.equals("Confirmed", ignoreCase = true) ||
                it.status.contains("Deposit", ignoreCase = true)
            }

            val hasPending = matchingRentals.any {
                it.status.equals("Pending", ignoreCase = true)
            }

            val hasClubEvent = matchingUnavailable.isNotEmpty()
            val hasBarService = matchingRentals.any { it.bartenderRequested }

            days.add(
                CalendarDayModel(
                    dayNumber = day,
                    dateString = dateStr,
                    isCurrentMonth = true,
                    hasBooked = hasBooked,
                    hasPending = hasPending,
                    hasClubEvent = hasClubEvent,
                    hasBarService = hasBarService
                )
            )
        }

        binding.recyclerCalendarGrid.adapter = CalendarDayAdapter(days)
    }

    private inner class CalendarDayAdapter(
        private val days: List<CalendarDayModel?>
    ) : RecyclerView.Adapter<CalendarDayAdapter.DayViewHolder>() {

        inner class DayViewHolder(val view: View) : RecyclerView.ViewHolder(view) {
            val layoutDayCell: View = view.findViewById(R.id.layoutDayCell)
            val txtDay: TextView = view.findViewById(R.id.txtDayNumber)
            val dotEmerald: View = view.findViewById(R.id.dotEmerald)
            val dotYellow: View = view.findViewById(R.id.dotYellow)
            val dotPurple: View = view.findViewById(R.id.dotPurple)
        }

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DayViewHolder {
            val view = layoutInflater.inflate(R.layout.item_calendar_day, parent, false)
            return DayViewHolder(view)
        }

        override fun onBindViewHolder(holder: DayViewHolder, position: Int) {
            val model = days[position]
            if (model == null) {
                holder.txtDay.text = ""
                holder.txtDay.background = null
                holder.layoutDayCell.background = null
                holder.dotEmerald.visibility = View.GONE
                holder.dotYellow.visibility = View.GONE
                holder.dotPurple.visibility = View.GONE
                holder.view.isClickable = false
                holder.view.setOnClickListener(null)
                return
            }

            holder.txtDay.text = model.dayNumber.toString()
            val isSelected = model.dateString == selectedDateString

            // Selected Day Circle Styling
            if (isSelected) {
                holder.txtDay.setBackgroundResource(R.drawable.bg_calendar_day_selected)
                holder.txtDay.setTextColor(getColor(R.color.bg_dark))
            } else {
                holder.txtDay.background = null
                holder.txtDay.setTextColor(getColor(R.color.text_primary))
            }

            // Day Cell Background: Subtle Amber tint if Bar Service is requested
            if (model.hasBarService) {
                holder.layoutDayCell.setBackgroundResource(R.drawable.bg_calendar_day_bar)
            } else {
                holder.layoutDayCell.setBackgroundResource(android.R.color.transparent)
            }

            holder.dotEmerald.visibility = if (model.hasBooked) View.VISIBLE else View.GONE
            holder.dotYellow.visibility = if (model.hasPending) View.VISIBLE else View.GONE
            holder.dotPurple.visibility = if (model.hasClubEvent) View.VISIBLE else View.GONE

            holder.view.isClickable = true
            holder.view.setOnClickListener {
                selectedDateString = model.dateString
                val sdf = SimpleDateFormat("yyyy-MM-dd", Locale.US)
                val parsed = sdf.parse(model.dateString)
                if (parsed != null) {
                    updateDateHeader(parsed)
                }
                notifyDataSetChanged()
                renderScheduleForSelectedDate()
            }
        }

        override fun getItemCount(): Int = days.size
    }

    private fun updateLastSyncLabel() {
        val syncTime = cacheManager.getLastSyncTime()
        if (syncTime > 0) {
            val sdf = SimpleDateFormat("h:mm a", Locale.US)
            binding.toolbarCalendar.subtitle = "Synced: ${sdf.format(Date(syncTime))}"
        } else {
            binding.toolbarCalendar.subtitle = null
        }
    }

    private fun loadData() {
        // 1. Instant Cache
        val cached = cacheManager.getRentals()
        val cachedUnavail = cacheManager.getUnavailableDates()
        if (cached.isNotEmpty() || cachedUnavail.isNotEmpty()) {
            if (cached.isNotEmpty()) {
                rentalsList.clear()
                rentalsList.addAll(cached)
            }
            if (cachedUnavail.isNotEmpty()) {
                unavailableDatesList.clear()
                unavailableDatesList.addAll(cachedUnavail)
            }
            updateSummaryCounters()
            renderCalendarMonth()
            renderScheduleForSelectedDate()
        }
        updateLastSyncLabel()

        // 2. Fresh Network Sync
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val rentalsResponse = ApiClient.service.getRentalsList()
                val unavailResponse = ApiClient.service.getUnavailableDates()

                withContext(Dispatchers.Main) {
                    binding.swipeRefreshCalendar.isRefreshing = false

                    if (rentalsResponse.isSuccessful && rentalsResponse.body() != null) {
                        isOfflineMode = false
                        rentalsList.clear()
                        rentalsList.addAll(rentalsResponse.body()!!)
                        cacheManager.saveRentals(rentalsList)
                    }

                    if (unavailResponse.isSuccessful && unavailResponse.body() != null) {
                        isOfflineMode = false
                        unavailableDatesList.clear()
                        unavailableDatesList.addAll(unavailResponse.body()!!)
                        cacheManager.saveUnavailableDates(unavailableDatesList)
                    }

                    updateLastSyncLabel()
                    updateSummaryCounters()
                    renderCalendarMonth()
                    renderScheduleForSelectedDate()
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    isOfflineMode = true
                    binding.swipeRefreshCalendar.isRefreshing = false
                    updateLastSyncLabel()
                }
            }
        }
    }

    private fun updateSummaryCounters() {
        val approvedCount = rentalsList.count { 
            !it.status.equals("Archived", ignoreCase = true) &&
            !it.status.equals("Denied", ignoreCase = true) &&
            !it.status.equals("Cancelled", ignoreCase = true) &&
            (it.status.equals("Approved", ignoreCase = true) || it.status.equals("Confirmed", ignoreCase = true) || it.status.contains("Deposit", ignoreCase = true))
        }

        val pendingCount = rentalsList.count { 
            !it.status.equals("Archived", ignoreCase = true) &&
            !it.status.equals("Denied", ignoreCase = true) &&
            !it.status.equals("Cancelled", ignoreCase = true) &&
            !it.status.equals("Inquiry", ignoreCase = true) &&
            !it.status.equals("Responded", ignoreCase = true) &&
            it.eventType?.contains("Inquiry", ignoreCase = true) != true &&
            it.status.equals("Pending", ignoreCase = true)
        }

        val clubEventsCount = unavailableDatesList.count { 
            (it.eventType?.contains("Club", ignoreCase = true) == true) || 
            (it.eventType?.contains("Meeting", ignoreCase = true) == true) ||
            (it.reason?.contains("Club", ignoreCase = true) == true) ||
            (it.reason?.contains("Meeting", ignoreCase = true) == true) ||
            it.status.equals("Blackout", ignoreCase = true)
        }

        binding.badgeApprovedCount.text = "🟢 $approvedCount Booked"
        binding.badgePendingCount.text = "🟡 $pendingCount Pending"
        binding.badgeClubEventsCount.text = "🟣 $clubEventsCount Club Events"
    }

    private data class StandardSlot(
        val name: String,
        val startTime: String,
        val endTime: String,
        val startMin: Int,
        val endMin: Int
    )

    private fun parseTimeToMinutes(timeStr: String?): Int {
        if (timeStr.isNullOrBlank()) return -1
        return try {
            val clean = timeStr.trim().uppercase(Locale.US)
            val isPm = clean.contains("PM")
            val isAm = clean.contains("AM")
            val timeOnly = clean.replace("AM", "").replace("PM", "").trim()
            val parts = timeOnly.split(":")
            if (parts.isEmpty()) return -1
            var hours = parts[0].trim().toIntOrNull() ?: return -1
            val minutes = if (parts.size > 1) parts[1].trim().toIntOrNull() ?: 0 else 0
            if (isPm && hours < 12) hours += 12
            if (isAm && hours == 12) hours = 0
            hours * 60 + minutes
        } catch (_: Exception) {
            -1
        }
    }

    private fun renderScheduleForSelectedDate() {
        val container = binding.layoutDayEventsContainer
        container.removeAllViews()

        val matchingRentals = rentalsList.filter { 
            !it.status.equals("Archived", ignoreCase = true) &&
            !it.status.equals("Cancelled", ignoreCase = true) &&
            !it.status.equals("Denied", ignoreCase = true) &&
            !it.status.equals("Inquiry", ignoreCase = true) &&
            !it.status.equals("Responded", ignoreCase = true) &&
            it.eventType?.contains("Inquiry", ignoreCase = true) != true &&
            it.eventDate.substringBefore('T') == selectedDateString
        }

        val matchingUnavailable = unavailableDatesList.filter { unavail ->
            unavail.date.substringBefore('T') == selectedDateString &&
            !unavail.status.equals("Inquiry", ignoreCase = true) &&
            !unavail.status.equals("Responded", ignoreCase = true) &&
            !unavail.status.equals("Archived", ignoreCase = true) &&
            unavail.eventType?.contains("Inquiry", ignoreCase = true) != true &&
            unavail.reason?.contains("Inquiry", ignoreCase = true) != true &&
            !matchingRentals.any { r ->
                (unavail.eventType != null && unavail.eventType.equals(r.eventType, ignoreCase = true)) ||
                (unavail.eventTime != null && unavail.eventTime.contains(r.startTime ?: "", ignoreCase = true))
            }
        }

        val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)
        val dp8 = (8 * resources.displayMetrics.density).toInt()
        val dp12 = (12 * resources.displayMetrics.density).toInt()

        val hasFullDayBlackout = matchingUnavailable.any { it.isFullDay }
        val standardSlots = listOf(
            StandardSlot("Afternoon Slot", "12:00 PM", "5:00 PM", 12 * 60, 17 * 60),
            StandardSlot("Evening Slot", "6:00 PM", "11:00 PM", 18 * 60, 23 * 60)
        )

        val availableSlots = if (hasFullDayBlackout) {
            emptyList()
        } else {
            standardSlots.filter { slot ->
                val hasConflict = matchingRentals.any { rental ->
                    val rStart = parseTimeToMinutes(rental.startTime)
                    val rEnd = parseTimeToMinutes(rental.endTime)
                    if (rStart >= 0 && rEnd >= 0) {
                        slot.startMin < rEnd && slot.endMin > rStart
                    } else {
                        true // Treat unspecified rental times as conflicting
                    }
                }
                !hasConflict
            }
        }

        binding.layoutDayEmpty.visibility = View.GONE

        // Update Day Status Pill
        when {
            matchingRentals.isEmpty() && matchingUnavailable.isEmpty() -> {
                binding.txtDayStatusPill.text = "Available (All Day)"
                binding.txtDayStatusPill.setTextColor(getColor(R.color.emerald_accent))
                binding.txtDayStatusPill.setBackgroundResource(R.drawable.bg_badge_emerald)
            }
            availableSlots.isNotEmpty() -> {
                binding.txtDayStatusPill.text = "${availableSlots.size} Slot${if (availableSlots.size > 1) "s" else ""} Available"
                binding.txtDayStatusPill.setTextColor(getColor(R.color.emerald_accent))
                binding.txtDayStatusPill.setBackgroundResource(R.drawable.bg_badge_emerald)
            }
            else -> {
                binding.txtDayStatusPill.text = "Fully Booked"
                binding.txtDayStatusPill.setTextColor(getColor(R.color.coral_red))
                binding.txtDayStatusPill.setBackgroundResource(R.drawable.bg_badge_inquiry)
            }
        }

        // 1. Render matching private hall rental bookings
        for (rental in matchingRentals) {
            val isApproved = rental.status.equals("Approved", ignoreCase = true) || rental.status.equals("Confirmed", ignoreCase = true)
            val isPending = rental.status.equals("Pending", ignoreCase = true)

            val card = MaterialCardView(this).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    bottomMargin = dp8
                }
                setCardBackgroundColor(getColor(R.color.surface_dark_card))
                radius = 12 * resources.displayMetrics.density
                strokeWidth = (1.5 * resources.displayMetrics.density).toInt()
                strokeColor = if (isApproved) getColor(R.color.emerald_accent) else getColor(R.color.status_yellow)

                val cardLayout = LinearLayout(context).apply {
                    orientation = LinearLayout.VERTICAL
                    setPadding(dp12, dp12, dp12, dp12)

                    // Title & Status Row
                    val topRow = LinearLayout(context).apply {
                        orientation = LinearLayout.HORIZONTAL
                        gravity = Gravity.CENTER_VERTICAL

                        val titleTv = TextView(context).apply {
                            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f)
                            text = "👤 ${rental.applicantName}"
                            setTextColor(getColor(R.color.text_primary))
                            textSize = 15f
                            setTypeface(null, Typeface.BOLD)
                        }

                        val statusBadge = TextView(context).apply {
                            text = rental.status
                            textSize = 11f
                            setTypeface(null, Typeface.BOLD)
                            setPadding(dp8, dp8 / 2, dp8, dp8 / 2)
                            if (isApproved) {
                                setTextColor(getColor(R.color.emerald_accent))
                                setBackgroundResource(R.drawable.bg_badge_emerald)
                            } else {
                                setTextColor(getColor(R.color.status_yellow))
                                setBackgroundResource(R.drawable.bg_badge_inquiry)
                            }
                        }

                        val isPaidInFull = (rental.isPaid == true) || ((rental.amountPaid ?: 0.0) >= rental.totalPrice && rental.totalPrice > 0)
                        val paidBadge = TextView(context).apply {
                            val amount = rental.amountPaid ?: 0.0
                            text = when {
                                isPaidInFull -> "PAID"
                                amount > 0 -> "PARTIAL ($${amount.toInt()})"
                                else -> "UNPAID"
                            }
                            textSize = 11f
                            setTypeface(null, Typeface.BOLD)
                            setPadding(dp8, dp8 / 2, dp8, dp8 / 2)
                            when {
                                isPaidInFull -> {
                                    setTextColor(getColor(R.color.emerald_accent))
                                    setBackgroundResource(R.drawable.bg_badge_emerald)
                                }
                                amount > 0 -> {
                                    setTextColor(getColor(R.color.status_yellow))
                                    setBackgroundResource(R.drawable.bg_badge_inquiry)
                                }
                                else -> {
                                    setTextColor(getColor(R.color.status_red))
                                    setBackgroundResource(R.drawable.bg_badge_inquiry)
                                }
                            }
                            val lp = LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT).apply {
                                marginStart = dp8 / 2
                            }
                            layoutParams = lp
                        }

                        addView(titleTv)
                        addView(statusBadge)
                        addView(paidBadge)
                    }
                    addView(topRow)

                    // Event Details (with prominent Bar Service indicator if selected)
                    val barTag = if (rental.bartenderRequested) " • 🍸 Bar Included" else ""
                    val detailsTv = TextView(context).apply {
                        text = "🏛️ ${rental.roomSelected ?: "Function Hall"} • ${rental.eventType ?: "Rental"} • ${rental.guestCount} Guests$barTag"
                        setTextColor(if (rental.bartenderRequested) getColor(R.color.gold_accent) else getColor(R.color.text_secondary))
                        textSize = 12f
                        setPadding(0, dp8 / 2, 0, 0)
                    }
                    addView(detailsTv)

                    // Time & Quote & Paid Status
                    val isPaidInFullSummary = (rental.isPaid == true) || ((rental.amountPaid ?: 0.0) >= rental.totalPrice && rental.totalPrice > 0)
                    val paidSummary = if (isPaidInFullSummary) "Paid in Full" else if ((rental.amountPaid ?: 0.0) > 0) "Paid: ${currencyFormat.format(rental.amountPaid)}" else "Unpaid"
                    val timePriceTv = TextView(context).apply {
                        text = "⏰ ${rental.startTime ?: "2:00 PM"} - ${rental.endTime ?: "7:00 PM"}  •  Quote: ${currencyFormat.format(rental.totalPrice)}  •  💳 $paidSummary"
                        setTextColor(getColor(R.color.cyan_accent))
                        textSize = 12f
                        setTypeface(null, Typeface.BOLD)
                        setPadding(0, dp8 / 2, 0, 0)
                    }
                    addView(timePriceTv)
                }

                addView(cardLayout)

                setOnClickListener {
                    val intent = Intent(this@RentalCalendarActivity, RentalDetailActivity::class.java).apply {
                        putExtra(RentalDetailActivity.EXTRA_RENTAL_ID, rental.id)
                    }
                    startActivity(intent)
                }
            }

            container.addView(card)
        }

        // 2. Render matching club blackout / public events
        for (unavail in matchingUnavailable) {
            val card = MaterialCardView(this).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    bottomMargin = dp8
                }
                setCardBackgroundColor(getColor(R.color.surface_dark_card))
                radius = 12 * resources.displayMetrics.density
                strokeWidth = (1.5 * resources.displayMetrics.density).toInt()
                strokeColor = getColor(R.color.purple_accent)
                isClickable = true
                isFocusable = true
                setOnClickListener {
                    promptEditClubEvent(unavail)
                }

                val cardLayout = LinearLayout(context).apply {
                    orientation = LinearLayout.VERTICAL
                    setPadding(dp12, dp12, dp12, dp12)

                    val topRow = LinearLayout(context).apply {
                        orientation = LinearLayout.HORIZONTAL
                        gravity = Gravity.CENTER_VERTICAL
                        layoutParams = LinearLayout.LayoutParams(
                            LinearLayout.LayoutParams.MATCH_PARENT,
                            LinearLayout.LayoutParams.WRAP_CONTENT
                        )

                        val titleTv = TextView(context).apply {
                            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f)
                            val displayTitle = unavail.eventType ?: unavail.reason ?: "Club Event / Blackout"
                            text = "🟣 $displayTitle"
                            setTextColor(getColor(R.color.text_primary))
                            textSize = 14f
                            setTypeface(null, Typeface.BOLD)
                        }

                        val editBadge = TextView(context).apply {
                            text = "✏️ EDIT"
                            textSize = 10f
                            setTypeface(null, Typeface.BOLD)
                            setTextColor(getColor(R.color.purple_accent))
                            setBackgroundResource(R.drawable.bg_badge_gray)
                            setPadding(dp8, dp8 / 2, dp8, dp8 / 2)
                        }

                        addView(titleTv)
                        addView(editBadge)
                    }
                    addView(topRow)

                    val timeTv = TextView(context).apply {
                        val timeDesc = when {
                            !unavail.eventTime.isNullOrBlank() -> "⏰ ${unavail.eventTime}"
                            unavail.isFullDay -> "⏰ Full Day Event / Blackout"
                            else -> "⏰ Club Event / Blackout Date"
                        }
                        text = timeDesc
                        setTextColor(getColor(R.color.purple_accent))
                        textSize = 12f
                        setPadding(0, dp8 / 2, 0, 0)
                    }
                    addView(timeTv)
                }

                addView(cardLayout)
            }
            container.addView(card)
        }

        // 3. Render Available Slots on this Date
        for (slot in availableSlots) {
            val card = MaterialCardView(this).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    bottomMargin = dp8
                }
                setCardBackgroundColor(getColor(R.color.surface_dark_card))
                radius = 12 * resources.displayMetrics.density
                strokeWidth = (1.5 * resources.displayMetrics.density).toInt()
                strokeColor = getColor(R.color.emerald_accent)

                val cardLayout = LinearLayout(context).apply {
                    orientation = LinearLayout.VERTICAL
                    setPadding(dp12, dp12, dp12, dp12)

                    // Header Row
                    val topRow = LinearLayout(context).apply {
                        orientation = LinearLayout.HORIZONTAL
                        gravity = Gravity.CENTER_VERTICAL

                        val titleTv = TextView(context).apply {
                            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f)
                            text = "✨ ${slot.name}"
                            setTextColor(getColor(R.color.emerald_accent))
                            textSize = 15f
                            setTypeface(null, Typeface.BOLD)
                        }

                        val statusBadge = TextView(context).apply {
                            text = "AVAILABLE"
                            textSize = 11f
                            setTypeface(null, Typeface.BOLD)
                            setTextColor(getColor(R.color.emerald_accent))
                            setBackgroundResource(R.drawable.bg_badge_emerald)
                            setPadding(dp8, dp8 / 2, dp8, dp8 / 2)
                        }

                        addView(titleTv)
                        addView(statusBadge)
                    }
                    addView(topRow)

                    // Time & info
                    val detailsTv = TextView(context).apply {
                        text = "⏰ ${slot.startTime} – ${slot.endTime}  •  Open for Hall Bookings"
                        setTextColor(getColor(R.color.text_secondary))
                        textSize = 12f
                        setPadding(0, dp8 / 2, 0, 0)
                    }
                    addView(detailsTv)
                }

                addView(cardLayout)
            }
            container.addView(card)
        }
    }


    private fun formatMinutesTo12Hour(minutes: Int): String {
        val h24 = minutes / 60
        val m = minutes % 60
        val isPm = h24 >= 12
        val h12 = when {
            h24 == 0 -> 12
            h24 > 12 -> h24 - 12
            else -> h24
        }
        val amPm = if (isPm) "PM" else "AM"
        return String.format(Locale.US, "%02d:%02d %s", h12, m, amPm)
    }

    private fun checkSlotConflict(
        dateStr: String,
        location: String,
        isFullDay: Boolean,
        startMin: Int,
        endMin: Int,
        excludeEvent: UnavailableDateDto? = null,
        excludeEventId: Int = 0
    ): String? {
        val isSecondary = location.equals("Office", ignoreCase = true)

        val matchingRentals = if (isSecondary) {
            // Function Hall rentals never conflict with the secondary space (Office)
            emptyList()
        } else {
            rentalsList.filter { 
                !it.status.equals("Archived", ignoreCase = true) &&
                !it.status.equals("Cancelled", ignoreCase = true) &&
                !it.status.equals("Denied", ignoreCase = true) &&
                !it.status.equals("Inquiry", ignoreCase = true) &&
                !it.status.equals("Responded", ignoreCase = true) &&
                it.eventType?.contains("Inquiry", ignoreCase = true) != true &&
                it.eventDate.substringBefore('T') == dateStr
            }
        }

        val matchingUnavailable = unavailableDatesList.filter { unavail ->
            if (unavail.date.substringBefore('T') != dateStr) return@filter false
            if (unavail.status.equals("Inquiry", ignoreCase = true) ||
                unavail.status.equals("Responded", ignoreCase = true) ||
                unavail.status.equals("Archived", ignoreCase = true) ||
                unavail.eventType?.contains("Inquiry", ignoreCase = true) == true ||
                unavail.reason?.contains("Inquiry", ignoreCase = true) == true) return@filter false

            // Exclude the event currently being edited
            if (excludeEvent != null) {
                if (unavail === excludeEvent) return@filter false
                if (excludeEvent.id > 0 && unavail.id == excludeEvent.id) return@filter false
                val eTitle = excludeEvent.eventType ?: excludeEvent.reason
                val uTitle = unavail.eventType ?: unavail.reason
                if (!eTitle.isNullOrBlank() && eTitle.equals(uTitle, ignoreCase = true) &&
                    unavail.eventTime == excludeEvent.eventTime &&
                    unavail.isFullDay == excludeEvent.isFullDay) {
                    return@filter false
                }
            } else if (excludeEventId > 0 && unavail.id == excludeEventId) {
                return@filter false
            }
            
            val desc = "${unavail.eventType ?: ""} ${unavail.reason ?: ""}"
            val isEventInOffice = desc.contains("Office", ignoreCase = true)

            if (isSecondary) {
                // Secondary space ONLY conflicts with other Office events
                isEventInOffice
            } else {
                // Function hall ONLY conflicts with non-Office events/blackouts
                !isEventInOffice && !matchingRentals.any { r ->
                    (unavail.eventType != null && unavail.eventType.equals(r.eventType, ignoreCase = true)) ||
                    (unavail.eventTime != null && unavail.eventTime.contains(r.startTime ?: "", ignoreCase = true))
                }
            }
        }

        if (isFullDay) {
            val rConflict = matchingRentals.firstOrNull()
            if (rConflict != null) {
                val time = if (!rConflict.startTime.isNullOrBlank()) " (${rConflict.startTime} - ${rConflict.endTime})" else " (Full Day)"
                return "Booking by '${rConflict.applicantName}'$time"
            }
            val uConflict = matchingUnavailable.firstOrNull()
            if (uConflict != null) {
                val name = uConflict.eventType ?: uConflict.reason ?: "Club Event"
                val time = if (!uConflict.eventTime.isNullOrBlank()) " (${uConflict.eventTime})" else " (Full Day)"
                return "Club Event '$name'$time"
            }
            return null
        }

        if (startMin >= 0 && endMin > startMin) {
            for (rental in matchingRentals) {
                val rStart = parseTimeToMinutes(rental.startTime)
                val rEnd = parseTimeToMinutes(rental.endTime)
                if (rStart < 0 || rEnd < 0) {
                    return "Full day booking by '${rental.applicantName}'"
                }
                if (startMin < rEnd && endMin > rStart) {
                    return "Booking by '${rental.applicantName}' (${rental.startTime} - ${rental.endTime})"
                }
            }

            for (unavail in matchingUnavailable) {
                if (unavail.isFullDay || unavail.eventTime.isNullOrBlank()) {
                    val name = unavail.eventType ?: unavail.reason ?: "Club Event"
                    return "Full day event '$name'"
                }
                val parts = unavail.eventTime.split("-")
                if (parts.size == 2) {
                    val uStart = parseTimeToMinutes(parts[0])
                    val uEnd = parseTimeToMinutes(parts[1])
                    if (uStart >= 0 && uEnd >= 0 && startMin < uEnd && endMin > uStart) {
                        val name = unavail.eventType ?: unavail.reason ?: "Club Event"
                        return "Club event '$name' (${unavail.eventTime})"
                    }
                }
            }
        }

        return null
    }

    private fun promptCreateClubEvent() {
        if (isOfflineMode || !networkMonitor.isOnline) {
            MaterialAlertDialogBuilder(this)
                .setTitle("⚠️ Offline Mode (View Only)")
                .setMessage("You are currently disconnected from the server. New club events cannot be created while offline. Please connect to the network to schedule events.")
                .setPositiveButton("OK", null)
                .show()
            return
        }

        val dp8 = (8 * resources.displayMetrics.density).toInt()
        val dp12 = (12 * resources.displayMetrics.density).toInt()
        val dp16 = (16 * resources.displayMetrics.density).toInt()

        val layout = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp16, dp8, dp16, dp8)
            isClickable = true
            isFocusable = true
            isFocusableInTouchMode = true
        }

        val dateLabel = TextView(this).apply {
            text = "📅 Target Date: $selectedDateString"
            setTextColor(getColor(R.color.gold_accent))
            textSize = 14f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 0, 0, dp8)
        }
        layout.addView(dateLabel)

        val inputReason = EditText(this).apply {
            hint = "Club Event Title (e.g. Board Meeting, Club Dinner)"
            setTextColor(getColor(R.color.text_primary))
            setHintTextColor(getColor(R.color.text_muted))
            textSize = 14f
            background = getDrawable(R.drawable.bg_edittext_dark)
            setPadding(dp16, dp8 * 3 / 2, dp16, dp8 * 3 / 2)
            maxLines = 1
            isSingleLine = true
            imeOptions = android.view.inputmethod.EditorInfo.IME_ACTION_DONE
            setOnEditorActionListener { v, actionId, _ ->
                if (actionId == android.view.inputmethod.EditorInfo.IME_ACTION_DONE) {
                    val imm = getSystemService(android.content.Context.INPUT_METHOD_SERVICE) as? android.view.inputmethod.InputMethodManager
                    imm?.hideSoftInputFromWindow(v.windowToken, 0)
                    v.clearFocus()
                    true
                } else {
                    false
                }
            }
        }
        layout.addView(inputReason)

        layout.setOnTouchListener { v, event ->
            if (event.action == android.view.MotionEvent.ACTION_DOWN) {
                val imm = getSystemService(android.content.Context.INPUT_METHOD_SERVICE) as? android.view.inputmethod.InputMethodManager
                imm?.hideSoftInputFromWindow(v.windowToken, 0)
                inputReason.clearFocus()
            }
            false
        }

        val locationLabel = TextView(this).apply {
            text = "Select Event Space / Location:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8 * 2, 0, dp8 / 2)
        }
        layout.addView(locationLabel)

        val locationRadioGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }

        val rbFunctionHall = RadioButton(this).apply {
            id = View.generateViewId()
            text = "🏛️ Function Hall (Primary Managed Space)"
            setTextColor(getColor(R.color.text_primary))
            isChecked = true
        }
        val rbOffice = RadioButton(this).apply {
            id = View.generateViewId()
            text = "🏢 Office (Secondary Flexible Space)"
            setTextColor(getColor(R.color.text_primary))
        }

        locationRadioGroup.addView(rbFunctionHall)
        locationRadioGroup.addView(rbOffice)
        layout.addView(locationRadioGroup)

        // ==========================================
        // SECTION A: FUNCTION HALL FIXED SLOTS LAYOUT
        // ==========================================
        val layoutHallSlots = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            visibility = View.VISIBLE
        }

        val hallTimeLabel = TextView(this).apply {
            text = "Function Hall Time Slot:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8, 0, dp8 / 2)
        }
        layoutHallSlots.addView(hallTimeLabel)

        val hallRadioGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }

        val rbFullDay = RadioButton(this).apply {
            id = View.generateViewId()
            setTextColor(getColor(R.color.text_primary))
            isChecked = true
        }
        val rbAfternoon = RadioButton(this).apply {
            id = View.generateViewId()
            setTextColor(getColor(R.color.text_primary))
        }
        val rbEvening = RadioButton(this).apply {
            id = View.generateViewId()
            setTextColor(getColor(R.color.text_primary))
        }

        hallRadioGroup.addView(rbFullDay)
        hallRadioGroup.addView(rbAfternoon)
        hallRadioGroup.addView(rbEvening)
        layoutHallSlots.addView(hallRadioGroup)
        layout.addView(layoutHallSlots)

        // ==========================================
        // SECTION B: OFFICE CUSTOM TIME PICKER LAYOUT
        // ==========================================
        val layoutOfficeTime = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            visibility = View.GONE
        }

        val officeTimeLabel = TextView(this).apply {
            text = "Office Schedule / Hours:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8, 0, dp8 / 2)
        }
        layoutOfficeTime.addView(officeTimeLabel)

        val officeTypeGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }
        val rbOfficeCustom = RadioButton(this).apply {
            id = View.generateViewId()
            text = "⏰ Specific Custom Hours"
            setTextColor(getColor(R.color.text_primary))
            isChecked = true
        }
        val rbOfficeFullDay = RadioButton(this).apply {
            id = View.generateViewId()
            text = "📅 Full Day Event / Blackout"
            setTextColor(getColor(R.color.text_primary))
        }
        officeTypeGroup.addView(rbOfficeCustom)
        officeTypeGroup.addView(rbOfficeFullDay)
        layoutOfficeTime.addView(officeTypeGroup)

        // Custom start & end time pickers
        var officeStartMin = 18 * 60 // 6:00 PM default
        var officeEndMin = 20 * 60 + 30 // 8:30 PM default

        val layoutCustomHoursRow = LinearLayout(this).apply {
            orientation = LinearLayout.HORIZONTAL
            setPadding(0, dp8, 0, 0)
        }

        val btnStartTime = MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f).apply {
                marginEnd = dp8 / 2
            }
            text = "Start: ${formatMinutesTo12Hour(officeStartMin)}"
            setTextColor(getColor(R.color.gold_accent))
            strokeColor = android.content.res.ColorStateList.valueOf(getColor(R.color.gold_accent))
            textSize = 12f
        }

        val btnEndTime = MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f).apply {
                marginStart = dp8 / 2
            }
            text = "End: ${formatMinutesTo12Hour(officeEndMin)}"
            setTextColor(getColor(R.color.gold_accent))
            strokeColor = android.content.res.ColorStateList.valueOf(getColor(R.color.gold_accent))
            textSize = 12f
        }

        layoutCustomHoursRow.addView(btnStartTime)
        layoutCustomHoursRow.addView(btnEndTime)
        layoutOfficeTime.addView(layoutCustomHoursRow)
        layout.addView(layoutOfficeTime)

        // ==========================================
        // CONFLICT / STATUS CARD LIVE BANNER
        // ==========================================
        val cardStatus = MaterialCardView(this).apply {
            layoutParams = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
            ).apply {
                topMargin = dp12
                bottomMargin = dp8
            }
            radius = 8 * resources.displayMetrics.density
            strokeWidth = (1.5 * resources.displayMetrics.density).toInt()
        }

        val txtStatus = TextView(this).apply {
            textSize = 12f
            setTypeface(null, Typeface.BOLD)
            setPadding(dp12, dp8, dp12, dp8)
        }
        cardStatus.addView(txtStatus)
        layout.addView(cardStatus)

        // Live evaluation function
        lateinit var updateSlotEvaluations: () -> Unit
        updateSlotEvaluations = {
            if (rbFunctionHall.isChecked) {
                layoutHallSlots.visibility = View.VISIBLE
                layoutOfficeTime.visibility = View.GONE

                val fullDayConflict = checkSlotConflict(selectedDateString, "Function Hall", true, -1, -1)
                val afternoonConflict = checkSlotConflict(selectedDateString, "Function Hall", false, 12 * 60, 17 * 60)
                val eveningConflict = checkSlotConflict(selectedDateString, "Function Hall", false, 18 * 60, 23 * 60)

                rbFullDay.text = if (fullDayConflict == null) "Full Day Blackout  •  🟢 Available" else "Full Day Blackout  •  🔴 Unavailable"
                rbAfternoon.text = if (afternoonConflict == null) "Afternoon (12:00 PM – 5:00 PM)  •  🟢 Available" else "Afternoon (12:00 PM – 5:00 PM)  •  🔴 Conflict"
                rbEvening.text = if (eveningConflict == null) "Evening (6:00 PM – 11:00 PM)  •  🟢 Available" else "Evening (6:00 PM – 11:00 PM)  •  🔴 Conflict"

                val currentConflict = when {
                    rbFullDay.isChecked -> fullDayConflict
                    rbAfternoon.isChecked -> afternoonConflict
                    rbEvening.isChecked -> eveningConflict
                    else -> null
                }

                if (currentConflict != null) {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.coral_red)
                    txtStatus.text = "⚠️ Function Hall Conflict: $currentConflict\nChoose a different slot or switch to Office."
                    txtStatus.setTextColor(getColor(R.color.coral_red))
                } else {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.emerald_accent)
                    txtStatus.text = "✓ Function Hall Available: No conflicting bookings for this slot."
                    txtStatus.setTextColor(getColor(R.color.emerald_accent))
                }
            } else {
                layoutHallSlots.visibility = View.GONE
                layoutOfficeTime.visibility = View.VISIBLE
                layoutCustomHoursRow.visibility = if (rbOfficeCustom.isChecked) View.VISIBLE else View.GONE

                val isOfficeFullDay = rbOfficeFullDay.isChecked
                val conflict = if (isOfficeFullDay) {
                    checkSlotConflict(selectedDateString, "Office", true, -1, -1)
                } else {
                    if (officeEndMin <= officeStartMin) {
                        "End time must be after start time"
                    } else {
                        checkSlotConflict(selectedDateString, "Office", false, officeStartMin, officeEndMin)
                    }
                }

                if (conflict != null) {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.coral_red)
                    txtStatus.text = "⚠️ Office Conflict: $conflict\nPlease adjust office hours."
                    txtStatus.setTextColor(getColor(R.color.coral_red))
                } else {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.emerald_accent)
                    val timeDesc = if (isOfficeFullDay) "Full Day" else "${formatMinutesTo12Hour(officeStartMin)} - ${formatMinutesTo12Hour(officeEndMin)}"
                    txtStatus.text = "✓ Office Space Available ($timeDesc): No office conflicts."
                    txtStatus.setTextColor(getColor(R.color.emerald_accent))
                }
            }
        }

        val hideKeyboardHelper = {
            val imm = getSystemService(android.content.Context.INPUT_METHOD_SERVICE) as? android.view.inputmethod.InputMethodManager
            imm?.hideSoftInputFromWindow(layout.windowToken, 0)
            inputReason.clearFocus()
        }

        btnStartTime.setOnClickListener {
            hideKeyboardHelper()
            val curH = officeStartMin / 60
            val curM = officeStartMin % 60
            TimePickerDialog(this, { _, hourOfDay, minute ->
                officeStartMin = hourOfDay * 60 + minute
                btnStartTime.text = "Start: ${formatMinutesTo12Hour(officeStartMin)}"
                updateSlotEvaluations()
            }, curH, curM, false).show()
        }

        btnEndTime.setOnClickListener {
            hideKeyboardHelper()
            val curH = officeEndMin / 60
            val curM = officeEndMin % 60
            TimePickerDialog(this, { _, hourOfDay, minute ->
                officeEndMin = hourOfDay * 60 + minute
                btnEndTime.text = "End: ${formatMinutesTo12Hour(officeEndMin)}"
                updateSlotEvaluations()
            }, curH, curM, false).show()
        }

        locationRadioGroup.setOnCheckedChangeListener { _, _ -> 
            hideKeyboardHelper()
            updateSlotEvaluations() 
        }
        hallRadioGroup.setOnCheckedChangeListener { _, _ -> 
            hideKeyboardHelper()
            updateSlotEvaluations() 
        }
        officeTypeGroup.setOnCheckedChangeListener { _, _ -> 
            hideKeyboardHelper()
            updateSlotEvaluations() 
        }

        // Initial evaluation
        updateSlotEvaluations()

        val initFullConflict = checkSlotConflict(selectedDateString, "Function Hall", true, -1, -1)
        val initAfternoonConflict = checkSlotConflict(selectedDateString, "Function Hall", false, 12 * 60, 17 * 60)
        val initEveningConflict = checkSlotConflict(selectedDateString, "Function Hall", false, 18 * 60, 23 * 60)
        if (initFullConflict != null) {
            when {
                initAfternoonConflict == null -> rbAfternoon.isChecked = true
                initEveningConflict == null -> rbEvening.isChecked = true
            }
        }

        val dialog = MaterialAlertDialogBuilder(this)
            .setTitle("Book Club Event / Blackout")
            .setMessage("Select date, location, and verified available time slot:")
            .setView(layout)
            .setPositiveButton("Book Event", null)
            .setNegativeButton("Cancel", null)
            .create()

        dialog.show()

        dialog.getButton(androidx.appcompat.app.AlertDialog.BUTTON_POSITIVE).setOnClickListener {
            val reason = inputReason.text.toString().trim()
            if (reason.isEmpty()) {
                MaterialAlertDialogBuilder(this)
                    .setTitle("⚠️ Event Title Required")
                    .setMessage("Please enter a title or description for the club event before booking.")
                    .setPositiveButton("OK", null)
                    .show()
                return@setOnClickListener
            }

            val selectedLocation = if (rbOffice.isChecked) "Office" else "Function Hall"
            val isFullDay: Boolean
            val startTime: String?
            val endTime: String?

            if (selectedLocation == "Office") {
                if (rbOfficeFullDay.isChecked) {
                    isFullDay = true
                    startTime = null
                    endTime = null
                } else {
                    if (officeEndMin <= officeStartMin) {
                        MaterialAlertDialogBuilder(this)
                            .setTitle("⚠️ Invalid Time Range")
                            .setMessage("End time must be after start time. Please select a valid time range.")
                            .setPositiveButton("OK", null)
                            .show()
                        return@setOnClickListener
                    }
                    isFullDay = false
                    startTime = formatMinutesTo12Hour(officeStartMin)
                    endTime = formatMinutesTo12Hour(officeEndMin)
                }
            } else {
                isFullDay = rbFullDay.isChecked
                startTime = when {
                    rbAfternoon.isChecked -> "12:00 PM"
                    rbEvening.isChecked -> "06:00 PM"
                    else -> null
                }
                endTime = when {
                    rbAfternoon.isChecked -> "05:00 PM"
                    rbEvening.isChecked -> "11:00 PM"
                    else -> null
                }
            }

            val startMin = parseTimeToMinutes(startTime)
            val endMin = parseTimeToMinutes(endTime)
            val conflict = checkSlotConflict(selectedDateString, selectedLocation, isFullDay, startMin, endMin)

            if (conflict != null) {
                MaterialAlertDialogBuilder(this)
                    .setTitle("⚠️ Time Conflict")
                    .setMessage("Cannot schedule event for this slot:\n\n• $conflict\n\nPlease select an available time window or switch location.")
                    .setPositiveButton("OK", null)
                    .show()
                return@setOnClickListener
            }

            val payload = CreateClubEventPayload(
                date = selectedDateString,
                reason = reason,
                location = selectedLocation,
                startTime = startTime,
                endTime = endTime,
                isFullDay = isFullDay
            )
            dialog.dismiss()
            executeCreateClubEvent(payload)
        }
    }

    private fun promptEditClubEvent(unavail: UnavailableDateDto) {
        if (isOfflineMode || !networkMonitor.isOnline) {
            MaterialAlertDialogBuilder(this)
                .setTitle("⚠️ Offline Mode (View Only)")
                .setMessage("You are currently disconnected from the server. Club events cannot be edited or deleted while offline. Please connect to the network to make changes.")
                .setPositiveButton("OK", null)
                .show()
            return
        }

        val dp8 = (8 * resources.displayMetrics.density).toInt()
        val dp12 = (12 * resources.displayMetrics.density).toInt()
        val dp16 = (16 * resources.displayMetrics.density).toInt()

        val rawTitle = unavail.eventType ?: unavail.reason ?: ""
        val cleanTitle = rawTitle
            .replace(Regex("^Club Event \\([^)]+\\):?\\s*", RegexOption.IGNORE_CASE), "")
            .trim()

        val isOfficeInitial = rawTitle.contains("Office", ignoreCase = true)

        val layout = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp16, dp8, dp16, dp8)
            isClickable = true
            isFocusable = true
            isFocusableInTouchMode = true
        }

        val dateLabel = TextView(this).apply {
            text = "📅 Target Date: $selectedDateString"
            setTextColor(getColor(R.color.gold_accent))
            textSize = 14f
            setTypeface(null, Typeface.BOLD)
            setPadding(0, 0, 0, dp8)
        }
        layout.addView(dateLabel)

        val inputReason = EditText(this).apply {
            hint = "Club Event Title (e.g. Board Meeting, Club Dinner)"
            setText(cleanTitle)
            setTextColor(getColor(R.color.text_primary))
            setHintTextColor(getColor(R.color.text_muted))
            textSize = 14f
            background = getDrawable(R.drawable.bg_edittext_dark)
            setPadding(dp16, dp8 * 3 / 2, dp16, dp8 * 3 / 2)
            maxLines = 1
            isSingleLine = true
            imeOptions = android.view.inputmethod.EditorInfo.IME_ACTION_DONE
            setOnEditorActionListener { v, actionId, _ ->
                if (actionId == android.view.inputmethod.EditorInfo.IME_ACTION_DONE) {
                    val imm = getSystemService(android.content.Context.INPUT_METHOD_SERVICE) as? android.view.inputmethod.InputMethodManager
                    imm?.hideSoftInputFromWindow(v.windowToken, 0)
                    v.clearFocus()
                    true
                } else {
                    false
                }
            }
        }
        layout.addView(inputReason)

        layout.setOnTouchListener { v, event ->
            if (event.action == android.view.MotionEvent.ACTION_DOWN) {
                val imm = getSystemService(android.content.Context.INPUT_METHOD_SERVICE) as? android.view.inputmethod.InputMethodManager
                imm?.hideSoftInputFromWindow(v.windowToken, 0)
                inputReason.clearFocus()
            }
            false
        }

        val locationLabel = TextView(this).apply {
            text = "Select Event Space / Location:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8 * 2, 0, dp8 / 2)
        }
        layout.addView(locationLabel)

        val locationRadioGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }

        val rbFunctionHall = RadioButton(this).apply {
            id = View.generateViewId()
            text = "🏛️ Function Hall (Primary Managed Space)"
            setTextColor(getColor(R.color.text_primary))
            isChecked = !isOfficeInitial
        }
        val rbOffice = RadioButton(this).apply {
            id = View.generateViewId()
            text = "🏢 Office (Secondary Flexible Space)"
            setTextColor(getColor(R.color.text_primary))
            isChecked = isOfficeInitial
        }

        locationRadioGroup.addView(rbFunctionHall)
        locationRadioGroup.addView(rbOffice)
        layout.addView(locationRadioGroup)

        // ==========================================
        // SECTION A: FUNCTION HALL FIXED SLOTS LAYOUT
        // ==========================================
        val layoutHallSlots = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            visibility = if (!isOfficeInitial) View.VISIBLE else View.GONE
        }

        val hallTimeLabel = TextView(this).apply {
            text = "Function Hall Time Slot:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8, 0, dp8 / 2)
        }
        layoutHallSlots.addView(hallTimeLabel)

        val hallRadioGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }

        val rbFullDay = RadioButton(this).apply {
            id = View.generateViewId()
            setTextColor(getColor(R.color.text_primary))
            isChecked = unavail.isFullDay || unavail.eventTime.isNullOrBlank()
        }
        val rbAfternoon = RadioButton(this).apply {
            id = View.generateViewId()
            setTextColor(getColor(R.color.text_primary))
            isChecked = !unavail.isFullDay && unavail.eventTime?.contains("12", ignoreCase = true) == true
        }
        val rbEvening = RadioButton(this).apply {
            id = View.generateViewId()
            setTextColor(getColor(R.color.text_primary))
            isChecked = !unavail.isFullDay && unavail.eventTime?.contains("6", ignoreCase = true) == true
        }

        hallRadioGroup.addView(rbFullDay)
        hallRadioGroup.addView(rbAfternoon)
        hallRadioGroup.addView(rbEvening)
        layoutHallSlots.addView(hallRadioGroup)
        layout.addView(layoutHallSlots)

        // ==========================================
        // SECTION B: OFFICE CUSTOM TIME PICKER LAYOUT
        // ==========================================
        val layoutOfficeTime = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            visibility = if (isOfficeInitial) View.VISIBLE else View.GONE
        }

        val officeTimeLabel = TextView(this).apply {
            text = "Office Schedule / Hours:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8, 0, dp8 / 2)
        }
        layoutOfficeTime.addView(officeTimeLabel)

        val officeTypeGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }
        val rbOfficeCustom = RadioButton(this).apply {
            id = View.generateViewId()
            text = "⏰ Specific Custom Hours"
            setTextColor(getColor(R.color.text_primary))
            isChecked = !unavail.isFullDay && !unavail.eventTime.isNullOrBlank()
        }
        val rbOfficeFullDay = RadioButton(this).apply {
            id = View.generateViewId()
            text = "📅 Full Day Event / Blackout"
            setTextColor(getColor(R.color.text_primary))
            isChecked = unavail.isFullDay || unavail.eventTime.isNullOrBlank()
        }
        officeTypeGroup.addView(rbOfficeCustom)
        officeTypeGroup.addView(rbOfficeFullDay)
        layoutOfficeTime.addView(officeTypeGroup)

        // Parse initial times
        var officeStartMin = 18 * 60
        var officeEndMin = 20 * 60 + 30
        if (!unavail.eventTime.isNullOrBlank()) {
            val parts = unavail.eventTime.split("-")
            if (parts.size == 2) {
                val s = parseTimeToMinutes(parts[0])
                val e = parseTimeToMinutes(parts[1])
                if (s >= 0) officeStartMin = s
                if (e >= 0) officeEndMin = e
            }
        }

        val layoutCustomHoursRow = LinearLayout(this).apply {
            orientation = LinearLayout.HORIZONTAL
            setPadding(0, dp8, 0, 0)
        }

        val btnStartTime = MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f).apply {
                marginEnd = dp8 / 2
            }
            text = "Start: ${formatMinutesTo12Hour(officeStartMin)}"
            setTextColor(getColor(R.color.gold_accent))
            strokeColor = android.content.res.ColorStateList.valueOf(getColor(R.color.gold_accent))
            textSize = 12f
        }

        val btnEndTime = MaterialButton(this, null, com.google.android.material.R.attr.materialButtonOutlinedStyle).apply {
            layoutParams = LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WRAP_CONTENT, 1f).apply {
                marginStart = dp8 / 2
            }
            text = "End: ${formatMinutesTo12Hour(officeEndMin)}"
            setTextColor(getColor(R.color.gold_accent))
            strokeColor = android.content.res.ColorStateList.valueOf(getColor(R.color.gold_accent))
            textSize = 12f
        }

        layoutCustomHoursRow.addView(btnStartTime)
        layoutCustomHoursRow.addView(btnEndTime)
        layoutOfficeTime.addView(layoutCustomHoursRow)
        layout.addView(layoutOfficeTime)

        // Conflict / Status Card Live Banner
        val cardStatus = MaterialCardView(this).apply {
            layoutParams = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
            ).apply {
                topMargin = dp12
                bottomMargin = dp8
            }
            radius = 8 * resources.displayMetrics.density
            strokeWidth = (1.5 * resources.displayMetrics.density).toInt()
        }

        val txtStatus = TextView(this).apply {
            textSize = 12f
            setTypeface(null, Typeface.BOLD)
            setPadding(dp12, dp8, dp12, dp8)
        }
        cardStatus.addView(txtStatus)
        layout.addView(cardStatus)

        // Live evaluation function
        lateinit var updateSlotEvaluations: () -> Unit
        updateSlotEvaluations = {
            if (rbFunctionHall.isChecked) {
                layoutHallSlots.visibility = View.VISIBLE
                layoutOfficeTime.visibility = View.GONE

                val fullDayConflict = checkSlotConflict(selectedDateString, "Function Hall", true, -1, -1, unavail, unavail.id)
                val afternoonConflict = checkSlotConflict(selectedDateString, "Function Hall", false, 12 * 60, 17 * 60, unavail, unavail.id)
                val eveningConflict = checkSlotConflict(selectedDateString, "Function Hall", false, 18 * 60, 23 * 60, unavail, unavail.id)

                rbFullDay.text = if (fullDayConflict == null) "Full Day Blackout  •  🟢 Available" else "Full Day Blackout  •  🔴 Unavailable"
                rbAfternoon.text = if (afternoonConflict == null) "Afternoon (12:00 PM – 5:00 PM)  •  🟢 Available" else "Afternoon (12:00 PM – 5:00 PM)  •  🔴 Conflict"
                rbEvening.text = if (eveningConflict == null) "Evening (6:00 PM – 11:00 PM)  •  🟢 Available" else "Evening (6:00 PM – 11:00 PM)  •  🔴 Conflict"

                val currentConflict = when {
                    rbFullDay.isChecked -> fullDayConflict
                    rbAfternoon.isChecked -> afternoonConflict
                    rbEvening.isChecked -> eveningConflict
                    else -> null
                }

                if (currentConflict != null) {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.coral_red)
                    txtStatus.text = "⚠️ Function Hall Conflict: $currentConflict\nChoose a different slot or switch to Office."
                    txtStatus.setTextColor(getColor(R.color.coral_red))
                } else {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.emerald_accent)
                    txtStatus.text = "✓ Function Hall Available: No conflicting bookings for this slot."
                    txtStatus.setTextColor(getColor(R.color.emerald_accent))
                }
            } else {
                layoutHallSlots.visibility = View.GONE
                layoutOfficeTime.visibility = View.VISIBLE
                layoutCustomHoursRow.visibility = if (rbOfficeCustom.isChecked) View.VISIBLE else View.GONE

                val isOfficeFullDay = rbOfficeFullDay.isChecked
                val conflict = if (isOfficeFullDay) {
                    checkSlotConflict(selectedDateString, "Office", true, -1, -1, unavail, unavail.id)
                } else {
                    if (officeEndMin <= officeStartMin) {
                        "End time must be after start time"
                    } else {
                        checkSlotConflict(selectedDateString, "Office", false, officeStartMin, officeEndMin, unavail, unavail.id)
                    }
                }

                if (conflict != null) {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.coral_red)
                    txtStatus.text = "⚠️ Office Conflict: $conflict\nPlease adjust office hours."
                    txtStatus.setTextColor(getColor(R.color.coral_red))
                } else {
                    cardStatus.setCardBackgroundColor(getColor(R.color.surface_dark_muted))
                    cardStatus.strokeColor = getColor(R.color.emerald_accent)
                    val timeDesc = if (isOfficeFullDay) "Full Day" else "${formatMinutesTo12Hour(officeStartMin)} - ${formatMinutesTo12Hour(officeEndMin)}"
                    txtStatus.text = "✓ Office Space Available ($timeDesc): No office conflicts."
                    txtStatus.setTextColor(getColor(R.color.emerald_accent))
                }
            }
        }

        val hideKeyboardHelper = {
            val imm = getSystemService(android.content.Context.INPUT_METHOD_SERVICE) as? android.view.inputmethod.InputMethodManager
            imm?.hideSoftInputFromWindow(layout.windowToken, 0)
            inputReason.clearFocus()
        }

        btnStartTime.setOnClickListener {
            hideKeyboardHelper()
            val curH = officeStartMin / 60
            val curM = officeStartMin % 60
            TimePickerDialog(this, { _, hourOfDay, minute ->
                officeStartMin = hourOfDay * 60 + minute
                btnStartTime.text = "Start: ${formatMinutesTo12Hour(officeStartMin)}"
                updateSlotEvaluations()
            }, curH, curM, false).show()
        }

        btnEndTime.setOnClickListener {
            hideKeyboardHelper()
            val curH = officeEndMin / 60
            val curM = officeEndMin % 60
            TimePickerDialog(this, { _, hourOfDay, minute ->
                officeEndMin = hourOfDay * 60 + minute
                btnEndTime.text = "End: ${formatMinutesTo12Hour(officeEndMin)}"
                updateSlotEvaluations()
            }, curH, curM, false).show()
        }

        locationRadioGroup.setOnCheckedChangeListener { _, _ -> 
            hideKeyboardHelper()
            updateSlotEvaluations() 
        }
        hallRadioGroup.setOnCheckedChangeListener { _, _ -> 
            hideKeyboardHelper()
            updateSlotEvaluations() 
        }
        officeTypeGroup.setOnCheckedChangeListener { _, _ -> 
            hideKeyboardHelper()
            updateSlotEvaluations() 
        }

        // Initial evaluation
        updateSlotEvaluations()

        val dialog = MaterialAlertDialogBuilder(this)
            .setTitle("Edit Club Event")
            .setMessage("Update event details, location, or hours:")
            .setView(layout)
            .setPositiveButton("Save", null)
            .setNeutralButton("🗑️ Delete", null)
            .setNegativeButton("Cancel") { d, _ -> d.dismiss() }
            .create()

        dialog.show()

        dialog.getButton(androidx.appcompat.app.AlertDialog.BUTTON_NEGATIVE)?.apply {
            setTextColor(getColor(R.color.text_secondary))
            setOnClickListener { dialog.dismiss() }
        }

        dialog.getButton(androidx.appcompat.app.AlertDialog.BUTTON_POSITIVE)?.apply {
            setTextColor(getColor(R.color.cyan_accent))
        }

        dialog.getButton(androidx.appcompat.app.AlertDialog.BUTTON_NEUTRAL)?.apply {
            setTextColor(getColor(R.color.coral_red))
            setOnClickListener {
                MaterialAlertDialogBuilder(this@RentalCalendarActivity)
                    .setTitle("Delete Club Event?")
                    .setMessage("Are you sure you want to delete '$cleanTitle'? This will remove the event and reopen the time slot.")
                    .setPositiveButton("Yes, Delete") { _, _ ->
                        dialog.dismiss()
                        executeDeleteClubEvent(unavail.id)
                    }
                    .setNegativeButton("Cancel", null)
                    .show()
            }
        }

        dialog.getButton(androidx.appcompat.app.AlertDialog.BUTTON_POSITIVE).setOnClickListener {
            val reason = inputReason.text.toString().trim()
            if (reason.isEmpty()) {
                MaterialAlertDialogBuilder(this)
                    .setTitle("⚠️ Event Title Required")
                    .setMessage("Please enter a title or description for the club event before saving.")
                    .setPositiveButton("OK", null)
                    .show()
                return@setOnClickListener
            }

            val selectedLocation = if (rbOffice.isChecked) "Office" else "Function Hall"
            val isFullDay: Boolean
            val startTime: String?
            val endTime: String?

            if (selectedLocation == "Office") {
                if (rbOfficeFullDay.isChecked) {
                    isFullDay = true
                    startTime = null
                    endTime = null
                } else {
                    if (officeEndMin <= officeStartMin) {
                        MaterialAlertDialogBuilder(this)
                            .setTitle("⚠️ Invalid Time Range")
                            .setMessage("End time must be after start time. Please select a valid time range.")
                            .setPositiveButton("OK", null)
                            .show()
                        return@setOnClickListener
                    }
                    isFullDay = false
                    startTime = formatMinutesTo12Hour(officeStartMin)
                    endTime = formatMinutesTo12Hour(officeEndMin)
                }
            } else {
                isFullDay = rbFullDay.isChecked
                startTime = when {
                    rbAfternoon.isChecked -> "12:00 PM"
                    rbEvening.isChecked -> "06:00 PM"
                    else -> null
                }
                endTime = when {
                    rbAfternoon.isChecked -> "05:00 PM"
                    rbEvening.isChecked -> "11:00 PM"
                    else -> null
                }
            }

            val startMin = parseTimeToMinutes(startTime)
            val endMin = parseTimeToMinutes(endTime)
            val conflict = checkSlotConflict(selectedDateString, selectedLocation, isFullDay, startMin, endMin, unavail, unavail.id)

            if (conflict != null) {
                MaterialAlertDialogBuilder(this)
                    .setTitle("⚠️ Time Conflict")
                    .setMessage("Cannot schedule event for this slot:\n\n• $conflict\n\nPlease select an available time window or switch location.")
                    .setPositiveButton("OK", null)
                    .show()
                return@setOnClickListener
            }

            val payload = UpdateClubEventPayload(
                date = selectedDateString,
                reason = reason,
                location = selectedLocation,
                startTime = startTime,
                endTime = endTime,
                isFullDay = isFullDay
            )
            dialog.dismiss()
            executeUpdateClubEvent(unavail.id, payload)
        }
    }

    private fun executeCreateClubEvent(payload: CreateClubEventPayload) {
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.createClubEvent(payload)
                withContext(Dispatchers.Main) {
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalCalendarActivity, "✅ Club event scheduled successfully!", Toast.LENGTH_LONG).show()
                        loadData()
                    } else {
                        val errMsg = try {
                            val errJson = response.errorBody()?.string()
                            if (!errJson.isNullOrBlank()) {
                                val obj = org.json.JSONObject(errJson)
                                obj.optString("error", obj.optString("message", "Failed to book club event."))
                            } else {
                                response.body()?.message ?: "Failed to book club event."
                            }
                        } catch (e: Exception) {
                            response.body()?.message ?: "Failed to book club event."
                        }
                        Toast.makeText(this@RentalCalendarActivity, "❌ $errMsg", Toast.LENGTH_LONG).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    Toast.makeText(this@RentalCalendarActivity, "❌ Network error: ${e.message}", Toast.LENGTH_LONG).show()
                }
            }
        }
    }

    private fun executeUpdateClubEvent(id: Int, payload: UpdateClubEventPayload) {
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.updateClubEvent(id, payload)
                withContext(Dispatchers.Main) {
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalCalendarActivity, "✅ Club event updated successfully!", Toast.LENGTH_LONG).show()
                        loadData()
                    } else {
                        val errMsg = try {
                            val errJson = response.errorBody()?.string()
                            if (!errJson.isNullOrBlank()) {
                                val obj = org.json.JSONObject(errJson)
                                obj.optString("error", obj.optString("message", "Failed to update club event."))
                            } else {
                                response.body()?.message ?: "Failed to update club event."
                            }
                        } catch (e: Exception) {
                            response.body()?.message ?: "Failed to update club event."
                        }
                        Toast.makeText(this@RentalCalendarActivity, "❌ $errMsg", Toast.LENGTH_LONG).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    Toast.makeText(this@RentalCalendarActivity, "❌ Network error: ${e.message}", Toast.LENGTH_LONG).show()
                }
            }
        }
    }

    private fun executeDeleteClubEvent(id: Int) {
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.deleteClubEvent(id)
                withContext(Dispatchers.Main) {
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@RentalCalendarActivity, "🗑️ Club event deleted successfully!", Toast.LENGTH_LONG).show()
                        loadData()
                    } else {
                        Toast.makeText(this@RentalCalendarActivity, "❌ Failed to delete club event.", Toast.LENGTH_LONG).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    Toast.makeText(this@RentalCalendarActivity, "❌ Network error: ${e.message}", Toast.LENGTH_LONG).show()
                }
            }
        }
    }
}

