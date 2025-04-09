import * as turf from 'https://esm.sh/@turf/turf@7.1.0';

const API_PATH = "/api/map";
const MAPBOX_RASTER_URL = "https://basemaps.cartocdn.com/gl/voyager-gl-style/style.json";

MapboxDraw.constants.classes.CANVAS = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE = 'maplibregl-ctrl';
MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

const map = new maplibregl.Map({
    container: 'map',
    style: MAPBOX_RASTER_URL,
    center: [4.729, 52.045],
    zoom: 15,
    dragRotate: false,
    pitchWithRotate: false,
});

const markers = []; // To keep track of added markers
const minDistance = 0.001; // Minimum distance between markers in meters
let lastTap = 0; // To track double-tap events

map.on('click', onMapClick);

function onMapClick(e) {
    const { lngLat } = e;

    // Double tap detection
    const currentTime = new Date().getTime();
    const tapLength = currentTime - lastTap;
    lastTap = currentTime;

    // If a marker already exists, remove it before adding a new one
    if (markers.length > 0) {
        markers[0].remove(); // Remove the existing marker
        markers.length = 0; // Clear the markers array
    }

    // Proceed with placing the marker only if it is at least 2 meters from the existing markers
    if (isTooCloseToOtherMarkers(lngLat)) {
        showAlert('warning', 'Er moet minstens 2 meter afstand zijn tussen markers.');
        return;
    }

    // Add the marker at the clicked position
    const marker = new maplibregl.Marker()
        .setLngLat([lngLat.lng, lngLat.lat])
        .addTo(map);

    // Store the marker in the markers array
    markers.push(marker);

    // Fill the hidden fields with the coordinates
    document.getElementById('LatitudeInput').value = lngLat.lat;
    document.getElementById('LongitudeInput').value = lngLat.lng;

    console.log(`Marker geplaatst op: ${lngLat.lng}, ${lngLat.lat}`);
    console.log(document.getElementById('LatitudeInput').value = lngLat.lat);
    console.log(document.getElementById('LongitudeInput').value);

    onAreaChanged();
}

function onAreaChanged() {
    const startDrawingBtn = document.querySelector('#drawMap');
    if (markers.length > 0) {
        console.log(`Er is ${markers.length} marker geplaatst.`);
    }
}

function isTooCloseToOtherMarkers(lngLat) {
    for (const marker of markers) {
        const markerLngLat = marker.getLngLat();
        const distance = turf.distance(
            turf.point([markerLngLat.lng, markerLngLat.lat]),
            turf.point([lngLat.lng, lngLat.lat])
        );
        if (distance < minDistance) {
            return true; // Found a marker within the minimum distance
        }
    }
    return false; // No markers are too close
}

// CRUD operations
function getMap() {
    try {
        const allMaps = `${BACKEND_URL}${API_PATH}`;
        fetch(`${allMaps}`, {
            method: 'GET',
        })
            .then(response => response.json())
            .then(data => {
                mapId = data[0].id;
                fetch(`${BACKEND_URL}${API_PATH}/${data[0].id}`, {
                    method: 'GET',
                })
                    .then(response => response.json())
                    .then(data => {
                        data.area.forEach(coord => {
                            const marker = new maplibregl.Marker()
                                .setLngLat([coord.longitude, coord.latitude])
                                .addTo(map);

                            markers.push(marker);
                        });

                        centerOnMap();
                    })
                    .catch(error => {
                        showAlert('error', 'Kan parkgrenzen niet ophalen.');
                    });
            })
            .catch(error => {
                showAlert('error', 'Kan parkgrenzen niet ophalen.');
            });
    } catch (error) {
        showAlert('error', 'Kan parkgrenzen niet ophalen.');
    }
}

function deleteMap() {
    if (markers.length > 0) {
        const marker = markers.pop();
        marker.remove();

        document.activeElement.blur();
        const modal = bootstrap.Modal.getInstance(document.getElementById('exampleModal'));
        if (modal) {
            modal.hide();
        }
        onAreaChanged();
    } else {
        showAlert('error', 'Er zijn geen markers om te verwijderen.');
    }
}

function saveMap() {
    if (markers.length === 0) {
        showAlert('warning', 'Er zijn geen markers om op te slaan.');
        return;
    }

    const coordinates = markers.map(marker => marker.getLngLat());

    fetch(`${BACKEND_URL}${API_PATH}/${mapId}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(translateCoordinates(coordinates))
    })
        .catch(error => {
            console.log(error);
            showAlert('error', 'Kan markers niet opslaan.');
        });
}

function translateCoordinates(coordinates) {
    return coordinates.map(p => new Object({ "Longitude": p.lng, "Latitude": p.lat }));
}

let mapId = '';

document.addEventListener('DOMContentLoaded', () => {
    document.querySelector('#latitudeInput').value = '51.6885178';
    document.querySelector('#longitudeInput').value = '5.2866805';

    getMap();

    document.getElementById("confirmDelete").addEventListener("click", function () {
        deleteMap();
    });

    document.querySelector('#saveMap').addEventListener('click', () => {
        saveMap();
    });

    document.querySelector('#drawMap').addEventListener('click', () => {
        if (markers.length === 0) {
            showAlert('info', 'Klik op de kaart om een marker toe te voegen.');
        } else {
            showAlert('info', 'Er is al een marker op de kaart.');
        }
    });

    document.querySelector('#submitGo').addEventListener('click', () => {
        try {
            map.setZoom(15)
            map.setCenter([
                document.querySelector('#longitudeInput').value,
                document.querySelector('#latitudeInput').value
            ]);
        } catch (error) {
            showAlert('warning', 'Coördinaten zijn niet geldig.');
        }
    });

    document.querySelector('#searchPark').addEventListener('click', () => {
        centerOnMap();
    });
});

function showAlert(type, message) {
    alert(`${type.toUpperCase()}: ${message}`);
}
