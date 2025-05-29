// src/modules/mapState.js
export const mapPoints = [];
export const markerElements = [];

export function clearMarkers() {
    while (markerElements.length > 0) {
        markerElements.pop().remove();
    }
    mapPoints.length = 0;
}
