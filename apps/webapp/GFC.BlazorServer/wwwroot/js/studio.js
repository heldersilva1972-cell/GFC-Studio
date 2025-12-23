// [NEW]
function previewAnimation(elementId, animation) {
    const element = document.querySelector(`[data-id="${elementId}"]`);
    if (element) {
        const animationName = animation.effect;
        const duration = animation.duration;
        const delay = animation.delay;

        // Reset animation
        element.style.animation = 'none';
        // Force reflow
        element.offsetHeight;

        // Apply new animation
        element.style.animation = `${animationName} ${duration}s ${delay}s ease-in-out`;
    }
}
