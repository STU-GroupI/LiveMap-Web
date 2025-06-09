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