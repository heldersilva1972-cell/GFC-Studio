window.barSalesCharts = {
    charts: {},
    renderChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');

        // Define color palette for multiple years
        const colorPalette = [
            { border: '#2ecc71', bg: 'rgba(46, 204, 113, 0.7)' },
            { border: '#3b82f6', bg: 'rgba(59, 130, 246, 0.7)' },
            { border: '#f59e0b', bg: 'rgba(245, 158, 11, 0.7)' },
            { border: '#ef4444', bg: 'rgba(239, 68, 68, 0.7)' },
            { border: '#8b5cf6', bg: 'rgba(139, 92, 246, 0.7)' }
        ];

        // Build datasets
        const datasets = config.datasets.map((dataset, index) => {
            const colors = colorPalette[index % colorPalette.length];
            return {
                label: dataset.label,
                data: dataset.data,
                backgroundColor: colors.bg,
                borderColor: colors.border,
                borderWidth: 2,
                borderRadius: 8,
                borderSkipped: false,
            };
        });

        // In-place smooth update if chart already exists
        if (this.charts[canvasId]) {
            const chart = this.charts[canvasId];
            chart.data.labels = config.labels;

            const existingMap = new Map((chart.data.datasets || []).map(d => [d.label, d]));
            const updatedDatasets = datasets.map(newDs => {
                const existing = existingMap.get(newDs.label);
                if (existing) {
                    existing.data = newDs.data;
                    existing.backgroundColor = newDs.backgroundColor;
                    existing.borderColor = newDs.borderColor;
                    return existing;
                }
                return newDs;
            });

            chart.data.datasets = updatedDatasets;
            chart.update({
                duration: 500,
                easing: 'easeInOutCubic'
            });
            return;
        }

        const chartConfig = {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 750,
                    easing: 'easeInOutCubic'
                },
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                plugins: {
                    legend: {
                        display: config.datasets.length > 1,
                        position: 'top',
                        labels: {
                            font: { size: 12, weight: 'bold' },
                            padding: 15,
                            usePointStyle: true,
                            pointStyle: 'circle'
                        }
                    },
                    tooltip: {
                        filter: function (tooltipItem) {
                            return tooltipItem.parsed.y !== 0 && tooltipItem.parsed.y !== null && tooltipItem.parsed.y !== undefined;
                        },
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                label += '$' + context.parsed.y.toLocaleString();
                                return label;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function (value) {
                                return '$' + value.toLocaleString();
                            }
                        }
                    }
                }
            }
        };

        this.charts[canvasId] = new Chart(ctx, chartConfig);
    }
};
