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
    
    if (!chartData.options) chartData.options = {};
    if (!chartData.options.plugins) chartData.options.plugins = {};
    
    const existingTooltip = chartData.options.plugins.tooltip || {};
    chartData.options.plugins.tooltip = Object.assign({
        padding: 12,
        backgroundColor: 'rgba(0, 0, 0, 0.85)',
        titleFont: { size: 14, weight: 'bold' },
        bodyFont: { size: 13 },
        footerFont: { size: 13, weight: 'bold' },
        footerColor: '#10b981',
        filter: function (tooltipItem) {
            return tooltipItem.parsed.y !== 0;
        },
        callbacks: {
            label: function (context) {
                let label = context.dataset.label || '';
                if (label) label += ': ';
                let val = context.parsed.y !== undefined ? context.parsed.y : context.raw;
                if (typeof val === 'number') {
                    const lLower = (context.dataset.label || '').toLowerCase();
                    const isUnit = lLower.includes('unit') || lLower.includes('count') || lLower.includes('qty') || lLower.includes('sold') || lLower.includes('volume') || lLower.includes('item') || context.dataset.yAxisID === 'y1';
                    if (isUnit) {
                        label += val.toLocaleString();
                    } else {
                        label += new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(val);
                    }
                } else {
                    label += val;
                }
                return label;
            },
            footer: function (tooltipItems) {
                let total = 0;
                let currencyCount = 0;
                let hasUnits = false;
                tooltipItems.forEach(function (item) {
                    const lLower = (item.dataset.label || '').toLowerCase();
                    const isUnit = lLower.includes('unit') || lLower.includes('count') || lLower.includes('qty') || lLower.includes('sold') || lLower.includes('volume') || lLower.includes('item') || item.dataset.yAxisID === 'y1';
                    let val = item.parsed.y !== undefined ? item.parsed.y : item.raw;
                    if (typeof val === 'number') {
                        if (isUnit) {
                            hasUnits = true;
                        } else {
                            total += val;
                            currencyCount++;
                        }
                    }
                });
                if (currencyCount > 1 && !hasUnits) {
                    const formatted = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(total);
                    return 'Total: ' + formatted;
                }
                return '';
            }
        }
    }, existingTooltip);
    
    const ctx = el.getContext('2d');
    window.incomeCharts[canvasId] = new Chart(ctx, {
        type: chartData.type,
        data: chartData.data,
        options: chartData.options
    });
};
