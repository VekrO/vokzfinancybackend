using System.ComponentModel.DataAnnotations;

namespace VokzFinancy.DTOs {

    public class UsuarioRegistroDTO {

        [Required]
        [MaxLength(255)]
        public string Name {get; set;} = String.Empty;
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email {get; set;} = String.Empty;
        [Required]
        [MaxLength(255)]
        public string Password { get; set;} = String.Empty;

    }

}