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
            }, '*');
        }
    },

    setupDragAndDrop: function (iframeElement) {
        if (!iframeElement || !iframeElement.contentDocument) return;

        const doc = iframeElement.contentDocument;
        let dropIndicator = null;

        // Create Drop Indicator
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

                // Store drop target info on the doc
                doc.dropTarget = { closestId: closest.getAttribute('data-studio-section'), insertAfter: insertAfter };
            } else {
                // Empty canvas case
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
                // Send message back to Blazor
                window.postMessage({
                    type: 'COMPONENT_DROP',
                    componentType: componentType,
                    ...doc.dropTarget
                }, '*'); // Dev origin
            }
        };

        const handleDragLeave = () => {
            if (dropIndicator) dropIndicator.style.display = 'none';
        };

        doc.addEventListener('dragover', handleDragOver);
        doc.addEventListener('dragleave', handleDragLeave);
        doc.addEventListener('drop', handleDrop);
        console.log("Drag and Drop Initialized in Preview Iframe");
    }
};

// Animation Orchestrator Interop
window.initializeAnimationOrchestrator = function (container, playhead, dotNetHelper) {
    let isDragging = false;

    container.addEventListener('mousedown', (e) => {
        const rect = container.getBoundingClientRect();
        // Check if clicking on the ruler area (top 30px)
        if (e.clientY - rect.top < 30) {
            isDragging = true;
            updatePlayhead(e);
        }
    });

    window.addEventListener('mousemove', (e) => {
        if (isDragging) {
            updatePlayhead(e);
        }
    });

    window.addEventListener('mouseup', () => {
        isDragging = false;
    });

    function updatePlayhead(e) {
        const rect = container.getBoundingClientRect();
        const timelineWidth = rect.width;
        let x = e.clientX - rect.left;

        // Clamp
        if (x < 0) x = 0;
        if (x > timelineWidth) x = timelineWidth;

        const percentage = (x / timelineWidth) * 100;
        const time = (percentage / 100) * 10.0; // Assuming 10s timeline

        playhead.style.left = percentage + '%';
        dotNetHelper.invokeMethodAsync('SetCurrentTime', time);
    }
};

window.destroyAnimationOrchestrator = function () {
    // Cleanup if necessary
};

// Auto-init drag and drop when iframe loads
window.addEventListener('load', () => {
    // Poll for iframe
    const checkIframe = setInterval(() => {
        const iframe = document.querySelector('.studio-preview-iframe');
        if (iframe && iframe.contentDocument && iframe.contentDocument.body) {
            clearInterval(checkIframe);
            window.studioPreview.setupDragAndDrop(iframe);
        }
    }, 1000);
});

// Listener for Blazor Interop
window.studioPreview.initDropListener = function (dotNetHelper) {
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
};

window.studioPreview.updateGlobalTheme = function (iframe, themeTokens) {
    if (!iframe || !iframe.contentWindow) return;
    const doc = iframe.contentDocument || iframe.contentWindow.document;

    // Find or create the theme style tag
    let styleTag = doc.getElementById('studio-global-theme');
    if (!styleTag) {
        styleTag = doc.createElement('style');
        styleTag.id = 'studio-global-theme';
        doc.head.appendChild(styleTag);
    }

    // Build CSS Variables
    let cssVars = ':root {\n';
    for (const [key, value] of Object.entries(themeTokens)) {
        if (key && value) {
            cssVars += `  --${key}: ${value};\n`;
        }
    }
    cssVars += '}';

    // Append generic utility classes that use these tokens
    cssVars += `
        .btn-primary { background-color: var(--primary-color, #3b82f6) !important; color: white !important; }
        .text-primary { color: var(--primary-color, #3b82f6) !important; }
        h1, h2, h3, h4, h5, h6 { font-family: var(--font-heading, inherit) !important; }
        body { font-family: var(--font-body, inherit); }
    `;

    styleTag.textContent = cssVars;
};

// Feature 4: Interaction Builder Runtime
window.studioPreview.attachInteractions = function (iframe, interactionsMap) {
    if (!iframe || !iframe.contentWindow) return;
    const doc = iframe.contentDocument || iframe.contentWindow.document;

    // interactionsMap is expected to be { "sectionId": [ { trigger, action, params } ] }
    for (const [sectionId, interactions] of Object.entries(interactionsMap)) {
        // Find element by data-studio-id (which maps to our ClientId or Id)
        // We need to ensure the Blazor renderer puts this attribute on elements.
        const el = doc.querySelector(`[data-studio-id="${sectionId}"]`);

        if (el) {
            // Clear existing listeners (crudely, by cloning) - in prod, use a better event manager
            // For prototype, we just append current ones.

            interactions.forEach(i => {
                if (i.Trigger === 'click') {
                    el.style.cursor = 'pointer';
                    el.addEventListener('click', (e) => {
                        e.preventDefault(); // Stop default link behavior in designer
                        e.stopPropagation();

                        // Execute Action
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
                else if (i.Trigger === 'hover') {
                    el.addEventListener('mouseenter', () => {
                        if (i.Action === 'toggleVisibility') el.style.opacity = '0.5';
                    });
                    el.addEventListener('mouseleave', () => {
                        if (i.Action === 'toggleVisibility') el.style.opacity = '1';
                    });
                }
            });
        }
    }
};
