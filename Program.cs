using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

class Program
{
    // Declarando variáveis globais
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
    static void MostrarTerceiraTela() // Caso não tenha um arquivo de texto, está criando um novo e registrando
    {
        using (StreamReader reader = new StreamReader("usuarios.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
            }
        }
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

    // Esta função mostra a tela principal
    public static void MostrarTelaPrincipal()
    {
        Console.Clear();
        System.Console.WriteLine($"Seja bem vindo(a) {nomeUsuario}");
        System.Console.WriteLine($"Sua senha atutal é {senhaUsuario}");
        System.Console.WriteLine("");
        System.Console.WriteLine($"------ {nomeUsuario} este é o nosso menu ------");
        System.Console.WriteLine("1- Jogo da cobrinha");
        System.Console.WriteLine("2- Calculadora");
        System.Console.WriteLine("3- Sair");
        resposta = Console.ReadLine();
        do
        {
            if (resposta == "1")
            {
                Console.Clear();
                
            }
            else if (resposta == "2")
            {
                Console.Clear();
                Calculadora();
            }

            else if (resposta != "3")
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Tela Inválida");
                System.Console.WriteLine("");
            }

            if (resposta != "3") // Caso tenha clicado algum número inválido temos a opção de voltar ao menu
            {
                System.Console.WriteLine("------------------------------------------------------------");
                System.Console.WriteLine("Pressione qualquer tecla par voltar ao menu");
                System.Console.WriteLine("------------------------------------------------------------");
                Console.ReadLine();
                Console.Clear();
                MostrarTelaPrincipal();
            }
        } while (resposta != "3");
    }
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
}
