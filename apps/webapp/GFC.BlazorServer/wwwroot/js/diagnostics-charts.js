// [NEW]
window.incomeCharts = {};
window.renderChart = (canvasId, chartData, dotNetHelper) => {
    const el = document.getElementById(canvasId);
    if (!el) return;
    
    if (window.incomeCharts[canvasId]) {
        window.incomeCharts[canvasId].destroy();
    }
    
    if (dotNetHelper) {
        if (!chartData.options) chartData.options = {};
        chartData.options.onClick = (event, elements) => {
            if (elements && elements.length > 0) {
                const elementIndex = elements[0].index;
                const chart = window.incomeCharts[canvasId];
                const label = chart.data.labels[elementIndex];
                dotNetHelper.invokeMethodAsync('OnChartBarClicked', label);
            }
        };
    }
    
    const ctx = el.getContext('2d');
    window.incomeCharts[canvasId] = new Chart(ctx, {
        type: chartData.type,
        data: chartData.data,
        options: chartData.options
    });
};
