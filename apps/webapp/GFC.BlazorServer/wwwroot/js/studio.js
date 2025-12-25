// [NEW]
function updatePreview(sections) {
    const iframe = document.querySelector('.preview-iframe');
    if (iframe) {
        iframe.contentWindow.postMessage({ type: 'UPDATE_PREVIEW', sections: sections }, '*');
    }
}
