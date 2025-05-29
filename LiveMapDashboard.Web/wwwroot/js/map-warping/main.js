// src/main.js
import { ImageUploader } from './modules/ImageUploader.js';
import { MarkerList } from './modules/MarkerList.js';
import { CanvasManager } from './modules/CanvasManager.js';
import { MapManager } from './modules/MapManager.js';
import { state } from './modules/canvasState.js';
import { mapPoints } from './modules/mapState.js';
import {drawCanvas} from "./modules/canvasUtils.js";

const canvasEl = document.getElementById('image-canvas');
const image = new Image();
const mapContainerId = 'warping-map';

const canvasManager = new CanvasManager(canvasEl, image);
const mapManager = new MapManager(mapContainerId, image);

new ImageUploader({
    image,
    canvasManager,
    onImageLoaded: () => {
        MarkerList.update('image-marker-list', state.imagePoints);
        MarkerList.update('map-marker-list', mapPoints);
    }
});

MarkerList.update('image-marker-list', state.imagePoints);
MarkerList.update('map-marker-list', mapPoints);

document.getElementById('image-canvas').addEventListener('click', () => {
    MarkerList.update('image-marker-list', state.imagePoints);
});

document.getElementById('warping-map').addEventListener('click', () => {
    MarkerList.update('map-marker-list', mapPoints);
});

document.getElementById('reset').addEventListener('click', () => {
    MarkerList.update('image-marker-list', []);
    MarkerList.update('map-marker-list', []);

    drawCanvas(canvasManager.ctx, image, state.scale, state.offsetX, state.offsetY);
});