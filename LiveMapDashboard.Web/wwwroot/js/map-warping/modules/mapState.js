/**
 * Array to hold map points across the modules.
 * @type {*[]}
 */
export const mapPoints = [];

/**
 * Array to hold marker elements for the map across the modules.
 * @type {*[]}
 */
export const markerElements = [];

/**
 * A simple helper to clear all markers
 */
export function clearMarkers() {
    while (markerElements.length > 0) {
        markerElements.pop().remove();
    }
    mapPoints.length = 0;
}
