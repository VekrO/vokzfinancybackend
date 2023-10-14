using VokzFinancy.Models;

namespace VokzFinancy.DTOs {

    public class ContaDTO {

        public int? Id {get; set;}
        public string? Nome {get; set;}
        public int UsuarioId { get; set; } // Chave estrangeira para o usuário
        public string? Descricao { get; set; }

    }

}