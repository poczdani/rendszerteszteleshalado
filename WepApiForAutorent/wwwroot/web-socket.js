// WebSocket kapcsolat létrehozása
// A LaunchSettings.json-be a 7090-es port van beállítva,, ezért erre kell figyelni a websocketnek különben nem fog működni
const socket = new WebSocket('wss://localhost:7090/ws');
const BASE_URL = 'https://localhost:7090/api/Car';

socket.addEventListener('open', function (event) {
    console.log('WebSocket connection opened:', event);

    //Üzenet küldése a szervernek, csak akkor ha a kapcsolat már nyitva van, különben hibára futhat
    sendMessageToServer('Kérlek, küldj nekem egy akciós ajánlatot!');
});

// Hiba esemény kezelése ( Logolás a hibakeresés miatt) 
socket.addEventListener('error', function (event) {
    console.error('WebSocket error:', event);
});

// Üzenet fogadása a szerverről
socket.addEventListener('message', function (event) {
    console.log('Received message:', event.data);

    displayOffer(event.data);
});

// Üzenet küldése a szervernek
function sendMessageToServer(message) {
    if (socket.readyState === WebSocket.OPEN) {
        socket.send(message);
    } else {
        console.error('A WebSocket nem nyitott. Nem tudott üzenetet küldeni');
    }
}

function listCars() {
    fetch(`${BASE_URL}/list`)
        .then(response => response.json())
        .then(cars => {
            const carList = document.getElementById('carList');
            carList.innerHTML = '';

            cars.forEach(car => {
                const carItem = document.createElement('div');
                carItem.textContent = `${car.Brand} ${car.Model}`; // Módosítás: Brand és Model használata
                carList.appendChild(carItem);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Hiba történt az autók lekérése során.');
        });
}

function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    fetch(`${BASE_URL}/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Hibás felhasználónév vagy jelszó.');
            }
            return response.json();
        })
        .then(data => {
            if (data && data.message) {
                alert(data.message);
            } else {
                throw new Error('Érvénytelen válasz formátum.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Hiba történt a bejelentkezés során.');
        });
}


function getUserRentals(userId) {
    fetch(`${BASE_URL}/user/${userId}/rentals`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Hiba történt a kéréskor.');
            }
            return response.json();
        })
        .then(data => {
            displayUserRentals(data);
        })
        .catch(error => {
            console.error('Hiba:', error);
            alert('Hiba történt a felhasználó kölcsönzéseinek lekérése során.');
        });
}

function checkCarAvailability() {
    const carId = document.getElementById('carId').value;
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;

    fetch(`${BASE_URL}/${carId}/availability`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Hiba történt az autó elérhetőségének ellenőrzése során.');
            }
            return response.json();
        })
        .then(data => {
            displayCarAvailability(data);
        })
        .catch(error => {
            console.error('Hiba:', error);
            alert('Hiba történt az autó elérhetőségének ellenőrzése során.');
        });
}

function displayCarAvailability(availability) {
    const carAvailabilityContainer = document.getElementById('carAvailability');

    if (carAvailabilityContainer) {
        carAvailabilityContainer.innerHTML = '';

        const availabilityList = document.createElement('ul');

        availability.availableDates.forEach(date => {
            const dateItem = document.createElement('li');
            dateItem.textContent = date;
            availabilityList.appendChild(dateItem);
        });

        carAvailabilityContainer.appendChild(availabilityList);
    }
}



function reserveCar() {
    const carId = document.getElementById('resCarId').value;
    const startDate = document.getElementById('resStartDate').value;
    const endDate = document.getElementById('resEndDate').value;
    const userId = document.getElementById('ResUserId').value;

    const reservationData = {
        carID: parseInt(carId),
        startDate: startDate,
        endDate: endDate,
        userID: userId
    };

    //console.log(carId);
    //console.log(startDate);
    //console.log(endDate);
    //console.log(userId);
    //console.log(reservationData);


    fetch('https://localhost:7090/api/Car/reserve', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(reservationData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Hiba történt a foglalás során.');
            }
            return response.json();
        })
        .then(data => {
            if (data && data.message) {
                console.log(data);
                console.log(data.message);
                // Sikeres foglalás esetén megjelenítheted az üzenetet vagy az eredményt
                displayReservationResultFromServer(data.message);
            } else {
                throw new Error('Érvénytelen válasz formátum.');
            }
        })
        .catch(error => {
            console.error('Hiba:', error);
            alert('Hiba történt az autó foglalása során.');
        });
}


function displayReservationResultFromServer(message) {
    const reservationResultContainer = document.getElementById('reservationResult');

    if (reservationResultContainer) {
        reservationResultContainer.innerHTML = '';

        const resultMessage = document.createElement('div');
        resultMessage.textContent = message;

        reservationResultContainer.appendChild(resultMessage);
    }
}









function displayUserRentals(rentals) {
    const userRentalsContainer = document.getElementById('userRentals');

    if (userRentalsContainer) {
        // Tiszta az előző tartalom
        userRentalsContainer.innerHTML = '';

        // Üres-e a rentals tömb
        if (rentals.length === 0) {
            const noRentalsMessage = document.createElement('div');
            noRentalsMessage.textContent = 'Nincsenek kölcsönzések.';
            userRentalsContainer.appendChild(noRentalsMessage);
        } else {
            // Kölcsönzések listájának megjelenítése
            const rentalsList = document.createElement('ul');

            rentals.forEach(rental => {
                const rentalItem = document.createElement('li');
                rentalItem.textContent = `${rental.car} - Kezdés: ${rental.startDate}, Vége: ${rental.endDate}`;
                rentalsList.appendChild(rentalItem);
            });

            userRentalsContainer.appendChild(rentalsList);
        }
    }
}


// Akciós ajánlat megjelenítése a felhasználónak
function displayOffer(offerMessage) {
    const offerContainer = document.getElementById('offer-container');

    if (offerContainer) {
        // Tiszta az előző tartalom
        offerContainer.innerHTML = '';

        // Új akciós ajánlat hozzáadása
        const offerElement = document.createElement('div');
        offerElement.textContent = offerMessage;

        offerContainer.appendChild(offerElement);
    }
}
