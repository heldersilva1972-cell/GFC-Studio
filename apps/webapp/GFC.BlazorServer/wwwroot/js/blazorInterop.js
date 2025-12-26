window.blazorInterop = {
    saveTextAsFile: function (filename, content) {
        var blob = new Blob([content], { type: 'application/json' });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(link.href);
    }
};
