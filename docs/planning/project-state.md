# Estado do projeto - Study Lab

Este e o documento vivo de continuidade do Study Lab. Ele deve ser lido junto com o `HARNESS.md` e atualizado ao fim de cada etapa concluida, antes de commit e push.

## Regra de atualizacao obrigatoria

- Atualizar este arquivo sempre que uma etapa planejada for concluida.
- Registrar a fase atual, a ultima etapa concluida e a proxima etapa executavel.
- Registrar verificacoes feitas e qualquer teste nao executado.
- Registrar decisoes recentes que afetem continuidade.
- Confirmar que nao ha segredos, dados locais privados, videos, bancos locais ou artefatos gerados sendo versionados.

## Ponto atual

- Data de referencia: 2026-05-05.
- Fase atual: Fase 5 - Player e progresso.
- Status da fase: em andamento, com retomada inicial da aula pela posicao salva concluida neste recorte.
- Ultima implementacao concluida: player aplica a duracao assistida salva ao abrir a aula.
- Commit publicado mais recente antes desta etapa: `4093341 feat: persist manual lesson progress`.
- Branch atual: `main`.
- Remoto oficial: `origin` em `https://github.com/jotaCorsino/Study-Lab.git`.

## Ultima etapa concluida

Retomada inicial da aula:

- `LessonPlayerViewModel` passou a expor `ResumePosition` e `ShouldResumePlayback`.
- A retomada so e habilitada quando a aula carregou sem erro, nao esta concluida e existe duracao assistida maior que zero.
- Aulas concluidas ou com erro retornam `ResumePosition` zero e nao acionam retomada automatica.
- `LessonPlayerPage` prepara a posicao de retomada antes de definir a fonte de midia e aplica a posicao quando o `MediaPlayer` dispara `MediaOpened`.
- A regra de decidir quando retomar fica no view model testavel; a UI apenas aplica a posicao no controle nativo.
- Nenhum caminho local e exposto em texto de status.

## Historico imediato

Progresso manual da aula:

- `RecordLessonProgressCommand` e `RecordLessonProgressUseCase` criados em Application para salvar progresso de aula existente no catalogo.
- O caso de uso rejeita ids vazios, duracao negativa e nao salva quando a aula nao existe.
- Ao gravar progresso existente, a duracao assistida nao diminui e uma conclusao ja registrada nao e revertida.
- `LoadLessonPlaybackUseCase` passou a carregar `WatchedDuration` e `IsCompleted` junto com a midia segura da aula.
- `LessonPlayerViewModel` passou a exibir progresso, estado concluido e acionar `MarkCompleted` sem expor caminhos locais.
- `LessonPlayerPage` recebeu acao nativa no `CommandBar` para marcar a aula como concluida usando a posicao atual do player quando disponivel.
- A persistencia continua no snapshot JSON existente e a UI segue como adaptador fino.

Correcao da arvore visual de detalhes do curso:

- Bug observado: apos importar/criar um curso, a pagina de detalhes carregava o curso, mas os nomes de modulos, aulas e videos nao apareciam na arvore.
- Causa: `CourseDetailPage` monta a arvore com `TreeViewNode.Content = CourseDetailItemViewModel`, mas o template XAML fazia binding direto em `Title`, `KindText` e `IconGlyph`.
- Correcao: o template passou a usar `Content.Title`, `Content.KindText` e `Content.IconGlyph`.
- `CourseTreeView_ItemInvoked` agora tambem entende quando o evento entrega o proprio `TreeViewNode`, mantendo a navegacao para o player.
- Teste de regressao `CourseDetailPageXamlTests` criado para garantir que o template continue apontando para `TreeViewNode.Content`.
- A correcao nao altera modelo de dominio, persistencia ou dados locais do usuario.

Player local inicial:

- Fase 4 foi considerada concluida no planejamento macro apos catalogo, importacao, detalhe e verificacao de solucao x64.
- `LessonPlaybackIdentity` criado em Application para derivar um identificador estavel de aula a partir de `courseId` e caminho relativo normalizado.
- `LoadLessonPlaybackUseCase` criado para localizar a aula no catalogo, validar extensao de video permitida, canonicalizar raiz + caminho relativo e bloquear abertura fora da raiz do curso.
- `CourseDetailItem` e `CourseDetailItemViewModel` passaram a expor `LessonId` apenas para itens de aula, sem expor caminho local na tela de detalhes.
- `LessonPlayerViewModel` criado em `StudyLab.Desktop.Presentation` com mensagens seguras para aula ausente ou midia rejeitada.
- `LessonPlayerPage` criada em WinUI com `MediaPlayerElement`, controles nativos e navegacao a partir do clique em uma aula na arvore de detalhes.
- A UI continua como adaptador fino: a decisao de qual arquivo pode ser aberto fica no caso de uso de Application.

Verificacao de solucao x64:

- `StudyLab.slnx` recebeu `<Configurations>` com `Any CPU` e `x64`.
- O projeto `StudyLab.Desktop` recebeu mapeamento de plataforma no `.slnx` com `<Platform Project="x64" />`.
- O erro `MSB4126` para `Debug|x64` foi eliminado.
- `dotnet build .\StudyLab.slnx -p:Platform=x64` compila o WinUI em `bin\x64\Debug\...\win-x64` sem avisos ou erros.
- `dotnet test .\StudyLab.slnx -p:Platform=x64` executa todos os projetos de teste da solucao com sucesso.
- A verificacao por projeto continua valida, mas a solucao agora e o comando principal para a regressao local x64.

Regra de duplicidade de importacao:

- `CourseLibraryImportStatus` criado para diferenciar importacao nova de duplicidade ignorada.
- `CourseLibraryImportResult` passou a expor `Status` e `WasImported`.
- `ImportCourseToLibraryUseCase` agora carrega o snapshot antes de ler arquivos e compara a raiz selecionada com as raizes ja importadas.
- A comparacao de duplicidade normaliza a raiz com `Path.GetFullPath`, remove separador final e usa comparacao case-insensitive, adequada ao alvo Windows.
- Quando a pasta ja existe no catalogo, o caso de uso retorna o curso existente, nao chama `ICourseFolderReader` e nao salva novo snapshot.
- `CatalogViewModel` mostra "Curso ja importado", recarrega o catalogo e limpa rejeicoes obsoletas quando a duplicidade e detectada.
- Testes cobrem que a reimportacao nao reler arquivos, nao salva duplicado e mantem a UI sem erro generico.

Resumo de arquivos rejeitados:

- `RejectedCourseFileViewModel` criado em `StudyLab.Desktop.Presentation` para projetar caminho relativo e motivo amigavel.
- `CatalogViewModel` passou a manter `RejectedFiles`, `HasRejectedFiles` e `RejectedFilesSummary` apos importacao.
- `MainPage` recebeu `InfoBar` de aviso com lista dos arquivos ignorados pela importacao.
- O painel mostra apenas `RejectedCourseFile.RelativePath`, que ja e normalizado e bloqueia raiz absoluta/traversal.
- Falha ou cancelamento de importacao limpa a lista de rejeicoes para evitar informacao obsoleta na UI.
- Testes garantem resumo, motivos amigaveis e ausencia de propriedades de raiz/caminho absoluto no view model de rejeicao.

Tela de detalhes do curso:

- `LoadCourseDetailUseCase` criado em Application para carregar um curso por id sem expor `RootPath` na superficie de detalhe.
- `CourseDetail` e `CourseDetailItem` criados para representar titulo, data de importacao, contagem de aulas e arvore importada.
- `CourseDetailViewModel` e `CourseDetailItemViewModel` criados em `StudyLab.Desktop.Presentation` com testes unitarios e sem propriedades de caminho local.
- `MainPage` passou a permitir clique em curso e navegar para `CourseDetailPage`.
- `CourseDetailPage` criada em WinUI com `CommandBar`, botao voltar, `InfoBar` e `TreeView` para inspecionar a estrutura importada.
- `DesktopCompositionRoot` passou a criar view models de catalogo e detalhe usando o mesmo caminho local de biblioteca.
- A UI continua sem regra de negocio; o code-behind apenas navega e materializa a arvore visual a partir do view model.
- Caminhos absolutos locais continuam fora da tela de catalogo e da tela de detalhe.

Importacao basica pela UI:

- `ImportCourseToLibraryUseCase` criado em Application para importar uma pasta, transformar a arvore importada em `CourseCatalogEntry`, preservar progresso/preferencias existentes e salvar o snapshot atualizado.
- `ImportCourseToLibraryCommand` criado com id e data de importacao explicitos para manter testes deterministicos.
- `CourseLibraryImportResult` criado para retornar o curso salvo e arquivos rejeitados.
- `ICourseFolderPicker` criado em `StudyLab.Desktop.Presentation` para manter o view model independente do WinUI.
- `CatalogViewModel.ImportCourseAsync` criado para selecionar pasta, importar, recarregar catalogo e exibir mensagens sanitizadas.
- `WinUiCourseFolderPicker` criado em `StudyLab.Desktop` usando `FolderPicker` inicializado com o HWND da janela.
- `MainPage` recebeu `CommandBar` com acao "Importar curso" e `InfoBar` de status.
- Erros esperados de importacao retornam mensagem generica sem caminho local ou stack trace.
- A UI continua sem regra de negocio; o code-behind apenas chama o view model e desabilita o botao durante a operacao.

App desktop e catalogo inicial:

- Template WinUI oficial Microsoft instalado via `dotnet new install Microsoft.WindowsAppSDK.WinUI.CSharp.Templates`.
- `dotnet new list winui` passou a listar o template `WinUI Blank App`.
- `StudyLab.Desktop` criado com WinUI 3, .NET 10 e single-project MSIX packaged.
- `StudyLab.Desktop.Presentation` criado para view models testaveis sem carregar WinUI/Windows App SDK nos testes unitarios.
- `CatalogViewModel` e `CatalogCourseViewModel` criados para carregar cursos de `LoadStudyLibraryUseCase`.
- Catalogo inicial exibe titulo do curso e quantidade de aulas, sem expor caminhos locais do curso na UI.
- `MainPage` criada como tela inicial de catalogo.
- `DesktopCompositionRoot` compoe `JsonStudyLibraryRepository`, `LoadStudyLibraryUseCase` e `CatalogViewModel`.
- Manifesto WinUI revisado para remover capability `systemAIModels`; `runFullTrust` permanece por ser requisito do app desktop empacotado.
- ADR-0004 criada para registrar scaffold WinUI, empacotamento inicial e separacao da camada de apresentacao.

Persistencia local:

- `IStudyLibraryRepository` criado em Application.
- `StudyLibrarySnapshot` criado para catalogo, progresso e preferencias.
- `CourseCatalogEntry`, `CourseCatalogItem`, `LessonProgressEntry` e `StudyPreferences` criados.
- `SaveStudyLibraryUseCase` e `LoadStudyLibraryUseCase` criados.
- `JsonStudyLibraryRepository` criado em Infrastructure usando `System.Text.Json`.
- Escrita local usa arquivo temporario no mesmo diretorio e substituicao do arquivo final.
- Carregamento de arquivo inexistente retorna snapshot vazio.
- JSON invalido falha com `InvalidDataException`.
- ADR-0003 criada para registrar persistencia local inicial em JSON.

Importacao segura de pastas publicada no commit `f533d06 feat: add secure course folder import`:

- `ImportCourseFromFolderUseCase`, `ICourseFolderReader` e `LocalCourseFolderReader` criados;
- allowlist de videos criada;
- `SafeRelativePath` criado para bloquear caminhos fora da raiz;
- ADR-0002 criada para registrar importacao como arvore flexivel de rascunho.

Dominio e testes iniciais publicados no commit `dab9a3b feat: add initial domain model and tests`:

- `StudyLab.slnx`, `StudyLab.Domain` e `StudyLab.Domain.Tests` criados;
- xUnit definido como framework de testes unitarios;
- modelos iniciais criados para curso, modulo, topico, aula, sessao, progresso, meta diaria e credito mensal;
- testes de dominio criados e aprovados.

Documento vivo de continuidade publicado no commit `f4379b0 docs: add project state tracking`:

- `docs/planning/project-state.md` criado como marcador operacional;
- `HARNESS.md` atualizado para incluir o estado vivo na leitura obrigatoria;
- documentos de suporte atualizados com a regra de continuidade.

Fundacao inicial do repositorio publicada no commit `4ea56bf docs: bootstrap study lab foundation`:

- documentacao obrigatoria criada;
- briefing original movido para `docs/product/study-lab-app.md`;
- pastas `src`, `tests`, `docs`, `build`, `tools`, `.agents` e `.github` organizadas;
- higiene de Git e configuracao .NET criadas;
- `main` publicado em `origin/main`.

## Proxima etapa executavel

Continuar a Fase 5 - Player e progresso:

- reler a arvore obrigatoria completa;
- evoluir registro automatico de duracao assistida durante a reproducao;
- tratar arquivo de midia ausente, movido ou renomeado sem expor caminho local em erro;
- manter UI sem regra de negocio e com view models testaveis.

## Pendencias praticas

- Definir identidade/assinatura/distribuicao MSIX antes de release.
- Framework MVVM/toolkit ainda precisa de decisao futura.
- Registro automatico durante reproducao ainda nao foi implementado.
- Arquivo de midia ausente, movido ou renomeado ainda precisa de tratamento dedicado na Fase 5.

## Decisoes recentes

- .NET 10 adotado como base inicial.
- Arquitetura limpa definida com Domain, Application, Infrastructure e Desktop.
- WinUI 3 definido como UI para Windows.
- Scaffold WinUI inicial definido pelo template oficial Microsoft `Microsoft.WindowsAppSDK.WinUI.CSharp.Templates`; ver `docs/decisions/ADR-0004-winui-scaffold-and-packaging.md`.
- Empacotamento inicial definido como single-project MSIX packaged; assinatura/distribuicao ainda pendentes para release.
- View models desktop ficam em `StudyLab.Desktop.Presentation` para evitar acoplamento de testes ao runtime WinUI.
- Dialogo de selecao de pasta fica isolado atras de `ICourseFolderPicker`; a implementacao WinUI inicial usa `FolderPicker` com HWND da janela.
- Detalhe do curso usa um caso de uso especifico que nao retorna `RootPath`; caminhos absolutos locais nao entram no view model.
- Rejeicoes de importacao sao exibidas no catalogo apenas com caminho relativo normalizado e motivo amigavel.
- Reimportacao da mesma pasta e tratada como duplicidade ignorada antes de ler o filesystem.
- `StudyLab.slnx` declara `Any CPU` e `x64`; o projeto WinUI e mapeado para `x64` na verificacao de solucao.
- Fase 4 foi encerrada apos catalogo, importacao, detalhes e verificacao x64.
- Fase 5 iniciou com identidade estavel de aula derivada de `courseId` + caminho relativo.
- Player local inicial usa `MediaPlayerElement`, mas a resolucao e validacao do arquivo ficam em Application.
- Erros de player devem continuar sem caminho local, stack trace ou detalhes internos na mensagem exibida.
- A arvore da tela de detalhes usa `TreeViewNode.Content`; templates XAML devem acessar propriedades via `Content.*`.
- Progresso de aula e salvo por `LessonId` deterministico no snapshot local.
- Marcacao manual de conclusao preserva a maior duracao assistida ja registrada.
- Retomada inicial usa a duracao assistida salva apenas quando a aula nao esta concluida.
- TDD definido como fluxo padrao para comportamento de negocio.
- xUnit definido como framework de testes unitarios.
- Importacao inicial definida como arvore flexivel de rascunho em Application; ver `docs/decisions/ADR-0002-imported-course-tree.md`.
- Persistencia local inicial definida como JSON com `System.Text.Json`; ver `docs/decisions/ADR-0003-local-json-persistence.md`.
- Security by design definido como requisito permanente.
- `docs/planning/project-state.md` definido como marcador operacional de continuidade.

## Verificacoes feitas

- `git status --short --branch` antes desta etapa: `main...origin/main`, sem alteracoes.
- `dotnet new list winui` inicialmente retornou nenhum modelo encontrado.
- Bootstrap WinUI via `winget configure -f config.yaml --accept-configuration-agreements --disable-interactivity` concluiu com sucesso, mas o template CLI ainda nao apareceu.
- `dotnet new search winui` encontrou o pacote oficial Microsoft `Microsoft.WindowsAppSDK.WinUI.CSharp.Templates`.
- `dotnet new install Microsoft.WindowsAppSDK.WinUI.CSharp.Templates` instalou templates WinUI.
- Red de TDD confirmado com `dotnet test .\tests\StudyLab.Desktop.Tests\StudyLab.Desktop.Tests.csproj -p:Platform=x64`: namespace `StudyLab.Desktop.Catalog` inexistente.
- Teste unitario desktop inicialmente falhou ao referenciar diretamente a assembly WinUI por inicializacao COM do Windows App SDK; a correcao foi separar view models em `StudyLab.Desktop.Presentation`.
- Red de TDD confirmado para Application: `ImportCourseToLibraryUseCase`, `CourseLibraryImportResult` e `ImportCourseToLibraryCommand` ausentes.
- Red de TDD confirmado para Presentation: `ICourseFolderPicker` e fluxo `ImportCourseAsync` ausentes.
- Red de TDD confirmado para detalhe Application: `LoadCourseDetailUseCase`, `CourseDetail` e `CourseDetailItem` ausentes.
- Red de TDD confirmado para detalhe Presentation: `CourseDetailViewModel` ausente.
- Red de TDD confirmado para resumo de rejeicoes Presentation: `RejectedCourseFileViewModel`, `RejectedFiles`, `HasRejectedFiles` e `RejectedFilesSummary` ausentes.
- Red de TDD confirmado para duplicidade: `CourseLibraryImportStatus`, `Status`, `WasImported` ausentes e leitor ainda chamado na reimportacao.
- Red de TDD confirmado para player inicial: namespaces/classes `StudyLab.Application.Playback` e `StudyLab.Desktop.Presentation.Playback` ausentes antes da implementacao.
- Red de regressao confirmado para arvore de detalhes: `CourseDetailPageXamlTests.CourseTreeTemplateBindsToTreeViewNodeContent` falhou com bindings antigos sem `Content.*`.
- Red de TDD confirmado para progresso manual: `RecordLessonProgressUseCase`, `RecordLessonProgressCommand`, `ProgressText`, `CanMarkCompleted` e `MarkCompleted` ausentes antes da implementacao.
- Red de TDD confirmado para retomada inicial: `ResumePosition` e `ShouldResumePlayback` ausentes antes da implementacao.
- `dotnet test .\tests\StudyLab.Application.Tests\StudyLab.Application.Tests.csproj`: 22 testes aprovados.
- `dotnet test .\tests\StudyLab.Infrastructure.Tests\StudyLab.Infrastructure.Tests.csproj`: 7 testes aprovados.
- `dotnet test .\tests\StudyLab.Desktop.Tests\StudyLab.Desktop.Tests.csproj`: 17 testes aprovados.
- `dotnet test .\tests\StudyLab.Domain.Tests\StudyLab.Domain.Tests.csproj`: 11 testes aprovados.
- `dotnet build .\StudyLab.slnx -p:Platform=x64`: sucesso, 0 avisos e 0 erros.
- `dotnet test .\StudyLab.slnx -p:Platform=x64`: 57 testes aprovados.
- Launch via `shell:AppsFolder` confirmou processo `StudyLab.Desktop` ativo com janela `Study Lab` apos a retomada inicial de aula.
- `git status --short -uall` revisado: apenas codigo, docs e assets do template WinUI; `bin/`, `obj/` e artefatos permanecem ignorados.

## Criterio para continuar

Ao receber o pedido "leia o harness e continue a implementacao", o agente deve:

1. Ler a arvore obrigatoria do `HARNESS.md`, incluindo este arquivo.
2. Identificar a proxima etapa executavel neste documento.
3. Validar `git status`.
4. Executar a proxima etapa em escopo pequeno.
5. Atualizar este arquivo ao concluir.
6. Commitar e enviar ao remoto quando a entrega estiver consistente.
