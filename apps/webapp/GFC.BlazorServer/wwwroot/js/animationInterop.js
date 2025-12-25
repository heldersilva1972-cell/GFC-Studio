// [NEW]
window.animationInterop = {
    initializeTimeline: (dotNetHelper) => {
        const timeline = document.getElementById('timeline-track');
        const animationBar = document.getElementById('animation-bar');

        if (!timeline || !animationBar) return;

        let isDragging = false;
        let isResizingLeft = false;
        let isResizingRight = false;
        let startX, startLeftPercent, startWidthPercent;

        const handleMouseDown = (e) => {
            e.preventDefault();
            if (e.target.classList.contains('left')) isResizingLeft = true;
            else if (e.target.classList.contains('right')) isResizingRight = true;
            else isDragging = true;

            startX = e.clientX;
            const timelineWidth = timeline.offsetWidth;
            startLeftPercent = (animationBar.offsetLeft / timelineWidth) * 100;
            startWidthPercent = (animationBar.offsetWidth / timelineWidth) * 100;

            document.addEventListener('mousemove', handleMouseMove);
            document.addEventListener('mouseup', handleMouseUp);
        };

        const handleMouseMove = (e) => {
            const timelineWidth = timeline.offsetWidth;
            const dxPercent = ((e.clientX - startX) / timelineWidth) * 100;

            if (isDragging) {
                const newLeftPercent = Math.max(0, Math.min(startLeftPercent + dxPercent, 100 - startWidthPercent));
                animationBar.style.left = `${newLeftPercent}%`;
            } else if (isResizingLeft) {
                const newLeftPercent = Math.max(0, Math.min(startLeftPercent + dxPercent, startLeftPercent + startWidthPercent));
                const newWidthPercent = startWidthPercent - (newLeftPercent - startLeftPercent);
                animationBar.style.left = `${newLeftPercent}%`;
                animationBar.style.width = `${newWidthPercent}%`;
            } else if (isResizingRight) {
                const newWidthPercent = Math.max(0, Math.min(startWidthPercent + dxPercent, 100 - startLeftPercent));
                animationBar.style.width = `${newWidthPercent}%`;
            }
        };

        const handleMouseUp = () => {
            isDragging = isResizingLeft = isResizingRight = false;
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);

            const delay = (parseFloat(animationBar.style.left) / 100) * 5;
            const duration = (parseFloat(animationBar.style.width) / 100) * 5;

            dotNetHelper.invokeMethodAsync('UpdateAnimationTimeline', delay, duration);
        };

        animationBar.addEventListener('mousedown', handleMouseDown);
    }
};
