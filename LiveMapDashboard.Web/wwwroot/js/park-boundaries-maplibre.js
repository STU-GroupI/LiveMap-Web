import * as turf from 'https://esm.sh/@turf/turf@7.1.0';

const BASE_PORT = 5006
const BASE_URL = `http://localhost:${BASE_PORT}`;
const API_PATH = "/api/map"

const MAPBOX_RASTER_URL = "https://basemaps.cartocdn.com/gl/voyager-gl-style/style.json";

MapboxDraw.constants.classes.CANVAS  = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE  = 'maplibregl-ctrl';
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

const draw = new MapboxDraw({
    displayControlsDefault: false,
    controls: {
        polygon: false,
        trash: false
    },
    styles: [
        {
            'id': 'gl-draw-polygon-and-line-vertex-active',
            'type': 'circle',
            'filter': ['==', '$type', 'Point'],
            'paint': {
                'circle-radius': 10,
                'circle-color': '#fff',
                'circle-stroke-width': 2,
                'circle-stroke-color': '#0f0'
            }
        },
        {
            'id': 'gl-draw-polygon-fill',
            'type': 'fill',
            'filter': ['==', '$type', 'Polygon'],
            'paint': {
                'fill-color': '#0f0',
                'fill-opacity': 0.1
            }
        },
        {
            'id': 'gl-draw-polygon-stroke',
            'type': 'line',
            'filter': ['==', '$type', 'Polygon'],
            'paint': {
                'line-color': '#0f0',
                'line-width': 4,
                'line-dasharray': ['literal', [2, 2]]
            }
        }
    ]
});
map.addControl(draw);

map.on('draw.create', onCreateArea);
map.on('draw.delete', onDeleteArea);
map.on('draw.update', updateArea);

// <Map API methods>
function updateArea(e) {
    const data = draw.getAll();

    if (data.features.length > 0) {
        const area = turf.area(data);

        // restrict to area to 2 decimal points
        const roundedArea = Math.round(area * 100) / 100;
    }

    onAreaChanged();
}

function onDeleteArea() {
    onAreaChanged();
}

function onCreateArea(e) {
    const data = draw.getAll();

    if (data.features.length > 1) {
        draw.delete(data.features[data.features.length - 1].id);
    }

    onAreaChanged();
}

function onAreaChanged() {
    const startDrawingBtn = document.querySelector('#drawMap');
}
// </Map API methods>

function hasDrawing() {
    return draw.getAll().features.length > 0;
}

function centerOnMap() {
    map.setZoom(15);
    console.log(draw.getAll().features);
    const features = draw.getAll().features;
    if (features.length === 0) {
        showAlert('error', 'Er zijn geen parkgrenzen gezet.');
        return;
    }

    const coordinates = features[0].geometry.coordinates[0];
    map.setCenter(getCenterOfCoordinates(coordinates));
}

function getCenterOfCoordinates(coordinates) {
    let totalLatitude = 0;
    let totalLongitude = 0;

    // Loop through each coordinate and sum up latitudes and longitudes
    coordinates.forEach(coord => {
        totalLatitude += coord[1];
        totalLongitude += coord[0];
    });

    // Calculate the average latitude and longitude
    const centerLatitude = totalLatitude / coordinates.length;
    const centerLongitude = totalLongitude / coordinates.length;

    return [centerLongitude, centerLatitude];  // Return in [longitude, latitude] format
}

// CRUD
function getMap() {
    try {
        const allMaps = `${BASE_URL}${API_PATH}`;
        fetch(`${allMaps}`, {
            method: 'GET',
        })
            .then(response => response.json())
            .then(data => {
                mapId = data[0].id;
                fetch(`${BASE_URL}${API_PATH}/${data[0].id}`, {
                    method: 'GET',
                })
                    .then(response => response.json())
                    .then(data => {
                        const newFeature = draw.add({
                            type: 'Feature',
                            properties: [],
                            geometry: {
                                type: 'Polygon',
                                coordinates: [ translateArea(data.area), ],
                            }
                        });
                        centerOnMap();
                        onAreaChanged();
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
    const data = draw.getAll();
    if (data.features && data.features.length > 0) {
        draw.delete(data.features[data.features.length - 1].id);

        document.activeElement.blur();
        const modal = bootstrap.Modal.getInstance(document.getElementById('exampleModal'));
        if (modal) {
            modal.hide();
        }
        onAreaChanged();
    } else {
        showAlert('error', 'Er zijn geen parkgrenzen om te verwijderen.');
    }
}

function saveMap() {
    const features = draw.getAll().features;
    if (features.length === 0) {
        showAlert('warning', 'Er zijn geen parkgrenzen om op te slaan.');
        return;
    }

    // Save Map
    const coordinates = features[0].geometry.coordinates[0];

    fetch(`${BASE_URL}${API_PATH}/${mapId}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(translateCoordinates(coordinates))
    })
        .catch(error => {
            console.log(error);
            showAlert('error', 'Kan park grenzen niet opslaan.');
        });
}

function translateArea(apiArea) {
    return apiArea.map(p => [
        p.latitude,
        p.longitude
    ]);
}

function translateCoordinates(coordinates) {
    // return coordinates.map(p => new Object({ "Longitude": p[0], "Latitude": p[1] }));
    // Javascript maps expect them then other way around. 
    // But real coordinates area formally longitude then latitude
    return coordinates.map(p => new Object({ "Longitude": p[1], "Latitude": p[0] }));
}

let mapId = '';

document.addEventListener('DOMContentLoaded', () => {
    {
        document.querySelector('#latitudeInput').value = '51.6885178';
        document.querySelector('#longitudeInput').value = '5.2866805';

        getMap();
    }

    document.getElementById("confirmDelete").addEventListener("click", function () {
        deleteMap();
    });

    document.querySelector('#saveMap').addEventListener('click', () => {
        saveMap();
    });

    document.querySelector('#drawMap').addEventListener('click', () => {
        if (hasDrawing() === false) {
            draw.changeMode('draw_polygon');  // Enable drawing polygon mode
        } else {
            showAlert('info', 'Er is al een park grens op de kaart.');
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
            showAlert('warning', 'Coordinaten zijn niet geldig.');
        }
    });

    document.querySelector('#searchPark').addEventListener('click', () => {
        centerOnMap();
    });
});