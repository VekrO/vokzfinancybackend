namespace VokzFinancy.DTOs {

    public class UsuarioToken {

        public bool Authenticated {get; set;}
        public string Token { get; set; } = String.Empty;
        public DateTime Expiration {get; set;}
        public string Message {get; set;} = String.Empty;

    }

}