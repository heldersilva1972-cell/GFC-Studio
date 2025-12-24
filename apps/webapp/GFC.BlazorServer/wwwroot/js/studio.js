// [MODIFIED]
function previewAnimation(elementId, animationKeyframes) {
    const iframe = document.querySelector('.preview-iframe'); // Assuming the iframe has this class
    if (iframe) {
        const message = {
            type: 'ANIMATION_PREVIEW',
            payload: {
                elementId: elementId,
                keyframes: animationKeyframes
            }
        };
        // Target the specific iframe's content window
        iframe.contentWindow.postMessage(message, 'http://localhost:3000');
    }
}
