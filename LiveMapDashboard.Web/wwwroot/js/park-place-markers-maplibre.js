import * as turf from 'https://esm.sh/@turf/turf@7.1.0';

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
const distance = 1;
const calculationformula = 1000; // 1 km = 1000 m
const minDistance = distance / calculationformula; // Minimum distance between markers in meters

map.on('click', onMapClick);

function onMapClick(e) {
    const { lngLat } = e;

    // If a marker already exists, remove it before adding a new one
    if (markers.length > 0) {
        markers[0].remove(); // Remove the existing marker
        markers.length = 0; // Clear the markers array
    }

    // Proceed with placing the marker only if it is at least 2 meters from the existing markers
    if (isTooCloseToOtherMarkers(lngLat)) {
        showAlert('warning', 'Er moet minstens 1 meter afstand zijn tussen markers.');
        return;
    }

    // Add the marker at the clicked position
    const marker = new maplibregl.Marker()
        .setLngLat([lngLat.lng, lngLat.lat])
        .addTo(map);
    
    document.getElementById('Coordinate_Latitude').value = lngLat.lat.toString().replace('.', ',');
    document.getElementById('Coordinate_Longitude').value = lngLat.lng.toString().replace('.', ',');


    // Store the marker in the markers array
    markers.push(marker);
    onAreaChanged();
}

function onAreaChanged() {
    centerOnMap();
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
