// src/modules/MarkerList.js
export const MarkerList = {
    update(containerId, points) {
        const container = document.getElementById(containerId);
        container.innerHTML = '';

        points.forEach((marker, index) => {
            const listItem = document.createElement('div');
            listItem.textContent = index + 1; // Display the marker number
            listItem.className = 'flex items-center justify-center w-8 h-8 bg-slate-100 border-2 text-blue-800 font-bold rounded-md shadow-md';
            container.appendChild(listItem);
        });

        for (let i = points.length; i < 6; i++) {
            const placeholder = document.createElement('div');
            placeholder.textContent = i + 1; // Display the placeholder number
            placeholder.className = 'flex items-center justify-center w-8 h-8 bg-gray-200 border-2 text-gray-500 font-bold rounded-md shadow-md opacity-20';
            container.appendChild(placeholder);
        }
    }
};
