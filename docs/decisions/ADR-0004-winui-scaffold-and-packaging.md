# ADR-0004 - Scaffold WinUI e empacotamento inicial

## Status

Aceito

## Contexto

A Fase 4 exige criar o app Windows desktop com WinUI 3. O ambiente inicialmente nao possuia o template `winui` em `dotnet new list winui`, mesmo apos o bootstrap WinUI da maquina. A busca de templates oficiais do `dotnet new` encontrou o pacote Microsoft `Microsoft.WindowsAppSDK.WinUI.CSharp.Templates`.

O projeto tambem precisa manter view models testaveis sem depender da inicializacao do runtime WinUI/Windows App SDK durante testes unitarios.

## Decisao

Instalar o pacote oficial Microsoft `Microsoft.WindowsAppSDK.WinUI.CSharp.Templates` e criar `StudyLab.Desktop` com:

- `dotnet new winui -o .\src\StudyLab.Desktop -n StudyLab.Desktop -tfm net10.0`;
- empacotamento inicial single-project MSIX packaged, que e o modelo exposto pelo template oficial;
- build local do app com `Platform=x64`;
- `StudyLab.Desktop.Presentation` como biblioteca .NET pura para view models e modelos de apresentacao testaveis;
- `StudyLab.Desktop` como adaptador WinUI/composicao, referenciando Application, Infrastructure e Desktop.Presentation.

Remover arquivos auxiliares de instrucao gerados pelo template dentro do projeto desktop, pois o HARNESS e a documentacao do repositorio continuam sendo a fonte de verdade. Remover tambem a capability `systemAIModels` do manifesto por menor privilegio; manter apenas `runFullTrust`, necessario para app desktop WinUI empacotado.

## Consequencias

Beneficios:

- O app desktop nasce ancorado em template oficial Microsoft.
- View models podem ser testados com xUnit sem carregar COM/runtime WinUI.
- A UI fica como adaptador, preservando regra de negocio fora do XAML/code-behind.
- O manifesto inicial reduz capacidades ao minimo conhecido para o shell WinUI.

Custos e riscos:

- O build e launch do app WinUI exigem `Platform=x64` nesta maquina.
- A verificacao por `.slnx` com `-p:Platform=x64` ainda precisa de ajuste de configuracao de solucao.
- O empacotamento, identidade, assinatura e estrategia de distribuicao ainda devem ser revisitados antes de release.
