using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IUsuarioRepository : IRepository<Usuario>
    {

        Task<Usuario> GetContasByIdUsuarioAsync(int idUsuario);
        Task<Usuario> GetByEmail(string email);
        Task<Usuario> GetByResetPasswordToken(string token);
        Task<Usuario> GetByVerificationEmailToken(string token);
        Task<double> GetAllDespesasByIdUsuarioAsync(int idUsuario, DateTime dtIni, DateTime dtFim);
        Task<double> GetAllReceitasByIdUsuarioAsync(int idUsuario, DateTime dtIni, DateTime dtFim);
        Task<double> GetAllDespesasByIdUsuarioAndIdContaAsync(int idUsuario, int idConta, DateTime dtIni, DateTime dtFim);
        Task<double> GetAllReceitasByIdUsuarioAndIdContaAsync(int idUsuario, int idConta, DateTime dtIni, DateTime dtFim);
        Task<double> GetSaldoByIdUsuarioAsync(int idUsuario);
        Task<double> GetSaldoByIdUsuarioAndIdContaAsync(int idUsuario, int idConta);


    }

}