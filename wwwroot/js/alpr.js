let alprWs = null;
let debugMode = true;

const WS_URL = `wss://${"localhost"}/ws`;

function log(message, type = 'info') {
    if (!debugMode && type === 'debug') return;
    const prefix = type === 'error' ? '❌' : type === 'success' ? '✅' : type === 'warning' ? '⚠️' : '📨';
    console.log(`${prefix} ${message}`);
}

function initAlprWebSocket() {
    if (alprWs && alprWs.readyState === WebSocket.OPEN) {
        log('WebSocket already connected', 'warning');
        return;
    }

    log('Connecting to WebSocket server...', 'info');
    log(`URL: ${WS_URL}`, 'debug');

    try {
        alprWs = new WebSocket(WS_URL);

        alprWs.onopen = function() {
            log('Connected to ALPR WebSocket', 'success');
            const loadingEl = document.getElementById('plates-loading');
            if (loadingEl) {
                loadingEl.innerHTML = '<i class="fa fa-check-circle"></i> Connected to server, waiting for plates...';
            }

            alprWs.send(JSON.stringify({
                type: 'client_ready',
                message: 'Client is ready to receive plates',
                timestamp: new Date().toISOString()
            }));
            log('Ready message sent to server', 'debug');
        };

        alprWs.onmessage = function(event) {
            try {
                const data = JSON.parse(event.data);
                log(`Message received: ${data.type}`, 'debug');
                console.log('Full message data:', data);

                if (data.type === 'new_plate') {
                    log(`🎯 NEW PLATE DETECTED: ${data.data.plate} (${(data.data.confidence * 100).toFixed(1)}%)`, 'success');
                    console.log('Plate details:', {
                        plate: data.data.plate,
                        confidence: data.data.confidence,
                        camera_id: data.data.camera_id,
                        timestamp: data.data.timestamp
                    });

                    const plate = data.data.plate;
                    const confidence = data.data.confidence;
                    const cameraId = data.data.camera_id;
                    const timestamp = new Date(data.data.timestamp || Date.now()).toLocaleTimeString('fa-IR');

                    if (window.addPlateToList) {
                        log('Calling addPlateToList function', 'debug');
                        window.addPlateToList(plate, confidence, cameraId, timestamp);
                    } else {
                        log('addPlateToList function not found! Make sure _partialVideo.cshtml is loaded', 'error');
                    }
                } else if (data.type === 'history') {
                    log(`History received: ${data.data.length} plates`, 'info');
                    if (window.displayHistory) {
                        window.displayHistory(data.data);
                    }
                } else if (data.type === 'connection_established') {
                    log(`Connection established: ${data.message}`, 'success');
                    if (data.history && data.history.length > 0) {
                        log(`Loaded ${data.history.length} plates from history`, 'info');
                    }
                } else {
                    log(`Unknown message type: ${data.type}`, 'warning');
                }
            } catch (err) {
                log(`Error parsing message: ${err.message}`, 'error');
                console.error('Raw message:', event.data);
            }
        };

        alprWs.onerror = function(error) {
            log(`WebSocket error: ${error}`, 'error');
            console.error('WebSocket error details:', error);
            const loadingEl = document.getElementById('plates-loading');
            if (loadingEl) {
                loadingEl.innerHTML = '<i class="fa fa-exclamation-triangle"></i> Server connection error';
            }
        };

        alprWs.onclose = function(event) {
            log(`WebSocket disconnected. Code: ${event.code}, Reason: ${event.reason || 'No reason'}`, 'warning');
            const loadingEl = document.getElementById('plates-loading');
            if (loadingEl) {
                loadingEl.innerHTML = '<i class="fa fa-refresh fa-spin"></i> Disconnected, retrying...';
            }
            setTimeout(() => {
                log('Attempting to reconnect...', 'info');
                initAlprWebSocket();
            }, 5000);
        };
    } catch (err) {
        log(`Failed to create WebSocket: ${err.message}`, 'error');
    }
}

function closeAlprWebSocket() {
    if (alprWs && alprWs.readyState === WebSocket.OPEN) {
        log('Closing WebSocket connection', 'info');
        alprWs.close();
    }
    alprWs = null;
}

function testPlate(plate = '۱۲۳۴۵۶۷۸', confidence = 0.95) {
    log(`Simulating test plate: ${plate}`, 'info');
    if (window.addPlateToList) {
        window.addPlateToList(plate, confidence, 'test_camera', new Date().toLocaleTimeString('fa-IR'));
        log('Test plate added to list', 'success');
    } else {
        log('addPlateToList not found!', 'error');
    }
}

function checkWS() {
    if (!alprWs) {
        log('WebSocket not initialized', 'warning');
        return;
    }
    const states = ['CONNECTING', 'OPEN', 'CLOSING', 'CLOSED'];
    log(`WebSocket Status: ${states[alprWs.readyState]}`, 'info');
}

window.testPlate = testPlate;
window.checkWS = checkWS;

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        log('DOM loaded, initializing WebSocket...', 'info');
        initAlprWebSocket();
    });
} else {
    log('Page already loaded, initializing WebSocket...', 'info');
    initAlprWebSocket();
}