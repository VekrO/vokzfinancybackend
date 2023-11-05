using System.ComponentModel.DataAnnotations.Schema;

namespace VokzFinancy.Models {

    public class Receita {

        public int Id { get; set; }
        public int ContaId { get; set; } // Chave estrangeira para a conta
        public Conta? Conta { get; set; } // Propriedade de navegação para a conta
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public double Valor { get; set; }
        public bool? Paga { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName="Date")]        
        public DateTime Data {get; set;}

    }

}