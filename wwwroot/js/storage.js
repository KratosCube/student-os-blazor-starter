window.appStorage = {
  // Načte hodnotu podle klíče
  get: function (key) {
    return localStorage.getItem(key);
  },
  // Uloží textovou hodnotu pod zadaný klíč
  set: function (key, value) {
    localStorage.setItem(key, value);
  },
  // Smaže hodnotu podle klíče
  remove: function (key) {
    localStorage.removeItem(key);
  }
};
