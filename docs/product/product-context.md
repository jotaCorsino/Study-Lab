# Contexto de produto - Study Lab

## Visao

Study Lab e um aplicativo local/offline-first para Windows que ajuda o usuario a transformar cursos em video armazenados no computador em uma experiencia organizada de estudo.

O app deve importar pastas, organizar cursos, reproduzir aulas, registrar progresso, controlar metas diarias, exibir historico mensal, permitir abono com horas extras e centralizar materiais complementares.

## Problema

Cursos baixados ou comprados frequentemente ficam espalhados em pastas, com nomes inconsistentes e sem controle de progresso. O usuario perde tempo tentando lembrar onde parou, quais aulas concluiu e se cumpriu sua rotina de estudos.

## Resultado esperado

O usuario abre o app, encontra seus cursos, continua a aula atual, acompanha sua rotina e enxerga progresso real sem depender de internet ou de uma plataforma externa.

## Capacidades principais

- Catalogo de cursos locais.
- Importacao de curso por pasta.
- Organizacao manual de cursos, modulos, topicos e aulas.
- Player integrado de video.
- Aula anterior, proxima aula, velocidade de reproducao e intro skip.
- Progresso por aula, modulo e curso.
- Meta diaria por curso.
- Calendario mensal de desempenho.
- Credito de horas extras e abono mensal.
- Historico anual.
- Materiais complementares como PDFs, documentos, links e notas.
- Configuracoes de tema, reproducao, diretorio padrao e backup futuro.

## MVP proposto

1. Fundacao do repositorio, arquitetura e testes.
2. Modelo de dominio para curso, modulo, topico, aula, progresso e rotina.
3. Importacao segura de uma pasta local em modo somente leitura.
4. Persistencia local basica de cursos e progresso.
5. Catalogo de cursos e tela de detalhes.
6. Reproducao de video local e registro de progresso.
7. Meta diaria, calendario mensal e calculo de credito/abono.

## Fora do MVP inicial

- Sincronizacao em nuvem.
- Conta de usuario online.
- Marketplace de cursos.
- Streaming remoto.
- DRM.
- Compartilhamento publico de materiais.

## Fonte original

O briefing completo original esta em `study-lab-app.md`.

