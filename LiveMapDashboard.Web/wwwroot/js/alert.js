function showAlert(type, message) {
    const alert = document.createElement('div');
    let clone;

    switch (type) {
        case 'danger':
        case 'error':
            clone = document.getElementById('danger-alert').content.cloneNode(true);
            break;
        case 'warning':
            clone = document.getElementById('warning-alert').content.cloneNode(true);
            break;
        case 'success':
            clone = document.getElementById('success-alert').content.cloneNode(true);
            break;
        case 'info':
        default:
            clone = document.getElementById('info-alert').content.cloneNode(true);
            break;
    }

    clone.querySelector('#alert-text').innerHTML = message;
    alert.appendChild(clone);

    setTimeout(() => {
        alert.classList.add('fade-out');

        // After 4 seconds (1 second for fade-out), remove the alert.
        setTimeout(() => {
            alert.remove();
        }, 1000);
    }, 4000);

    document.querySelector('#alertContainer').append(alert);
}