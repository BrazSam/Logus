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

O projeto segue os princípios de **Domain-Driven Design (DDD)**, com **Clean Architecture** e **SOLID**, organizado em camadas:

Logus.Domain → Logus.Application → Logus.Infrastructure → Logus.MAUI (Presentation)

Atualmente estão implementados: a camada **Logus.Domain** e o projeto de testes **Logus.Domain.Tests**.

Logus.Domain/
├── Common/
│   ├── IAggregateRoot.cs      (marcador de aggregate root)
│   ├── Notification.cs        (record Propriedade + Mensagem)
│   └── Result.cs              (Result — Success/Failure com notifications)
├── Entities/
│   ├── Entity.cs              (classe base — int Id)
│   ├── Pessoa.cs              (classe base abstrata)
│   ├── Aluno.cs               (herda de Pessoa)
│   ├── Colaborador.cs         (herda de Pessoa)
│   ├── Curso.cs
│   ├── Modulo.cs
│   ├── SolicitacaoCertificado.cs
│   ├── ModuloConcluido.cs
│   └── Rematricula.cs
├── Enums/
│   ├── TipoPerfil.cs          (Direção, Professor, Comercial, Certificados)
│   ├── StatusSolicitacao.cs   (PendenteNotas, Completa)
│   └── StatusRematricula.cs   (NaoContatado, Contatado, Rematriculado, Recusou)
├── Exceptions/
│   └── DomainException.cs     (falhas irrecuperáveis)
├── Services/
│   └── NormalizadoService.cs  (normalização de texto antes de validar)
└── ValueObjects/
├── Cpf.cs                 (11 dígitos + dígitos verificadores)
├── Telefone.cs            (celular — 11 dígitos)
├── Senha.cs               (mínimo 6 caracteres)
├── Endereco.cs            (logradouro, número, cidade, bairro)
└── Nota.cs


## 🧪 Testes
Projeto de testes com xUnit, cobrindo Value Objects e Entidades do domínio:
Padrão AAA (Arrange, Act, Assert)
[Fact] para casos únicos e [Theory]/[InlineData] para múltiplos cenários
Cobertura de validações: campos obrigatórios, formatos (CPF, telefone) e regras de negócio (ex.: menor de idade exige nome do responsável)


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

## 📐 Regras de Negócio
- Aluno: não possui login/senha (ferramenta interna da escola). Nome do responsável é obrigatório para menores de 18 anos.
- Colaborador: CPF é único e utilizado como login. O perfil define as permissões de acesso.
- CPF: valida 11 dígitos e os dígitos verificadores.
- Telefone: somente celular (11 dígitos).
- Senha: mínimo de 6 caracteres.
Rematrícula: gerada automaticamente na Etapa 2, com status de acompanhamento do comercial.

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
- ✅ Projeto de testes criado (Logus.Domain.Tests — xUnit)
- 🔄 Em desenvolvimento
