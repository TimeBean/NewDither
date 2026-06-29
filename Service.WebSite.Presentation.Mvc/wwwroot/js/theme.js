(function () {
  const themeSwitcher = document.getElementById('themeSwitcher');

  if (themeSwitcher) {
    themeSwitcher.addEventListener('change', (e) => {
      const currentTheme = e.target.value;
      document.documentElement.setAttribute('data-theme', currentTheme);
      localStorage.setItem('dither_platform_theme', currentTheme);
    });
  }

  function initializeTheme() {
    const savedTheme = localStorage.getItem('dither_platform_theme') || 'light';
    document.documentElement.setAttribute('data-theme', savedTheme);
    if (themeSwitcher) {
      themeSwitcher.value = savedTheme;
    }
  }

  document.addEventListener('DOMContentLoaded', initializeTheme);
})();
