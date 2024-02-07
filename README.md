# Login_Com_Senha_E_Menu_Com_Jogos

Este programa em C# é um sistema de login simples com funcionalidades adicionais, como cadastro de usuário, verificação de senha, e uma calculadora básica. Aqui está um resumo das principais partes do código:

<br/>Variáveis Globais e Inicialização: O programa começa declarando várias variáveis globais para armazenar informações do usuário e do sistema, como nome de usuário, senha, número de tentativas, etc. Em seguida, a função Main() é chamada, que serve como ponto de entrada do programa.

<br/>Menu de Login: O programa exibe um menu para o usuário onde ele pode digitar seu nome de usuário e senha. Se o usuário existir no sistema e inserir a senha correta, ele é redirecionado para a tela principal. Caso contrário, ele tem um limite de três tentativas antes de ser bloqueado.

<br/>Verificação de Senha e Usuário: Duas funções são usadas para verificar se o usuário existe no arquivo e se a senha inserida está correta. As informações de usuário são armazenadas em um arquivo de texto usuarios.txt.

<br/>Cadastro de Usuário: Se o usuário não existir, ele é solicitado a criar uma senha, que deve conter apenas números e estar entre 5 e 10 caracteres. A senha é então confirmada antes de ser armazenada no arquivo.

<br/>Tela Principal: Após o login bem-sucedido, o usuário é levado para a tela principal, onde pode escolher entre jogar um jogo de cobrinha, usar a calculadora ou sair do programa.

<br/>Calculadora: A função Calculadora() permite que o usuário realize operações matemáticas básicas como adição, subtração, multiplicação e divisão. Ele pode continuar fazendo cálculos até decidir sair da calculadora e voltar para a tela principal.

<br/>Este resumo abrange as principais funcionalidades e fluxos do programa em C#. Se precisar de mais detalhes ou esclarecimentos sobre partes específicas do código, sinta-se à vontade para perguntar!

<br/>Ainda está em BETA, tenho que adicionar o jogo da cobrinha, e futuramente placares de líderes.
