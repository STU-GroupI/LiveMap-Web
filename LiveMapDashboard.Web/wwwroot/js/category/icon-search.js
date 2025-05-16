import * as mdi from 'https://cdn.jsdelivr.net/npm/@mdi/js/+esm';

function searchIcons(query) {
    const lower = query.toLowerCase();
    return Object.entries(mdi)
        .filter(([name]) => name.toLowerCase().includes(lower))
        .map(([name, path]) => ({ name, path }));
}

function renderResults(results) {
    const container = document.getElementById('iconSearchResults');
    container.innerHTML = '';
    results.forEach(icon => {
        const wrapper = document.createElement('div');
        wrapper.className = 'flex items-center gap-2 px-2 py-1 cursor-pointer hover:bg-gray-100 dark:hover:bg-neutral-800 rounded';
        wrapper.innerHTML = `
    <svg viewBox="0 0 24 24" width="20" height="20"><path d="${icon.path}" /></svg>
    <span>${icon.name}</span>
    `;
        wrapper.addEventListener('click', () => {
            document.getElementById('iconSearchInput').value = icon.name;
            document.getElementById('IconName').value = icon.name;
            // clear out the results
            container.innerHTML = '';

            // build a “chosen” display
            const chosen = document.createElement('div');
            chosen.className = 'flex items-center gap-2 px-2 py-1 rounded bg-blue-50 dark:bg-neutral-900';
            chosen.innerHTML = `
    <svg viewBox="0 0 24 24" width="20" height="20">
      <path d="${icon.path}" />
    </svg>
    <span>${icon.name} has been chosen</span>
  `;

            container.appendChild(chosen);
        });
        container.appendChild(wrapper);
    });
}

document.getElementById('iconSearchInput')?.addEventListener('input', (e) => {
    const results = searchIcons(e.target.value);
    renderResults(results.slice(0, 50));
});

document.addEventListener('DOMContentLoaded', () => {
    const input = document.getElementById('iconSearchInput');
    if (input && input.value.trim()) {
        const value = input.value.trim();
        const result = searchIcons(value).find(i => i.name === value);
        renderResults([result]);
    }
});



