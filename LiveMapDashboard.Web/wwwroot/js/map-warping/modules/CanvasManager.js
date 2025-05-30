import { drawCanvas } from './canvasUtils.js';
import { setupCanvasEvents } from './canvasEvents.js';
import { state, resetState } from './canvasState.js';

/**
 * Manages the canvas for image related operations such as points, scaling, and panning.
 */
export class CanvasManager {
    constructor(canvas, image) {
        this.canvas = canvas;
        this.image = image;
        this.ctx = canvas.getContext('2d');

        setupCanvasEvents(canvas, image, this.ctx);

        window.addEventListener('resize', () => this.resizeCanvas());
        this.resizeCanvas();
    }

    resizeCanvas() {
        const container = this.canvas.parentElement;
        const rect = container.getBoundingClientRect();

        this.canvas.width = rect.width;
        this.canvas.height = rect.height;

        drawCanvas(this.ctx, this.image, state.scale, state.offsetX, state.offsetY);
    }

    resetCanvas() {
        resetState();
        drawCanvas(this.ctx, this.image, state.scale, state.offsetX, state.offsetY);
    }
}
