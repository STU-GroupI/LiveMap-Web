import * as turf from 'https://esm.sh/@turf/turf@7.1.0';
import * as mdi from 'https://cdn.jsdelivr.net/npm/@mdi/js/+esm';

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
    placeMarkerOnMap(true); // Call the function to place the marker
}

function onMapDoubleClick(e) {
    const { lngLat } = e;
    clickedLngLat = lngLat; // Store the clicked coordinates
    document.getElementById('Coordinate_Latitude').value = clickedLngLat.lat.toString().replace('.', ',');
    document.getElementById('Coordinate_Longitude').value = clickedLngLat.lng.toString().replace('.', ',');
    showAlert('success', 'Coördinaten zijn toegepast.');
    placeMarkerOnMap(true); // Call the function to place the marker
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

function getSelectedCategoryIconName() {
    const categoryDropdown = document.getElementById('Category');
    const selectedOption = categoryDropdown.options[categoryDropdown.selectedIndex];
    const iconName = selectedOption.getAttribute('data-iconname');
    return iconName;
}

function placeMarkerOnMap(shouldCenter) {
    // If a marker already exists, remove it before adding a new one
    if (markers.length > 0) {
        markers[0].remove(); // Remove the existing marker
        markers.length = 0; // Clear the markers array
    }

    // Get the selected category's icon name
    const iconName = getSelectedCategoryIconName();

    // Create a custom marker element
    const markerElement = document.createElement('div');
    markerElement.className = 'custom-marker';

    if (iconName) {
        // Use the Material Design Icons (mdi) library to get the SVG path
        const iconPath = mdi[iconName];
        if (iconPath) {
            markerElement.innerHTML = `
                <svg viewBox="0 0 24 24" width="40" height="40" fill="currentColor">
                    <path d="${iconPath}" />
                </svg>
            `;


            // Add the custom marker to the map
            const marker = new maplibregl.Marker({ element: markerElement })
                .setLngLat([clickedLngLat.lng, clickedLngLat.lat])
                .addTo(map);

            // Store the marker in the markers array
            markers.push(marker);

            // Center the map on the new marker
            if (shouldCenter) {
                centerOnMap();
            }
            return;
        } else {
            PlaceDefaultMarker(shouldCenter)
            return;
        }
    } else {
        PlaceDefaultMarker(shouldCenter)
        return;
    }
}

function PlaceDefaultMarker(shouldCenter) {
    // Add the marker at the clicked position
    const marker = new maplibregl.Marker()
        .setLngLat([clickedLngLat.lng, clickedLngLat.lat])
        .addTo(map);

    window.mapCenter = marker;
    
    // Store the marker in the markers array
    markers.push(marker);
    if (shouldCenter) {
        centerOnMap(); // Center the map on the new marker
    }
}


document.getElementById('Category').addEventListener('change', () => {
        placeMarkerOnMap(false); // Update the marker's appearance
});

document.getElementById('applyLocationButton').addEventListener('click', () => {
    if (!clickedLngLat) {
        showAlert('warning', 'Klik eerst op de kaart om een locatie te selecteren.');
        return;
    }

    document.getElementById('Coordinate_Latitude').value = clickedLngLat.lat.toString().replace('.', ',');
    document.getElementById('Coordinate_Longitude').value = clickedLngLat.lng.toString().replace('.', ',');
});

map.on('load', () => {
    const long = parseFloat(document.getElementById('Coordinate_Longitude').value.replace(',', '.'))
    const lat = parseFloat(document.getElementById('Coordinate_Latitude').value.replace(',', '.'))

    if ((long && lat) && (long !== 0 && lat !== 0)) {
        const clampedLong = Math.max(Math.min(long, 90), -90);
        const clampedLat = Math.max(Math.min(lat, 90), -90);

        clickedLngLat = {lng: clampedLong, lat: clampedLat};
        placeMarkerOnMap(true);
    }
});
