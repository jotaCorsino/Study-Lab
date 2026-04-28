StudyHub — Aplicativo de estudos por cursos locais

O StudyHub é um aplicativo de estudos, inicialmente para Windows, criado para organizar, assistir e acompanhar cursos em vídeo armazenados localmente no computador.

A proposta principal é permitir que o usuário transforme uma pasta cheia de videoaulas em uma estrutura organizada de curso, com controle de progresso, rotina de estudos, metas diárias, histórico mensal e acesso rápido à aula atual.

Objetivo do app

O aplicativo deve ajudar o usuário a:

organizar cursos locais salvos em pastas;
assistir às aulas dentro do próprio app;
acompanhar progresso por aula, módulo e curso;
manter uma rotina de estudos com metas diárias;
visualizar desempenho mensal;
continuar estudando exatamente de onde parou;
organizar materiais complementares como PDFs, documentos e links.
Importação de cursos por pasta

O usuário poderá selecionar uma pasta local do computador para importar um curso.

A estrutura ideal seria:

Curso
└── Matéria
    └── Módulo
        └── Tópico
            └── Aula.mp4

Porém, o app deve ser flexível o suficiente para interpretar estruturas diferentes, identificando automaticamente possíveis níveis como:

curso;
matéria;
módulo;
tópico;
aula.

Após a importação, o usuário poderá editar títulos, reorganizar conteúdos e ajustar a estrutura caso necessário.

Organização manual de cursos

Além da importação automática, o usuário também poderá criar cursos manualmente.

Ele poderá:

criar um curso do zero;
adicionar matérias, módulos e tópicos;
inserir videoaulas manualmente;
renomear aulas;
reorganizar conteúdos;
criar áreas de materiais complementares;
editar a estrutura do curso depois da criação.

Sempre que possível, a organização deve funcionar com drag and drop, facilitando a reorganização de aulas, módulos e materiais.

Tela inicial

A tela inicial funciona como um catálogo de cursos.

No menu lateral esquerdo, devem existir:

logo do app;
link para o catálogo;
seção “Meus cursos”;
botão “+” para adicionar novo curso;
lista dos cursos cadastrados;
acesso às configurações.

Cada item da lista de cursos deve exibir:

indicador colorido da meta diária;
ícone do curso;
nome do curso.
Página do curso

Ao selecionar um curso, o usuário acessa a página principal daquele curso.

Essa página deve exibir:

painel da meta diária;
calendário mensal de estudos;
tempo estudado no dia;
progresso geral do curso;
botão “Continuar estudando”;
resumo do curso;
lista de módulos, tópicos e aulas;
área de materiais complementares.
Meta diária e rotina de estudos

Cada curso pode ter uma rotina própria.

O usuário poderá configurar:

dias da semana em que pretende estudar;
quantidade de horas ou minutos por dia;
meta diária específica para aquele curso.

O desempenho diário será representado por quadrados no calendário mensal. Cada quadrado representa um dia do mês.

A cor do quadrado muda conforme o percentual da meta cumprida:

neutro: dia sem estudo ou não programado;
vermelho: meta muito abaixo do esperado;
laranja/amarelo: progresso parcial;
verde claro: quase concluída;
verde: meta batida.

O cálculo deve ser feito com base no tempo real de videoaula assistido.

Horas extras e abono mensal

Quando o usuário estuda mais do que a meta diária, o tempo excedente vira crédito de estudo.

Esse crédito:

é exibido no formato 0h00;
acumula apenas dentro do mês atual;
pode ser usado para compensar metas não batidas em dias anteriores;
não deve ser usado para compensar o dia atual;
é descontado automaticamente ao abonar pendências.

Quando um dia é abonado, a cor original do quadrado não muda. Em vez disso, o quadrado recebe um marcador visual, como um pequeno círculo verde no centro.

Histórico mensal

Ao mudar de mês, o calendário anterior vira histórico.

O app deve permitir que o usuário visualize o desempenho dos meses anteriores em uma visão anual.

Cada mês pode ser representado por um quadrado com abreviação:

Jan, Fev, Mar, Abr...

A cor de cada mês deve representar a média de desempenho daquele período.

Dias abonados contam como meta cumprida para o cálculo mensal. Dias parcialmente compensados devem considerar a soma do tempo estudado com o crédito consumido.

Progresso do curso

Além da rotina diária, o app também deve exibir o progresso estrutural do curso.

Esse progresso considera:

quantidade total de aulas;
quantidade de aulas concluídas;
porcentagem concluída;
tempo total do curso;
tempo já assistido;
ritmo de estudo;
sequência de dias estudados.

A página do curso deve exibir uma barra de progresso geral com a porcentagem concluída.

Organização dos módulos e aulas

A área de conteúdo do curso deve ser dividida em módulos expansíveis.

Cada módulo deve mostrar:

nome do módulo;
quantidade de aulas;
duração total;
quantidade de aulas concluídas no formato 0/999.

Cada aula deve exibir:

ícone de status;
título;
duração;
indicação se foi assistida ou concluída.

Aulas não vistas usam estado neutro. Aulas concluídas usam estado verde.

Player de aula

Ao clicar em uma aula, o vídeo será aberto na tela de reprodução.

A página da aula deve conter:

player central;
botão para marcar aula como concluída;
botão de aula anterior;
botão de próxima aula;
seletor de velocidade;
configuração de intro skip;
menu lateral direito com módulos, tópicos e aulas.

Quando uma aula for assistida até o final, ela deve ser marcada automaticamente como concluída.

Velocidade de reprodução

O usuário poderá selecionar a velocidade de reprodução da aula.

Velocidades sugeridas:

1x;
1,25x;
1,5x;
2x.

A velocidade pode ser mantida como preferência do usuário.

Intro skip

A função intro skip permite pular automaticamente os primeiros segundos de cada aula.

O usuário informa um número em segundos. Por exemplo:

10

Nesse caso, toda aula daquele curso começará no segundo 10.

A função deve ter um botão de ativar/desativar. Quando estiver ativa, o comportamento se aplica a todas as aulas daquele curso específico.

Menu lateral da aula

Na página da aula, deve existir um menu lateral direito com a estrutura do curso.

Esse menu mostra:

módulos;
tópicos;
aulas;
aula atual destacada;
posição do usuário dentro do curso.

Ele funciona como uma âncora de navegação para o usuário localizar rapidamente onde está.

Materiais complementares

Cada curso poderá ter uma área dedicada a conteúdos complementares.

Essa área pode conter:

PDFs;
documentos;
links;
arquivos auxiliares;
anotações;
referências externas.

Esses materiais também devem poder ser organizados por módulos, tópicos ou categorias.

Adicionar novo curso

Ao clicar no botão “+”, o usuário acessa uma tela de criação/importação de curso.

Essa tela deve conter:

breve explicação de como usar o app;
exemplo visual de organização de pastas;
boas práticas de nomeação de arquivos;
botão “Selecionar diretório”;
opção de criar curso manualmente.

Após selecionar a pasta, o app cria automaticamente o curso e permite edição posterior.

Configurações

A área de configurações deve permitir ajustes como:

tema claro/escuro;
preferências de reprodução;
velocidade padrão;
comportamento do intro skip;
diretório padrão de cursos;
dados locais do app;
backup/restauração futura;
preferências visuais.
Requisitos importantes

O aplicativo deve ser:

responsivo;
intuitivo;
offline-first;
rápido para abrir cursos locais;
capaz de salvar progresso automaticamente;
seguro ao lidar com arquivos locais;
flexível para diferentes organizações de pasta;
simples de usar mesmo para usuários não técnicos.
Resumo do produto

O StudyHub é um app de estudos focado em transformar cursos em vídeo armazenados no computador em uma experiência organizada, acompanhável e produtiva.

Ele combina importação automática de pastas, player integrado, organização manual, metas diárias, histórico mensal, abono de estudos por horas extras, progresso por curso e materiais complementares em uma única interface.