const API_PATH = "/api/map"

const MapFactory = (() => {
    const MAPBOX_RASTER_URL = "https://basemaps.cartocdn.com/gl/voyager-gl-style/style.json";

    const geocoderApi = {
        forwardGeocode: async (config) => {
            const request = `https://nominatim.openstreetmap.org/search?q=${config.query}&format=geojson&polygon_geojson=1&addressdetails=1`;

            try {
                const response = await fetch(request);
                const geojson = await response.json();

                return {
                    features: geojson.features.map(feature => {
                        const center = [
                            feature.bbox[0] + (feature.bbox[2] - feature.bbox[0]) / 2,
                            feature.bbox[1] + (feature.bbox[3] - feature.bbox[1]) / 2
                        ];

                        return {
                            type: 'Feature',
                            geometry: {
                                type: 'Point',
                                coordinates: center
                            },
                            place_name: feature.properties.display_name,
                            text: feature.properties.display_name,
                            place_type: ['place'],
                            center,
                            properties: feature.properties
                        };
                    })
                };
            } catch (err) {
                console.error("Geocoder fetch failed:", err);
                return { features: [] };
            }
        }
    };
    
    function addBasicControls(map) {
        map.addControl(new maplibregl.NavigationControl({
            visualizePitch: true,
            visualizeRoll: true,
            showZoom: true,
            showCompass: true
        }));
        map.addControl(new maplibregl.FullscreenControl());
    }

    function addGeocoder(map) {
        const geocoder = new MaplibreGeocoder(geocoderApi, { maplibregl, marker: false });

        geocoder.on('result', (event) => {
            const searchInput = document.querySelector('.maplibregl-ctrl-geocoder input');
            if (searchInput) {
                searchInput.value = '';
            }
        });

        map.addControl(geocoder);
    }

    function createMap(containerId, center = [4.729, 52.045], zoom = 15) {
        const map = new maplibregl.Map({
            container: containerId,
            style: MAPBOX_RASTER_URL,
            center,
            zoom,
            dragRotate: false,
            pitchWithRotate: false,
        });

        map.on("load", () => {
            addGeocoder(map);
            addBasicControls(map);
        });

        return map;
    }

    return {
        createMap,
        API_PATH
    };
})();

window.MapFactory = MapFactory;
