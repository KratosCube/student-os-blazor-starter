window.appNotes = {
  autoGrow: function (element) {
    if (!element) return;
    element.style.height = "auto";
    element.style.height = element.scrollHeight + "px";
  }
};