
namespace VokzFinancy.Models {

    public class Conta {

        public int Id {get; set;}
        public string? Nome {get; set;}
        public int? UsuarioId { get; set; } // Chave estrangeira para o usuário
        public Usuario? Usuario { get; set; } // Propriedade de navegação para o usuário
        public string? Descricao { get; set; }
        public bool Padrao {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

}