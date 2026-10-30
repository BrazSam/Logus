# Logus Educacional

Sistema interno para organizar solicitações de certificado e o acompanhamento comercial de alunos interessados em continuar os estudos.

**Status: documentação e protótipo inicial. O backend ainda não foi implementado.**

## Documentação

Leia a [visão, os fluxos e as regras do Logus](docs/01-visao-e-fluxo.md), versão 0.8. Ela reúne as decisões confirmadas e identifica as pendências.

A documentação ainda não está fechada integralmente. Restam detalhes como arredondamento da média para aprovação, limite de interesses, prevenção de solicitações duplicadas, apresentação dos avisos e algumas permissões específicas.

## Protótipo

- [Protótipo navegável](prototipos/logus-desktop.html): baixe ou clone o repositório e abra este arquivo no navegador. Para a referência desktop de 1920 × 1080, use tela cheia.
- [Interações da demonstração](prototipos/logus-demo.js): fonte das interações locais, também incorporada no HTML exportado.

O GitHub exibe o código do HTML; este repositório não está publicado no GitHub Pages.

Os 20 alunos e seus dados são fictícios. As alterações feitas na demonstração são perdidas ao atualizar a página. Não há autenticação real, banco de dados ou implementação de permissões.

## Decisões ainda não refletidas no protótipo

- Um cadastro de aluno reutilizado em várias solicitações.
- Cursos cadastrados com carregamento automático de módulos e adaptações individuais.
- Etapa **Emitido**, confirmada pela equipe de Certificados após a emissão externa.
- Correções com retorno a Em andamento, aviso para Certificados e nova conferência.
- Encerramento e reabertura de acompanhamentos comerciais com histórico.
- Retirada dos campos de assinatura/confirmação do aluno.
- Permissões reais e identificação confiável do autor de cada alteração.

Quando houver diferença entre o protótipo e uma decisão confirmada na documentação, prevalece a documentação.

## Base tecnológica definida

- Backend em C#, com DDD.
- PostgreSQL como preferência de banco.
- ASP.NET Core proposto para a API; estrutura e dependências ainda serão definidas.

O certificado e a matrícula efetiva são realizados fora do Logus. O sistema organiza as solicitações, registra a confirmação da emissão externa e acompanha o resultado comercial.
