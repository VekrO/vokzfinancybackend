using System.ComponentModel.DataAnnotations.Schema;
using VokzFinancy.Models;

namespace VokzFinancy.DTOs {

    public class ReceitaDTO
    {
        public int Id { get; set; }
        public int ContaId { get; set; } // Chave estrangeira para a conta
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public bool? Paga { get; set; }
        public double Valor { get; set; }
        [Column(TypeName="Date")]        
        public DateTime Data {get; set;}
    }

}