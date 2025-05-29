import { state } from './canvasState.js';

export function drawCanvas(ctx, image, scale, offsetX, offsetY) {
    ctx.setTransform(1, 0, 0, 1, 0, 0);
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);

    ctx.setTransform(scale, 0, 0, scale, offsetX, offsetY);
    ctx.drawImage(image, 0, 0);

    state.imagePoints.forEach(([x, y], i) => {
        ctx.beginPath();
        ctx.arc(x, y, 12 / scale, 0, 2 * Math.PI);
        ctx.fillStyle = '#f7f7ff';
        ctx.fill();
        ctx.lineWidth = 2 / scale;
        ctx.strokeStyle = '#0017EE';
        ctx.stroke();
        ctx.font = `${14 / scale}px sans-serif`;
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillStyle = '#0017EE';
        ctx.fillText((i + 1).toString(), x, y);
    });
}
