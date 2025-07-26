namespace DesafioFundamentos.Models
{
    public class Funcionario
    {
        public string Usuario { get; set; } = "";
        public string Senha { get; set; } = "";
        public string NomeCompleto { get; set; } = "";
        public string Funcao { get; set; } = ""; // "Gerente" ou "Operador"
    }
}
