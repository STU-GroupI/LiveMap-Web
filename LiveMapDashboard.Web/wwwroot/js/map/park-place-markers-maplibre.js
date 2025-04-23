import * as turf from 'https://esm.sh/@turf/turf@7.1.0';

MapboxDraw.constants.classes.CANVAS = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE = 'maplibregl-ctrl';
MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

const map = MapFactory.createMap('map', [4.729, 52.045], 15);

//Prevent the map from zooming in when double clicking
map.doubleClickZoom.disable();

const markers = []; // To keep track of added markers
let clickedLngLat = null; // To store the clicked coordinates

let clickTimeout = null; // Timer to differentiate between single and double clicks

// Event listener voor single click
map.on('click', (e) => {
    // Start a timer to delay the single click action
    if (clickTimeout) clearTimeout(clickTimeout);
    clickTimeout = setTimeout(() => {
        onMapClick(e);
        clickTimeout = null;
    }, 250); // 250 ms is a common threshold, we can change this if needed
});

// Event listener voor double click
map.on('dblclick', (e) => {
    // Cancel the single click action if it was triggered
    if (clickTimeout) {
        clearTimeout(clickTimeout);
        clickTimeout = null;
    }
    onMapDoubleClick(e);
});

function onMapClick(e) {
    const { lngLat } = e;
    clickedLngLat = lngLat; // Store the clicked coordinates
    placeMarkerOnMap(); // Call the function to place the marker
}

function onMapDoubleClick(e) {
    const { lngLat } = e;
    clickedLngLat = lngLat; // Store the clicked coordinates
    document.getElementById('Coordinate_Latitude').value = clickedLngLat.lat.toString().replace('.', ',');
    document.getElementById('Coordinate_Longitude').value = clickedLngLat.lng.toString().replace('.', ',');
    showAlert('success', 'Coördinaten zijn toegepast.');
    placeMarkerOnMap(); // Call the function to place the marker
}

function centerOnMap() {
    if (markers.length === 0) {
        showAlert('error', 'Er zijn geen markers gezet.');
        return;
    }

    const points = markers.map(marker => turf.point([marker.getLngLat().lng, marker.getLngLat().lat]));
    const featureCollection = turf.featureCollection(points);
    const center = turf.center(featureCollection);

    map.flyTo({
        center: center.geometry.coordinates,
        zoom: 17,
        speed: 1.2,     // Adjust speed (default: 1.2)
        curve: 1.42,    // Adjust curvature (default: 1.42)
        easing: t => t  // Optionally adjust the easing function
    });
}

function showAlert(type, message) {
    alert(`${type.toUpperCase()}: ${message}`);
}

function placeMarkerOnMap(){

    // If a marker already exists, remove it before adding a new one
    if (markers.length > 0) {
        markers[0].remove(); // Remove the existing marker
        markers.length = 0; // Clear the markers array
    }

    // Add the marker at the clicked position
    const marker = new maplibregl.Marker()
        .setLngLat([clickedLngLat.lng, clickedLngLat.lat])
        .addTo(map);

    window.mapCenter = marker;
    
    // Store the marker in the markers array
    markers.push(marker);
    centerOnMap(); // Center the map on the new marker
}

document.getElementById('applyLocationButton').addEventListener('click', () => {
    if (!clickedLngLat) {
        showAlert('warning', 'Klik eerst op de kaart om een locatie te selecteren.');
        return;
    }

    document.getElementById('Coordinate_Latitude').value = clickedLngLat.lat.toString().replace('.', ',');
    document.getElementById('Coordinate_Longitude').value = clickedLngLat.lng.toString().replace('.', ',');
    showAlert('success', 'Coördinaten zijn toegepast.');
});

map.on('load', () => {
    const long = parseFloat(document.getElementById('Coordinate_Longitude').value.replace(',', '.'))
    const lat = parseFloat(document.getElementById('Coordinate_Latitude').value.replace(',', '.'))

    if ((long && lat) && (long !== 0 && lat !== 0)) {
        const clampedLong = Math.max(Math.min(long, 90), -90);
        const clampedLat = Math.max(Math.min(lat, 90), -90);

        clickedLngLat = {lng: clampedLong, lat: clampedLat};
        placeMarkerOnMap();
    }
});
