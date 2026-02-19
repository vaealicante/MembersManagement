document.addEventListener("DOMContentLoaded", function () {

    /* =========================
       DELETE MODAL LOGIC
    ========================== */
    const deleteButtons = document.querySelectorAll(".delete-button");
    const modalBodyText = document.getElementById("modal-body-text");
    const memberIdInput = document.getElementById("memberIdInput");
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");
    const deleteForm = document.getElementById("deleteForm");
    const deleteModalEl = document.getElementById("deleteModal");

    // Initialize Bootstrap modal instance
    const deleteModal = deleteModalEl ? new bootstrap.Modal(deleteModalEl, { backdrop: 'static', keyboard: false }) : null;

    // When a table delete button is clicked
    deleteButtons.forEach(btn => {
        btn.addEventListener("click", function () {
            const memberId = this.dataset.memberId;
            const memberName = this.dataset.memberName;

            // Fill modal
            memberIdInput.value = memberId;
            modalBodyText.textContent = `Are you sure you want to delete "${memberName}"?`;

            // Show modal
            if (deleteModal) deleteModal.show();
        });
    });

    // Confirm delete button inside modal
    if (confirmDeleteBtn && deleteForm) {
        confirmDeleteBtn.addEventListener("click", function () {
            if (!memberIdInput.value) {
                console.error("Member ID is empty. Delete aborted.");
                return;
            }

            // Hide modal first to prevent blocking form submit
            if (deleteModal) deleteModal.hide();

            // Wait 200ms for modal to hide then submit
            setTimeout(() => {
                deleteForm.submit();
            }, 200);
        });
    }

    /* =========================
       AUTO-HIDE SUCCESS ALERT
    ========================== */
    const successAlert = document.getElementById("success-alert");
    if (successAlert) {
        setTimeout(() => {
            successAlert.remove();
        }, 5000);
    }

    /* =========================
       PAGE SIZE HANDLING
    ========================== */
    const pageSizeSelector = document.getElementById("pageSizeSelector");
    const filterForm = document.getElementById("filterForm");

    if (pageSizeSelector && filterForm) {
        const params = new URLSearchParams(window.location.search);
        const pageSize = params.get("pageSize");
        if (pageSize !== null) pageSizeSelector.value = pageSize;

        pageSizeSelector.addEventListener("change", () => filterForm.submit());
    }
});
