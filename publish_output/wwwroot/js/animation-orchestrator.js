// [MODIFIED]
(function() {
    let dotNetHelper = null;
    let timelineContainer = null;
    let playhead = null;
    let layersContainer = null;
    let isDraggingPlayhead = false;

    let activeMotionBar = null;
    let isDraggingBar = false;
    let isResizingBar = false;
    let dragStartX = 0;
    let originalLeft = 0;
    let originalWidth = 0;

    const SNAP_INTERVAL = 0.1; // 100ms

    function getTimelineWidth() {
        return layersContainer.offsetWidth;
    }

    function pxToSeconds(px) {
        return (px / getTimelineWidth()) * 10.0; // 10 seconds total
    }

    function secondsToPx(seconds) {
        return (seconds / 10.0) * getTimelineWidth();
    }

    function handlePlayheadMouseDown(e) {
        isDraggingPlayhead = true;
        playhead.classList.add('dragging');
        document.body.style.cursor = 'ew-resize';
    }

    function handleLayersContainerMouseDown(e) {
        const motionBar = e.target.closest('.motion-bar');
        if (!motionBar) return;

        activeMotionBar = motionBar;
        dragStartX = e.clientX;

        const rect = activeMotionBar.getBoundingClientRect();
        originalLeft = activeMotionBar.offsetLeft;
        originalWidth = rect.width;

        if (e.clientX > rect.left + rect.width - 10) {
            isResizingBar = true;
            document.body.style.cursor = 'ew-resize';
        } else {
            isDraggingBar = true;
            document.body.style.cursor = 'grabbing';
        }
    }

    function handleDocumentMouseMove(e) {
        if (isDraggingPlayhead) {
            const rect = timelineContainer.querySelector('.time-ruler').getBoundingClientRect();
            let newX = e.clientX - rect.left;
            newX = Math.max(0, Math.min(newX, rect.width));

            const time = pxToSeconds(newX);
            const snappedTime = Math.round(time / SNAP_INTERVAL) * SNAP_INTERVAL;
            const snappedX = secondsToPx(snappedTime);

            playhead.style.left = `${snappedX}px`;
            dotNetHelper.invokeMethodAsync('SetCurrentTime', snappedTime);
        } else if (isDraggingBar) {
            const deltaX = e.clientX - dragStartX;
            const newLeftPx = Math.max(0, originalLeft + deltaX);
            const newDelay = pxToSeconds(newLeftPx);
            const snappedDelay = Math.round(newDelay / SNAP_INTERVAL) * SNAP_INTERVAL;

            activeMotionBar.style.left = `${secondsToPx(snappedDelay)}px`;
        } else if (isResizingBar) {
            const deltaX = e.clientX - dragStartX;
            const newWidthPx = Math.max(10, originalWidth + deltaX); // Min width
            const newDuration = pxToSeconds(newWidthPx);
            const snappedDuration = Math.round(newDuration / SNAP_INTERVAL) * SNAP_INTERVAL;

            activeMotionBar.style.width = `${secondsToPx(snappedDuration)}px`;
        }
    }

    function handleDocumentMouseUp(e) {
        if (isDraggingPlayhead) {
            isDraggingPlayhead = false;
            playhead.classList.remove('dragging');
        }

        if (isDraggingBar || isResizingBar) {
            const keyframeId = activeMotionBar.dataset.id;
            const newDelay = pxToSeconds(activeMotionBar.offsetLeft);
            const newDuration = pxToSeconds(activeMotionBar.offsetWidth);

            dotNetHelper.invokeMethodAsync('UpdateKeyframeTiming', keyframeId, newDelay, newDuration);
        }

        isDraggingBar = false;
        isResizingBar = false;
        document.body.style.cursor = '';
        activeMotionBar = null;
    }

    window.initializeAnimationOrchestrator = (container, ph, helper) => {
        timelineContainer = container;
        playhead = ph;
        dotNetHelper = helper;
        layersContainer = timelineContainer.querySelector('.layers-container');

        playhead.addEventListener('mousedown', handlePlayheadMouseDown);
        layersContainer.addEventListener('mousedown', handleLayersContainerMouseDown);
        document.addEventListener('mousemove', handleDocumentMouseMove);
        document.addEventListener('mouseup', handleDocumentMouseUp);
    };

    window.destroyAnimationOrchestrator = () => {
        if (playhead) playhead.removeEventListener('mousedown', handlePlayheadMouseDown);
        if (layersContainer) layersContainer.removeEventListener('mousedown', handleLayersContainerMouseDown);
        document.removeEventListener('mousemove', handleDocumentMouseMove);
        document.removeEventListener('mouseup', handleDocumentMouseUp);

        dotNetHelper = null;
        timelineContainer = null;
        playhead = null;
        layersContainer = null;
    };
})();
