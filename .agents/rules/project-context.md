# Contexto obrigatorio do projeto

Este arquivo resume o contexto operacional que deve ser lido antes de qualquer tarefa no Study Lab.

## Produto

Study Lab e um aplicativo de estudos local/offline-first para transformar pastas de cursos em video em uma experiencia organizada, acompanhavel e produtiva.

O produto deve permitir importar cursos locais, organizar aulas, assistir videos, salvar progresso, configurar metas diarias, acompanhar historico mensal, usar credito de horas extras para abono, manter materiais complementares e continuar exatamente de onde o usuario parou.

## Direcao tecnica

- Usar .NET moderno, inicialmente .NET 10.
- Construir primeiro para Windows desktop.
- Manter o dominio independente de UI, banco de dados, sistema de arquivos e bibliotecas de terceiros.
- Favorecer codigo simples, legivel, testavel e facil de alterar.
- Usar SOLID com pragmatismo: clareza antes de abstracao excessiva.
- Tratar seguranca, privacidade e integridade de dados como requisitos de produto, nao como etapa final.

## Estrutura esperada

```text
.
|-- HARNESS.md
|-- README.md
|-- docs/
|   |-- architecture/
|   |-- decisions/
|   |-- planning/
|   |-- process/
|   |-- product/
|   |-- security/
|   `-- testing/
|-- src/
|   |-- StudyLab.Domain/
|   |-- StudyLab.Application/
|   |-- StudyLab.Infrastructure/
|   `-- StudyLab.Desktop/
|-- tests/
|   |-- StudyLab.Domain.Tests/
|   |-- StudyLab.Application.Tests/
|   |-- StudyLab.Infrastructure.Tests/
|   |-- StudyLab.Desktop.Tests/
|   `-- StudyLab.Architecture.Tests/
|-- tools/
`-- build/
```

Pastas de codigo podem ser criadas incrementalmente. Pastas sem implementacao ainda documentam a direcao arquitetural e nao devem receber codigo fora de sua responsabilidade.

## Regras permanentes

- Antes de implementar, consulte a arvore de leitura em `HARNESS.md`.
- Use `docs/planning/project-state.md` para localizar a fase atual, a ultima etapa concluida e a proxima etapa executavel.
- Atualize `docs/planning/project-state.md` ao concluir cada etapa, antes de commit e push.
- Antes de criar uma dependencia, explique o problema que ela resolve.
- Antes de mexer em arquivos locais do usuario, valide caminho, extensao, permissao e intencao.
- Antes de persistir dados, defina quais dados sao sensiveis e como serao protegidos.
- Antes de enviar ao remoto, revise arquivos rastreados e ignore dados privados.
