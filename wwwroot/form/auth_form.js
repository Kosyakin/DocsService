document.createElement("userInfoBtn").addEventListener('click', function () {
    console.log(`Вызвана функция для открытия формы с данными авторизованного пользователя`);
    const infoBlock = document.getElementById('userInfo');
    infoBlock.style.display = infoBlock.style.display == 'none' ? 'block' : 'none';
});
