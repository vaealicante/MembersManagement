document.addEventListener("DOMContentLoaded", function () {
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
                // Shows the red border
                this.classList.add('is-invalid');
                this.classList.remove('is-valid');
            } else if (this.value !== "") {
                // If you want a green border without the check icon:
                this.classList.add('is-valid');
                this.classList.remove('is-invalid');
            } else {
                this.classList.remove('is-invalid', 'is-valid');
            }
        });
    }
});