const apiUrl = "https://localhost:7018/api";
let token = "";

function showToast(message) {
    const toast = document.getElementById("toast");
    toast.textContent = message;
    toast.classList.add("show");

    setTimeout(() => {
        toast.classList.remove("show");
    }, 3000);
}

function updateLoginStatus(isLoggedIn) {
    const status = document.getElementById("loginStatus");
    status.textContent = isLoggedIn ? "Logged in successfully" : "Not logged in";
}

async function login() {
    try {
        const response = await fetch(`${apiUrl}/Auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: document.getElementById("email").value,
                password: document.getElementById("password").value
            })
        });

        if (!response.ok) {
            showToast("Login failed. Check your credentials.");
            return;
        }

        const data = await response.json();
        token = data.token;

        updateLoginStatus(true);
        showToast("Login successful.");
    } catch (error) {
        showToast("Connection error while logging in.");
    }
}

async function addMedication() {
    if (!token) {
        showToast("Please login first.");
        return;
    }

    const name = document.getElementById("name").value;
    const dosage = document.getElementById("dosage").value;
    const frequency = document.getElementById("frequency").value;

    if (!name || !dosage || !frequency) {
        showToast("Please fill all medication fields.");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/Medications`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                name: name,
                dosage: dosage,
                frequency: frequency,
                startDate: new Date().toISOString(),
                isActive: true
            })
        });

        if (!response.ok) {
            showToast("Failed to add medication.");
            return;
        }

        showToast("Medication added successfully.");

        document.getElementById("name").value = "";
        document.getElementById("dosage").value = "";
        document.getElementById("frequency").value = "";

        await getMedications();
    } catch (error) {
        showToast("Connection error while adding medication.");
    }
}

async function getMedications() {
    if (!token) {
        showToast("Please login first.");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/Medications`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            showToast("Failed to load medications.");
            return;
        }

        const medications = await response.json();
        const list = document.getElementById("medicationList");
        list.innerHTML = "";

        if (medications.length === 0) {
            list.innerHTML = `<p>No medications found.</p>`;
            return;
        }

        medications.forEach(med => {
            const card = document.createElement("div");
            card.className = "med-card";

            card.innerHTML = `
                <h3>${med.name}</h3>
                <p><strong>Dosage:</strong> ${med.dosage}</p>
                <p><strong>Frequency:</strong> ${med.frequency}</p>
                <p><strong>Status:</strong> ${med.isActive ? "Active" : "Inactive"}</p>
                <span class="badge">Medication ID: ${med.id}</span>
            `;

            list.appendChild(card);
        });

        showToast("Medications loaded.");
    } catch (error) {
        showToast("Connection error while loading medications.");
    }
}

async function getReminders() {
    if (!token) {
        showToast("Please login first.");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/Reminders/upcoming`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            showToast("Failed to load reminders.");
            return;
        }

        const reminders = await response.json();
        const list = document.getElementById("reminderList");
        list.innerHTML = "";

        if (reminders.length === 0) {
            list.innerHTML = `<p>No upcoming reminders found.</p>`;
            return;
        }

        reminders.forEach(reminder => {
            const card = document.createElement("div");
            card.className = "med-card";

            const nextDose = new Date(reminder.nextDoseTime).toLocaleString();

            card.innerHTML = `
                <h3>${reminder.medicationName}</h3>
                <p><strong>Dosage:</strong> ${reminder.dosage}</p>
                <p><strong>Next Dose:</strong> ${nextDose}</p>
                <p><strong>Hours Until Next:</strong> ${reminder.hoursUntilNext}</p>
                <span class="badge">Upcoming Reminder</span>
            `;

            list.appendChild(card);
        });

        showToast("Reminders loaded.");
    } catch (error) {
        showToast("Connection error while loading reminders.");
    }
}