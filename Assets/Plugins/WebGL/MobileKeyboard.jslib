mergeInto(LibraryManager.library, {
  MK_Init: function () {
    if (Module.mkb_input) return;

    var input = document.createElement('input');
    input.type = 'text';
    input.autocapitalize = 'none';
    input.autocomplete = 'off';
    input.autocorrect = 'off';
    input.spellcheck = false;

    var s = input.style;
    s.position = 'fixed';
    s.left = '0';
    s.bottom = '0';
    s.width = '100%';
    s.height = '1px';
    s.opacity = '0';
    s.zIndex = '99999';
    s.background = 'transparent';
    s.border = '0';
    s.color = 'transparent';
    s.caretColor = 'transparent';

    input.addEventListener('input', function () {
      if (Module.mkb_receiver) {
        SendMessage(Module.mkb_receiver, 'OnMobileKeyboardInput', input.value);
      }
    });

    document.body.appendChild(input);
    Module.mkb_input = input;
    // Nota: NO fijamos receptor aquí.
  },

  // Fija dinámicamente el GO receptor de las teclas
  MK_SetReceiver: function (goNamePtr) {
    var goName = UTF8ToString(goNamePtr || 0);
    Module.mkb_receiver = goName || null;
  },

  MK_Show: function (typePtr) {
    if (!Module.mkb_input) return;
    var type = UTF8ToString(typePtr || 0) || 'text';
    Module.mkb_input.type = type;
    try { Module.mkb_input.focus({ preventScroll: true }); } catch(e){}
    setTimeout(function () {
      try { Module.mkb_input.focus({ preventScroll: true }); } catch(e){}
    }, 0);
  },

  MK_Hide: function () {
    if (!Module.mkb_input) return;
    try { Module.mkb_input.blur(); } catch(e){}
  },

  MK_SetValue: function (valPtr) {
    if (!Module.mkb_input) return;
    Module.mkb_input.value = UTF8ToString(valPtr || 0) || '';
  }
});
