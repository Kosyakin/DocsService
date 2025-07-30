
function setCurrentDate() {
    const dateField = document.getElementById('date');
    if (!dateField.value) { // Устанавливаем только если поле пустое
        const today = new Date();
        const formattedDate = today.toISOString().substr(0, 10);
        dateField.value = formattedDate;
    }
    
}

function toggleDropdown(currentGroup) {
    
    //Закрываем все другие выпадающие списки
    document.querySelectorAll('.input-group').forEach(group => {
        if (group !== currentGroup) {
            group.classList.remove('active');
        }
    });

    //Открываем/закрываем текущий список
    currentGroup.classList.toggle('active');
   


}

document.addEventListener('click', (e) => {
    if (!e.target.closest('.input-group')) {
        document.querySelectorAll('.input-group').forEach(group => {
            group.classList.remove('active');
        });
    }
});


function selectItem(element, value) {
    const inputGroup = element.closest('.input-group');
    const customSelect = inputGroup.querySelector('.custom-select');

    // Устанавливаем отображаемый текст
    customSelect.textContent = element.textContent;

    // Устанавливаем значение в hidden input
    const hiddenInput = inputGroup.querySelector('input[type="hidden"]');
    if (hiddenInput) {
        hiddenInput.value = value;
        
        hiddenInput.dispatchEvent(new Event('change'));
    }

    // Закрываем выпадающий список
    inputGroup.classList.remove('active');
    

}

document.addEventListener('DOMContentLoaded', function () {
    const instructionTypeInput = document.getElementById('instructionType');
    const reasonInput = document.querySelector('input[name="reason"]');
    const localActInput = document.querySelector('input[name="localAct"]'); 
    

    // Элементы для локального акта
    const localActDropdownGroup = document.getElementById('localActDropdownGroup');
    
    const localActInputFieldGroup = document.getElementById('localActInputFieldGroup');
    
    const localActHiddenInput = document.getElementById('localAct');
    
    const localActTextInput = document.getElementById('localActInputField');
    

    // Находим отображаемый элемент (div с текстом)
    const localActDisplay = localActInput
        .closest('.input-group')
        .querySelector('.custom-select');

    if (!reasonInput) {
        console.error('Поле "reason" не найдено');
        return;
    }

    if (!localActInput || !localActDisplay) {
        console.error('Элементы для "localAct" не найдены');
        return;
    }

    // Таблица соответствия: тип инструктажа - причина
    const reasonMap = {
        Первичный: 'Приём на работу',
        Повторный: '-',
        Внеплановый: '',
        Целевой: ''
    };

    // Таблица соответствия: тип инструктажа - локальный акт
    const localActMap = {
        Внеплановый: 'СТО 07-12',
        Целевой: 'СТО 07-12'
    };

    instructionTypeInput.addEventListener('change', function () {
        const selectedType = this.value;
        const isSpecialType = selectedType === 'Внеплановый' || selectedType === 'Целевой';

        // --- Обновляем "Причину" ---
        const reason = reasonMap[selectedType];
        reasonInput.value = reason;

        localActHiddenInput.value = ''; // Очищаем скрытое поле
        localActTextInput.value = '';   // Очищаем текстовое поле 
        document.querySelector('#localActDropdownGroup .custom-select').textContent = 'Наименование локального акта';

        if (selectedType == 'Внеплановый' || selectedType == 'Целевой') {

            // Добавляем красные границы
            reasonInput.classList.add('required-field');
            reasonInput.placeholder = "Заполните номер приказа";
            
            //reasonInput.value = "Заполните номер приказа";

            // Показываем текстовое поле
            localActDropdownGroup.style.display = 'none';
            localActInputFieldGroup.style.display = 'block';

            // Устанавливаем placeholder
            //localActTextInput.placeholder = 'Введите номер СТО, например: СТО 07-12';
            localActTextInput.value = 'СТО 07-12';
            //localActTextInput.classList.add('required-field');

            //// Переносим значение из скрытого поля в текстовое
            //localActTextInput.value = localActHiddenInput.value;

            // Обновляем скрытое поле при изменении текстового
            //localActTextInput.addEventListener('input', function () {
            //    localActHiddenInput.value = this.value;
            //});

            
            localActTextInput.addEventListener('input', function () {
                if (this.value !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });

            reasonInput.addEventListener('input', function () {
                if (this.value.trim() !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });
            
        } else {
            // Убираем красные границы
            reasonInput.classList.remove('required-field');
            localActTextInput.classList.remove('required-field');

            // Показываем выпадающий список
            localActDropdownGroup.style.display = 'block';
            localActInputFieldGroup.style.display = 'none';

            // Сбрасываем значение для стандартных типов
            //localActHiddenInput.value = '';
            //document.querySelector('#localActDropdownGroup .custom-select').textContent = 'Наименование локального акта';
        }
        
    });
});


// При загрузке страницы
window.addEventListener('load', setCurrentDate);

// Или при фокусе на поле, если оно пустое
document.getElementById('date').addEventListener('focus', function () {
    if (!this.value) setCurrentDate();
});



document.getElementById('dataForm').addEventListener('submit', async function (e) {
    console.log("Событие submit вызвано!");
    
    

    try {
        const formData = new FormData(this);

       
        for (let [key, value] of formData.entries()) {
            console.log(key, value);
        }

        const response = await fetch(this.action, {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'Инструктажи.docx';
            document.body.appendChild(a);
            a.click();
            setTimeout(() => {
                document.body.removeChild(a);
                window.URL.revokeObjectURL(url);
            }, 100);
        } else {
            const error = await response.text();
            document.getElementById('errorMessage').textContent = error;
        }
    } catch (error) {
        console.error('Ошибка:', error);
        document.getElementById('errorMessage').textContent =
            'Ошибка при отправке формы: ' + error.message;
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = 'Сформировать файл';
    }
});

//function checkFormValidity() {
//    const date = document.getElementById('date').value;
//    const instructionType = document.getElementById('instructionType').value;
//    const reason = document.querySelector('input[name="reason"]').value;
//    const localAct = document.getElementById('localAct').value;
//    const localActInput = document.getElementById("localActInputFieldGroup").value;
//    const anyEmployeeSelected = document.querySelector('input[name="employees"]:checked') !== null;

//    const isFormValid = date && instructionType && reason && (localAct || localActInput) && anyEmployeeSelected;
//    console.log(isFormValid);

//    const submitBtn = document.getElementById("submitBtn");
//    submitBtn.disabled = !isFormValid;
//}




//// При изменении типа инструктажа (срабатывает при выборе)
//instructionTypeInput.addEventListener('change', checkFormValidity);

//// При изменении "Причины" (ввод текста)
//reasonInput.addEventListener('input', checkFormValidity);

//// При изменении "Локального акта"
//localActInput.addEventListener('change', checkFormValidity);

//// При клике на чекбоксы сотрудников
//employeeCheckboxes.forEach(checkbox => {
//    checkbox.addEventListener('change', checkFormValidity);
//});


document.addEventListener('DOMContentLoaded', function () {
    const submitBtn = document.getElementById('submitBtn');
    if (!submitBtn) return;

    function checkFormValidity() {
        const date = document.getElementById('date').value;
        const instructionType = document.getElementById('instructionType').value;
        const reason = document.querySelector('input[name="reason"]').value;
        const localAct = document.getElementById('localAct').value;
        const localActInput = document.getElementById("localActInputField").value;
        const anyEmployeeSelected = document.querySelector('input[name="employees"]:checked') !== null;

        const isFormValid = date && instructionType && reason && (localAct || localActInput) && anyEmployeeSelected;
        submitBtn.disabled = !isFormValid;
    }

    // --- Назначаем обработчики ---

    // Дата
    document.getElementById('date').addEventListener('change', checkFormValidity);

    // Тип инструктажа (hidden input)
    document.getElementById('instructionType').addEventListener('change', checkFormValidity);

    // Причина (текстовое поле)
    document.querySelector('input[name="reason"]').addEventListener('input', checkFormValidity);

    // Локальный акт (hidden input)
    document.getElementById('localAct').addEventListener('change', checkFormValidity);

    document.getElementById('localActInputFieldGroup').addEventListener('change', checkFormValidity);

    // Чекбоксы сотрудников
    document.querySelectorAll('input[name="employees"]').forEach(checkbox => {
        checkbox.addEventListener('change', checkFormValidity);
    });

    // --- Проверка при загрузке ---
    checkFormValidity();
});
