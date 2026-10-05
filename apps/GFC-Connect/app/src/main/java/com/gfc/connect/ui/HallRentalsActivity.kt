package com.gfc.connect.ui

import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.gfc.connect.R
import com.gfc.connect.api.ApiClient
import com.gfc.connect.api.NetworkMonitor
import com.gfc.connect.data.cache.RentalCacheManager
import com.gfc.connect.data.models.ApprovalActionRequest
import com.gfc.connect.data.models.HallRentalDto
import com.gfc.connect.databinding.ActivityHallRentalsBinding
import com.gfc.connect.databinding.ItemHallRentalBinding
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.text.NumberFormat
import java.text.SimpleDateFormat
import java.util.*

class HallRentalsActivity : AppCompatActivity() {

    private lateinit var binding: ActivityHallRentalsBinding
    private lateinit var cacheManager: RentalCacheManager
    private lateinit var networkMonitor: NetworkMonitor

    private val allRentals = mutableListOf<HallRentalDto>()
    private val filteredRentals = mutableListOf<HallRentalDto>()
    private lateinit var adapter: HallRentalsAdapter
    private var currentFilterIndex: Int = 0
    private var isOfflineMode: Boolean = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityHallRentalsBinding.inflate(layoutInflater)
        setContentView(binding.root)

        cacheManager = RentalCacheManager(this)
        networkMonitor = NetworkMonitor(this)

        setupToolbar()
        setupRecyclerView()
        setupStatusTabs()
        setupSwipeRefresh()
        setupOfflineListeners()

        // 1. Instant Cache Load (Zero-latency offline rendering)
        loadCachedData()

        // 2. Fresh Network Sync
        loadRentals(silent = allRentals.isNotEmpty())

        // 3. Observe real-time network reconnects
        observeNetwork()
    }

    override fun onResume() {
        super.onResume()
        // Always attempt silent background refresh on screen return
        loadRentals(silent = true)
    }

    private fun setupToolbar() {
        binding.toolbarRentals.setNavigationOnClickListener {
            finish()
        }
    }

    private fun setupRecyclerView() {
        adapter = HallRentalsAdapter(filteredRentals)
        binding.recyclerRentals.layoutManager = LinearLayoutManager(this)
        binding.recyclerRentals.adapter = adapter
    }

    private fun setupSwipeRefresh() {
        binding.swipeRefreshRentals.setColorSchemeResources(R.color.cyan_accent, R.color.blue_primary)
        binding.swipeRefreshRentals.setProgressBackgroundColorSchemeResource(R.color.surface_dark)
        binding.swipeRefreshRentals.setOnRefreshListener {
            loadRentals(isManual = true)
        }
    }

    private fun setupOfflineListeners() {
        binding.btnBannerReconnect.setOnClickListener {
            loadRentals(isManual = true)
        }

        binding.btnRetryLoad.setOnClickListener {
            loadRentals(isManual = true)
        }
    }

    private fun setupStatusTabs() {
        val tabLayout = binding.tabLayoutStatus
        tabLayout.removeAllTabs()
        tabLayout.addTab(tabLayout.newTab().setText("All Bookings"))
        tabLayout.addTab(tabLayout.newTab().setText("Pending Review"))
        tabLayout.addTab(tabLayout.newTab().setText("Approved"))
        tabLayout.addTab(tabLayout.newTab().setText("Completed"))

        tabLayout.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                currentFilterIndex = tab?.position ?: 0
                applyFilter()

                // If currently empty or offline, proactively re-attempt network query on tab change
                if (allRentals.isEmpty() || isOfflineMode) {
                    loadRentals(silent = true)
                }
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {
                // Tapping active tab forces a fresh refresh
                loadRentals(silent = false)
            }
        })
    }

    private fun loadCachedData() {
        val cached = cacheManager.getRentals()
        if (cached.isNotEmpty()) {
            allRentals.clear()
            allRentals.addAll(cached)
            applyFilter()
        }
    }

    private fun observeNetwork() {
        lifecycleScope.launch {
            networkMonitor.observeNetworkState().collect { online ->
                if (online && isOfflineMode) {
                    // Automatically re-fetch data when internet/server connectivity returns
                    loadRentals(silent = true)
                }
            }
        }
    }

    private fun applyFilter() {
        filteredRentals.clear()
        when (currentFilterIndex) {
            1 -> filteredRentals.addAll(allRentals.filter { it.status.equals("Pending", ignoreCase = true) })
            2 -> filteredRentals.addAll(allRentals.filter { 
                it.status.equals("Approved", ignoreCase = true) || it.status.equals("Confirmed", ignoreCase = true) || it.status.contains("Deposit", ignoreCase = true)
            })
            3 -> filteredRentals.addAll(allRentals.filter { it.status.equals("Completed", ignoreCase = true) })
            else -> filteredRentals.addAll(allRentals)
        }

        adapter.notifyDataSetChanged()

        val isEmpty = filteredRentals.isEmpty()
        binding.layoutEmptyRentals.visibility = if (isEmpty) View.VISIBLE else View.GONE
        
        if (isEmpty) {
            if (isOfflineMode && allRentals.isEmpty()) {
                binding.txtEmptyFilterTitle.text = "Server Offline"
                binding.txtEmptyFilterSubtitle.text = "Could not connect to GFC Server. Check connection and retry."
            } else {
                binding.txtEmptyFilterTitle.text = "No Bookings Found"
                binding.txtEmptyFilterSubtitle.text = "There are no rentals matching this status filter."
            }
        }
    }

    private fun loadRentals(silent: Boolean = false, isManual: Boolean = false) {
        if (!silent && !binding.swipeRefreshRentals.isRefreshing) {
            binding.progressRentals.visibility = View.VISIBLE
        }

        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.getRentalsList()
                withContext(Dispatchers.Main) {
                    binding.progressRentals.visibility = View.GONE
                    binding.swipeRefreshRentals.isRefreshing = false

                    if (response.isSuccessful && response.body() != null) {
                        isOfflineMode = false
                        binding.bannerOffline.visibility = View.GONE

                        val remoteRentals = response.body()!!
                        allRentals.clear()
                        allRentals.addAll(remoteRentals)

                        // Save to local offline cache
                        cacheManager.saveRentals(remoteRentals)

                        applyFilter()

                        if (isManual) {
                            Toast.makeText(this@HallRentalsActivity, "✅ Updated from server.", Toast.LENGTH_SHORT).show()
                        }
                    } else {
                        handleFetchFailure(isManual, "Server returned error ${response.code()}")
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    binding.progressRentals.visibility = View.GONE
                    binding.swipeRefreshRentals.isRefreshing = false
                    handleFetchFailure(isManual, e.localizedMessage ?: "Connection error")
                }
            }
        }
    }

    private fun handleFetchFailure(isManual: Boolean, reason: String) {
        isOfflineMode = true

        // If we have cached records on disk, show offline banner and keep cached data on screen
        if (allRentals.isNotEmpty()) {
            binding.bannerOffline.visibility = View.VISIBLE
            binding.txtOfflineBanner.text = "⚠️ Offline • Showing cached bookings"
            if (isManual) {
                Toast.makeText(this, "Could not reach server. Showing cached data.", Toast.LENGTH_SHORT).show()
            }
        } else {
            binding.bannerOffline.visibility = View.GONE
            applyFilter()
            if (isManual) {
                Toast.makeText(this, "Network error: $reason", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun promptApprove(item: HallRentalDto) {
        val input = EditText(this).apply {
            hint = "Optional admin note..."
            setPadding(40, 24, 40, 24)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle("✅ Approve Rental Request")
            .setMessage("Approve booking for ${item.applicantName} on ${item.eventDate.substringBefore('T')}?")
            .setView(input)
            .setPositiveButton("Approve") { _, _ ->
                val note = input.text.toString().trim()
                executeApproval(item.id, note)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeApproval(id: Int, note: String) {
        binding.progressRentals.visibility = View.VISIBLE
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.approveRental(id, ApprovalActionRequest(note))
                withContext(Dispatchers.Main) {
                    binding.progressRentals.visibility = View.GONE
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@HallRentalsActivity, "✅ Rental request approved!", Toast.LENGTH_SHORT).show()
                        loadRentals(silent = false)
                    } else {
                        Toast.makeText(this@HallRentalsActivity, "Failed to approve rental.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    binding.progressRentals.visibility = View.GONE
                    Toast.makeText(this@HallRentalsActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun promptDeny(item: HallRentalDto) {
        val input = EditText(this).apply {
            hint = "Reason for denial..."
            setPadding(40, 24, 40, 24)
        }

        MaterialAlertDialogBuilder(this)
            .setTitle("❌ Deny Rental Request")
            .setMessage("Are you sure you want to deny this request from ${item.applicantName}?")
            .setView(input)
            .setPositiveButton("Deny Request") { _, _ ->
                val reason = input.text.toString().trim()
                executeDenial(item.id, reason)
            }
            .setNegativeButton("Cancel", null)
            .show()
    }

    private fun executeDenial(id: Int, reason: String) {
        binding.progressRentals.visibility = View.VISIBLE
        lifecycleScope.launch(Dispatchers.IO) {
            try {
                val response = ApiClient.service.denyRental(id, ApprovalActionRequest(reason))
                withContext(Dispatchers.Main) {
                    binding.progressRentals.visibility = View.GONE
                    if (response.isSuccessful && response.body()?.success == true) {
                        Toast.makeText(this@HallRentalsActivity, "Rental request denied.", Toast.LENGTH_SHORT).show()
                        loadRentals(silent = false)
                    } else {
                        Toast.makeText(this@HallRentalsActivity, "Failed to deny rental.", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    binding.progressRentals.visibility = View.GONE
                    Toast.makeText(this@HallRentalsActivity, "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    inner class HallRentalsAdapter(private val items: List<HallRentalDto>) : RecyclerView.Adapter<HallRentalsAdapter.ViewHolder>() {

        inner class ViewHolder(val itemBinding: ItemHallRentalBinding) : RecyclerView.ViewHolder(itemBinding.root)

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
            val itemBinding = ItemHallRentalBinding.inflate(LayoutInflater.from(parent.context), parent, false)
            return ViewHolder(itemBinding)
        }

        override fun onBindViewHolder(holder: ViewHolder, position: Int) {
            val item = items[position]
            with(holder.itemBinding) {
                txtItemEventType.text = item.eventType ?: "Hall Rental"
                txtItemStatusBadge.text = item.status

                // Date Formatting
                val dateStr = try {
                    val inFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.US)
                    val outFormat = SimpleDateFormat("EEE, MMM d, yyyy", Locale.US)
                    val d = inFormat.parse(item.eventDate)
                    if (d != null) outFormat.format(d) else item.eventDate.substringBefore('T')
                } catch (e: Exception) {
                    item.eventDate.substringBefore('T')
                }

                txtItemDateRange.text = "📅 $dateStr • ${item.startTime ?: "2:00 PM"} - ${item.endTime ?: "7:00 PM"}"
                val barText = if (item.bartenderRequested) " • Bar Included" else ""
                txtItemRoomGuests.text = "🏛️ ${item.roomSelected ?: "Function Hall"} • ${item.guestCount} Guests$barText"
                txtItemApplicant.text = "Renter: ${item.applicantName}"

                // Matrix Selected Badge
                val matrix = item.matrixSelected ?: if (item.isVerifiedMember) "Member" else "Non-Member"
                txtItemMatrixBadge.text = "🏷️ Matrix: $matrix"

                // Member Verification Badge
                if (item.isVerifiedMember) {
                    val idText = if (item.verifiedMemberId != null && item.verifiedMemberId > 0) " #${item.verifiedMemberId}" else ""
                    txtItemMemberVerifyBadge.text = "✓ Verified Member$idText"
                    txtItemMemberVerifyBadge.setTextColor(getColor(R.color.emerald_accent))
                    txtItemMemberVerifyBadge.setBackgroundResource(R.drawable.bg_badge_emerald)
                    txtItemMemberVerifyBadge.visibility = View.VISIBLE
                } else if (item.memberVerificationBadge == "UNVERIFIED_CLAIM") {
                    txtItemMemberVerifyBadge.text = "⚠️ Claimed (Not in Dir)"
                    txtItemMemberVerifyBadge.setTextColor(getColor(R.color.status_yellow))
                    txtItemMemberVerifyBadge.setBackgroundResource(R.drawable.bg_pill_sync)
                    txtItemMemberVerifyBadge.visibility = View.VISIBLE
                } else {
                    // Standard non-member booking: hide duplicate badge
                    txtItemMemberVerifyBadge.visibility = View.GONE
                }

                val contactDetails = StringBuilder()
                if (!item.requesterPhone.isNullOrEmpty()) contactDetails.append("📞 ${item.requesterPhone}  ")
                if (!item.requesterEmail.isNullOrEmpty()) contactDetails.append("✉️ ${item.requesterEmail}")
                txtItemContact.text = if (contactDetails.isNotEmpty()) contactDetails.toString() else "No contact info"

                val currencyFormat = NumberFormat.getCurrencyInstance(Locale.US)
                txtItemPrice.text = currencyFormat.format(item.totalPrice)

                // Show Admin Action buttons if Status is Pending
                if (item.status.equals("Pending", ignoreCase = true)) {
                    layoutAdminActions.visibility = View.VISIBLE
                    btnApproveRental.setOnClickListener { promptApprove(item) }
                    btnDenyRental.setOnClickListener { promptDeny(item) }
                } else {
                    layoutAdminActions.visibility = View.GONE
                }

                // Clicking the card opens the full Rental Detail & Edit screen
                root.setOnClickListener {
                    val intent = Intent(this@HallRentalsActivity, RentalDetailActivity::class.java).apply {
                        putExtra(RentalDetailActivity.EXTRA_RENTAL_ID, item.id)
                    }
                    startActivity(intent)
                }
            }
        }

        override fun getItemCount() = items.size
    }
}
