using DesafioFundamentos.Models;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Funcionario> funcionarios = new()
{
    new Funcionario { Usuario = "adm.sc", Senha = "0011", NomeCompleto = "Administrador", Funcao = "Gerente" },
    new Funcionario { Usuario = "joao.op", Senha = "1234", NomeCompleto = "João Operador", Funcao = "Operador" }
};

Funcionario? funcionarioLogado = FazerLogin(funcionarios);
if (funcionarioLogado == null)
{
    Console.WriteLine("\n❌ Acesso negado. Consulte o Gerente do Parque.");
    return;
}

const int limiteVagas = 100;
decimal precoPorHora = 50;
decimal precoPorDia = 1000;
decimal precoPorMes = 30000;

Console.WriteLine($"\n✅ Acesso autorizado. Bem-vindo, {funcionarioLogado.NomeCompleto} ({funcionarioLogado.Funcao})\n");

Estacionamento es = new Estacionamento(precoPorHora, precoPorDia, precoPorMes, limiteVagas);

bool exibirMenu = true;

while (exibirMenu)
{
    Console.Clear();
    Console.WriteLine("===== MENU PRINCIPAL =====");
    Console.WriteLine("1 - Cadastrar veículo");
    Console.WriteLine("2 - Remover veículo");
    Console.WriteLine("3 - Listar veículos");
    Console.WriteLine("4 - Buscar veículo por placa");
    Console.WriteLine("5 - Ver mapa de vagas");

    // ⚠️ Opção 6 visível apenas para gerente
    if (funcionarioLogado.Funcao.ToLower() == "gerente")
        Console.WriteLine("6 - Exibir relatório de faturamento");

    Console.WriteLine("7 - Encerrar");
    Console.Write("\nEscolha uma opção: ");

    switch (Console.ReadLine())
    {
        case "1":
            es.AdicionarVeiculo();
            break;
        case "2":
            es.RemoverVeiculo();
            break;
        case "3":
            es.ListarVeiculos();
            break;
        case "4":
            es.BuscarVeiculo();
            break;
        case "5":
            es.ExibirMapaVagas();
            break;
        case "6":
            if (funcionarioLogado.Funcao.ToLower() == "gerente")
                es.ExibirRelatorio();
            else
                Console.WriteLine("❌ Acesso negado. Esta opção é restrita ao gerente.");
            break;
        case "7":
            exibirMenu = false;
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }

    if (exibirMenu)
    {
        Console.WriteLine("\nPressione Enter para continuar...");
        Console.ReadLine();
    }
}

Console.WriteLine("O programa se encerrou.");


Funcionario? FazerLogin(List<Funcionario> lista)
{
    int tentativas = 3;

    while (tentativas > 0)
    {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine("         🔐 INÍCIO DE SESSÃO");
        Console.WriteLine("=========================================");
        Console.WriteLine("Insira suas credenciais para aceder ao sistema.");
        Console.WriteLine("-----------------------------------------");

        Console.Write("👤 Usuário: ");
        string? usuario = Console.ReadLine();

        Console.Write("🔑 Senha: ");
        string? senha = Console.ReadLine();

        var funcionario = lista.FirstOrDefault(f =>
            f.Usuario == usuario && f.Senha == senha
        );

        if (funcionario != null)
        {
            Console.WriteLine($"\n✅ Login bem-sucedido. Bem-vindo, {funcionario.NomeCompleto}!");
            Thread.Sleep(1000);
            return funcionario;
        }
        else
        {
            tentativas--;
            Console.WriteLine($"\n❌ Dados incorretos. Tentativas restantes: {tentativas}");
            Thread.Sleep(2000);
        }
    }

    return null;
}
