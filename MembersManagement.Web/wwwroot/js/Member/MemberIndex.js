document.addEventListener("DOMContentLoaded", function () {

    console.log("MemberIndex.js loaded");

    const deleteForm = document.getElementById("deleteForm");
    const memberIdInput = document.getElementById("memberIdInput");
    const modalBodyText = document.getElementById("modal-body-text");
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");

    if (!deleteForm || !confirmDeleteBtn) {
        console.error("Delete modal elements not found");
        return;
    }

    document.querySelectorAll(".delete-button").forEach(button => {
        button.addEventListener("click", function () {

            const memberId = this.dataset.memberId;
            const memberName = this.dataset.memberName;

            console.log("Delete clicked:", memberId, memberName);

            memberIdInput.value = memberId;
            modalBodyText.innerHTML =
                `Are you sure you want to delete <strong>${memberName}</strong>?<br>
                <small class="text-muted">This action cannot be undone.</small>`;
        });
    });

    confirmDeleteBtn.addEventListener("click", function () {
        console.log("Confirm delete clicked");
        deleteForm.submit();
    });
});