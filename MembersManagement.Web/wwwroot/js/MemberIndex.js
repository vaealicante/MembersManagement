/**
 * MemberIndex.js - Logic for Member Index Page
 */
document.addEventListener("DOMContentLoaded", function () {
    // DOM Elements
    const filterForm = document.getElementById('filterForm');
    const branchSelect = document.getElementById('branchSelect');
    const pageSizeSelector = document.getElementById('pageSizeSelector');
    const successAlert = document.getElementById('success-alert');
    const deleteModal = document.getElementById('deleteModal');
    const currentPageInput = document.getElementById('currentPageInput');

    /**
     * Helper: Reset to page 1 and submit form.
     * Prevents being stuck on an empty page when filters change.
     */
    const refreshTable = () => {
        if (currentPageInput) {
            currentPageInput.value = 1;
        }
        if (filterForm) {
            filterForm.submit();
        }
    };

    // 1. Success Alert Auto-Fade Logic
    if (successAlert) {
        // Smoothly fade out after 3 seconds
        setTimeout(function () {
            successAlert.style.transition = "opacity 0.6s ease, transform 0.6s ease";
            successAlert.style.opacity = "0";
            successAlert.style.transform = "translate(-50%, -20px)";

            // Remove from DOM once animation finishes
            setTimeout(() => {
                successAlert.remove();
            }, 600);
        }, 3000);
    }

    // 2. Branch Dropdown Auto-Submit
    // Triggers when a valid branch is selected from the dropdown
    if (branchSelect) {
        branchSelect.addEventListener('change', refreshTable);
    }

    // 3. Page Size Selector Auto-Submit
    if (pageSizeSelector) {
        pageSizeSelector.addEventListener('change', refreshTable);
    }

    // 4. Delete Modal Logic
    // Populates the modal with the specific member's data from data attributes
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            // Button that triggered the modal
            const button = event.relatedTarget;

            // Extract data attributes
            const memberId = button.getAttribute('data-member-id');
            const memberName = button.getAttribute('data-member-name');

            // Find elements inside the modal
            const modalBodyText = deleteModal.querySelector('#modal-body-text');
            const memberIdInput = deleteModal.querySelector('#memberIdInput');

            // Inject the data
            if (modalBodyText) {
                modalBodyText.textContent = `Are you sure you want to delete member "${memberName}"?`;
            }
            if (memberIdInput) {
                memberIdInput.value = memberId;
            }
        });
    }

    // 5. URL Optimization
    // Prevents empty search parameters from appearing in the URL on submission
    if (filterForm) {
        filterForm.addEventListener('submit', function () {
            const inputs = filterForm.querySelectorAll('input, select');
            inputs.forEach(input => {
                if (!input.value || input.value === "") {
                    input.disabled = true; // Temporary disable so it's not sent
                }
            });
        });
    }
});