/**
 * dashboard.js - Logic for Management Dashboard
 */
document.addEventListener("DOMContentLoaded", function () {
    const ctx = document.getElementById('statusChart');

    if (ctx) {
        // Retrieve dynamic data from HTML data attributes
        const activeCount = parseInt(ctx.getAttribute('data-active')) || 0;
        const inactiveCount = parseInt(ctx.getAttribute('data-inactive')) || 0;

        new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ['Active', 'Inactive'],
                datasets: [{
                    data: [activeCount, inactiveCount],
                    backgroundColor: [
                        '#198754', // Success Green
                        '#adb5bd'  // Muted Gray
                    ],
                    borderWidth: 0,
                    hoverOffset: 12
                }]
            },
            options: {
                cutout: '75%', // Creates a thinner, modern doughnut look
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            boxWidth: 12,
                            padding: 20,
                            font: {
                                size: 12,
                                weight: '500'
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const total = activeCount + inactiveCount;
                                const value = context.raw;
                                const percentage = total > 0 ? ((value / total) * 100).toFixed(1) : 0;
                                return ` ${context.label}: ${value} (${percentage}%)`;
                            }
                        }
                    }
                }
            }
        });
    }
});