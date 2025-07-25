document.getElementById("fetchBtn").addEventListener("click", async () => {
    try {
        const response = await fetch("http://localhost:5126/api/test/hello");

        if (!response.ok) {
            throw new Error("Ошибка HTTP: " + response.status);
        }

        const data = await response.json();
        document.getElementById("output").textContent = data.message;
    } catch (error) {
        console.error("Ошибка:", error);
        document.getElementById("output").textContent = "Не удалось загрузить данные";
    }
});