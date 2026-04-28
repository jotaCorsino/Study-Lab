# ADR-0003 - Persistencia local inicial em JSON

## Status

Aceito

## Contexto

O Study Lab e offline-first e precisa salvar catalogo, progresso e preferencias localmente. O projeto ainda esta antes da UI e antes de decisoes de empacotamento Windows. Tambem existe uma regra permanente de evitar dependencias externas sem justificativa forte.

## Decisao

Usar um arquivo JSON local como persistencia inicial, implementado com `System.Text.Json` e uma porta em Application:

- `IStudyLibraryRepository`
- `StudyLibrarySnapshot`
- `CourseCatalogEntry`
- `LessonProgressEntry`
- `StudyPreferences`

A Infrastructure fornece `JsonStudyLibraryRepository`, que salva o snapshot em arquivo local com escrita em arquivo temporario no mesmo diretorio e substituicao do arquivo final.

O arquivo JSON e dado local do app e nao deve ser versionado.

## Consequencias

Beneficios:

- Sem dependencia externa neste momento.
- Facil de testar e inspecionar durante o desenvolvimento inicial.
- Bom encaixe para o MVP local-first antes da UI e do empacotamento.
- Permite evoluir o contrato de persistencia por schema version.

Custos:

- Nao e ideal para consultas complexas quando o volume de dados crescer.
- Pode exigir migracao futura para SQLite ou outro armazenamento estruturado.
- Caminhos locais armazenados devem continuar sendo tratados como dados sensiveis.

