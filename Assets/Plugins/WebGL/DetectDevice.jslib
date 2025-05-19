mergeInto(LibraryManager.library, {
    IsTouchDevice: function () {
        if (typeof navigator !== "undefined") {
            return (navigator.maxTouchPoints > 0 || 'ontouchstart' in window || 'ontouchstart' in document.documentElement) ? 1 : 0;
        }
        return 0;
    }
});
