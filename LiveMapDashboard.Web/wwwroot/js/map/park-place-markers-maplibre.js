import * as turf from 'https://esm.sh/@turf/turf@7.1.0';

MapboxDraw.constants.classes.CANVAS = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE = 'maplibregl-ctrl';
MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

const map = MapFactory.createMap('map', [4.729, 52.045], 15);

//Prevent the map from zooming in when double clicking
map.doubleClickZoom.disable();

const markers = [];
let clickedLngLat = null;

// Event listener voor single click
map.on('click', (e) => {
    onMapClick(e);
});

function onMapClick(e) {
    const { lngLat } = e;
    clickedLngLat = lngLat; // Store the clicked coordinates
    
    updateCoordinateFields(clickedLngLat.lat, clickedLngLat.lng);

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

function placeMarkerOnMap() {

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

// Add these helper functions at the top of your file after imports

function updateCoordinateFields(lat, lng) {
    // Try to update the hidden coordinate fields
    try {
        // The hidden fields format
        const latField = document.querySelector('input[name="Coordinate.Latitude"]');
        const lngField = document.querySelector('input[name="Coordinate.Longitude"]');

        if (latField && lngField) {
            latField.value = lat.toString().replace('.', ',');
            lngField.value = lng.toString().replace('.', ',');
            return true;
        }

        // The standard fields format
        const latField2 = document.getElementById('Coordinate_Latitude');
        const lngField2 = document.getElementById('Coordinate_Longitude');

        if (latField2 && lngField2) {
            latField2.value = lat.toString().replace('.', ',');
            lngField2.value = lng.toString().replace('.', ',');
            return true;
        }

        return false;
    } catch (error) {
        console.error('Error updating coordinate fields:', error);
        return false;
    }
}

function getCoordinateFromFields() {
    try {
        // Try hidden fields first
        let lat = null;
        let lng = null;

        const latField = document.querySelector('input[name="Coordinate.Latitude"]');
        const lngField = document.querySelector('input[name="Coordinate.Longitude"]');

        if (latField && lngField) {
            lat = parseFloat(latField.value.replace(',', '.'));
            lng = parseFloat(lngField.value.replace(',', '.'));
        } else {
            // Try standard fields
            const latField2 = document.getElementById('Coordinate_Latitude');
            const lngField2 = document.getElementById('Coordinate_Longitude');

            if (latField2 && lngField2) {
                lat = parseFloat(latField2.value.replace(',', '.'));
                lng = parseFloat(lngField2.value.replace(',', '.'));
            }
        }

        if (lat && lng && !isNaN(lat) && !isNaN(lng)) {
            return { lat, lng };
        }

        return null;
    } catch (error) {
        console.error('Error getting coordinates from fields:', error);
        return null;
    }
}

document.getElementById('applyLocationButton').addEventListener('click', () => {
    if (!clickedLngLat) {
        showAlert('warning', 'Klik eerst op de kaart om een locatie te selecteren.');
        return;
    }

    updateCoordinateFields(clickedLngLat.lat, clickedLngLat.lng);
});

map.on('load', () => {
    const coords = getCoordinateFromFields();

    if (coords && (coords.lng !== 0 || coords.lat !== 0)) {
        const clampedLong = Math.max(Math.min(coords.lng, 90), -90);
        const clampedLat = Math.max(Math.min(coords.lat, 90), -90);

        clickedLngLat = { lng: clampedLong, lat: clampedLat };
        placeMarkerOnMap();
    }
});
