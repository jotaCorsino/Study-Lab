# Estrategia de TDD - Study Lab

## Regra principal

Para comportamento de negocio, escreva o teste antes da implementacao. O ciclo padrao e:

1. Red: criar um teste que falha pelo motivo certo.
2. Green: implementar o minimo necessario.
3. Refactor: melhorar design sem alterar comportamento.

## Prioridade de testes

1. Domain: regras puras de curso, aula, progresso, metas, credito e abono.
2. Application: casos de uso como importar curso, continuar aula, registrar progresso e calcular calendario.
3. Infrastructure: leitura segura de pastas, persistencia local e adaptadores.
4. Desktop: view models, navegacao e estados de UI testaveis sem depender da janela real.
5. UI/end-to-end: fluxos criticos quando a base estiver estavel.

## Regras para bons testes

- Nome do teste descreve comportamento e condicao.
- Cada teste valida uma ideia central.
- Evitar depender de relogio real; usar porta de tempo.
- Evitar depender do sistema de arquivos real em teste unitario; usar fake ou fixture controlada.
- Testes de infraestrutura devem usar diretorios temporarios isolados.
- Testes de seguranca devem cobrir caminhos maliciosos, extensoes inesperadas e entradas vazias.

## Casos iniciais de alto valor

- Importador reconhece videos em estrutura Curso/Materia/Modulo/Topico/Aula.
- Importador ignora extensoes nao permitidas.
- Importador nao sai da raiz selecionada.
- Progresso marca aula concluida ao atingir criterio definido.
- Meta diaria calcula percentual por tempo real assistido.
- Credito mensal acumula excedente apenas no mes atual.
- Credito nao compensa o dia atual.
- Abono preserva cor original e adiciona marcador visual/logico.
- Historico mensal considera dias abonados como meta cumprida.

## Ferramentas planejadas

- Test framework .NET a definir no primeiro scaffold de testes.
- Preferencia inicial: xUnit para testes unitarios e de aplicacao.
- Testes de arquitetura podem ser feitos por reflexao propria antes de adicionar bibliotecas.
- Playwright ou equivalente so quando houver fluxos de UI que justifiquem automacao.

## Criterio de pronto

Uma mudanca so deve ser considerada pronta quando:

- testes relevantes foram escritos ou atualizados;
- build passa;
- comportamento foi validado manualmente quando envolver UI;
- riscos de seguranca foram revisados;
- documentacao ou ADR foi atualizada se a decisao mudou.

