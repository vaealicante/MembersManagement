document.addEventListener("DOMContentLoaded", function () {
    const filterForm = document.getElementById('filterForm');
    const branchInput = document.getElementById('branchInput');
    const pageSizeSelector = document.getElementById('pageSizeSelector');
    const successAlert = document.getElementById('success-alert');
    const deleteModal = document.getElementById('deleteModal');

    // Helper: Refresh Table
    const refreshTable = () => {
        const pageInput = document.getElementById('currentPageInput');
        if (pageInput) pageInput.value = 1;
        filterForm.submit();
    };

    // 1. Success Alert Auto-Fade (Centered version)
    if (successAlert) {
        setTimeout(function () {
            successAlert.style.transition = "opacity 0.6s ease, transform 0.6s ease";
            successAlert.style.opacity = "0";
            successAlert.style.transform = "translate(-50%, -20px)";

            setTimeout(function () {
                successAlert.remove();
            }, 600);
        }, 3000);
    }

    // 2. Branch Datalist Auto-Submit
    if (branchInput) {
        branchInput.addEventListener('input', function () {
            const val = this.value;
            const options = document.getElementById('branchOptions').options;
            for (let i = 0; i < options.length; i++) {
                if (val === options[i].value) {
                    refreshTable();
                    break;
                }
            }
        });
    }

    // 3. Page Size Selector Change
    if (pageSizeSelector) {
        pageSizeSelector.addEventListener('change', refreshTable);
    }

    // 4. Delete Modal Logic
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const memberId = button.getAttribute('data-member-id');
            const memberName = button.getAttribute('data-member-name');

            deleteModal.querySelector('#modal-body-text').textContent =
                `Are you sure you want to delete member "${memberName}"?`;
            deleteModal.querySelector('#memberIdInput').value = memberId;
        });
    }
});