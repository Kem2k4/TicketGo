/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.cshtml", // Nếu dùng Razor Pages
    "./Views/**/*.cshtml", // Nếu dùng MVC
    "./wwwroot/**/*.html"  // File HTML tĩnh (nếu có)
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}

