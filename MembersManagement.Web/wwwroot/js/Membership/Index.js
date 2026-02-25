document.addEventListener('DOMContentLoaded', function () {
    // ----------------------------
    // Delete Modal
    // ----------------------------
    var deleteButtons = document.querySelectorAll('.delete-btn');
    var deleteModalElem = document.getElementById('deleteModal');
    var deleteModal = new bootstrap.Modal(deleteModalElem);

    var membershipNameElem = document.getElementById('membershipName');
    var membershipIdInput = document.getElementById('membershipId');

    deleteButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var id = btn.getAttribute('data-id');
            var name = btn.getAttribute('data-name');

            membershipNameElem.textContent = name;
            membershipIdInput.value = id;

            deleteModal.show();
        });
    });

    // ----------------------------
    // Auto-hide Success Message
    // ----------------------------
    var alert = document.getElementById('successAlert');
    if (alert) {
        setTimeout(function () {
            alert.classList.add('fade');
            alert.classList.add('show'); // Required by Bootstrap
            alert.style.transition = 'opacity 0.5s ease-out';
            alert.style.opacity = '0';
            setTimeout(function () {
                alert.remove();
            }, 500);
        }, 3000);
    }
});