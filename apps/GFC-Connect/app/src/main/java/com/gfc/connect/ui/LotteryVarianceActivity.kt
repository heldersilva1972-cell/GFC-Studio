package com.gfc.connect.ui

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.gfc.connect.GfcConnectApp
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.data.models.LotteryVarianceDto
import com.gfc.connect.databinding.ActivityLotteryVarianceBinding
import com.gfc.connect.databinding.ItemBartenderGroupBinding
import com.gfc.connect.databinding.ItemLotteryVarianceBinding
import com.gfc.connect.databinding.ItemMonthGroupBinding
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.tabs.TabLayout
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.text.NumberFormat
import java.text.SimpleDateFormat
import java.util.*

class LotteryVarianceActivity : AppCompatActivity() {

    private lateinit var binding: ActivityLotteryVarianceBinding
    private val activeVarianceList = mutableListOf<LotteryVarianceDto>()
    private val monthGroupList = mutableListOf<MonthGroup>()
    private lateinit var activeAdapter: LotteryVarianceAdapter
    private lateinit var historyMonthAdapter: MonthGroupAdapter
    private var currentTabIndex = 0 // 0 = Active Alerts, 1 = Shift History

    // -------------------------------------------------------------------------
    // Tiered Data Structures for Shift History: Month -> Bartender -> Shifts
    // -------------------------------------------------------------------------
    data class BartenderGroup(
        val employeeName: String,
        val totalShifts: Int,
        val netVariance: Double,
        val positiveVariance: Double,
        val negativeVariance: Double,
        val shifts: List<LotteryVarianceDto>,
        var isExpanded: Boolean = false
    )

    data class MonthGroup(
        val monthKey: String, // e.g. "2026-10"
        val monthTitle: String, // e.g. "October 2026"
        val totalShifts: Int,
        val netVariance: Double,
        val positiveVariance: Double,
        val negativeVariance: Double,
        val bartenders: List<BartenderGroup>,
        var isExpanded: Boolean = false
    )

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLotteryVarianceBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setupToolbar()
        setupTabs()
        setupRecyclerViews()
        setupSwipeRefresh()

        loadData()
    }

    private fun setupToolbar() {
        binding.toolbarLottery.setNavigationOnClickListener {
            finish()
        }
    }

    private fun setupTabs() {
        binding.tabLayoutLottery.addTab(binding.tabLayoutLottery.newTab().setText("⚠️ Active Alerts"))
        binding.tabLayoutLottery.addTab(binding.tabLayoutLottery.newTab().setText("📊 Shift History"))

        binding.tabLayoutLottery.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                currentTabIndex = tab?.position ?: 0
                binding.recyclerLotteryVariances.adapter = if (currentTabIndex == 0) activeAdapter else historyMonthAdapter
                loadData()
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })
    }

    private fun setupRecyclerViews() {
        // Requirement 4: In active alerts, cards start collapsed (not opened in detail view)
        activeAdapter = LotteryVarianceAdapter(
            items = activeVarianceList,
            isHistoryMode = { false },
            onAcknowledge = { item -> promptAcknowledge(item) },
            startExpanded = false
        )

        historyMonthAdapter = MonthGroupAdapter(
            groups = monthGroupList,
            onAcknowledge = { item -> promptAcknowledge(item) }
        )

        binding.recyclerLotteryVariances.layoutManager = LinearLayoutManager(this)
        binding.recyclerLotteryVariances.adapter = activeAdapter
    }

    private fun setupSwipeRefresh() {
        binding.swipeRefreshLottery.setColorSchemeResources(R.color.status_yellow, R.color.emerald_accent)
        binding.swipeRefreshLottery.setProgressBackgroundColorSchemeResource(R.color.surface_dark)
        binding.swipeRefreshLottery.setOnRefreshListener {
            loadData()
        }
    }

    private fun loadData() {
        val appSettings = GfcConnectApp.instance.appSettings
        val threshold = appSettings.lotteryVarianceThreshold

        val hasExistingItems = if (currentTabIndex == 0) activeVarianceList.isNotEmpty() else monthGroupList.isNotEmpty()
        binding.progressLottery.visibility = if (!hasExistingItems) View.VISIBLE else View.GONE
        binding.layoutEmptyState.visibility = View.GONE

        lifecycleScope.launch(Dispatchers.IO) {
            val cacheKey = if (currentTabIndex == 0) "cache_lottery_active" else "cache_lottery_history"

            // 1. Load from local cache immediately (offline-safe)
            val cachedJson = appSettings.getCachedLotteryJson(cacheKey)
            if (!cachedJson.isNullOrBlank()) {
                try {
                    val type = object : TypeToken<List<LotteryVarianceDto>>() {}.type
                    val cachedList: List<LotteryVarianceDto> = Gson().fromJson(cachedJson, type)
                    withContext(Dispatchers.Main) {
                        applyShiftData(cachedList)
                    }
                } catch (_: Exception) {}
            }

            // 2. Fetch fresh updates from server
            try {
                val response = if (currentTabIndex == 0) {
                    ApiClient.service.getActiveLotteryVariances(threshold)
                } else {
                    ApiClient.service.getLotteryVarianceHistory(threshold)
                }

                if (response.isSuccessful && response.body() != null) {
                    val fetched = response.body()!!
                    appSettings.saveCachedLotteryJson(cacheKey, Gson().toJson(fetched))

                    withContext(Dispatchers.Main) {
                        binding.progressLottery.visibility = View.GONE
                        binding.swipeRefreshLottery.isRefreshing = false
                        applyShiftData(fetched)
                    }
                } else {
                    withContext(Dispatchers.Main) {
                        binding.progressLottery.visibility = View.GONE
                        binding.swipeRefreshLottery.isRefreshing = false
                    }
                }
            } catch (ex: Exception) {
                withContext(Dispatchers.Main) {
                    binding.progressLottery.visibility = View.GONE
                    binding.swipeRefreshLottery.isRefreshing = false
                }
            }
        }
    }

    private fun applyShiftData(allShifts: List<LotteryVarianceDto>) {
        val appSettings = GfcConnectApp.instance.appSettings

        if (currentTabIndex == 0) {
            // Active Tab: shifts NOT yet dismissed locally by this user
            val unacknowledged = allShifts.filter { !appSettings.isLotteryShiftDismissed(it.shiftId) }
            activeVarianceList.clear()
            activeVarianceList.addAll(unacknowledged)
            activeAdapter.notifyDataSetChanged()

            if (activeVarianceList.isEmpty()) {
                binding.layoutEmptyState.visibility = View.VISIBLE
                binding.txtEmptyTitle.text = "All Shifts Reconciled"
                binding.txtEmptySubtitle.text = "No active lottery shifts require review."
            } else {
                binding.layoutEmptyState.visibility = View.GONE
            }
        } else {
            // Requirement 3: History Tab ONLY displays shifts that have been acknowledged & dismissed locally
            val historyShifts = allShifts.filter { appSettings.isLotteryShiftDismissed(it.shiftId) }

            monthGroupList.clear()
            val monthParser = SimpleDateFormat("yyyy-MM", Locale.US)
            val monthTitleFormat = SimpleDateFormat("MMMM yyyy", Locale.US)

            val monthGroups = historyShifts.groupBy { shift ->
                try {
                    val dateStr = shift.shiftDate
                    if (dateStr.length >= 7) dateStr.substring(0, 7) else "Unknown"
                } catch (_: Exception) {
                    "Unknown"
                }
            }

            monthGroups.forEach { (monthKey, monthShifts) ->
                val monthTitle = try {
                    val date = monthParser.parse(monthKey)
                    if (date != null) monthTitleFormat.format(date) else monthKey
                } catch (_: Exception) {
                    monthKey
                }

                // Group by Bartender inside Month
                val bartenderMap = monthShifts.groupBy { it.employeeName }
                val bartenderGroups = bartenderMap.map { (name, bShifts) ->
                    val bNet = bShifts.sumOf { it.variance }
                    val bPos = bShifts.filter { it.variance > 0 }.sumOf { it.variance }
                    val bNeg = bShifts.filter { it.variance < 0 }.sumOf { it.variance }

                    BartenderGroup(
                        employeeName = name,
                        totalShifts = bShifts.size,
                        netVariance = bNet,
                        positiveVariance = bPos,
                        negativeVariance = bNeg,
                        shifts = bShifts.sortedByDescending { it.shiftDate },
                        isExpanded = false
                    )
                }.sortedBy { it.employeeName }

                val totalMonthShifts = monthShifts.size
                val totalMonthNet = monthShifts.sumOf { it.variance }
                val totalMonthPos = monthShifts.filter { it.variance > 0 }.sumOf { it.variance }
                val totalMonthNeg = monthShifts.filter { it.variance < 0 }.sumOf { it.variance }

                monthGroupList.add(
                    MonthGroup(
                        monthKey = monthKey,
                        monthTitle = monthTitle,
                        totalShifts = totalMonthShifts,
                        netVariance = totalMonthNet,
                        positiveVariance = totalMonthPos,
                        negativeVariance = totalMonthNeg,
                        bartenders = bartenderGroups,
                        isExpanded = false
                    )
                )
            }

            // Sort months descending (newest month first)
            monthGroupList.sortByDescending { it.monthKey }

            historyMonthAdapter.notifyDataSetChanged()

            if (monthGroupList.isEmpty()) {
                binding.layoutEmptyState.visibility = View.VISIBLE
                binding.txtEmptyTitle.text = "No History Found"
                binding.txtEmptySubtitle.text = "No past shifts have been acknowledged and dismissed yet."
            } else {
                binding.layoutEmptyState.visibility = View.GONE
            }
        }
    }

    private fun promptAcknowledge(item: LotteryVarianceDto) {
        MaterialAlertDialogBuilder(this)
            .setTitle("Acknowledge & Dismiss")
            .setMessage("Clear this ${formatCurrency(item.variance)} variance for ${item.employeeName} on ${item.shiftDate} from your active alerts?")
            .setPositiveButton("Dismiss") { _, _ ->
                executeAcknowledge(item)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeAcknowledge(item: LotteryVarianceDto) {
        GfcConnectApp.instance.appSettings.dismissLotteryShift(item.shiftId)
        Toast.makeText(this@LotteryVarianceActivity, "Variance dismissed from active alerts", Toast.LENGTH_SHORT).show()

        val idx = activeVarianceList.indexOfFirst { it.shiftId == item.shiftId }
        if (idx != -1) {
            activeVarianceList.removeAt(idx)
            activeAdapter.notifyItemRemoved(idx)
            if (activeVarianceList.isEmpty()) {
                binding.layoutEmptyState.visibility = View.VISIBLE
                binding.txtEmptyTitle.text = "All Shifts Reconciled"
                binding.txtEmptySubtitle.text = "No active lottery shifts require review."
            }
        }

        val memberName = GfcConnectApp.instance.tokenStorage.getMemberName() ?: "Manager"
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                ApiClient.service.acknowledgeLotteryVariance(
                    item.shiftId,
                    com.gfc.connect.data.models.AcknowledgeVariancePayload(item.shiftId, memberName)
                )
            } catch (_: Exception) {}
        }
    }

    private fun formatCurrency(amount: Double): String {
        val format = NumberFormat.getCurrencyInstance(Locale.US)
        return format.format(amount)
    }

    // =========================================================================
    // LEVEL 1: Month Group Adapter (e.g. June 2026)
    // Single Accordion: expanding one month collapses previous month
    // =========================================================================
    inner class MonthGroupAdapter(
        private val groups: List<MonthGroup>,
        private val onAcknowledge: (LotteryVarianceDto) -> Unit
    ) : RecyclerView.Adapter<MonthGroupAdapter.MonthViewHolder>() {

        private var expandedMonthIndex: Int = -1

        inner class MonthViewHolder(val binding: ItemMonthGroupBinding) : RecyclerView.ViewHolder(binding.root)

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MonthViewHolder {
            val b = ItemMonthGroupBinding.inflate(LayoutInflater.from(parent.context), parent, false)
            return MonthViewHolder(b)
        }

        override fun onBindViewHolder(holder: MonthViewHolder, position: Int) {
            val group = groups[position]
            val b = holder.binding
            val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)

            b.txtMonthTitle.text = group.monthTitle
            b.txtMonthStats.text = "${group.totalShifts} shift${if (group.totalShifts > 1) "s" else ""}"

            // Requirement 5: Display positive and negative totals separately
            if (group.positiveVariance > 0.005) {
                b.txtMonthPositiveBadge.visibility = View.VISIBLE
                b.txtMonthPositiveBadge.text = "+${currencyFormat.format(group.positiveVariance)}"
            } else {
                b.txtMonthPositiveBadge.visibility = View.GONE
            }

            if (group.negativeVariance < -0.005) {
                b.txtMonthNegativeBadge.visibility = View.VISIBLE
                b.txtMonthNegativeBadge.text = "-${currencyFormat.format(Math.abs(group.negativeVariance))}"
            } else {
                b.txtMonthNegativeBadge.visibility = View.GONE
            }

            val isExpanded = (position == expandedMonthIndex)
            group.isExpanded = isExpanded

            val bartenderAdapter = BartenderGroupAdapter(
                groups = group.bartenders,
                onAcknowledge = onAcknowledge
            )
            b.recyclerMonthBartenders.layoutManager = LinearLayoutManager(holder.itemView.context)
            b.recyclerMonthBartenders.adapter = bartenderAdapter

            b.recyclerMonthBartenders.visibility = if (isExpanded) View.VISIBLE else View.GONE
            b.dividerMonth.visibility = if (isExpanded) View.VISIBLE else View.GONE
            b.imgMonthChevron.rotation = if (isExpanded) 90f else 0f

            // Requirement 2: Expanding a month collapses the previously expanded month
            b.cardMonthGroup.setOnClickListener {
                val currentPos = holder.adapterPosition
                if (currentPos == RecyclerView.NO_POSITION) return@setOnClickListener

                if (expandedMonthIndex == currentPos) {
                    expandedMonthIndex = -1
                    notifyItemChanged(currentPos)
                } else {
                    val prev = expandedMonthIndex
                    expandedMonthIndex = currentPos
                    if (prev != -1) notifyItemChanged(prev)
                    notifyItemChanged(currentPos)
                }
            }
        }

        override fun getItemCount(): Int = groups.size
    }

    // =========================================================================
    // LEVEL 2: Bartender Sub-Group Adapter (Inside Month)
    // Single Accordion: expanding one bartender collapses previous bartender
    // =========================================================================
    inner class BartenderGroupAdapter(
        private val groups: List<BartenderGroup>,
        private val onAcknowledge: (LotteryVarianceDto) -> Unit
    ) : RecyclerView.Adapter<BartenderGroupAdapter.BartenderViewHolder>() {

        private var expandedBartenderIndex: Int = -1

        inner class BartenderViewHolder(val binding: ItemBartenderGroupBinding) : RecyclerView.ViewHolder(binding.root)

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): BartenderViewHolder {
            val b = ItemBartenderGroupBinding.inflate(LayoutInflater.from(parent.context), parent, false)
            return BartenderViewHolder(b)
        }

        override fun onBindViewHolder(holder: BartenderViewHolder, position: Int) {
            val group = groups[position]
            val b = holder.binding
            val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)

            b.txtBartenderName.text = group.employeeName
            b.txtBartenderInitial.text = if (group.employeeName.isNotEmpty()) group.employeeName.first().uppercase() else "B"
            b.txtBartenderStats.text = "${group.totalShifts} shift${if (group.totalShifts > 1) "s" else ""}"

            // Requirement 5: Display positive and negative totals separately for bartender
            if (group.positiveVariance > 0.005) {
                b.txtBartenderPositiveBadge.visibility = View.VISIBLE
                b.txtBartenderPositiveBadge.text = "+${currencyFormat.format(group.positiveVariance)}"
            } else {
                b.txtBartenderPositiveBadge.visibility = View.GONE
            }

            if (group.negativeVariance < -0.005) {
                b.txtBartenderNegativeBadge.visibility = View.VISIBLE
                b.txtBartenderNegativeBadge.text = "-${currencyFormat.format(Math.abs(group.negativeVariance))}"
            } else {
                b.txtBartenderNegativeBadge.visibility = View.GONE
            }

            val isExpanded = (position == expandedBartenderIndex)
            group.isExpanded = isExpanded

            val shiftAdapter = LotteryVarianceAdapter(
                items = group.shifts,
                isHistoryMode = { true },
                onAcknowledge = onAcknowledge,
                startExpanded = false
            )
            b.recyclerBartenderShifts.layoutManager = LinearLayoutManager(holder.itemView.context)
            b.recyclerBartenderShifts.adapter = shiftAdapter

            b.recyclerBartenderShifts.visibility = if (isExpanded) View.VISIBLE else View.GONE
            b.dividerBartender.visibility = if (isExpanded) View.VISIBLE else View.GONE
            b.imgGroupChevron.rotation = if (isExpanded) 90f else 0f

            // Requirement 2: Expanding a bartender collapses the previously expanded bartender
            b.cardBartenderGroup.setOnClickListener {
                val currentPos = holder.adapterPosition
                if (currentPos == RecyclerView.NO_POSITION) return@setOnClickListener

                if (expandedBartenderIndex == currentPos) {
                    expandedBartenderIndex = -1
                    notifyItemChanged(currentPos)
                } else {
                    val prev = expandedBartenderIndex
                    expandedBartenderIndex = currentPos
                    if (prev != -1) notifyItemChanged(prev)
                    notifyItemChanged(currentPos)
                }
            }
        }

        override fun getItemCount(): Int = groups.size
    }

    // =========================================================================
    // LEVEL 3: Individual Shift Variance Card (Expandable Full Breakdown)
    // =========================================================================
    inner class LotteryVarianceAdapter(
        private val items: List<LotteryVarianceDto>,
        private val isHistoryMode: () -> Boolean,
        private val onAcknowledge: (LotteryVarianceDto) -> Unit,
        private val startExpanded: Boolean = false
    ) : RecyclerView.Adapter<LotteryVarianceAdapter.ViewHolder>() {

        private val expandedStates = mutableMapOf<Int, Boolean>()

        inner class ViewHolder(val binding: ItemLotteryVarianceBinding) : RecyclerView.ViewHolder(binding.root)

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
            val itemBinding = ItemLotteryVarianceBinding.inflate(LayoutInflater.from(parent.context), parent, false)
            return ViewHolder(itemBinding)
        }

        override fun onBindViewHolder(holder: ViewHolder, position: Int) {
            val item = items[position]
            val b = holder.binding
            val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)

            b.txtShiftEmployee.text = item.employeeName
            b.txtShiftDateAndType.text = "${formatDisplayDate(item.shiftDate)} • ${item.shiftType ?: "Shift"}"

            val isShort = item.variance < 0
            val varianceStr = (if (isShort) "-" else "+") + currencyFormat.format(Math.abs(item.variance))
            b.txtVarianceAmountBadge.text = varianceStr
            b.txtVarianceAmountBadge.setTextColor(
                if (isShort) getColor(R.color.status_red) else getColor(R.color.emerald_accent)
            )

            // Visual Status Stripe on the Left Edge
            b.viewShiftStripe.setBackgroundResource(
                if (isShort) R.drawable.bg_stripe_red else R.drawable.bg_stripe_emerald
            )

            // Primary Overview (Expected Cash vs Actual Ending Cash)
            b.txtExpectedCash.text = currencyFormat.format(item.expectedCash)
            b.txtEndingCash.text = currencyFormat.format(item.endingCash)

            // Requirement 5: Bartender history shows both positive and negative totals separately
            val posStr = "+${currencyFormat.format(item.bartenderPositiveVariance)}"
            val negStr = "-${currencyFormat.format(Math.abs(item.bartenderNegativeVariance))}"
            b.txtBartenderLifetimeSummary.text = "Bartender History (${item.bartenderTotalShifts} shifts): Over $posStr • Short $negStr"

            // Full Shift Entry Breakdown
            b.txtDetailStartingCash.text = currencyFormat.format(item.startingCash)
            b.txtDetailBagAdded.text = currencyFormat.format(item.backupBagAmount)
            b.txtDetailBagRefill.text = currencyFormat.format(item.bagRefillAmount)

            b.txtDetailTotalSales.text = currencyFormat.format(item.totalSales)
            b.txtDetailTotalPayouts.text = currencyFormat.format(item.totalPayouts)
            b.txtDetailTotalCancels.text = currencyFormat.format(item.totalCancels)

            b.txtDetailShiftType.text = if (!item.shiftType.isNullOrBlank()) "${item.shiftType} Shift" else "Shift"
            b.txtDetailEnvelope.text = currencyFormat.format(item.envelopeAmount)
            b.txtDetailStatus.text = "${item.status ?: "Submitted"}${if (!item.machineId.isNullOrBlank()) " • M#${item.machineId}" else ""}"

            if (!item.notes.isNullOrBlank()) {
                b.layoutDetailNotes.visibility = View.VISIBLE
                b.txtDetailNotes.text = item.notes
            } else {
                b.layoutDetailNotes.visibility = View.GONE
            }

            // Requirement 4: Start collapsed so user can tap to expand details
            val isExpanded = expandedStates[item.shiftId] ?: startExpanded
            b.layoutCompleteDetails.visibility = if (isExpanded) View.VISIBLE else View.GONE
            b.imgExpandChevron.rotation = if (isExpanded) 90f else 0f

            b.cardShiftItem.setOnClickListener {
                val nextState = !(expandedStates[item.shiftId] ?: startExpanded)
                expandedStates[item.shiftId] = nextState
                b.layoutCompleteDetails.visibility = if (nextState) View.VISIBLE else View.GONE
                b.imgExpandChevron.rotation = if (nextState) 90f else 0f
            }

            // Acknowledgment Button (Active Alerts tab only)
            val isDismissedLocally = GfcConnectApp.instance.appSettings.isLotteryShiftDismissed(item.shiftId)
            if (isHistoryMode() || isDismissedLocally) {
                b.btnAcknowledgeShift.visibility = View.GONE
                b.txtAcknowledgedInfo.visibility = View.VISIBLE
                b.txtAcknowledgedInfo.text = "✓ Dismissed"
            } else {
                b.btnAcknowledgeShift.visibility = View.VISIBLE
                b.txtAcknowledgedInfo.visibility = View.GONE
                b.btnAcknowledgeShift.setOnClickListener {
                    onAcknowledge(item)
                }
            }
        }

        override fun getItemCount(): Int = items.size

        private fun formatDisplayDate(isoDate: String): String {
            return try {
                val parser = SimpleDateFormat("yyyy-MM-dd", Locale.US)
                val formatter = SimpleDateFormat("MMM d, yyyy", Locale.US)
                val d = parser.parse(isoDate)
                if (d != null) formatter.format(d) else isoDate
            } catch (e: Exception) {
                isoDate
            }
        }
    }
}
