export default class CoursePagination {

    constructor({ data = [], pageSize = 6, onPageChange = () => { } }) {
        this.data = data;
        this.pageSize = pageSize;
        this.currentPage = 1;
        this.onPageChange = onPageChange;

        this.paginationEl = document.querySelector("#pagination");

        this.renderPagination();
        this.emitPageData();
    }

    get totalPages() {
        return Math.max(1, Math.ceil(this.data.length / this.pageSize));
    }

    getPageData(page) {
        const start = (page - 1) * this.pageSize;
        return this.data.slice(start, start + this.pageSize);
    }

    renderPagination() {
        if (!this.paginationEl) return;

        let html = "";

        // Prev
        html += `
            <li class="page-item ${this.currentPage === 1 ? "disabled" : ""}">
                <a class="page-link" data-page="${this.currentPage - 1}" href="#">«</a>
            </li>
        `;

        // Pages
        for (let i = 1; i <= this.totalPages; i++) {
            html += `
                <li class="page-item ${i === this.currentPage ? "active" : ""}">
                    <a class="page-link" data-page="${i}" href="#">${i}</a>
                </li>
            `;
        }

        // Next
        html += `
            <li class="page-item ${this.currentPage === this.totalPages ? "disabled" : ""}">
                <a class="page-link" data-page="${this.currentPage + 1}" href="#">»</a>
            </li>
        `;

        this.paginationEl.innerHTML = html;

        // Attach events cleanly
        this.paginationEl.querySelectorAll("a").forEach(a => {
            a.onclick = (e) => {
                e.preventDefault();
                this.goToPage(Number(a.dataset.page));
            };
        });
    }

    goToPage(page) {
        if (page < 1 || page > this.totalPages) return;

        this.currentPage = page;
        this.renderPagination();
        this.emitPageData();
    }

    emitPageData() {
        this.onPageChange(this.getPageData(this.currentPage), this.currentPage);
    }

    updateData(newData) {
        this.data = newData;

        // nếu không có data -> clear pagination UI
        if (newData.length === 0) {
            this.paginationEl.innerHTML = "";
            this.onPageChange([], 1);
            return;
        }

        this.currentPage = 1;
        this.renderPagination();
        this.emitPageData();
        window.scrollTo({ top: 0, behavior: "smooth" });
    }

}
