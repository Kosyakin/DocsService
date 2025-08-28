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





// Добавьте этот код в ваш app.js

// Функция для проверки валидности формы регистрации
function checkRegisterFormValidity() {
    console.log('Checking register form validity...');

    const requiredFields = [
        'regLastName',
        'regFirstName',
        'regDocumentNumber',
        'regPosition',
        'regEmail',
        'regPassword',
        'regConfirmPassword'
    ];

    const registerBtn = document.getElementById('register-btn');

    // Проверяем все обязательные поля
    const allFilled = requiredFields.every(fieldId => {
        const field = document.getElementById(fieldId);
        const isFilled = field && field.value.trim() !== '';
        console.log(fieldId, 'filled:', isFilled);
        return isFilled;
    });

    // Дополнительная проверка совпадения паролей
    const password = document.getElementById('regPassword').value;
    const confirmPassword = document.getElementById('regConfirmPassword').value;
    const passwordsMatch = password === confirmPassword && password !== '';
    console.log('Passwords match:', passwordsMatch);

    // Проверка email на валидность
    const email = document.getElementById('regEmail').value;
    const emailValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    console.log('Email valid:', emailValid);

    const isFormValid = allFilled && passwordsMatch && emailValid;
    console.log('Form valid:', isFormValid);

    // Обновляем состояние кнопки
    registerBtn.disabled = !isFormValid;

    if (isFormValid) {
        registerBtn.classList.remove('disabled');
    } else {
        registerBtn.classList.add('disabled');
    }

    return isFormValid;
}

// Функция для показа ошибок валидации
function showValidationError(message) {
    const errorDiv = document.getElementById('regErrorMessage');
    errorDiv.textContent = message;
    setTimeout(() => {
        errorDiv.textContent = '';
    }, 5000);
}

// Обработчик отправки формы регистрации
// Обработчик отправки формы регистрации
function setupRegisterForm() {
    const registerForm = document.getElementById('registerForm');
    if (!registerForm) {
        console.error('Register form not found!');
        return;
    }

    registerForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        console.log('Register form submitted');

        if (!checkRegisterFormValidity()) {
            showValidationError('Пожалуйста, заполните все поля корректно');
            return;
        }

        const formData = {
            lastName: document.getElementById('regLastName').value.trim(),
            firstName: document.getElementById('regFirstName').value.trim(),
            middleName: document.getElementById('regMiddleName').value.trim() || null,
            documentNumber: document.getElementById('regDocumentNumber').value.trim(),
            position: document.getElementById('regPosition').value.trim(),
            email: document.getElementById('regEmail').value.trim(),
            password: document.getElementById('regPassword').value
        };

        // Проверка совпадения паролей
        const password = document.getElementById('regPassword').value;
        const confirmPassword = document.getElementById('regConfirmPassword').value;

        if (password !== confirmPassword) {
            showValidationError('Пароли не совпадают');
            return;
        }

        try {
            const response = await fetch('/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
                credentials: 'include'
            });

            if (response.ok) {
                
                alert('Регистрация успешна! Теперь вы можете войти.');

                // Очищаем все поля формы
                registerForm.reset();

                // Деактивируем кнопку
                const registerBtn = document.getElementById('register-btn');
                registerBtn.disabled = true;
                registerBtn.classList.add('disabled');

                // Очищаем сообщения об ошибках
                document.getElementById('regErrorMessage').textContent = '';

                // Перенаправляем на страницу логина
                window.location.href = '/';

            } else {
                const error = await response.json();
                showValidationError(error.message || 'Ошибка регистрации');
            }
        } catch (error) {
            console.error('Network error:', error);
            showValidationError('Ошибка сети. Попробуйте позже.');
        }
    });
}

// Добавляем обработчики событий для полей формы регистрации
function setupRegisterFields() {
    // Инициализируем кнопку как disabled
    const registerBtn = document.getElementById('register-btn');
    if (registerBtn) {
        registerBtn.disabled = true;
        registerBtn.classList.add('disabled');
    }

    // Список полей для отслеживания
    const registerFields = [
        'regLastName', 'regFirstName', 'regMiddleName',
        'regDocumentNumber', 'regPosition', 'regEmail',
        'regPassword', 'regConfirmPassword'
    ];

    // Добавляем обработчики input для всех полей
    registerFields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (field) {
            field.addEventListener('input', function () {
                console.log('Field changed:', fieldId, field.value);
                checkRegisterFormValidity();

                // Специальная проверка для email
                if (fieldId === 'regEmail' && field.value.trim() !== '') {
                    const emailValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(field.value);
                    if (!emailValid) {
                        showValidationError('Введите корректный email');
                    } else {
                        document.getElementById('regErrorMessage').textContent = '';
                    }
                }

                // Проверка совпадения паролей
                if (fieldId === 'regPassword' || fieldId === 'regConfirmPassword') {
                    const password = document.getElementById('regPassword').value;
                    const confirmPassword = document.getElementById('regConfirmPassword').value;

                    if (password !== '' && confirmPassword !== '' && password !== confirmPassword) {
                        showValidationError('Пароли не совпадают');
                    } else {
                        document.getElementById('regErrorMessage').textContent = '';
                    }
                }
            });
        } else {
            console.error('Field not found:', fieldId);
        }
    });
}

// Модифицируем функцию переключения вкладок
function setupAuthTabs() {
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

                // При переключении на регистрацию инициализируем форму
                if (tabName === 'register') {
                    setTimeout(() => {
                        setupRegisterFields();
                        checkRegisterFormValidity();
                    }, 100);
                }
            }

            // Очищаем сообщения об ошибках
            document.getElementById('errorMessage').textContent = '';
            document.getElementById('regErrorMessage').textContent = '';
        });
    });
}

// Инициализация при загрузке документа
document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM loaded, setting up forms...');

    // Настраиваем вкладки
    setupAuthTabs();

    // Настраиваем форму регистрации
    setupRegisterForm();
    setupRegisterFields();

    // Настраиваем форму авторизации (существующий код)
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
    }

    if (loginInput && passwordInput && submitBtn) {
        loginInput.addEventListener('input', checkValid);
        passwordInput.addEventListener('input', checkValid);
        checkValid();
    }

    // Обработчик формы авторизации
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            // ... существующий код авторизации ...
        });
    }

    // Первоначальная проверка
    setTimeout(checkRegisterFormValidity, 100);
});