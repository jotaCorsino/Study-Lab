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
- Fase atual: Fase 0 - Fundacao do repositorio.
- Status da fase: concluida apos a publicacao desta etapa de continuidade.
- Ultima implementacao concluida: documento vivo de estado do projeto, regra de continuidade e leitura obrigatoria atualizadas.
- Commit publicado mais recente antes da regra de continuidade: `4ea56bf docs: bootstrap study lab foundation`.
- Branch atual: `main`.
- Remoto oficial: `origin` em `https://github.com/jotaCorsino/Study-Lab.git`.

## Ultima etapa concluida

Documento vivo de continuidade:

- `docs/planning/project-state.md` criado como marcador operacional.
- `HARNESS.md` atualizado para incluir o estado vivo na leitura obrigatoria.
- `.agents/rules/project-context.md`, `docs/README.md`, `docs/process/versioning-and-repository.md` e `docs/planning/implementation-plan.md` atualizados com a regra de continuidade.
- Fase 0 marcada como concluida para permitir iniciar a Fase 1 na proxima execucao.

## Historico imediato

Fundacao inicial do repositorio publicada no commit `4ea56bf docs: bootstrap study lab foundation`:

- documentacao obrigatoria criada;
- briefing original movido para `docs/product/study-lab-app.md`;
- pastas `src`, `tests`, `docs`, `build`, `tools`, `.agents` e `.github` organizadas;
- higiene de Git e configuracao .NET criadas;
- `main` publicado em `origin/main`.

## Proxima etapa executavel

Depois que esta etapa estiver publicada, iniciar a Fase 1 - Dominio e testes iniciais:

- reler a arvore obrigatoria completa;
- criar a solucao .NET e os projetos iniciais de dominio/testes;
- escolher e configurar o framework de testes;
- escrever os primeiros testes de dominio para progresso, meta diaria, credito mensal e abono;
- implementar o minimo de dominio necessario para passar nos testes.

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
- Security by design definido como requisito permanente.
- `docs/planning/project-state.md` definido como marcador operacional de continuidade.

## Verificacoes feitas

- `git status --short --branch` antes desta etapa: `main...origin/main`, sem alteracoes.
- `dotnet --info` confirmou SDK .NET 10.0.202 instalado.
- `dotnet new list winui` confirmou que o template WinUI ainda nao esta disponivel.
- Esta etapa e documental; testes .NET nao sao necessarios.
- Apos commit/push desta etapa, `main` deve permanecer sincronizada com `origin/main`.

## Criterio para continuar

Ao receber o pedido "leia o harness e continue a implementacao", o agente deve:

1. Ler a arvore obrigatoria do `HARNESS.md`, incluindo este arquivo.
2. Identificar a proxima etapa executavel neste documento.
3. Validar `git status`.
4. Executar a proxima etapa em escopo pequeno.
5. Atualizar este arquivo ao concluir.
6. Commitar e enviar ao remoto quando a entrega estiver consistente.
