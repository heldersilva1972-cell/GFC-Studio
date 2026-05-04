window.barSalesCharts = {
    charts: {},
    renderChart: function (canvasId, config) {
        console.log('renderChart called with:', canvasId, config);
        console.log('Labels:', config.labels);
        console.log('Datasets:', config.datasets);

        const ctx = document.getElementById(canvasId).getContext('2d');

        // Destroy existing chart if it exists
        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

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

        const chartConfig = {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
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
                        backgroundColor: 'rgba(15, 23, 42, 0.95)',
                        titleFont: { size: 14, weight: 'bold' },
                        bodyFont: { size: 13 },
                        padding: 12,
                        displayColors: true,
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                if (context.parsed.y !== null) {
                                    label += new Intl.NumberFormat('en-US', {
                                        style: 'currency',
                                        currency: 'USD'
                                    }).format(context.parsed.y);
                                }
                                return label;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grace: '5%',
                        grid: {
                            color: 'rgba(0, 0, 0, 0.05)',
                            drawBorder: false
                        },
                        ticks: {
                            callback: function (value) {
                                return '$' + value.toLocaleString();
                            },
                            font: { size: 11 }
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        },
                        ticks: {
                            font: { size: 11 }
                        }
                    }
                }
            }
        };

        this.charts[canvasId] = new Chart(ctx, chartConfig);
    }
};
