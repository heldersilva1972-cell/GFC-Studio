// [NEW]
window.initializeTimeline = (timelineContainer, dotNetHelper) => {
    const blocks = timelineContainer.querySelectorAll('.animation-block');

    blocks.forEach(block => {
        let isResizing = false;
        let isDragging = false;
        let startX, startWidth, startLeft;

        const onMouseMove = (e) => {
            if (isResizing) {
                const dx = e.clientX - startX;
                const newWidth = startWidth + dx;
                block.style.width = `${newWidth}px`;
            } else if (isDragging) {
                const dx = e.clientX - startX;
                const newLeft = startLeft + dx;
                block.style.left = `${newLeft}px`;
            }
        };

        const onMouseUp = () => {
            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('mouseup', onMouseUp);

            const newWidth = parseFloat(block.style.width);
            const newLeft = parseFloat(block.style.left);

            // Convert back to percentage for Blazor
            const timelineWidth = timelineContainer.offsetWidth;
            const newDuration = (newWidth / timelineWidth) * 5; // 5s total time
            const newDelay = (newLeft / timelineWidth) * 5;

            dotNetHelper.invokeMethodAsync('UpdateKeyframe', block.id, newDuration, newDelay);
            isResizing = false;
            isDragging = false;
        };

        block.addEventListener('mousedown', (e) => {
            startX = e.clientX;

            if (e.offsetX > block.offsetWidth - 10) {
                isResizing = true;
                startWidth = block.offsetWidth;
            } else {
                isDragging = true;
                startLeft = block.offsetLeft;
            }

            document.addEventListener('mousemove', onMouseMove);
            document.addEventListener('mouseup', onMouseUp);
        });
    });
};
