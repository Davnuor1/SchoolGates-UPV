mergeInto(LibraryManager.library, {
    DetectTouchDevice: function () {
        var isTouch = /android|ipad|iphone|ipod/i.test(navigator.userAgent);
        SendMessage('DeviceDetector', 'SetTouchMode', isTouch ? 'true' : 'false');
    }
});
