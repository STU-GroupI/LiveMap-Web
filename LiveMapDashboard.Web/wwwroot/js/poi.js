document.addEventListener('DOMContentLoaded', () => {
    const imageInput = document.querySelector('#image');
    const previewImage = document.querySelector('#previewImage');
    const previewImageError = document.querySelector('#imagePreviewError');

    handlePreview(imageInput, previewImage, previewImageError);

    imageInput.addEventListener('change', () => {
        handlePreview(imageInput, previewImage, previewImageError);
    });

    previewImage.onerror = () => {
        if (!previewImage.src || previewImage.src.endsWith('blank') || previewImage.src === window.location.href) {
            return;
        }
        previewImage.hidden = true;
        previewImageError.hidden = false;
    };
});

function handlePreview(imageInput, previewImage, previewImageError) {
    previewImageError.hidden = true;
    previewImage.hidden = false;
    previewImage.src = imageInput.value;
}