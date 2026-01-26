// [MODIFIED]
window.sessionMonitor = {
    isWarningActive: false,
    lastActivityTime: 0,

    init: function (dotNetReference, idleMinutes, warningMinutes) {
        this.dotNetRef = dotNetReference;
        this.idleTimeout = idleMinutes * 60 * 1000;
        this.warningTimeout = warningMinutes * 60 * 1000;
        this.isWarningActive = false;
        this.lastActivityTime = Date.now();

        this.addEventListeners();
        this.startTimers();
    },

    startTimers: function () {
        clearTimeout(this.warningTimeoutId);
        clearTimeout(this.logoutTimeoutId);

        this.warningTimeoutId = setTimeout(() => this.showWarning(), this.warningTimeout);
        this.logoutTimeoutId = setTimeout(() => this.performLogout(), this.idleTimeout);
    },

    resetTimer: function () {
        // Throttle activity resets to once every 2 seconds unless warning is active
        const now = Date.now();
        if (!this.isWarningActive && (now - this.lastActivityTime < 2000)) {
            return;
        }
        this.lastActivityTime = now;

        clearTimeout(this.warningTimeoutId);
        clearTimeout(this.logoutTimeoutId);

        if (this.isWarningActive) {
            this.isWarningActive = false;
            if (this.dotNetRef) {
                this.dotNetRef.invokeMethodAsync('ResetFromJavaScript').catch(err => console.log("Session ignored: Ref disposed"));
            }
        }

        this.warningTimeoutId = setTimeout(() => this.showWarning(), this.warningTimeout);
        this.logoutTimeoutId = setTimeout(() => this.performLogout(), this.idleTimeout);
    },

    showWarning: function () {
        if (this.dotNetRef) {
            this.isWarningActive = true;
            this.dotNetRef.invokeMethodAsync('ShowIdleWarning').catch(err => console.log("Session warning ignored: Ref disposed"));
        }
    },

    performLogout: function () {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('LogoutUser').catch(err => console.log("Logout ignored: Ref disposed"));
            this.cleanup();
        }
    },

    addEventListeners: function () {
        this.boundResetTimer = this.resetTimer.bind(this);
        document.addEventListener('mousemove', this.boundResetTimer);
        document.addEventListener('keydown', this.boundResetTimer);
        document.addEventListener('click', this.boundResetTimer);
        document.addEventListener('scroll', this.boundResetTimer);
    },

    cleanup: function () {
        if (this.boundResetTimer) {
            document.removeEventListener('mousemove', this.boundResetTimer);
            document.removeEventListener('keydown', this.boundResetTimer);
            document.removeEventListener('click', this.boundResetTimer);
            document.removeEventListener('scroll', this.boundResetTimer);
        }
        clearTimeout(this.warningTimeoutId);
        clearTimeout(this.logoutTimeoutId);
        if (this.dotNetRef) {
            try {
                this.dotNetRef.dispose();
            } catch (e) {
                // Ignore errors on dispose, it might have already been disposed.
            }
            this.dotNetRef = null;
        }
    },

    hideWarningModal: function () {
        var modalElement = document.getElementById('sessionWarningModal');
        if (modalElement) {
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }
        }
    },

    showWarningModal: function () {
        var modalElement = document.getElementById('sessionWarningModal');
        if (modalElement) {
            var modal = new bootstrap.Modal(modalElement, {
                backdrop: 'static',
                keyboard: false
            });
            modal.show();
        }
    }
};
