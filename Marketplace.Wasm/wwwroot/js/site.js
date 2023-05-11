window.openModal = (modalId) => {
    var modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
};

window.closeModal = (modalId) => {
    var modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.hide();
};
function openModal(modalId) {
    $(`#${modalId}`).modal('show');
}

function closeModal(modalId) {
    $(`#${modalId}`).modal('hide');
}