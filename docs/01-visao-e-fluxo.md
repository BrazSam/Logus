# Logus — visão, fluxos e especificação do protótipo

Versão 0.10 · 30/10/2026 · Documento em construção.

Este arquivo registra as decisões conversadas. Sugestões e pontos em aberto estão identificados para não virarem requisitos por acidente.

Referência navegável: [protótipo desktop](../prototipos/logus-desktop.html). As seções de detalhamento abaixo descrevem o protótipo atual; comportamentos da demonstração não são automaticamente decisões para o sistema definitivo.

As decisões da versão 0.6 atualizam o escopo do sistema. O protótipo ainda precisa incorporar o cadastro com múltiplas solicitações por aluno, catálogo de cursos, etapa Emitido, novas regras de correção e reabertura comercial e retirada dos campos de confirmação do aluno. Onde houver diferença, a regra confirmada prevalece sobre a demonstração.

## Objetivo

Organizar solicitações de certificado e o acompanhamento de alunos interessados em continuar os estudos. O sistema é interno à escola e será acessado pelo navegador.

## Decisões confirmadas

- Backend em C#, com DDD.
- O Logus organiza solicitações; o certificado é emitido em outro sistema.
- A matrícula efetiva acontece fora do Logus. O comercial registra o resultado do acompanhamento.
- O aluno deve escolher **no mínimo 1 e no máximo 3 cursos de interesse**, mesmo sem intenção imediata de matrícula. Isso ajuda o professor a conhecer suas preferências e, eventualmente, oferecer uma aula teste.
- Após a etapa do aluno, o professor preenche os dados acadêmicos, confere as informações e confirma o envio ao departamento de certificados.
- Ao salvar a etapa inicial com os dados do aluno e pelo menos um curso de interesse, o sistema cria automaticamente o acompanhamento em Rematrículas. Isso não depende de notas nem da confirmação do professor.
- O professor pode corrigir notas mesmo depois da emissão externa do certificado.
- A solicitação deve ter uma área de conversa e histórico que permita identificar alterações e seus autores.
- PostgreSQL é a preferência atual para o banco. Hospedagem será escolhida depois.
- Os professores podem visualizar e alterar as solicitações de todos os alunos, inclusive as iniciadas por outro professor. Cada alteração deve gerar mensagem automática no histórico, identificando o colaborador, a data e hora e os valores anteriores e novos. Isso não concede automaticamente permissões administrativas ou de emissão dos demais perfis.
- Um aluno pode ter várias solicitações, reutilizando o mesmo cadastro identificado pelo CPF.
- Os cursos serão previamente cadastrados com seus módulos. Ao selecionar o curso concluído, os módulos serão preenchidos automaticamente; o professor pode adicionar ou remover módulos naquela solicitação.
- A área de solicitações terá **Em andamento**, **Solicitações prontas** e **Emitido**. A equipe de Certificados confirma no Logus a emissão feita externamente.
- Correções posteriores devem ficar no histórico, avisar a equipe de Certificados e exigir nova conferência.
- Marcar **Recusou** encerra automaticamente aquele acompanhamento comercial, que pode ser reaberto depois.
- O aluno pode ter um novo acompanhamento para outra oportunidade de curso, sem substituir o atendimento anterior.
- Não será exigida assinatura do aluno, nome digitado para simular assinatura ou marcação de confirmação do aluno. A conferência do professor continua fazendo parte do encaminhamento.
- A média final será a **média aritmética simples** das médias dos módulos cursados: soma das médias dividida pela quantidade de módulos, todos com o mesmo peso. Linhas vazias, de módulos não cursados, não entram na conta.
- Depois de calcular a média aritmética, **somente o final ,9 sobe para o próximo inteiro**. Exemplos: 6,9 → 7; 7,9 → 8; 9,9 → 10. Uma média 6,1 continua 6,1; 6,8 continua 6,8; um inteiro permanece igual. A interpretação anterior de arredondar qualquer decimal para cima estava errada e foi corrigida. A aprovação usa a média final e ainda exige frequência mínima de 75%.
- Direção administra os cadastros de colaboradores, cursos e módulos padrão e pode corrigir qualquer solicitação.
- Correções após encaminhamento ou emissão devolvem a solicitação para **Em andamento**, preservam o histórico e a emissão anterior, avisam Certificados e exigem nova conferência.
- Comercial ou Direção pode reabrir um acompanhamento **Recusou**; ele retorna a **Contatado**, preservando o histórico da recusa e da reabertura.

## Pessoas envolvidas

| Perfil | Participação |
| --- | --- |
| Professor | Visualiza e altera solicitações de todos os alunos, inclusive de outros professores. Conduz o preenchimento, informa curso e médias, confere e encaminha. Todas as alterações identificam automaticamente quem alterou e o que mudou no histórico. |
| Certificados | Consulta as solicitações prontas, usa os dados para emitir o certificado fora do Logus e confirma a emissão, movendo o registro para Emitido. |
| Comercial | Acompanha os interesses, registra contatos e informa o resultado. Pode reabrir um acompanhamento Recusou, que volta para Contatado. |
| Direção | Acesso geral; administra colaboradores, cursos e módulos padrão, pode corrigir qualquer solicitação e reabrir acompanhamentos comerciais. |
| Aluno | Informa seus dados e interesses na etapa conduzida pelo professor; sem conta própria, conforme a documentação anterior. |

## Fluxo principal

1. O professor abre uma solicitação e conduz o preenchimento com o aluno.
2. O aluno informa ou confirma seus dados pessoais e escolhe de 1 a 3 cursos de interesse.
3. Ao salvar essa etapa, a solicitação entra em **Em andamento**, aguardando o professor, e o sistema adiciona automaticamente em **Rematrículas** os dados de identificação e contato do aluno e seus cursos de interesse.
4. O professor informa o curso concluído, os módulos cursados, as médias e a frequência. Início e término do curso ficam como informações complementares; a data da conferência faz parte da confirmação.
5. O professor confere os dados pessoais e acadêmicos antes de confirmar.
6. A solicitação sai de **Em andamento** e entra em **Solicitações prontas**, para o departamento de certificados.
7. A equipe emite o certificado no sistema externo e confirma essa ação no Logus. A solicitação passa de **Solicitações prontas** para **Emitido**.
8. O comercial registra contatos, observações e o resultado do acompanhamento no Logus, sem efetivar matrícula nele. Esse trabalho pode começar desde o passo 3, em paralelo ao preenchimento das notas pelo professor.

O acompanhamento comercial e a solicitação de certificado têm andamentos independentes: uma solicitação pode continuar **Em andamento** enquanto o comercial já contata o aluno. Encaminhar para certificados não cria um segundo acompanhamento comercial.

## Conversa e histórico — proposta para validar

Uma mesma área pode mostrar dois tipos de registro:

- **Mensagem:** comentário escrito por um colaborador, com autor, data e hora.
- **Alteração automática:** registro gerado pelo sistema ao salvar uma mudança, com autor, data e hora, campo alterado e valores anterior e novo.

Exemplo fictício: “Samuel Braz alterou a média de Excel de 7,0 para 8,0 em 30/10/2026 às 14:32”.

Uma mensagem sozinha não comprova que um campo foi alterado; o registro automático é o que permite rastrear a mudança. A conversa complementa esse registro com contexto.

Foi confirmado que correções posteriores devem gerar histórico, aviso à equipe de Certificados e nova conferência. Ainda precisamos definir quem pode escrever mensagens, se o motivo de uma correção será obrigatório e como o aviso será apresentado. Alterar o Logus não altera o certificado já emitido no outro sistema.

## Referências para os primeiros rascunhos

- CAEZ: menu lateral, cabeçalho com identificação, fundo claro, blocos simples e tabelas de trabalho.
- Primeira imagem enviada: conversa e histórico ao lado dos dados da solicitação.
- Telas iniciais para discutir: acesso, lista de solicitações, detalhe com conferência e histórico e acompanhamento comercial.
- Os rascunhos usam dados fictícios. Botões, rótulos e estados são propostas visuais, não decisões finais.
- Referência de visualização desktop: **1920 × 1080**. O rascunho também se adapta a janelas menores.
- Nomes de demonstração escolhidos pelo usuário: professor **Samuel Braz**; alunos **Charlie Kirk** e **Jeffrey Epstein**. Os registros exibidos são fictícios.
- Paleta visual aplicada a partir das referências CAEZ e SiDY: aparência clara fixa, fundo **#F4F5F6**, painéis brancos, azul CAEZ **#004794**, faixas azul-escuro do formulário **#1F3864** e verde-lima **#AAD04F** nas ações principais. Indicadores usam azul **#31708F**, verde **#3C763D** e ocre **#8A6D3B**, como no painel de referência.

### Organização proposta da tela de solicitações

O protótipo atual mostra duas filas. A organização definida para a próxima versão passa a ter três áreas lado a lado:

- **Esquerda — Em andamento:** solicitações preenchidas com os alunos, ainda aguardando notas, ajustes de informações ou conferência pelo professor.
- **Ao lado — Solicitações prontas:** solicitações já concluídas e confirmadas pelos professores, disponíveis para a equipe emitir o certificado externamente.
- **Nova área — Emitido:** solicitações cuja emissão externa já foi confirmada pela equipe de Certificados. Fica ao lado de Solicitações prontas.

Ao clicar no nome do aluno em qualquer fila, abre-se o detalhe da solicitação com dados, notas, conversa e mensagens automáticas do histórico. A conversa não aparece na visão geral das filas.

As abas **Solicitações** e **Rematrículas** têm uma barra de pesquisa por nome do aluno, CPF ou número da solicitação. A filtragem acontece ao digitar, aceita trechos do nome e CPF com ou sem pontuação, e ignora maiúsculas e acentos. Em Solicitações, a pesquisa filtra as duas filas simultaneamente. Cada aba mantém sua própria busca; ao apagar o texto, todos os registros voltam a aparecer. Resultados vazios têm mensagem explícita. A busca de Rematrículas não apaga observações comerciais ainda em edição.

Ao confirmar uma solicitação, o professor a encaminha para Solicitações prontas. Após a emissão externa, a equipe de Certificados usa a ação de confirmação para movê-la para Emitido. Professores podem visualizar solicitações de todos os alunos; as demais permissões por ação ainda serão detalhadas.

### Acesso e navegação

- A tela de acesso é separada da área de trabalho. O colaborador informa usuário e senha para entrar.
- Após entrar, a primeira tela é **Visão geral (dashboard)**.
- No canto superior direito, o cabeçalho mostra o nome do colaborador conectado e um espaço para sua foto de perfil.
- O menu lateral contém **Visão geral**, **Solicitações** e **Rematrículas**, respeitando as permissões que ainda serão detalhadas. Não há item “Tela de acesso”.
- O botão **Sair** fica na parte inferior do menu lateral, visível sem precisar rolar a página, e retorna à tela de acesso. O menu acompanha a área visível da janela mesmo em páginas longas. No sistema real, também encerrará a sessão.
- O dashboard inicial do rascunho propõe contagens de solicitações em andamento, solicitações prontas e alunos não contatados, além de um atalho para solicitações aguardando o professor. Métricas e visibilidade por perfil ainda serão refinadas.
- No rascunho, login, saída e escolha de foto são apenas simulações locais. A prévia abre na Visão geral para demonstrar a área de trabalho; **Sair → Entrar** permite testar o fluxo de acesso. Nenhuma senha é armazenada, e a foto escolhida não é enviada a um servidor.

### Nova solicitação: etapa do aluno

- A aba Solicitações tem o botão **+ Solicitação de certificado**, que abre o formulário conduzido pelo professor com o aluno.
- O formulário reúne nome completo, CPF, nascimento, telefone, responsável opcional, endereço, cidade, bairro e certificações em interesse. A etapa inicial não pede notas.
- Ao salvar com pelo menos um interesse, cria-se uma solicitação **Em andamento**, aguardando o professor, e seu acompanhamento em **Rematrículas** com identificação, contato e interesses.
- O detalhe da solicitação permite voltar ao formulário por **Editar dados do aluno**. Essas alterações atualizam o mesmo acompanhamento comercial, sem criar outro.
- O rascunho permite experimentar o fluxo de criação, notas, encaminhamento e acompanhamento comercial. Os dados ficam apenas na memória da página e são perdidos ao atualizar; não há banco de dados nem autenticação real.
- Campos obrigatórios e validações do rascunho são uma proposta para avaliação. Ainda vamos detalhar as regras reais, inclusive validação de CPF e critérios de conclusão.

## Pontos em aberto, sem bloquear este começo

- Validações definitivas dos campos e apresentação dos avisos. Média aritmética com elevação apenas do final ,9 e limite de 1 a 3 interesses estão confirmados. O tratamento de resultados com mais de uma casa decimal, como 6,89 ou 6,95, ainda precisa ser especificado, sem presumir arredondamento intermediário.
- Apresentação dos avisos de correção para Certificados e detalhes de eventual reemissão externa. O retorno para Em andamento, a nova conferência e a preservação do histórico já estão definidos.
- Permissões específicas de conversa, exclusão/inativação de registros e outros comandos ainda não descritos. Administração dos cadastros por Direção, edição de todas as solicitações pelos professores e reabertura comercial por Comercial ou Direção já estão definidas.
- Critério para distinguir nova solicitação legítima de uma repetição acidental do mesmo curso e como iniciar outro acompanhamento sem uma nova solicitação de certificado.

## Tecnologia, no nível necessário agora

ASP.NET Core é a ferramenta do ecossistema .NET que podemos usar para receber pedidos do navegador, aplicar as regras em C# e acessar o banco. Também oferece recursos para páginas web, mas pode servir apenas ao backend.

PostgreSQL é gratuito e de código aberto. Os usuários acessam o Logus pelo navegador; o backend conversa com o banco. A licença gratuita do banco não significa hospedagem gratuita garantida. Não precisamos escolher um provedor neste momento.

DDD orientará a organização das regras do negócio. A estrutura do código será discutida quando detalharmos o primeiro fluxo.

Referências: [ASP.NET Core para backend](https://dotnet.microsoft.com/en-us/learn/back-end-web-dev) · [Licença do PostgreSQL](https://www.postgresql.org/about/licence/).

## Adaptação ao formulário impresso SiDY — 30/10/2026

Fonte: [solicitação impressa, frente e verso](E:/Logus/Novo/documentos/Solicitacao_de_Certificado_SiDY.pdf).

**Frente — aluno:** o rascunho segue os campos do papel: nome, nascimento, CPF, responsável para menores, telefone, endereço, cidade e bairro. Endereço fica em um campo completo, incluindo número e complemento.

As certificações em interesse agora são as do documento, com descrições resumidas: **Design Digital**, **Assistente Administrativo**, **Power BI**, **Desenvolvedor de Games**, **Desenho Mecânico e Arquitetônico**, **Edição de Vídeo** e **Outros**, com descrição livre. Devem ser selecionadas de 1 a 3 opções; Outros conta como uma opção e exige descrição. Ao editar, o texto de Outros é preservado e atualizado no acompanhamento comercial.

O papel tem data e assinatura do aluno, mas o usuário definiu que **a assinatura não fará parte do Logus**. A proposta anterior de nome para confirmação e marcação de conferência do aluno foi retirada dos requisitos. A data da solicitação permanece; a conferência do professor é mantida. Os campos antigos ainda aparecem no protótipo e precisam ser removidos em sua próxima atualização.

**Verso — professor:** nome do curso, professor identificado como Samuel Braz na demonstração, até **13 módulos com suas médias**, média final e data da conferência do professor. Linhas vazias são opcionais; módulos iniciados precisam de média antes do encaminhamento. A conversa e o histórico permanecem ao lado. É possível salvar o preenchimento parcial e continuar depois.

O impresso informa **nota maior ou igual a 7 e frequência mínima de 75%**. O rascunho acrescenta um campo de frequência, para permitir conferir esse critério antes do encaminhamento. Essa informação é uma adaptação digital: o papel cita a exigência no rodapé, mas não tem campo próprio para preencher a frequência. Não foi feita verificação da referência normativa citada no papel.

A média aritmética simples foi confirmada pelo usuário: somam-se as médias dos módulos cursados e divide-se pela quantidade desses módulos, sem pesos diferentes. **Apenas o final ,9 é elevado ao próximo inteiro.** Exemplo: médias 8, 7 e 9 resultam em média final 8; média calculada 6,9 resulta em média final 7; média calculada 6,1 não sobe para 7. Um módulo cursado ainda sem média impede o encaminhamento; não é tratado como zero nem ignorado para concluir a solicitação. O protótipo antigo ainda arredonda a duas casas e precisa ser atualizado para a regra confirmada.

| Média calculada | Média final para aprovação |
| --- | --- |
| 6 | 6 |
| 6,1 | 6,1 |
| 6,8 | 6,8 |
| 6,9 | 7 |
| 7 | 7 |
| 7,9 | 8 |
| 9,9 | 10 |

Essa regra não é um arredondamento geral para cima: médias com outros décimos permanecem com seu valor. As médias dos módulos continuam na escala de 0 a 10; a frequência não recebe esse arredondamento e continua precisando ser pelo menos 75%. A definição para resultados com mais casas decimais permanece em aberto e não deve ser inferida a partir dos exemplos.

As datas de início e término do curso, previstas na documentação anterior mas ausentes do verso enviado, ficam em **Datas do curso (informação complementar)**. Não são exigidas pelo novo rascunho para concluir a conferência; quando ambas forem preenchidas, sua ordem é validada.

Depois que o professor salva e confirma os dados, atendendo aos critérios do formulário, a solicitação muda para **Solicitações prontas**. Essa ação registra o colaborador e a data no histórico. O aluno já aparece em **Rematrículas** desde o salvamento da frente, sem esperar o verso.

## Detalhamento das telas do protótipo

Esta seção transforma o que já pode ser experimentado em uma descrição funcional. A divisão do fluxo foi definida na conversa; métricas, validações e campos de conferência ainda marcados como provisórios continuam sujeitos a revisão.

| Tela | Objetivo | Ações principais |
| --- | --- | --- |
| Acesso | Permitir a entrada do colaborador. | Informar usuário/CPF e senha; entrar na Visão geral. |
| Visão geral | Mostrar o trabalho pendente e os totais do momento. | Consultar indicadores; abrir uma solicitação ou uma das áreas. |
| Solicitações | Organizar as filas dos professores e de certificados. | Pesquisar; criar solicitação; abrir o detalhe de um aluno. |
| Nova solicitação / Dados do aluno | Registrar ou corrigir a frente do formulário. | Preencher dados e interesses; conferir; salvar ou cancelar. |
| Detalhe da solicitação | Completar o verso e consultar a conversa. | Editar dados do aluno; salvar dados acadêmicos; enviar mensagem; confirmar o encaminhamento. |
| Rematrículas | Acompanhar interesses e o resultado comercial. | Pesquisar; selecionar aluno; registrar situação e observação; salvar acompanhamento. |

### Visão geral

O dashboard atual apresenta três indicadores, calculados a partir dos registros da demonstração:

- **Solicitações em andamento:** quantidade ainda não encaminhada para certificados.
- **Solicitações prontas:** quantidade já confirmada pelos professores.
- **Alunos não contatados:** quantidade de acompanhamentos comerciais com situação Não contatado.

A lista **Aguardando o professor** permite abrir diretamente uma solicitação em andamento. Os indicadores levam às áreas correspondentes; hoje, clicar em um indicador de solicitações abre as duas filas, sem aplicar um filtro específico.

Não há filtro por período nem taxa de conversão nesta versão. Antes de calcular conversão, precisamos definir qual período e quais situações entram na conta. Os totais por perfil e a contagem de alunos únicos também serão definidos quando tratarmos das permissões e de múltiplas solicitações do mesmo aluno.

### Lista de solicitações e pesquisa

Cada item mostra nome do aluno, número da solicitação, curso concluído quando já informado, professor e indicação da etapa atual. Se o curso ainda estiver em branco, aparece **Curso a informar pelo professor**.

A pesquisa fica acima das duas filas. As quantidades e os itens de cada fila acompanham o resultado da busca. A tela informa quantos registros foram encontrados em relação ao total. Ao criar uma nova solicitação, a busca dessa aba é limpa para que o registro recém-criado fique visível.

**Cancelar**, na criação, retorna à lista sem criar a solicitação nem o acompanhamento comercial. Na edição dos dados do aluno, retorna ao detalhe sem salvar as alterações. Um aviso para outras saídas com alterações pendentes ainda não foi definido nem implementado.

### Campos da frente: dados do aluno

Os campos abaixo compõem a frente, já considerando a retirada da confirmação do aluno. As demais obrigatoriedades continuam baseadas no protótipo, para avaliação. A regra de selecionar pelo menos um interesse foi confirmada pelo usuário.

| Informação | Preenchimento no protótipo | Observação |
| --- | --- | --- |
| Nome completo | Obrigatório | Identifica o aluno na lista e no acompanhamento. |
| CPF | Obrigatório | Aceita pontuação; verifica a quantidade de 11 dígitos. A validação real dos dígitos verificadores ainda não existe. |
| Data de nascimento | Obrigatório | A demonstração limita a data a 30/10/2026; no sistema real, a referência deve ser a data atual. |
| Telefone | Obrigatório | Usado no acompanhamento comercial. Validação definitiva de formato ainda será definida. |
| Nome do responsável | Opcional | Exibido para o caso de menores de idade. |
| Endereço | Obrigatório | Campo completo com rua, número e complemento. |
| Cidade e bairro | Obrigatórios | Campos separados. |
| Certificações em interesse | No mínimo 1 e no máximo 3 | Opções distintas do impresso; Outros conta como uma opção e exige descrição. O limite vale também na edição. |
| Descrição de Outros | Obrigatória quando Outros estiver marcado | Texto livre que acompanha o interesse no comercial. |
| Data da solicitação | Obrigatória | Sugerida com a data da demonstração. |

**Regra confirmada:** um aluno pode ter mais de uma solicitação. O CPF identifica o cadastro do aluno, não uma solicitação exclusiva. Ao localizar um CPF já cadastrado, o sistema deve permitir reutilizar seus dados para uma nova solicitação, com número, curso concluído, módulos, médias e andamento próprios. A demonstração ainda bloqueia o CPF repetido; esse bloqueio deve ser substituído. Continua pendente apenas o tratamento de repetição acidental da mesma conclusão de curso.

### Aviso de possível solicitação repetida — proposta para validar

A ideia indicada pelo usuário é avisar e perguntar se se trata de outra solicitação, em vez de bloquear. A proposta foi simplificada para avaliação:

1. O professor informa o CPF e o sistema encontra o cadastro do aluno e suas solicitações anteriores.
2. A tela avisa: **“Este aluno já tem uma solicitação de certificado. Você quer abrir a existente ou criar outra?”** Mostra o curso, a data e a situação para ajudar a pessoa a identificar o registro.
3. **Abrir a existente** leva ao registro anterior para continuar ou consultar o preenchimento, sem criar outro acompanhamento comercial.
4. **Criar outra solicitação** reutiliza o cadastro do aluno e inicia uma nova solicitação com número próprio e seu novo acompanhamento, preservando os anteriores. A decisão pode ser registrada automaticamente no histórico.

Exemplo: João já tem uma solicitação de Informática e agora concluiu Desenvolvedor de Games. O aviso permite confirmar a criação de outra solicitação, sem precisar cadastrar João novamente. Se o professor estava apenas tentando continuar a solicitação de Informática, pode abrir a que já existe.

O aviso continua sendo uma proposta a validar com o usuário. Ter o mesmo CPF não prova que a nova solicitação está errada, pois um aluno pode ter várias solicitações legítimas.

Também é recomendável que dois cliques seguidos em Salvar não criem duas solicitações: repetição acidental do envio deve ser tratada separadamente da criação intencional de outra solicitação.

### Campos do verso: preenchimento do professor

| Informação | Durante o preenchimento | Para encaminhar no protótipo |
| --- | --- | --- |
| Nome do curso concluído | Pode ficar pendente em um salvamento parcial. | Deve estar preenchido. |
| Professor | Preenchido como Samuel Braz na demonstração. | No sistema real, deve representar o colaborador responsável; regras de atribuição ainda serão definidas. |
| Módulos e médias | Até 13 linhas; permite salvar módulo com média ainda pendente. | Pelo menos um módulo e média para cada módulo informado. Linhas totalmente vazias não entram no cálculo. |
| Médias dos módulos | Entre 0 e 10; uma média preenchida precisa estar associada a um módulo. | Todos os módulos informados precisam ter média válida. |
| Média final | Média aritmética simples dos módulos cursados, com pesos iguais; somente o final ,9 sobe para o próximo inteiro. Fica sem valor quando o preenchimento está incompleto. | Mínimo 7 depois da regra aplicável. A frequência mínima de 75% continua sendo exigida separadamente. |
| Frequência | Pode ficar pendente no salvamento parcial; quando informada, entre 0 e 100%. | Preenchida e maior ou igual a 75%. |
| Data da conferência do professor | Sugerida na abertura do detalhe. | Deve estar preenchida e salva. |
| Início e término do curso | Complementares e opcionais nesta versão. | Se ambos forem informados, o término não pode anteceder o início. |
| Conferência final | Marcação separada do salvamento. | O professor precisa marcar a conferência e confirmar. |

**Salvar dados acadêmicos** mantém a solicitação na etapa em que já estava; em um registro em andamento, isso permite continuar depois. **Confirmar e encaminhar** conclui a etapa do professor. Alterações acadêmicas ainda não salvas impedem o encaminhamento. Alterar esses campos desmarca a conferência, exigindo uma nova revisão.

Os 13 módulos reproduzem o impresso, mas o usuário confirmou que o professor poderá adicionar e remover módulos conforme o curso realmente feito pelo aluno. O número máximo ainda não foi definido como regra do sistema. O preenchimento manual do protótipo deve evoluir para seleção de curso cadastrado e carregamento automático dos seus módulos.

### Catálogo de cursos e módulos personalizados

Os cursos serão cadastrados previamente com nome e módulos padrão. A Direção administra esse catálogo. O professor pode adaptar os módulos de uma solicitação individual, mas essa permissão não altera o cadastro padrão do curso.

Exemplo: ao selecionar **Desenvolvedor de Games**, a solicitação recebe os módulos cadastrados desse curso, sem o professor precisar digitá-los novamente. O professor informa as médias e pode remover módulos não cursados ou acrescentar módulos extras.

Essas adaptações pertencem àquela solicitação e não modificam o catálogo padrão para os demais alunos. As médias e os módulos efetivamente cursados devem permanecer associados à solicitação, mesmo se o cadastro do curso mudar no futuro.

### Emissão externa e confirmação no Logus

O departamento de Certificados abre uma solicitação de **Solicitações prontas**, emite o documento no outro sistema e confirma no Logus que a emissão já aconteceu. Essa confirmação move o registro para **Emitido** e registra o colaborador e o momento da ação no histórico.

O Logus não gera nem altera o PDF do certificado. Emitido informa que a equipe confirmou a emissão externa; não é uma emissão automática provocada pelo preenchimento das médias.

### Correção após encaminhamento ou emissão

O usuário aceitou o fluxo de registrar a alteração, avisar Certificados e exigir nova conferência. O histórico deve mostrar os valores anteriores e novos e identificar quem corrigiu.

A confirmação de emissão já registrada precisa ser preservada no histórico, mesmo quando os dados forem corrigidos depois. O certificado externo pode precisar de correção ou reemissão; alterar o Logus não realiza isso automaticamente.

**Fluxo confirmado:** ao salvar uma correção depois do encaminhamento ou da emissão, a solicitação volta para **Em andamento**. A alteração gera mensagem automática com autor, momento e valores alterados; Certificados recebe o aviso. O professor deve salvar os dados corrigidos, conferir novamente e encaminhar para **Solicitações prontas**. O registro da emissão anterior permanece no histórico.

Quando necessária, a correção ou reemissão do certificado acontece no sistema externo. A solicitação só retorna para **Emitido** quando a equipe de Certificados confirmar novamente a emissão. A forma de apresentação dos avisos e os dados adicionais de uma reemissão ainda serão detalhados.

Editar uma solicitação de outro professor não deve atribuir a mudança ao professor original: a mensagem automática identifica o colaborador que realmente fez a alteração. O professor que iniciou a solicitação e os autores das correções são informações distintas.

### Rematrículas e acompanhamento comercial

A lista permite selecionar um aluno e abrir o acompanhamento ao lado. O detalhe mostra nome, contato, interesses e a solicitação de origem, além dos campos editáveis pelo comercial.

Ao entrar na aba **Rematrículas**, nenhum aluno vem selecionado, inclusive quando o colaborador retorna de outra área. O painel da direita pede que seja escolhido um aluno. A busca e os filtros não selecionam automaticamente o primeiro resultado.

Após a seleção explícita de um aluno, o topo do acompanhamento destaca **os interesses**, em etiquetas azul-escuras, e **o telefone**, em um bloco próprio com número ampliado. O nome e a situação ficam acima; professor, solicitação de origem e data aparecem como informações secundárias abaixo. Todos os interesses permanecem visíveis, inclusive descrições longas em Outros. Em janelas menores, interesses e contato são empilhados para manter a leitura.

Estas são as situações do acompanhamento, com o encerramento por recusa agora confirmado:

| Situação | Significado pretendido |
| --- | --- |
| Não contatado | O acompanhamento foi criado e ainda não houve registro de contato. |
| Contatado | A equipe já entrou em contato e pode continuar o atendimento. |
| Rematriculado | A equipe informa que houve matrícula fora do Logus. |
| Recusou | O aluno informou que não deseja continuar naquele acompanhamento; essa escolha o encerra automaticamente, permitindo reabertura posterior. |

Na revisão 0.4, a aba ganhou filtros visíveis por essas quatro situações e a opção **Todos**. A situação aparece tanto no filtro quanto no cartão e no detalhe do aluno, sempre escrita por extenso:

| Filtro / situação | Cor |
| --- | --- |
| Não contatado | Amarelo / âmbar. |
| Contatado | Azul. |
| Rematriculados | Verde. |
| Recusou | Vermelho. |
| Todos | Neutro, com destaque azul-escuro quando selecionado. |

Um filtro fica ativo por vez e pode ser combinado com a pesquisa por nome, CPF ou solicitação. As quantidades dos filtros consideram os resultados da pesquisa antes do filtro de situação, permitindo comparar os grupos. A opção Todos remove apenas a restrição por situação. As cores são acompanhadas de texto, quantidade e uma marca de seleção no filtro ativo.

**Ordenação definida:** os acompanhamentos aparecem do mais recente ao mais antigo, pela data e hora em que foram criados ao salvar a etapa inicial do aluno. Cada cartão mostra esse momento. A data digitada na frente e a data do último contato não substituem a data de criação na ordenação. Alterar a situação comercial preserva a posição cronológica; empates usam o número da solicitação, do maior para o menor.

Ao salvar uma nova situação, os filtros, quantidades e etiquetas são atualizados. Se o aluno deixar de corresponder ao filtro ativo, seu cartão sai daquela lista e a confirmação do salvamento permanece visível acima dos resultados.

O comercial salva a situação e uma observação livre. Marcar **Recusou** encerra automaticamente o acompanhamento, sem excluir o registro. Ele permanece disponível na situação Recusou e pode ser reaberto quando o aluno retomar o interesse. **Comercial ou Direção** pode reabrir; a situação retorna para **Contatado**. A recusa e a reabertura permanecem registradas no histórico. O detalhamento de cada contato ainda será definido; a observação atual do protótipo é um campo editável.

O mesmo aluno pode ter um novo acompanhamento para outra oportunidade de curso, inclusive para continuar em um terceiro curso. Esse novo registro não substitui nem reescreve o resultado de um atendimento anterior. Uma nova solicitação de certificado continua gerando o acompanhamento automaticamente; a forma de iniciar uma nova oportunidade comercial sem outra solicitação de certificado ainda será detalhada.

Uma mudança comercial não conclui a solicitação de certificado. Da mesma forma, a conclusão do professor não altera a situação comercial nem cria outro acompanhamento. Correções nos dados e interesses atualizam o acompanhamento existente, preservando a situação e a observação já salvas.

Quando a pesquisa não encontra o aluno que estava selecionado, o detalhe dele fica oculto. Havendo outros resultados, a tela pede que seja selecionado um deles. A pesquisa preserva o texto comercial ainda em edição; sair da área ou selecionar outro aluno sem salvar não tem proteção contra perda de alterações nesta versão.

## Etapas da solicitação e indicações visuais

As três etapas definidas representam o andamento principal. O protótipo ainda mostra apenas as duas primeiras. As indicações dentro dos itens ajudam a identificar o que falta; não precisam virar estados independentes no modelo de domínio.

| Fila | Indicação no protótipo | Significado |
| --- | --- | --- |
| Em andamento | Aguardando médias | A frente foi salva e ainda faltam médias ou informações dos módulos. |
| Em andamento | Aguardando conferência | Os módulos têm médias; o professor ainda não confirmou o encaminhamento. Isso, sozinho, não garante que os demais critérios estejam atendidos. |
| Solicitações prontas | Pronta para certificados | O professor salvou e confirmou os dados, atendendo às verificações do rascunho. |
| Emitido | Emissão confirmada | A equipe de Certificados confirmou no Logus que o certificado foi emitido externamente. Ainda será adicionada ao protótipo. |

O fluxo principal é **salvar a frente → preencher e salvar o verso → conferir → encaminhar para Solicitações prontas → emitir externamente → confirmar e mover para Emitido**. O acompanhamento comercial começa no primeiro salvamento, em paralelo às demais etapas.

**Pronta para certificados não significa certificado emitido.** A emissão acontece fora do Logus, e a etapa **Emitido** registra a confirmação posterior feita pela equipe.

Correções posteriores são permitidas na demonstração e aparecem no histórico. O retorno para Em andamento, o aviso para Certificados e a nova conferência obrigatória estão aprovados para o sistema, mas ainda não estão implementados no protótipo.

## Vocabulário inicial para o domínio

Este vocabulário ajuda a manter documentação, telas e futuro código falando a mesma língua. Não define ainda classes, agregados ou estrutura de pastas.

| Termo | Significado no Logus |
| --- | --- |
| Aluno | Pessoa identificada pelo CPF, cujo cadastro pode ser reutilizado em várias solicitações e acompanhamentos. Não tem conta de acesso própria. |
| Colaborador | Funcionário que acessa o sistema com suas permissões. |
| Solicitação de certificado | Registro que reúne a frente do aluno, o verso do professor e sua conferência para a emissão externa. |
| Curso concluído | Curso usado pelo professor no preenchimento acadêmico daquela solicitação. |
| Curso cadastrado | Item do catálogo com seus módulos padrão, usado para iniciar o preenchimento do professor. |
| Módulo cursado | Parte do curso concluído, associada à média obtida pelo aluno. |
| Certificação em interesse | Opção de continuidade dos estudos escolhida na frente; não é o certificado que está sendo solicitado. |
| Acompanhamento comercial | Registro de uma oportunidade de continuidade, com interesses, situação e observação. Um aluno pode ter mais de um ao longo do tempo. |
| Conversa | Mensagens escritas pela equipe dentro do detalhe da solicitação. |
| Histórico de alterações | Registros automáticos das mudanças e confirmações da solicitação. |

## Cenários para conferir o fluxo

Estes exemplos reúnem comportamentos demonstrados e decisões já confirmadas para o sistema. Os cenários de catálogo, múltiplas solicitações, Emitido, revisão e reabertura ainda precisam ser incorporados ao protótipo e ao backend.

| Cenário | Resultado esperado |
| --- | --- |
| Salvar a frente preenchida, mas sem interesse. | Informar a pendência e não criar registros. |
| Selecionar mais de 3 interesses na criação ou edição. | Informar o limite e impedir o salvamento até que a seleção seja ajustada, sem remover opções silenciosamente. |
| Marcar Outros sem descrever a certificação. | Pedir a descrição antes de salvar. |
| Salvar a frente com interesse e dados conferidos, ainda sem médias. | Criar a solicitação em andamento e o acompanhamento Não contatado. |
| Salvar um módulo com média pendente. | Permitir continuar depois; não permitir encaminhamento incompleto. |
| Tentar encaminhar com média final abaixo de 7 ou frequência abaixo de 75%. | Manter a solicitação em andamento e informar o critério pendente. |
| A média aritmética resultar em 6,9, com frequência de pelo menos 75%. | A média final vira 7; permite encaminhar quando os demais requisitos forem atendidos. |
| A média aritmética resultar exatamente em 6. | A média final permanece 6; não atende ao mínimo de aprovação. |
| A média aritmética resultar em 6,1 ou 6,8. | O valor permanece igual; não sobe para 7 e não atende ao mínimo de aprovação. |
| Salvar dados completos, média final 7 e frequência 75%, conferir e encaminhar. | Mover para Solicitações prontas e registrar a confirmação no histórico. |
| Alterar médias e tentar encaminhar sem salvar. | Solicitar o salvamento e a conferência antes de concluir. |
| Editar contato ou interesse de um aluno já acompanhado. | Atualizar o mesmo acompanhamento, preservando a situação e a observação salvas. |
| Pesquisar por nome, CPF com pontuação ou número da solicitação. | Mostrar os registros correspondentes; em Solicitações, filtrar ambas as filas. |
| Limpar a pesquisa. | Voltar a mostrar todos os registros daquela aba. |
| Não encontrar resultados. | Exibir mensagem de lista vazia, sem mostrar um acompanhamento comercial fora da busca. |
| Clicar em Sair. | Voltar ao acesso; na demonstração, a próxima entrada leva à Visão geral. |
| Criar uma nova solicitação para um CPF já cadastrado. | Reutilizar o aluno e criar outra solicitação, preservando as anteriores. |
| Selecionar um curso cadastrado. | Carregar seus módulos padrão para o professor preencher as médias. |
| Remover ou acrescentar um módulo para um aluno. | Ajustar apenas aquela solicitação, sem mudar o catálogo. |
| Certificados confirmar a emissão externa de uma solicitação pronta. | Mover para Emitido e registrar a ação no histórico. |
| Corrigir dados depois do encaminhamento ou da emissão. | Voltar para Em andamento, registrar a alteração com autor, avisar Certificados e exigir nova conferência, preservando os registros anteriores. |
| Um professor alterar uma solicitação iniciada por outro. | Permitir a edição e registrar automaticamente quem realmente alterou, quando e quais valores mudaram. |
| Marcar um acompanhamento como Recusou. | Encerrar automaticamente o atendimento e mantê-lo disponível para consulta e reabertura. |
| Comercial ou Direção reabrir um atendimento recusado. | Retornar para Contatado e preservar no histórico a recusa e a reabertura. |
| O aluno demonstrar interesse em outro curso depois de um atendimento anterior. | Permitir outro acompanhamento sem substituir o anterior. |
| Salvar a frente sem assinatura ou confirmação de identidade do aluno. | Permitir salvar quando os dados necessários e pelo menos um interesse estiverem preenchidos. |

## Limites desta versão e próxima definição

O protótipo valida o fluxo e a organização das telas. Ainda não implementa autenticação real, permissões por perfil, banco, cadastros administrativos de cursos e colaboradores, emissão de certificado ou matrícula. Os dados pessoais e acadêmicos são apenas locais e se perdem ao atualizar a página.

O nome do colaborador é fixo na demonstração. O histórico usa mensagens como Agora, sem um mecanismo real de auditoria. Data, hora e autor confiáveis deverão ser registrados pelo backend no sistema definitivo. A foto de perfil é somente uma prévia local.

A demonstração contém **20 alunos fictícios**, com datas de criação, interesses, etapas acadêmicas e situações comerciais variadas para experimentar listas mais cheias, ordenação e filtros. Os registros são exemplos, não dados reais de alunos. Novos registros criados na página também entram na mesma ordenação e nos mesmos filtros.

As permissões principais e as transições de correção e reabertura estão definidas. Já existe base para começar a modelagem do domínio, separando as regras confirmadas das pendências pontuais listadas acima. Isso não significa que todas as funções descritas já estejam implementadas no protótipo.

### Permissões e transições confirmadas na revisão 0.8

- Professor visualiza e altera todas as solicitações. Toda alteração gera mensagem automática com autor, data e hora e valores alterados; Direção também pode corrigir qualquer uma.
- Direção administra cadastros de cursos, módulos padrão e colaboradores.
- Uma correção após encaminhamento ou emissão retorna a solicitação para Em andamento e exige nova conferência. A emissão anterior continua registrada no histórico, e Certificados recebe o aviso.
- Comercial ou Direção pode reabrir um atendimento Recusou; ele volta para Contatado, pois o atendimento anterior já aconteceu. A recusa e a reabertura permanecem no histórico.

Essas regras foram confirmadas pelo usuário. O limite de 1 a 3 interesses está confirmado; o arredondamento foi corrigido na revisão 0.10 para elevar somente o final ,9. O aviso de possível duplicidade, o tratamento de médias com mais casas decimais e os detalhes dos comandos não especificados ainda precisam ser validados.
