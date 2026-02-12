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
                // If the field is empty, clear validation (it's optional)
                this.classList.remove('is-invalid', 'is-valid');
            }
        });
    }

    // --- Dynamic Age Validation Logic ---
    const birthDateInput = document.getElementById('BirthDate');

    if (birthDateInput) {
        birthDateInput.addEventListener('change', function () {
            const errorSpan = document.querySelector('[data-valmsg-for="BirthDate"]');
            const submitBtn = document.getElementById('btnSaveMember');

            // --- LOCAL HELPER FUNCTIONS ---
            function showError(msg) {
                if (errorSpan) errorSpan.textContent = msg;
                birthDateInput.classList.add('is-invalid');
                if (submitBtn) submitBtn.disabled = true;
            }

            function clearError() {
                if (errorSpan) errorSpan.textContent = "";
                birthDateInput.classList.remove('is-invalid');
                if (submitBtn) submitBtn.disabled = false;
            }

            // --- OPTIONAL CHECK ---
            // If the user clears the date field, remove errors and allow saving
            if (!this.value) {
                clearError();
                return;
            }

            const birthDate = new Date(this.value);
            const today = new Date();
            today.setHours(0, 0, 0, 0);

            if (isNaN(birthDate)) return;

            // 1. Future Date Validation
            if (birthDate > today) {
                showError("Birthdate cannot be in the future.");
                return;
            }

            // 2. Minimum Age Calculation (18y)
            const minDate = new Date();
            minDate.setFullYear(today.getFullYear() - 18);
            minDate.setHours(0, 0, 0, 0);

            // 3. Precise Max Age Calculation (65y, 6m, 1d)
            const maxDate = new Date();
            maxDate.setFullYear(today.getFullYear() - 65);
            maxDate.setMonth(today.getMonth() - 6);
            maxDate.setDate(today.getDate() - 1);
            maxDate.setHours(0, 0, 0, 0);

            // Validation logic tree
            if (birthDate > minDate) {
                showError("Member must be at least 18 years old.");
            } else if (birthDate < maxDate) {
                showError("Member cannot be older than 65 years, 6 months, and 1 day.");
            } else {
                clearError();
            }
        });
    }
});