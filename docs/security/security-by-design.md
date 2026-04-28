# Security by design - Study Lab

## Objetivo

O Study Lab manipula arquivos locais do usuario, caminhos de pastas, videos, documentos e historico de estudo. Esses dados podem revelar informacoes privadas. A seguranca deve ser parte do design desde o primeiro teste.

## Principios

- Offline-first e privacidade por padrao.
- Menor privilegio em acesso a arquivos e dados.
- Validacao de todos os caminhos e entradas do usuario.
- Nenhuma execucao automatica de arquivos importados.
- Nenhum segredo, dado pessoal, curso, video, banco local ou material privado no repositorio.
- Logs sem dados sensiveis quando possivel.
- Dependencias externas minimas, verificadas e justificadas.

## Riscos principais

- Path traversal ao importar ou abrir arquivos.
- Links simbolicos ou atalhos apontando para locais inesperados.
- Arquivos com extensoes falsas ou nomes maliciosos.
- Vazamento de caminhos locais em logs, erros, prints ou commits.
- Corrupcao de progresso por falha de escrita.
- Execucao involuntaria de documentos ou links externos.
- Backup contendo dados sensiveis sem protecao.
- Dependencias de terceiros vulneraveis.

## Controles obrigatorios

- Canonicalizar e validar caminhos antes de usar.
- Trabalhar em modo somente leitura durante a importacao de cursos.
- Usar allowlist de extensoes para video e materiais.
- Nao seguir links simbolicos sem decisao explicita e teste.
- Nunca executar arquivos importados; apenas abrir com fluxo controlado pelo app.
- Sanitizar nomes exibidos e armazenados.
- Tratar caminhos locais como dados potencialmente sensiveis.
- Persistir progresso com operacoes atomicas quando possivel.
- Usar Secret Manager ou armazenamento seguro quando houver segredos de desenvolvimento.
- Bloquear commit de `.env`, bancos locais, videos, materiais privados e artefatos.

## Requisitos para novas funcionalidades

Toda funcionalidade nova deve responder:

- Quais dados sensiveis toca?
- Que entradas nao confiaveis recebe?
- Que abuso ou erro humano pode acontecer?
- Que teste prova o comportamento seguro?
- Que log pode vazar informacao?
- Que arquivo novo pode acabar no git indevidamente?

## Checklist antes de commit

- `git status` revisado.
- Nenhum arquivo de curso, video, PDF privado, banco local ou segredo rastreado.
- Testes de caminho/entrada maliciosa adicionados quando aplicavel.
- Dependencias novas justificadas e registradas.
- Erros nao revelam detalhes internos desnecessarios.

