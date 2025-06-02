/**
 * A simple state object to hold the current state of the canvas.
 * @type {{offsetX: number, offsetY: number, scale: number, imagePoints: *[]}}
 */
export const state = {
    imagePoints: [],
    scale: 1,
    offsetX: 0,
    offsetY: 0,
};

/**
 * Resets the state of the canvas to its initial values.
 */
export function resetState() {
    state.imagePoints.length = 0;
    state.scale = 1;
    state.offsetX = 0;
    state.offsetY = 0;
}
