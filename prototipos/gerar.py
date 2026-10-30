"""Gera o HTML portátil usando apenas a biblioteca padrão do Python 3."""
from html import escape
from pathlib import Path

base = Path(__file__).resolve().parent
layout = (base / 'logus-layout.html').read_text(encoding='utf-8')
data = (base / 'logus-dados.js').read_text(encoding='utf-8')
app = (base / 'logus-app.js').read_text(encoding='utf-8')
fragment = layout + '<script>\n' + data + '\n' + app + '\n</script>'
template = (base / 'logus-moldura.html').read_text(encoding='utf-8')
assert template.count('__LOGUS_FRAGMENT__') == 1
destination = base / 'logus-desktop.html'
destination.write_text(template.replace('__LOGUS_FRAGMENT__', escape(fragment, quote=True)), encoding='utf-8')
print(destination)
