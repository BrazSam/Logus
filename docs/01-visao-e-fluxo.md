# Logus — visão, fluxos e regras

Versão 1.0 · 30/10/2026 · Base funcional para iniciar o backend.

Este documento consolida as decisões do projeto. O [protótipo navegável](../prototipos/logus-desktop.html) demonstra os fluxos com dados fictícios. Onde uma escolha ainda é proposta, isso está indicado. As referências impressas e visuais servem de contexto; as decisões do usuário prevalecem.

## 1. Objetivo e limites

Sistema interno da escola para organizar solicitações de certificado e acompanhar oportunidades de continuidade dos estudos. Funcionários acessam pelo navegador. O aluno preenche ou confirma os dados com o professor, sem conta própria.

O Logus **não gera o certificado em PDF**: a emissão acontece em outro sistema. Também **não efetiva matrículas**: o Comercial registra o resultado de um atendimento realizado fora dele.

O primeiro desenvolvimento será em C#, com DDD. PostgreSQL é a preferência para banco; ASP.NET Core é a proposta para a API do backend. Interface definitiva, hospedagem e autenticação serão escolhidas na implementação. Software gratuito não garante hospedagem gratuita para qualquer volume de uso.

## 2. Perfis

| Perfil | Responsabilidades confirmadas |
| --- | --- |
| Professor | Consulta e altera solicitações de todos os alunos, inclusive as iniciadas por outro professor. Conduz o preenchimento, informa curso, módulos, médias e frequência, confere e encaminha. Pode corrigir após emissão. |
| Certificados | Usa as solicitações prontas para emitir externamente e confirma essa emissão no Logus. Recebe avisos de correção. |
| Comercial | Registra contato, observações e resultado do acompanhamento. Pode reabrir um atendimento recusado e iniciar nova oportunidade. |
| Direção | Acesso geral. Administra colaboradores, cursos e módulos padrão; pode corrigir solicitações e reabrir acompanhamentos. |
| Aluno | Fornece os dados pessoais e de 1 a 3 interesses, na etapa conduzida pelo professor. Sem login próprio. |

Na demonstração, o seletor **Funcionário da demonstração** permite experimentar esses perfis. Professor e Direção editam dados acadêmicos; Certificados e Direção confirmam emissão; Comercial e Direção alteram atendimentos; somente Direção administra cadastros. As funções executoras também verificam o perfil simulado.

**Detalhes propostos para validar:** Professor consulta o comercial sem editar; Certificados consulta as três filas sem editar notas; Professor, Certificados e Direção escrevem na conversa da solicitação. Essas escolhas de apresentação não substituem a futura matriz de autorização do backend.

## 3. Fluxo principal

1. O professor abre **Solicitações → + Solicitação de certificado**.
2. O aluno informa ou confirma os dados pessoais e escolhe de 1 a 3 cursos de interesse.
3. Ao salvar, o sistema cria uma solicitação **Em andamento** e um acompanhamento **Não contatado** em Rematrículas, ainda sem médias.
4. O professor seleciona o curso concluído. Os módulos cadastrados são carregados automaticamente e podem ser adaptados àquele aluno.
5. O professor preenche médias, frequência e data da conferência; pode salvar parcialmente e continuar depois.
6. Com os requisitos atendidos, salva, marca a conferência e usa **Confirmar e encaminhar**. A solicitação passa para **Solicitações prontas**.
7. Certificados emite no sistema externo e confirma a emissão no Logus. A solicitação passa para **Emitido**.
8. O Comercial trabalha em paralelo desde o primeiro salvamento. Encaminhar ou emitir um certificado não cria outro atendimento nem altera sua situação comercial.

Solicitação de certificado e acompanhamento comercial possuem andamentos independentes. Um aluno pode estar com notas pendentes e já ter sido contatado.

## 4. Cadastro do aluno e solicitações anteriores

O CPF identifica um cadastro reutilizado em várias solicitações. Cada solicitação conserva número, curso, módulos, interesses, etapa, conversa e registros de emissão próprios.

Ao informar um CPF com solicitações anteriores, aparece **Este aluno já tem solicitações**, com os registros: número, curso, data e etapa.

- **Abrir existente:** abre a solicitação escolhida, sem criar outra solicitação ou acompanhamento.
- **Criar outra solicitação:** reutiliza o aluno e permite criar uma solicitação independente, com seus interesses e novo acompanhamento.
- **Cancelar:** fecha o aviso sem criar registros.

O aviso evita repetição acidental, mas não impede uma segunda solicitação legítima, inclusive para outro curso. Duplo clique no salvamento não deve criar dois registros.

**Comportamento adotado na demonstração para avaliar:** dados pessoais são compartilhados. Ao corrigir telefone, endereço ou outro dado cadastral, solicitações e acompanhamentos do mesmo aluno passam a consultar o dado atualizado. A alteração fica nos históricos afetados; solicitações prontas ou emitidas voltam para conferência. Interesses, notas e situação comercial pertencem a cada solicitação/oportunidade. Não há unificação automática de dois cadastros com CPFs diferentes.

## 5. Frente — dados do aluno

A referência é a primeira página do formulário impresso SiDY. As obrigatoriedades abaixo são aplicadas no rascunho, sujeitas à revisão com a escola.

| Campo | Regra no protótipo |
| --- | --- |
| Nome completo | Obrigatório, até 120 caracteres. |
| CPF | Obrigatório; normalizado para 11 dígitos. Validação real dos dígitos verificadores fica para o backend. |
| Nascimento | Obrigatório; não pode ser uma data futura. |
| Telefone | Obrigatório, exibido completo para a equipe; fictício na demonstração. Validação detalhada de DDD/número fica para a implementação. |
| Responsável | Opcional, para menores; sem regra automática de idade definida. |
| Endereço | Obrigatório; rua, número e complemento no mesmo campo. |
| Bairro e cidade | Obrigatórios. |
| Data da solicitação | Obrigatória; sugerida como a data atual. |
| Certificações em interesse | **Mínimo 1 e máximo 3**, na criação e edição. |
| Descrição de Outros | Obrigatória quando Outros estiver marcado; conta como um interesse. |

Opções do impresso: **Design Digital**, **Assistente Administrativo**, **Power BI**, **Desenvolvedor de Games**, **Desenho Mecânico e Arquitetônico**, **Edição de Vídeo** e **Outros**. As descrições são resumidas na tela; o texto de Outros é preservado na solicitação e no acompanhamento.

Não haverá assinatura, nome digitado para simular assinatura ou marcação de confirmação do aluno. A conferência do professor permanece.

Salvar a frente completa cria imediatamente o acompanhamento comercial, sem exigir curso concluído, notas, frequência ou conferência acadêmica. Cancelar a criação não grava nada; cancelar uma edição conserva os valores já salvos.

## 6. Verso — curso, módulos e conferência

O professor escolhe um curso cadastrado pela Direção. Seus módulos padrão são copiados para a solicitação. Pode remover módulos não cursados, acrescentar extras e ajustar nomes para representar o curso realmente feito pelo aluno.

Essa adaptação **não modifica o catálogo**. Alterações posteriores no catálogo não substituem automaticamente os módulos e médias de solicitações existentes. Os cursos e módulos do protótipo são exemplos, não o catálogo oficial da escola.

O impresso oferece 13 linhas, mas isso não foi definido como limite de negócio. O rascunho permite adicionar e remover linhas. Selecionar outro curso pede confirmação antes de substituir os módulos e médias que estão na tela.

| Campo | Salvamento parcial | Encaminhamento |
| --- | --- | --- |
| Curso concluído | Pode ficar pendente. | Curso cadastrado selecionado. |
| Professor responsável | Colaborador que iniciou a solicitação. | Diferente de quem fez a última edição. |
| Módulos | Pode existir módulo sem média. | Pelo menos um módulo; nome e média em todos os módulos informados. |
| Médias dos módulos | Valores de 0 a 10. No protótipo, até duas casas decimais. | Todas preenchidas e válidas. |
| Frequência | Pode ficar pendente; quando informada, entre 0 e 100%. | Pelo menos **75%**, sem arredondamento. |
| Início e término | Complementares; término não pode anteceder início quando ambos existem. | Permanecem complementares. |
| Data da conferência | Sugerida ao abrir o detalhe. | Preenchida e salva. |
| Conferência final | Desnecessária para salvar rascunho. | Marcação explícita, seguida da confirmação. |

Uma linha totalmente vazia não representa módulo cursado e não entra na média. Um módulo com nome e média pendente impede encaminhar; não é convertido em zero nem ignorado para aprovar.

Alterar campos acadêmicos desmarca a conferência. É necessário salvar antes de encaminhar. **Salvar dados acadêmicos** não equivale a **Confirmar e encaminhar**.

### Cálculo e arredondamento confirmados

1. Calcular a **média aritmética simples** das médias dos módulos cursados: soma dividida pela quantidade de módulos, todos com o mesmo peso.
2. Usar o resultado sem arredondamento intermediário.
3. Se a parte decimal for **igual ou maior que 0,90**, elevar para o próximo inteiro. Nos demais casos, conservar o valor calculado. Um inteiro permanece igual.
4. A média final precisa ser **maior ou igual a 7**, junto com frequência **maior ou igual a 75%** e os demais dados completos.

| Média calculada | Média final | Atende ao mínimo da média? |
| --- | --- | --- |
| 6 | 6 | Não |
| 6,1 | 6,1 | Não |
| 6,8 | 6,8 | Não |
| 6,89 | 6,89 | Não |
| 6,90 | 7 | Sim |
| 6,95 | 7 | Sim |
| 7 | 7 | Sim |
| 7,9 | 8 | Sim |
| 9,9 | 10 | Sim |

Exemplo: médias 6,90 e 7,00 produzem média calculada 6,95 e média final 7. Já 6,89 não pode virar 6,9 antes de aplicar a regra. Frequência 74,9% continua abaixo do mínimo, mesmo com média suficiente.

A tela mostra média calculada e final. A apresentação pode limitar casas decimais para leitura; a aprovação deve usar o cálculo sem esse arredondamento de apresentação.

O impresso cita nota e frequência no rodapé, mas não traz campo próprio de frequência; esse campo é uma adaptação digital. Este documento registra a regra operacional decidida para o Logus, sem validar a referência normativa citada no papel.

## 7. Etapas, emissão e correções

| Etapa | Significado | Próxima ação |
| --- | --- | --- |
| Em andamento | Faltam preenchimento, ajustes ou conferência. | Professor ou Direção completa e encaminha. |
| Solicitações prontas | Dados salvos, critérios atendidos e conferência confirmada. | Certificados ou Direção confirma emissão externa. |
| Emitido | Emissão externa confirmada no Logus. | Consulta; pode voltar para revisão se houver correção. |

**Solicitações prontas não significa certificado emitido.** **Confirmar emissão externa** pede confirmação, muda a etapa e registra colaborador, data e hora. Não gera arquivo nem interfere no sistema externo.

Ao salvar uma correção em dados pessoais ou acadêmicos, curso, módulos, médias ou interesses de uma solicitação pronta ou emitida:

1. Registrar automaticamente autor, data/hora, valores anteriores e novos.
2. Retornar para **Em andamento**.
3. Avisar Certificados da correção e da necessidade de nova conferência.
4. Exigir nova conferência e encaminhamento pelo professor.
5. Preservar histórico e todas as confirmações de emissão anteriores.

Na demonstração, os avisos ficam em **Avisos de correção para Certificados**, na tela de Solicitações desse perfil e da Direção. Abrir um aviso marca-o como lido naquela sessão. Esse formato é uma proposta; não envolve e-mail ou WhatsApp.

Depois da nova conferência, Certificados pode confirmar outra emissão externa. As anteriores permanecem em **Emissões registradas**. Corrigir o Logus não corrige nem cancela automaticamente o certificado no outro sistema.

Uma simples mensagem na conversa não altera a etapa nem exige nova conferência.

## 8. Conversa e histórico

Ao clicar no aluno, abre-se o detalhe com preenchimento à esquerda e **Conversa e histórico** à direita. Mensagens escritas e eventos automáticos aparecem com seus tipos identificados.

Alterações mostram o colaborador que realmente executou a ação, perfil, data/hora e valores anteriores e novos. O professor responsável original não deve ser usado como autor da edição de outro funcionário.

Exemplo: “Ana Pereira alterou módulos e médias de ‘Excel: 7’ para ‘Excel: 8’”. Conferência, emissão, retorno para revisão e criação também entram no histórico.

No backend, autor e horário devem vir da sessão autenticada e do servidor. O navegador do protótipo apenas simula esses dados. O histórico não tem botões de edição/exclusão.

Ainda a definir: obrigatoriedade de motivo de correção; política de edição/exclusão de mensagens; limites e retenção. No rascunho, o motivo pode ser escrito na conversa e cada mensagem aceita até 500 caracteres.

## 9. Rematrículas e acompanhamento comercial

Um acompanhamento representa uma oportunidade de continuidade, não uma matrícula efetiva. Ao salvar a frente, recebe identificação, contato, interesses, vínculo com a solicitação e situação **Não contatado**.

A tela sempre abre **sem aluno selecionado**. O detalhe aparece após clique explícito. Busca e filtros não selecionam automaticamente o primeiro resultado. Ao sair e voltar, a seleção é limpa; se um filtro esconder o aluno selecionado, seu detalhe também é ocultado.

No detalhe, **interesses** aparecem em etiquetas destacadas e **telefone completo** em bloco próprio, ampliado. Nome e situação vêm acima; professor, origem e data ficam como informações secundárias. Textos longos e Outros permanecem legíveis.

| Situação | Cor | Comportamento |
| --- | --- | --- |
| Não contatado | Amarelo/ocre | Atendimento ainda não iniciado. |
| Contatado | Azul | Houve contato; atendimento pode continuar. |
| Rematriculado | Verde | Comercial registrou a matrícula feita fora do Logus. |
| Recusou | Vermelho | Encerra automaticamente aquele atendimento. |

Filtros **Todos**, **Não contatado**, **Contatado**, **Rematriculados** e **Recusou** são explícitos, coloridos e têm contadores. Cor sempre vem acompanhada de texto. Rematriculados corresponde à situação individual Rematriculado.

Busca por nome, CPF ou número da solicitação combina com o filtro. Contadores refletem o resultado da busca por situação. Sem resultados, mostrar mensagem de lista vazia.

Ordenação: **mais recentes primeiro, pela criação do acompanhamento**. Alterar situação ou observação não muda a data e não sobe o item. Empates usam o identificador como critério estável.

### Encerramento e nova oportunidade

Salvar **Recusou** encerra o atendimento, que permanece para consulta. Os campos ficam bloqueados até a reabertura. **Comercial ou Direção** usa **Reabrir como Contatado**: volta para Contatado, preservando recusa, observação e histórico da reabertura.

**+ Novo acompanhamento** cria outra oportunidade para o mesmo aluno, com novos interesses e situação Não contatado, sem substituir a anterior. Pode representar um terceiro curso ou tentativa futura. Também exige de 1 a 3 interesses. Uma oportunidade manual não gera outra solicitação de certificado.

Editar interesses da solicitação atualiza o acompanhamento vinculado, preservando situação e observação. Oportunidades independentes conservam seus interesses. Atualizar contato do aluno não apaga o histórico comercial.

## 10. Telas e identidade visual

| Tela | Conteúdo |
| --- | --- |
| Acesso | Usuário e senha antes de entrar; sem item Acesso no menu da área de trabalho. |
| Visão geral | Primeira tela após entrar: pendências e indicadores pertinentes ao perfil. |
| Solicitações | Busca; três filas lado a lado no desktop; botão de nova solicitação. |
| Frente do aluno | Dados pessoais e interesses, na criação ou edição. |
| Detalhe | Dados pessoais/acadêmicos, conferência, emissão, conversa e histórico. |
| Rematrículas | Busca, filtros, lista por recência e detalhe após seleção. |
| Cadastros | Cursos/módulos padrão e colaboradores; acesso da Direção. |

Canto superior direito com nome, perfil e espaço para foto. A foto do protótipo é uma prévia local por colaborador. **Sair** fica no rodapé da navegação, visível no desktop sem rolar toda a lista; leva ao acesso. A próxima entrada abre Visão geral.

Referência **1920 × 1080**, com adaptação para janelas menores. Filas e painéis são empilhados em larguras reduzidas. Há 20 alunos fictícios, incluindo Charlie Kirk e Jeffrey Epstein, e Samuel Braz como professor inicial.

Paleta CAEZ/SiDY: fundo #F4F5F6, painéis brancos, azul #004794, faixas azul-escuro #1F3864 e verde-lima #AAD04F nas ações principais. Indicadores #31708F, #3C763D e #8A6D3B. Aparência clara fixa.

## 11. Vocabulário para o domínio

| Termo | Significado |
| --- | --- |
| Aluno | Cadastro reutilizável por CPF; não é usuário do sistema. |
| Colaborador | Funcionário que acessa com um perfil. |
| Solicitação de certificado | Pedido com dados pessoais, curso, médias, conferência e histórico para emissão externa. |
| Curso cadastrado | Modelo do catálogo com módulos padrão. |
| Módulo cursado | Módulo daquela solicitação, com sua média. |
| Certificação em interesse | Preferência de continuidade; diferente do curso cujo certificado é solicitado. |
| Acompanhamento comercial | Oportunidade com interesses, situação, observações e histórico próprios. |
| Confirmação de emissão | Registro de que o certificado foi emitido externamente. |
| Conferência | Revisão explícita do professor antes de encaminhar. |

Os termos orientam o DDD; ainda não fixam classes, agregados ou projetos. A modelagem deve separar solicitação e acompanhamento, com ciclos independentes, e proteger no backend as regras de média, interesses e transições.

## 12. Verificação e próximos passos

Os [testes de fluxo](../tests/prototipo.cjs) exercitam o HTML no navegador: médias e limites, frequência, encaminhamento/emissão/correção, autoria de outro professor, duplicidade por CPF, criação sem notas, catálogo e módulos personalizados, cadastros, recusa/reabertura/nova oportunidade, busca/filtros, acesso/saída e ausência de rolagem horizontal nas telas principais em cinco larguras.

Roteiro manual complementar: preencher Outros; salvar módulo sem média e continuar depois; consultar emissões após revisão; experimentar os cinco funcionários de exemplo.

**Demonstração local:** sem autenticação real, autorização segura, banco, sincronização entre pessoas, auditoria confiável ou notificações externas. Atualizar a página restaura os exemplos. Trocar funcionário é um recurso de teste, não uma tela prevista para produção.

Antes da produção: validação real de CPF/telefone; precisão e armazenamento numérico; autenticação e matriz final de permissões; inativação de cursos/colaboradores; recuperação de senha; política de mensagens e retenção; proteção de dados, backups, hospedagem e concorrência entre edições. Aviso ao sair com alterações não salvas também fica para refinamento.

As decisões funcionais centrais estão consolidadas e demonstradas. Detalhes técnicos e operacionais podem ser tratados em tarefas pequenas durante a construção; não é necessário antecipar toda a arquitetura para começar.

Primeira entrega sugerida do backend: criar aluno/solicitação com 1 a 3 interesses e gerar o acompanhamento inicial na mesma operação, com testes de domínio. Depois, preenchimento/conferência e emissão, seguindo esta documentação.
