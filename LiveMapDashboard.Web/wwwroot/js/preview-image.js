document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('imageUpload').addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const img = document.querySelector('#preview');
            img.src = URL.createObjectURL(file);
            img.style.display = 'block';
        }
    });
});

// KEEP THIS CODE!!!
//document.addEventListener('DOMContentLoaded', () => {
//    const imageInput = document.querySelector('#imageUpload');
//    const previewImage = document.querySelector('#previewImage');
//    const previewImageError = document.querySelector('#imagePreviewError');
//    const previewImageLoader = document.querySelector('#previewImageLoader');

//    handlePreview(imageInput, previewImage, previewImageError, previewImageLoader);

//    imageInput.addEventListener('change', () => {
//        handlePreview(imageInput, previewImage, previewImageError, previewImageLoader);
//    });

//    previewImage.onload = () => {
//        togglePreviewImageLoader(true);
//        togglePreviewImage(false);
//    };

//    previewImage.onerror = () => {
//        togglePreviewImageLoader(true);
//        togglePreviewImage(true);
//        toggleClass(previewImageError, 'hidden', false);
//    };
//});

//function handlePreview(imageInput, previewImage, previewImageError, previewImageLoader) {
//    if (!imageInput.value || imageInput.value === "") {
//        togglePreviewImage(true);
//        return;
//    }

//    togglePreviewImageLoader(false);
//    togglePreviewImage(true);
//    previewImage.src = imageInput.value;
//}

//function togglePreviewImage(hidden = false) {
//    const previewImage = document.querySelector('#previewImage');
//    const previewImageError = document.querySelector('#imagePreviewError');

//    previewImage.hidden = hidden;
//    previewImageError.hidden = hidden;

//    if (hidden) {
//        toggleClass(previewImage, 'hidden', true);
//        toggleClass(previewImageError, 'hidden', true);
//    } else {
//        toggleClass(previewImage, 'hidden', false);
//    }
//}

//function togglePreviewImageLoader(hidden = false) {
//    const previewImageLoader = document.querySelector('#previewImageLoader');
//    toggleClass(previewImageLoader, 'hidden', hidden);
//}

//function toggleClass(element, className, add = true) {
//    if (!element) {
//        return;
//    }

//    if (add) {
//        if (!element.classList.contains(className)) {
//            element.classList.add(className);
//        }
//    } else {
//        if (element.classList.contains(className)) {
//            element.classList.remove(className);
//        }
//    }
//}