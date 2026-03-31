window.financialCharts = {
    charts: {},
    renderChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');
        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

        const colorPalette = [
            { border: '#2ecc71', bg: 'rgba(46, 204, 113, 0.7)' },
            { border: '#3b82f6', bg: 'rgba(59, 130, 246, 0.7)' },
            { border: '#f59e0b', bg: 'rgba(245, 158, 11, 0.7)' },
            { border: '#ef4444', bg: 'rgba(239, 68, 68, 0.7)' },
            { border: '#8b5cf6', bg: 'rgba(139, 92, 246, 0.7)' },
            { border: '#ec4899', bg: 'rgba(236, 72, 153, 0.7)' },
            { border: '#06b6d4', bg: 'rgba(6, 182, 212, 0.7)' }
        ];

        const datasets = config.datasets.map((dataset, index) => {
            const colors = dataset.color || colorPalette[index % colorPalette.length].border;
            const bgColor = dataset.bg || colorPalette[index % colorPalette.length].bg;

            return {
                type: dataset.type || config.type, // Use individual dataset type if provided
                label: dataset.label,
                data: dataset.data,
                backgroundColor: bgColor,
                borderColor: colors,
                borderWidth: 1,
                borderRadius: config.type === 'bar' ? 4 : 0,
                tension: 0.4,
                fill: false,
                pointRadius: config.type === 'line' ? 4 : 0,
                pointHoverRadius: 6,
                barPercentage: 0.9,
                categoryPercentage: 0.8
            };
        });

        this.charts[canvasId] = new Chart(ctx, {
            type: config.type || 'bar',
            data: {
                labels: config.labels,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    mode: 'index',
                    intersect: false
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            usePointStyle: true,
                            pointStyle: 'circle',
                            font: { weight: 'bold', size: 11 },
                            padding: 20
                        }
                    },
                    tooltip: {
                        padding: 12,
                        backgroundColor: 'rgba(0, 0, 0, 0.8)',
                        titleFont: { size: 14, weight: 'bold' },
                        bodyFont: { size: 13 },
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                label += new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(context.parsed.y);
                                return label;
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        stacked: false,
                        offset: true,
                        grid: { display: false },
                        ticks: { font: { size: 10 } }
                    },
                    y: {
                        stacked: false,
                        beginAtZero: true,
                        grid: { color: 'rgba(0,0,0,0.05)' },
                        ticks: {
                            callback: value => '$' + value.toLocaleString(),
                            font: { size: 10 }
                        }
                    }
                }
            }
        });
    }
};
