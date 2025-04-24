document.addEventListener('DOMContentLoaded', () => {
    const imageInput = document.querySelector('#image');
    const previewImage = document.querySelector('#previewImage');
    const previewImageError = document.querySelector('#imagePreviewError');

    previewImageError.hidden = true;
    previewImage.src = imageInput.value;

    imageInput.addEventListener('change', () => {
        previewImageError.hidden = true;
        previewImage.src = imageInput.value;
    });

    previewImage.onerror = () => {
        previewImage.src = '';
        previewImageError.hidden = false;
    };
});