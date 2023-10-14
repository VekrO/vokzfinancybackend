namespace VokzFinancy.DTOs {

    public class UsuarioPasswordResetDTO {

        public string? Token { get; set; }
        public string? Password {get; set;}
        public string? PasswordConfirm {get; set;}

    }

}