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
import android.widget.EditText
import android.widget.RadioButton
import android.widget.RadioGroup
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import com.gfc.connect.data.models.CreateClubEventPayload
import com.gfc.connect.databinding.ActivityRentalCalendarBinding
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
        val hasClubEvent: Boolean
    )

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRentalCalendarBinding.inflate(layoutInflater)
        setContentView(binding.root)

        cacheManager = RentalCacheManager(this)

        setupToolbar()
        setupSwipeRefresh()
        setupCalendar()
        setupFab()
        loadData()
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
                it.eventDate.substringBefore('T') == dateStr
            }

            val matchingUnavailable = unavailableDatesList.filter { unavail ->
                unavail.date.substringBefore('T') == dateStr &&
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
                it.status.equals("Pending", ignoreCase = true) ||
                it.status.equals("Inquiry", ignoreCase = true)
            }

            val hasClubEvent = matchingUnavailable.isNotEmpty()

            days.add(
                CalendarDayModel(
                    dayNumber = day,
                    dateString = dateStr,
                    isCurrentMonth = true,
                    hasBooked = hasBooked,
                    hasPending = hasPending,
                    hasClubEvent = hasClubEvent
                )
            )
        }

        binding.recyclerCalendarGrid.adapter = CalendarDayAdapter(days)
    }

    private inner class CalendarDayAdapter(
        private val days: List<CalendarDayModel?>
    ) : RecyclerView.Adapter<CalendarDayAdapter.DayViewHolder>() {

        inner class DayViewHolder(val view: View) : RecyclerView.ViewHolder(view) {
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
                holder.dotEmerald.visibility = View.GONE
                holder.dotYellow.visibility = View.GONE
                holder.dotPurple.visibility = View.GONE
                holder.view.isClickable = false
                holder.view.setOnClickListener(null)
                return
            }

            holder.txtDay.text = model.dayNumber.toString()
            val isSelected = model.dateString == selectedDateString

            if (isSelected) {
                holder.txtDay.setBackgroundResource(R.drawable.bg_calendar_day_selected)
                holder.txtDay.setTextColor(getColor(R.color.bg_dark))
            } else {
                holder.txtDay.background = null
                holder.txtDay.setTextColor(getColor(R.color.text_primary))
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

    private fun loadData() {
        // 1. Instant Cache
        val cached = cacheManager.getRentals()
        if (cached.isNotEmpty()) {
            rentalsList.clear()
            rentalsList.addAll(cached)
            updateSummaryCounters()
            renderCalendarMonth()
            renderScheduleForSelectedDate()
        }

        // 2. Fresh Network Sync
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val rentalsResponse = ApiClient.service.getRentalsList()
                val unavailResponse = ApiClient.service.getUnavailableDates()

                withContext(Dispatchers.Main) {
                    binding.swipeRefreshCalendar.isRefreshing = false

                    if (rentalsResponse.isSuccessful && rentalsResponse.body() != null) {
                        rentalsList.clear()
                        rentalsList.addAll(rentalsResponse.body()!!)
                        cacheManager.saveRentals(rentalsList)
                    }

                    if (unavailResponse.isSuccessful && unavailResponse.body() != null) {
                        unavailableDatesList.clear()
                        unavailableDatesList.addAll(unavailResponse.body()!!)
                    }

                    updateSummaryCounters()
                    renderCalendarMonth()
                    renderScheduleForSelectedDate()
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    binding.swipeRefreshCalendar.isRefreshing = false
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
            (it.status.equals("Pending", ignoreCase = true) || it.status.equals("Inquiry", ignoreCase = true))
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
            val clean = timeStr.trim().uppercase()
            val isPm = clean.contains("PM")
            val isAm = clean.contains("AM")
            val timeOnly = clean.replace("AM", "").replace("PM", "").trim()
            val parts = timeOnly.split(":")
            var hours = parts[0].trim().toInt()
            val minutes = if (parts.size > 1) parts[1].trim().toInt() else 0
            if (isPm && hours < 12) hours += 12
            if (isAm && hours == 12) hours = 0
            hours * 60 + minutes
        } catch (e: Exception) {
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
            it.eventDate.substringBefore('T') == selectedDateString
        }

        val matchingUnavailable = unavailableDatesList.filter { unavail ->
            unavail.date.substringBefore('T') == selectedDateString &&
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

                        addView(titleTv)
                        addView(statusBadge)
                    }
                    addView(topRow)

                    // Event Details
                    val detailsTv = TextView(context).apply {
                        text = "🏛️ ${rental.roomSelected ?: "Function Hall"} • ${rental.eventType ?: "Rental"} • ${rental.guestCount} Guests"
                        setTextColor(getColor(R.color.text_secondary))
                        textSize = 12f
                        setPadding(0, dp8 / 2, 0, 0)
                    }
                    addView(detailsTv)

                    // Time & Quote
                    val timePriceTv = TextView(context).apply {
                        text = "⏰ ${rental.startTime ?: "2:00 PM"} - ${rental.endTime ?: "7:00 PM"}  •  Quote: ${currencyFormat.format(rental.totalPrice)}"
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

                val cardLayout = LinearLayout(context).apply {
                    orientation = LinearLayout.VERTICAL
                    setPadding(dp12, dp12, dp12, dp12)

                    val titleTv = TextView(context).apply {
                        val displayTitle = unavail.eventType ?: unavail.reason ?: "Club Event / Blackout"
                        text = "🟣 $displayTitle"
                        setTextColor(getColor(R.color.text_primary))
                        textSize = 14f
                        setTypeface(null, Typeface.BOLD)
                    }
                    addView(titleTv)

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

    private fun promptCreateClubEvent() {
        val dp8 = (8 * resources.displayMetrics.density).toInt()
        val dp16 = (16 * resources.displayMetrics.density).toInt()

        val layout = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp16, dp8, dp16, dp8)
        }

        val dateLabel = TextView(this).apply {
            text = "Event Date: $selectedDateString"
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
        }
        layout.addView(inputReason)

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

        val timeLabel = TextView(this).apply {
            text = "Time Slot / Duration:"
            setTextColor(getColor(R.color.text_secondary))
            textSize = 13f
            setPadding(0, dp8 * 2, 0, dp8 / 2)
        }
        layout.addView(timeLabel)

        val radioGroup = RadioGroup(this).apply {
            orientation = RadioGroup.VERTICAL
        }

        val rbFullDay = RadioButton(this).apply {
            id = View.generateViewId()
            text = "Full Day Blackout"
            setTextColor(getColor(R.color.text_primary))
            isChecked = true
        }
        val rbAfternoon = RadioButton(this).apply {
            id = View.generateViewId()
            text = "Afternoon (12:00 PM – 5:00 PM)"
            setTextColor(getColor(R.color.text_primary))
        }
        val rbEvening = RadioButton(this).apply {
            id = View.generateViewId()
            text = "Evening (6:00 PM – 11:00 PM)"
            setTextColor(getColor(R.color.text_primary))
        }

        radioGroup.addView(rbFullDay)
        radioGroup.addView(rbAfternoon)
        radioGroup.addView(rbEvening)
        layout.addView(radioGroup)

        MaterialAlertDialogBuilder(this)
            .setTitle("Book Club Event / Blackout")
            .setMessage("Block out dates/times for club functions directly on the calendar:")
            .setView(layout)
            .setPositiveButton("Book Event") { _, _ ->
                val reason = inputReason.text.toString().trim()
                if (reason.isEmpty()) {
                    Toast.makeText(this, "Please enter an event title or reason", Toast.LENGTH_SHORT).show()
                    return@setPositiveButton
                }

                val selectedLocation = if (rbOffice.isChecked) "Office" else "Function Hall"
                val isFullDay = rbFullDay.isChecked
                val startTime = when {
                    rbAfternoon.isChecked -> "12:00 PM"
                    rbEvening.isChecked -> "06:00 PM"
                    else -> null
                }
                val endTime = when {
                    rbAfternoon.isChecked -> "05:00 PM"
                    rbEvening.isChecked -> "11:00 PM"
                    else -> null
                }

                val payload = CreateClubEventPayload(
                    date = selectedDateString,
                    reason = reason,
                    location = selectedLocation,
                    startTime = startTime,
                    endTime = endTime,
                    isFullDay = isFullDay
                )
                executeCreateClubEvent(payload)
            }
            .setNegativeButton("Cancel", null)
            .show()
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
                        val msg = response.body()?.message ?: "Failed to book club event"
                        Toast.makeText(this@RentalCalendarActivity, "❌ $msg", Toast.LENGTH_LONG).show()
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

