$('document').ready(function () {
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#oldPassword');

    togglePassword.addEventListener('click', function (e) {
        // toggle the type attribute
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        // toggle the eye slash icon
        this.classList.toggle('fa-eye-slash');
    });
});

$('document').ready(function () {
    const togglePassword = document.querySelector('#togglePassword2');
    const password = document.querySelector('#newPassword');

    togglePassword.addEventListener('click', function (e) {
        // toggle the type attribute
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        // toggle the eye slash icon
        this.classList.toggle('fa-eye-slash');
    });
});

$('document').ready(function () {
    const togglePassword = document.querySelector('#togglePassword3');
    const password = document.querySelector('#newPassword2');

    togglePassword.addEventListener('click', function (e) {
        // toggle the type attribute
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        // toggle the eye slash icon
        this.classList.toggle('fa-eye-slash');
    });
});

