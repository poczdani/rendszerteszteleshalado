// WebSocket kapcsolat létrehozása
// A LaunchSettings.json-be a 7090-es port van beállítva,, ezért erre kell figyelni a websocketnek különben nem fog működni
const socket = new WebSocket('wss://localhost:7090/ws');

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
