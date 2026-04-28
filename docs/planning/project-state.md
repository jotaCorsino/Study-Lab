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
- Fase atual: Fase 2 - Importacao segura de pastas.
- Status da fase: concluida no recorte inicial.
- Ultima implementacao concluida: caso de uso de importacao, porta de leitura segura e adaptador local de sistema de arquivos.
- Commit publicado mais recente antes desta etapa: `dab9a3b feat: add initial domain model and tests`.
- Branch atual: `main`.
- Remoto oficial: `origin` em `https://github.com/jotaCorsino/Study-Lab.git`.

## Ultima etapa concluida

Importacao segura de pastas:

- `src/StudyLab.Application` criado.
- `src/StudyLab.Infrastructure` criado.
- `tests/StudyLab.Application.Tests` criado.
- `tests/StudyLab.Infrastructure.Tests` criado.
- `ImportCourseFromFolderUseCase` criado para montar uma arvore importada a partir de caminhos relativos seguros.
- `ICourseFolderReader` criado como porta de leitura de pasta.
- `LocalCourseFolderReader` criado como adaptador local em modo somente leitura.
- Allowlist inicial de videos criada: `.mp4`, `.mkv`, `.avi`, `.mov`, `.wmv`, `.webm`, `.flv`, `.m4v`.
- `SafeRelativePath` criado para bloquear caminhos fora da raiz selecionada.
- Arquivos com extensao nao permitida sao rejeitados com motivo controlado.
- ADR-0002 criada para registrar a importacao como arvore flexivel de rascunho.

## Historico imediato

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

Depois que esta etapa estiver publicada, iniciar a Fase 3 - Persistencia local:

- reler a arvore obrigatoria completa;
- criar ADR escolhendo a tecnologia de persistencia local;
- definir portas de repositorio em Application;
- escrever testes primeiro para salvar e carregar catalogo/progresso sem acessar dados privados;
- implementar adaptador inicial em Infrastructure;
- garantir escrita segura/atomica ou registrar limite conhecido se o primeiro recorte ainda nao cobrir isso.

## Pendencias praticas

- Template WinUI ainda nao esta disponivel: `dotnet new list winui` retornou nenhum modelo encontrado.
- Antes da Fase 4, preparar o ambiente/template WinUI ou registrar ADR para alternativa de scaffold.
- Persistencia local ainda precisa de ADR.
- Empacotamento WinUI ainda precisa de ADR.
- Framework MVVM/toolkit ainda precisa de decisao futura.

## Decisoes recentes

- .NET 10 adotado como base inicial.
- Arquitetura limpa definida com Domain, Application, Infrastructure e Desktop.
- WinUI 3 definido como UI planejada para Windows, condicionado a preparacao do ambiente.
- TDD definido como fluxo padrao para comportamento de negocio.
- xUnit definido como framework de testes unitarios.
- Importacao inicial definida como arvore flexivel de rascunho em Application; ver `docs/decisions/ADR-0002-imported-course-tree.md`.
- Security by design definido como requisito permanente.
- `docs/planning/project-state.md` definido como marcador operacional de continuidade.

## Verificacoes feitas

- `git status --short --branch` antes desta etapa: `main...origin/main`, sem alteracoes.
- `dotnet test .\StudyLab.slnx` confirmou o Red inicial por tipos ausentes em Application/Infrastructure.
- `dotnet test .\StudyLab.slnx` executado apos implementacao com 17 testes aprovados.
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
