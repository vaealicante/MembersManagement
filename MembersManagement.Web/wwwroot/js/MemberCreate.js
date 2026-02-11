document.addEventListener("DOMContentLoaded", function () {
    // --- Contact Input Logic ---
    const contactInput = document.getElementById('contactInput');

    if (contactInput) {
        contactInput.addEventListener('input', function (e) {
            let val = this.value.replace(/[^0-9+]/g, '');
            if (val.includes('+')) {
                val = '+' + val.replace(/\+/g, '');
            }
            if (val.length > 13) val = val.slice(0, 13);
            this.value = val;
        });

        contactInput.addEventListener('blur', function () {
            const phRegex = /^(09|\+639)\d{9}$/;

            if (this.value !== "" && !phRegex.test(this.value)) {
                this.classList.add('is-invalid');
                this.classList.remove('is-valid');
            } else if (this.value !== "") {
                this.classList.add('is-valid');
                this.classList.remove('is-invalid');
            } else {
                this.classList.remove('is-invalid', 'is-valid');
            }
        });
    }

    // --- Dynamic Age Validation Logic ---
    const birthDateInput = document.getElementById('BirthDate');

    if (birthDateInput) {
        birthDateInput.addEventListener('change', function () {
            const birthDate = new Date(this.value);
            const today = new Date();

            if (isNaN(birthDate)) return;

            // Precise Max Age Calculation (65y, 6m, 1d)
            // Subtracting from today's date to find the earliest possible valid birthdate
            const maxDate = new Date();
            maxDate.setFullYear(today.getFullYear() - 65);
            maxDate.setMonth(today.getMonth() - 6);
            maxDate.setDate(today.getDate() - 1);

            // Minimum Age Calculation (18y)
            const minDate = new Date();
            minDate.setFullYear(today.getFullYear() - 18);

            const errorSpan = document.querySelector('[data-valmsg-for="BirthDate"]');

            if (birthDate > minDate) {
                showError("Member must be at least 18 years old.");
            } else if (birthDate < maxDate) {
                showError("Member cannot be older than 65 years, 6 months, and 1 day.");
            } else {
                clearError();
            }

            function showError(msg) {
                if (errorSpan) errorSpan.textContent = msg;
                birthDateInput.classList.add('is-invalid');
            }

            function clearError() {
                if (errorSpan) errorSpan.textContent = "";
                birthDateInput.classList.remove('is-invalid');
            }
        });
    }
});