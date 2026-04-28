# HARNESS - Study Lab

Este arquivo e o ponto de entrada obrigatorio para qualquer implementacao, revisao, refatoracao, correcao, teste, publicacao ou decisao tecnica neste repositorio.

Antes de executar qualquer tarefa, leia a arvore abaixo na ordem indicada. Se a tarefa alterar arquitetura, seguranca, testes, escopo do produto ou versionamento, atualize tambem o documento correspondente.

## Arvore de leitura obrigatoria

1. `HARNESS.md`
2. `.agents/rules/project-context.md`
3. `docs/product/product-context.md`
4. `docs/architecture/clean-architecture.md`
5. `docs/security/security-by-design.md`
6. `docs/testing/tdd-strategy.md`
7. `docs/process/versioning-and-repository.md`
8. `docs/planning/implementation-plan.md`
9. `docs/planning/project-state.md`

O briefing original do produto fica em `docs/product/study-lab-app.md` e deve ser consultado quando houver duvida sobre comportamento esperado, linguagem de produto ou escopo funcional.

## Regras de execucao

- Comece sempre pelo entendimento do problema e pelos criterios de aceite.
- Trabalhe em passos pequenos, verificaveis e versionaveis.
- Use TDD por padrao: teste falhando, implementacao minima, refatoracao.
- Preserve uma arquitetura simples, limpa e orientada a manutencao.
- Mantenha regras de negocio fora da UI, infraestrutura e frameworks.
- Aplique security by design desde o primeiro teste.
- Nao commite segredos, dados locais, videos, materiais privados, bancos locais ou artefatos gerados.
- Prefira dependencias nativas da plataforma .NET. Adicione bibliotecas externas apenas com justificativa tecnica.
- Cada mudanca relevante deve ser acompanhada de teste, documentacao ou justificativa clara quando teste automatizado ainda nao for possivel.

## Gates obrigatorios

### Gate 0 - Leitura

Confirme mentalmente que a arvore de leitura foi consultada e que a tarefa esta coerente com o produto.

### Gate 1 - Design

Defina o menor recorte entregavel. Identifique entidade, caso de uso, porta, adaptador, UI, persistencia e risco de seguranca envolvidos.

### Gate 2 - Teste primeiro

Crie ou atualize testes antes da implementacao sempre que o comportamento puder ser automatizado.

### Gate 3 - Implementacao

Implemente o minimo necessario, mantendo dependencias apontando para dentro da arquitetura limpa.

### Gate 4 - Verificacao

Execute testes, build e verificacoes relevantes. Registre qualquer teste nao executado e o motivo.

### Gate 5 - Versionamento

Revise `git status`, garanta que nao ha arquivos sensiveis, faca commit pequeno e envie para o remoto quando a entrega estiver consistente.

### Gate 6 - Estado do projeto

Atualize `docs/planning/project-state.md` ao fim de cada etapa concluida, antes do commit/push, registrando a ultima entrega, a proxima etapa executavel, verificacoes feitas e pendencias praticas.

## Pilha tecnica inicial

- Plataforma: .NET 10.
- Linguagem: C#.
- Aplicativo alvo: Windows desktop.
- UI planejada: WinUI 3 com Windows App SDK.
- Arquitetura: Clean Architecture com Domain, Application, Infrastructure e Desktop.
- Testes: unitarios primeiro; integracao e UI conforme a funcionalidade amadurecer.
- Dados: local-first/offline-first, com persistencia local a definir por ADR antes da implementacao.
