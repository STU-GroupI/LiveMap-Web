document.addEventListener('DOMContentLoaded', () => {
    document.querySelector('#timeSort').addEventListener('click', () => {
        const url = new URL(window.location.href);
        const currentVal = url.searchParams.get("ascending");

        if (currentVal === "true") {
            url.searchParams.set("ascending", "false");
        } else {
            url.searchParams.set("ascending", "true");
        }

        window.location.href = url;
    });
});