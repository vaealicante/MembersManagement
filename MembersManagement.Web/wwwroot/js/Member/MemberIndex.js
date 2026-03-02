document.addEventListener("DOMContentLoaded", function () {

    console.log("MemberIndex.js loaded");

    // =========================
    // DELETE MODAL LOGIC
    // =========================
    const deleteForm = document.getElementById("deleteForm");
    const memberIdInput = document.getElementById("memberIdInput");
    const modalBodyText = document.getElementById("modal-body-text");
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");

    if (deleteForm && confirmDeleteBtn) {

        document.querySelectorAll(".delete-button").forEach(button => {
            button.addEventListener("click", function () {

                const memberId = this.dataset.memberId;
                const memberName = this.dataset.memberName;

                memberIdInput.value = memberId;
                modalBodyText.innerHTML =
                    `Are you sure you want to delete <strong>${memberName}</strong>?<br>
                     <small class="text-muted">This action cannot be undone.</small>`;
            });
        });

        confirmDeleteBtn.addEventListener("click", function () {
            deleteForm.submit();
        });

    } else {
        console.warn("Delete modal elements not found — skipping delete logic");
    }

    // =========================
    // SUCCESS ALERT AUTO-HIDE
    // =========================
    const alert = document.getElementById("success-alert");

    if (!alert) {
        console.log("No success alert found");
        return;
    }

    console.log("Success alert detected");

    setTimeout(() => {
        alert.classList.add("hide");                                                                                                                                                                                                                                                                                                                                                                                                                                  

        setTimeout(() => {
            alert.remove();
        }, 600);

    }, 3000);
});