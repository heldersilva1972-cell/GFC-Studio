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
                display: inline-flex !important;
                align-items: center !important;
                justify-content: center !important;
                gap: 14px !important;
            }
            .gfc-calendar-wrap .fc-toolbar-chunk:last-child {
                flex: 0 0 auto;
                text-align: right;
            }
            .gfc-calendar-wrap .fc-toolbar-title {
                font-weight: 800;
                color: #1a1a1a;
                text-align: center;
                font-size: 1.2rem;
                display: inline-block;
                margin: 0 !important;
                min-width: 170px;
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

            /* Day Grid Cell cursor and hover */
            .gfc-calendar-wrap .fc-daygrid-day {
                cursor: pointer;
                transition: background-color 0.15s ease;
            }
            .gfc-calendar-wrap .fc-daygrid-day:hover {
                background-color: #fefce8 !important;
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
                background: #166534;
                border: 1px solid #14532d;
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

            /* Partial Day Availability Indicator Pill */
            .gfc-fc-partial-avail-pill {
                margin: 4px 4px 2px;
                padding: 3px 6px;
                background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
                border: 1px dashed #86efac;
                border-radius: 6px;
                color: #15803d;
                font-size: 0.70rem;
                font-weight: 700;
                display: flex;
                align-items: center;
                gap: 4px;
                cursor: pointer;
                transition: all 0.15s ease;
                box-shadow: 0 1px 3px rgba(22,101,52,0.06);
            }
            .gfc-fc-partial-avail-pill:hover {
                background: #bbf7d0;
                border-color: #4ade80;
                color: #14532d;
                transform: translateY(-1px);
                box-shadow: 0 2px 6px rgba(22,101,52,0.15);
            }
            .gfc-fc-partial-avail-dot {
                width: 6px;
                height: 6px;
                background-color: #16a34a;
                border-radius: 50%;
                display: inline-block;
                flex-shrink: 0;
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
            .gfc-mobile-dot-booked { background-color: #166534; }
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
        const isClub = event.extendedProps?.isClubEvent === true;
        if (config.showModalStatus !== false && !isClub) {
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

        document.getElementById('gfc-modal-body').innerHTML = `${rowsHtml}`;

        document.getElementById('gfc-cal-modal-overlay').style.display = 'block';
        document.getElementById('gfc-cal-modal').style.display = 'block';
    }

    function parseTimeToMinutes(timeStr) {
        if (!timeStr) return -1;
        timeStr = timeStr.trim();
        if (timeStr.toLowerCase() === '12:00 midnight') return 24 * 60;
        const match = timeStr.match(/^(\d{1,2}):(\d{2})\s*(AM|PM)?$/i);
        if (match) {
            let h = parseInt(match[1], 10);
            const m = parseInt(match[2], 10);
            const ampm = (match[3] || '').toUpperCase();
            if (ampm === 'PM' && h < 12) h += 12;
            if (ampm === 'AM' && h === 12) h = 0;
            return h * 60 + m;
        }
        const d = new Date('2000-01-01 ' + timeStr);
        if (!isNaN(d)) return d.getHours() * 60 + d.getMinutes();
        return -1;
    }

    function checkDateAvailability(dateStr, events, config) {
        const dObj = new Date(dateStr + 'T12:00:00');
        const dayOfWeek = dObj.getDay(); // 0 = Sunday, 1 = Monday...
        const daySchedules = config.daySchedules || [];
        const dayConfig = daySchedules.find(d => {
            const dw = typeof d.dayOfWeek === 'number' ? d.dayOfWeek : (typeof d.day === 'number' ? d.day : null);
            return dw === dayOfWeek;
        });

        if (dayConfig && dayConfig.isAvailable === false) {
            return { isFullyBooked: true, reason: 'disabled', remainingCount: 0 };
        }

        const activeSlots = (dayConfig && dayConfig.slots) ? dayConfig.slots.filter(s => s.isActive !== false) : [];

        // Check if any all-day booking or blackout event exists
        const hasAllDayOrBlackout = events.some(e => e.allDay || e.isBlackoutBlocked || (e.extendedProps && e.extendedProps.isBlackoutBlocked));
        if (hasAllDayOrBlackout) {
            return { isFullyBooked: true, reason: 'allday', remainingCount: 0 };
        }

        if (activeSlots.length > 0) {
            // Count how many configured slots have overlapping bookings
            let bookedSlotsCount = 0;
            const openSlots = [];
            activeSlots.forEach(slot => {
                const sStart = parseTimeToMinutes(slot.startTime);
                const sEnd = parseTimeToMinutes(slot.endTime);
                if (sStart < 0 || sEnd < 0) return;

                const isSlotTaken = events.some(e => {
                    if (e.allDay) return true;
                    if (!e.start || !e.end) return true;
                    const eStart = new Date(e.start).getHours() * 60 + new Date(e.start).getMinutes();
                    const eEnd = new Date(e.end).getHours() * 60 + new Date(e.end).getMinutes();
                    return (sStart < eEnd && sEnd > eStart);
                });

                if (isSlotTaken) {
                    bookedSlotsCount++;
                } else {
                    openSlots.push({
                        label: slot.label || `${slot.startTime} - ${slot.endTime}`,
                        startTime: slot.startTime,
                        endTime: slot.endTime
                    });
                }
            });

            if (bookedSlotsCount >= activeSlots.length) {
                return { isFullyBooked: true, reason: 'all_slots_taken', remainingCount: 0, totalSlots: activeSlots.length, openSlots: [] };
            }
            return {
                isFullyBooked: false,
                reason: 'partial',
                remainingCount: activeSlots.length - bookedSlotsCount,
                totalSlots: activeSlots.length,
                openSlots: openSlots
            };
        }

        // Flexible hours: if 2 or more events exist or total duration >= 10 hours, treat as fully booked
        if (events.length >= 2) {
            return { isFullyBooked: true, reason: 'capacity', remainingCount: 0, openSlots: [] };
        }

        return { isFullyBooked: false, reason: events.length > 0 ? 'partial' : 'empty', remainingCount: 1, openSlots: [] };
    }

    function openDateModal(dateStr, containerId) {
        ensureModal();
        const config = _configs[containerId] || {};
        if (config.showEventModal === false) return;

        const events = (config.events || []).filter(e => {
            const startD = e.start ? e.start.substring(0, 10) : '';
            return startD === dateStr;
        });

        const dObj = new Date(dateStr + 'T12:00:00');
        const dateDisplay = dObj.toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric' });

        document.getElementById('gfc-modal-title').textContent = `📅 ${dateDisplay}`;

        let eventsHtml = '';

        if (events.length > 0) {
            events.forEach(e => {
                let cleanTitle = (e.title || 'Reserved Event').replace(/^(PENDING:\s*|INQUIRY:\s*)/i, '').trim();
                const status = (e.status || 'Approved').toLowerCase();
                const isPending = status === 'pending';
                const isInquiry = status === 'inquiry';
                const isAllDay = e.allDay;

                const formatT = (ds) => {
                    if (!ds) return '';
                    const d = new Date(ds);
                    return isNaN(d) ? '' : d.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' });
                };

                let timeDisplay = isAllDay ? 'All-Day Booking' : `${formatT(e.start)} - ${formatT(e.end)}`;
                
                const showTitle = config.showModalTitle !== false;
                const showStatus = config.showModalStatus !== false;
                const showTime = config.showModalTime !== false;
                const showLoc = config.showModalLoc !== false;
                const showDesc = config.showModalDesc !== false;
                const showSource = config.showModalSource !== false;

                let badge = '';
                if (showStatus && !e.isClubEvent) {
                    badge = isPending 
                        ? `<span class="gfc-fc-badge gfc-fc-badge-pending">${config.pendingBadgeText || 'PENDING'}</span>`
                        : (isInquiry 
                            ? `<span class="gfc-fc-badge gfc-fc-badge-inquiry">INQUIRY</span>`
                            : `<span class="gfc-fc-badge gfc-fc-badge-booked">${config.approvedBadgeText || 'RESERVED'}</span>`);
                }

                const titleHtml = showTitle ? `<span style="font-weight:700; color:#1a1a1a; font-size:0.92rem;">${cleanTitle}</span>` : '';
                const timeHtml = (showTime && timeDisplay) ? `<div style="font-size:0.82rem; color:#666; margin-bottom:2px;"><i class="bi bi-clock me-1"></i>${timeDisplay}</div>` : '';
                const locHtml = (showLoc && e.location) ? `<div style="font-size:0.80rem; color:#777; margin-bottom:2px;"><i class="bi bi-geo-alt me-1"></i>${e.location}</div>` : '';
                const descHtml = (showDesc && e.description) ? `<div style="font-size:0.78rem; color:#555; background:#fff; border:1px solid #eee; border-radius:4px; padding:4px 8px; margin-top:4px;"><i class="bi bi-card-text me-1 text-muted"></i>${e.description}</div>` : '';
                const sourceHtml = (showSource && e.source) ? `<div style="font-size:0.72rem; color:#888; margin-top:3px;"><i class="bi bi-info-circle me-1"></i>Source: ${e.source}</div>` : '';

                eventsHtml += `
                    <div style="background:#fdfbf7; border:1px solid #e8e3d8; border-left:4px solid ${isPending ? '#d97706' : (isInquiry ? '#0284c7' : '#166534')}; border-radius:8px; padding:10px 14px; margin-bottom:10px;">
                        ${(titleHtml || badge) ? `
                        <div style="display:flex; align-items:center; justify-content:space-between; margin-bottom:4px; flex-wrap:wrap; gap:4px;">
                            ${titleHtml}
                            ${badge}
                        </div>` : ''}
                        ${timeHtml}
                        ${locHtml}
                        ${descHtml}
                        ${sourceHtml}
                    </div>`;
            });
        } else {
            eventsHtml = `
                <div style="padding:18px 16px; text-align:center; background:#f0fdf4; border:1px solid #86efac; border-radius:10px; margin-bottom:12px;">
                    <div style="font-size:1.4rem; margin-bottom:4px;">✨</div>
                    <div style="font-weight:700; color:#15803d; font-size:0.95rem; margin-bottom:2px;">Date is Open &amp; Available!</div>
                    <div style="font-size:0.84rem; color:#166534;">No existing bookings are scheduled for this date.</div>
                </div>`;
        }

        // Available slot notice and action buttons
        let actionsHtml = '';
        const availInfo = checkDateAvailability(dateStr, events, config);

        if (availInfo.isFullyBooked) {
            actionsHtml += `
                <div style="background:#fef2f2; border:1px solid #fecaca; border-radius:12px; padding:14px 16px; margin-top:14px; text-align:center;">
                    <div style="font-size:0.92rem; font-weight:700; color:#991b1b; margin-bottom:2px;">🚫 Date is Fully Booked</div>
                    <div style="font-size:0.82rem; color:#b91c1c;">All available time slots for this date are currently reserved or pending. Please select another date.</div>
                </div>`;
        } else {
            const showBook = config.mobileShowBookAvailableSlot !== false && config.showModalBookButton !== false;
            const showInq  = config.mobileShowQuestionAvailableSlot !== false && config.showModalQuestionButton !== false;

            if (showBook || showInq) {
                const hasPartial = events.length > 0;
                const openMsg = hasPartial 
                    ? (config.mobileOpenSlotMessage || 'Remaining time slot(s) are available for booking on this date!')
                    : 'This date is open and available for hall rental reservations!';

                actionsHtml += `
                    <div style="background:linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%); border:2px solid #86efac; border-radius:12px; padding:16px; margin-top:14px; box-shadow:0 4px 12px rgba(22,101,52,0.08);">
                        <div style="display:flex; align-items:center; justify-content:space-between; margin-bottom:8px; flex-wrap:wrap; gap:6px;">
                            <span style="font-size:0.75rem; font-weight:800; background:#16a34a; color:#ffffff; padding:2px 8px; border-radius:20px; text-transform:uppercase; letter-spacing:0.04em;">
                                <i class="bi bi-check-circle-fill me-1"></i>Slots Available
                            </span>
                            <span style="font-size:0.75rem; font-weight:700; color:#15803d;">Reserve Today</span>
                        </div>
                        <div style="font-size:0.90rem; font-weight:700; color:#14532d; margin-bottom:12px; line-height:1.35;">
                            ✨ ${openMsg}
                        </div>
                        <div style="display:flex; gap:10px; flex-wrap:wrap;">
                            ${showBook ? `<a href="/rentals/apply?date=${dateStr}" target="_blank" rel="noopener" class="gfc-modal-cta" style="flex:1.2; min-width:140px; margin-top:0; font-size:0.92rem; padding:10px 14px; background:linear-gradient(135deg, #16a34a, #15803d); box-shadow:0 3px 10px rgba(22,163,74,0.3);">📅 ${config.mobileBookAvailableSlotText || config.modalBookButtonText || 'Book Available Slot'}</a>` : ''}
                            ${showInq ? `<a href="/rentals/apply?mode=inquiry&date=${dateStr}" target="_blank" rel="noopener" class="gfc-modal-cta" style="flex:1; min-width:130px; margin-top:0; font-size:0.90rem; padding:10px 14px; background:#ffffff; color:#166534; border:1.5px solid #86efac; box-shadow:none;">❓ ${config.mobileQuestionAvailableSlotText || config.modalQuestionButtonText || 'Ask a Question'}</a>` : ''}
                        </div>
                    </div>`;
            }
        }

        document.getElementById('gfc-modal-body').innerHTML = `
            <div style="margin-bottom:12px;">
                <span style="font-size:0.75rem; font-weight:700; color:#7a5c1e; text-transform:uppercase; letter-spacing:0.06em; display:block; margin-bottom:8px;">
                    Schedule &amp; Availability (${events.length} booking${events.length === 1 ? '' : 's'})
                </span>
                ${eventsHtml}
            </div>
            ${actionsHtml}
        `;

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
        const isClub = event.extendedProps?.isClubEvent === true;
        let badgeHtml = '';
        if (!isClub) {
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
                
                const showTitle = config.showModalTitle !== false;
                const showStatus = config.showModalStatus !== false;
                const showTime = config.showModalTime !== false;
                const showLoc = config.showModalLoc !== false;
                const showDesc = config.showModalDesc !== false;
                const showSource = config.showModalSource !== false;

                let badge = '';
                if (showStatus && !e.isClubEvent) {
                    badge = isPending 
                        ? `<span class="gfc-fc-badge gfc-fc-badge-pending">${config.pendingBadgeText || 'PENDING'}</span>`
                        : `<span class="gfc-fc-badge gfc-fc-badge-booked">${config.approvedBadgeText || 'RESERVED'}</span>`;
                }

                const titleHtml = showTitle ? `<span class="fw-bold small text-dark">${cleanTitle}</span>` : '';
                const timeHtml = (showTime && timeDisplay) ? `<div class="small text-muted mb-1"><i class="bi bi-clock me-1"></i>${timeDisplay}</div>` : '';
                const locHtml = (showLoc && e.location) ? `<div class="small text-secondary mb-1"><i class="bi bi-geo-alt me-1"></i>${e.location}</div>` : '';
                const descHtml = (showDesc && e.description) ? `<div class="small text-muted p-1 bg-white rounded border mb-1" style="font-size:0.75rem;"><i class="bi bi-card-text me-1"></i>${e.description}</div>` : '';
                const sourceHtml = (showSource && e.source) ? `<div class="small text-muted" style="font-size:0.70rem;"><i class="bi bi-info-circle me-1"></i>Source: ${e.source}</div>` : '';

                content += `<div class="gfc-mobile-event-card ${isPending ? 'pending' : ''}">
                              ${(titleHtml || badge) ? `
                              <div class="d-flex align-items-center justify-content-between mb-1">
                                ${titleHtml}
                                ${badge}
                              </div>` : ''}
                              ${timeHtml}
                              ${locHtml}
                              ${descHtml}
                              ${sourceHtml}
                            </div>`;
            });
        } else {
            content += `<div class="p-3 text-center text-muted small bg-light rounded-3 mb-2">
                          <i class="bi bi-calendar-check text-success fs-5 d-block mb-1"></i>
                          <span>No events scheduled. Date is available!</span>
                        </div>`;
        }

        // Available slots prompt & Action Buttons
        const availInfo = checkDateAvailability(dateStr, events, config);

        if (availInfo.isFullyBooked) {
            content += `<div class="p-3 text-center rounded-3 mb-2" style="background:#fef2f2; border:1px solid #fecaca;">
                          <div class="small fw-bold text-danger mb-1">🚫 Date is Fully Booked</div>
                          <div class="text-muted" style="font-size:0.78rem;">All available time slots for this date are currently reserved or pending.</div>
                        </div>`;
        } else {
            const showBook = config.mobileShowBookAvailableSlot !== false;
            const showInq  = config.mobileShowQuestionAvailableSlot !== false;

            if (showBook || showInq) {
                const hasPartial = events.length > 0;
                const openMsg = hasPartial 
                    ? (config.mobileOpenSlotMessage || 'Remaining time slot(s) are available for booking on this date!')
                    : 'This entire date is open and available for booking!';

                content += `<div class="gfc-mobile-open-banner" style="background:linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%); border:2px solid #86efac; border-radius:12px; padding:14px; margin-top:12px; box-shadow:0 3px 10px rgba(22,101,52,0.08);">
                              <div style="display:flex; align-items:center; justify-content:space-between; margin-bottom:6px;">
                                <span style="font-size:0.72rem; font-weight:800; background:#16a34a; color:#ffffff; padding:2px 8px; border-radius:20px; text-transform:uppercase;">
                                  <i class="bi bi-check-circle-fill me-1"></i>Available
                                </span>
                              </div>
                              <div class="small fw-bold text-success mb-2" style="font-size:0.88rem; color:#14532d !important; text-align:left;">✨ ${openMsg}</div>
                              <div class="gfc-mobile-action-btns" style="margin-top:10px;">
                                ${showBook ? `<a href="/rentals/apply?date=${dateStr}" class="gfc-mobile-action-btn" style="background:linear-gradient(135deg, #16a34a, #15803d); color:#ffffff !important; box-shadow:0 3px 10px rgba(22,163,74,0.3); font-weight:700;">📅 ${config.mobileBookAvailableSlotText || 'Book Available Slot'}</a>` : ''}
                                ${showInq ? `<a href="/rentals/apply?mode=inquiry&date=${dateStr}" class="gfc-mobile-action-btn" style="background:#ffffff; color:#166534 !important; border:1.5px solid #86efac; font-weight:700;">❓ ${config.mobileQuestionAvailableSlotText || 'Ask a Question'}</a>` : ''}
                              </div>
                            </div>`;
            }
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
    function buildEvents(events, config) {
        config = config || {};
        const defaultBg = config.primaryColor || '#C49A49';
        const defaultText = config.textColor || '#FFFFFF';
        const colorCodeClub = config.colorCodeClubEvents !== false; // default true
        const clubBg = config.clubEventColor || '#7c3aed';
        const clubText = config.clubEventTextColor || '#FFFFFF';
        const secBg = config.secondarySpaceEventColor || '#0284c7';
        const secText = config.secondarySpaceEventTextColor || '#FFFFFF';

        return (events || []).map(e => {
            const isClub = e.isClubEvent || e.extendedProps?.isClubEvent || false;
            const isSec = e.isSecondarySpace || e.extendedProps?.isSecondarySpace || false;

            let bg = defaultBg;
            let fg = defaultText;

            if (colorCodeClub && isClub) {
                if (isSec) {
                    bg = secBg;
                    fg = secText;
                } else {
                    bg = clubBg;
                    fg = clubText;
                }
            }

            return {
                id:              e.id || e.googleEventId,
                title:           e.title || '(No Title)',
                start:           e.start,
                end:             e.end   || undefined,
                allDay:          e.allDay,
                backgroundColor: bg,
                borderColor:     bg,
                textColor:       fg,
                extendedProps:   {
                    status:            e.status            || e.extendedProps?.status || 'Approved',
                    isClubEvent:       isClub,
                    isSecondarySpace:  isSec,
                    isBlackoutBlocked: e.isBlackoutBlocked || e.extendedProps?.isBlackoutBlocked || false,
                    description:       e.description       || e.extendedProps?.description,
                    location:          e.location          || e.extendedProps?.location,
                    source:            e.source
                }
            };
        });
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
                    left:   '',
                    center: 'prev title next',
                    right:  'dayGridMonth,listMonth'
                },
                height:       'auto',
                nowIndicator: true,
                events:       buildEvents(payload.events, payload),
                eventContent: info => renderEventContent(info, containerId),
                eventDidMount: function(info) {
                    if (info.el && info.event) {
                        const bg = info.event.backgroundColor;
                        const text = info.event.textColor;
                        if (bg) {
                            info.el.style.setProperty('background-color', bg, 'important');
                            info.el.style.setProperty('border-color', bg, 'important');
                            info.el.style.setProperty('box-shadow', '0 1px 3px rgba(0,0,0,0.06)', 'important');
                        }
                        if (text) {
                            info.el.style.setProperty('color', text, 'important');
                        }
                    }
                },
                eventClick:   info => {
                    info.jsEvent.preventDefault();
                    info.jsEvent.stopPropagation();
                    const dStr = info.event.startStr ? info.event.startStr.substring(0, 10) : '';
                    if (isMobileViewport(containerId) && payload.mobileDisplayMode !== 'standard') {
                        if (dStr) {
                            // Find and highlight matching day element
                            document.querySelectorAll('.gfc-mobile-selected-day').forEach(d => d.classList.remove('gfc-mobile-selected-day'));
                            const matchingDayEl = document.querySelector(`[data-date="${dStr}"]`);
                            if (matchingDayEl) matchingDayEl.classList.add('gfc-mobile-selected-day');
                            renderMobileDayDrawer(dStr, containerId);
                        }
                    } else {
                        if (dStr) {
                            openDateModal(dStr, containerId);
                        } else {
                            openModal(info.event, containerId);
                        }
                    }
                },
                dateClick:    info => {
                    if (isMobileViewport(containerId) && payload.mobileDisplayMode !== 'standard') {
                        // Highlight day cell
                        document.querySelectorAll('.gfc-mobile-selected-day').forEach(d => d.classList.remove('gfc-mobile-selected-day'));
                        if (info.dayEl) info.dayEl.classList.add('gfc-mobile-selected-day');
                        renderMobileDayDrawer(info.dateStr, containerId);
                    } else {
                        openDateModal(info.dateStr, containerId);
                    }
                },
                dayCellDidMount: info => {
                    const config = _configs[containerId] || {};
                    if (config.showPartialAvailabilityBadge === false) return;
                    if (isMobileViewport(containerId) && config.mobileDisplayMode !== 'standard') return;

                    const dateStr = info.date ? info.date.toISOString().substring(0, 10) : '';
                    if (!dateStr) return;

                    const dayEvents = (config.events || []).filter(e => {
                        const startD = e.start ? e.start.substring(0, 10) : '';
                        return startD === dateStr;
                    });

                    // ONLY display on days that have at least 1 booking AND at least 1 remaining open slot
                    if (dayEvents.length === 0) return;

                    const avail = checkDateAvailability(dateStr, dayEvents, config);
                    if (!avail.isFullyBooked && avail.reason === 'partial' && avail.remainingCount > 0) {
                        const pill = document.createElement('div');
                        pill.className = 'gfc-fc-partial-avail-pill';
                        
                        const count = avail.remainingCount || 1;
                        let badgeLabel = count === 1 
                            ? `${count} Slot Open` 
                            : `${count} Slots Open`;

                        if (config.partialAvailabilityBadgeText && config.partialAvailabilityBadgeText.trim() !== '' && config.partialAvailabilityBadgeText !== 'Slot Open') {
                            badgeLabel = config.partialAvailabilityBadgeText.replace('{count}', count);
                        }

                        pill.innerHTML = `<span class="gfc-fc-partial-avail-dot"></span><span class="text-truncate">${badgeLabel}</span>`;
                        pill.title = `Partial day availability: ${avail.remainingCount} slot(s) open for booking. Click to view schedule or book.`;
                        
                        pill.addEventListener('click', (ev) => {
                            ev.preventDefault();
                            ev.stopPropagation();
                            openDateModal(dateStr, containerId);
                        });

                        // Append to day cell frame
                        const frame = info.el.querySelector('.fc-daygrid-day-frame') || info.el;
                        frame.appendChild(pill);
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
            buildEvents(payload.events, payload).forEach(e => cal.addEvent(e));
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
