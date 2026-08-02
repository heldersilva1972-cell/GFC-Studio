// [NEW]
window.incomeCharts = {};
window.renderChart = (canvasId, chartData) => {
    const el = document.getElementById(canvasId);
    if (!el) return;
    
    if (window.incomeCharts[canvasId]) {
        window.incomeCharts[canvasId].destroy();
    }
    
    const ctx = el.getContext('2d');
    window.incomeCharts[canvasId] = new Chart(ctx, {
        type: chartData.type,
        data: chartData.data,
        options: chartData.options
    });
};
