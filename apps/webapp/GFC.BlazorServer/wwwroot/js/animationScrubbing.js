// [NEW]
window.animationScrubbing = {
    initializeScrubbing: (previewIframe) => {
        const timeline = document.getElementById('timeline-track');
        const playhead = document.getElementById('playhead');

        if (!timeline || !playhead || !previewIframe) return;

        let isScrubbing = false;

        const handleMouseDown = (e) => {
            isScrubbing = true;
            document.addEventListener('mousemove', handleMouseMove);
            document.addEventListener('mouseup', handleMouseUp);
            updatePlayhead(e.clientX);
        };

        const handleMouseMove = (e) => {
            if (isScrubbing) {
                updatePlayhead(e.clientX);
            }
        };

        const handleMouseUp = () => {
            isScrubbing = false;
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);
        };

        const updatePlayhead = (clientX) => {
            const timelineRect = timeline.getBoundingClientRect();
            const newLeft = Math.max(0, Math.min(clientX - timelineRect.left, timelineRect.width));
            playhead.style.left = `${newLeft}px`;

            const scrubPosition = newLeft / timelineRect.width; // 0 to 1

            previewIframe.contentWindow.postMessage({
                type: 'ANIMATION_SCRUB',
                position: scrubPosition
            }, '*'); // In a real app, use a specific target origin
        };

        timeline.addEventListener('mousedown', handleMouseDown);
    }
};
