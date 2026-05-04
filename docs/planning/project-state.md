# Estado do projeto - Study Lab

Este e o documento vivo de continuidade do Study Lab. Ele deve ser lido junto com o `HARNESS.md` e atualizado ao fim de cada etapa concluida, antes de commit e push.

## Regra de atualizacao obrigatoria

- Atualizar este arquivo sempre que uma etapa planejada for concluida.
- Registrar a fase atual, a ultima etapa concluida e a proxima etapa executavel.
- Registrar verificacoes feitas e qualquer teste nao executado.
- Registrar decisoes recentes que afetem continuidade.
- Confirmar que nao ha segredos, dados locais privados, videos, bancos locais ou artefatos gerados sendo versionados.

## Ponto atual

- Data de referencia: 2026-05-04.
- Fase atual: Fase 4 - App desktop e catalogo.
- Status da fase: em andamento, com importacao basica pela UI concluida neste recorte.
- Ultima implementacao concluida: fluxo de importacao pela UI com persistencia no catalogo local.
- Commit publicado mais recente antes desta etapa: `e67caa0 feat: add winui catalog shell`.
- Branch atual: `main`.
- Remoto oficial: `origin` em `https://github.com/jotaCorsino/Study-Lab.git`.

## Ultima etapa concluida

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

## Historico imediato

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

Continuar a Fase 4 - App desktop e catalogo:

- reler a arvore obrigatoria completa;
- criar navegacao/tela de detalhes do curso para inspecionar a estrutura importada;
- exibir arquivos rejeitados da importacao sem vazar caminhos sensiveis alem dos relativos ja sanitizados;
- avaliar tratamento de duplicidade quando a mesma pasta for importada novamente;
- ajustar verificacao de solucao para builds WinUI com plataforma x64;
- manter UI sem regra de negocio e com view models testaveis.

## Pendencias praticas

- Ajustar a estrategia de verificacao por `.slnx`: `dotnet test .\StudyLab.slnx -p:Platform=x64` retornou configuracao de solucao invalida `Debug|x64`.
- Criar tela de detalhes do curso e navegacao a partir do catalogo.
- Exibir resumo de arquivos rejeitados apos importacao.
- Definir regra para reimportacao/duplicidade de cursos.
- Definir identidade/assinatura/distribuicao MSIX antes de release.
- Framework MVVM/toolkit ainda precisa de decisao futura.

## Decisoes recentes

- .NET 10 adotado como base inicial.
- Arquitetura limpa definida com Domain, Application, Infrastructure e Desktop.
- WinUI 3 definido como UI para Windows.
- Scaffold WinUI inicial definido pelo template oficial Microsoft `Microsoft.WindowsAppSDK.WinUI.CSharp.Templates`; ver `docs/decisions/ADR-0004-winui-scaffold-and-packaging.md`.
- Empacotamento inicial definido como single-project MSIX packaged; assinatura/distribuicao ainda pendentes para release.
- View models desktop ficam em `StudyLab.Desktop.Presentation` para evitar acoplamento de testes ao runtime WinUI.
- Dialogo de selecao de pasta fica isolado atras de `ICourseFolderPicker`; a implementacao WinUI inicial usa `FolderPicker` com HWND da janela.
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
- `dotnet test .\tests\StudyLab.Domain.Tests\StudyLab.Domain.Tests.csproj`: 11 testes aprovados.
- Red de TDD confirmado para Application: `ImportCourseToLibraryUseCase`, `CourseLibraryImportResult` e `ImportCourseToLibraryCommand` ausentes.
- Red de TDD confirmado para Presentation: `ICourseFolderPicker` e fluxo `ImportCourseAsync` ausentes.
- `dotnet test .\tests\StudyLab.Application.Tests\StudyLab.Application.Tests.csproj`: 9 testes aprovados.
- `dotnet test .\tests\StudyLab.Infrastructure.Tests\StudyLab.Infrastructure.Tests.csproj`: 7 testes aprovados.
- `dotnet test .\tests\StudyLab.Desktop.Tests\StudyLab.Desktop.Tests.csproj`: 6 testes aprovados.
- `dotnet build .\src\StudyLab.Desktop\StudyLab.Desktop.csproj -p:Platform=x64`: sucesso, 0 avisos e 0 erros.
- Launch via `shell:AppsFolder` confirmou processo `StudyLab.Desktop` ativo com janela `Study Lab`.
- `dotnet test .\StudyLab.slnx -p:Platform=x64` nao passou por configuracao de solucao invalida `Debug|x64`; usar testes por projeto ate ajustar a solucao.
- `git status --short -uall` revisado: apenas codigo, docs e assets do template WinUI; `bin/`, `obj/` e artefatos permanecem ignorados.

## Criterio para continuar

Ao receber o pedido "leia o harness e continue a implementacao", o agente deve:

1. Ler a arvore obrigatoria do `HARNESS.md`, incluindo este arquivo.
2. Identificar a proxima etapa executavel neste documento.
3. Validar `git status`.
4. Executar a proxima etapa em escopo pequeno.
5. Atualizar este arquivo ao concluir.
6. Commitar e enviar ao remoto quando a entrega estiver consistente.
