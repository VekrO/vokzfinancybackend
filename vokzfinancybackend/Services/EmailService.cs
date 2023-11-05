using System.Net;
using System.Net.Mail;
using VokzFinancy.DTOs;


namespace VokzFinancy.Services {

    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private MailMessage message = new MailMessage();
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailConfirmation(EmailDTO email)
        {
            
            try {

                message.From = new MailAddress(_configuration["Mail:From"]);
                message.Subject = "Vokz Financy - Confirmação de conta!";
                message.To.Add(new MailAddress(email.To));
                
                string token = email.Token;

                string body = $@"
                    <h2>Vokz Financy</h2><br>
                    <p>Ative sua conta clicando no link abaixo!</p>
                    <a href='https://vokzfinancy.app/verify?token={token}'>Ativar Conta!</a>
                ";

                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com") {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration["Mail:From"], _configuration["Mail:FromPassword"]),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);

                return true;

            } catch (Exception ex){

                throw new Exception(ex.Message);

            }

        }

        public async Task<bool> SendEmailResetPassword(EmailDTO email)
        {
            
            try {

                message.From = new MailAddress(_configuration["Mail:From"]);
                message.Subject = "Vokz Financy - Resetar a Senha!";
                message.To.Add(new MailAddress(email.To));
                
                string token = email.Token;

                string body = $@"
                    <h2>Vokz Financy</h2><br>
                    <p>Você pode resetar a senha apertando no link abaixo!</p>
                    <a href='https://vokzfinancyfront.vercel.app/reset-password?token={token}'>Resetar minha Senha!</a>
                ";

                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com") {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration["Mail:From"], _configuration["Mail:FromPassword"]),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);

                return true;

            } catch (Exception ex){

                throw new Exception(ex.Message);

            }

        }

    }

}