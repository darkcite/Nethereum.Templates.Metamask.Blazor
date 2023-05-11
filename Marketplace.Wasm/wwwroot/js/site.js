window.openModal = function (modalId) {
    let modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
}

window.closeModal = function (modalId) {
    let modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) {
        modal.hide();
    }
}

function openModal(modalId) {
    $(`#${modalId}`).modal('show');
}

function closeModal(modalId) {
    $(`#${modalId}`).modal('hide');
}