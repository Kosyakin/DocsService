function toggleDropdown(element) {
    element.classList.toggle("active");
    updateSelectedItems();
}

function updateSelectedItems() {
    const checkboxes = document.querySelectorAll('.dropdown-content input[type="checkbox"]:checked');
    const selectedItems = document.getElementById('selectedItems');

    if (checkboxes.length > 0) {
        const selectedValues = Array.from(checkboxes).map(cb => cb.parentElement.textContent.trim());
        selectedItems.textContent = "Выбрано: " + selectedValues.join(", ");
    } else {
        selectedItems.textContent = "";
    }
}

// Закрываем dropdown при клике вне его
document.addEventListener('click', function (event) {
    if (!event.target.closest('.input-group')) {
        const dropdowns = document.querySelectorAll('.input-group');
        dropdowns.forEach(dropdown => {
            dropdown.classList.remove('active');
        });
    }
});

// Обновляем список при изменении чекбоксов
document.querySelectorAll('.dropdown-content input').forEach(checkbox => {
    checkbox.addEventListener('change', updateSelectedItems);
});
