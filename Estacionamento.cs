// Arquivo: Models/Estacionamento.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoPorHora;
        private decimal precoPorDia;
        private decimal precoPorMes;
        private int limiteVagas;
        private List<Veiculo> veiculos = new();
        private decimal faturamentoTotal = 0;

        public Estacionamento(decimal precoHora, decimal precoDia, decimal precoMes, int limite)
        {
            precoPorHora = precoHora;
            precoPorDia = precoDia;
            precoPorMes = precoMes;
            limiteVagas = limite;
        }

        public void AdicionarVeiculo()
        {
            if (veiculos.Count >= limiteVagas)
            {
                Console.WriteLine("Estacionamento cheio.");
                return;
            }

            Console.Write("Digite a placa do veículo: ");
            string? placa = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(placa))
            {
                Console.WriteLine("Placa inválida.");
                return;
            }

            Console.Write("Digite o nome completo do proprietário: ");
            string? proprietario = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(proprietario))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            Console.WriteLine("\nEscolha o tipo de permanência:");
            Console.WriteLine("1 - Hora (50 Kz/hora)");
            Console.WriteLine("2 - Dia (1000 Kz/dia)");
            Console.WriteLine("3 - Mês (30.000 Kz/mês)");
            Console.Write("Tipo: ");
            string? tipoStr = Console.ReadLine();

            if (!int.TryParse(tipoStr, out int tipo) || tipo < 1 || tipo > 3)
            {
                Console.WriteLine("Tipo inválido.");
                return;
            }

            decimal valor = 0;
            int quantidade = 0;

            if (tipo == 1)
            {
                Console.Write("Quantas horas deseja estacionar? ");
                int.TryParse(Console.ReadLine(), out quantidade);
                valor = precoPorHora * quantidade;
            }
            else if (tipo == 2)
            {
                Console.Write("Quantos dias deseja estacionar? ");
                int.TryParse(Console.ReadLine(), out quantidade);
                valor = precoPorDia * quantidade;
            }
            else if (tipo == 3)
            {
                Console.Write("Quantos meses deseja estacionar? ");
                int.TryParse(Console.ReadLine(), out quantidade);
                valor = precoPorMes * quantidade;
            }

            int numeroVaga = veiculos.Count + 1;
            string senha = $"VAGA-{numeroVaga:D3}";

            Veiculo v = new Veiculo
            {
                Placa = placa.ToUpper(),
                Proprietario = proprietario,
                Entrada = DateTime.Now,
                Tipo = tipo,
                Senha = senha,
                Vaga = numeroVaga,
                ValorPagoAntecipado = valor
            };

            veiculos.Add(v);
            faturamentoTotal += valor;

            Console.WriteLine($"\n✅ Veículo cadastrado com sucesso!");
            Console.WriteLine($"📍 Vaga: {numeroVaga:D3}");
            Console.WriteLine($"🔐 Senha: {senha}");
            Console.WriteLine($"💰 Valor a pagar: {valor:F2} Kz");
        }

        public void RemoverVeiculo()
        {
            Console.Write("Digite a placa do veículo a remover: ");
            string? placa = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(placa))
            {
                Console.WriteLine("Placa inválida.");
                return;
            }

            var veiculo = veiculos.FirstOrDefault(v => v.Placa == placa.ToUpper());
            if (veiculo == null)
            {
                Console.WriteLine("Veículo não encontrado.");
                return;
            }

            veiculos.Remove(veiculo);
            Console.WriteLine($"✅ Veículo removido com sucesso. Nenhum valor adicional a pagar (valor já foi pago no início).");
        }

        public void ListarVeiculos()
        {
            if (veiculos.Count == 0)
            {
                Console.WriteLine("Nenhum veículo estacionado.");
                return;
            }

            foreach (var v in veiculos)
            {
                Console.WriteLine($"📍 Vaga: {v.Vaga:D3} | Placa: {v.Placa} | Proprietário: {v.Proprietario} | Entrada: {v.Entrada} | Tipo: {TipoDescricao(v.Tipo)} | Senha: {v.Senha} | Valor Pago: {v.ValorPagoAntecipado:F2} Kz");
            }
        }

        public void BuscarVeiculo()
        {
            Console.Write("Digite a placa do veículo: ");
            string? placa = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(placa))
            {
                Console.WriteLine("Placa inválida.");
                return;
            }

            var v = veiculos.FirstOrDefault(v => v.Placa == placa.ToUpper());
            if (v != null)
            {
                Console.WriteLine($"📍 Vaga: {v.Vaga:D3} | Placa: {v.Placa} | Proprietário: {v.Proprietario} | Entrada: {v.Entrada} | Tipo: {TipoDescricao(v.Tipo)} | Senha: {v.Senha} | Valor Pago: {v.ValorPagoAntecipado:F2} Kz");
            }
            else
            {
                Console.WriteLine("Veículo não encontrado.");
            }
        }

        public void ExibirMapaVagas()
        {
            Console.WriteLine($"\n🅿️ Vagas ocupadas: {veiculos.Count}/{limiteVagas}");
            for (int i = 1; i <= limiteVagas; i++)
            {
                string status = veiculos.Any(v => v.Vaga == i) ? "🔴 Ocupada" : "🟢 Livre";
                Console.WriteLine($"Vaga {i:D3} - {status}");
            }
        }

        public void ExibirRelatorio()
        {
            Console.WriteLine($"📊 Total de veículos cadastrados: {veiculos.Count}");
            Console.WriteLine($"💵 Faturamento total: {faturamentoTotal:F2} Kz");
        }

        private string TipoDescricao(int tipo)
        {
            return tipo switch
            {
                1 => "Por Hora",
                2 => "Por Dia",
                3 => "Por Mês",
                _ => "Desconhecido"
            };
        }
    }

    public class Veiculo
    {
        public string? Placa { get; set; }
        public string? Proprietario { get; set; }
        public DateTime Entrada { get; set; }
        public int Tipo { get; set; }
        public string? Senha { get; set; }
        public int Vaga { get; set; }
        public decimal ValorPagoAntecipado { get; set; }
    }
}
