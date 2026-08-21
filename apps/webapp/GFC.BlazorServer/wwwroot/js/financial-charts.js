window.financialCharts = {
    charts: {},
    renderChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;
        const ctx = el.getContext('2d');

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
                type: dataset.type || config.type,
                label: dataset.label,
                data: dataset.data,
                backgroundColor: bgColor,
                borderColor: colors,
                borderWidth: 1.5,
                borderRadius: config.type === 'bar' ? 6 : 0,
                tension: 0.4,
                fill: false,
                pointRadius: config.type === 'line' ? 4 : 0,
                pointHoverRadius: 6,
                barPercentage: 0.9,
                categoryPercentage: 0.8,
                stack: dataset.stack || undefined
            };
        });

        const isStacked = config.datasets.some(d => d.stack);

        // If chart instance already exists, animate data value transitions smoothly
        if (this.charts[canvasId]) {
            const chart = this.charts[canvasId];
            chart.data.labels = config.labels;

            const activeLabels = datasets.map(d => d.label);

            // 1. Unchecked datasets: set values to 0 so Chart.js smoothly animates bars shrinking down to height 0
            chart.data.datasets.forEach((existingDs) => {
                if (!activeLabels.includes(existingDs.label)) {
                    existingDs.data = existingDs.data.map(() => 0);
                }
            });

            // 2. Active datasets: update values in-place so bars smoothly adjust height
            datasets.forEach((newDs) => {
                const existingDs = chart.data.datasets.find(d => d.label === newDs.label);
                if (existingDs) {
                    existingDs.data = newDs.data;
                    existingDs.backgroundColor = newDs.backgroundColor;
                    existingDs.borderColor = newDs.borderColor;
                } else {
                    // Start new dataset at 0 so it smoothly grows up from ground
                    const zeroStart = { ...newDs, data: newDs.data.map(() => 0) };
                    chart.data.datasets.push(zeroStart);
                    setTimeout(() => {
                        if (this.charts[canvasId]) {
                            const ds = this.charts[canvasId].data.datasets.find(d => d.label === newDs.label);
                            if (ds) {
                                ds.data = newDs.data;
                                this.charts[canvasId].update({ duration: 600, easing: 'easeOutQuart' });
                            }
                        }
                    }, 30);
                }
            });

            if (chart.options.scales && chart.options.scales.x) chart.options.scales.x.stacked = isStacked;
            if (chart.options.scales && chart.options.scales.y) chart.options.scales.y.stacked = isStacked;

            chart.update({
                duration: 650,
                easing: 'easeInOutCubic'
            });

            // 3. Cleanup zeroed-out datasets after shrink animation finishes
            setTimeout(() => {
                if (this.charts[canvasId]) {
                    const activeSet = new Set(activeLabels);
                    this.charts[canvasId].data.datasets = this.charts[canvasId].data.datasets.filter(
                        d => activeSet.has(d.label)
                    );
                    this.charts[canvasId].update('none');
                }
            }, 700);

            return;
        }

        // Create new chart instance with smooth initial entrance
        this.charts[canvasId] = new Chart(ctx, {
            type: config.type || 'bar',
            data: {
                labels: config.labels,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 650,
                    easing: 'easeInOutCubic'
                },
                transitions: {
                    active: {
                        animation: {
                            duration: 650,
                            easing: 'easeInOutCubic'
                        }
                    }
                },
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
                        stacked: isStacked,
                        offset: true,
                        grid: { display: false },
                        ticks: { font: { size: 10 } }
                    },
                    y: {
                        stacked: isStacked,
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
