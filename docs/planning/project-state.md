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
- Fase atual: Fase 1 - Dominio e testes iniciais.
- Status da fase: concluida no recorte inicial.
- Ultima implementacao concluida: solucao .NET, projeto de dominio, projeto de testes xUnit e primeiras regras puras de estudo.
- Commit publicado mais recente antes desta etapa: `f4379b0 docs: add project state tracking`.
- Branch atual: `main`.
- Remoto oficial: `origin` em `https://github.com/jotaCorsino/Study-Lab.git`.

## Ultima etapa concluida

Dominio e testes iniciais:

- `StudyLab.slnx` criado.
- `src/StudyLab.Domain` criado como biblioteca `net10.0`.
- `tests/StudyLab.Domain.Tests` criado com xUnit e referencia para o dominio.
- Central Package Management atualizado com pacotes de teste.
- `.gitignore` ajustado para proteger pastas locais de cursos sem ignorar namespaces/pastas de codigo como `Courses`.
- Modelos iniciais criados para curso, modulo, topico, aula, sessao, progresso, meta diaria e credito mensal.
- Testes criados para progresso de aula, meta diaria, credito/abono mensal e estrutura hierarquica de curso.

## Historico imediato

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

Depois que esta etapa estiver publicada, iniciar a Fase 2 - Importacao segura de pastas:

- reler a arvore obrigatoria completa;
- criar `StudyLab.Application` e `StudyLab.Infrastructure`;
- definir o caso de uso de importacao em Application;
- definir porta de leitura segura de arquivos/pastas;
- escrever testes primeiro para extensoes permitidas, estrutura de pasta e bloqueio de saida da raiz;
- implementar adaptador de sistema de arquivos em Infrastructure em modo somente leitura.

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
- Security by design definido como requisito permanente.
- `docs/planning/project-state.md` definido como marcador operacional de continuidade.

## Verificacoes feitas

- `git status --short --branch` antes desta etapa: `main...origin/main`, sem alteracoes.
- `dotnet test .\StudyLab.slnx` executado com 11 testes aprovados.
- `dotnet build .\StudyLab.slnx` executado com sucesso, 0 avisos e 0 erros.
- `git status --ignored --short` usado para confirmar que `bin/` e `obj/` ficam ignorados e que codigo-fonte nao fica oculto pelo `.gitignore`.
- O ciclo TDD foi seguido: testes criados primeiro, falha inicial confirmada por tipos ausentes, implementacao minima adicionada e suite aprovada.
- Apos commit/push desta etapa, `main` deve permanecer sincronizada com `origin/main`.

## Criterio para continuar

Ao receber o pedido "leia o harness e continue a implementacao", o agente deve:

1. Ler a arvore obrigatoria do `HARNESS.md`, incluindo este arquivo.
2. Identificar a proxima etapa executavel neste documento.
3. Validar `git status`.
4. Executar a proxima etapa em escopo pequeno.
5. Atualizar este arquivo ao concluir.
6. Commitar e enviar ao remoto quando a entrega estiver consistente.
