# ADR-0001 - Stack inicial e arquitetura

## Status

Aceito

## Contexto

O Study Lab sera um aplicativo de estudos local/offline-first, inicialmente para Windows, construido com ferramentas da plataforma .NET. O produto precisa manipular arquivos locais com seguranca, manter progresso de estudo e evoluir com baixo custo de manutencao.

## Decisao

Usar .NET 10 como base inicial e organizar o codigo em Clean Architecture:

- `StudyLab.Domain`
- `StudyLab.Application`
- `StudyLab.Infrastructure`
- `StudyLab.Desktop`

A UI planejada para Windows e WinUI 3 com Windows App SDK. A implementacao deve comecar por testes de dominio e aplicacao antes da UI.

## Consequencias

Beneficios:

- Regras de negocio ficam independentes da UI e infraestrutura.
- Testes podem nascer antes da interface.
- Trocas futuras de persistencia ou player ficam mais controladas.

Custos:

- Mais projetos e disciplina de dependencia desde o inicio.
- Algumas decisoes, como persistencia e empacotamento WinUI, ainda precisam de ADR propria.

