//document.getElementById("fetchBtn").addEventListener("click", async () => {
//    try {
//        const response = await fetch("http://localhost:5126/api/test/hello");

//        if (!response.ok) {
//            throw new Error("Ошибка HTTP: " + response.status);
//        }

//        const data = await response.json();
//        getActiveFormElement("output").textContent = data.message;
//    } catch (error) {
//        console.error("Ошибка:", error);
//        getActiveFormElement("output").textContent = "Не удалось загрузить данные";
//    }
//});

document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Отменяем стандартную отправку формы

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const csrfToken = document.getElementById('csrf-token').value;

    try {
        const response = await fetch('/Account/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': csrfToken // Передаём CSRF-токен
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        });

        if (response.ok) {
            // Перенаправляем на защищённую страницу после успешного входа
            window.location.href = '/Home/Index';
        } else {
            const errorData = await response.json();
            document.getElementById('errorMessage').textContent =
                errorData.error || 'Ошибка авторизации';
        }
    } catch (error) {
        document.getElementById('errorMessage').textContent =
            'Ошибка сети или сервера';
        console.error('Error:', error);
    }
});