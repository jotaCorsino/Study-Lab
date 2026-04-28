# Versionamento e repositorio

## Remoto oficial

Repositorio remoto:

```text
https://github.com/jotaCorsino/Study-Lab.git
```

## Estrategia de branch

- `main` deve refletir uma base consistente e verificavel.
- Trabalhos maiores devem usar branches `feature/<descricao-curta>`.
- Correcoes urgentes devem usar `hotfix/<descricao-curta>`.
- Evitar commits grandes com muitas responsabilidades.

## Commits

Usar Conventional Commits quando possivel:

- `docs: ...`
- `test: ...`
- `feat: ...`
- `fix: ...`
- `refactor: ...`
- `chore: ...`
- `build: ...`
- `ci: ...`

## Regras antes de push

1. Ler `HARNESS.md`.
2. Revisar `git status`.
3. Atualizar `docs/planning/project-state.md` quando uma etapa tiver sido concluida.
4. Confirmar que nao ha segredos, dados locais, videos, PDFs privados, bancos locais ou artefatos gerados.
5. Executar testes/build relevantes.
6. Fazer commit pequeno e descritivo.
7. Enviar para o remoto oficial.

## Arquivos que nao devem ir para o remoto

- `.env`, `.env.*`, segredos e credenciais.
- Bancos locais reais: `*.db`, `*.sqlite`, `*.sqlite3`.
- Videos e midias de cursos: `*.mp4`, `*.mkv`, `*.avi`, `*.mov`, `*.wmv`, `*.webm`.
- Materiais privados importados pelo usuario.
- Pastas de dados locais do app.
- `bin/`, `obj/`, `.vs/`, `TestResults/`, cobertura e logs.
- Backups locais contendo dados pessoais.

## Documentos permitidos

Documentacao tecnica, ADRs, planos, checklists e exemplos sem dados sensiveis podem ser versionados.

Documentos com caminhos reais do usuario, nomes de cursos privados, materiais pagos, credenciais, tokens, chaves ou dados pessoais nao devem ser versionados.
