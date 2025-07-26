using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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

            Console.WriteLine("\n✅ Veículo cadastrado com sucesso!\n");

            Console.WriteLine("=========================================");
            Console.WriteLine("         📄 RECIBO DE ESTACIONAMENTO");
            Console.WriteLine("=========================================");
            Console.WriteLine($"Placa do Veículo . . : {v.Placa}");
            Console.WriteLine($"Proprietário . . . . : {v.Proprietario}");
            Console.WriteLine($"Número da Vaga . . . : {v.Vaga:D3}");
            Console.WriteLine($"Tipo de Permanência  : {TipoDescricao(v.Tipo)}");
            Console.WriteLine($"Valor Pago . . . . . : {v.ValorPagoAntecipado:F2} Kz");
            Console.WriteLine($"Data/Hora de Entrada : {v.Entrada:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Senha de Acesso  . . : {v.Senha}");
            Console.WriteLine("=========================================");
            Console.WriteLine("⚠️  Guarde este recibo para futuras consultas.");

            SalvarVeiculos();
            SalvarFaturamento();
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

            Console.WriteLine("\n🔄 Calculando tempo real de permanência...");

            DateTime agora = DateTime.Now;
            TimeSpan tempoTotal = agora - veiculo.Entrada;

            decimal valorExcedente = 0;

            if (veiculo.Tipo == 1) // Por Hora
            {
                double horasUsadas = tempoTotal.TotalHours;
                double horasPagas = (double)(veiculo.ValorPagoAntecipado / precoPorHora);

                if (horasUsadas > horasPagas)
                {
                    double excedente = Math.Ceiling(horasUsadas - horasPagas);
                    valorExcedente = precoPorHora * (decimal)excedente;
                }
            }
            else if (veiculo.Tipo == 2) // Por Dia
            {
                double diasUsados = tempoTotal.TotalDays;
                double diasPagos = (double)(veiculo.ValorPagoAntecipado / precoPorDia);

                if (diasUsados > diasPagos)
                {
                    double excedente = Math.Ceiling(diasUsados - diasPagos);
                    valorExcedente = precoPorDia * (decimal)excedente;
                }
            }

            veiculos.Remove(veiculo);

            Console.WriteLine("\n✅ Veículo removido com sucesso.");
            Console.WriteLine($"⏱️ Tempo de permanência: {tempoTotal.Hours}h {tempoTotal.Minutes}min");

            if (valorExcedente > 0)
            {
                Console.WriteLine($"⚠️ Tempo excedido! Valor adicional a pagar: {valorExcedente:F2} Kz");
                faturamentoTotal += valorExcedente;
            }
            else
            {
                Console.WriteLine("💰 Nenhum valor adicional a pagar (dentro do tempo pago).");
            }

            SalvarVeiculos();
            SalvarFaturamento();
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

        private void SalvarVeiculos()
        {
            string caminhoVeiculos = "veiculos.txt";
            using (StreamWriter writer = new StreamWriter(caminhoVeiculos, false))
            {
                foreach (var v in veiculos)
                {
                    writer.WriteLine($"{v.Placa},{v.Proprietario},{v.Entrada},{v.Tipo},{v.Senha},{v.Vaga},{v.ValorPagoAntecipado}");
                }
            }
        }

        private void SalvarFaturamento()
        {
            string caminhoFaturamento = "faturamento.txt";
            using (StreamWriter writer = new StreamWriter(caminhoFaturamento, false))
            {
                writer.WriteLine($"Faturamento Total: {faturamentoTotal:F2} Kz");
            }
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
