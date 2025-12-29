// Studio Preview Communication
window.sendSectionsToPreview = function (iframeElement, sectionsJson) {
    if (!iframeElement || !iframeElement.contentWindow) {
        console.warn('Preview iframe not ready');
        return;
    }

    try {
        const sections = JSON.parse(sectionsJson);
        iframeElement.contentWindow.postMessage({
            type: 'UPDATE_SECTIONS',
            payload: sections
        }, 'http://localhost:3000');

        console.log('Sent sections to preview:', sections);
    } catch (error) {
        console.error('Failed to send sections to preview:', error);
    }
};

// Wait for preview iframe to be ready
window.waitForPreviewReady = function (iframeElement, callback) {
    const messageHandler = (event) => {
        if (event.origin !== 'http://localhost:3000') return;
        if (event.data.type === 'PREVIEW_READY') {
            console.log('Preview iframe is ready');
            window.removeEventListener('message', messageHandler);
            if (callback) callback();
        }
    };

    window.addEventListener('message', messageHandler);
};

// Studio Preview Namespace
window.studioPreview = {
    updateElementStyle: function (sectionId, property, value) {
        // Find the specific iframe - assuming class .studio-preview-iframe
        const iframe = document.querySelector('.studio-preview-iframe');
        if (iframe && iframe.contentWindow) {
            iframe.contentWindow.postMessage({
                type: 'STYLE_UPDATE',
                sectionId: sectionId,
                property: property,
                value: value
            }, '*'); // Allowing * for dev, ideally lock to specific origin in prod
        }
    }
};
