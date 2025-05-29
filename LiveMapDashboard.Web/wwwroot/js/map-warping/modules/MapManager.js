// src/modules/MapManager.js
import {resetState, state} from './canvasState.js';
import { mapPoints, markerElements, clearMarkers } from './mapState.js';
import { computeAffine, applyAffine } from '../affine.js';
import {MarkerList} from "./MarkerList.js";

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

        const topLeftLat = document.getElementById('TopLeft-latitude');
        const topLeftLng = document.getElementById('TopLeft-longitude');

        const bottomLeftLat = document.getElementById('BottomLeft-latitude');
        const bottomLeftLng = document.getElementById('BottomLeft-longitude');

        const topRightLat = document.getElementById('TopRight-latitude');
        const topRightLng = document.getElementById('TopRight-longitude');

        const bottomRightLat = document.getElementById('BottomRight-latitude');
        const bottomRightLng = document.getElementById('BottomRight-longitude');

        
        const corners = [
            [parseFloat(topLeftLng.value), parseFloat(topLeftLat.value)],
            [parseFloat(bottomLeftLng.value), parseFloat(bottomLeftLat.value)],
            [parseFloat(topRightLng.value), parseFloat(topRightLat.value)],
            [parseFloat(bottomRightLng.value), parseFloat(bottomRightLat.value)],
        ];

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

            const marker = new maplibregl.Marker({ element: el }).setLngLat(lngLat).addTo(this.map);

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

    addGenerateHandler() {
        document.getElementById('generate').addEventListener('click', () => {
            if (state.imagePoints.length < 6 || mapPoints.length < 6 || state.imagePoints.length !== mapPoints.length) {
                alert('You need at least 6 matching points on both image and map.');
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
            
            const topLeftLat = document.getElementById('TopLeft-latitude');
            const topLeftLng = document.getElementById('TopLeft-longitude');

            const bottomLeftLat = document.getElementById('BottomLeft-latitude');
            const bottomLeftLng = document.getElementById('BottomLeft-longitude');

            const topRightLat = document.getElementById('TopRight-latitude');
            const topRightLng = document.getElementById('TopRight-longitude');

            const bottomRightLat = document.getElementById('BottomRight-latitude');
            const bottomRightLng = document.getElementById('BottomRight-longitude');

            topLeftLat.value = corners[0][1];
            topLeftLng.value = corners[0][0];

            bottomLeftLat.value = corners[1][1];
            bottomLeftLng.value = corners[1][0];

            topRightLat.value = corners[2][1];
            topRightLng.value = corners[2][0];

            bottomRightLat.value = corners[3][1];
            bottomRightLng.value = corners[3][0];

            
            this.setMapOverlay(corners, window.MapRegistry.formMap);
        });
    }

    setMapOverlay(corners, mapReference) {
        const imageUrl = document.getElementById("ImageUrl").value;
        if (!imageUrl) {
            console.error('Image URL is missing!');
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
        });

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
        clearMarkers();
        resetState();

        if (this.map.getSource('overlay')) {
            this.map.removeLayer('overlay-layer');
            this.map.removeSource('overlay');
        }
    }
}
