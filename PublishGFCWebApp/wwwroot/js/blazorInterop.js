window.blazorInterop = {
    saveTextAsFile: function (filename, content) {
        var blob = new Blob([content], { type: 'application/json' });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(link.href);
    },
    downloadFile: function (filename, contentType, content) {
        var blob = new Blob([content], { type: contentType });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(link.href);
    },
    registerKeyListener: function (dotNetHelper) {
        window.blazorInterop_dotNetHelper = dotNetHelper;
        window.blazorInterop_keyListener = function (e) {
            // Ignore if user is typing in an input
            if (e.target.tagName === 'INPUT' || e.target.tagName === 'TEXTAREA' || e.target.isContentEditable) {
                return;
            }
            if (e.key.length === 1 || e.key === 'Enter') {
                dotNetHelper.invokeMethodAsync('HandleGlobalKeyPress', e.key);
            }
        };
        document.addEventListener('keydown', window.blazorInterop_keyListener);
    },
    unregisterKeyListener: function () {
        if (window.blazorInterop_keyListener) {
            document.removeEventListener('keydown', window.blazorInterop_keyListener);
            window.blazorInterop_keyListener = null;
            window.blazorInterop_dotNetHelper = null;
        }
    }
};
