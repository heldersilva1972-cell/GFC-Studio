// [NEW]
window.renderChart = (canvasId, chartData) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    new Chart(ctx, {
        type: chartData.type,
        data: chartData.data,
        options: chartData.options
    });
};
