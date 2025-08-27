document.addEventListener('DOMContentLoaded', function () {
    const loginInput = document.getElementById('login');
    const passwordInput = document.getElementById('password');
    const submitBtn = document.getElementById('login-btn');

    function checkValid() {
        const loginFilled = loginInput.value.trim() !== '';
        const passwordFilled = passwordInput.value.trim() !== '';

        const isFormValid = loginFilled && passwordFilled;

        submitBtn.disabled = !isFormValid;

        
        if (isFormValid) {
            submitBtn.classList.remove('disabled'); 
        } else {
            submitBtn.classList.add('disabled');   
        }

        console.log('Form valid:', isFormValid, 'Button disabled:', submitBtn.disabled);
    }

    loginInput.addEventListener('input', checkValid);
    passwordInput.addEventListener('input', checkValid);

    checkValid();
});

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault(); 

    const loginInput = document.getElementById("login");
    const passwordInput = document.getElementById("password");

    if (!loginInput || !passwordInput) {
        console.error('Input elements not found!');
        return;
    }

    const formData = {
        email: loginInput.value,
        password: passwordInput.value
    };

    console.log('Sending:');

    try {
        const response = await fetch('/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
            credentials: 'include'
        });

        console.log('Response status:', response.status);

        if (response.ok) {
            //console.log(response);
            //const result = await response.json();
            //console.log('Success:', result);
            window.location.href = '/account';
        } else {
            //const error = await response.json();
            //console.error('Error:', error);
            alert('Неверный пароль');
        }
    } catch (error) {
        console.error('Network error:', error);
        
    }
});

// Переключение между вкладками авторизации/регистрации
document.addEventListener('DOMContentLoaded', function () {
    const authTabs = document.querySelectorAll('.auth-tab');
    const authContainers = document.querySelectorAll('.auth-container');

    authTabs.forEach(tab => {
        tab.addEventListener('click', function () {
            const tabName = this.dataset.authTab;

            // Убираем активный класс у всех вкладок
            authTabs.forEach(t => t.classList.remove('active'));
            // Добавляем активный класс текущей вкладке
            this.classList.add('active');

            // Скрываем все контейнеры
            authContainers.forEach(container => {
                container.classList.add('hidden');
                container.classList.remove('active');
            });

            // Показываем соответствующий контейнер
            const activeContainer = document.querySelector(`[data-auth-content="${tabName}"]`);
            if (activeContainer) {
                activeContainer.classList.remove('hidden');
                activeContainer.classList.add('active');
            }
        });
    });
});