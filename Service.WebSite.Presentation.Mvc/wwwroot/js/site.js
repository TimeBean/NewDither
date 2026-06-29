function showToast(message, type) {
  const container = document.getElementById('toastContainer');
  if (!container) return;

  const alertBox = document.createElement('div');
  const alertClass = type === 'success' ? 'alert-success' : 'alert-error';

  alertBox.className = `alert ${alertClass} shadow-xl border border-black/10 transition-all duration-300 opacity-0 translate-y-4 max-w-sm`;

  const iconSvg = type === 'success'
    ? `<svg xmlns="http://www.w3.org/2000/svg" class="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>`
    : `<svg xmlns="http://www.w3.org/2000/svg" class="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>`;

  alertBox.innerHTML = `
    <div class="flex items-center gap-2">
      ${iconSvg}
      <span class="text-sm font-medium">${message}</span>
    </div>
  `;

  container.appendChild(alertBox);

  requestAnimationFrame(() => {
    alertBox.classList.remove('opacity-0', 'translate-y-4');
  });

  setTimeout(() => {
    alertBox.classList.add('opacity-0', 'translate-y-4');
    setTimeout(() => alertBox.remove(), 300);
  }, 3500);
}

function wireResultImageClick() {
  const img = document.getElementById('resultImage');
  if (img) {
    img.addEventListener('click', function () {
      const a = document.createElement('a');
      a.href = this.src;
      a.download = 'image.png';
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    });
  }
}

document.querySelector('form').addEventListener('submit', async (e) => {
  e.preventDefault();
  const form = e.target;
  const formData = new FormData(form);
  const submitBtn = form.querySelector('button[type="submit"]');
  const originalText = submitBtn.textContent;

  submitBtn.disabled = true;
  submitBtn.textContent = 'Обработка...';

  try {
    const response = await fetch(form.action, {
      method: 'POST',
      body: formData
    });

    if (!response.ok) {
      showToast('Ошибка сервера: ' + response.status, 'error');
      return;
    }

    const html = await response.text();
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');

    const newPanel = doc.getElementById('result-panel');
    const currentPanel = document.getElementById('result-panel');

    if (newPanel && currentPanel) {
      currentPanel.innerHTML = newPanel.innerHTML;
    }

    const errorMatch = html.match(/showToast\('([^']*)',\s*'error'\)/);
    if (errorMatch) {
      showToast(errorMatch[1], 'error');
    } else if (html.includes('Изображение успешно обработано')) {
      showToast('Изображение успешно обработано!', 'success');
      wireResultImageClick();
    }
  } catch (err) {
    showToast('Ошибка сети: ' + err.message, 'error');
  } finally {
    submitBtn.disabled = false;
    submitBtn.textContent = originalText;
  }
});
