# Plano de implementacao - Study Lab

Este plano organiza a construcao incremental do Study Lab usando .NET, Clean Architecture, TDD e security by design.

## Fase 0 - Fundacao do repositorio

Objetivo: preparar a base de trabalho.

Status: concluida apos a publicacao da etapa de continuidade. A fundacao inicial ja foi publicada no commit `4ea56bf docs: bootstrap study lab foundation`; a continuidade operacional passa a ser acompanhada em `docs/planning/project-state.md`.

Entregas:

- HARNESS obrigatorio.
- Documentacao de produto, arquitetura, seguranca, TDD e versionamento.
- Git inicializado e remoto configurado.
- Higiene de `.gitignore`, `.editorconfig`, `global.json` e arquivos .NET compartilhados.
- Documento vivo de estado do projeto incluido na leitura obrigatoria.

Validacao:

- `git status` limpo apos commit.
- Remoto configurado para `https://github.com/jotaCorsino/Study-Lab.git`.

## Fase 1 - Dominio e testes iniciais

Objetivo: modelar regras centrais sem UI e sem infraestrutura.

Status: concluida no recorte inicial. A solucao, o projeto de dominio, o projeto de testes xUnit e as primeiras regras puras foram criados; detalhes operacionais ficam em `docs/planning/project-state.md`.

Entregas:

- Projetos `StudyLab.Domain` e `StudyLab.Domain.Tests`.
- Entidades e value objects para Course, Module, Topic, Lesson, StudySession, Goal, MonthlyCredit e Progress.
- Testes para progresso, meta diaria, credito mensal e abono.

Seguranca:

- Invariantes para impedir estados invalidos.
- Validacao de duracoes, datas, percentuais e identificadores.

## Fase 2 - Importacao segura de pastas

Objetivo: transformar uma pasta local em estrutura de curso.

Status: concluida no recorte inicial. Application possui caso de uso, porta e arvore de importacao; Infrastructure possui leitor local com allowlist, caminhos relativos seguros e rejeicoes controladas.

Entregas:

- Caso de uso de importacao em `StudyLab.Application`.
- Porta para leitura de sistema de arquivos.
- Adaptador em `StudyLab.Infrastructure`.
- Testes com estruturas variadas de pasta.

Seguranca:

- Canonicalizacao de caminhos.
- Allowlist de extensoes.
- Bloqueio de traversal e links simbolicos sem permissao explicita.
- Importacao em modo somente leitura.

## Fase 3 - Persistencia local

Objetivo: salvar catalogo, progresso e configuracoes localmente.

Status: concluida no recorte inicial. Application possui porta e snapshot de biblioteca; Infrastructure possui repositorio JSON local com escrita atomica.

Entregas:

- ADR escolhendo tecnologia de persistencia.
- Repositorios/adaptadores em Infrastructure.
- Testes de leitura, escrita, migracao e recuperacao de falha.

Seguranca:

- Separar dados de app de arquivos de curso.
- Escrita atomica quando possivel.
- Nao vazar caminhos sensiveis em logs.

## Fase 4 - App desktop e catalogo

Objetivo: criar shell Windows e fluxo basico de catalogo.

Status: em andamento. O scaffold WinUI, a camada `StudyLab.Desktop.Presentation`, os view models testaveis, o catalogo inicial, o fluxo basico de importacao pela UI e a tela de detalhes do curso ja foram criados. A decisao de scaffold/empacotamento esta registrada em `docs/decisions/ADR-0004-winui-scaffold-and-packaging.md`.

Entregas:

- Projeto `StudyLab.Desktop` com WinUI 3. Concluido no recorte inicial.
- Projeto `StudyLab.Desktop.Presentation` para view models testaveis. Concluido no recorte inicial.
- Navegacao principal. Iniciada com shell, `MainPage` e navegacao para `CourseDetailPage`.
- Catalogo de cursos. Iniciado com listagem e atualizacao apos importacao.
- Tela de detalhes do curso. Iniciada com arvore visual da estrutura importada.
- Tela de criacao/importacao. Iniciada com `FolderPicker` controlado e persistencia no catalogo.
- View models testaveis. Iniciados em `StudyLab.Desktop.Presentation`.

Seguranca:

- Dialogos de selecao de pasta controlados.
- Mensagens de erro sem expor stack traces ou dados sensiveis.

## Fase 5 - Player e progresso

Objetivo: assistir aulas e registrar progresso.

Entregas:

- Tela de aula.
- Player local.
- Aula anterior/proxima.
- Velocidade de reproducao.
- Intro skip por curso.
- Marcacao automatica/manual de conclusao.

Seguranca:

- Abrir somente arquivos permitidos dentro do curso importado.
- Tratar arquivo ausente, movido ou renomeado sem quebrar o app.

## Fase 6 - Metas, calendario e abono

Objetivo: entregar rotina de estudos.

Entregas:

- Configuracao de dias e tempo diario por curso.
- Calendario mensal.
- Calculo de percentual da meta.
- Credito mensal de horas extras.
- Abono de pendencias anteriores.
- Historico anual.

Seguranca:

- Regras deterministicas e testadas para datas, fuso horario e virada de mes.

## Fase 7 - Materiais complementares

Objetivo: organizar arquivos auxiliares e links.

Entregas:

- Cadastro de PDFs, documentos, links e notas.
- Associacao por curso/modulo/topico.
- Organizacao manual.

Seguranca:

- Allowlist de tipos.
- Confirmacao antes de abrir links externos.
- Nao copiar materiais privados para o repositorio.

## Fase 8 - Configuracoes, backup e restauracao

Objetivo: dar controle local ao usuario.

Entregas:

- Tema claro/escuro.
- Preferencias de reproducao.
- Diretorio padrao.
- Backup/restauracao local.

Seguranca:

- ADR para formato de backup.
- Aviso claro sobre dados incluidos no backup.
- Opcao futura de protecao/criptografia.

## Fase 9 - Hardening e release

Objetivo: estabilizar para uso real.

Entregas:

- Testes de regressao.
- Testes de arquitetura.
- Revisao de acessibilidade.
- Revisao de performance.
- Pipeline CI.
- Empacotamento Windows.

Seguranca:

- Revisao de dependencias.
- Checklist de privacidade.
- Validacao de logs e artefatos de build.
