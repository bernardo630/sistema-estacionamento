// Arquivo: Program.cs
using DesafioFundamentos.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int limiteVagas = 100;
decimal precoPorHora = 50;
decimal precoPorDia = 1000;
decimal precoPorMes = 30000;

Console.WriteLine("Seja bem-vindo ao sistema de estacionamento!\n");

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
            es.ExibirRelatorio();
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
