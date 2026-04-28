# Estado do projeto - Study Lab

Este e o documento vivo de continuidade do Study Lab. Ele deve ser lido junto com o `HARNESS.md` e atualizado ao fim de cada etapa concluida, antes de commit e push.

## Regra de atualizacao obrigatoria

- Atualizar este arquivo sempre que uma etapa planejada for concluida.
- Registrar a fase atual, a ultima etapa concluida e a proxima etapa executavel.
- Registrar verificacoes feitas e qualquer teste nao executado.
- Registrar decisoes recentes que afetem continuidade.
- Confirmar que nao ha segredos, dados locais privados, videos, bancos locais ou artefatos gerados sendo versionados.

## Ponto atual

- Data de referencia: 2026-04-28.
- Fase atual: Fase 3 - Persistencia local.
- Status da fase: concluida no recorte inicial.
- Ultima implementacao concluida: persistencia local inicial em JSON para catalogo, progresso e preferencias.
- Commit publicado mais recente antes desta etapa: `f533d06 feat: add secure course folder import`.
- Branch atual: `main`.
- Remoto oficial: `origin` em `https://github.com/jotaCorsino/Study-Lab.git`.

## Ultima etapa concluida

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

## Historico imediato

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

Depois que esta etapa estiver publicada, iniciar a Fase 4 - App desktop e catalogo:

- reler a arvore obrigatoria completa;
- resolver a pendencia do template WinUI, que ainda nao esta disponivel;
- preparar o ambiente/template WinUI ou registrar ADR para alternativa de scaffold;
- criar `StudyLab.Desktop`;
- compor DI com Application e Infrastructure;
- criar fluxo inicial de catalogo usando `IStudyLibraryRepository`;
- manter UI sem regra de negocio e com view models testaveis.

## Pendencias praticas

- Template WinUI ainda nao esta disponivel: `dotnet new list winui` retornou nenhum modelo encontrado.
- Antes da Fase 4, preparar o ambiente/template WinUI ou registrar ADR para alternativa de scaffold.
- Empacotamento WinUI ainda precisa de ADR.
- Framework MVVM/toolkit ainda precisa de decisao futura.

## Decisoes recentes

- .NET 10 adotado como base inicial.
- Arquitetura limpa definida com Domain, Application, Infrastructure e Desktop.
- WinUI 3 definido como UI planejada para Windows, condicionado a preparacao do ambiente.
- TDD definido como fluxo padrao para comportamento de negocio.
- xUnit definido como framework de testes unitarios.
- Importacao inicial definida como arvore flexivel de rascunho em Application; ver `docs/decisions/ADR-0002-imported-course-tree.md`.
- Persistencia local inicial definida como JSON com `System.Text.Json`; ver `docs/decisions/ADR-0003-local-json-persistence.md`.
- Security by design definido como requisito permanente.
- `docs/planning/project-state.md` definido como marcador operacional de continuidade.

## Verificacoes feitas

- `git status --short --branch` antes desta etapa: `main...origin/main`, sem alteracoes.
- `dotnet test .\StudyLab.slnx` confirmou o Red inicial por tipos ausentes em Persistence.
- `dotnet test .\StudyLab.slnx` executado apos implementacao com 25 testes aprovados.
- `dotnet build .\StudyLab.slnx` executado com sucesso, 0 avisos e 0 erros.
- O ciclo TDD foi seguido: testes criados primeiro, falha inicial confirmada, implementacao minima adicionada e suite aprovada.
- Apos commit/push desta etapa, `main` deve permanecer sincronizada com `origin/main`.

## Criterio para continuar

Ao receber o pedido "leia o harness e continue a implementacao", o agente deve:

1. Ler a arvore obrigatoria do `HARNESS.md`, incluindo este arquivo.
2. Identificar a proxima etapa executavel neste documento.
3. Validar `git status`.
4. Executar a proxima etapa em escopo pequeno.
5. Atualizar este arquivo ao concluir.
6. Commitar e enviar ao remoto quando a entrega estiver consistente.
