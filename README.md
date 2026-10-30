# Logus Educacional

Sistema interno para organizar solicitações de certificado e acompanhar alunos interessados em continuar os estudos.

**Fase atual: documentação funcional e protótipo navegável. Backend ainda não iniciado.**

## Começar por aqui

- [Regras e fluxos — versão 1.0](docs/01-visao-e-fluxo.md)
- [Como trabalhar com branch, commits e pull request](docs/02-fluxo-github.md)
- [Protótipo navegável](prototipos/logus-desktop.html)

Baixe ou clone o repositório e abra `prototipos/logus-desktop.html` no navegador. O GitHub mostra o código desse arquivo; não há site publicado no GitHub Pages. Referência desktop: **1920 × 1080**.

## O que experimentar

1. Em Solicitações, crie um pedido com dados fictícios e de **1 a 3 interesses**. Ele entra em andamento e já gera acompanhamento comercial, sem médias.
2. Selecione um curso para carregar seus módulos; adapte as linhas, preencha médias/frequência, salve e confira antes de encaminhar.
3. No topo, troque o **Funcionário da demonstração** para Certificados e confirme a emissão externa. Use Professor para corrigir e observar o retorno para revisão.
4. Use Comercial para registrar contatos, recusar, reabrir e criar nova oportunidade. Em Direção, experimente os cadastros de cursos e colaboradores.
5. Informe novamente o CPF de um aluno: o aviso permite abrir a solicitação existente ou criar outra, reaproveitando o cadastro.

A média é aritmética simples. Parte decimal **a partir de 0,90** sobe ao próximo inteiro: 6,90 e 6,95 viram 7; 6,89 permanece 6,89. Aprovação exige média final ≥ 7 e frequência ≥ 75%, além dos dados e conferência completos.

Há 20 alunos fictícios. As alterações nos registros e a foto são locais e se perdem ao recarregar. Perfis e acesso são **simulados**: não há autenticação real, banco ou segurança de backend. Não use dados pessoais reais na demonstração.

## Fontes do protótipo

| Arquivo | Papel |
| --- | --- |
| `prototipos/logus-layout.html` | Telas e estilos. |
| `prototipos/logus-dados.js` | Exemplos fictícios. |
| `prototipos/logus-app.js` | Interações e regras demonstradas. |
| `prototipos/logus-moldura.html` | Moldura portátil da prévia exportada, com isolamento em iframe. |
| `prototipos/gerar.py` | Une as fontes no HTML final, usando Python 3 sem dependências extras. |
| `prototipos/logus-desktop.html` | Arquivo gerado para abrir no navegador; não editar diretamente. |

Depois de alterar uma fonte:

```sh
python prototipos/gerar.py
```

## Verificações

Os testes verificam os fluxos no navegador, incluindo limites das médias, revisão após emissão, duplicidade, catálogo e comercial, além das telas principais em 1920, 1366, 1024, 736 e 320 pixels de largura.

Com Node.js 20 ou superior:

```sh
npm ci
npx playwright install chromium
npm test
```

Como alternativa ao Chromium do Playwright, configure `LOGUS_CHROME_PATH` com o caminho do Chrome instalado antes de executar os testes. Eles abrem o arquivo local; não dependem de servidor ou backend.

## Próxima etapa

Backend em **C# com DDD**; preferência por **PostgreSQL** e proposta de API em **ASP.NET Core**. Primeira entrega sugerida: salvar aluno/solicitação e criar o acompanhamento inicial na mesma operação. O Logus registra a emissão e o resultado comercial; certificado e matrícula efetiva continuam externos.
