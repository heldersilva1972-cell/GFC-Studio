window.inventoryCharts = {
    charts: {},
    renderStockVsUsage: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');

        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [
                    {
                        label: 'Stock on Hand',
                        data: config.stockData,
                        backgroundColor: 'rgba(34, 197, 94, 0.75)', // green
                        borderColor: '#22c55e',
                        borderWidth: 1.5,
                        borderRadius: 6
                    },
                    {
                        label: 'Quantity Consumed (Used)',
                        data: config.usedData,
                        backgroundColor: 'rgba(239, 68, 68, 0.75)', // red
                        borderColor: '#ef4444',
                        borderWidth: 1.5,
                        borderRadius: 6
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'top' },
                    tooltip: { mode: 'index', intersect: false }
                },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    },
    renderStockLevels: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');

        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [
                    {
                        label: 'Current Stock',
                        data: config.stockData,
                        backgroundColor: 'rgba(59, 130, 246, 0.75)', // blue
                        borderColor: '#3b82f6',
                        borderWidth: 1.5,
                        borderRadius: 6
                    },
                    {
                        label: 'Quantity Consumed (Used)',
                        data: config.usedData,
                        backgroundColor: 'rgba(239, 68, 68, 0.75)', // red
                        borderColor: '#ef4444',
                        borderWidth: 1.5,
                        borderRadius: 6
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'top' },
                    tooltip: { mode: 'index', intersect: false }
                },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    },
    renderCategoryShare: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');

        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

        const colors = [
            '#3b82f6', // blue
            '#10b981', // green
            '#f59e0b', // amber
            '#ec4899', // pink
            '#8b5cf6', // purple
            '#ef4444', // red
            '#14b8a6', // teal
            '#f97316', // orange
            '#06b6d4', // cyan
            '#6b7280'  // gray
        ];

        this.charts[canvasId] = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: config.labels,
                datasets: [{
                    data: config.data,
                    backgroundColor: colors.slice(0, config.labels.length),
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            font: { size: 10, weight: 'bold' },
                            padding: 8,
                            boxWidth: 12,
                            generateLabels: function (chart) {
                                const data = chart.data;
                                if (data.labels.length && data.datasets.length) {
                                    return data.labels.map(function (label, i) {
                                        const meta = chart.getDatasetMeta(0);
                                        const val = data.datasets[0].data[i];
                                        const formattedVal = new Intl.NumberFormat('en-US', {
                                            style: 'currency',
                                            currency: 'USD'
                                        }).format(val);
                                        
                                        return {
                                            text: label + ': ' + formattedVal,
                                            fillStyle: data.datasets[0].backgroundColor[i],
                                            strokeStyle: '#fff',
                                            lineWidth: 1,
                                            hidden: isNaN(data.datasets[0].data[i]) || (meta.data[i] && meta.data[i].hidden),
                                            index: i
                                        };
                                    });
                                }
                                return [];
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const val = context.raw;
                                const formattedVal = new Intl.NumberFormat('en-US', {
                                    style: 'currency',
                                    currency: 'USD'
                                }).format(val);
                                return ' ' + context.label + ': ' + formattedVal;
                            }
                        }
                    }
                }
            }
        });
    },
    renderTrendLine: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');

        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

        this.charts[canvasId] = new Chart(ctx, {
            type: 'line',
            data: {
                labels: config.labels,
                datasets: [
                    {
                        label: 'Sales Revenue ($)',
                        data: config.salesData,
                        borderColor: '#3b82f6',
                        backgroundColor: 'rgba(59, 130, 246, 0.1)',
                        fill: true,
                        tension: 0.3,
                        yAxisID: 'y'
                    },
                    {
                        label: 'Cost of Inventory Used ($)',
                        data: config.costData,
                        borderColor: '#f43f5e',
                        backgroundColor: 'transparent',
                        tension: 0.3,
                        borderDash: [5, 5],
                        yAxisID: 'y1'
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'top' },
                    tooltip: { mode: 'index', intersect: false }
                },
                scales: {
                    y: {
                        type: 'linear',
                        display: true,
                        position: 'left',
                        title: { display: true, text: 'Sales ($)' }
                    },
                    y1: {
                        type: 'linear',
                        display: true,
                        position: 'right',
                        title: { display: true, text: 'Wholesale Cost ($)' },
                        grid: { drawOnChartArea: false }
                    }
                }
            }
        });
    }
};
