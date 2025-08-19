//document.getElementById('loginForm').addEventListener('submit', async function (event) {
//    event.preventDefault();

//    // 1. Собираем данные формы
//    const formData = {
//        Login: document.getElementById('login').value,
//        Password: document.getElementById('password').value
//    };

//    // 2. Получаем CSRF-токен
//    const csrfToken = getCookie("XSRF-TOKEN");
//    console.log(csrfToken);
//    console.log(JSON.stringify(formData));
//    try {
//        // 3. Отправляем запрос
//        const response = await fetch('/Account/Login', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json',
//                'RequestVerificationToken': csrfToken
//            },
//            body: JSON.stringify(formData)
//        });

//        // 4. Обрабатываем ответ
//        if (!response.ok) {
//            throw new Error(`HTTP error! status: ${response.status}`);
//        }

//        const result = await response.json();

//        if (result.success) {
//            window.location.href = '/form.html'; // Перенаправление после успеха
//        } else {
//            document.getElementById('errorMessage').textContent =
//                result.error || 'Ошибка авторизации';
//        }
//    } catch (error) {
//        console.error('Error:', error);
//        document.getElementById('errorMessage').textContent =
//            'Ошибка соединения с сервером';
//    }
//});

//function getCookie(name) {
//    let nameEQ = name + "=";
//    let ca = document.cookie.split(';');
//    for (let i = 0; i < ca.length; i++) {
//        let c = ca[i];
//        while (c.charAt(0) == ' ') c = c.substring(1);
//        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
//    }
//    return null;
//}