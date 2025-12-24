// [NEW]
var studioInterop = {};

studioInterop.initializeDnd = function (dotNetHelper) {
    var iframe = document.querySelector('.center-canvas iframe');
    if (!iframe) {
        console.error("Studio iframe not found.");
        return;
    }

    var lastDragEnter = null;

    iframe.contentWindow.addEventListener('dragover', function (e) {
        e.preventDefault();
        lastDragEnter = e.target;
        // You could add drop zone indicators here
    }, false);

    iframe.contentWindow.addEventListener('dragleave', function (e) {
        if (lastDragEnter === e.target) {
            // Remove drop zone indicators here
        }
    }, false);

    iframe.contentWindow.addEventListener('drop', function (e) {
        e.preventDefault();
        dotNetHelper.invokeMethodAsync('HandleComponentDrop');
    }, false);
};

studioInterop.initializeComponentSelection = function (dotNetHelper) {
    var iframe = document.querySelector('.center-canvas iframe');
    if (!iframe) {
        return;
    }

    iframe.contentWindow.document.body.addEventListener('click', function (e) {
        var componentElement = e.target.closest('[data-component-id]');
        if (componentElement) {
            var componentId = componentElement.getAttribute('data-component-id');
            dotNetHelper.invokeMethodAsync('SelectComponent', componentId);
        }
    });
}

studioInterop.refreshPreview = function() {
    var iframe = document.querySelector('.center-canvas iframe');
    if (iframe) {
        iframe.src = iframe.src; // Simple refresh
    }
}

window.studioInterop = studioInterop;
