using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

class Program
{
    // Declarando variáveis globais
    public static double altura, peso, imc;

    #region Variaveis de conversão
    public static double taxaCambioRealParaReal = 1.00;
    public static double taxaCambioDolarParaReal = 4.97;
    public static double taxaCambioEuroParaReal = 5.35;
    public static double taxaCambioBitcoinParaReal = 258447.42;
    public static double taxaCambioEthereumParaReal = 13779.65;
    public static double valorMonetario;

    #endregion Variaveis de conversão
    public static string strSenhaTentativa, nomeUsuario, resposta;
    public static int SenhaTentativa, senhaUsuario;
    public static bool senhaCorreta = false;
    public static int tentativas;

    public static string ultimaTelaVisitada; static void Main()
    {
        // Menu
        Console.Clear();
        System.Console.WriteLine("##### PARA INICIARMOS DIGITE SEU NOME DE USUÁRIO #####");
        nomeUsuario = Console.ReadLine();

        // Verifica se o usuário existe no arquivo
        if (VerificarUsuarioExiste(nomeUsuario))
        {
            do
            {
                Console.WriteLine("Digite sua senha:");
                senhaUsuario = int.Parse(Console.ReadLine());

                // Verifica se a senha está correta
                if (VerificarSenha(nomeUsuario, senhaUsuario))
                {
                    senhaCorreta = true;
                }
                else
                {
                    Console.WriteLine("Senha incorreta. Tente novamente.");
                    tentativas++;
                }
            } while (!senhaCorreta && tentativas < 3); // Limita a  3 tentativas

            if (senhaCorreta)
            {
                // Se a senha estiver correta, vai para a tela principal
                Console.Clear();
                ultimaTelaVisitada = "MostrarTelaPrincipal";
                MostrarTelaPrincipal();
            }
            else
            {
                // Se a senha estiver incorreta após  3 tentativas, encerra o login ou executa outra ação
                Console.WriteLine("Tentativas esgotadas. Login cancelado.");
            }
        }
        else
        {
            // Se o usuário não existir, vai para a tela de cadastro
            Console.Clear();
            ultimaTelaVisitada = "Tela de Cadastro";
            MostrarPrimeiraTela();
        }
    }

    #region Menu para fazer cadastro
    static void MostrarPrimeiraTela()
    {
        // Solicita ao usuário que digite sua senha
        System.Console.WriteLine($"###### Ola {nomeUsuario} Sejam Bem-Vindo(a) pela primeira vez ######");
        System.Console.WriteLine("------------------------------------------------------------");
        System.Console.WriteLine($"{nomeUsuario} preciso que crie um senha para você");
        System.Console.WriteLine("É importante lembrar que a senha deve conter apenas números e");
        System.Console.WriteLine("Deve conter entre 5 à 10 caracteres");
        System.Console.WriteLine("------------------------------------------------------------");
        strSenhaTentativa = Console.ReadLine();

        // Verifica se a senha contém apenas dígitos
        if (!ValidaSenha(strSenhaTentativa))
        {
            System.Console.WriteLine("Senha Inválida.");
            return;
        }

        bool senhaCorreta = false;
        // Loop para verificar a senha
        do
        {
            System.Console.WriteLine("------------------------------------------------------------");
            System.Console.WriteLine("Para finalizarmos confirme a senha");
            System.Console.WriteLine("------------------------------------------------------------");
            string confirmacaoSenha = Console.ReadLine();
            if (confirmacaoSenha == strSenhaTentativa)
            {
                senhaCorreta = true;
            }
            else
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Senha inválida. Digite novamente.");
                System.Console.WriteLine("");
            }
        } while (!senhaCorreta);

        // Salvar os dados registrados em um arquivo .txt criado na função SalvarUsuariosNoArquivo
        senhaUsuario = int.Parse(strSenhaTentativa);
        SalvarUsuarioNoArquivo(nomeUsuario, senhaUsuario);
        Console.Clear();
        MostrarTelaPrincipal();

    }
    #endregion Menu para fazer cadastro

    // Função Criada para Verificar se senha é composta por apenas números e com caracteres de 5 a 10 números
    public static bool ValidaSenha(string senha)
    {
        Regex regex = new Regex(@"^\d{5,10}$");
        return regex.IsMatch(senha);
        // A expressão @"^\d{5,10}$" é utilizada para validar se uma string é composta por apenas números e qntd de caracteres
        // @ - conhecido como prefixo de string verbatim. Ele permite que você crie strings longas que abrangem várias linhas e que não precisam escapar caracteres especiais.
        // ^ - Este é o caractere de início de linha. Ele indica que a correspondência deve começar no início da string.
        // \d - Este é um metacaracter que corresponde a qualquer dígito decimal. Em outras palavras, corresponde a qualquer número de 0 a 9.
        // {5,10} - Estes são quantificadores que especificam que o padrão anterior (neste caso, um dígito) deve aparecer pelo menos 5 vezes, mas não mais do que 10 vezes.
        // $ - Este é o caractere de fim de linha. Ele indica que a correspondência deve terminar no final da string.
    }

    static void SalvarUsuarioNoArquivo(string nome, int senha)
    {
        using (StreamWriter writer = File.AppendText("usuarios.txt")) // Escrevendo usuários no arquivo
        {

            writer.WriteLine("-------------------------------");
            writer.WriteLine($"Nome: {nome}");
            writer.WriteLine($"Senha: {senha}");
            writer.WriteLine("-------------------------------");
            writer.Close();

        }
    }

    #region Verificação de Usuario no .txt
    // Esta função verifica se a senha é correta para o usuário
    static bool VerificarSenha(string nomeUsuario, int senhaUsuario)
    {
        try
        {
            using (StreamReader reader = new StreamReader("usuarios.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Nome: ") && line.Substring(6).Trim() == nomeUsuario)
                    {
                        // Lê a próxima linha para obter a senha
                        string nextLine = reader.ReadLine();
                        if (nextLine.StartsWith("Senha: ") && int.Parse(nextLine.Substring(7).Trim()) == senhaUsuario)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch (IOException)
        {

        }

        return false;
    }

    // Esta função verifica se o usuário existe no arquivo, mas não verifica a senha
    static bool VerificarUsuarioExiste(string nomeUsuario)
    {
        try
        {
            using (StreamReader reader = new StreamReader("usuarios.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Nome: ") && line.Substring(6).Trim() == nomeUsuario)
                    {
                        return true;
                    }
                }
            }
        }
        catch (IOException)
        {
            // Trate o erro conforme necessário
        }

        return false;
    }
    #endregion Verificação de Usuario no .txt

    #region Verificação de menu
    // Esta função mostra a tela principal
    public static void MostrarTelaPrincipal()
    {

        Console.Clear();
        VerficacaoUsuario();
        do
        {
            if (resposta == "1")
            {
                Console.Clear();
                TaxaPopulacao();

            }
            else if (resposta == "2")
            {
                Console.Clear();
                Calculadora();
            }
            else if (resposta == "3")
            {
                Console.Clear();
                GerarNumAleatorio();
            }
            else if (resposta == "4")
            {
                Console.Clear();
                CalcularIMC();
            }
            else if (resposta == "5")
            {
                Console.Clear();
                ConversorMoedas();
            }
            else if (resposta == "6")
            {
                Console.Clear();
                Calc.ExecutarCalculo();
            }
            else if (resposta == "7")
            {
                Console.Clear();
                Sair();
            }

            else if (resposta != "8")
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Tela Inválida");
                System.Console.WriteLine("");
            }

            if (resposta != "8") // Caso tenha clicado algum número inválido temos a opção de voltar ao menu
            {
                System.Console.WriteLine("------------------------------------------------------------");
                System.Console.WriteLine("Pressione qualquer tecla par voltar ao menu");
                System.Console.WriteLine("------------------------------------------------------------");
                Console.ReadLine();
                Console.Clear();
                MostrarTelaPrincipal();
            }
        } while (resposta != "7");
    }
    #endregion Verificação de menu

    #region Calculadora
    public static void Calculadora()
    {
        // Declarando Variavéis
        int numero1, numero2;
        char operacao;
        int resultado = 0;
        string simOuNao = string.Empty;

        // Perguntas ↓
        do
        {
            Console.Clear();
            System.Console.WriteLine("############## Vamos começar a nossa calculadora ##############");
            System.Console.WriteLine("");

            System.Console.WriteLine("Por Favor, digite o primeiro número:");
            numero1 = int.Parse(Console.ReadLine());
            System.Console.WriteLine("");

            System.Console.WriteLine("Agora, digite o segundo número");
            numero2 = int.Parse(Console.ReadLine());
            System.Console.WriteLine("");

            System.Console.WriteLine("Informe a operação que vai ser utilizada");
            operacao = char.Parse(Console.ReadLine());

            // Verficicando qual operação foi utilizada e chamando as funções para cada operação
            if (operacao == '+')
            {
                resultado = Soma(numero1, numero2);
                System.Console.WriteLine($"O valor da soma é");
                System.Console.WriteLine(resultado);
            }
            else if (operacao == '-')
            {
                resultado = Diminuir(numero1, numero2);
                System.Console.WriteLine($"O valor da subtração é");
                System.Console.WriteLine(resultado);
            }
            else if (operacao == '*')
            {
                resultado = Multiplicar(numero1, numero2);
                System.Console.WriteLine($"O valor da multiplicaço é");
                System.Console.WriteLine(resultado);
            }
            else if (operacao == '/')
            {
                if (numero2 == 0)
                {
                    System.Console.WriteLine("O valor 0 é inválido"); //Para prevenir que o número que vai ser dividir seja 0
                }
                else
                {
                    resultado = Dividir(numero1, numero2);
                    System.Console.WriteLine($"O valor da divisao é");
                    System.Console.WriteLine(resultado);
                }
            }
            else
            {
                System.Console.WriteLine("Operador númerico desconhecido"); // Quando o usuário escreve o operador inválido
            }

            System.Console.WriteLine("Você deseja fazer outra conta? Digite S ou N");
            simOuNao = Console.ReadLine();

        } while (simOuNao == "S" || simOuNao == "s");
        System.Console.WriteLine("------------------------------------------------------------");
        System.Console.WriteLine("Pressione qualquer tecla par voltar ao menu");
        System.Console.WriteLine("------------------------------------------------------------");
        Console.ReadLine();
        Console.Clear();
        MostrarTelaPrincipal();
        // while usando caso o usuário queira fazer uma nova conta

        //Funções
        static int Soma(int parametroNumero, int parametroNumero2)
        {
            int resultado = parametroNumero + parametroNumero2;
            return resultado;
        }

        static int Diminuir(int parametroNumero, int paremtroNumero2)
        {
            int resultado = parametroNumero - paremtroNumero2;
            return resultado;
        }

        static int Multiplicar(int parametroNumero, int paremtroNumero2)
        {
            int resultado = parametroNumero * paremtroNumero2;
            return resultado;
        }

        static int Dividir(int parametroNumero, int paremtroNumero2)
        {
            int resultado = parametroNumero / paremtroNumero2;
            return resultado;
        }
    }
    #endregion Calculadora

    #region Taxa De crescimento
    public static void TaxaPopulacao()
    {
        Console.Clear();
        System.Console.WriteLine($"############# Seja Bem Vindo {nomeUsuario} ao programa de calcular taxa de crescimento #############");

        #region Perguntar nome dos países

        string nomePaisA, nomePaisb;
        System.Console.WriteLine("");
        System.Console.WriteLine($"Para iniciarmos nosso programa {nomeUsuario} digite o nome do primeiro país:");
        nomePaisA = Console.ReadLine();
        System.Console.WriteLine("");
        System.Console.WriteLine("Agora digite o nome do segundo o país:");
        nomePaisb = Console.ReadLine();
        System.Console.WriteLine("");

        #endregion Perguntar nome dos países

        #region Informações

        System.Console.WriteLine($"{nomeUsuario} digite a população do país {nomePaisA}");
        int populacaoA = int.Parse(Console.ReadLine());

        System.Console.WriteLine("");
        System.Console.WriteLine($"Agora {nomeUsuario} digite a taxa de crescimento do país {nomePaisA}");
        int taxaPaisA = int.Parse(Console.ReadLine());

        System.Console.WriteLine("");
        System.Console.WriteLine($"{nomeUsuario} digite a população do país {nomePaisb}");
        int populacaoB = int.Parse(Console.ReadLine());

        System.Console.WriteLine("");
        System.Console.WriteLine($"Por fim {nomeUsuario} digite a taxa de crescimento país {nomePaisb}");
        int taxaPaisB = int.Parse(Console.ReadLine());
        int contarAnos = 0;

        #endregion Informações

        do
        {
            contarAnos += 1;

            populacaoA += RetornaDaPopulacao(populacaoA, taxaPaisA);
            populacaoB += RetornaDaPopulacao(populacaoB, taxaPaisB);
            System.Console.WriteLine($"----- Estamos no ano {contarAnos} -----");

        } while (populacaoA > populacaoB);

        System.Console.WriteLine("");
        System.Console.WriteLine($"{nomeUsuario} a população do país {nomePaisb} passou a do país {nomePaisA} em {contarAnos} anos");
        System.Console.WriteLine($"Sendo que atualmente a população do país {nomePaisA} é de {populacaoA}");
        System.Console.WriteLine($"E a população atualmente do país {nomePaisb} é de {populacaoB}");
        System.Console.WriteLine("");
        System.Console.WriteLine("------------------------------------------------------------");
        System.Console.WriteLine("Pressione qualquer tecla par voltar ao menu");
        System.Console.WriteLine("------------------------------------------------------------");
        Console.ReadLine();
        Console.Clear();
        MostrarTelaPrincipal();

        static int RetornaDaPopulacao(int parametroPopulacaoPais, int parametroTaxa)
        {
            int resultado = (parametroPopulacaoPais * parametroTaxa) / 100;

            return resultado;
        }
    }
    #endregion Taxa De crescimento

    #region Gerador de números
    public static void GerarNumAleatorio()
    {
        System.Console.WriteLine($"Seja bem vindo(a) {nomeUsuario}");
        // Solicita ao usuário a quantidade de números que deseja gerar
        Console.Write("Digite a quantidade de números que deseja gerar: ");
        int quantidade = int.Parse(Console.ReadLine());

        // Cria uma instância da classe Random
        Random rng = new Random();

        // Lista para armazenar os números gerados
        var numerosAleatorios = new List<int>();

        // Gera os números aleatórios e adiciona à lista
        for (int i = 0; i < quantidade; i++)
        {
            numerosAleatorios.Add(rng.Next(1, 101)); // Gera um número entre  1 e  100
        }

        // Exibe os números gerados na tela, separados por vírgulas
        for (int i = 0; i < numerosAleatorios.Count; i++)
        {
            Console.Write(numerosAleatorios[i]);
            if (i < numerosAleatorios.Count - 1) // Não coloca vírgula após o último número
                Console.Write(", ");
        }
        System.Console.WriteLine("");
        System.Console.WriteLine("------------------------------------------------------------");
        System.Console.WriteLine("Pressione qualquer tecla par voltar ao menu");
        System.Console.WriteLine("------------------------------------------------------------");
        Console.ReadLine();
        Console.Clear();
        MostrarTelaPrincipal();
    }
    #endregion Gerador de números

    #region calculadora de imc
    public static void CalcularIMC()
    {
        System.Console.WriteLine($"Ola, {nomeUsuario}, aqui você pode calcular seu Índice de Massa Corporal");
        System.Console.WriteLine("Por favor, informe sua altura: ");
        altura = double.Parse(Console.ReadLine());
        System.Console.WriteLine("Agora informe seu peso: ");
        peso = double.Parse(Console.ReadLine());

        imc = peso / (altura * altura);

        System.Console.WriteLine($"Seu IMC é de {imc:F2}");
        switch (imc)
        {
            case < 18.5:
                System.Console.WriteLine("Você está abaixo do peso");
                break;
            case < 24.9:
                System.Console.WriteLine("Você está no peso ideal");
                break;
            case < 29.9:
                System.Console.WriteLine("Você está um pouco acima do peso");
                break;
            case < 34.9:
                System.Console.WriteLine("Você está com obesidade grau 1");
                break;
            case < 40:
                System.Console.WriteLine("Você está com obesidade grau 2");
                break;
            case > 40:
                System.Console.WriteLine("Você está com obesidade grau 3");
                break;
        }
    }
    #endregion calculadora de imc

    #region conversor de moedas
    public static void ConversorMoedas()
    {
        System.Console.WriteLine($"###### Ola {nomeUsuario}, este é o nosso conversor de moedas");
        System.Console.WriteLine("Aqui você pode converter as principais moedas do mundo");
        System.Console.WriteLine("");
        System.Console.WriteLine("Selecione entre essas moedas para converter");
        System.Console.WriteLine("-----------------------");
        System.Console.WriteLine("1- Dólar (USD)");
        System.Console.WriteLine("2- Euro (EUR)");
        System.Console.WriteLine("3- Bitcoin (BTC)");
        System.Console.WriteLine("4- Ethereum (ETH)");
        System.Console.WriteLine("-----------------------");
        int convesorMoeda = int.Parse(Console.ReadLine());

        System.Console.WriteLine("Agora digite quantidade que deseja converter:");
        valorMonetario = double.Parse(Console.ReadLine());

        #region situações
        if (convesorMoeda == 1)
        {
            double valorEmDolar = valorMonetario; // Valor que você quer converter de Dólar para Real
            double valorEmReal = ConverterDolarParaReal(valorEmDolar);

            Console.WriteLine($"{valorEmDolar:F2} USD é igual a {valorEmReal:F2} BRL");
        }
        else if (convesorMoeda == 2)
        {
            double valorEmEuro = valorMonetario; // Valor que você quer converter de Dólar para Real
            double valorEmReal1 = ConverterEuroParaReal(valorEmEuro);

            Console.WriteLine($"{valorEmEuro:F2} é igual a {valorEmReal1:F2} BRL");
        }
        else if (convesorMoeda == 3)
        {
            double valorEmBitcoin = valorMonetario; // Valor que você quer converter de Bitcoin para Real
            double valorEmRealBitcoin = ConverterBitcoinParaReal(valorEmBitcoin);

            Console.WriteLine($"{valorEmBitcoin:F2} BTC é igual a {valorEmRealBitcoin:F2} BRL");
        }
        else if (convesorMoeda == 4)
        {
            double valorEmEthereum = valorMonetario; // Valor que você quer converter de Ethereum para Real
            double valorEmRealEthereum = ConverterEthereumParaReal(valorEmEthereum);

            Console.WriteLine($"{valorEmEthereum:F2} ETH é igual a {valorEmRealEthereum:F2} BRL");
        }
        #endregion situações

        #region funcoes

        static double ConverterDolarParaReal(double valorEmDolar)
        {
            double valorEmReal = valorEmDolar * taxaCambioDolarParaReal;
            return valorEmReal;
        }

        static double ConverterEuroParaReal(double valorEmEuro)
        {
            double valorEmReal1 = valorEmEuro * taxaCambioEuroParaReal;
            return valorEmReal1;
        }

        static double ConverterBitcoinParaReal(double valorEmBitcoin)
        {
            double valorEmReal = valorEmBitcoin * taxaCambioBitcoinParaReal;
            return valorEmReal;
        }

        static double ConverterEthereumParaReal(double valorEmEthereum)
        {
            double valorEmReal = valorEmEthereum * taxaCambioEthereumParaReal;
            return valorEmReal;
        }
        #endregion funcoes
    }

    #endregion conversor de moedas

    #region Probabilidade
    public class Probabilidade
    {
        public static long Fatorial(int numero)
        {
            long resultado = 1;
            for (int i = 1; i <= numero; i++)
            {
                resultado *= i;
            }
            return resultado;
        }
    }

    public static class Calc
    {
        public static void ExecutarCalculo()
        {
            Console.Clear();
            System.Console.WriteLine($"####### Ola {nomeUsuario} esté é o menu de calcular números fatoriais #######");
            System.Console.WriteLine();
            System.Console.WriteLine("Digite um número para fazer seu fatorial");
            int numero = int.Parse(Console.ReadLine());

            if (numero >= 60)
            {
                System.Console.WriteLine("Número Inválido, por favor digite um número menor que 59");
                Console.ReadLine();
                Calc.ExecutarCalculo();
            }

            long fatorial = Probabilidade.Fatorial(numero);
            Console.WriteLine($"Há um total de {fatorial} possibilidades.");
        }

        public static void executa(string[] args)
        {
            ExecutarCalculo();
        }
    }
    #endregion Probabilidade

    #region sair
    public static void Sair()
    {
        System.Console.WriteLine($"Obrigado pela preferência {nomeUsuario}, volte sempre!");
        System.Console.WriteLine("Pressione qualquer tecla para sair: ");
        string sair = Console.ReadLine();
        Environment.Exit(0); // O '0' indica sucesso, enquanto outros valores indicam diferentes tipos de erros
    }
    #endregion sair

    #region Menu
    public static void VerficacaoUsuario()
    {
        if (senhaCorreta = false)
        {
            System.Console.WriteLine($"Seja bem vindo(a) pela sua primeira vez {nomeUsuario}");
            System.Console.WriteLine($"{nomeUsuario} sua senha atual é {senhaUsuario}");
            System.Console.WriteLine("");
            System.Console.WriteLine($"------ {nomeUsuario} este é o nosso menu ------");
            System.Console.WriteLine("1- Calculando a taxa de Crescimento da População");
            System.Console.WriteLine("2- Calculadora");
            System.Console.WriteLine("3- Gerador de Números Aleatórios");
            System.Console.WriteLine("4- Calcular IMC");
            System.Console.WriteLine("5- Conversor de Moedas");
            System.Console.WriteLine("6- Probabilidade");
            System.Console.WriteLine("7- Sair");
            resposta = Console.ReadLine();
        }
        else
        {
            System.Console.WriteLine($"Seja bem vindo(a) {nomeUsuario}");
            System.Console.WriteLine("");
            System.Console.WriteLine($"------ {nomeUsuario} este é o nosso menu ------");
            System.Console.WriteLine("1- Calculando a taxa de Crescimento da População");
            System.Console.WriteLine("2- Calculadora");
            System.Console.WriteLine("3- Gerador de Números Aleatórios");
            System.Console.WriteLine("4- Calcular IMC");
            System.Console.WriteLine("5- Conversor de Moedas");
            System.Console.WriteLine("6- Probabilidade");
            System.Console.WriteLine("7- Sair");
            resposta = Console.ReadLine();
        }
    }
    #endregion Menu 

}
