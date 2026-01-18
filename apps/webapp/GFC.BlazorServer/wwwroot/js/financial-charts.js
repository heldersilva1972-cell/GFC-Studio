window.financialCharts = {
    charts: {},
    renderChart: function (canvasId, config) {
        const ctx = document.getElementById(canvasId).getContext('2d');
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
                label: dataset.label,
                data: dataset.data,
                backgroundColor: bgColor,
                borderColor: colors,
                borderWidth: 2,
                borderRadius: config.type === 'bar' ? 4 : 0,
                tension: 0.4,
                fill: config.type === 'line',
                pointRadius: 4,
                pointHoverRadius: 6,
                stack: dataset.stack || null
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
                interaction: { mode: 'index', intersect: false },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: { usePointStyle: true, pointStyle: 'circle', font: { weight: 'bold' } }
                    },
                    tooltip: {
                        padding: 12,
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
                        stacked: config.stacked || false,
                        grid: { display: false }
                    },
                    y: {
                        stacked: config.stacked || false,
                        beginAtZero: true,
                        ticks: { callback: value => '$' + value.toLocaleString() }
                    }
                }
            }
        });
    }
};
