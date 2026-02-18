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
        setTimeout(function () {
            successAlert.style.transition = "opacity 0.6s ease, transform 0.6s ease";
            successAlert.style.opacity = "0";
            successAlert.style.transform = "translate(-50%, -20px)";

            setTimeout(() => {
                successAlert.remove();
            }, 600);
        }, 3000);
    }

    // 2. Branch Dropdown Auto-Submit
    if (branchSelect) {
        branchSelect.addEventListener('change', refreshTable);
    }

    // 3. Page Size Selector Auto-Submit
    if (pageSizeSelector) {
        pageSizeSelector.addEventListener('change', refreshTable);
    }

    // 4. Delete Modal Logic
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const memberId = button.getAttribute('data-member-id');
            // Fallback to "this member" if name is missing/optional
            const memberName = button.getAttribute('data-member-name') || "this member";

            const modalBodyText = deleteModal.querySelector('#modal-body-text');
            const memberIdInput = deleteModal.querySelector('#memberIdInput');

            if (modalBodyText) {
                modalBodyText.textContent = `Are you sure you want to delete member "${memberName}"?`;
            }
            if (memberIdInput) {
                memberIdInput.value = memberId;
            }
        });
    }

    // 5. URL Optimization (Clean URL parameters)
    if (filterForm) {
        filterForm.addEventListener('submit', function () {
            const inputs = filterForm.querySelectorAll('input, select');
            inputs.forEach(input => {
                // Only disable if it's an empty text search. 
                // We keep selects enabled if they are part of the core filtering.
                if (!input.value && input.tagName === 'INPUT') {
                    input.disabled = true;
                }
            });
        });
    }
});