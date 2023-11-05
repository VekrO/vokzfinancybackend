namespace VokzFinancy.DTOs {

    public class ReceitaGraficoDTO
    {
        public int Id { get; set; }
        public int ContaId { get; set; } // Chave estrangeira para a conta
        public double Valor { get; set; }
        public DateTime Data { get; set; }
    }

}