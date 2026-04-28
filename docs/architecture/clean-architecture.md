# Arquitetura limpa - Study Lab

## Objetivo

Manter o Study Lab simples, testavel e sustentavel. A arquitetura deve permitir que regras de estudo, progresso, calendario, abono e importacao sejam evoluidas sem acoplar o dominio a UI, banco de dados, sistema de arquivos ou detalhes do Windows.

## Direcao arquitetural

Usaremos Clean Architecture com dependencias apontando para dentro:

```text
StudyLab.Desktop         -> Application
StudyLab.Infrastructure  -> Application, Domain
StudyLab.Application     -> Domain
StudyLab.Domain          -> nenhuma camada do app
```

## Projetos previstos

- `StudyLab.Domain`: entidades, value objects, regras de negocio puras, eventos de dominio e erros de dominio.
- `StudyLab.Application`: casos de uso, portas, DTOs internos, validacoes de aplicacao e orquestracao.
- `StudyLab.Infrastructure`: adaptadores para sistema de arquivos, persistencia local, relogio, configuracao, logs e servicos externos quando existirem.
- `StudyLab.Desktop`: WinUI 3, shell, navegacao, view models, recursos visuais e composicao de DI.

## Testes previstos

- `StudyLab.Domain.Tests`: regras puras e calculos de negocio.
- `StudyLab.Application.Tests`: casos de uso com portas fake/in-memory.
- `StudyLab.Infrastructure.Tests`: adaptadores reais ou controlados, especialmente sistema de arquivos e persistencia.
- `StudyLab.Desktop.Tests`: view models e logica de apresentacao sem depender da UI real.
- `StudyLab.Architecture.Tests`: regras de dependencia entre camadas.

## Principios SOLID aplicados

- Single Responsibility: cada classe deve ter um motivo claro para mudar.
- Open/Closed: novas formas de importar, persistir ou exibir dados devem entrar por extensao, nao por reescrita ampla.
- Liskov Substitution: portas e implementacoes devem preservar contratos e invariantes.
- Interface Segregation: casos de uso dependem de interfaces pequenas e especificas.
- Dependency Inversion: dominio e aplicacao nao dependem de infraestrutura.

## Regras de dependencia

- Domain nao referencia Application, Infrastructure, Desktop, WinUI, EF, SQLite ou sistema de arquivos.
- Application define portas e casos de uso; Infrastructure implementa portas.
- Desktop chama casos de uso; nao contem regra de negocio.
- Infrastructure nao deve vazar tipos tecnicos para Domain.
- Conversoes entre modelos de UI, DTOs e dominio devem ser explicitas.

## Padrao de implementacao

1. Escrever teste de dominio ou aplicacao.
2. Criar comportamento no projeto mais interno possivel.
3. Criar porta quando houver dependencia externa.
4. Implementar adaptador na Infrastructure.
5. Expor pela UI somente depois do caso de uso estar coberto.

## Decisoes pendentes

- Persistencia local: SQLite, arquivos JSON estruturados ou outra opcao.
- Framework MVVM e toolkit: usar somente se reduzir codigo repetitivo real.
- Empacotamento WinUI: packaged ou unpackaged.
- Estrategia de backup/restauracao.

Cada decisao relevante deve gerar ADR em `docs/decisions/`.

