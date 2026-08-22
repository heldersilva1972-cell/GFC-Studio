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
                stack: dataset.stack || undefined,
                hidden: !!dataset.hidden
            };
        });

        const isStacked = config.datasets.some(d => d.stack);

        // If existing chart instance is pointing to a detached or replaced canvas element, destroy it first
        if (this.charts[canvasId]) {
            if (!document.body.contains(this.charts[canvasId].canvas) || this.charts[canvasId].canvas !== el) {
                try { this.charts[canvasId].destroy(); } catch (e) {}
                delete this.charts[canvasId];
            }
        }

        // If chart instance already exists, update data smoothly in 1 unified animation pass
        if (this.charts[canvasId]) {
            const chart = this.charts[canvasId];
            chart.data.labels = config.labels;

            if (chart.options.scales && chart.options.scales.x) chart.options.scales.x.stacked = isStacked;
            if (chart.options.scales && chart.options.scales.y) chart.options.scales.y.stacked = isStacked;

            for (let i = 0; i < datasets.length; i++) {
                if (chart.data.datasets[i]) {
                    chart.data.datasets[i].data = datasets[i].data;
                    chart.data.datasets[i].backgroundColor = datasets[i].backgroundColor;
                    chart.data.datasets[i].borderColor = datasets[i].borderColor;
                    chart.data.datasets[i].stack = datasets[i].stack;
                    chart.setDatasetVisibility(i, !datasets[i].hidden);
                } else {
                    chart.data.datasets[i] = datasets[i];
                    if (datasets[i].hidden !== undefined) {
                        chart.setDatasetVisibility(i, !datasets[i].hidden);
                    }
                }
            }
            if (chart.data.datasets.length > datasets.length) {
                chart.data.datasets.length = datasets.length;
            }

            chart.update();
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
                    duration: 500,
                    easing: 'easeInOutCubic'
                },
                transitions: {
                    active: {
                        animation: {
                            duration: 500,
                            easing: 'easeInOutCubic'
                        }
                    },
                    show: {
                        animations: {
                            x: { duration: 500, easing: 'easeInOutCubic' },
                            y: { duration: 500, easing: 'easeInOutCubic' }
                        }
                    },
                    hide: {
                        animations: {
                            x: { duration: 500, easing: 'easeInOutCubic' },
                            y: { duration: 500, easing: 'easeInOutCubic' }
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
                        backgroundColor: 'rgba(0, 0, 0, 0.85)',
                        titleFont: { size: 14, weight: 'bold' },
                        bodyFont: { size: 13 },
                        footerFont: { size: 13, weight: 'bold' },
                        footerColor: '#10b981',
                        filter: function (tooltipItem) {
                            return tooltipItem.parsed.y !== 0 && tooltipItem.parsed.y !== null && tooltipItem.parsed.y !== undefined;
                        },
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                label += new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(context.parsed.y);
                                return label;
                            },
                            footer: function (tooltipItems) {
                                let totalCash = 0;
                                let cashCount = 0;

                                let selectedEarningsTotal = 0;
                                let earningsCount = 0;
                                let feesTotal = 0;
                                let hasFees = false;

                                tooltipItems.forEach(function (item) {
                                    const l = item.dataset.label || '';
                                    const val = item.parsed.y || 0;

                                    if (l === 'Envelope Drops' || l === 'Vending Machine Drops') {
                                        totalCash += val;
                                        cashCount++;
                                    }
                                    else if (l === 'Commissions' || l === 'Cash Bonus' || l === 'Claims Bonus') {
                                        selectedEarningsTotal += val;
                                        earningsCount++;
                                    }
                                    else if (l === 'Weekly Fees') {
                                        feesTotal += val;
                                        hasFees = true;
                                    }
                                });

                                if (cashCount > 0) {
                                    const formatted = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(totalCash);
                                    return 'Total Cash Available: ' + formatted;
                                }

                                if (earningsCount > 0) {
                                    const formattedEarnings = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(selectedEarningsTotal);
                                    if (hasFees) {
                                        const netTotal = selectedEarningsTotal - feesTotal;
                                        const formattedNet = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(netTotal);
                                        return 'Total Selected Earnings: ' + formattedEarnings + '\nNet Total: ' + formattedNet;
                                    } else {
                                        return 'Total Selected: ' + formattedEarnings;
                                    }
                                }

                                return '';
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
