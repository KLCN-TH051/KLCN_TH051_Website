window.Toast = (function () {

    // Tạo toast container nếu chưa có
    function ensureContainer() {
        if (!document.getElementById('toastContainer')) {
            let container = document.createElement('div');
            container.id = 'toastContainer';
            container.className = 'position-fixed top-0 end-0 p-3';
            container.style.zIndex = '1055';
            document.body.appendChild(container);
        }
    }

    // Hiển thị toast
    function show(message, type = 'danger', delay = 3000) {
        ensureContainer();

        let container = document.getElementById('toastContainer');

        let toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-bg-${type} border-0`;
        toastEl.role = 'alert';
        toastEl.ariaLive = 'assertive';
        toastEl.ariaAtomic = 'true';
        toastEl.dataset.bsDelay = delay;

        toastEl.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">${message}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;

        container.appendChild(toastEl);

        let toast = new bootstrap.Toast(toastEl);
        toast.show();

        toastEl.addEventListener('hidden.bs.toast', function () {
            toastEl.remove();
        });
    }

    return {
        show: show
    };

})();
