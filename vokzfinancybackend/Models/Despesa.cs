using System.ComponentModel.DataAnnotations.Schema;

namespace VokzFinancy.Models
{
    public class Despesa
    {
        public int Id { get; set; }
        public int ContaId { get; set; }
        public Conta Conta { get; set; }
        public string? Titulo { get; set; }
        public bool? Paga {get; set;}
        public string? Descricao { get; set; }
        public double Valor { get; set; }
        public bool Vencida {get; set; }
        [Column(TypeName="Date")]
        public DateTime Vencimento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
