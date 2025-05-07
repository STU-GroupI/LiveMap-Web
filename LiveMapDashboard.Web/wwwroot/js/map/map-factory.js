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
        class RecenterButton {
            onAdd(map) {
                this.map = map;
                
                const button = document.createElement('button');
                button.className = 'maplibregl-ctrl-icon';
                button.type = 'button';
                button.title = 'Recenter map';

                button.innerHTML = `
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"
                        width="20" height="20" fill="none" stroke="#333333"
                        stroke-width="3" stroke-linecap="round" stroke-linejoin="round"
                        style="display: block; margin: auto; vertical-align: middle;">
                        <line x1="2" x2="5" y1="12" y2="12"/>
                        <line x1="19" x2="22" y1="12" y2="12"/>
                        <line x1="12" x2="12" y1="2" y2="5"/>
                        <line x1="12" x2="12" y1="19" y2="22"/>
                        <circle cx="12" cy="12" r="7"/>
                        <circle cx="12" cy="12" r="3"/>
                    </svg>
                `;

                button.addEventListener('click', () => {
                    if (window.mapCenter) {
                        const coordinates = window.mapCenter.getLngLat();
                        map.flyTo({ center: [coordinates.lng, coordinates.lat], zoom: 15 });
                    } else {
                        button.classList.add('failShake');
                        button.addEventListener('animationend', () => {
                            button.classList.remove('failShake');
                        }, { once: true });
                    }
                });

                const container = document.createElement('div');
                container.className = 'maplibregl-ctrl maplibregl-ctrl-group';
                container.appendChild(button);

                return container;
            }

            onRemove() {
                this.map = undefined;
            }
        }
        map.addControl(new RecenterButton(), 'top-right');

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
