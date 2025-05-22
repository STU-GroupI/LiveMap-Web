import * as turf from 'https://esm.sh/@turf/turf@7.1.0';
import * as mdi from 'https://cdn.jsdelivr.net/npm/@mdi/js/+esm';

MapboxDraw.constants.classes.CANVAS = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE = 'maplibregl-ctrl';
MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

let pois = [];

const map = MapFactory.createMap('map', [4.729, 52.045], 15);

map.doubleClickZoom.disable();

const markers = [];
let clickedLngLat = null;

map.on('click', (e) => {
    onMapClick(e);
});

function onMapClick(e) {
    const { lngLat } = e;
    clickedLngLat = lngLat;
    placeMarkerOnMap(true);
}

function centerOnMap() {
    if (markers.length === 0) {
        showAlert('error', 'Er zijn geen markers gezet.');
        return;
    }

    let center = [clickedLngLat.lng, clickedLngLat.lat];

    //This centering method is impressive, but the question is if we want to center the map between markers if there are more than 1 present.
    if (markers.length === 1) {
        const points = markers.map(marker => turf.point([marker.getLngLat().lng, marker.getLngLat().lat]));
        const featureCollection = turf.featureCollection(points);
        center = turf.center(featureCollection).geometry.coordinates;
    }

    map.flyTo({
        center: center,
        zoom: 17,
        speed: 1.2,
        curve: 1.42,
        easing: t => t
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
        markers[0].remove();
        markers.length = 0;
        pois.length = 0;
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
   if (map.getSource('pois')) {
        map.getSource('pois').setData(updatedGeoJson);
    }

    if (iconName) {
        // Use the Material Design Icons (mdi) library to get the SVG path
        const iconPath = mdi[iconName];
        if (iconPath) {
            markerElement.innerHTML = `
                <svg viewBox="0 0 24 24" width="40" height="40" fill="currentColor">
                    <path d="${iconPath}" />
                </svg>
            `;

            const marker = new maplibregl.Marker({ element: markerElement })
                .setLngLat([clickedLngLat.lng, clickedLngLat.lat])
                .addTo(map);

            markers.push(marker);

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
    const marker = new maplibregl.Marker()
        .setLngLat([clickedLngLat.lng, clickedLngLat.lat])
        .addTo(map);

    window.mapCenter = marker;

    markers.push(marker);
    if (shouldCenter) {
        centerOnMap();
    }
}


document.querySelector('[id$="Category"]').addEventListener('change', () => {
    if (clickedLngLat) {
        if (markers.length > 0) {
            const lastIndex = markers.length - 1;
            const lastMarker = markers.pop();
            lastMarker.remove();
            pois.splice(lastIndex, 1)
        }
        placeMarkerOnMap(false);
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
        clusterRadius: 60,
        clusterMaxZoom: 15
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

    // Cluster count labels
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

    map.on('zoom', () => {
        const z = map.getZoom();
        const hide = z < 16;
        if (markers.length > 1) {
            markers.forEach(marker => {
                const el = marker.getElement();
                el.style.display = hide ? 'none' : '';
            });
        }
    });
});
