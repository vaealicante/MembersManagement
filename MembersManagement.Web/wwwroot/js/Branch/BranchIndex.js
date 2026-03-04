document.addEventListener("DOMContentLoaded", function () {

    // =========================
    // DELETE MODAL
    // =========================
    const deleteForm = document.getElementById("deleteForm");
    const branchIdInput = document.getElementById("branchIdInput");
    const modalBodyText = document.getElementById("modal-body-text");
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");

    if (deleteForm && branchIdInput && modalBodyText && confirmDeleteBtn) {

        document.addEventListener("click", function (e) {
            const btn = e.target.closest(".delete-button");
            if (!btn) return;

            branchIdInput.value = btn.dataset.branchId;
            modalBodyText.innerHTML =
                `Are you sure you want to delete <strong>${btn.dataset.branchName}</strong>?`;
        });

        confirmDeleteBtn.addEventListener("click", function () {
            deleteForm.submit(); // ✅ POST /Branch/SoftDelete
        });

    } else {
        console.error("Delete modal elements not found");
    }

    // =========================
    // SUCCESS ALERT AUTO-HIDE
    // =========================
    const alert = document.getElementById("success-alert");
    if (alert) {
        setTimeout(() => {
            alert.classList.add("hide");
            setTimeout(() => alert.remove(), 600);
        }, 3000);
    }

    /* =========================
       SEARCH + PAGINATION + PAGE SIZE
    ========================= */
    const searchInput = document.getElementById("branchSearch");
    const pageSizeSelect = document.getElementById("pageSize");
    const pagination = document.getElementById("pagination");
    const tbody = document.querySelector("#branchTable tbody");

    let currentPage = 1;

    const allData = Array.from(tbody.querySelectorAll("tr")).map(tr => ({
        html: tr.innerHTML,
        text: tr.innerText.toLowerCase()
    }));

    let filteredData = [...allData];

    function renderTable() {
        tbody.innerHTML = "";

        const pageSizeValue = pageSizeSelect.value;
        const pageSize =
            pageSizeValue === "all"
                ? filteredData.length
                : parseInt(pageSizeValue);

        if (filteredData.length === 0) {
            tbody.innerHTML = `
                <tr>
                    <td colspan="4" class="text-center text-muted py-4">
                        No branches found.
                    </td>
                </tr>`;
            pagination.innerHTML = "";
            return;
        }

        const totalPages = Math.ceil(filteredData.length / pageSize);
        currentPage = Math.min(currentPage, totalPages);

        const start = (currentPage - 1) * pageSize;
        const end = start + pageSize;

        filteredData.slice(start, end).forEach(row => {
            const tr = document.createElement("tr");
            tr.innerHTML = row.html;
            tbody.appendChild(tr);
        });

        pagination.innerHTML = "";
        const maxVisible = 2;

        // PREVIOUS
        const prevLi = document.createElement("li");
        prevLi.className = `page-item ${currentPage === 1 ? "disabled" : ""}`;

        const prevA = document.createElement("a");
        prevA.className = "page-link";
        prevA.href = "#";
        prevA.textContent = "Previous";

        prevA.onclick = e => {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                renderTable();
            }
        };

        prevLi.appendChild(prevA);
        pagination.appendChild(prevLi);

        // PAGE NUMBERS + ELLIPSIS
        for (let i = 1; i <= totalPages; i++) {
            const isFirst = i === 1;
            const isLast = i === totalPages;
            const inRange =
                i >= currentPage - maxVisible &&
                i <= currentPage + maxVisible;

            if (isFirst || isLast || inRange) {
                const li = document.createElement("li");
                li.className = `page-item ${i === currentPage ? "active" : ""}`;

                const a = document.createElement("a");
                a.className = "page-link";
                a.href = "#";
                a.textContent = i;

                a.onclick = e => {
                    e.preventDefault();
                    currentPage = i;
                    renderTable();
                };

                li.appendChild(a);
                pagination.appendChild(li);
            }
            else if (
                i === currentPage - maxVisible - 1 ||
                i === currentPage + maxVisible + 1
            ) {
                const ellipsis = document.createElement("li");
                ellipsis.className = "page-item disabled";
                ellipsis.innerHTML = `<span class="page-link">…</span>`;
                pagination.appendChild(ellipsis);
            }
        }

        // NEXT
        const nextLi = document.createElement("li");
        nextLi.className = `page-item ${currentPage === totalPages ? "disabled" : ""}`;

        const nextA = document.createElement("a");
        nextA.className = "page-link";
        nextA.href = "#";
        nextA.textContent = "Next";

        nextA.onclick = e => {
            e.preventDefault();
            if (currentPage < totalPages) {
                currentPage++;
                renderTable();
            }
        };

        nextLi.appendChild(nextA);
        pagination.appendChild(nextLi);
    }

    searchInput?.addEventListener("input", () => {
        const term = searchInput.value.toLowerCase();
        filteredData = allData.filter(r => r.text.includes(term));
        currentPage = 1;
        renderTable();
    });

    pageSizeSelect?.addEventListener("change", () => {
        currentPage = 1;
        renderTable();
    });

    renderTable();
});