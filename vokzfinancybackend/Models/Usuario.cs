using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VokzFinancy.Models {

    public class Usuario {
        
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name {get; set;}
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string? Email {get;set;}
        [MaxLength(255)]
        public string? Password {get;set;}
        public string? VerificationEmailToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Conta> Contas {get; set;}
        
    }

}