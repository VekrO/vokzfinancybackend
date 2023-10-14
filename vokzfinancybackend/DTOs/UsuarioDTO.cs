using System.ComponentModel.DataAnnotations;

namespace VokzFinancy.DTOs {

    public class UsuarioDTO {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name {get; set;} = String.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email {get;set;} = String.Empty;
    }

}