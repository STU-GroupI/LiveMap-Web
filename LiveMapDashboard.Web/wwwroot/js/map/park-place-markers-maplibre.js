import * as turf from 'https://esm.sh/@turf/turf@7.1.0';
import * as mdi from 'https://cdn.jsdelivr.net/npm/@mdi/js/+esm';

MapboxDraw.constants.classes.CANVAS = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE = 'maplibregl-ctrl';
MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

let pois = [];

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
    const categoryDropdown = document.querySelector('[id$="Category"]');
    const selectedOption = categoryDropdown.options[categoryDropdown.selectedIndex];
    const iconName = selectedOption.getAttribute('data-iconname');
    return iconName;
}

function placeMarkerOnMap(shouldCenter) {
    // If a marker already exists, remove it before adding a new one
    //REMOVE THE IF-STATEMENT BELOW TO ESSENTIALLY ACTIVATE CLUSTERING AND MULTIPLE POIS(Although the last added POI will only be saved :P)
    if (markers.length > 0) {
        markers[0].remove(); // Remove the existing marker
        markers.length = 0; // Clear the markers array
        pois.length = 0; // Clears the POI array
    }

    // Get the selected category's icon name
    let iconName = getSelectedCategoryIconName();

    if (!iconName || iconName == "-") {
        iconName = null;
    }

    // Create a custom marker element
    const markerElement = document.createElement('div');
    markerElement.className = 'custom-marker';

    const newPoi = {
        guid: crypto.randomUUID(),
        coordinate: { longitude: clickedLngLat.lng, latitude: clickedLngLat.lat },
        category: {
            iconName,
            category: iconName ? document.querySelector('[id$="Category"]').value : 'default',
            categoryName: iconName
                ? document.querySelector('[id$="Category"] option:checked').text
                : 'Uncategorized'
        }
    };
    pois.push(newPoi);

    // Re‑set the GeoJSON
    const updatedGeoJson = {
        type: 'FeatureCollection',
        features: pois.map(poi => ({
            type: 'Feature',
            geometry: {
                type: 'Point',
                coordinates: [poi.coordinate.longitude, poi.coordinate.latitude]
            },
            properties: {
                guid: poi.guid,
                iconName: poi.category.iconName,
                category: poi.category.category,
                categoryName: poi.category.categoryName
            }
        }))
    };
    map.getSource('pois').setData(updatedGeoJson);

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


document.querySelector('[id$="Category"]').addEventListener('change', () => {
    if (clickedLngLat) {
        placeMarkerOnMap(false); // Update the marker's appearance
    }
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
    // If there is an existing marker, place it
    const long = parseFloat(document.getElementById('Coordinate_Longitude').value.replace(',', '.'))
    const lat = parseFloat(document.getElementById('Coordinate_Latitude').value.replace(',', '.'))

    if ((long && lat) && (long !== 0 && lat !== 0)) {
        const clampedLong = Math.max(Math.min(long, 90), -90);
        const clampedLat = Math.max(Math.min(lat, 90), -90);

        clickedLngLat = {lng: clampedLong, lat: clampedLat};
        placeMarkerOnMap(true);
    }

    // GeoJSON to be able to show clusters
    const geoJsonPois = {
        type: 'FeatureCollection',
        features: pois.map(poi => ({
            type: 'Feature',
            geometry: {
                type: 'Point',
                coordinates: [poi.coordinate.longitude, poi.coordinate.latitude]
            },
            properties: {
                guid: poi.guid,
                iconName: poi.category.iconName,
                category: poi.category.category,
                categoryName: poi.category.categoryName
            }
        }))
    };

    // Source for clusters
    map.addSource('pois', {
        type: 'geojson',
        data: geoJsonPois,
        cluster: true,
        clusterRadius: 80,
        clusterMaxZoom: 13
    });

    // Circles for clusters
    map.addLayer({
        id: 'cluster-circles',
        type: 'circle',
        source: 'pois',
        filter: ['has', 'point_count'],
        paint: {
            'circle-color': '#ffffff',
            'circle-stroke-color': '#cccccc',
            'circle-stroke-width': 2,
            'circle-radius': 20
        }
    });

    // Add Cluster count labels to show contents of cluster
    map.addLayer({
        id: 'cluster-count',
        type: 'symbol',
        source: 'pois',
        filter: ['has', 'point_count'],
        layout: {
            'text-field': '{point_count}',
            'text-size': 14,
            'text-anchor': 'center'
        },
        paint: {
            'text-color': '#0017EE'
        }
    });

    // If-statement to present markers based on zoom level
    map.on('zoom', () => {
        const z = map.getZoom();
        //If zoom level is below 14, which is pretty zoomed out, remove the markers from the map for clustering :)
        const hide = z < 14;
        //Obviously, we can only do this if there are multiple markers, if there is only one present, it's no use to hide it
        if (markers.length > 1) {
            markers.forEach(marker => {
                const el = marker.getElement();
                // either use display or visibility
                el.style.display = hide ? 'none' : '';
            });
        }
    });
});
