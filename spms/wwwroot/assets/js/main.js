/**
 * NiceAdmin Main JavaScript
 * Handles sidebar toggle, search bar toggle, and back-to-top button.
 */
(function () {
    "use strict";

    /* ==============================
       Sidebar Toggle
    ============================== */
    const toggleSidebarBtn = document.querySelector('.toggle-sidebar-btn');
    if (toggleSidebarBtn) {
        toggleSidebarBtn.addEventListener('click', function () {
            document.body.classList.toggle('toggle-sidebar');
        });
    }

    /* ==============================
       Search Bar Toggle (Mobile)
    ============================== */
    const searchBarToggle = document.querySelector('.search-bar-toggle');
    if (searchBarToggle) {
        searchBarToggle.addEventListener('click', function () {
            document.querySelector('.search-bar').classList.toggle('search-bar-show');
        });
    }

    /* ==============================
       Back to Top Button
    ============================== */
    const backToTop = document.querySelector('.back-to-top');
    if (backToTop) {
        const toggleBacktotop = function () {
            if (window.scrollY > 100) {
                backToTop.classList.add('active');
            } else {
                backToTop.classList.remove('active');
            }
        };
        window.addEventListener('load', toggleBacktotop);
        document.addEventListener('scroll', toggleBacktotop);

        backToTop.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    /* ==============================
       Client-side Search Filter
       Filters table rows based on the search input
    ============================== */
    const searchInput = document.getElementById('tableSearchInput');
    if (searchInput) {
        searchInput.addEventListener('keyup', function () {
            const filter = this.value.toLowerCase();
            const table = document.getElementById('dataTable');
            if (table) {
                const rows = table.querySelectorAll('tbody tr');
                rows.forEach(function (row) {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(filter) ? '' : 'none';
                });
            }
        });
    }

})();
