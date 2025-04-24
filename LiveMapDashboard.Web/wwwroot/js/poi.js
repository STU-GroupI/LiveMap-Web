document.addEventListener('DOMContentLoaded', () => {
    const imageInput = document.querySelector('#image');
    imageInput.addEventListener('change', () => {
        document.querySelector('#previewImage').src = imageInput.value;
    });
});