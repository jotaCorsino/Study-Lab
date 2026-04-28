# ADR-0002 - Importacao como arvore flexivel de rascunho

## Status

Aceito

## Contexto

A importacao de cursos precisa interpretar pastas locais com estruturas diferentes. O briefing original cita uma estrutura ideal com curso, materia, modulo, topico e aula, mas tambem exige flexibilidade para organizacoes variadas.

O dominio inicial possui `Course`, `CourseModule`, `Topic` e `Lesson`. A entidade `Lesson` exige duracao positiva, mas a importacao segura de arquivos ainda nao extrai metadados reais de video.

## Decisao

A Fase 2 importa a pasta para uma arvore flexivel de rascunho na camada Application:

- `ImportedCourse`
- `ImportedCourseItem`
- `ImportedCourseItemType`

O caso de uso `ImportCourseFromFolderUseCase` monta essa arvore a partir de caminhos relativos seguros fornecidos pela porta `ICourseFolderReader`.

A conversao para entidades finais de dominio deve acontecer em uma etapa posterior, quando houver metadados suficientes, persistencia e fluxo de revisao/edicao pelo usuario.

## Consequencias

Beneficios:

- Preserva estruturas de pasta com profundidade variavel.
- Evita criar aulas finais com duracao falsa ou desconhecida.
- Mantem a importacao testavel sem depender de player, codec ou metadados de video.
- Mantem o dominio protegido de detalhes do sistema de arquivos.

Custos:

- Havera uma etapa futura de confirmacao/conversao do rascunho importado para curso persistido.
- A UI precisara representar uma arvore importada antes de salvar o curso definitivo.

