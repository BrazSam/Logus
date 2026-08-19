# Logus

Sistema de Gestão de Rematrícula e Certificação Educacional — projeto em C#/.NET com Domain-Driven Design.

## 📋 Sobre o Projeto

O **Logus** é um sistema desenvolvido para resolver a falta de controle sobre o processo de rematrícula em instituições de ensino. Ele centraliza as informações de alunos que concluíram o curso e integra esse processo ao fluxo de solicitação de certificado.

Hoje, esse processo costuma ser feito de forma manual e informal (planilhas soltas, anotações, WhatsApp), o que faz a escola perder oportunidades de rematrícula por falta de organização. O Logus substitui isso por um fluxo estruturado, onde:

- O **professor**, junto com o aluno que está concluindo o curso, registra a solicitação de certificado e os cursos de interesse para continuidade dos estudos.
- O professor lança as **notas** do curso concluído por módulo.
- A equipe de **Certificados** visualiza apenas o necessário para emitir o certificado.
- O departamento **Comercial** recebe uma lista organizada de alunos interessados em continuar, para contato.

## 🏗️ Arquitetura

O projeto segue os princípios de **Domain-Driven Design (DDD)**, com a camada de domínio contendo:

Logus.Domain/
Logus.Domain/
├── Entities/
│   ├── Entity.cs              (classe base)
│   ├── Pessoa.cs               (classe base abstrata)
│   ├── Aluno.cs                (herda de Pessoa)
│   ├── Colaborador.cs          (herda de Pessoa)
│   ├── Curso.cs
│   ├── Modulo.cs
│   ├── SolicitacaoCertificado.cs
│   ├── ModuloConcluido.cs
│   └── Rematricula.cs
├── Enums/
│   ├── TipoPerfil.cs           (Direção, Professor, Comercial, Certificados)
│   ├── StatusSolicitacao.cs    (PendenteNotas, Completa)
│   └── StatusRematricula.cs    (NaoContatado, Contatado, Rematriculado, Recusou)
├── ValueObjects/
│   ├── Cpf.cs
│   ├── Telefone.cs
│   ├── Senha.cs
│   ├── Endereco.cs
│   └── Nota.cs
└── Exceptions/
└── DomainException.cs


## 🔧 Tecnologias

- **Linguagem:** C# (.NET 10)
- **IDE:** Visual Studio 2022
- **Padrão de arquitetura:** Domain-Driven Design (DDD)
- **Controle de versão:** Git + GitHub

## 📊 Fluxo Principal — Solicitação de Certificado

O fluxo é dividido em **3 etapas obrigatórias**, organizadas em sub-abas:

| Sub-aba | Etapa | Quem faz | O que faz |
|---|---|---|---|
| **Cadastro** | Etapa 1 | Professor + Aluno | Dados pessoais do aluno |
| **Cadastro** | Etapa 2 | Professor + Aluno | Escolha de 1 a 3 cursos de interesse |
| **Pendente** | Etapa 3 | Professor (sozinho) | Curso concluído, módulos, notas e média |
| **Completo** | — | Certificados | Visualização para emissão do certificado |

## 👥 Perfis de Acesso

| Perfil | Permissões |
|---|---|
| **Direção** | Acesso total — cadastros, matrículas, emissão de certificados e dashboard |
| **Professor** | Realiza as Etapas 1, 2 e 3 da solicitação de certificado |
| **Comercial** | Visualiza a aba Rematrículas, altera status de contato |
| **Certificados** | Visualiza apenas solicitações com status "Completa" |

## 🚀 Como Executar
```bash
# Clone o repositório
git clone https://github.com/seu-usuario/Logus.git

# Abra a solution no Visual Studio
# Ou compile via CLI:
dotnet build
```

- 📌 Status do Projeto
- ✅ Domain Layer (Enums, Value Objects e Entities)
- 🔄 Em desenvolvimento
