// [MODIFIED]
window.sessionMonitor = {
    dotNetRef: null,
    idleTimeout: 0,
    warningTimeout: 0,
    logoutTimeoutId: 0,
    warningTimeoutId: 0,

    init: function (dotNetReference, idleMinutes, warningMinutes) {
        this.dotNetRef = dotNetReference;
        this.idleTimeout = idleMinutes * 60 * 1000;
        this.warningTimeout = warningMinutes * 60 * 1000;

        this.addEventListeners();
        this.resetTimer();
    },

    resetTimer: function () {
        clearTimeout(this.warningTimeoutId);
        clearTimeout(this.logoutTimeoutId);

        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('ResetFromJavaScript');
            this.warningTimeoutId = setTimeout(() => this.showWarning(), this.warningTimeout);
            this.logoutTimeoutId = setTimeout(() => this.performLogout(), this.idleTimeout);
        }
    },

    showWarning: function () {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('ShowIdleWarning');
        }
    },

    performLogout: function () {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('LogoutUser');
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
