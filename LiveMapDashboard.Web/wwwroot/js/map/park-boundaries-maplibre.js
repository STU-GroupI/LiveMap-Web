import * as turf from 'https://esm.sh/@turf/turf@7.1.0';

MapboxDraw.constants.classes.CANVAS  = 'maplibregl-canvas';
MapboxDraw.constants.classes.CONTROL_BASE  = 'maplibregl-ctrl';
MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';

const map = MapFactory.createMap('map', [4.729, 52.045], 15);

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
    const features = draw.getAll().features;
    if (features.length === 0) {
        showAlert('error', 'There are no map boundaries yet.');
        return;
    }

    const coordinates = features[0].geometry.coordinates[0];
    const center = getCenterOfCoordinates(coordinates);
    
    map.setCenter(center);
    window.mapCenter = new maplibregl.Marker().setLngLat(center);
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

function deleteBoundaries() {
    const data = draw.getAll();
    if (data.features && data.features.length > 0) {
        draw.delete(data.features[data.features.length - 1].id);

        document.activeElement.blur();
        onAreaChanged();
    } else {
        showAlert('error', 'There are no map boundaries to delete.');
    }
}

function translateArea(apiArea) {
    return apiArea.map(p => [
        p.Latitude,
        p.Longitude
    ]);
}

function translateCoordinates(coordinates) {
    // return coordinates.map(p => new Object({ "Longitude": p[0], "Latitude": p[1] }));
    // Javascript maps expect them then other way around. 
    // But real coordinates area formally longitude then latitude
    return coordinates.map(p => new Object({ "Longitude": p[1], "Latitude": p[0] }));
}

document.addEventListener('DOMContentLoaded', () => {
    {
        try {
            const area = document.querySelector('#Area').value; 
            if (area !== '') {
                draw.add({
                    type: 'Feature',
                    properties: [],
                    geometry: {
                        type: 'Polygon',
                        coordinates: [translateArea(JSON.parse(area)),],
                    }
                });
                centerOnMap();
                onAreaChanged();
            }
        } catch (error) {
            showAlert('error', 'Could not load map boundaries');
        }
    }

    document.querySelector('#map_form').addEventListener('submit', (e) => {
        try {
            const features = draw.getAll().features;
            const coordinates = features[0].geometry.coordinates[0];
            const jsonArea = JSON.stringify(translateCoordinates(coordinates));
            document.querySelector('#Area').value = jsonArea;
        } catch (error) {
            showAlert('error', 'Could not submit map boundaries.');
        }
    });

    document.querySelector('#drawMap').addEventListener('click', () => {
        if (hasDrawing() === false) {
            draw.changeMode('draw_polygon');  // Enable drawing polygon mode
        } else {
            showAlert('info', 'There is a map boundary already present.');
        }
    });

    document.querySelector('#deleteBoundaries').addEventListener('click', () => {
        deleteBoundaries();
    });
});