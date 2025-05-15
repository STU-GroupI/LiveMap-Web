document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById("reject_form");
    const btnSubmitDelete = document.getElementById("btn_reject_rfc");

    btnSubmitDelete.addEventListener("click", (e) => {
        e.preventDefault();
        e.stopPropagation();

        form.submit();
    });
})