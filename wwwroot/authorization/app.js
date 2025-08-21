document.addEventListener('DOMContentLoaded', function () {
    const loginInput = document.getElementById('login');
    const passwordInput = document.getElementById('password');
    const submitBtn = document.getElementById('login-btn');

    function checkValid() {
        const loginFilled = loginInput.value.trim() !== '';
        const passwordFilled = passwordInput.value.trim() !== '';

        submitBtn.disabled = !(loginFilled && passwordFilled);
        console.log(submitBtn.disabled);

        if (!submitBtn.disabled) {
            submitBtn.classList.add('disabled');
        } else {
            submitBtn.classList.remove('disabled');
        }
    }

    loginInput.addEventListener('input', checkValid);
    passwordInput.addEventListener('input', checkValid);

    checkValid();
});