
(function () {
    let timeout;
    let dotNetHelper;

    window.idleTimer = {
        initialize: function (helper, timeoutMs) {
            dotNetHelper = helper;
            this.resetTimer(timeoutMs);

            const events = ['mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart', 'click'];
            events.forEach(name => {
                document.addEventListener(name, () => this.resetTimer(timeoutMs), true);
            });
            
            console.log("[IDLE] Timer initialized for " + (timeoutMs / 60000) + " minutes.");
        },
        resetTimer: function (timeoutMs) {
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                if (dotNetHelper) {
                    console.log("[IDLE] Inactivity threshold reached. Notifying app...");
                    dotNetHelper.invokeMethodAsync('OnIdleTimeout');
                }
            }, timeoutMs);
        }
    };
})();
