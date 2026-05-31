(function () {
  // Vrátí uložený režim vzhledu
  function getPreferredTheme() {
    const saved = localStorage.getItem("theme");
    if (saved === "dark" || saved === "light") {
      return saved;
    }

    return window.matchMedia("(prefers-color-scheme: dark)").matches
      ? "dark"
      : "light";
  }

  // Aplikuje theme do HTML elementu
  function applyTheme(theme) {
    const root = document.documentElement;

    if (theme === "dark") {
      root.classList.add("dark");
    } else {
      root.classList.remove("dark");
    }
  }

  const initialTheme = getPreferredTheme();
  applyTheme(initialTheme);

  window.appTheme = {
    get: function () {
      return getPreferredTheme();
    },
    toggle: function () {
      const current = getPreferredTheme();
      const next = current === "dark" ? "light" : "dark";
      localStorage.setItem("theme", next);
      applyTheme(next);
      return next;
    }
  };
})();
