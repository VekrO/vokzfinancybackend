using System.ComponentModel.DataAnnotations.Schema;

namespace VokzFinancy.DTOs {

    public class DespesaDTO
    {
        public int Id { get; set; }
        public int ContaId { get; set; } // Chave estrangeira para a conta
        public string? Titulo { get; set; }
        public bool? Paga {get; set;}
        public string? Descricao { get; set; }
        public double Valor { get; set; }
        [Column(TypeName="Date")]
        public DateTime Vencimento { get; set; }
    }

}