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

    document.querySelector('#toPreviousPage').addEventListener('click', () => {
        addToPaginationSkip(-getPaginationTake());
    });

    try {
        document.querySelector('#toNextPage').addEventListener('click', () => {
            addToPaginationSkip(getPaginationTake());
        });
    } catch (e) {

    }

    document.querySelectorAll('.paginationClick').forEach(button => {
        button.addEventListener('click', (e) => {
            const pageNumber = parseInt(e.target.textContent); // Get number from button
            const take = getPaginationTake(); // Get the number of items per page
            const offset = (pageNumber - 1) * take;

            console.log(`Page: ${pageNumber}, Take: ${take}, Offset: ${offset}`);
            setPaginationSkip(offset);
        });
    });
});

function getPaginationTake() {
    const url = new URL(window.location.href);
    const currentVal = url.searchParams.get("take");
    if (currentVal === null) {
        return 5;
    }

    return currentVal;
}

function addToPaginationSkip(amount) {
    const url = new URL(window.location.href);
    const currentVal = url.searchParams.get("skip");
    if (currentVal === null) {
        url.searchParams.set("skip", 0);
    }

    let newValue = parseInt(currentVal) + parseInt(amount);
    if (newValue < 0) {
        newValue = 0;
    }
    url.searchParams.set("skip", newValue);
    window.location.href = url;
}

function setPaginationSkip(amount) {
    const url = new URL(window.location.href);
    url.searchParams.set("skip", parseInt(amount));
    window.location.href = url;
}