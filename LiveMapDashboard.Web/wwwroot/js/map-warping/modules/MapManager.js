import {resetState, state} from './canvasState.js';
import { mapPoints, markerElements, clearMarkers } from './mapState.js';
import { computeAffine, applyAffine } from '../affine.js';
import {MarkerList} from "./MarkerList.js";

/**
 * Manages the map functionalities including adding markers, generating overlays, and resets.
 */
export class MapManager {
    constructor(containerId, image) {
        this.map = MapFactory.createMap(containerId, [4.729, 52.045], 15);
        this.image = image;
        this.map.on('load', () => this.initializeMap());
        
        document.getElementById('reset').addEventListener('click', () => this.reset());
    }
    
    initializeMap() {
        this.addControls();
        this.addClickHandler();
        this.addGenerateHandler();
        this.addOpacityHandler();

        this.coordinateIdentifiers = {
            topLeftLat: document.getElementById('TopLeft-latitude'),
            topLeftLng: document.getElementById('TopLeft-longitude'),

            bottomLeftLat: document.getElementById('BottomLeft-latitude'),
            bottomLeftLng: document.getElementById('BottomLeft-longitude'),

            topRightLat: document.getElementById('TopRight-latitude'),
            topRightLng: document.getElementById('TopRight-longitude'),

            bottomRightLat: document.getElementById('BottomRight-latitude'),
            bottomRightLng: document.getElementById('BottomRight-longitude'),
        }
        
        this.initialValues = {
            topLeftLat: this.coordinateIdentifiers.topLeftLat.value,
            topLeftLng: this.coordinateIdentifiers.topLeftLng.value,
            
            bottomLeftLat: this.coordinateIdentifiers.bottomLeftLat.value,
            bottomLeftLng: this.coordinateIdentifiers.bottomLeftLng.value,
            
            topRightLat: this.coordinateIdentifiers.topRightLat.value,
            topRightLng: this.coordinateIdentifiers.topRightLng.value,
            
            bottomRightLat: this.coordinateIdentifiers.bottomRightLat.value,
            bottomRightLng: this.coordinateIdentifiers.bottomRightLng.value,
        };
        
        this.initializeFormMapWithOverlayImage();
    }
    
    initializeFormMapWithOverlayImage() {
        const corners = [
            [parseFloat(this.coordinateIdentifiers.topLeftLng.value), parseFloat(this.coordinateIdentifiers.topLeftLat.value)],
            [parseFloat(this.coordinateIdentifiers.bottomLeftLng.value), parseFloat(this.coordinateIdentifiers.bottomLeftLat.value)],
            [parseFloat(this.coordinateIdentifiers.topRightLng.value), parseFloat(this.coordinateIdentifiers.topRightLat.value)],
            [parseFloat(this.coordinateIdentifiers.bottomRightLng.value), parseFloat(this.coordinateIdentifiers.bottomRightLat.value)],
        ];
        
        if (corners.some(corner => (isNaN(corner[0]) || corner[0] === 0) || (isNaN(corner[0]) || corner[0] === 0))) {
            return;
        }

        this.setMapOverlay(corners, window.MapRegistry.formMap);
    }

    addControls() {
        const button = document.createElement('button');
        button.innerHTML =
            '<div class="tools-box">' +
            '<button>' +
            '<span class="flex justify-center align-center" aria-hidden="true" title="Description">' +
            '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-undo2-icon lucide-undo-2 shrink-0 size-5"><path d="M9 14 4 9l5-5"/><path d="M4 9h10.5a5.5 5.5 0 0 1 5.5 5.5a5.5 5.5 0 0 1-5.5 5.5H11"/></svg>' +
            '</span>' +
            '</button>' +
            '</div>';
        button.onclick = () => this.undoLastMarker();

        const controlContainer = document.createElement('div');
        controlContainer.className = 'maplibregl-ctrl maplibregl-ctrl-group';
        controlContainer.appendChild(button);

        this.map.addControl({
            onAdd: () => controlContainer,
            onRemove: () => controlContainer.remove()
        }, 'top-right');
    }

    undoLastMarker() {
        if (markerElements.length > 0) {
            markerElements.pop().remove();
            mapPoints.pop();
        }
    }

    addClickHandler() {
        this.map.on('click', (e) => {
            const lngLat = [e.lngLat.lng, e.lngLat.lat];
            const el = document.createElement('div');
            el.className = 'inline-flex items-center rounded-md bg-blue-50 px-2 py-1 text-xs font-medium text-blue-800 ring-1 ring-blue-800 ring-inset';
            el.textContent = mapPoints.length + 1;

            const marker = new maplibregl.Marker({ element: el })
                .setLngLat(lngLat)
                .addTo(this.map);

            el.addEventListener('click', (event) => {
                event.stopPropagation();
                const index = markerElements.findIndex((m) => m.getElement() === el);
                
                if (index !== -1) {
                    markerElements.splice(index, 1);
                    mapPoints.splice(index, 1);
                    marker.remove();

                    markerElements.forEach((m, i) => {
                        m.getElement().textContent = i + 1;
                    });

                    MarkerList.update('map-marker-list', mapPoints);
                }
            });

            mapPoints.push(lngLat);
            markerElements.push(marker);
        });
    }

    /**
     * Adds a click handler for the "Generate" button to compute the affine transformation and apply them if successful.
     */
    addGenerateHandler() {
        document.getElementById('generate').addEventListener('click', () => {
            try {
                if (state.imagePoints.length < 6 || mapPoints.length < 6) {
                    showAlert('error', 'You need at least 6 matching points on both image and map.');
                    return;
                }
                
                if (state.imagePoints.length !== mapPoints.length) {
                    showAlert('error', 'The number of points on the image and map must match.');
                    return;
                }

                const t = computeAffine(state.imagePoints, mapPoints);
                const corners = [
                    applyAffine(t, 0, 0),
                    applyAffine(t, this.image.width, 0),
                    applyAffine(t, this.image.width, this.image.height),
                    applyAffine(t, 0, this.image.height)
                ];

                if (this.map.getSource('overlay')) {
                    this.map.removeLayer('overlay-layer');
                    this.map.removeSource('overlay');
                }

                this.map.addSource('overlay', {
                    type: 'image',
                    url: this.image.src,
                    coordinates: corners
                });

                this.map.addLayer({
                    id: 'overlay-layer',
                    type: 'raster',
                    source: 'overlay',
                    paint: { 'raster-opacity': parseFloat(document.getElementById('opacity').value) }
                });

                this.map.fitBounds([corners[0], corners[2]], { padding: 20 });

                this.coordinateIdentifiers.topLeftLat.value = corners[0][1];
                this.coordinateIdentifiers.topLeftLng.value = corners[0][0];

                this.coordinateIdentifiers.bottomLeftLat.value = corners[1][1];
                this.coordinateIdentifiers.bottomLeftLng.value = corners[1][0];

                this.coordinateIdentifiers.topRightLat.value = corners[2][1];
                this.coordinateIdentifiers.topRightLng.value = corners[2][0];

                this.coordinateIdentifiers.bottomRightLat.value = corners[3][1];
                this.coordinateIdentifiers.bottomRightLng.value = corners[3][0];
                
                this.setMapOverlay(corners, window.MapRegistry.formMap);
                showAlert('success', 'Overlay generated successfully! Do not forget to save your changes.')
            } catch (error) {
                console.error('Error generating overlay:', error);
                showAlert('error', 'An error occurred while generating the overlay. Please check your points and try again.');
            }
        });
    }

    /**
     * Sets the map overlay with the provided corners and map reference. Note that the image URL is fetched from the input field with ID "ImageUrl".
     * @param corners
     * @param mapReference
     */
    setMapOverlay(corners, mapReference) {
        const imageUrl = document.getElementById("ImageUrl").value;
        if (!imageUrl) {
            showAlert('error', 'Please provide a valid image URL.')
            return;
        }
        
        if (mapReference.getLayer('image-overlay-layer')) {
            mapReference.removeLayer('image-overlay-layer');
        }
        if (mapReference.getSource('image-overlay')) {
            mapReference.removeSource('image-overlay');
        }

        mapReference.addSource('image-overlay', {
            type: 'image',
            url: imageUrl,
            coordinates: corners
        });

        mapReference.addLayer({
            id: 'image-overlay-layer',
            type: 'raster',
            source: 'image-overlay',
            paint: { 'raster-opacity': 0.8 }
        }, 'housenumber');

        mapReference.fitBounds([corners[0], corners[2]], { padding: 20 });
    }

    addOpacityHandler() {
        document.getElementById('opacity').addEventListener('input', (e) => {
            if (this.map.getLayer('overlay-layer')) {
                this.map.setPaintProperty('overlay-layer', 'raster-opacity', parseFloat(e.target.value));
            }
        });
    }

    reset() {
        try {
            clearMarkers();
            resetState();

            if (this.map.getSource('overlay')) {
                this.map.removeLayer('overlay-layer');
                this.map.removeSource('overlay');
            }

            this.resetCoordinates();
            this.initializeFormMapWithOverlayImage();

            showAlert('info', 'Map and markers reset successfully!');
        } catch (error) {
            console.error('Error resetting map:', error);
            showAlert('error', 'An error occurred while resetting the map. Please try again.');
        }
    }

    resetCoordinates() {
        this.coordinateIdentifiers.topLeftLat.value = this.initialValues.topLeftLat;
        this.coordinateIdentifiers.topLeftLng.value = this.initialValues.topLeftLng;

        this.coordinateIdentifiers.bottomLeftLat.value = this.initialValues.bottomLeftLat;
        this.coordinateIdentifiers.bottomLeftLng.value = this.initialValues.bottomLeftLng;

        this.coordinateIdentifiers.topRightLat.value = this.initialValues.topRightLat;
        this.coordinateIdentifiers.topRightLng.value = this.initialValues.topRightLng;

        this.coordinateIdentifiers.bottomRightLat.value = this.initialValues.bottomRightLat;
        this.coordinateIdentifiers.bottomRightLng.value = this.initialValues.bottomRightLng;
    }
}
