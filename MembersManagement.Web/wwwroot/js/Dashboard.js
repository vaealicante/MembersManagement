/**
 * dashboard.js
 * Handles dashboard chart rendering
 */

document.addEventListener("DOMContentLoaded", function () {

    const chartCanvas = document.getElementById("statusChart");

    // Stop if chart element doesn't exist
    if (!chartCanvas) return;

    // Get values from Razor data attributes
    const activeCount = parseInt(chartCanvas.dataset.active) || 0;
    const inactiveCount = parseInt(chartCanvas.dataset.inactive) || 0;

    const totalMembers = activeCount + inactiveCount;

    // Initialize Chart
    new Chart(chartCanvas, {
        type: "doughnut",
        data: {
            labels: ["Active Members", "Inactive Members"],
            datasets: [{
                data: [activeCount, inactiveCount],
                backgroundColor: [
                    "#198754", // Bootstrap success
                    "#adb5bd"  // muted gray
                ],
                borderWidth: 0,
                hoverOffset: 12
            }]
        },
        options: {

            // Modern doughnut look
            cutout: "75%",

            responsive: true,
            maintainAspectRatio: false,

            plugins: {

                legend: {
                    position: "bottom",
                    labels: {
                        boxWidth: 12,
                        padding: 20,
                        font: {
                            size: 12,
                            weight: "500"
                        }
                    }
                },

                tooltip: {
                    callbacks: {
                        label: function (context) {

                            const value = context.raw;

                            const percentage =
                                totalMembers > 0
                                    ? ((value / totalMembers) * 100).toFixed(1)
                                    : 0;

                            return `${context.label}: ${value} (${percentage}%)`;
                        }
                    }
                }

            }
        }
    });

});