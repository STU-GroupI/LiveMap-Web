// src/modules/canvasState.js
export const state = {
    imagePoints: [],
    scale: 1,
    offsetX: 0,
    offsetY: 0,
};

export function resetState() {
    state.imagePoints.length = 0;
    state.scale = 1;
    state.offsetX = 0;
    state.offsetY = 0;
}
