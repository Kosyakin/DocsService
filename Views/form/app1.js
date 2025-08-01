//function getActiveFormElement(name) {
//    const activeForm = document.querySelector(`.form-container:not(.hidden)`);
//    return activeForm.querySelector('[]')
//}
function setCurrentDate() {

    const dateField = document.getElementById('date');
    if (!dateField.value) { // Устанавливаем только если поле пустое
        const today = new Date();
        const formattedDate = today.toISOString().substr(0, 10);
        dateField.value = formattedDate;
        
    }
    
}

function setCurrentDate1() {

    const dateField = document.getElementById('date1');
    if (!dateField.value) { // Устанавливаем только если поле пустое
        const today = new Date();
        const formattedDate = today.toISOString().substr(0, 10);
        dateField.value = formattedDate;

    }

}

function setCurrentDate2() {

    const dateField = document.getElementById('date2');
    if (!dateField.value) { // Устанавливаем только если поле пустое
        const today = new Date();
        const formattedDate = today.toISOString().substr(0, 10);
        dateField.value = formattedDate;

    }
}

function toggleDropdown(currentGroup) {
    console.log("toggleDropdown");
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
    const instructionTypeInput1 = document.getElementById('instructionType1');
    const instructionTypeInput2 = document.getElementById('instructionType2');



    
   

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
        const reasonInput = document.querySelector('input[name="reason"]');
        const localActInput = document.querySelector('input[name="localAct"]');


        // Элементы для локального акта
        const localActDropdownGroup = document.getElementById('localActDropdownGroup');

        const localActInputFieldGroup = document.getElementById('localActInputFieldGroup');

        const localActHiddenInput = document.getElementById('localAct');

        const localActTextInput = document.getElementById('localActInputField');

        const selectedType = this.value;
        const isSpecialType = selectedType === 'Внеплановый' || selectedType === 'Целевой';

        // --- Обновляем "Причину" ---
        const reason = reasonMap[selectedType];
        reasonInput.value = reason;

        localActHiddenInput.value = ''; // Очищаем скрытое поле
        localActTextInput.value = '';   // Очищаем текстовое поле 
        document.querySelector('#localActDropdownGroup .custom-select').textContent = 'Наименование локального акта';

        if (selectedType == 'Внеплановый' || selectedType == 'Целевой') {

            reasonInput.classList.add('required-field');
            reasonInput.placeholder = "Заполните номер приказа Общества";

            localActDropdownGroup.style.display = 'none';
            localActInputFieldGroup.style.display = 'block';

            localActTextInput.value = 'СТО 07-12';
            
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

            reasonInput.addEventListener('input', function () {
                if (this.value.trim() !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });
        }
        
    });

    instructionTypeInput1.addEventListener('change', function () {
        const reasonInput1 = document.getElementById('reason1');
        //const localActInput = document.querySelector('input[name="localAct"]');


        // Элементы для локального акта
        const localActDropdownGroup1 = document.getElementById('localActDropdownGroup1');

        const localActInputFieldGroup1 = document.getElementById('localActInputFieldGroup1');

        //const localActHiddenInput = document.getElementById('localAct');

        const localActTextInput1 = document.getElementById('localActInputField1');


        const selectedType = this.value;
        //const isSpecialType = selectedType === 'Внеплановый' || selectedType === 'Целевой';

        // --- Обновляем "Причину" ---
        const reason = reasonMap[selectedType];
        reasonInput1.value = reason;

        if (selectedType == 'Внеплановый' || selectedType == 'Целевой') {

            reasonInput1.classList.add('required-field');
            reasonInput1.placeholder = "Заполните номер приказа Общества";

            localActDropdownGroup1.style.display = 'none';
            localActInputFieldGroup1.style.display = 'block';

            localActTextInput1.value = 'СТО 07-12';

            localActTextInput1.addEventListener('input', function () {
                if (this.value !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });

            reasonInput1.addEventListener('input', function () {
                if (this.value.trim() !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });

        } else {
            // Убираем красные границы
            reasonInput1.classList.remove('required-field');
            //localActTextInput.classList.remove('required-field');

            // Показываем выпадающий список
            localActDropdownGroup1.style.display = 'block';
            localActInputFieldGroup1.style.display = 'none';

            reasonInput1.addEventListener('input', function () {
                if (this.value.trim() !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });
        }

    });

    instructionTypeInput2.addEventListener('change', function () {
        const reasonInput2 = document.getElementById('reason2');
        const selectedType = this.value;
        //const isSpecialType = selectedType === 'Внеплановый' || selectedType === 'Целевой';

        // --- Обновляем "Причину" ---
        const reason = reasonMap[selectedType];
        reasonInput2.value = reason;

        //localActHiddenInput.value = ''; // Очищаем скрытое поле
        //localActTextInput.value = '';   // Очищаем текстовое поле 
        //document.querySelector('#localActDropdownGroup .custom-select').textContent = 'Наименование локального акта';

        if (selectedType == 'Внеплановый' || selectedType == 'Целевой') {

            reasonInput2.classList.add('required-field');
            reasonInput2.placeholder = "Заполните номер приказа Общества";

            //localActDropdownGroup.style.display = 'none';
            //localActInputFieldGroup.style.display = 'block';

            //localActTextInput.value = 'СТО 07-12';

            //localActTextInput.addEventListener('input', function () {
            //    if (this.value !== '') {
            //        this.classList.remove('required-field');
            //    } else {
            //        this.classList.add('required-field');
            //    }
            //});

            reasonInput2.addEventListener('input', function () {
                if (this.value.trim() !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });

        } else {
            // Убираем красные границы
            reasonInput2.classList.remove('required-field');
            //localActTextInput.classList.remove('required-field');

            //// Показываем выпадающий список
            //localActDropdownGroup.style.display = 'block';
            //localActInputFieldGroup.style.display = 'none';

            reasonInput2.addEventListener('input', function () {
                if (this.value.trim() !== '') {
                    this.classList.remove('required-field');
                } else {
                    this.classList.add('required-field');
                }
            });
        }

    });
});


// При загрузке страницы
window.addEventListener('load', setCurrentDate);
window.addEventListener('load', setCurrentDate1);
window.addEventListener('load', setCurrentDate2);

// Или при фокусе на поле, если оно пустое
//document.getElementById('date').addEventListener('focus', function () {
//    if (!this.value) setCurrentDate();
//});



document.getElementById('dataForm').addEventListener('submit', async function (e) {
    console.log("Событие submit вызвано!");
    
    

    try {
        const formData = new FormData(this);
        formData.append('formId', this.id);
        if (formData.has('localAct')) {
            const localActValues = formData.getAll('localAct');
            formData.delete('localAct');

            // Добавляем только непустые значения обратно
            localActValues.forEach(value => {
                if (value) formData.append('localAct', value);
            });
        }

       
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

document.getElementById('dataForm1').addEventListener('submit', async function (e) {
    console.log("Событие submit1 вызвано!");



    try {
        const formData = new FormData(this);
        formData.append('formId', this.id);


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
        submitBtn1.disabled = false;
        submitBtn1.textContent = 'Сформировать файл';
    }
});

document.getElementById('dataForm2').addEventListener('submit', async function (e) {
    console.log("Событие submit2 вызвано!");



    try {
        const formData = new FormData(this);
        formData.append('formId', this.id);

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
        submitBtn2.disabled = false;
        submitBtn2.textContent = 'Сформировать файл';
    }
});



document.addEventListener('DOMContentLoaded', function () {
    const submitBtn = document.getElementById('submitBtn');
    const submitBtn1 = document.getElementById('submitBtn1');
    const submitBtn2 = document.getElementById('submitBtn2');
    if (!submitBtn) return;

    function checkFormValidity(formId) {

        switch (formId) {
            case 'dataForm':
                const date = document.getElementById('date').value;
                const instructionType = document.getElementById('instructionType').value;
                const reason = document.querySelector('input[name="reason"]').value;
                const localAct = document.getElementById('localAct').value;
                const localActInput = document.getElementById("localActInputField").value;
                const anyEmployeeSelected = document.querySelector('input[name="employees"]:checked') !== null;

                const isFormValid = date && instructionType && reason && (localAct || localActInput) && anyEmployeeSelected;
                submitBtn.disabled = !isFormValid;

                break;

            case 'dataForm1':
                const date1 = document.getElementById('date1').value;
                const instructionType1 = document.getElementById('instructionType1').value;
                const reason1 = document.getElementById('reason1').value;
                const localAct1 = document.getElementById('localAct1').value;
                const localActInput1 = document.getElementById("localActInputField1").value;
                const anyEmployeeSelected1 = document.querySelector('input[name="employees1"]:checked') !== null;

                const isFormValid1 = date1 && instructionType1 && reason1 && (localAct1 || localActInput1) && anyEmployeeSelected1;
                submitBtn1.disabled = !isFormValid1;

                break;

            case 'dataForm2':
                const date2 = document.getElementById('date2').value;
                const instructionType2 = document.getElementById('instructionType2').value;
                const reason2 = document.getElementById('reason2').value;
                const localAct2 = document.getElementById('localAct2').value;
                const localActInput2 = document.getElementById("localActInputField2").value;
                const anyEmployeeSelected2 = document.querySelector('input[name="employees2"]:checked') !== null;

                const isFormValid2 = date2 && instructionType2 && reason2 && (localAct2 || localActInput2) && anyEmployeeSelected2;
                submitBtn2.disabled = !isFormValid2;

                break;

            default:
                console.log("Default")
        }
        
    }

    // --- Назначаем обработчики ---

    // Дата
    document.getElementById('date').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById('date1').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById('date2').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });

    // Тип инструктажа (hidden input)
    document.getElementById('instructionType').addEventListener('change', function (event) {
        
        const formId = event.target.form.id;
        console.log(`instructionType: ${formId}`);
        checkFormValidity(formId);
    });
    document.getElementById('instructionType1').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById('instructionType2').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });

    // Причина (текстовое поле)
    document.getElementById("reason").addEventListener('input', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById("reason1").addEventListener('input', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById("reason2").addEventListener('input', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });

    // Локальный акт (hidden input)
    document.getElementById('localAct').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById('localAct1').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById('localAct2').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });

    document.getElementById('localActInputFieldGroup').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId)
    });
    document.getElementById('localActInputFieldGroup1').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });
    document.getElementById('localActInputFieldGroup2').addEventListener('change', function (event) {
        const formId = event.target.form.id;
        checkFormValidity(formId);
    });

    // Чекбоксы сотрудников
    document.querySelectorAll('input[name="employees"]').forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            const formId = event.target.form.id;
            checkFormValidity(formId);
        });
    });
    document.querySelectorAll('input[name="employees1"]').forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            const formId = event.target.form.id;
            checkFormValidity(formId);
        });
    });
    document.querySelectorAll('input[name="employees2"]').forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            const formId = event.target.form.id;
            checkFormValidity(formId);
        });
    });

    

    // --- Проверка при загрузке ---
    checkFormValidity();
});

document.querySelectorAll('.tab').forEach(tab => {
    tab.addEventListener('click', function () {
        const tabId = this.getAttribute('data-tab');

        document.querySelectorAll('.tab').forEach(t => {
            t.classList.remove('active');
        });

        this.classList.add('active');

        document.querySelectorAll('.form-container').forEach(c => c.classList.add('hidden'));
        //document.querySelectorAll('.form-container').forEach(c => console.log(c.classList));

        document.querySelector(`.form-container[data-tab-content="${tabId}"]`).classList.remove('hidden');
        //document.querySelectorAll('.form-container').forEach(c => console.log(c.classList));
        //console.log("Форма сменилась");
    })
});


document.getElementById('edit').addEventListener('click', function (event) {
    event.preventDefault();
    console.log("Нажата кнопка edit");

    document.querySelectorAll('.form-container').forEach(c => c.classList.add('hidden'));
    document.getElementById('edit-employees').classList.remove('hidden');
    document.getElementById('form-tabs').classList.add('hidden');
})