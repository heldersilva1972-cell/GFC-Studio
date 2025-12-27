// [MODIFIED]
window.studioInterop = {
    dotNetHelper: null,
    previewReady: false,
    _previewIframe: null,

    setDotNetHelper: function (helper) {
        this.dotNetHelper = helper;
    },

    initializeMessageHandler: function (previewIframe) {
        this._previewIframe = previewIframe;
        window.addEventListener('message', (event) => {
            // For security, you should check event.origin
            // if (event.origin !== new URL(this._previewIframe.src).origin) return;

            if (event.data.type === 'PREVIEW_READY') {
                this.previewReady = true;
                console.log("Preview iframe is ready.");
                if (this.dotNetHelper) {
                    this.dotNetHelper.invokeMethodAsync('HandlePreviewReady');
                }
            }
            else if (event.data.type === 'SECTION_SELECTED' && this.dotNetHelper) {
                this.dotNetHelper.invokeMethodAsync('HandleSectionSelected', event.data.section.clientId);
            }
            else if (event.data.type === 'CURRENT_STATE' && this.dotNetHelper) {
                this.dotNetHelper.invokeMethodAsync('ReceiveStateFromPreview', event.data.payload);
            }
        });
    },

    sendSectionsToPreview: function(iframeElement, sections) {
        if (iframeElement && iframeElement.contentWindow) {
            const targetOrigin = new URL(iframeElement.src).origin;
            iframeElement.contentWindow.postMessage({
                type: 'LOAD_SECTIONS',
                sections: sections
            }, targetOrigin);
            console.log("Sent sections to preview:", sections);
        }
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
