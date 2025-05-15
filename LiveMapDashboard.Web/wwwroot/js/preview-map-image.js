document.addEventListener('DOMContentLoaded', () => {
    const imageInput = document.querySelector('#ImageUrl');
    if (!imageInput) return; // Exit if the element doesn't exist

    const previewImage = document.querySelector('#previewImage');
    const previewImageError = document.querySelector('#imagePreviewError');
    const previewImageLoader = document.querySelector('#previewImageLoader');

    handlePreview(imageInput, previewImage, previewImageError, previewImageLoader);

    imageInput.addEventListener('change', () => {
        handlePreview(imageInput, previewImage, previewImageError, previewImageLoader);
    });

    if (previewImage) {
        previewImage.onload = () => {
            togglePreviewImageLoader(true);
            togglePreviewImage(false);
        };

        previewImage.onerror = () => {
            togglePreviewImageLoader(true);
            togglePreviewImage(true);
            toggleClass(previewImageError, 'hidden', false);
        };
    }
});

function handlePreview(imageInput, previewImage, previewImageError, previewImageLoader) {
    if (!imageInput || !imageInput.value || imageInput.value === "") {
        togglePreviewImage(true);
        return;
    }

    togglePreviewImageLoader(false);
    togglePreviewImage(true);
    previewImage.src = imageInput.value;
}

function togglePreviewImage(hidden = false) {
    const previewImage = document.querySelector('#previewImage');
    const previewImageError = document.querySelector('#imagePreviewError');

    if (!previewImage || !previewImageError) return;

    previewImage.hidden = hidden;
    previewImageError.hidden = hidden;

    if (hidden) {
        toggleClass(previewImage, 'hidden', true);
        toggleClass(previewImageError, 'hidden', true);
    } else {
        toggleClass(previewImage, 'hidden', false);
    }
}

function togglePreviewImageLoader(hidden = false) {
    const previewImageLoader = document.querySelector('#previewImageLoader');
    if (!previewImageLoader) return;

    toggleClass(previewImageLoader, 'hidden', hidden);
}

function toggleClass(element, className, add = true) {
    if (!element) {
        return;
    }

    if (add) {
        if (!element.classList.contains(className)) {
            element.classList.add(className);
        }
    } else {
        if (element.classList.contains(className)) {
            element.classList.remove(className);
        }
    }
}