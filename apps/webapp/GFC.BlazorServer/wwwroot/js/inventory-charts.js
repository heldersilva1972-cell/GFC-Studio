window.inventoryCharts = {
    charts: {},
    renderStockVsUsage: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const ctx = el.getContext('2d');
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
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: config.yAxisTitle || 'Bottles Amount'
                        }
                    }
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

        const datasets = [];

        if (config.startingStockData && config.startingStockData.length > 0) {
            datasets.push({
                label: 'Starting Stock (Period Baseline)',
                data: config.startingStockData,
                backgroundColor: 'rgba(100, 116, 139, 0.75)', // slate/gray
                borderColor: '#64748b',
                borderWidth: 1.5,
                borderRadius: 6
            });
        }

        datasets.push({
            label: 'Current Stock (On-Hand)',
            data: config.stockData,
            backgroundColor: 'rgba(59, 130, 246, 0.85)', // vibrant blue
            borderColor: '#2563eb',
            borderWidth: 1.5,
            borderRadius: 6
        });

        if (config.addedData && config.addedData.length > 0) {
            datasets.push({
                label: 'Restocked / Added (+)',
                data: config.addedData,
                backgroundColor: 'rgba(16, 185, 129, 0.8)', // emerald green
                borderColor: '#059669',
                borderWidth: 1.5,
                borderRadius: 6
            });
        }

        datasets.push({
            label: 'Quantity Consumed (Used)',
            data: config.usedData,
            rawUnits: config.rawUsedData,
            pourSizes: config.pourSizes,
            backgroundColor: 'rgba(239, 68, 68, 0.8)', // red
            borderColor: '#dc2626',
            borderWidth: 1.5,
            borderRadius: 6
        });

        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'top' },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        callbacks: {
                            label: function (context) {
                                const ds = context.dataset;
                                if (ds.rawUnits && ds.rawUnits[context.dataIndex]) {
                                    return ds.label + ': ' + ds.rawUnits[context.dataIndex] + ' units (' + context.formattedValue + ' btls)';
                                }
                                return ds.label + ': ' + context.formattedValue + ' btls';
                            },
                            afterBody: function (tooltipItems) {
                                if (!tooltipItems || tooltipItems.length === 0) return [];
                                const idx = tooltipItems[0].dataIndex;
                                const lines = [];
                                if (config.lastRestocked && config.lastRestocked[idx]) {
                                    lines.push('📦 Last Restocked: ' + config.lastRestocked[idx]);
                                }
                                if (config.lastAudited && config.lastAudited[idx]) {
                                    lines.push('📋 Last Audited: ' + config.lastAudited[idx]);
                                }
                                return lines;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: config.yAxisTitle || 'Bottles Amount'
                        }
                    }
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
    },
    renderTokenFlowChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [
                    {
                        label: 'Tokens Sold (Qty)',
                        data: config.soldData,
                        backgroundColor: 'rgba(34, 197, 94, 0.75)', // green
                        borderColor: '#22c55e',
                        borderWidth: 1.5,
                        borderRadius: 6
                    },
                    {
                        label: 'Tokens Redeemed (Qty)',
                        data: config.redeemedData,
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
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        filter: function (tooltipItem) {
                            return tooltipItem.parsed.y !== 0 && tooltipItem.raw !== 0;
                        },
                        callbacks: {
                            label: function (context) {
                                let val = context.parsed.y;
                                if (!val) return null;
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                return label + val + ' tokens';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { precision: 0 },
                        title: { display: true, text: 'Token Count' }
                    }
                }
            }
        });
    },
    renderBartenderTokenChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [
                    {
                        label: 'Tokens Sold (Qty)',
                        data: config.soldData,
                        backgroundColor: 'rgba(34, 197, 94, 0.75)',
                        borderColor: '#22c55e',
                        borderWidth: 1.5,
                        borderRadius: 6
                    },
                    {
                        label: 'Tokens Redeemed (Qty)',
                        data: config.redeemedData,
                        backgroundColor: 'rgba(239, 68, 68, 0.75)',
                        borderColor: '#ef4444',
                        borderWidth: 1.5,
                        borderRadius: 6
                    },
                    {
                        label: 'Net Balance (Sold - Redeemed)',
                        data: config.netData,
                        backgroundColor: 'rgba(59, 130, 246, 0.65)',
                        borderColor: '#3b82f6',
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
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        filter: function (tooltipItem) {
                            return tooltipItem.parsed.y !== 0 && tooltipItem.raw !== 0;
                        },
                        callbacks: {
                            label: function (context) {
                                let val = context.parsed.y;
                                if (!val) return null;
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                return label + (val > 0 ? '+' : '') + val + ' tokens';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        ticks: { precision: 0 },
                        title: { display: true, text: 'Tokens Count' }
                    }
                }
            }
        });
    },
    renderProductTokenFlowChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [
                    {
                        label: 'Tokens Sold (Units)',
                        data: config.tokensSoldData,
                        backgroundColor: 'rgba(34, 197, 94, 0.75)',
                        borderColor: '#22c55e',
                        borderWidth: 1.5,
                        borderRadius: 6
                    },
                    {
                        label: 'Tokens Redeemed (Units)',
                        data: config.tokensRedeemedData,
                        backgroundColor: 'rgba(168, 85, 247, 0.85)',
                        borderColor: '#a855f7',
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
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        filter: function (tooltipItem) {
                            return tooltipItem.parsed.y !== 0 && tooltipItem.raw !== 0;
                        },
                        callbacks: {
                            label: function (context) {
                                let val = context.parsed.y;
                                if (!val) return null;
                                let label = context.dataset.label || '';
                                if (label) label += ': ';
                                return label + val + ' units';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { precision: 0 },
                        title: { display: true, text: 'Token Count (Units)' }
                    }
                }
            }
        });
    },
    renderRankingPerformanceChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const defaultColors = [
            'rgba(59, 130, 246, 0.85)',  // blue
            'rgba(16, 185, 129, 0.85)',  // green
            'rgba(139, 92, 246, 0.85)',  // purple
            'rgba(245, 158, 11, 0.85)',  // amber
            'rgba(236, 72, 153, 0.85)',  // pink
            'rgba(6, 182, 212, 0.85)',   // cyan
            'rgba(239, 68, 68, 0.85)',   // red
            'rgba(249, 115, 22, 0.85)',  // orange
            'rgba(20, 184, 166, 0.85)',  // teal
            'rgba(107, 114, 128, 0.85)'  // gray
        ];

        const borderColors = [
            '#2563eb', '#059669', '#7c3aed', '#d97706', '#db2777',
            '#0891b2', '#dc2626', '#ea580c', '#0d9488', '#4b5563'
        ];

        const bgColors = config.colors || defaultColors.slice(0, config.labels.length);
        const borders = config.borderColors || borderColors.slice(0, config.labels.length);
        const metric = config.metricType || 'REVENUE';

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [{
                    label: config.datasetLabel || 'Performance',
                    data: config.data,
                    backgroundColor: bgColors,
                    borderColor: borders,
                    borderWidth: 1.5,
                    borderRadius: 6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: config.horizontal ? 'y' : 'x',
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        callbacks: {
                            label: function (context) {
                                const val = context.raw;
                                if (metric === 'REVENUE') {
                                    const formatted = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(val);
                                    return ' Gross Sales: ' + formatted;
                                } else if (metric === 'VOLUME') {
                                    const unitType = (config.units && config.units[context.dataIndex]) ? config.units[context.dataIndex] : 'units';
                                    return ' Volume Sold: ' + new Intl.NumberFormat('en-US').format(val) + ' ' + unitType;
                                } else if (metric === 'SHARE') {
                                    return ' Market Share: ' + val.toFixed(1) + '%';
                                }
                                return ' ' + val;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function (value) {
                                if (config.horizontal) return value;
                                if (metric === 'REVENUE') {
                                    return '$' + new Intl.NumberFormat('en-US', { notation: 'compact', compactDisplay: 'short' }).format(value);
                                } else if (metric === 'SHARE') {
                                    return value + '%';
                                }
                                return new Intl.NumberFormat('en-US', { notation: 'compact' }).format(value);
                            }
                        }
                    },
                    x: {
                        beginAtZero: true,
                        ticks: {
                            callback: function (value) {
                                if (!config.horizontal) return value;
                                if (metric === 'REVENUE') {
                                    return '$' + new Intl.NumberFormat('en-US', { notation: 'compact', compactDisplay: 'short' }).format(value);
                                } else if (metric === 'SHARE') {
                                    return value + '%';
                                }
                                return new Intl.NumberFormat('en-US', { notation: 'compact' }).format(value);
                            }
                        }
                    }
                }
            }
        });
    },
    renderRankingShareDonut: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const colors = [
            '#3b82f6', '#10b981', '#f59e0b', '#ec4899', '#8b5cf6',
            '#ef4444', '#14b8a6', '#f97316', '#06b6d4', '#6b7280'
        ];

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: config.labels,
                datasets: [{
                    data: config.data,
                    backgroundColor: colors.slice(0, config.labels.length),
                    borderWidth: 2,
                    borderColor: '#ffffff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            font: { size: 11, weight: 'bold' },
                            padding: 12
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const val = context.raw;
                                const formatted = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(val);
                                return ' ' + context.label + ': ' + formatted;
                            }
                        }
                    }
                },
                cutout: '65%'
            }
        });
    },
    renderVelocityMovementChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const ctx = el.getContext('2d');
        const metric = config.metricType || 'DAYS';

        const valueDataLabelPlugin = {
            id: 'velocityValueLabelPlugin',
            afterDatasetsDraw: function (chart) {
                const chartCtx = chart.ctx;
                chart.data.datasets.forEach((dataset, i) => {
                    const meta = chart.getDatasetMeta(i);
                    meta.data.forEach((bar, index) => {
                        const val = dataset.data[index];
                        if (val === undefined || val === null) return;

                        chartCtx.save();
                        chartCtx.font = 'bold 11px sans-serif';
                        chartCtx.fillStyle = '#0f172a';

                        let text = '';
                        if (config.displayTexts && config.displayTexts[index]) {
                            text = config.displayTexts[index];
                        } else if (metric === 'VALUE') {
                            text = '$' + new Intl.NumberFormat('en-US', { maximumFractionDigits: 0 }).format(val);
                        } else {
                            if (val <= 0.45) {
                                text = 'Today';
                            } else if (val >= 999) {
                                text = 'Never';
                            } else {
                                text = Math.round(val) + 'd';
                            }
                        }

                        if (config.horizontal) {
                            chartCtx.textAlign = 'left';
                            chartCtx.textBaseline = 'middle';
                            chartCtx.fillText(text, bar.x + 6, bar.y);
                        } else {
                            chartCtx.textAlign = 'center';
                            chartCtx.textBaseline = 'bottom';
                            chartCtx.fillText(text, bar.x, bar.y - 4);
                        }
                        chartCtx.restore();
                    });
                });
            }
        };

        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels,
                datasets: [{
                    label: config.datasetLabel || 'Movement Velocity',
                    data: config.data,
                    backgroundColor: config.colors,
                    borderColor: config.colors,
                    borderWidth: 1.5,
                    borderRadius: 6
                }]
            },
            plugins: [valueDataLabelPlugin],
            options: {
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: config.horizontal ? 'y' : 'x',
                layout: {
                    padding: {
                        top: config.horizontal ? 10 : 25,
                        right: config.horizontal ? 50 : 15
                    }
                },
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        callbacks: {
                            label: function (context) {
                                const index = context.dataIndex;
                                if (config.tooltipDetails && config.tooltipDetails[index]) {
                                    return config.tooltipDetails[index];
                                }
                                const val = context.raw;
                                if (metric === 'VALUE') {
                                    const formatted = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(val);
                                    return ' Stock Capital at Risk: ' + formatted;
                                }
                                if (val <= 0.45) {
                                    return ' Days Since Last Sale: Today (0 days ago)';
                                }
                                if (val >= 999) {
                                    return ' Days Since Last Sale: Never / No sales on record';
                                }
                                return ' Days Since Last Sale: ' + Math.round(val) + ' days';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        suggestedMax: metric === 'DAYS' ? 5 : undefined,
                        ticks: {
                            autoSkip: false,
                            font: { weight: 'bold', size: 10 },
                            callback: function (value) {
                                if (config.horizontal) return this.getLabelForValue(value);
                                if (metric === 'VALUE') {
                                    return '$' + new Intl.NumberFormat('en-US', { notation: 'compact', compactDisplay: 'short' }).format(value);
                                }
                                return value === 0 ? 'Today' : value + 'd';
                            }
                        }
                    },
                    x: {
                        beginAtZero: true,
                        suggestedMax: metric === 'DAYS' ? 5 : undefined,
                        ticks: {
                            autoSkip: false,
                            font: { weight: 'bold', size: 10 },
                            maxRotation: 45,
                            minRotation: 45,
                            callback: function (value) {
                                if (!config.horizontal) return this.getLabelForValue(value);
                                if (metric === 'VALUE') {
                                    return '$' + new Intl.NumberFormat('en-US', { notation: 'compact', compactDisplay: 'short' }).format(value);
                                }
                                return value === 0 ? 'Today' : value + 'd';
                            }
                        }
                    }
                }
            }
        });
    },
    renderSimpleMonthlyNetGapChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const bgColors = (config.data || []).map(v => v >= 0 ? 'rgba(16, 185, 129, 0.85)' : 'rgba(239, 68, 68, 0.85)');
        const borderColors = (config.data || []).map(v => v >= 0 ? '#059669' : '#dc2626');

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels || [],
                datasets: [{
                    label: 'Net Monthly Profit / Deficit ($)',
                    data: config.data || [],
                    backgroundColor: bgColors,
                    borderColor: borderColors,
                    borderWidth: 1.5,
                    borderRadius: 6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(15, 23, 42, 0.95)',
                        padding: 12,
                        cornerRadius: 8,
                        titleFont: { weight: 'bold', size: 12 },
                        callbacks: {
                            label: function (context) {
                                const val = Number(context.parsed.y || 0);
                                const sign = val >= 0 ? '+' : '';
                                return `Net Result: ${sign}$${val.toLocaleString()}`;
                            },
                            afterLabel: function (context) {
                                const idx = context.dataIndex;
                                const actual = config.actual ? config.actual[idx] : null;
                                const expected = config.expected ? config.expected[idx] : null;
                                const lines = [];
                                if (actual !== null) lines.push(`Actual Sales: $${Number(actual).toLocaleString()}`);
                                if (expected !== null) lines.push(`Target Expected: $${Number(expected).toLocaleString()}`);
                                return lines.join('\n');
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        grid: { display: false },
                        ticks: { font: { weight: 'bold', size: 11 } }
                    },
                    y: {
                        beginAtZero: true,
                        title: { display: true, text: 'Net Gap vs Target ($)', font: { size: 10, weight: 'bold' } },
                        ticks: {
                            callback: function (val) {
                                const sign = val >= 0 ? '+' : '';
                                return sign + '$' + val;
                            }
                        }
                    }
                }
            }
        });
    },
    renderProductVarianceChart: function (canvasId, config) {
        const el = document.getElementById(canvasId);
        if (!el) return;

        const existingChart = (typeof Chart !== 'undefined' && Chart.getChart) ? Chart.getChart(el) || Chart.getChart(canvasId) : null;
        if (existingChart) {
            try { existingChart.destroy(); } catch (e) {}
        }
        if (this.charts[canvasId]) {
            try { this.charts[canvasId].destroy(); } catch (e) {}
            delete this.charts[canvasId];
        }

        const percentages = config.percentages || [];
        const bgColors = percentages.map(pct => {
            if (pct >= 90 && pct <= 112) return 'rgba(16, 185, 129, 0.85)'; // Green (Normal / Healthy Tolerance)
            if (pct >= 80 && pct < 90) return 'rgba(245, 158, 11, 0.85)'; // Yellow / Amber (Mild Warning / Watchlist)
            if (pct > 112) return 'rgba(147, 51, 234, 0.85)'; // Purple (Unrecorded Bottle Pull / Surplus >112%)
            return 'rgba(239, 68, 68, 0.85)'; // Red (< 80% Critical Deficit)
        });
        const borderColors = percentages.map(pct => {
            if (pct >= 90 && pct <= 112) return '#059669';
            if (pct >= 80 && pct < 90) return '#d97706';
            if (pct > 112) return '#7e22ce'; // Deep purple
            return '#dc2626';
        });

        // Dynamically adjust container height so every product has comfortable spacing
        const numItems = (config.labels || []).length;
        if (numItems > 0 && el.parentElement) {
            const dynamicHeight = Math.max(480, numItems * 25 + 70);
            el.parentElement.style.height = `${dynamicHeight}px`;
        }

        const ctx = el.getContext('2d');
        this.charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: config.labels || [],
                datasets: [{
                    label: '% Target Yield Realized',
                    data: percentages,
                    backgroundColor: bgColors,
                    borderColor: borderColors,
                    borderWidth: 1,
                    borderRadius: 4,
                    maxBarThickness: 13,
                    categoryPercentage: 0.85,
                    barPercentage: 0.9
                }]
            },
            plugins: [{
                id: 'targetLinePlugin',
                afterDraw: function (chart) {
                    const ctx2 = chart.ctx;
                    const xAxis = chart.scales.x;
                    const yAxis = chart.scales.y;
                    if (!xAxis || !yAxis) return;
                    const xPos = xAxis.getPixelForValue(100);
                    if (xPos >= xAxis.left && xPos <= xAxis.right) {
                        ctx2.save();
                        ctx2.beginPath();
                        ctx2.setLineDash([4, 3]);
                        ctx2.moveTo(xPos, yAxis.top);
                        ctx2.lineTo(xPos, yAxis.bottom);
                        ctx2.lineWidth = 1.5;
                        ctx2.strokeStyle = '#0284c7'; // Clean sky blue
                        ctx2.stroke();

                        // Label at the top of the line
                        ctx2.fillStyle = '#0284c7';
                        ctx2.font = 'bold 8.5px sans-serif';
                        ctx2.textAlign = 'center';
                        ctx2.fillText('100% TARGET', xPos, yAxis.top - 4);
                        ctx2.restore();
                    }
                }
            }],
            options: {
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: 'y', // Horizontal bars for clean reading of brand names
                interaction: {
                    mode: 'nearest',
                    intersect: true
                },
                layout: {
                    padding: { top: 12, right: 10 }
                },
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        enabled: true,
                        backgroundColor: 'rgba(15, 23, 42, 0.95)',
                        padding: 10,
                        cornerRadius: 8,
                        caretSize: 6,
                        caretPadding: 18, // Generous padding so tooltip does not block cursor
                        titleFont: { weight: 'bold', size: 11 },
                        callbacks: {
                            title: function (tooltipItems) {
                                if (!tooltipItems || tooltipItems.length === 0) return '';
                                const idx = tooltipItems[0].dataIndex;
                                const name = config.names ? config.names[idx] : tooltipItems[0].label;
                                const pct = (config.percentages && config.percentages[idx] !== undefined) ? config.percentages[idx] : null;
                                return pct !== null ? `${name} — ${pct}% Target Yield` : name;
                            },
                            label: function (context) {
                                const val = Number(context.parsed.x || 0);
                                const idx = context.dataIndex;
                                const pulls = (config.pulls && config.pulls[idx] !== undefined) ? Number(config.pulls[idx]) : 0;
                                const shots = (config.shots && config.shots[idx] !== undefined) ? Number(config.shots[idx]) : 0;
                                const targetShots = (config.targetShots && config.targetShots[idx] !== undefined && config.targetShots[idx] > 0) ? Number(config.targetShots[idx]) : 22;

                                let status = '';
                                if (val >= 90 && val <= 112) {
                                    status = '🟢 Healthy / Normal';
                                } else if (val >= 80 && val < 90) {
                                    status = '🟡 Mild Variance / Watchlist';
                                } else if (val > 112) {
                                    const expectedShots = pulls * targetShots;
                                    const extraShots = shots - expectedShots;
                                    const estMissing = Math.max(1, Math.ceil(extraShots / targetShots));
                                    const bottleWord = estMissing === 1 ? 'bottle' : 'bottles';
                                    status = `🟣 Unrecorded Pull (~${estMissing} ${bottleWord} missing)`;
                                } else {
                                    status = '🔴 Action Needed / Deficit';
                                }
                                return `Yield Realized: ${val}% (${status})`;
                            },
                            afterLabel: function (context) {
                                const idx = context.dataIndex;
                                const pulls = config.pulls ? config.pulls[idx] : null;
                                const shots = config.shots ? config.shots[idx] : null;
                                const avgShots = config.avgShots ? config.avgShots[idx] : null;
                                const targetShots = config.targetShots ? config.targetShots[idx] : null;
                                const actual = config.actual ? config.actual[idx] : null;
                                const expected = config.expected ? config.expected[idx] : null;
                                const gap = (actual !== null && expected !== null) ? (actual - expected) : null;
                                const val = Number(context.parsed.x || 0);

                                const lines = [];
                                const traj = (config.trajectories && config.trajectories[idx]) ? config.trajectories[idx] : null;
                                const trendBadge = (config.trendBadges && config.trendBadges[idx]) ? config.trendBadges[idx] : null;
                                const confBadge = (config.confidenceBadges && config.confidenceBadges[idx]) ? config.confidenceBadges[idx] : null;

                                if (traj) {
                                    lines.push(`📈 Trajectory: ${traj}${trendBadge ? ` [${trendBadge}]` : ''}`);
                                }
                                if (confBadge) {
                                    lines.push(`🔍 Confidence: ${confBadge}`);
                                }
                                if (val > 112 && targetShots && targetShots > 0 && pulls !== null && shots !== null) {
                                    const extra = shots - (pulls * targetShots);
                                    const estMissing = Math.Max ? Math.max(1, Math.ceil(extra / targetShots)) : Math.max(1, Math.ceil(extra / targetShots));
                                    const btlWord = estMissing === 1 ? 'bottle' : 'bottles';
                                    lines.push(`⚠️ Suspected Missing Pull: ~${estMissing} ${btlWord} missing from checkout logs`);
                                }
                                if (avgShots !== null && targetShots !== null) {
                                    lines.push(`🥃 Shots / Bottle: ${avgShots} avg / ${targetShots} target`);
                                }
                                if (pulls !== null && shots !== null) {
                                    lines.push(`📦 Total Activity: ${pulls} btls pulled, ${shots} shots sold`);
                                }
                                if (actual !== null && expected !== null && gap !== null) {
                                    const sign = gap >= 0 ? '+' : '';
                                    lines.push(`💰 Revenue: $${Number(actual).toLocaleString()} actual / $${Number(expected).toLocaleString()} target (${sign}$${Number(gap).toLocaleString()})`);
                                }
                                return lines.join('\n');
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        min: 0,
                        suggestedMax: 120,
                        grid: { color: 'rgba(0, 0, 0, 0.05)' },
                        ticks: {
                            font: { weight: 'bold', size: 9.5 },
                            callback: function (val) {
                                return val + '%';
                            }
                        },
                        title: { display: true, text: 'Target Yield Realized (% of Target Yield, 100% = Target)', font: { size: 9.5, weight: 'bold' } }
                    },
                    y: {
                        grid: { display: false },
                        ticks: {
                            autoSkip: false, // Always show every single product name
                            font: { weight: 'bold', size: 10 }
                        }
                    }
                }
            }
        });
    }
};
