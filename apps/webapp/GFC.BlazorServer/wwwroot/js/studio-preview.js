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
        const iframe = document.querySelector('.studio-preview-iframe');
        if (iframe && iframe.contentWindow) {
            iframe.contentWindow.postMessage({
                type: 'STYLE_UPDATE',
                sectionId: sectionId,
                property: property,
                value: value
            }, '*');
        }
    },

    setupDragAndDrop: function (iframeElement) {
        if (!iframeElement || !iframeElement.contentDocument) return;

        const doc = iframeElement.contentDocument;
        let dropIndicator = null;

        const createIndicator = () => {
            const div = doc.createElement('div');
            div.style.position = 'absolute';
            div.style.height = '4px';
            div.style.backgroundColor = '#3b82f6';
            div.style.borderRadius = '2px';
            div.style.transition = 'all 0.1s ease';
            div.style.pointerEvents = 'none';
            div.style.zIndex = '9999';
            div.style.display = 'none';
            doc.body.appendChild(div);
            return div;
        };

        const handleDragOver = (e) => {
            e.preventDefault();
            e.dataTransfer.dropEffect = 'copy';

            if (!dropIndicator) dropIndicator = createIndicator();

            const elements = Array.from(doc.querySelectorAll('[data-studio-section]'));
            const y = e.clientY;

            let closest = null;
            let checkDistance = Infinity;
            let insertAfter = false;

            elements.forEach(el => {
                const box = el.getBoundingClientRect();
                const center = box.top + box.height / 2;
                const distance = Math.abs(y - center);

                if (distance < checkDistance) {
                    checkDistance = distance;
                    closest = el;
                    insertAfter = y > center;
                }
            });

            if (closest) {
                const box = closest.getBoundingClientRect();
                dropIndicator.style.display = 'block';
                dropIndicator.style.left = box.left + 'px';
                dropIndicator.style.width = box.width + 'px';
                dropIndicator.style.top = (insertAfter ? box.bottom : box.top) + 'px';
                doc.dropTarget = { closestId: closest.getAttribute('data-studio-section'), insertAfter: insertAfter };
            } else {
                dropIndicator.style.display = 'block';
                dropIndicator.style.left = '10px';
                dropIndicator.style.width = 'calc(100% - 20px)';
                dropIndicator.style.top = '10px';
                doc.dropTarget = { empty: true };
            }
        };

        const handleDrop = (e) => {
            e.preventDefault();
            if (dropIndicator) dropIndicator.style.display = 'none';

            const componentType = e.dataTransfer.getData('componentType');
            if (componentType && doc.dropTarget) {
                window.postMessage({
                    type: 'COMPONENT_DROP',
                    componentType: componentType,
                    ...doc.dropTarget
                }, '*');
            }
        };

        const handleDragLeave = () => {
            if (dropIndicator) dropIndicator.style.display = 'none';
        };

        doc.addEventListener('dragover', handleDragOver);
        doc.addEventListener('dragleave', handleDragLeave);
        doc.addEventListener('drop', handleDrop);
    },

    initDropListener: function (dotNetHelper) {
        window.dotNetHelper = dotNetHelper;

        // 1. Listen for Iframe Messages
        window.addEventListener('message', (event) => {
            if (event.data && event.data.type === 'COMPONENT_DROP') {
                dotNetHelper.invokeMethodAsync(
                    'HandleComponentDrop',
                    event.data.componentType,
                    event.data.closestId || "",
                    event.data.insertAfter || false
                );
            }
        });

        // 2. Setup Drop Zone on Main Window (for Blank Canvas)
        document.body.addEventListener('dragover', (e) => {
            if (e.target.closest('#studio-canvas-dropzone')) {
                e.preventDefault();
                e.dataTransfer.dropEffect = 'copy';
            }
        });

        document.body.addEventListener('drop', (e) => {
            const dropzone = e.target.closest('#studio-canvas-dropzone');
            if (dropzone) {
                e.preventDefault();
                const componentType = e.dataTransfer.getData('componentType');
                if (componentType) {
                    console.log('Dropped on Blank Canvas:', componentType);
                    dotNetHelper.invokeMethodAsync(
                        'HandleComponentDrop',
                        componentType,
                        "", // closestId (none)
                        false // insertAfter
                    );
                }
            }
        });
    },

    updateGlobalTheme: function (iframe, themeTokens) {
        if (!iframe || !iframe.contentWindow) return;
        const doc = iframe.contentDocument || iframe.contentWindow.document;

        let styleTag = doc.getElementById('studio-global-theme');
        if (!styleTag) {
            styleTag = doc.createElement('style');
            styleTag.id = 'studio-global-theme';
            doc.head.appendChild(styleTag);
        }

        let cssVars = ':root {\n';
        for (const [key, value] of Object.entries(themeTokens)) {
            if (key && value) {
                cssVars += `  --${key}: ${value};\n`;
            }
        }
        cssVars += '}';

        cssVars += `
            .btn-primary { background-color: var(--primary-color, #3b82f6) !important; color: white !important; }
            .text-primary { color: var(--primary-color, #3b82f6) !important; }
            h1, h2, h3, h4, h5, h6 { font-family: var(--font-heading, inherit) !important; }
            body { font-family: var(--font-body, inherit); }
        `;

        styleTag.textContent = cssVars;
    },

    attachInteractions: function (iframe, interactionsMap) {
        if (!iframe || !iframe.contentWindow) return;
        const doc = iframe.contentDocument || iframe.contentWindow.document;

        for (const [sectionId, interactions] of Object.entries(interactionsMap)) {
            const el = doc.querySelector(`[data-studio-id="${sectionId}"]`);
            if (el) {
                interactions.forEach(i => {
                    if (i.Trigger === 'click') {
                        el.style.cursor = 'pointer';
                        el.addEventListener('click', (e) => {
                            e.preventDefault();
                            e.stopPropagation();

                            if (i.Action === 'navigate') {
                                alert(`[Designer Mode] Navigation to: ${i.Parameters['url']}`);
                            }
                            else if (i.Action === 'alert') {
                                alert('Interaction Alert!');
                            }
                            else if (i.Action === 'toggleVisibility') {
                                el.style.opacity = el.style.opacity === '0.5' ? '1' : '0.5';
                            }
                        });
                    }
                });
            }
        }
    },

    applyDataBindings: function (iframe, updatesMap) {
        if (!iframe || !iframe.contentWindow) return;
        const doc = iframe.contentDocument || iframe.contentWindow.document;

        for (const [sectionId, props] of Object.entries(updatesMap)) {
            const el = doc.querySelector(`[data-studio-id="${sectionId}"]`);
            if (el) {
                for (const [prop, value] of Object.entries(props)) {
                    if (prop === 'content') {
                        el.innerHTML = value;
                    }
                    else if (prop === 'src' && el.tagName === 'IMG') {
                        el.src = value;
                    }
                    else {
                        el.setAttribute(prop, value);
                    }
                }
            }
        }
    }
};

// GLOBAL DRAG START LISTENER
window.addEventListener('dragstart', (e) => {
    const card = e.target.closest('[data-comp-type]');
    if (card) {
        const type = card.getAttribute('data-comp-type');
        e.dataTransfer.setData('componentType', type);
        e.dataTransfer.effectAllowed = 'copy';

        // CRITICAL: Notify Blazor backend immediately so 'draggedComponent' is set
        if (window.dotNetHelper) {
            window.dotNetHelper.invokeMethodAsync('SetDraggedComponentType', type);
        }
    }
});

// Auto-init drag and drop when iframe loads
window.addEventListener('load', () => {
    const checkIframe = setInterval(() => {
        const iframe = document.querySelector('.studio-preview-iframe');
        if (iframe && iframe.contentDocument && iframe.contentDocument.body) {
            clearInterval(checkIframe);
            window.studioPreview.setupDragAndDrop(iframe);
        }
    }, 1000);
});
