# Como vamos trabalhar no GitHub

Git guarda o histórico das alterações; GitHub hospeda esse histórico e permite revisar o trabalho. Um fluxo comum para projetos pequenos é o [GitHub Flow](https://docs.github.com/en/get-started/using-github/github-flow): branch, commits, pull request, revisão e merge.

## O caminho de uma alteração

1. **Main:** versão principal do projeto, que deve estar coerente e funcionando.
2. **Branch:** linha separada para uma tarefa. Aqui usamos `codex/alinha-documentacao-prototipo` para atualizar documentação e rascunho juntos.
3. **Commit:** ponto do histórico com uma mudança e uma mensagem clara. Por exemplo: `feat: alinhar protótipo aos fluxos de certificado e comercial`.
4. **Push:** envio dos commits ao GitHub. Eles ficam na branch da tarefa, ainda separados da main.
5. **Pull request (PR):** pedido para juntar a branch à main. Apresenta o problema resolvido, as mudanças e a validação. **Abrir PR não publica um site nem coloca um backend em produção.**
6. **Revisão:** conferir o resumo e a aba **Files changed**; abrir o protótipo localmente e testar os fluxos. Ajustes entram em novos commits no mesmo PR.
7. **Merge:** incorporar o trabalho aprovado à main. Depois disso, pode-se excluir a branch concluída e atualizar a cópia local.

Mesmo trabalhando sozinho, o PR ajuda a revisar antes de juntar tudo. Não é obrigatório usar esse fluxo para cada pequeno ajuste; aqui ele serve também como exercício para o desenvolvimento do backend.

## Nesta atualização do Logus

Os arquivos de base já estavam na main. A nova atualização será entregue por branch e PR, com dois grupos de commits:

- Protótipo e testes: três filas, cálculo, interesses, múltiplas solicitações, catálogo, revisão e fluxo comercial.
- Documentação: regras consolidadas, instruções de uso e este guia.

O PR deve ficar aberto para você conhecer a revisão. O passo seguinte é abrir **Files changed** e, após conferir, usar **Merge pull request → Confirm merge** se o GitHub disponibilizar esse método. Outras opções de merge podem aparecer conforme a configuração do repositório.

Os testes desta entrega foram executados localmente. Ter arquivos de teste no repositório não faz o GitHub executá-los sozinho: para isso, adicionaremos uma automação de GitHub Actions em uma tarefa própria.

## Como seria pelo terminal no trabalho diário

Exemplo para uma próxima tarefa, quando houver uma cópia local com Git configurado:

```sh
git switch main
git pull --ff-only
git switch -c codex/nome-da-tarefa
# Editar os arquivos e executar as verificações.
git status
git diff
git add docs/01-visao-e-fluxo.md
git commit -m "docs: esclarecer regra de conferência"
git push -u origin codex/nome-da-tarefa
```

Depois, abrir o PR no GitHub. O exemplo adiciona um arquivo explícito para facilitar conferir o que será enviado. Evite incluir senhas, arquivos de ambiente, dados reais de alunos ou saídas temporárias.

Nesta entrega, o envio usa a integração do GitHub disponível no chat, com o mesmo resultado de branch, commits e PR. O backend continua para a próxima etapa.
