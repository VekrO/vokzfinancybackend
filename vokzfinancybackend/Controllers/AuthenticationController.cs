using Microsoft.AspNetCore.Mvc;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
using AutoMapper;
using VokzFinancy.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VokzFinancy.Services;

namespace VokzFinancy.Controllers
{


    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        public AuthenticationController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IEmailService emailService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailService = emailService;
            _config = config;
        }

        [HttpGet]
        [Route("get-config")]
        public ActionResult GetConfig() {
            return Ok(new {
                Ambiente = _config["Ambiente"]
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UsuarioRegistroDTO model)
        {

            try
            {

                // Verificar se o e-mail já está sendo utilizado.
                Usuario usuarioDb = await _unitOfWork.UsuarioRepository.GetByEmail(model.Email);
                if (usuarioDb != null)
                {
                    return BadRequest("O e-mail já está sendo utilizado!");
                }

                // Criptografar a senha.
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                await _unitOfWork.BeginTransactionAsync();

                Usuario usuario = new Usuario
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    VerificationEmailToken = CreateEmailAndResetToken(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.UsuarioRepository.Add(usuario);

                Conta conta = new Conta()
                {
                    Nome = "Conta Padrão",
                    Usuario = usuario,
                    Padrao = true,
                    Descricao = "Conta Padrão",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.ContaRepository.Add(conta);

                await _unitOfWork.CommitAsync(); // Finaliza a transação com o banco de dados...

                EmailDTO email = new EmailDTO
                {
                    To = usuario.Email,
                    Token = usuario.VerificationEmailToken
                };

                await _emailService.SendEmailConfirmation(email);

                return Ok();


            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioToken>> Login(UsuarioLoginDTO model)
        {

            try
            {

                if (model.Email == null || model.Password == null)
                {
                    return BadRequest("Por favor, preencha os campos corretamente!");
                }

                Usuario usuario = await _unitOfWork.UsuarioRepository.GetByEmail(model.Email);

                if (usuario == null)
                {
                    return NotFound("A conta não foi encontrada ou não existe. Verifique os dados e tente novamente!");
                }

                bool isUser = BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password);

                if (usuario.VerifiedAt == null)
                {
                    return BadRequest("Por favor, verifique seu e-mail e confirme sua conta!");
                }

                if (!isUser)
                {
                    return NotFound("Verifique os dados e tente novamente!");
                }


                return Ok(CreateAuthToken(usuario));

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(UsuarioPasswordForgotDTO request)
        {

            try
            {
                Usuario usuario = await _unitOfWork.UsuarioRepository.GetByEmail(request.Email);

                if (usuario == null)
                {
                    return NotFound("Registro não encontrado!");
                }

                // Gera o token com Guid.
                string ResetPasswordToken = CreateEmailAndResetToken();
                // Gera o tempo de expiração do Token.
                DateTime ResetTokenExpires = DateTime.Now.AddDays(1);

                usuario.ResetPasswordToken = ResetPasswordToken;
                usuario.ResetTokenExpires = ResetTokenExpires;

                EmailDTO email = new EmailDTO
                {
                    To = usuario.Email,
                    Token = usuario.ResetPasswordToken
                };

                await _emailService.SendEmailResetPassword(email);

                // Da um update no Usuário, salvando as propriedades do Token!
                await _unitOfWork.UsuarioRepository.Update(usuario);

                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(UsuarioPasswordResetDTO request)
        {

            try
            {
                Usuario usuario = await _unitOfWork.UsuarioRepository.GetByResetPasswordToken(request.Token);

                if (usuario == null || usuario.ResetTokenExpires < DateTime.Now)
                {
                    return BadRequest("Token Inválido!");
                }

                if (request.Password != request.PasswordConfirm)
                {
                    return BadRequest("As senhas enviadas não coincidem!");
                }

                usuario.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                usuario.ResetPasswordToken = null;
                usuario.ResetTokenExpires = null;

                // Da um update no Usuário, salvando as propriedades do Token!
                await _unitOfWork.UsuarioRepository.Update(usuario);

                return Ok(usuario);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost("verify")]
        public async Task<ActionResult> Verify(string token)
        {

            try
            {
                Usuario usuario = await _unitOfWork.UsuarioRepository.GetByVerificationEmailToken(token);

                if (usuario == null)
                {
                    return BadRequest("Token Inválido!");
                }

                usuario.VerifiedAt = DateTime.Now;
                usuario.VerificationEmailToken = null;

                // Da um update no Usuário, salvando as propriedades do Token!
                await _unitOfWork.UsuarioRepository.Update(usuario);

                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private string CreateEmailAndResetToken()
        {
            return new string(Guid.NewGuid().ToString("N").Where(char.IsLetterOrDigit).ToArray());
        }

        private UsuarioToken CreateAuthToken(Usuario usuario)
        {

            try 
            {

                var claims = new[] {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim("email", usuario.Email.ToString()),
                    new Claim("name", usuario.Name.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:Expiration"]));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                );

                return new UsuarioToken
                {
                    Authenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration,
                    Message = "Token Generated"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }

}