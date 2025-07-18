import { drawCanvas } from './canvasUtils.js';
import { state } from './canvasState.js';
import { mapPoints, clearMarkers } from './mapState.js';

/**
 * Handles image uploading and initialization for the canvas.
 */
export class ImageUploader {
    constructor({ image, canvasManager, onImageLoaded }) {
        this.canvasManager = canvasManager;
        this.image = image;
        this.onImageLoaded = onImageLoaded;
        this.setupFileInput();
    }

    setupFileInput() {
        const input = document.getElementById('imageUpload');
        const step2 = document.getElementById('step-2');
        const bottomToolkit = document.getElementById('bottom-toolkit');

        input.addEventListener('change', (e) => {
            const file = e.target.files[0];

            if (!file) return;
            
            bottomToolkit.classList.add('opacity-20', 'pointer-events-none');
            step2.classList.add('opacity-20', 'pointer-events-none');

            const reader = new FileReader();
            reader.onload = (event) => {
                this.image.src = event.target.result;

                this.image.onload = () => {
                    this.canvasManager.resizeCanvas();

                    requestAnimationFrame(() => {
                        drawCanvas(this.canvasManager.ctx, this.image, state.scale, state.offsetX, state.offsetY);

                        bottomToolkit.classList.remove('opacity-20', 'pointer-events-none');
                        step2.classList.remove('opacity-20', 'pointer-events-none');

                        clearMarkers();
                        this.onImageLoaded?.();
                        showAlert('success', 'Image loaded successfully!');
                    });
                };

            };
            reader.readAsDataURL(file);
        });
    }
}
