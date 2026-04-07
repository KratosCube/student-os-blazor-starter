/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Components/**/*.{razor,cshtml,html}",
    "./Pages/**/*.{razor,cshtml,html}",
    "./**/*.{razor,cshtml,html}"
  ],
  darkMode: "class",
  theme: {
    extend: {}
  },
  plugins: []
};