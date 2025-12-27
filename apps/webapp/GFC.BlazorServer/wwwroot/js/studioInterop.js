// [MODIFIED]
window.studioInterop = {
    dotNetHelper: null,

    setDotNetHelper: function (helper) {
        this.dotNetHelper = helper;
    },

    initializeMessageHandler: function () {
        window.addEventListener('message', (event) => {
            if (event.data.type === 'SECTION_SELECTED' && this.dotNetHelper) {
                this.dotNetHelper.invokeMethodAsync('SetSelectedSection', event.data.section);
            }
            if (event.data.type === 'CURRENT_STATE' && this.dotNetHelper) {
                this.dotNetHelper.invokeMethodAsync('ReceiveStateFromPreview', event.data.payload);
            }
        });
    },

    sendComponentToPreview: function (iframeElement, component) {
        if (iframeElement && iframeElement.contentWindow) {
            const targetOrigin = new URL(iframeElement.src).origin;
            iframeElement.contentWindow.postMessage({
                type: 'ADD_COMPONENT',
                component: component
            }, targetOrigin);
        }
    },

    updateAnimationInPreview: function (iframeElement, keyframes) {
        if (iframeElement && iframeElement.contentWindow) {
            const targetOrigin = new URL(iframeElement.src).origin;
            iframeElement.contentWindow.postMessage({
                type: 'UPDATE_ANIMATION',
                payload: keyframes
            }, targetOrigin);
        }
    },

    updateStyleInPreview: function (iframeElement, sectionId, style) {
        if (iframeElement && iframeElement.contentWindow) {
            const targetOrigin = new URL(iframeElement.src).origin;
            iframeElement.contentWindow.postMessage({
                type: 'UPDATE_STYLE',
                sectionId: sectionId,
                style: style
            }, targetOrigin);
        }
    },

    requestStateFromPreview: function(iframeElement) {
        if (iframeElement && iframeElement.contentWindow) {
            const targetOrigin = new URL(iframeElement.src).origin;
            iframeElement.contentWindow.postMessage({
                type: 'REQUEST_STATE'
            }, targetOrigin);
        }
    }
};
