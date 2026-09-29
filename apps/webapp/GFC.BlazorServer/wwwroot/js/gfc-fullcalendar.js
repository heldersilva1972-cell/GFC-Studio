/**
 * GFC Full Calendar Integration — gfc-fullcalendar.js
 * Renders a public-facing FullCalendar.js instance with:
 *  - Configurable Month Grid Display (Compact pill vs Multi-line cards)
 *  - Configurable and re-orderable field display (time, title, location, status, notes)
 *  - Branded GFC light theme (white / cream / gold accents)
 *  - Configurable event modal popup with customizable CTA button
 *  - Auto-refresh support via Blazor interop
 */

window.GfcFullCalendar = (function () {
    'use strict';

    const _instances = {};   // keyed by containerId
    const _timers    = {};   // auto-refresh timers
    const _configs   = {};   // store payload per container

    /* ── Inject modal + styles once ─────────────────────────────────── */
    function ensureModal() {
        if (document.getElementById('gfc-cal-modal')) return;

        const style = document.createElement('style');
        style.textContent = `
            #gfc-cal-modal-overlay {
                display: none;
                position: fixed; inset: 0;
                background: rgba(0,0,0,0.45);
                backdrop-filter: blur(4px);
                z-index: 9998;
                animation: gfc-fade-in 0.2s ease;
            }
            #gfc-cal-modal {
                display: none;
                position: fixed;
                top: 50%; left: 50%;
                transform: translate(-50%, -48%);
                width: min(460px, 94vw);
                background: #ffffff;
                border: 1px solid #e4e0d8;
                border-radius: 16px;
                box-shadow: 0 20px 60px rgba(0,0,0,0.18);
                z-index: 9999;
                overflow: hidden;
                animation: gfc-slide-up 0.25s cubic-bezier(0.34,1.56,0.64,1);
            }
            .gfc-modal-header {
                background: linear-gradient(135deg, #C49A49 0%, #a07830 100%);
                padding: 20px 24px 16px;
                position: relative;
            }
            .gfc-modal-header h3 {
                margin: 0;
                font-size: 1.15rem;
                font-weight: 700;
                color: #fff;
                line-height: 1.3;
                padding-right: 32px;
            }
            .gfc-modal-close {
                position: absolute; top: 14px; right: 16px;
                background: rgba(255,255,255,0.25);
                border: none; border-radius: 50%;
                width: 28px; height: 28px;
                color: #fff; font-size: 1rem; line-height: 1;
                cursor: pointer; display: flex; align-items: center; justify-content: center;
                transition: background 0.15s;
            }
            .gfc-modal-close:hover { background: rgba(255,255,255,0.4); }
            .gfc-modal-body {
                padding: 22px 24px 24px;
                color: #333;
                font-family: 'Inter', -apple-system, sans-serif;
                background: #ffffff;
            }
            .gfc-modal-row {
                display: flex; align-items: flex-start; gap: 12px;
                margin-bottom: 12px; font-size: 0.92rem;
            }
            .gfc-modal-row:last-of-type { margin-bottom: 0; }
            .gfc-modal-icon { font-size: 1rem; margin-top: 1px; flex-shrink: 0; }
            .gfc-modal-label { font-weight: 700; color: #7a5c1e; font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.06em; margin-bottom: 2px; }
            .gfc-modal-value { color: #1a1a1a; line-height: 1.45; }
            .gfc-modal-divider { border: none; border-top: 1px solid #ebebeb; margin: 16px 0; }
            .gfc-modal-cta {
                display: block; width: 100%;
                background: linear-gradient(135deg, #C49A49, #a07830);
                color: #fff; font-weight: 700; font-size: 0.92rem;
                border: none; border-radius: 8px;
                padding: 11px 0; text-align: center;
                cursor: pointer; text-decoration: none;
                transition: opacity 0.15s, transform 0.1s;
                margin-top: 18px;
                box-shadow: 0 3px 12px rgba(196,154,73,0.3);
            }
            .gfc-modal-cta:hover { opacity: 0.88; transform: translateY(-1px); color: #fff; }
            @keyframes gfc-fade-in  { from { opacity:0 } to { opacity:1 } }
            @keyframes gfc-slide-up { from { opacity:0; transform:translate(-50%,-44%) } to { opacity:1; transform:translate(-50%,-48%) } }

            /* Calendar light-theme button overrides */
            .gfc-calendar-wrap .fc { font-family: 'Inter', -apple-system, sans-serif; }
            .gfc-calendar-wrap .fc-header-toolbar {
                display: flex !important;
                align-items: center !important;
                justify-content: space-between !important;
                margin-bottom: 12px !important;
            }
            .gfc-calendar-wrap .fc-toolbar-chunk:first-child {
                flex: 0 0 auto;
                text-align: left;
            }
            .gfc-calendar-wrap .fc-toolbar-chunk:nth-child(2) {
                flex: 1 1 auto;
                text-align: center;
            }
            .gfc-calendar-wrap .fc-toolbar-chunk:last-child {
                flex: 0 0 auto;
                text-align: right;
            }
            .gfc-calendar-wrap .fc-toolbar-title {
                font-weight: 800;
                color: #1a1a1a;
                text-align: center;
                font-size: 1.15rem;
                display: block;
                width: 100%;
            }
            .gfc-calendar-wrap .fc-button {
                background: #fff !important;
                border: 1px solid #d0ccc4 !important;
                color: #333 !important;
                font-weight: 600;
                box-shadow: none !important;
                transition: background 0.15s, color 0.15s;
            }
            .gfc-calendar-wrap .fc-button:hover {
                background: #f5f0e8 !important;
                border-color: #C49A49 !important;
                color: #7a5c1e !important;
            }
            .gfc-calendar-wrap .fc-button-primary:not(:disabled):active,
            .gfc-calendar-wrap .fc-button-primary.fc-button-active {
                background: #C49A49 !important;
                border-color: #a07830 !important;
                color: #fff !important;
            }
            .gfc-calendar-wrap .fc-today-button {
                background: #f5f0e8 !important;
                border-color: #C49A49 !important;
                color: #7a5c1e !important;
            }
            .gfc-calendar-wrap .fc-today-button:disabled {
                opacity: 0.5 !important;
            }

            /* Event Card Styles for FullCalendar */
            .gfc-calendar-wrap .fc-event {
                cursor: pointer;
                border-radius: 6px;
                font-family: 'Inter', -apple-system, sans-serif;
                border: 1px solid rgba(196,154,73,0.3) !important;
                transition: transform 0.1s, box-shadow 0.1s;
                overflow: hidden;
            }
            .gfc-calendar-wrap .fc-event:hover {
                opacity: 0.95;
                transform: translateY(-1px);
                box-shadow: 0 4px 10px rgba(0,0,0,0.12);
            }
            .gfc-fc-card-body {
                padding: 4px 6px;
                font-size: 0.78rem;
                line-height: 1.35;
                color: inherit;
            }
            .gfc-fc-card-line {
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
                margin-bottom: 2px;
            }
            .gfc-fc-card-line:last-child {
                margin-bottom: 0;
            }
            .gfc-fc-badge {
                display: inline-block;
                padding: 1px 5px;
                font-size: 0.70rem;
                font-weight: 700;
                border-radius: 4px;
                text-transform: uppercase;
                letter-spacing: 0.03em;
            }
            .gfc-fc-badge-booked {
                background: #1e293b;
                color: #ffffff;
            }
            .gfc-fc-badge-pending {
                background: #fef3c7;
                border: 1px solid #fcd34d;
                color: #92400e;
            }
            .gfc-fc-badge-inquiry {
                background: #e0f2fe;
                border: 1px solid #7dd3fc;
                color: #0369a1;
            }

            /* Mobile Day Drawer & Dot Indicators */
            .gfc-mobile-dot-wrap {
                display: flex;
                align-items: center;
                justify-content: center;
                gap: 3px;
                margin-top: 2px;
            }
            .gfc-mobile-dot {
                width: 6px;
                height: 6px;
                border-radius: 50%;
                display: inline-block;
            }
            .gfc-mobile-dot-booked { background-color: #1e293b; }
            .gfc-mobile-dot-pending { background-color: #d97706; }
            .gfc-mobile-dot-inquiry { background-color: #0284c7; }

            .gfc-mobile-selected-day {
                background-color: #fff9ed !important;
                outline: 2px solid #C49A49 !important;
                outline-offset: -2px;
                border-radius: 6px;
            }

            .gfc-mobile-day-drawer {
                background: #ffffff;
                border: 1px solid #e2ded5;
                border-radius: 12px;
                padding: 14px 16px;
                margin-top: 14px;
                box-shadow: 0 4px 12px rgba(0,0,0,0.06);
                animation: gfc-fade-in 0.2s ease;
            }
            .gfc-mobile-drawer-header {
                font-size: 0.92rem;
                font-weight: 800;
                color: #1a1a1a;
                border-bottom: 1px solid #f0ede6;
                padding-bottom: 8px;
                margin-bottom: 10px;
                display: flex;
                align-items: center;
                justify-content: space-between;
            }
            .gfc-mobile-event-card {
                background: #fdfbf7;
                border: 1px solid #e8e3d8;
                border-left: 4px solid #C49A49;
                border-radius: 8px;
                padding: 10px 12px;
                margin-bottom: 8px;
            }
            .gfc-mobile-event-card.pending {
                border-left-color: #d97706;
                background: #fffdf5;
            }
            .gfc-mobile-open-banner {
                background: #f0fdf4;
                border: 1px dashed #86efac;
                border-radius: 8px;
                padding: 10px 12px;
                margin-top: 10px;
                text-align: center;
            }
            .gfc-mobile-action-btns {
                display: flex;
                gap: 8px;
                margin-top: 10px;
                flex-wrap: wrap;
            }
            .gfc-mobile-action-btn {
                flex: 1;
                min-width: 130px;
                padding: 8px 12px;
                font-size: 0.82rem;
                font-weight: 700;
                border-radius: 8px;
                text-align: center;
                text-decoration: none;
                display: inline-flex;
                align-items: center;
                justify-content: center;
                gap: 4px;
                transition: opacity 0.15s, transform 0.1s;
            }
            .gfc-mobile-btn-book {
                background: linear-gradient(135deg, #C49A49, #a07830);
                color: #fff !important;
                border: none;
                box-shadow: 0 2px 6px rgba(196,154,73,0.3);
            }
            .gfc-mobile-btn-inquiry {
                background: #fff;
                color: #7a5c1e !important;
                border: 1px solid #C49A49;
            }
        `;
        document.head.appendChild(style);

        const overlay = document.createElement('div');
        overlay.id = 'gfc-cal-modal-overlay';
        overlay.addEventListener('click', closeModal);
        document.body.appendChild(overlay);

        const modal = document.createElement('div');
        modal.id = 'gfc-cal-modal';
        modal.innerHTML = `
            <div class="gfc-modal-header">
                <h3 id="gfc-modal-title">Event</h3>
                <button class="gfc-modal-close" onclick="GfcFullCalendar.closeModal()" aria-label="Close">✕</button>
            </div>
            <div class="gfc-modal-body" id="gfc-modal-body"></div>
        `;
        document.body.appendChild(modal);
    }

    function openModal(event, containerId) {
        ensureModal();
        const config   = _configs[containerId] || {};
        if (config.showEventModal === false) return;

        let title    = event.title || 'Upcoming Event';
        title        = title.replace(/^(PENDING:\s*|INQUIRY:\s*)/i, '').trim();
        const start    = event.start;
        const end      = event.end;
        const allDay   = event.allDay;
        const desc     = event.extendedProps?.description || '';
        const location = event.extendedProps?.location    || '';
        const source   = event.extendedProps?.source      || '';
        const status   = (event.extendedProps?.status || 'Approved').toLowerCase();

        // Title Row
        if (config.showModalTitle !== false) {
            document.getElementById('gfc-modal-title').textContent = title;
        } else {
            document.getElementById('gfc-modal-title').textContent = 'Event Details';
        }

        const fmt = (d, includeTime) => {
            if (!d) return '';
            const opts = { weekday:'long', year:'numeric', month:'long', day:'numeric' };
            if (!allDay && includeTime) { opts.hour = '2-digit'; opts.minute = '2-digit'; }
            return d.toLocaleDateString('en-US', opts);
        };

        let rowsHtml = '';

        // Status Badge Row
        if (config.showModalStatus !== false) {
            let statusBadge = '';
            if (status === 'pending') {
                const label = config.pendingBadgeText || 'PENDING';
                statusBadge = `<span class="gfc-fc-badge gfc-fc-badge-pending" style="font-size:0.78rem; padding:2px 8px;">${label}</span>`;
            } else if (status === 'inquiry') {
                statusBadge = `<span class="gfc-fc-badge gfc-fc-badge-inquiry" style="font-size:0.78rem; padding:2px 8px;">INQUIRY</span>`;
            } else {
                const label = config.approvedBadgeText || 'RESERVED';
                statusBadge = `<span class="gfc-fc-badge gfc-fc-badge-booked" style="font-size:0.78rem; padding:2px 8px;">${label}</span>`;
            }
            rowsHtml += row('🏷️', 'Booking Status', statusBadge);
        }

        // Time / Date Row
        if (config.showModalTime !== false) {
            if (allDay) {
                rowsHtml += row('📅', 'Date', fmt(start, false) + (end ? ' – ' + fmt(new Date(end - 86400000), false) : ''));
            } else {
                rowsHtml += row('📅', 'Date', fmt(start, false));
                const t1 = start ? start.toLocaleTimeString('en-US', {hour:'2-digit', minute:'2-digit'}) : '';
                const t2 = end   ? end.toLocaleTimeString('en-US',   {hour:'2-digit', minute:'2-digit'}) : '';
                if (t1) rowsHtml += row('🕐', 'Time Window', t1 + (t2 ? ' – ' + t2 : ''));
            }
        }

        // Location Space Row
        if (config.showModalLoc !== false && location) {
            rowsHtml += row('📍', 'Room / Space', location);
        }

        // Description / Notes Row
        if (config.showModalDesc !== false && desc) {
            rowsHtml += `<hr class="gfc-modal-divider">${row('📝', 'Public Details', desc)}`;
        }

        // Source / Reference Row
        if (config.showModalSource !== false && source) {
            rowsHtml += row('ℹ️', 'Reference Source', source);
        }

        // Action Buttons (Independent Book & Ask Question)
        let buttonsHtml = '';
        const showBook = config.showModalBookButton !== false;
        const showQuestion = config.showModalQuestionButton !== false;

        if (showBook || showQuestion) {
            buttonsHtml += '<div style="display:flex; gap:10px; margin-top:20px; flex-wrap:wrap;">';
            if (showBook) {
                const bText = config.modalBookButtonText || 'Book the Hall';
                const bUrl  = config.modalBookButtonUrl  || '/rentals/apply';
                buttonsHtml += `<a class="gfc-modal-cta" style="flex:1; min-width:140px; margin-top:0;" href="${bUrl}" target="_blank" rel="noopener">📅 ${bText}</a>`;
            }
            if (showQuestion) {
                const qText = config.modalQuestionButtonText || 'Ask a Question';
                const qUrl  = config.modalQuestionButtonUrl  || '/rentals/apply?mode=inquiry';
                buttonsHtml += `<a class="gfc-modal-cta" style="flex:1; min-width:140px; margin-top:0; background:#fff; color:#7a5c1e; border:1px solid #C49A49; box-shadow:none;" href="${qUrl}" target="_blank" rel="noopener">❓ ${qText}</a>`;
            }
            buttonsHtml += '</div>';
        }

        document.getElementById('gfc-modal-body').innerHTML = `${rowsHtml}${buttonsHtml}`;

        document.getElementById('gfc-cal-modal-overlay').style.display = 'block';
        document.getElementById('gfc-cal-modal').style.display = 'block';
    }

    function row(icon, label, value) {
        return `<div class="gfc-modal-row">
                    <span class="gfc-modal-icon">${icon}</span>
                    <div><div class="gfc-modal-label">${label}</div><div class="gfc-modal-value">${value}</div></div>
                </div>`;
    }

    function closeModal() {
        const overlay = document.getElementById('gfc-cal-modal-overlay');
        const modal   = document.getElementById('gfc-cal-modal');
        if (overlay) overlay.style.display = 'none';
        if (modal)   modal.style.display   = 'none';
    }

    function isMobileViewport(containerId) {
        const el = document.getElementById(containerId);
        if (!el) return window.innerWidth < 640;
        return el.offsetWidth < 520 || window.innerWidth < 640;
    }

    /* ── Render custom event cell based on admin field configuration ─── */
    function renderEventContent(eventInfo, containerId) {
        const config = _configs[containerId] || {};
        const isMobile = isMobileViewport(containerId) && (config.mobileDisplayMode !== 'standard');
        const isCompact = config.monthDisplayMode === 'compact';
        const fields = config.monthFields || [
            { key: 'time_window', order: 1 },
            { key: 'event_title', order: 2 }
        ];

        const event = eventInfo.event;
        const start = event.start;
        const end = event.end;
        const allDay = event.allDay;
        let title = event.title || 'Reserved Event';
        // Strip any remaining PENDING: or INQUIRY: prefixes safely
        title = title.replace(/^(PENDING:\s*|INQUIRY:\s*)/i, '').trim();

        const location = event.extendedProps?.location || '';
        const desc = event.extendedProps?.description || '';
        const status = (event.extendedProps?.status || 'Approved').toLowerCase();

        // On mobile in drawer mode: render clean dot indicator instead of cramped cards
        if (isMobile) {
            let dotClass = 'gfc-mobile-dot-booked';
            if (status === 'pending') dotClass = 'gfc-mobile-dot-pending';
            else if (status === 'inquiry') dotClass = 'gfc-mobile-dot-inquiry';
            return {
                html: `<div class="gfc-mobile-dot-wrap"><span class="gfc-mobile-dot ${dotClass}" title="${title}"></span></div>`
            };
        }

        // Determine badge label and visibility
        let badgeHtml = '';
        if (status === 'pending') {
            if (config.showPendingBadge !== false) {
                const label = config.pendingBadgeText || 'PENDING';
                badgeHtml = `<span class="gfc-fc-badge gfc-fc-badge-pending">${label}</span>`;
            }
        } else if (status === 'inquiry') {
            badgeHtml = `<span class="gfc-fc-badge gfc-fc-badge-inquiry">INQUIRY</span>`;
        } else {
            if (config.showApprovedBadge !== false) {
                const label = config.approvedBadgeText || 'RESERVED';
                badgeHtml = `<span class="gfc-fc-badge gfc-fc-badge-booked">${label}</span>`;
            }
        }

        const formatTime = (d) => d ? d.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' }) : '';
        let timeStr = '';
        if (!allDay && start) {
            timeStr = formatTime(start) + (end ? ' - ' + formatTime(end) : '');
        }

        if (isCompact) {
            // Single-line compact badge
            const timePart = timeStr ? `<span>${timeStr}</span> ` : '';
            return {
                html: `<div class="gfc-fc-card-body d-flex align-items-center justify-content-between gap-1 text-truncate" style="padding:2px 4px;">
                         <span class="text-truncate fw-semibold">${timePart}${title}</span>
                         ${badgeHtml}
                       </div>`
            };
        }

        // Multi-line stacked card
        const lines = [];
        fields.forEach(f => {
            switch (f.key) {
                case 'time_window':
                    if (timeStr) {
                        lines.push(`<div class="gfc-fc-card-line fw-bold" style="font-size:0.75rem;"><i class="bi bi-clock me-1"></i>${timeStr}</div>`);
                    } else if (allDay) {
                        lines.push(`<div class="gfc-fc-card-line fw-bold" style="font-size:0.75rem;"><i class="bi bi-calendar-event me-1"></i>All Day</div>`);
                    }
                    break;
                case 'event_title':
                    lines.push(`<div class="gfc-fc-card-line fw-bold" style="font-size:0.80rem;">${title}</div>`);
                    break;
                case 'room_location':
                    if (location) {
                        lines.push(`<div class="gfc-fc-card-line" style="font-size:0.72rem; opacity:0.95;"><i class="bi bi-geo-alt me-1"></i>${location}</div>`);
                    }
                    break;
                case 'booking_status':
                    if (badgeHtml) {
                        lines.push(`<div class="gfc-fc-card-line">${badgeHtml}</div>`);
                    }
                    break;
                case 'public_notes':
                    if (desc) {
                        lines.push(`<div class="gfc-fc-card-line text-truncate" style="font-size:0.70rem; opacity:0.85;">${desc}</div>`);
                    }
                    break;
            }
        });

        if (lines.length === 0) {
            lines.push(`<div class="gfc-fc-card-line fw-bold">${title}</div>`);
        }

        return {
            html: `<div class="gfc-fc-card-body">${lines.join('')}</div>`
        };
    }

    /* ── Mobile Drawer Renderer for Selected Date ───────────────────── */
    function renderMobileDayDrawer(dateStr, containerId) {
        const config = _configs[containerId] || {};
        if (!isMobileViewport(containerId) || config.mobileDisplayMode === 'standard') return;

        let drawer = document.getElementById(containerId + '-mobile-drawer');
        if (!drawer) {
            drawer = document.createElement('div');
            drawer.id = containerId + '-mobile-drawer';
            drawer.className = 'gfc-mobile-day-drawer';
            const calEl = document.getElementById(containerId);
            if (calEl && calEl.parentNode) {
                calEl.parentNode.insertBefore(drawer, calEl.nextSibling);
            }
        }

        const events = (config.events || []).filter(e => {
            const startD = e.start ? e.start.substring(0, 10) : '';
            return startD === dateStr;
        });

        const dObj = new Date(dateStr + 'T12:00:00');
        const dateDisplay = dObj.toLocaleDateString('en-US', { weekday: 'long', month: 'short', day: 'numeric', year: 'numeric' });

        let content = `<div class="gfc-mobile-drawer-header">
                         <span>📅 ${dateDisplay}</span>
                         <span class="small text-muted">${events.length} booking${events.length === 1 ? '' : 's'}</span>
                       </div>`;

        let hasFullDayBooking = false;
        let bookedHours = 0;

        if (events.length > 0) {
            events.forEach(e => {
                let cleanTitle = (e.title || 'Reserved Event').replace(/^(PENDING:\s*|INQUIRY:\s*)/i, '').trim();
                const status = (e.status || 'Approved').toLowerCase();
                const isPending = status === 'pending';
                const isAllDay = e.allDay;
                if (isAllDay) hasFullDayBooking = true;

                const formatT = (ds) => {
                    if (!ds) return '';
                    const d = new Date(ds);
                    return isNaN(d) ? '' : d.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' });
                };

                let timeDisplay = isAllDay ? 'All-Day Booking' : `${formatT(e.start)} - ${formatT(e.end)}`;
                let badge = isPending 
                    ? `<span class="gfc-fc-badge gfc-fc-badge-pending">${config.pendingBadgeText || 'PENDING'}</span>`
                    : `<span class="gfc-fc-badge gfc-fc-badge-booked">${config.approvedBadgeText || 'RESERVED'}</span>`;

                content += `<div class="gfc-mobile-event-card ${isPending ? 'pending' : ''}">
                              <div class="d-flex align-items-center justify-content-between mb-1">
                                <span class="fw-bold small text-dark">${cleanTitle}</span>
                                ${badge}
                              </div>
                              <div class="small text-muted mb-1"><i class="bi bi-clock me-1"></i>${timeDisplay}</div>
                              ${e.location ? `<div class="small text-secondary"><i class="bi bi-geo-alt me-1"></i>${e.location}</div>` : ''}
                            </div>`;
            });
        } else {
            content += `<div class="p-3 text-center text-muted small bg-light rounded-3 mb-2">
                          <i class="bi bi-calendar-check text-success fs-5 d-block mb-1"></i>
                          <span>No events scheduled. Date is available!</span>
                        </div>`;
        }

        // Available slots prompt & Action Buttons
        const showBook = config.mobileShowBookAvailableSlot !== false;
        const showInq  = config.mobileShowQuestionAvailableSlot !== false;

        if (!hasFullDayBooking && (showBook || showInq)) {
            const hasPartial = events.length > 0;
            const openMsg = hasPartial 
                ? (config.mobileOpenSlotMessage || 'Remaining time slot(s) are available for booking on this date!')
                : 'This entire date is currently open for booking!';

            content += `<div class="gfc-mobile-open-banner">
                          <div class="small fw-semibold text-success mb-2">✨ ${openMsg}</div>
                          <div class="gfc-mobile-action-btns">
                            ${showBook ? `<a href="/rentals/apply?date=${dateStr}" class="gfc-mobile-action-btn gfc-mobile-btn-book">📅 ${config.mobileBookAvailableSlotText || 'Book Available Slot'}</a>` : ''}
                            ${showInq ? `<a href="/rentals/apply?mode=inquiry&date=${dateStr}" class="gfc-mobile-action-btn gfc-mobile-btn-inquiry">❓ ${config.mobileQuestionAvailableSlotText || 'Ask a Question'}</a>` : ''}
                          </div>
                        </div>`;
        }

        drawer.innerHTML = content;

        // Auto-scroll drawer smoothly into view so mobile users immediately see the tapped date details
        setTimeout(() => {
            try {
                drawer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
            } catch (e) { }
        }, 50);
    }

    /* ── Build event array ───────────────────────────────────────────── */
    function buildEvents(events, primaryColor, textColor) {
        return (events || []).map(e => ({
            id:              e.id || e.googleEventId,
            title:           e.title || '(No Title)',
            start:           e.start,
            end:             e.end   || undefined,
            allDay:          e.allDay,
            backgroundColor: primaryColor || '#C49A49',
            borderColor:     primaryColor || '#C49A49',
            textColor:       textColor    || '#FFFFFF',
            extendedProps:   {
                status:      e.status      || e.extendedProps?.status || 'Approved',
                description: e.description || e.extendedProps?.description,
                location:    e.location    || e.extendedProps?.location,
                source:      e.source
            }
        }));
    }

    /* ── Public API ──────────────────────────────────────────────────── */
    return {

        closeModal,

        init: function (containerId, payload) {
            ensureModal();
            const el = document.getElementById(containerId);
            if (!el || typeof FullCalendar === 'undefined') return;

            _configs[containerId] = payload;

            if (_instances[containerId]) _instances[containerId].destroy();

            const isMobile = isMobileViewport(containerId);

            const cal = new FullCalendar.Calendar(el, {
                initialView:    payload.defaultView || 'dayGridMonth',
                buttonText: {
                    prev: '< Prev',
                    next: 'Next >'
                },
                headerToolbar: isMobile ? {
                    left:   'prev',
                    center: 'title',
                    right:  'next'
                } : {
                    left:   'prev,next today',
                    center: 'title',
                    right:  'dayGridMonth,listMonth'
                },
                height:       'auto',
                nowIndicator: true,
                events:       buildEvents(payload.events, payload.primaryColor, payload.textColor),
                eventContent: info => renderEventContent(info, containerId),
                eventClick:   info => {
                    info.jsEvent.preventDefault();
                    info.jsEvent.stopPropagation();
                    if (isMobileViewport(containerId) && payload.mobileDisplayMode !== 'standard') {
                        const dStr = info.event.startStr ? info.event.startStr.substring(0, 10) : '';
                        if (dStr) {
                            // Find and highlight matching day element
                            document.querySelectorAll('.gfc-mobile-selected-day').forEach(d => d.classList.remove('gfc-mobile-selected-day'));
                            const matchingDayEl = document.querySelector(`[data-date="${dStr}"]`);
                            if (matchingDayEl) matchingDayEl.classList.add('gfc-mobile-selected-day');
                            renderMobileDayDrawer(dStr, containerId);
                        }
                    } else {
                        openModal(info.event, containerId);
                    }
                },
                dateClick:    info => {
                    if (isMobileViewport(containerId) && payload.mobileDisplayMode !== 'standard') {
                        // Highlight day cell
                        document.querySelectorAll('.gfc-mobile-selected-day').forEach(d => d.classList.remove('gfc-mobile-selected-day'));
                        if (info.dayEl) info.dayEl.classList.add('gfc-mobile-selected-day');
                        renderMobileDayDrawer(info.dateStr, containerId);
                    }
                },
                loading:      isLoading => {
                    const spinner = document.getElementById(containerId + '-loading');
                    if (spinner) spinner.style.display = isLoading ? 'flex' : 'none';
                }
            });

            cal.render();
            _instances[containerId] = cal;

            // Auto-render today's drawer on mobile initial load
            if (isMobile && payload.mobileDisplayMode !== 'standard') {
                const todayStr = new Date().toISOString().substring(0, 10);
                setTimeout(() => {
                    const todayEl = document.querySelector(`[data-date="${todayStr}"]`);
                    if (todayEl) todayEl.classList.add('gfc-mobile-selected-day');
                    renderMobileDayDrawer(todayStr, containerId);
                }, 100);
            }

            // Auto-refresh every refreshMs if requested
            if (payload.refreshMs && payload.dotNetRef) {
                if (_timers[containerId]) clearInterval(_timers[containerId]);
                _timers[containerId] = setInterval(() => {
                    payload.dotNetRef.invokeMethodAsync('AutoRefresh').catch(() => {});
                }, payload.refreshMs);
            }
        },

        update: function (containerId, payload) {
            _configs[containerId] = payload;
            const cal = _instances[containerId];
            if (!cal) { this.init(containerId, payload); return; }
            if (payload.defaultView && cal.view.type !== payload.defaultView) cal.changeView(payload.defaultView);
            cal.removeAllEvents();
            buildEvents(payload.events, payload.primaryColor, payload.textColor).forEach(e => cal.addEvent(e));
            cal.render();

            if (isMobileViewport(containerId) && payload.mobileDisplayMode !== 'standard') {
                const todayStr = new Date().toISOString().substring(0, 10);
                const todayEl = document.querySelector(`[data-date="${todayStr}"]`);
                if (todayEl) todayEl.classList.add('gfc-mobile-selected-day');
                renderMobileDayDrawer(todayStr, containerId);
            }
        },

        destroy: function (containerId) {
            if (_timers[containerId])    { clearInterval(_timers[containerId]); delete _timers[containerId]; }
            if (_instances[containerId]) { _instances[containerId].destroy();   delete _instances[containerId]; }
            const drawer = document.getElementById(containerId + '-mobile-drawer');
            if (drawer) drawer.remove();
            delete _configs[containerId];
        }
    };
})();
