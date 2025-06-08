/**
 * Close and remove a toast notification by its ID
 * @param id
 */
function closeToast(id) {
    const toast = document.getElementById(id);
    if (!toast) return;

    toast.classList.remove('toast-enter-active');
    toast.classList.add('toast-exit-active');

    toast.addEventListener('transitionend', () => {
        toast.remove();
    }, { once: true });
}

/**
 * Create and add a toast notification to the DOM
 * @param id
 * @param type
 * @param message
 * @param duration
 * @param container
 */
function createToast({ id, type = 'info', message = '', duration = 5000, container = document.body }) {
    if (!container) {
        container = document.getElementById('toast-container');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.className = 'fixed top-5 right-5 flex flex-col space-y-4 z-50';
            document.body.appendChild(container);
        }
    }
    
    const colors = {
        success: {
            text: "text-green-800",
            bg: "bg-green-50",
            border: "border-green-200",
            iconBg: "bg-green-100",
            iconText: "text-green-500",
            darkText: "dark:text-green-400",
            darkBg: "dark:bg-green-900",
            darkBorder: "dark:border-green-800",
            darkIconBg: "dark:bg-green-800",
            darkIconText: "dark:text-green-200"
        },
        error: {
            text: "text-red-800",
            bg: "bg-red-50",
            border: "border-red-200",
            iconBg: "bg-red-100",
            iconText: "text-red-500",
            darkText: "dark:text-red-400",
            darkBg: "dark:bg-red-900",
            darkBorder: "dark:border-red-800",
            darkIconBg: "dark:bg-red-800",
            darkIconText: "dark:text-red-200"
        },
        warning: {
            text: "text-yellow-800",
            bg: "bg-yellow-50",
            border: "border-yellow-200",
            iconBg: "bg-yellow-100",
            iconText: "text-yellow-500",
            darkText: "dark:text-yellow-400",
            darkBg: "dark:bg-yellow-900",
            darkBorder: "dark:border-yellow-800",
            darkIconBg: "dark:bg-yellow-800",
            darkIconText: "dark:text-yellow-200"
        },
        info: {
            text: "text-blue-800",
            bg: "bg-blue-50",
            border: "border-blue-200",
            iconBg: "bg-blue-100",
            iconText: "text-blue-500",
            darkText: "dark:text-blue-400",
            darkBg: "dark:bg-blue-900",
            darkBorder: "dark:border-blue-800",
            darkIconBg: "dark:bg-blue-800",
            darkIconText: "dark:text-blue-200"
        },
        default: {
            text: "text-gray-800",
            bg: "bg-gray-50",
            border: "border-gray-200",
            iconBg: "bg-gray-100",
            iconText: "text-gray-500",
            darkText: "dark:text-gray-400",
            darkBg: "dark:bg-gray-900",
            darkBorder: "dark:border-gray-800",
            darkIconBg: "dark:bg-gray-800",
            darkIconText: "dark:text-gray-200"
        }
    };

    const color = colors[type.toLowerCase()] || colors.default;

    const toast = document.createElement('div');
    toast.id = id;
    toast.setAttribute('role', 'alert');
    toast.className = `flex items-center w-full max-w-md p-3 mb-4
      ${color.text} ${color.bg} ${color.border} border rounded-lg shadow
      ${color.darkText} ${color.darkBg} ${color.darkBorder} z-50 toast-enter`;


    const iconDiv = document.createElement('div');
    iconDiv.className = `inline-flex items-center justify-center flex-shrink-0 w-10 h-10
        ${color.iconText} ${color.iconBg} rounded-lg ${color.darkIconBg} ${color.darkIconText}`;

    if (type === 'success') {
        iconDiv.innerHTML = `
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-circle-check-icon lucide-circle-check shrink size-4"><circle cx="12" cy="12" r="10"/><path d="m9 12 2 2 4-4"/></svg>
          `;
    } else {
        iconDiv.innerHTML = `
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-bell-icon lucide-bell shrink size-4"><path d="M10.268 21a2 2 0 0 0 3.464 0"/><path d="M3.262 15.326A1 1 0 0 0 4 17h16a1 1 0 0 0 .74-1.673C19.41 13.956 18 12.499 18 8A6 6 0 0 0 6 8c0 4.499-1.411 5.956-2.738 7.326"/></svg>
          `;
    }

    const messageDiv = document.createElement('div');
    messageDiv.className = 'ml-4 text-sm font-medium';
    messageDiv.textContent = message;

    const closeButton = document.createElement('button');
    closeButton.type = 'button';
    closeButton.className = `ml-auto -mx-1.5 -my-1.5 ${color.iconText} rounded-lg p-1.5 hover:${color.iconBg} ${color.darkIconBg} inline-flex h-8 w-8`;
    closeButton.setAttribute('aria-label', 'Close');
    closeButton.innerHTML = `
        <span class="sr-only">Close</span>
        <svg class="shrink-0 size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M18 6 6 18"></path>
          <path d="m6 6 12 12"></path>
        </svg>
      `;

    closeButton.onclick = () => closeToast(id);

    toast.appendChild(iconDiv);
    toast.appendChild(messageDiv);
    toast.appendChild(closeButton);

    container.prepend(toast);
    toast.offsetHeight;
    toast.classList.add('toast-enter-active');

    setTimeout(() => closeToast(id), duration);
}

/**
 * Show an alert notification
 * @param type
 * @param message
 */
function showAlert(type, message) {
    const typeMap = {
        danger: 'error',
        error: 'error',
        warning: 'warning',
        success: 'success',
        info: 'info'
    };

    const toastType = typeMap[type.toLowerCase()] || 'default';
    const id = `toast-${Date.now()}-${Math.floor(Math.random() * 1000)}`;
    const container = document.querySelector('#alertContainer') || document.body;

    createToast({
        id,
        type: toastType,
        message,
        duration: 5000,
        container
    });
}
