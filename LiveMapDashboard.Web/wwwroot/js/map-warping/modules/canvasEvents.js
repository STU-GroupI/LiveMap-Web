import { state } from './canvasState.js';
import { drawCanvas } from './canvasUtils.js';

/**
 * Sets up event listeners for canvas interactions.
 * @param canvas
 * @param image
 * @param ctx
 */
export function setupCanvasEvents(canvas, image, ctx) {
    let dragging = false;
    let lastX, lastY;
    let isClick = false;

    canvas.style.cursor = 'grab';

    canvas.addEventListener('mousedown', (e) => {
        dragging = true;
        isClick = true;
        lastX = e.clientX;
        lastY = e.clientY;
        canvas.style.cursor = 'grabbing';
    });

    canvas.addEventListener('mouseup', (e) => {
        dragging = false;
        canvas.style.cursor = 'grab';

        if (isClick) {
            const rect = canvas.getBoundingClientRect();
            const x = (e.clientX - rect.left - state.offsetX) / state.scale;
            const y = (e.clientY - rect.top - state.offsetY) / state.scale;

            const markerIndex = state.imagePoints.findIndex(([mx, my]) => {
                const distance = Math.sqrt((mx - x) ** 2 + (my - y) ** 2);
                return distance <= 12 / state.scale;
            });

            if (markerIndex !== -1) {
                state.imagePoints.splice(markerIndex, 1);
            } else {
                state.imagePoints.push([x, y]);
            }

            drawCanvas(ctx, image, state.scale, state.offsetX, state.offsetY);
        }
    });

    canvas.addEventListener('mouseleave', () => {
        dragging = false;
        canvas.style.cursor = 'default';
    });

    canvas.addEventListener('mousemove', (e) => {
        if (dragging) {
            isClick = false;
            state.offsetX += e.clientX - lastX;
            state.offsetY += e.clientY - lastY;
            lastX = e.clientX;
            lastY = e.clientY;
            drawCanvas(ctx, image, state.scale, state.offsetX, state.offsetY);
        }
    });

    canvas.addEventListener('wheel', (e) => {
        e.preventDefault();

        const minScale = 0.3;
        const maxScale = 5;

        const delta = e.deltaY < 0 ? 1.1 : 0.9;
        const rect = canvas.getBoundingClientRect();
        const cx = (e.clientX - rect.left - state.offsetX) / state.scale;
        const cy = (e.clientY - rect.top - state.offsetY) / state.scale;

        const newScale = Math.min(maxScale, Math.max(minScale, state.scale * delta));

        state.offsetX = e.clientX - rect.left - cx * newScale;
        state.offsetY = e.clientY - rect.top - cy * newScale;

        state.scale = newScale;

        drawCanvas(ctx, image, state.scale, state.offsetX, state.offsetY);
    });
}
