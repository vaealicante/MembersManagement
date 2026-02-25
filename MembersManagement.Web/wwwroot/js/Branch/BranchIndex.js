document.addEventListener("DOMContentLoaded", function () {

    const deleteForm = document.getElementById("deleteForm");
    const branchIdInput = document.getElementById("branchIdInput");
    const modalBodyText = document.getElementById("modal-body-text");
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");

    if (!deleteForm || !modalBodyText || !confirmDeleteBtn) {
        console.error("Delete modal elements not found");
        return;
    }

    // ----------------------------
    // Populate Delete Modal
    // ----------------------------
    document.querySelectorAll(".delete-button").forEach(button => {
        button.addEventListener("click", function () {
            const branchId = this.dataset.branchId;
            const branchName = this.dataset.branchName;

            branchIdInput.value = branchId;
            modalBodyText.innerHTML =
                `Are you sure you want to delete <strong>${branchName}</strong>?<br>
                <small class="text-muted">This will deactivate the branch.</small>`;

            console.log("Delete modal populated:", branchId, branchName);
        });
    });

    // ----------------------------
    // Confirm Delete
    // ----------------------------
    confirmDeleteBtn.addEventListener("click", function () {
        deleteForm.submit();
    });

    // ----------------------------
    // Auto-hide Success Message
    // ----------------------------
    const alert = document.getElementById('success-alert');
    if (alert) {
        setTimeout(() => {
            alert.classList.add('fade');
            alert.style.transition = 'opacity 0.5s ease-out';
            alert.style.opacity = '0';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    }

});